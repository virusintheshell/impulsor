
//AUTOR: DIEGO OLVERA
//FECHA: 23-04-2019
//DESCRIPCIÓN: CLASE QUE GESTIONA EL CRUD DE ASIGNATURAS

namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using System.IO;
    using Framework;
    using System.Net;
    using System.Web;
    using System.Data;
    using Newtonsoft.Json;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;

    public class AsignaturaDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        public object Listado(Guid token, int estatus, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_LISTADO_ASIGNATURAS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@ESTATUS", estatus));

                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<ListadoAsingaturas> objAsignatura = new List<ListadoAsingaturas>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objAsignatura.Add(new ListadoAsingaturas
                        {
                            idAsignatura = objReader["idAsignatura"].ToString(),
                            clave = objReader["clave"].ToString(),
                            nombre = objReader["nombre"].ToString(),
                            temario = objReader["temario"].ToString(),
                            estatus = Convert.ToInt16(objReader["estatus"])
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = 200;
                textoRespuesta = "OK";
                miObjetoRespuesta = objAsignatura;

               
            }
            catch (Exception e) {
                EscrituraLog.guardar("AsignaturaDataAccess-Listado. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Guardar(Asignaturas propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_ASIGNATURAS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@CLAVE", SqlDbType.VarChar).Value = propiedades.clave.Trim();
                ObjCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = propiedades.nombre.Trim();

                ObjCommand.Parameters.Add("@ID_ASIGNATURA_OUT", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_EMPRESA_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;

                connection.Open();
                ObjCommand.ExecuteNonQuery();

                string idAsignaturaOut = Convert.ToString(ObjCommand.Parameters["@ID_ASIGNATURA_OUT"].Value);
                int idEmpresaOut = Convert.ToInt16(ObjCommand.Parameters["@ID_EMPRESA_OUT"].Value);
                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    GuardarArchivo(connection, propiedades, idAsignaturaOut, idEmpresaOut, ref respuesta);
                }

                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = new object();

               
            }
            catch (Exception e) {
                EscrituraLog.guardar("AsignaturaDataAccess-Listado. ", e.Message.ToString());
            }
            finally { connection.Close();  connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private static void GuardarArchivo(SqlConnection objMyConnection, Asignaturas propiedades, string idEvidenciaOut, int idEmpresa, ref int respuesta)
        {
            try
            {
                if (propiedades.archivo.Count > 0)
                {
                    for (int i = 0; i < propiedades.archivo.Count; i++)
                    {
                        string extension = System.IO.Path.GetExtension(propiedades.archivo[0].FileName);
                        string nombreArchivo = string.Concat(idEvidenciaOut, extension);

                        MemoryStream fileStream = General.getStreamFromFile(propiedades.archivo[i]);
                        string filePath = HttpContext.Current.Server.MapPath(string.Concat("~/Temarios/", idEmpresa, "/", nombreArchivo));
                        bool saveFile = General.saveAttachment(fileStream, filePath);

                        if (saveFile)
                        {
                            string rutaBaseDatos = string.Empty;
                            rutaBaseDatos = string.Concat("/Temarios/", idEmpresa, "/", nombreArchivo);
                            UrlImagen.Actualizar(objMyConnection, "temarios", nombreArchivo, rutaBaseDatos);
                        }
                    }
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e) {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("AsignaturaDataAccess-GuardarArchivo. ", e.Message.ToString());
            }
        }

        public object Obtener(Guid token, string pagina, string idAsignatura, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_ASIGNATURA", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@PAGINA", pagina));
                ObjCommand.Parameters.Add(new SqlParameter("@ID_ASIGNATURA", idAsignatura));

                connection.Open();
                Asignaturas objAsignatura = new Asignaturas();
                SqlDataReader objReader = ObjCommand.ExecuteReader();

                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objAsignatura.clave = objReader["clave"].ToString();
                        objAsignatura.nombre = objReader["nombre"].ToString();
                        objAsignatura.urlDocumento = objReader["urlDocumento"].ToString();
                    }
                }
                objReader.Close();

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objAsignatura;

                
            }
            catch (Exception e){
                EscrituraLog.guardar("AsignaturaDataAccess-Obtener. ", e.Message.ToString());
            }
            finally { connection.Close();  connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Editar(Asignaturas propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_EDITAR_ASIGNATURA", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@ID_ASIGNATURA", SqlDbType.VarChar).Value = propiedades.idAsignatura;

                ObjCommand.Parameters.Add("@CLAVE", SqlDbType.VarChar).Value = propiedades.clave.Trim();
                ObjCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = propiedades.nombre.Trim();

                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_EMPRESA_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                connection.Open();
                ObjCommand.ExecuteNonQuery();

                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);
                int idEmpresa = Convert.ToInt16(ObjCommand.Parameters["@ID_EMPRESA_OUT"].Value);

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                string rutaArchivo = string.Empty;
               
                string nombrArchivoGeneral = propiedades.idAsignatura;

                string nombreArchivoWord = string.Concat(nombrArchivoGeneral, ".docx");
                string nombreArchivoPDF = string.Concat(nombrArchivoGeneral, ".pdf");

                if (propiedades.archivo.Count > 0)
                {
                    for (int i = 0; i < propiedades.archivo.Count; i++)
                    {
                        rutaArchivo = HttpContext.Current.Server.MapPath(string.Concat("~/Temarios/", idEmpresa, "/", nombreArchivoWord));
                        EliminarDocumento(rutaArchivo, ref Codigo_Respuesta_servicio);

                        if (Codigo_Respuesta_servicio == 0)
                        {
                            rutaArchivo = HttpContext.Current.Server.MapPath(string.Concat("~/Temarios/", idEmpresa, "/", nombreArchivoPDF));
                            EliminarDocumento(rutaArchivo, ref Codigo_Respuesta_servicio);

                        }
                        UrlImagen.Actualizar(connection, "temarios", Convert.ToString(nombrArchivoGeneral), "");

                        string extension = System.IO.Path.GetExtension(propiedades.archivo[0].FileName);
                        string nombreArchivo = string.Concat(propiedades.idAsignatura, extension);

                        MemoryStream fileStream = General.getStreamFromFile(propiedades.archivo[i]);
                        string filePath = HttpContext.Current.Server.MapPath(string.Concat("~/Temarios/", idEmpresa, "/", nombreArchivo));
                        bool saveFile = General.saveAttachment(fileStream, filePath);

                        if (saveFile)
                        {
                            string rutaBaseDatos = string.Empty;
                            rutaBaseDatos = string.Concat("/Temarios/", idEmpresa, "/", nombreArchivo);
                            UrlImagen.Actualizar(connection, "temarios", nombreArchivo, rutaBaseDatos);
                        }
                    }
                }
                if (propiedades.contieneDocumento == 0) {

                    rutaArchivo = HttpContext.Current.Server.MapPath(string.Concat("~/Temarios/", idEmpresa, "/", nombreArchivoWord));
                    EliminarDocumento(rutaArchivo, ref Codigo_Respuesta_servicio);

                    if (Codigo_Respuesta_servicio == 0)
                    {
                        rutaArchivo = HttpContext.Current.Server.MapPath(string.Concat("~/Temarios/", idEmpresa, "/", nombreArchivoPDF));
                        EliminarDocumento(rutaArchivo, ref Codigo_Respuesta_servicio);

                    }
                    UrlImagen.Actualizar(connection, "temarios", Convert.ToString(nombrArchivoGeneral), "");
                }

                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = new object();

                connection.Close();
            }
            catch (Exception e) {
                EscrituraLog.guardar("AsignaturaDataAccess-Editar. ", e.Message.ToString());
            }
            finally { connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public static void EliminarDocumento(string rutaArchivo, ref int respuesta)
        {
            try
            {
                if (File.Exists(rutaArchivo))
                {
                    File.Delete(rutaArchivo);
                    respuesta = Convert.ToInt16(200);
                }
                else {
                    respuesta = Convert.ToInt16(0);
                }
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(500);
                EscrituraLog.guardar("AsignaturaDataAccess-EliminarDocumento. ", e.Message.ToString());
            }
        }

    }
}
