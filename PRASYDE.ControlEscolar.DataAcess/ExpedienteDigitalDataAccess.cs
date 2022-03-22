
namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using Entities;
    using System.IO;
    using Framework;
    using System.Net;
    using System.Web;
    using System.Data;
    using Newtonsoft.Json;
    using System.Data.SqlClient;
    using System.Collections.Generic;

    public class ExpedienteDigitalDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        public object Guardar(propiedadesExpediente propiedades, Guid token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            SqlTransaction objMyTransaction = null;

            string textoRespuestaServicio = string.Empty;
            int Codigo_Respuesta_servicio = 0;

            try
            {
                connection.Open();
                objMyTransaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                int respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                int idAlumno = 0;
                int idEmpresa = 0;

                respuesta = ObtenerEmpresa(connection, objMyTransaction, token, propiedades.idAlumno.Trim(), ref idEmpresa, ref idAlumno);

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK) && idAlumno != 0 && propiedades.archivos.Count > 0)
                {
                    for (int i = 0; i < propiedades.archivos.Count; i++)
                    {
                        string nombreArchivo = System.IO.Path.GetFileName(propiedades.archivos[i].FileName);

                        MemoryStream fileStream = General.getStreamFromFile(propiedades.archivos[i]);
                        string filePath = HttpContext.Current.Server.MapPath(string.Concat("~/ExpendienteDigital/", idEmpresa, "/", idAlumno, "/", nombreArchivo));
                        bool saveFile = General.saveAttachment(fileStream, filePath);
                        string rutaBaseDatos = string.Concat("/ExpendienteDigital/", idEmpresa, "/", idAlumno, "/", nombreArchivo);

                        if (saveFile)
                        {
                            SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_ARCHIVOS", connection);
                            ObjCommand.CommandType = CommandType.StoredProcedure;
                            ObjCommand.Transaction = objMyTransaction;

                            ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;

                            ObjCommand.Parameters.Add("@ID_ALUMNO", SqlDbType.VarChar).Value = propiedades.idAlumno.Trim();
                            ObjCommand.Parameters.Add("@ID_DOCUMENTO", SqlDbType.Int).Value = propiedades.idDocumento;
                            ObjCommand.Parameters.Add("@TIPO", SqlDbType.Int).Value = propiedades.TipoEvidencia;
                            ObjCommand.Parameters.Add("@URL", SqlDbType.VarChar).Value = rutaBaseDatos;

                            ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                            ObjCommand.ExecuteNonQuery();

                            respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);
                            if (respuesta != Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                            {
                                break;
                            }
                        }
                    }

                    if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        objMyTransaction.Commit();
                    }
                    else
                    {
                        objMyTransaction.Rollback();
                    }
                }
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = new object();
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("ExpedienteDigitalDataAccess-Guardar. ", e.Message.ToString());
                objMyTransaction.Rollback();
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return miObjetoRespuesta;
        }

        private int ObtenerEmpresa(SqlConnection connection, SqlTransaction objMyTransaction, Guid token, string Alumno, ref int idEmpresa, ref int idAlumno)
        {
            int respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_ID_EMPRESA", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Transaction = objMyTransaction;


                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@ID_ALUMNO_UNIQUE", SqlDbType.VarChar).Value = Alumno;

                ObjCommand.Parameters.Add("@ID_EMPRESA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_ALUMNO", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.ExecuteNonQuery();

                idEmpresa = Convert.ToInt16(ObjCommand.Parameters["@ID_EMPRESA"].Value);
                idAlumno = Convert.ToInt16(ObjCommand.Parameters["@ID_ALUMNO"].Value);

                if (idAlumno == 0)
                {
                    respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.NoContent);
                }
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("ExpedienteDigitalDataAccess-ObtenerEmpresa. ", e.Message.ToString());
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
            }

            return respuesta;
        }

        public object ListadoArchivos(Guid token, string idAlumno, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_ARCHIVOS_ALUMNOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@ID_ALUMNO_UNIQUE", SqlDbType.VarChar).Value = idAlumno;
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<listadoArchivosAlumnos> obj = new List<listadoArchivosAlumnos>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        obj.Add(new listadoArchivosAlumnos
                        {
                            idExpediente = Convert.ToInt32(objReader["idExpediente"]),
                            urlDocumento = objReader["urlDocumento"].ToString(),
                        });
                    }
                    objReader.Close();
                    Codigo_Respuesta = HttpStatusCode.OK;
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    textoRespuesta = "OK";
                }
                else
                {
                    Codigo_Respuesta = HttpStatusCode.NoContent;
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.NoContent);
                    textoRespuesta = "No Content";
                }


                miObjetoRespuesta = obj;
            }
            catch (Exception e)
            {
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("GruposDataAccess-ListadoArchivos. ", e.Message.ToString());
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return miObjetoRespuesta;
        }



        public object Editar(propiedadesExpediente propiedades, Guid token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            SqlTransaction objMyTransaction = null;

            string textoRespuestaServicio = string.Empty;
            int Codigo_Respuesta_servicio = 0;

            try
            {
                connection.Open();
                objMyTransaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                int respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                int idAlumno = 0;
                int idEmpresa = 0;

                respuesta = ObtenerEmpresa(connection, objMyTransaction, token, propiedades.idAlumno.Trim(), ref idEmpresa, ref idAlumno);

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    EditarArchivos(connection, objMyTransaction, token, propiedades.idAlumno.Trim());
                }


                foreach (string urlArchivo in propiedades.urlArchivosEdicion)
                {
                    if (urlArchivo != null)
                    {
                        string[] cadena = urlArchivo.Split('/');
                        string nombreArchivo = cadena[4];

                        string rutaBaseDatos = string.Concat("/ExpendienteDigital/", idEmpresa, "/", idAlumno, "/", nombreArchivo);

                        //GUARDAMOS ARCHIVOS EN BASE DE DATOS 
                        respuesta = GuardarArchivosEdicion(connection, objMyTransaction, token, propiedades.idAlumno.Trim(), rutaBaseDatos);
                        if (respuesta != Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                        {
                            respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                            break;
                        }
                    }
                }

                //ELIMINAMOS DE LA CARPETA LOS ARCHIVOS 
                foreach (string urlArchivoEliminado in propiedades.urlArchivosEliminados)
                {
                    if (urlArchivoEliminado != null)
                    {
                        string[] cadena = urlArchivoEliminado.Split('/');
                        string nombreArchivo = cadena[4];
                                             
                        string filePath = HttpContext.Current.Server.MapPath(string.Concat("~/ExpendienteDigital/", idEmpresa, "/", idAlumno, "/", nombreArchivo));

                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }
                }

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK) && idAlumno != 0 && propiedades.archivos.Count > 0)
                {
                    for (int i = 0; i < propiedades.archivos.Count; i++)
                    {
                        string nombreArchivo = System.IO.Path.GetFileName(propiedades.archivos[i].FileName);

                        MemoryStream fileStream = General.getStreamFromFile(propiedades.archivos[i]);
                        string filePath = HttpContext.Current.Server.MapPath(string.Concat("~/ExpendienteDigital/", idEmpresa, "/", idAlumno, "/", nombreArchivo));
                        bool saveFile = General.saveAttachment(fileStream, filePath);
                        string rutaBaseDatos = string.Concat("/ExpendienteDigital/", idEmpresa, "/", idAlumno, "/", nombreArchivo);

                        if (saveFile)
                        {
                            SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_ARCHIVOS", connection);
                            ObjCommand.CommandType = CommandType.StoredProcedure;
                            ObjCommand.Transaction = objMyTransaction;

                            ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;

                            ObjCommand.Parameters.Add("@ID_ALUMNO", SqlDbType.VarChar).Value = propiedades.idAlumno.Trim();
                            ObjCommand.Parameters.Add("@ID_DOCUMENTO", SqlDbType.Int).Value = propiedades.idDocumento;
                            ObjCommand.Parameters.Add("@TIPO", SqlDbType.Int).Value = propiedades.TipoEvidencia;
                            ObjCommand.Parameters.Add("@URL", SqlDbType.VarChar).Value = rutaBaseDatos;

                            ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                            ObjCommand.ExecuteNonQuery();

                            respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);
                            if (respuesta != Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                            {
                                break;
                            }
                        }
                    }

                    if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        objMyTransaction.Commit();
                    }
                    else
                    {
                        objMyTransaction.Rollback();
                    }
                }
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = new object();
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("ExpedienteDigitalDataAccess-Guardar. ", e.Message.ToString());
                objMyTransaction.Rollback();
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return miObjetoRespuesta;
        }

        private int EditarArchivos(SqlConnection connection, SqlTransaction objMyTransaction, Guid token, string Alumno)
        {
            int respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_EDITAR_ARCHIVOS_ALUMNOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Transaction = objMyTransaction;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@ID_ALUMNO_UNIQUE", SqlDbType.VarChar).Value = Alumno;
                ObjCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("ExpedienteDigitalDataAccess-EditarArchivos. ", e.Message.ToString());
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
            }
            return respuesta;
        }

        //METODO NOS SIRVE PARA GUARDAR IMAGENES SOLO CON LA URL QUE VIENEN DE LA E DICION 
        private int GuardarArchivosEdicion(SqlConnection connection, SqlTransaction objMyTransaction, Guid token, string Alumno, string rutaBaseDatos)
        {
            int respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_ARCHIVOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Transaction = objMyTransaction;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;

                ObjCommand.Parameters.Add("@ID_ALUMNO", SqlDbType.VarChar).Value = Alumno.Trim();
                ObjCommand.Parameters.Add("@ID_DOCUMENTO", SqlDbType.Int).Value = 1;
                ObjCommand.Parameters.Add("@TIPO", SqlDbType.Int).Value = 1;
                ObjCommand.Parameters.Add("@URL", SqlDbType.VarChar).Value = rutaBaseDatos;

                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("ExpedienteDigitalDataAccess-GuardarArchivosEdicion. ", e.Message.ToString());
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
            }
            return respuesta;
        }

    }

}

