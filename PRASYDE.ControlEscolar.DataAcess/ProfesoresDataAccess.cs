
//AUTOR: DIEGO OLVERA
//FECHA: 28-05-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA EL MODULO DE PROFESORES

namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using Framework;
    using System.IO;
    using System.Net;
    using System.Web;
    using System.Data;
    using Newtonsoft.Json;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;
    using System.Collections;

    public class ProfesoresDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        static SqlTransaction objMyTransaction = null;

        public object Listado(Guid token, int estatus, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_LISTADO_PROFESORES", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@ESTATUS", estatus));

                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<ListadoProfesores> objProfesores = new List<ListadoProfesores>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objProfesores.Add(new ListadoProfesores
                        {
                            idUsuario = objReader["idUsuarioUnique"].ToString(),
                            idProfesor = objReader["idProfesorUnique"].ToString(),
                            matricula = objReader["matricula"].ToString(),
                            nombre = objReader["nombre"].ToString(),
                            correo = objReader["correo"].ToString(),
                            celular = objReader["celular"].ToString(),
                            gradoEstudios = objReader["gradoEstudios"].ToString(),
                            estatus = Convert.ToInt16(objReader["estatus"])
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objProfesores;
            }
            catch (Exception e)
            {
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("ProfesoresDataAccess-Listado. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object GuardarAsignacion(Guid token, string pagina, AsignacionProfesorAsignatura propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            int contador = 0;
            int respuesta = 200;
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                objMyTransaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                foreach (string item in propiedades.asignaturas)
                {
                    SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_ASIGNACION_PROFESOR_MATERIA", connection);
                    ObjCommand.CommandType = CommandType.StoredProcedure;
                    ObjCommand.Transaction = objMyTransaction;

                    ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                    ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                    ObjCommand.Parameters.Add("@ID_PROFESOR", SqlDbType.VarChar).Value = propiedades.idProfesor;
                    ObjCommand.Parameters.Add("@ID_ASIGNATURA", SqlDbType.VarChar).Value = item;
                    ObjCommand.Parameters.Add("@ID", SqlDbType.Int).Value = contador;
                    int filasAfectadas = ObjCommand.ExecuteNonQuery();
                    contador += 1;
                    if (filasAfectadas == 0) { objMyTransaction.Rollback(); respuesta = 500; break; }
                    ObjCommand.Dispose();
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
            catch (Exception e)
            {
                EscrituraLog.guardar("ProfesoresDataAccess-GuardarAsignacion. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }
            return miObjetoRespuesta;
        }

        public object ListadoHorario(Guid token, string idGrupo, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_HORARIO_PROFESOR", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
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
            catch (Exception e)
            {
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("ProfesoresDataAccess-ListadoHorario. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private List<PropiedadesHorarioProfesor> ArmarObjetoGeneraHorarios(DataSet ds, ref int respuesta)
        {
            List<PropiedadesHorarioProfesor> objHorarios = new List<PropiedadesHorarioProfesor>();
            try
            {
                DataTable datosGenerales = ds.Tables[0];

                string[] columna = { "idAsignatura", "clave", "nombreAsignatura" };
                DataTable dataTable = RegistrosDiferentes.Obtener(datosGenerales, columna);

                foreach (DataRow item in dataTable.Rows)
                {
                    if (Convert.ToInt16(item["idAsignatura"]) != 0)
                    {
                        objHorarios.Add(new PropiedadesHorarioProfesor
                        {
                            idAsignatura = Convert.ToString(item["idAsignatura"]),
                            asignatura = Convert.ToString(item["nombreAsignatura"]),
                            dias = ObtenerDetalleHorario(ds, Convert.ToInt16(item["idAsignatura"]), ref respuesta)
                        });
                    }
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("ProfesoresDataAccess-ArmarObjetoGeneraHorarios. ", e.Message.ToString());
            }
            return objHorarios;
        }

        private List<PropiedadesHorarioDetalleProfesor> ObtenerDetalleHorario(DataSet ds, int idAsignatura, ref int respuesta)
        {
            List<PropiedadesHorarioDetalleProfesor> objHorarios = new List<PropiedadesHorarioDetalleProfesor>();
            try
            {
                DataTable datosGenerales = ds.Tables[0];
                DataTable datosHorarios = ds.Tables[1];
                GruposDataAccess obj = new GruposDataAccess();

                foreach (DataRow item in datosHorarios.Rows)
                {
                    int dia = Convert.ToInt16(item["dia"]);
                    DataRow[] resultado = datosGenerales.Select("idAsignatura ='" + idAsignatura + "' AND dia ='" + dia + "'");

                    if (resultado.Length == 0)
                    {
                        objHorarios.Add(new PropiedadesHorarioDetalleProfesor
                        {
                            nombre = obj.RegresarDiaSemana(dia),
                            inicio = "",
                            final = "",
                        });
                    }

                    foreach (DataRow itemA in resultado)
                    {
                        if (dia == Convert.ToInt16(itemA["dia"]))
                        {
                            objHorarios.Add(new PropiedadesHorarioDetalleProfesor
                            {
                                nombre = Convert.ToString(itemA["nombre"]),
                                inicio = Convert.ToString(itemA["horaInicial"]),
                                final = Convert.ToString(itemA["horaFinal"]),
                            });
                        }
                        else
                        {
                            objHorarios.Add(new PropiedadesHorarioDetalleProfesor
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
                EscrituraLog.guardar("ProfesoresDataAccess-ObtenerDetalleHorario. ", e.Message.ToString());
            }
            return objHorarios;
        }

        public object ListadoAsignaturasAsignadas(Guid token, string idProfesor, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_CATALOGOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = "ProfesorAsignatura";
                ObjCommand.Parameters.Add("@ID", SqlDbType.Int).Value = 0;
                ObjCommand.Parameters.Add("@IDS", SqlDbType.VarChar).Value = idProfesor;
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<AsignaturasAsignadasProfesor> objAsignaturas = new List<AsignaturasAsignadasProfesor>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objAsignaturas.Add(new AsignaturasAsignadasProfesor
                        {
                            idAsignaturaUnique = objReader["idAsignaturaUnique"].ToString(),
                            nombre = objReader["nombre"].ToString(),
                            idProfesor = Convert.ToInt16(objReader["idProfesor"]),
                            urlDocumento = objReader["urlDocumento"].ToString(),
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objAsignaturas;
            }
            catch (Exception e)
            {
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("ProfesoresDataAccess-ListadoAsignaturasAsignadas. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }


        #region "CLASES QUE REGRESAN EL LISTADO DE CALIFICACIONES POR GRUPO Y MATERIA"

        public object ListadoEvaluaciones(Guid token, filtrosCalificaion propiedades, ref int calificacionFinal, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_DETALLE_GRUPO_PROFESOR", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@TIPO", SqlDbType.Int).Value = propiedades.tipo;
                ObjCommand.Parameters.Add("@ID_GRUPOUNIQUE", SqlDbType.VarChar).Value = propiedades.idGrupo.ToString().ToUpper();
                ObjCommand.Parameters.Add("@ID_ASIGNATURA", SqlDbType.Int).Value = propiedades.idAsignatura;
                ObjCommand.Parameters.Add("@ID_ALUMNOUNIQUE", SqlDbType.VarChar).Value = "";
                connection.Open();

                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(ObjCommand);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                int respuesta = 0;
                object objetoFinal = new object();
                objetoFinal = ArmarObjetoCalificaciones(ds, ref calificacionFinal, ref respuesta);

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objetoFinal;
            }
            catch (Exception e)
            {
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("ProfesoresDataAccess-ListadoEvaluaciones. ", e.Message.ToString());
            }
            finally { connection.Dispose(); connection.Close(); }

            return miObjetoRespuesta;
        }

        private List<Parciales> ArmarObjetoCalificaciones(DataSet ds, ref int calificacionFinal, ref int respuesta)
        {
            List<Parciales> objeto = new List<Parciales>();
            try
            {
                DataTable datosGenerales = ds.Tables[0];

                DataRow[] resultado = datosGenerales.Select("idCategoriaEvaluacion = 2");
                calificacionFinal = resultado.Length >= 1 ? 1 : 0;

                string[] columna = { "idAlumno", "matricula", "nombre" };
                DataTable dataTable = RegistrosDiferentes.Obtener(datosGenerales, columna);

                foreach (DataRow item in dataTable.Rows)
                {
                    double final = 0;
                    double promedio = 0;
                    int idDetalleEvaluacion = 0;

                    objeto.Add(new Parciales
                    {
                        matricula = Convert.ToString(item["matricula"]),
                        idAlumno = Convert.ToInt32(item["idAlumno"]),
                        nombre = Convert.ToString(item["nombre"]),
                        parciales = ObtenerParciales(ds, Convert.ToInt32(item["idAlumno"]), ref respuesta),
                        tipo = ObtenerTipo(ds, Convert.ToInt32(item["idAlumno"]), ref final, ref promedio, ref idDetalleEvaluacion, ref respuesta),
                        promedio = promedio,
                        final = final,
                        idDetalleEvaluacion = idDetalleEvaluacion
                    });
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("ProfesoresDataAccess-ArmarObjetoCalificaciones. ", e.Message.ToString());
            }
            return objeto;
        }

        private List<DetalleParcial> ObtenerParciales(DataSet ds, int idAlumno, ref int respuesta)
        {
            List<DetalleParcial> objeto = new List<DetalleParcial>();
            try
            {
                DataTable datosGenerales = ds.Tables[0];
                DataRow[] resultado = datosGenerales.Select("idAlumno ='" + idAlumno + "' AND idCategoriaEvaluacion = 1");

                foreach (DataRow item in resultado)
                {
                    objeto.Add(new DetalleParcial
                    {
                        nombre = Convert.ToString(item["categoriaEvaluacion"]),
                        calif = Convert.ToDouble(item["calificacion"])
                    });
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("ProfesoresDataAccess-ObtenerParciales. ", e.Message.ToString());
            }
            return objeto;
        }

        private Tipo ObtenerTipo(DataSet ds, int idAlumno, ref double final, ref double promedio, ref int idDetalleEvaluacion, ref int respuesta)
        {
            Tipo objeto = new Tipo();
            try
            {
                DataTable datosGenerales = ds.Tables[0];
                DataRow[] resultado = datosGenerales.Select("idAlumno ='" + idAlumno + "' AND idCategoriaEvaluacion = 2");

                if (resultado.Length > 0)
                {
                    objeto.id = 2;
                    objeto.nombreTipo = resultado[0][6].ToString();
                    final = Convert.ToDouble(resultado[0][7]);
                    promedio = Convert.ToDouble(resultado[0][9]);
                    idDetalleEvaluacion = Convert.ToInt32(resultado[0][10]);
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("ProfesoresDataAccess-ObtenerTipo. ", e.Message.ToString());
            }
            return objeto;
        }

        #endregion

        #region "METODOS QUE REGRESAN EL LISTADO DE ASISTENCIAS POR GRUPO Y MATERIA"

        public object ListadoAsistencias(Guid token, filtrosCalificaion propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_DETALLE_GRUPO_PROFESOR", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@TIPO", SqlDbType.Int).Value = propiedades.tipo;
                ObjCommand.Parameters.Add("@ID_GRUPOUNIQUE", SqlDbType.VarChar).Value = propiedades.idGrupo.ToString().ToUpper();
                ObjCommand.Parameters.Add("@ID_ASIGNATURA", SqlDbType.Int).Value = propiedades.idAsignatura;

                string idAlumno = string.Empty; // SE UTILIZA ESTE ID SOLO EN EL DETALLE DEL ALUMNO 
                if (propiedades.tipo == 4) { idAlumno = propiedades.idAlumno.ToString().ToUpper(); }

                ObjCommand.Parameters.Add("@ID_ALUMNOUNIQUE", SqlDbType.VarChar).Value = idAlumno.ToUpper();
                connection.Open();

                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(ObjCommand);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                int respuesta = 0;
                object objetoFinal = new object();
                objetoFinal = ArmarObjetoAsistencias(ds, ref respuesta);

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objetoFinal;
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("ProfesoresDataAccess-ListadoAsistencias. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private List<AsistenciaAlumnos> ArmarObjetoAsistencias(DataSet ds, ref int respuesta)
        {
            List<AsistenciaAlumnos> objeto = new List<AsistenciaAlumnos>();
            try
            {
                DataTable datosGenerales = ds.Tables[0];

                string[] columna = { "idAlumno", "matricula", "nombre", "fechaHoraRegistro" };
                DataTable dataTable = RegistrosDiferentes.Obtener(datosGenerales, columna);

                foreach (DataRow item in dataTable.Rows)
                {
                    objeto.Add(new AsistenciaAlumnos
                    {
                        idAlumno = Convert.ToInt16(item["idAlumno"]),
                        matricula = Convert.ToString(item["matricula"]),
                        nombre = Convert.ToString(item["nombre"]),
                        fechaHoraRegistro = Convert.ToString(item["fechaHoraRegistro"]),
                        asistencias = ObtenerAsistencias(ds, Convert.ToInt32(item["idAlumno"]), ref respuesta)
                    });
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("ProfesoresDataAccess-ArmarObjetoAsistencias. ", e.Message.ToString());
            }
            return objeto;
        }

        private List<DetalleAsistenciaLista> ObtenerAsistencias(DataSet ds, int idAlumno, ref int respuesta)
        {
            List<DetalleAsistenciaLista> objeto = new List<DetalleAsistenciaLista>();
            try
            {
                DataTable datosGenerales = ds.Tables[0];
                DataRow[] resultado = datosGenerales.Select("idAlumno ='" + idAlumno + "'");

                if (resultado[0][3].ToString() != "")
                {
                    foreach (DataRow item in resultado)
                    {
                        objeto.Add(new DetalleAsistenciaLista
                        {
                            fecha = Convert.ToString(item["fechaAsistencia"]),
                            asistencia = Convert.ToInt16(item["asistencia"])
                        });
                    }
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("ProfesoresDataAccess-ObtenerAsistencias. ", e.Message.ToString());
            }
            return objeto;
        }

        #endregion

        #region "METODOS PARA REGRESAR EL LISTADO DE MIS ASIGNATURAS"

        public object ListadoMisAsignaturas(Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_LISTADO_MIS_MATERIAS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<PropiedadesMisAsignaturas> objMisAsignaturas = new List<PropiedadesMisAsignaturas>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objMisAsignaturas.Add(new PropiedadesMisAsignaturas
                        {
                            idDetalleDocumento = Convert.ToInt16(objReader["idDetalleDocumento"]),
                            idAsignatura = Convert.ToInt16(objReader["idAsignatura"]),
                            clave = objReader["clave"].ToString(),
                            nombre = objReader["nombre"].ToString(),
                            temarioPlantel = objReader["temarioPlantel"].ToString(),
                            temarioProfesor = objReader["temarioProfesor"].ToString(),
                            gruposAsignados = Convert.ToInt16(objReader["gruposAsignados"])
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objMisAsignaturas;
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("ProfesoresDataAccess-ListadoMisAsignaturas. ", e.Message.ToString());
            }
            finally { connection.Dispose(); connection.Close(); }

            return miObjetoRespuesta;
        }

        public object GuardarDetalleDocumento(PropiedadesDocumentosProfesor propiedades, Guid token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_DETALLE_DOCUMENTOS_PROFESOR", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@ID_ASIGNATURA", SqlDbType.Int).Value = propiedades.idAsignatura;

                ObjCommand.Parameters.Add("@ID_PROFESOR_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@RESPUESTA_DOCUMENTO", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_EMPRESA_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;

                connection.Open();
                ObjCommand.ExecuteNonQuery();

                int idProfesor = Convert.ToInt16(ObjCommand.Parameters["@ID_PROFESOR_OUT"].Value);
                int idDocumento = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA_DOCUMENTO"].Value);
                int idEmpresa = Convert.ToInt16(ObjCommand.Parameters["@ID_EMPRESA_OUT"].Value);
                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    if (propiedades.archivo.Count > 0)
                    {
                        for (int i = 0; i < propiedades.archivo.Count; i++)
                        {
                            string extension = System.IO.Path.GetExtension(propiedades.archivo[0].FileName);
                            string nombreArchivo = string.Concat(idProfesor, '_', idDocumento, extension);

                            MemoryStream fileStream = General.getStreamFromFile(propiedades.archivo[i]);
                            string filePath = HttpContext.Current.Server.MapPath(string.Concat("~/TemariosProfesor/", idEmpresa, "/", nombreArchivo));
                            bool saveFile = General.saveAttachment(fileStream, filePath);

                            if (saveFile)
                            {
                                string rutaBaseDatos = string.Empty;
                                rutaBaseDatos = string.Concat("/TemariosProfesor/", idEmpresa, "/", nombreArchivo);
                                UrlImagen.Actualizar(connection, "temariosProfesor", idDocumento.ToString(), rutaBaseDatos);
                            }
                        }
                    }
                }
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = idDocumento;
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("ProfesoresDataAccess-GuardarDetalleDocumento. ", e.Message.ToString());
            }
            finally { connection.Dispose(); connection.Close(); }

            return miObjetoRespuesta;
        }

        public object EditarDetalleDocumento(PropiedadesDocumentosProfesor propiedades, Guid token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            int respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_DETALLE_DOCUMENTOS_PROFESOR", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@ID_DETALLE_DOCUMENTO", SqlDbType.Int).Value = propiedades.idDetalleDocumento;
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<ListadoProfesores> objProfesores = new List<ListadoProfesores>();

                int idEmpresa = 0;
                int idProfesor = 0;
                int idAsignatura = 0;
                string urlDocumento = string.Empty;

                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        idEmpresa = Convert.ToInt16(objReader["idEmpresa"]);
                        idProfesor = Convert.ToInt16(objReader["idProfesor"]);
                        idAsignatura = Convert.ToInt16(objReader["idAsignatura"]);
                        urlDocumento = objReader["urlDocumento"].ToString();
                    }
                    objReader.Close();
                }

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                string rutaArchivo = string.Empty;

                string nombreArchivo = string.Concat(idProfesor, '_', propiedades.idDetalleDocumento, ".pdf");

                if (propiedades.archivo.Count > 0)
                {
                    for (int i = 0; i < propiedades.archivo.Count; i++)
                    {
                        rutaArchivo = HttpContext.Current.Server.MapPath(string.Concat("~/TemariosProfesor/", idEmpresa, "/", nombreArchivo));
                        EliminarDocumento(rutaArchivo, ref Codigo_Respuesta_servicio);

                        UrlImagen.Actualizar(connection, "temariosProfesor", Convert.ToString(propiedades.idDetalleDocumento), "");

                        MemoryStream fileStream = General.getStreamFromFile(propiedades.archivo[i]);
                        string filePath = HttpContext.Current.Server.MapPath(string.Concat("~/TemariosProfesor/", idEmpresa, "/", nombreArchivo));
                        bool saveFile = General.saveAttachment(fileStream, filePath);

                        if (saveFile)
                        {
                            string rutaBaseDatos = string.Empty;
                            rutaBaseDatos = string.Concat("/TemariosProfesor/", idEmpresa, "/", nombreArchivo);
                            UrlImagen.Actualizar(connection, "temariosProfesor", Convert.ToString(propiedades.idDetalleDocumento), rutaBaseDatos);
                        }
                    }
                }
                if (propiedades.contieneDocumento == 0)
                {

                    rutaArchivo = HttpContext.Current.Server.MapPath(string.Concat("~/TemariosProfesor/", idEmpresa, "/", nombreArchivo));
                    EliminarDocumento(rutaArchivo, ref Codigo_Respuesta_servicio);
                    UrlImagen.Actualizar(connection, "temariosProfesor", Convert.ToString(propiedades.idDetalleDocumento), "");
                }

                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = new object();
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("ProfesoresDataAccess-EditarDetalleDocumento. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private static void EliminarDocumento(string rutaArchivo, ref int respuesta)
        {
            try
            {
                if (File.Exists(rutaArchivo))
                {
                    File.Delete(rutaArchivo);
                    respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                }
                else
                {
                    respuesta = Convert.ToInt16(0);
                }
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("ProfesoresDataAccess-EliminarDocumento. ", e.Message.ToString());
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
            }
        }

        #endregion


        #region "METODOS PARA ARMAR EL LISTADO DE TAREAS ENTREGADAS"

        public object ListadoTareasEntregadas(Guid token, string idTarea, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_LISTADO_TAREAS_ENTREGADAS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@ID_TAREA", idTarea));
                connection.Open();

                ArrayList objeto = new ArrayList();

                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(ObjCommand);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = ArmarObjetoTareasEntregadas(ds);
            }
            catch (Exception e)
            {
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("ProfesoresDataAccess-ListadoTareasEntregadas. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private List<object> ArmarObjetoTareasEntregadas(DataSet ds)
        {
            DataTable datosGenerales = ds.Tables[0];
            List<object> miLista = new List<object>(); 
            Dictionary<string, string> objetoDinamco;

            try
            {
                DataRow[] resultado = datosGenerales.Select();
                for (int i = 0; i < datosGenerales.Rows.Count; i++)
                {
                    objetoDinamco = new Dictionary<string, string>();
                    int contador = 0;
                    foreach (DataColumn column in ds.Tables[0].Columns)
                    {
                        objetoDinamco.Add(column.ColumnName.ToString(), resultado[i][contador].ToString());
                        contador += 1;
                    }
                    miLista.Add(objetoDinamco);
                }
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("ProfesoresDataAccess-ListadoTareasEntregadas. ", e.Message.ToString());
            }
            return miLista;
        }

        #endregion
    }
}
