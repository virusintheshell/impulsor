
//AUTOR: DIEGO OLVERA
//FECHA: 06-06-2019
//DESCRIPCIÓN: CLASE QUE GESTIONA LOS OBJETOS DE LAS INSCRIPCIONES

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

    public class DocumentosDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        public object Obtener(Guid token, string idPrograma, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_LISTADO_DOCUMENTOS_PROGRAMA", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@ID_PROGRAMA_EDUCATIVO", SqlDbType.VarChar).Value = idPrograma;
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<ListaDocumentosPrograma> objDocumentos = new List<ListaDocumentosPrograma>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objDocumentos.Add(new ListaDocumentosPrograma
                        {
                            idDocumento = Convert.ToInt16(objReader["idDocumento"]),
                            nombre = objReader["nombre"].ToString(),
                        });
                    }
                    objReader.Close();
                }
                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objDocumentos;
            }
            catch (Exception e) {
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("DocumentosDataAccess-Obtener. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }
    }
}
