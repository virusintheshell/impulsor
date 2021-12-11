
//AUTOR: DIEGO OLVERA
//FECHA: 12-07-2020
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA OBTENER EL LISTADO DE USUARIOS A QUIEN SE ESTARA ENVIANDO NOTIFICACION

namespace PRASYDE.ControlEscolar.DataAcess.Notificaciones
{
    using System;
    using System.Net;
    using System.Data;
    using Newtonsoft.Json;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.DataAcess.Framework;

    public class TokenActivosDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        public object ListadoTokenUsuarios(Guid token, int idReferencia, int tipoNotificacion, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_TOKEN_ACTIVOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@ID_REFERENCIA", idReferencia));
                ObjCommand.Parameters.Add(new SqlParameter("@TIPO_NOTIFICACION", tipoNotificacion));
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<tokenFirebase> ListaNotificacion = new List<tokenFirebase>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        ListaNotificacion.Add(new tokenFirebase
                        {
                            id = Convert.ToInt32(objReader["id"]), //ID_REFERENCIA
                            token = Convert.ToString(objReader["token"]),
                            title = Convert.ToString(objReader["title"]),
                            body = Convert.ToString(objReader["body"]),
                            icon = Convert.ToString(objReader["icon"])
                        });
                    }
                }
                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = ListaNotificacion;
            }
            catch (Exception exception) { throw exception; }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }
    }
}
