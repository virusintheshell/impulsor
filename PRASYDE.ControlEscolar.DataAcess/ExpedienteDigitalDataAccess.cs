
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
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_ARCHIVOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;

                ObjCommand.Parameters.Add("@ID_ALUMNO", SqlDbType.VarChar).Value = propiedades.idAlumno.Trim();
                ObjCommand.Parameters.Add("@ID_DOCUMENTO", SqlDbType.Int).Value = propiedades.idDocumento;
                ObjCommand.Parameters.Add("@TIPO", SqlDbType.Int).Value = propiedades.TipoEvidencia;

                ObjCommand.Parameters.Add("@ID_EVIDENCIA_OUT", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_EMPRESA_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;

                connection.Open();
                ObjCommand.ExecuteNonQuery();

                string idEvidenciaOut = Convert.ToString(ObjCommand.Parameters["@ID_EVIDENCIA_OUT"].Value);
                int idEmpresaOut = Convert.ToInt16(ObjCommand.Parameters["@ID_EMPRESA_OUT"].Value);
                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
              
                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    GuardarImagenes(connection, propiedades, propiedades.TipoEvidencia, idEvidenciaOut, idEmpresaOut, ref respuesta);
                }
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = new object();
            }
            catch (Exception e) {
                EscrituraLog.guardar("ExpedienteDigitalDataAccess-Guardar. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public static void GuardarImagenes(SqlConnection objMyConnection, propiedadesExpediente propiedades, int tipoEvidencia, string idEvidenciaOut, int idEmpresa, ref int respuesta)
        {
            try
            {
                if (propiedades.archivos.Count > 0)
                {
                    for (int i = 0; i < propiedades.archivos.Count; i++)
                    {

                        string extension = System.IO.Path.GetExtension(propiedades.archivos[0].FileName);
                        string nombreArchivo = string.Concat(idEvidenciaOut,extension);

                        MemoryStream fileStream = General.getStreamFromFile(propiedades.archivos[i]);
                        string filePath = HttpContext.Current.Server.MapPath(string.Concat("~/ExpendienteDigital/", idEmpresa, "/", tipoEvidencia, "/", nombreArchivo));
                        bool saveFile = General.saveAttachment(fileStream, filePath);

                        if (saveFile)
                        {
                            string rutaBaseDatos = string.Empty;
                            rutaBaseDatos = string.Concat("/ExpendienteDigital/", idEmpresa, "/", tipoEvidencia, "/", nombreArchivo);
                            UrlImagen.Actualizar(objMyConnection, "evidencias", nombreArchivo, rutaBaseDatos);
                        }
                    }
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e) {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("ExpedienteDigitalDataAccess-GuardarImagenes. ", e.Message.ToString());
            }
        }
    }

}

