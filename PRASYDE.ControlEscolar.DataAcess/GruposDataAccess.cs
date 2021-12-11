
//AUTOR: DIEGO OLVERA
//FECHA: 23-05-2019
//DESCRIPCIÓN: CLASE QUE CONTROLA LOS METODOS PARA GUARDAR, EDITAR, CONSULTAR, LISTADO DE LOS GRUPOS

namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using Framework;
    using System.Net;
    using System.Data;
    using Newtonsoft.Json;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;

    public class GruposDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        static SqlTransaction objMyTransaction = null;

        #region "CLASES PARA EL CRUD"

        public object Listado(Guid token, int estatus, int tipo, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_LISTADO_GRUPOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@TIPO", SqlDbType.Int).Value = tipo;
                ObjCommand.Parameters.Add("@ESTATUS", SqlDbType.Int).Value = estatus;

                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<ListadoGrupos> objGrupos = new List<ListadoGrupos>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objGrupos.Add(new ListadoGrupos
                        {
                            idGrupo = objReader["idGrupo"].ToString(),
                            clave = objReader["clave"].ToString(),
                            idOfertaEducativa = Convert.ToInt16(objReader["idOfertaEducativa"]),
                            planEducativo = objReader["nombrePlan"].ToString(),
                            programaEducativo = objReader["programaEducativo"].ToString(),
                            modalidad = objReader["modalidad"].ToString(),
                            plantel = objReader["plantel"].ToString(),
                            cupo = Convert.ToInt16(objReader["cupo"]),
                            inscritos = Convert.ToInt16(objReader["inscritos"]),
                            horarioGrupo = Convert.ToInt16(objReader["horarioGrupo"]),
                            estatusGrupo = objReader["estatusGrupo"].ToString(),
                            nivelActual = Convert.ToInt16(objReader["nivelActual"]),
                            generarNiveles = Convert.ToInt16(objReader["generarNiveles"]),
                            estatus = Convert.ToInt16(objReader["estatus"])
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt32(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objGrupos;
            }
            catch (Exception e) {
                codigoRespuesta = Convert.ToInt32(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("GruposDataAccess-Listado. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Guardar(Grupos propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();

            object miObjetoRespuesta = new object();
            try
            {
                objMyTransaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_GRUPOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Transaction = objMyTransaction;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = propiedades.nombreGrupo;
                ObjCommand.Parameters.Add("@ID_DETALLE_PLANTEL", SqlDbType.VarChar).Value = propiedades.idOfertaEducativa;
                ObjCommand.Parameters.Add("@ID_PLAN_EDUCATIVO", SqlDbType.VarChar).Value = propiedades.idPlanEducativo;
                ObjCommand.Parameters.Add("@CUPO", SqlDbType.Int).Value = propiedades.cupo;
                ObjCommand.Parameters.Add("@NIVEL", SqlDbType.Int).Value = propiedades.nivel;
                ObjCommand.Parameters.Add("@ID_GRUPO_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;

                ObjCommand.ExecuteNonQuery();

                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);
                int idGrupo = Convert.ToInt32(ObjCommand.Parameters["@ID_GRUPO_OUT"].Value);

                //GUARDAMOS EL DETALLE 
                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    GuardarDetalle(connection, propiedades, idGrupo, propiedades.nivel, ref respuesta);
                    if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK)) { objMyTransaction.Commit(); }
                }

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = new object();
            }
            catch (Exception e) {
                objMyTransaction.Rollback();
                EscrituraLog.guardar("GruposDataAccess-Guardar. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object GuardarHorarioPorNivel(HorariosPorNivel propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();

            objMyTransaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
            object miObjetoRespuesta = new object();
            int respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);

            try
            {
                foreach (Horarios item in propiedades.horario)
                {
                    foreach (AsignaturasHorarios itemA in item.asignaturas)
                    {
                        SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_DETALLE_GRUPOS", connection);
                        ObjCommand.CommandType = CommandType.StoredProcedure;
                        ObjCommand.Transaction = objMyTransaction;

                        ObjCommand.Parameters.Add("@ID_GRUPO", SqlDbType.Int).Value = 0;
                        ObjCommand.Parameters.Add("@ID_DETALLE_PLANTEL", SqlDbType.VarChar).Value = "";
                        ObjCommand.Parameters.Add("@ID_GRUPO_UNIQUE", SqlDbType.VarChar).Value = propiedades.idGrupo;
                        ObjCommand.Parameters.Add("@DIA", SqlDbType.Int).Value = item.dia;
                        ObjCommand.Parameters.Add("@ID_ASIGNATURA", SqlDbType.VarChar).Value = itemA.idAsignatura;
                        ObjCommand.Parameters.Add("@HORA_INICIAL", SqlDbType.VarChar).Value = itemA.inicio;
                        ObjCommand.Parameters.Add("@HORA_FINAL", SqlDbType.VarChar).Value = itemA.fin;
                        ObjCommand.Parameters.Add("@NIVEL", SqlDbType.Int).Value = 0;

                        int filasAfectadas = ObjCommand.ExecuteNonQuery();
                        if (filasAfectadas == 0) { objMyTransaction.Rollback(); respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError); break; }
                        ObjCommand.Dispose();
                    }
                }

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK)) { objMyTransaction.Commit(); }

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = new object();
            }
            catch (Exception e) {
                objMyTransaction.Rollback();
                EscrituraLog.guardar("GruposDataAccess-GuardarHorarioPorNivel. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private void GuardarDetalle(SqlConnection connection, Grupos propiedades, int idGrupo, int nivel, ref int respuesta)
        {
            respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            try
            {
                foreach (Horarios item in propiedades.horario)
                {
                    foreach (AsignaturasHorarios itemA in item.asignaturas)
                    {
                        SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_DETALLE_GRUPOS", connection);
                        ObjCommand.CommandType = CommandType.StoredProcedure;
                        ObjCommand.Transaction = objMyTransaction;

                        ObjCommand.Parameters.Add("@ID_GRUPO", SqlDbType.Int).Value = idGrupo;
                        ObjCommand.Parameters.Add("@ID_DETALLE_PLANTEL", SqlDbType.VarChar).Value = propiedades.idOfertaEducativa;
                        ObjCommand.Parameters.Add("@ID_GRUPO_UNIQUE", SqlDbType.VarChar).Value = "";
                        ObjCommand.Parameters.Add("@DIA", SqlDbType.Int).Value = item.dia;
                        ObjCommand.Parameters.Add("@ID_ASIGNATURA", SqlDbType.VarChar).Value = itemA.idAsignatura;
                        ObjCommand.Parameters.Add("@HORA_INICIAL", SqlDbType.VarChar).Value = itemA.inicio;
                        ObjCommand.Parameters.Add("@HORA_FINAL", SqlDbType.VarChar).Value = itemA.fin;
                        ObjCommand.Parameters.Add("@NIVEL", SqlDbType.Int).Value = nivel;

                        int filasAfectadas = ObjCommand.ExecuteNonQuery();
                        if (filasAfectadas == 0) { objMyTransaction.Rollback(); respuesta = 500; break; }
                        ObjCommand.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("GruposDataAccess-GuardarDetalle. ", e.Message.ToString());
            }
        }

        public object CerrarGrupo(Guid token, string grupo, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();

            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_CERRAR_GRUPOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@ID_GRUPO", SqlDbType.VarChar).Value = grupo.ToUpper();
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.ExecuteNonQuery();

                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = new object();
            }
            catch (Exception e) {
                EscrituraLog.guardar("GruposDataAccess-CerrarGrupo. ", e.Message.ToString());
            }
            finally { connection.Close();  connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Editar(GruposEdicion propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();

            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_EDITAR_GRUPO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@ID_GRUPO_UNIQUE", SqlDbType.VarChar).Value = propiedades.idGrupo;
                ObjCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = propiedades.nombreGrupo;
                ObjCommand.Parameters.Add("@CUPO", SqlDbType.Int).Value = propiedades.cupo;

                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.ExecuteNonQuery();

                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = new object();
            }
            catch (Exception e) {
                EscrituraLog.guardar("GruposDataAccess-Editar. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        #endregion

        #region "CLASES PARA ARMAR EL FORMULARIO DE GRUPOS"

        public object ArmarFormularioGrupos(Guid token, string idPlantel, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_DATOS_GRUPOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@ID_PLANTEL", idPlantel));
                connection.Open();

                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(ObjCommand);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                int respuesta = 0;
                object objetoFinal = new object();
                objetoFinal = ArmarObjetoGeneralGrupos(ds, ref respuesta);

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objetoFinal;
            }
            catch (Exception e) {
                EscrituraLog.guardar("GruposDataAccess-ArmarFormularioGrupos. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private List<GruposGrados> ArmarObjetoGeneralGrupos(DataSet ds, ref int respuesta)
        {
            List<GruposGrados> objGrados = new List<GruposGrados>();
            try
            {
                DataTable datosGenerales = ds.Tables[0];
              
                string[] columna = { "idGradoEducativo", "gradoEducativo" };
                DataTable dataTable = RegistrosDiferentes.Obtener(datosGenerales, columna);

                foreach (DataRow item in dataTable.Rows)
                {
                    objGrados.Add(new GruposGrados
                    {
                        idGrado = Convert.ToInt16(item["idGradoEducativo"]),
                        nombre = Convert.ToString(item["gradoEducativo"]),
                        ProgramasEducativos = ObtnerProgramasEducativos(ds, Convert.ToInt16(item["idGradoEducativo"]), ref respuesta)
                    });
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("GruposDataAccess-ArmarObjetoGeneralGrupos. ", e.Message.ToString());
            }
            return objGrados;
        }

        private List<GruposPrograma> ObtnerProgramasEducativos(DataSet ds, int idGradoEducativo, ref int respuesta)
        {
            List<GruposPrograma> objProgramas = new List<GruposPrograma>();
            try
            {
                DataTable datosGenerales = ds.Tables[0];
                DataTable datosPlanes = ds.Tables[1];
                DataTable datosAsignaturas = ds.Tables[2];
                
                string[] columna = { "idProgramaEducativo", "nombrePrograma", "descripcion", "idGradoEducativo" };
                DataTable dataTable = RegistrosDiferentes.Obtener(datosGenerales, columna);
                DataRow[] resultado = dataTable.Select("idGradoEducativo ='" + idGradoEducativo + "'");

                foreach (DataRow item in resultado)
                {
                    objProgramas.Add(new GruposPrograma
                    {
                        idProgramaEducativo = Convert.ToInt16(item["idProgramaEducativo"]),
                        nombre = Convert.ToString(item["nombrePrograma"]),
                        descripcion = Convert.ToString(item["descripcion"]),
                        modalidades = ObtenerModalidades(datosGenerales, Convert.ToInt16(item["idProgramaEducativo"]), ref respuesta),
                        planesEducativos = ObtenerPlanes(ds,Convert.ToInt16(item["idProgramaEducativo"]), ref respuesta)
                    });
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK); 
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("GruposDataAccess-ObtnerProgramasEducativos. ", e.Message.ToString());
            }
            return objProgramas;
        }

        //-------------------------------------//
        private List<GruposModalidades> ObtenerModalidades(DataTable datosGenerales, int idPrograma, ref int respuesta)
        {
            List<GruposModalidades> objModalidades = new List<GruposModalidades>();
            try
            {
                DataRow[] resultado = datosGenerales.Select("idProgramaEducativo ='" + idPrograma + "'");

                foreach (DataRow item in resultado)
                {
                    objModalidades.Add(new GruposModalidades
                    {
                        idModalidad = Convert.ToInt16(item["idModalidad"]),
                        nombreModalidad = Convert.ToString(item["modaldidad"]),
                        Horario = ObtenerHorarios(datosGenerales, idPrograma, Convert.ToInt16(item["idModalidad"]), ref respuesta)
                    });
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("GruposDataAccess-ObtenerModalidades. ", e.Message.ToString());
            }
            return objModalidades;
        }

        private List<GruposHorarios> ObtenerHorarios(DataTable datosGenerales, int idPrograma, int idModalidad, ref int respuesta)
        {
            List<GruposHorarios> objHorarios = new List<GruposHorarios>();
            try
            {
                DataRow[] resultado = datosGenerales.Select("idModalidad ='" + idModalidad + "' AND idProgramaEducativo='" + idPrograma + "'");

                foreach (DataRow item in resultado)
                {
                    objHorarios.Add(new GruposHorarios
                    {
                        idOfertaEducativa = Convert.ToString(item["idDetallePlantelOferta"]),
                        idHorario = Convert.ToInt16(item["idHorario"]),
                        Horario = Convert.ToString(item["Horarios"])
                    });
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("GruposDataAccess-ObtenerHorarios. ", e.Message.ToString());
            }
            return objHorarios;
        }

        //-------------------------------------//
    
        private List<PlanesEducativosLista> ObtenerPlanes(DataSet ds, int idPrograma, ref int respuesta)
        {
            List<PlanesEducativosLista> objPlanes = new List<PlanesEducativosLista>();
            try
            {
             
                DataTable datosPlanes = ds.Tables[1];
                DataTable datosAsignaturas = ds.Tables[2];
                DataRow[] resultado = datosPlanes.Select("idProgramaEducativo ='" + idPrograma + "'");

                foreach (DataRow item in resultado)
                {
                    objPlanes.Add(new PlanesEducativosLista
                    {
                        idPlanEducativo = Convert.ToString(item["idPlanEducativo"]),
                        nombrePlan = Convert.ToString(item["nombrePlan"]),
                        nivelesGenerados = Convert.ToInt16(item["generarNiveles"]),
                        niveles = ObtenerNiveles(ds, Convert.ToInt16(item["idProgramaEducativo"]), Convert.ToString(item["idPlanEducativo"]), Convert.ToInt16(item["nivel"]), Convert.ToInt16(item["generarNiveles"]), ref respuesta),
                        asignaturasSinNivel = ObtenerAsignaturas(datosAsignaturas, Convert.ToInt16(item["idProgramaEducativo"]), Convert.ToString(item["idPlanEducativo"]), 0, ref respuesta)
                    });
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("GruposDataAccess-ObtenerPlanes. ", e.Message.ToString());
            }
            return objPlanes;
        }

        private List<GruposNiveles> ObtenerNiveles(DataSet ds, int idProgramaEducativo, string idPlan, int totalNiveles, int generarNiveles, ref int respuesta)
        {
            List<GruposNiveles> objASignaturas = new List<GruposNiveles>();
            try
            {
                if (generarNiveles == 1)
                {
                    DataTable datosAsignaturas = ds.Tables[2];

                    DataRow[] resultado = datosAsignaturas.Select("idProgramaEducativo='" + idProgramaEducativo + "' AND idPlanEducativo='"+ idPlan + "'");

                    DataTable nuevaTabla = new DataTable();
                    nuevaTabla.Columns.Add("idProgramaEducativo");
                    nuevaTabla.Columns.Add("nivel");
                    nuevaTabla.Columns.Add("idAsignatura");
                    nuevaTabla.Columns.Add("clave");
                    nuevaTabla.Columns.Add("nombre");

                    for (int i = 0; i < resultado.Length; i++)
                    {
                        DataRow fila = nuevaTabla.NewRow();

                        fila["idProgramaEducativo"] = resultado[i][0];
                        fila["nivel"] = resultado[i][1];
                        fila["idAsignatura"] = resultado[i][2];
                        fila["clave"] = resultado[i][3];
                        fila["nombre"] = resultado[i][4];
                        nuevaTabla.Rows.Add(fila);

                    }

                    string[] columna = { "nivel" };
                    DataTable dataTable = RegistrosDiferentes.Obtener(nuevaTabla, columna);

                    foreach (DataRow item in dataTable.Rows)
                    {
                        objASignaturas.Add(new GruposNiveles
                        {
                            nivel = Convert.ToInt16(item["nivel"]),
                            Asignaturas = ObtenerAsignaturas(datosAsignaturas, idProgramaEducativo, idPlan, Convert.ToInt16(item["nivel"]), ref respuesta)
                        });
                    }

                }
                else
                {
                    //SE ARMA EL OBJETO VACIO
                    List<GruposAsignaturas> objAsignaturas = new List<GruposAsignaturas>();
                    for (int i = 1; i <= totalNiveles; i++)
                    {
                        objASignaturas.Add(new GruposNiveles
                        {
                            nivel = i,
                            Asignaturas = objAsignaturas
                        });
                    }
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("GruposDataAccess-ObtenerNiveles. ", e.Message.ToString());
            }
            return objASignaturas;
        }
        
        private List<GruposAsignaturas> ObtenerAsignaturas(DataTable datosAsignaturas, int idProgramaEducativo, string idPlan, int idNivel, ref int respuesta)
        {
            List<GruposAsignaturas> objAsignaturas = new List<GruposAsignaturas>();
            try
            {
                DataRow[] resultado = datosAsignaturas.Select("nivel ='" + idNivel + "' AND idProgramaEducativo= '" + idProgramaEducativo + "' AND idPlanEducativo='" + idPlan + "'");

                foreach (DataRow item in resultado)
                {
                    objAsignaturas.Add(new GruposAsignaturas
                    {
                        idAsignatura = Convert.ToString(item["idAsignatura"]),
                        clave = Convert.ToString(item["clave"]),
                        nombreAsignatura = Convert.ToString(item["nombre"])
                    });
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("GruposDataAccess-ObtenerAsignaturas. ", e.Message.ToString());
            }
            return objAsignaturas;
        }

        #endregion

        #region "METODOS PARA ARMAR EL FORMULARIO PARA LA ASINGACION DE GRUPOS A PROFESORES"

        public object ObtenerListaAsignaturasProfesor(Guid token, string idGrupo, int nivel, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_LISTADO_ASIGNATURAS_PROFESOR", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@ID_GRUPO_UNIQUE", idGrupo));
                ObjCommand.Parameters.Add(new SqlParameter("@NIVEL", nivel));
                connection.Open();

                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(ObjCommand);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                int respuesta = 0;
                object objetoFinal = new object();
                objetoFinal = ArmarObjetoGeneralAsingacion(ds, ref respuesta);

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objetoFinal;
            }
            catch (Exception e) {
                EscrituraLog.guardar("GruposDataAccess-ObtenerListaAsignaturasProfesor. ", e.Message.ToString());
            }
            finally { connection.Dispose(); connection.Close(); }

            return miObjetoRespuesta;
        }

        private GrupoAsingaturas ArmarObjetoGeneralAsingacion(DataSet ds, ref int respuesta)
        {
            GrupoAsingaturas objeto = new GrupoAsingaturas();
            try
            {
                DataTable datosGenerales = ds.Tables[0];
                DataTable datosAsignaturas = ds.Tables[1];
                DataTable datosAsignacion = ds.Tables[2];

                string claveGrupo = string.Empty;
                int cupo = 0;

                if (datosGenerales.Rows.Count > 0)
                {
                    claveGrupo = datosGenerales.Rows[0].ItemArray[1].ToString();
                    cupo = Convert.ToInt32(datosGenerales.Rows[0].ItemArray[2]);
                }

                objeto.claveGrupo = claveGrupo;
                objeto.cupo = cupo;
                objeto.asignaturas = ObtnerListaAsignaturas(datosGenerales, datosAsignaturas, datosAsignacion, ref respuesta);

                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("GruposDataAccess-ArmarObjetoGeneralAsingacion. ", e.Message.ToString());
            }
            return objeto;
        }

        private List<AsignaturaProfesor> ObtnerListaAsignaturas(DataTable datosGenerales, DataTable datosAsignaturas, DataTable datosAsignacion, ref int respuesta)
        {
            List<AsignaturaProfesor> objAsignaturaProfesor = new List<AsignaturaProfesor>();
            try
            {
                string[] columna = { "idAsignatura", "idAsignaturaUnique", "clave", "nombreAsignatura" };
                DataTable dataTable = RegistrosDiferentes.Obtener(datosGenerales, columna);

                foreach (DataRow item in dataTable.Rows)
                {
                    objAsignaturaProfesor.Add(new AsignaturaProfesor
                    {
                        idAsignatura = Convert.ToString(item["idAsignaturaUnique"]),
                        clave = Convert.ToString(item["clave"]),
                        nombre = Convert.ToString(item["nombreAsignatura"]),
                        profesores = ObtenerListadoProfesor(datosGenerales, datosAsignaturas, datosAsignacion, Convert.ToInt32(item["idAsignatura"]), ref respuesta)
                    });
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("GruposDataAccess-ObtnerListaAsignaturas. ", e.Message.ToString());
            }
            return objAsignaturaProfesor;
        }

        private List<DetalleAsignaturaProfesor> ObtenerListadoProfesor(DataTable datosGenerales, DataTable datosAsignaturas, DataTable datosAsignacion, int idAsignatura, ref int respuesta)
        {
            List<DetalleAsignaturaProfesor> objListaProfesor = new List<DetalleAsignaturaProfesor>();
            try
            {
                DataRow[] resultado = datosGenerales.Select("idAsignatura ='" + idAsignatura + "'");

                foreach (DataRow item in resultado)
                {
                    DataRow[] resultadoAsignaturas = datosAsignaturas.Select("idAsignatura ='" + idAsignatura + "'");

                    foreach (DataRow itemA in resultadoAsignaturas)
                    {
                        var iAsignado = 0;
                        var idProfesorA = Convert.ToInt32(itemA["idProfesor"]);
                        var idAsignaturaUniqueA = itemA["idAsignaturaUnique"].ToString().Trim();

                        foreach (DataRow itemB in datosAsignacion.Rows)
                        {
                            var idProfesorB = Convert.ToInt32(itemB["idProfesor"]);
                            var idAsignaturaUniqueB = itemB["idAsignatura"].ToString().Trim();

                            if (idAsignaturaUniqueB.ToUpper() == idAsignaturaUniqueA.ToUpper() && idProfesorA == idProfesorB)
                            {
                                iAsignado = 1;
                                break;
                            }
                        }

                        objListaProfesor.Add(new DetalleAsignaturaProfesor
                        {
                            idProfesor = Convert.ToInt32(itemA["idProfesor"]),
                            matricula = Convert.ToString(itemA["matricula"]),
                            nombre = Convert.ToString(itemA["nombre"]),
                            asignado = iAsignado
                        });

                    }
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("GruposDataAccess-ObtenerListadoProfesor. ", e.Message.ToString());
            }
            return objListaProfesor;
        }

        #endregion

        #region "METODOS PARA GUARDAR LA ASIGNACION DE GRUPOS A PROFESORES"

        public object GuardarAsignacionGrupoProfesor(PropiedadesGruposProfesor propiedades, Guid token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();

            object miObjetoRespuesta = new object();
            int respuesta = 0;
            int respuestaIteracion = 0;
            try
            {
                objMyTransaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                foreach (PropiedadesGruposProfesorDetalle item in propiedades.asignaturas)
                {
                    SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_GRUPOS_PROFESOR", connection);
                    ObjCommand.CommandType = CommandType.StoredProcedure;
                    ObjCommand.Transaction = objMyTransaction;

                    ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                    ObjCommand.Parameters.Add("@ID_PROFESOR", SqlDbType.Int).Value = item.idProfesor;
                    ObjCommand.Parameters.Add("@ID_GRUPO", SqlDbType.VarChar).Value = propiedades.idgrupo;
                    ObjCommand.Parameters.Add("@ID_ASIGNATURA", SqlDbType.VarChar).Value = item.idAsignatura;
                    ObjCommand.Parameters.Add("@ITERACION", SqlDbType.Int).Value = respuestaIteracion;

                    ObjCommand.Parameters.Add("@RESPUESTA_ITERACION", SqlDbType.Int).Direction = ParameterDirection.Output;
                    ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                    ObjCommand.ExecuteNonQuery();

                    respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);
                    respuestaIteracion = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA_ITERACION"].Value);
                }

                if (respuesta == 200) { objMyTransaction.Commit(); }

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = new object();
            }

            catch (Exception e) {
                objMyTransaction.Rollback();
                EscrituraLog.guardar("GruposDataAccess-GuardarAsignacionGrupoProfesor. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        #endregion

        #region "METODOS QUE REGRESA EL OBJETO PARA MOSTRAR EL RESUMEN DEL HORARIO"

        public object ListadoHorario(Guid token, string idGrupo, int nivel, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_HORARIO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@NIVEL", SqlDbType.Int).Value = nivel;
                ObjCommand.Parameters.Add("@ID_GRUPOUNIQUE", SqlDbType.VarChar).Value = idGrupo.ToString().ToUpper();
                connection.Open();

                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(ObjCommand);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                int respuesta = 0;
                object objetoFinal = new object();
                objetoFinal = ArmarObjetoGeneraHorarios(ds, ref respuesta);

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objetoFinal;
            }
            catch (Exception e) {
                EscrituraLog.guardar("GruposDataAccess-ListadoHorario. ", e.Message.ToString());
            }
            finally { connection.Dispose(); connection.Close(); }

            return miObjetoRespuesta;
        }

        private List<PropiedadesHorarioAsignaturas> ArmarObjetoGeneraHorarios(DataSet ds, ref int respuesta)
        {
            List<PropiedadesHorarioAsignaturas> objHorarios = new List<PropiedadesHorarioAsignaturas>();
            try
            {
                DataTable datosGenerales = ds.Tables[0];

                string[] columna = { "idAsignatura", "clave", "nombreAsignatura", "idProfesor", "nombreProfesor" };
                DataTable dataTable = RegistrosDiferentes.Obtener(datosGenerales, columna);

                foreach (DataRow item in dataTable.Rows)
                {
                    if (Convert.ToInt16(item["idAsignatura"]) != 0)
                    {
                        objHorarios.Add(new PropiedadesHorarioAsignaturas
                        {
                            id = Convert.ToInt16(item["idAsignatura"]),
                            clave = Convert.ToString(item["clave"]),
                            nombre = Convert.ToString(item["nombreAsignatura"]),
                            profesor = Convert.ToString(item["nombreProfesor"]),
                            dias = ObtenerDetalleHorario(ds, Convert.ToInt16(item["idAsignatura"]), Convert.ToInt16(item["idProfesor"]), ref respuesta)
                        });
                    }
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("GruposDataAccess-ArmarObjetoGeneraHorarios. ", e.Message.ToString());
            }
            return objHorarios;
        }

        private List<PropiedadesHorarioDetalle> ObtenerDetalleHorario(DataSet ds, int idAsignatura, int idProfesor, ref int respuesta)
        {
            List<PropiedadesHorarioDetalle> objHorarios = new List<PropiedadesHorarioDetalle>();
            try
            {
                DataTable datosGenerales = ds.Tables[0];
                DataTable datosHorarios = ds.Tables[1];

                foreach (DataRow item in datosHorarios.Rows)
                {
                    int dia = Convert.ToInt16(item["dia"]);
                    DataRow[] resultado = datosGenerales.Select("idAsignatura ='" + idAsignatura + "' AND idProfesor =" + idProfesor + " AND dia='" + dia + "'");

                    if (resultado.Length == 0)
                    {
                        objHorarios.Add(new PropiedadesHorarioDetalle
                        {
                            nombre = RegresarDiaSemana(dia),
                            inicio = "",
                            final = "",
                        });
                    }

                    foreach (DataRow itemA in resultado)
                    {
                        if (dia == Convert.ToInt16(itemA["dia"]))
                        {
                            objHorarios.Add(new PropiedadesHorarioDetalle
                            {
                                nombre = Convert.ToString(itemA["nombre"]),
                                inicio = Convert.ToString(itemA["horaInicial"]),
                                final = Convert.ToString(itemA["horaFinal"]),
                            });
                        }
                        else
                        {
                            objHorarios.Add(new PropiedadesHorarioDetalle
                            {
                                nombre = Convert.ToString(item["nombre"]),
                                inicio = "",
                                final = "",
                            });
                        }
                    }
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("GruposDataAccess-ObtenerDetalleHorario. ", e.Message.ToString());
            }
            return objHorarios;
        }

        public string RegresarDiaSemana(int dia)
        {
            string diaSemana = string.Empty;

            switch (dia)
            {
                case 1:
                    diaSemana = "D";
                    break;
                case 2:
                    diaSemana = "L";
                    break;
                case 3:
                    diaSemana = "M";
                    break;
                case 4:
                    diaSemana = "Mi";
                    break;
                case 5:
                    diaSemana = "J";
                    break;
                case 6:
                    diaSemana = "V";
                    break;
                case 7:
                    diaSemana = "S";
                    break;
            }

            return diaSemana;

        }

        #endregion

        #region "METODOS QUE REGRESA EL OBJETO CON LAS ASIGNATURAS POR NIVEL (PARA GENERAR UN NUEVO HORARIO)"

        public object ListadoAsignaturasPorNivel(Guid token, string idGrupo, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_LISTADO_ASIGNATURAS_POR_NIVELES", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@ID_GRUPO_UNIQUE", SqlDbType.VarChar).Value = idGrupo.ToUpper();
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<PropiedadesAsignaturasPorNivel> objAsignaturas = new List<PropiedadesAsignaturasPorNivel>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objAsignaturas.Add(new PropiedadesAsignaturasPorNivel
                        {
                            idAsignatura = objReader["idAsignatura"].ToString(),
                            clave = objReader["clave"].ToString(),
                            nombreAsignatura = objReader["nombre"].ToString(),
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = 200;
                textoRespuesta = "OK";
                miObjetoRespuesta = objAsignaturas;
            }
            catch (Exception e) {
                EscrituraLog.guardar("GruposDataAccess-ListadoAsignaturasPorNivel. ", e.Message.ToString());
            }
            finally { connection.Dispose(); connection.Close(); }

            return miObjetoRespuesta;
        }

        #endregion
    }
}
