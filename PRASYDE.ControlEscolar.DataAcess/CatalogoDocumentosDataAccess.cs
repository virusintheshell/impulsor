namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using System.Net;
    using System.Data;
    using Newtonsoft.Json;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.DataAcess.Framework;


    public class CatalogoDocumentosDataAccess : BaseDataAccess 
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        public object Obtener(Guid token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try 
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_CATALOGO_DOCUMENTOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<CatalogoDocumentos> objDocumentos = new List<CatalogoDocumentos>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objDocumentos.Add(new CatalogoDocumentos
                        {
                            idCatalogoExpedienteDigital = Convert.ToInt16(objReader["idCatalogoExpedienteDigital"]),
                            nombre = objReader["nombre"].ToString(),
                        });
                    }
                }
                objReader.Close();
                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objDocumentos;
            }
            
            catch (Exception e)
            {
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("DocumentosDataAccess-Obtener. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }
    }

}
