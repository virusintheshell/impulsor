
//AUTOR: DIEGO OLVERA
//FECHA: 20-04-2019
//DESCRIPCIÓN: CLASE QUE CONTROLA LOS METODOS PARA GUARDAR, EDITAR, CONSULTAR, LISTADO DE LOS PLANES EDUCATIVOS

namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using Framework;
    using System.Net;
    using System.Data;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;
    
    public class PlanEducativoDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        static SqlTransaction objMyTransaction = null;

        #region "METODOS PARA EL CRUD DE PLAN EDUCATIVO"

        public object Listado(Guid token, int estatus, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_LISTADO_PLANES_EDUCATIVOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@ESTATUS", estatus));
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<ListadoPlanEducativo> objPlan = new List<ListadoPlanEducativo>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objPlan.Add(new ListadoPlanEducativo
                        {
                            idPlanEducativo = objReader["idPlanUnique"].ToString(),
                            nombre = objReader["nombre"].ToString(),
                            nombrePrograma = objReader["nombrePrograma"].ToString(),
                            generarNiveles = objReader["generarNiveles"].ToString(),
                            fechaRegistro = objReader["fecha"].ToString()
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objPlan;
            }
            catch (Exception exception) { throw exception; }
            finally { connection.Close();  connection.Dispose(); }

            return miObjetoRespuesta;
        }
        
        public object Guardar(PlanEducativo propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();

            object miObjetoRespuesta = new object();
            string cadenaIDs = string.Empty;
            int respuestaObtenerCadena = 0;

            try
            {
                objMyTransaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_PLAN_EDUCATIVO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Transaction = objMyTransaction;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = propiedades.nombre;
                ObjCommand.Parameters.Add("@ID_PROGRAMA_EDUCATIVO", SqlDbType.VarChar).Value = propiedades.idProgramaEducativo;
                ObjCommand.Parameters.Add("@GENERAR_NIVELES", SqlDbType.Int).Value = propiedades.generarNiveles;
                ObjCommand.Parameters.Add("@NIVELES", SqlDbType.Int).Value = propiedades.niveles;

                cadenaIDs = ObtenerCadenaIDs(propiedades.listadoDocumentos, ref respuestaObtenerCadena);

                ObjCommand.Parameters.Add("@DOCUMENTOS", SqlDbType.VarChar).Value = cadenaIDs;

                ObjCommand.Parameters.Add("@ID_PLAN_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.ExecuteNonQuery();

                int idPlan = Convert.ToInt16(ObjCommand.Parameters["@ID_PLAN_OUT"].Value);
                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);


                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    GuardarDetalle(connection, propiedades, idPlan, ref respuesta);
                    if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                        objMyTransaction.Commit();
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
                EscrituraLog.guardar("PlanEducativoDataAccess-Guardar. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Editar(PlanEducativo propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();

            object miObjetoRespuesta = new object();
            string cadenaIDs = string.Empty;
            int respuestaObtenerCadena = 0;

            try
            {
                objMyTransaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                SqlCommand ObjCommand = new SqlCommand("USP_EDITAR_PLAN_EDUCATIVO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Transaction = objMyTransaction;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = propiedades.nombre;
                ObjCommand.Parameters.Add("@ID_PLAN_EDUCATIVO", SqlDbType.VarChar).Value = propiedades.idPlanEducativo;
                ObjCommand.Parameters.Add("@ID_PROGRAMA_EDUCATIVO", SqlDbType.VarChar).Value = propiedades.idProgramaEducativo;
                ObjCommand.Parameters.Add("@GENERAR_NIVELES", SqlDbType.Int).Value = propiedades.generarNiveles;
                ObjCommand.Parameters.Add("@NIVELES", SqlDbType.Int).Value = propiedades.niveles;

                cadenaIDs = ObtenerCadenaIDs(propiedades.listadoDocumentos, ref respuestaObtenerCadena);

                ObjCommand.Parameters.Add("@DOCUMENTOS", SqlDbType.VarChar).Value = cadenaIDs;

                ObjCommand.Parameters.Add("@ID_PLAN", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.ExecuteNonQuery();

                int idPlan = Convert.ToInt16(ObjCommand.Parameters["@ID_PLAN"].Value);
                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    GuardarDetalle(connection, propiedades, idPlan, ref respuesta);
                    if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                        objMyTransaction.Commit();
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
                EscrituraLog.guardar("PlanEducativoDataAccess-Editar. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private void GuardarDetalle(SqlConnection connection, PlanEducativo propiedades, int idPlan, ref int respuesta)
        {
            respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            try
            {
                foreach (DetalleProgramaEducativo item in propiedades.listadoAsignaturas)
                {
                    string cadenaIDs = string.Empty;
                    int respuestaObtenerCadena = 0;
                    cadenaIDs = ObtenerCadenaIDs(item.idAsignatura, ref respuestaObtenerCadena);

                    if (respuestaObtenerCadena == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_DETALLE_PROGRAMA_EDUCATIVO", connection);
                        ObjCommand.CommandType = CommandType.StoredProcedure;
                        ObjCommand.Transaction = objMyTransaction;

                        ObjCommand.Parameters.Add("@ID_PLAN", SqlDbType.Int).Value = idPlan;
                        ObjCommand.Parameters.Add("@NIVEL", SqlDbType.Int).Value = item.idNivel;
                        ObjCommand.Parameters.Add("@CADENA_ASIGNATURAS", SqlDbType.VarChar).Value = cadenaIDs;

                        int filasAfectadas = ObjCommand.ExecuteNonQuery();
                        if (filasAfectadas == 0) { objMyTransaction.Rollback(); respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError); break; }
                        ObjCommand.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("PlanEducativoDataAccess-GuardarDetalle. ", e.Message.ToString());
            }
        }

        private static string ObtenerCadenaIDs(ArrayList propiedades, ref int respuesta)
        {
            string resultado = string.Empty;
            respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            try
            {
                if (propiedades.Count > 0)
                {
                    for (int i = 0; i < propiedades.Count; i++)
                    {
                        if (Convert.ToString(propiedades[i]) != "") { resultado = propiedades[i].ToString().ToUpper() + ',' + resultado; }
                        else { respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError); break; }
                    }
                }
                else { respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.ErrorData); }
            }
            catch (Exception e) {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("PlanEducativoDataAccess-ObtenerCadenaIDs. ", e.Message.ToString()); }

            resultado = resultado.TrimEnd(',');
            return resultado;
        }

        #endregion

        #region "METODOS PARA OBTENER EL PLAN EDUCATIVO"

        public object Obtener(Guid token, string pagina, string idPlan, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_PLAN_EDUCATIVO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;
                ObjCommand.Parameters.Add("@ID_PLAN", SqlDbType.VarChar).Value = idPlan;
                connection.Open();

                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(ObjCommand);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                int respuesta = 0;
                object objetoFinal = new object();
                objetoFinal = ArmarObjetoGeneral(ds, ref respuesta);

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objetoFinal;
            }
            catch (Exception e) {
                EscrituraLog.guardar("PlanEducativoDataAccess-Obtener. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private PlanObtener ArmarObjetoGeneral(DataSet ds, ref int respuesta)
        {
            PlanObtener objPlan = new PlanObtener();
            try
            {
                DataTable datosGenerales = ds.Tables[0];
                DataTable asignaturas = ds.Tables[1];
                DataTable documentos = ds.Tables[2];

                if (datosGenerales.Rows.Count > 0)
                {
                    objPlan.nombre = datosGenerales.Rows[0].ItemArray[6].ToString();
                    objPlan.idProgramaEducativo = datosGenerales.Rows[0].ItemArray[3].ToString();
                    objPlan.idPlanEducativo = datosGenerales.Rows[0].ItemArray[0].ToString();
                    objPlan.gradoEducativo = Convert.ToInt16(datosGenerales.Rows[0].ItemArray[1]);
                    objPlan.generarNiveles = Convert.ToInt16(datosGenerales.Rows[0].ItemArray[4]);
                    objPlan.niveles = Convert.ToInt16(datosGenerales.Rows[0].ItemArray[5]);
                    objPlan.listadoAsignaturas = ObtenerAsignaturas(asignaturas);
                    objPlan.listadoDocumentos = ObtenerListaDocumentos(documentos);
                    respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                }
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("PlanEducativoDataAccess-ArmarObjetoGeneral. ", e.Message.ToString());
            }
            return objPlan;
        }

        private List<ProgramaAsignaturasObtener> ObtenerAsignaturas(DataTable asignaturas)
        {
            List<ProgramaAsignaturasObtener> objLista = new List<ProgramaAsignaturasObtener>();
            try
            {
                string[] columna = { "nivel", };
                DataTable dataTable = General.ObtenerRegistrosDiferentes(asignaturas, columna);

                foreach (DataRow item in dataTable.Rows)
                {
                    objLista.Add(new ProgramaAsignaturasObtener
                    {
                        idNivel = Convert.ToInt16(item["nivel"]),
                        Asignaturas = ObtenerPrograma(asignaturas, Convert.ToInt16(item["nivel"]))
                    });
                }
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("PlanEducativoDataAccess-ObtenerAsignaturas. ", e.Message.ToString());
            }
            return objLista;
        }

        private List<DetalleProgramaObtener> ObtenerPrograma(DataTable asignaturas, int nivel)
        {
            List<DetalleProgramaObtener> objLista = new List<DetalleProgramaObtener>();
            try
            {
                DataRow[] resultado = asignaturas.Select("nivel =" + nivel);

                foreach (DataRow registro in resultado)
                {
                    objLista.Add(new DetalleProgramaObtener
                    {
                        idAsignatura = registro["idAsignatura"].ToString(),
                        nombre = registro["nombre"].ToString(),
                    });
                }
            }
            catch (Exception e) {
                EscrituraLog.guardar("PlanEducativoDataAccess-ObtenerPrograma. ", e.Message.ToString());
            }
            return objLista;
        }

        private ArrayList ObtenerListaDocumentos(DataTable documentos)
        {
            ArrayList arrDocumentos = new ArrayList();
            try
            {
                foreach (DataRow item in documentos.Rows)
                {
                    arrDocumentos.Add(item["idDocumento"]);
                }
            }
            catch (Exception e) {
                EscrituraLog.guardar("PlanEducativoDataAccess-ObtenerListaDocumentos. ", e.Message.ToString());
            }
            return arrDocumentos;
        }

        #endregion

    }
}
