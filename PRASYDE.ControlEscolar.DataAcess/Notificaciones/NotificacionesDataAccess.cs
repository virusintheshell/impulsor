
//AUTOR: DIEGO OLVERA
//FECHA: 25-07-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA ENTREGAR EL LISTADO DE NOTIFIACIONES POR USUARIO

namespace PRASYDE.ControlEscolar.DataAcess.Notificaciones
{
    using System;
    using Framework;
    using System.Net;
    using System.Data;
    using Newtonsoft.Json;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;

    public class NotificacionesDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        public object ListadoMisNotificaciones(Guid token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_NOTIFICACIONES", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<PropiedadesMisNotificaciones> objMisNotificaciones = new List<PropiedadesMisNotificaciones>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objMisNotificaciones.Add(new PropiedadesMisNotificaciones
                        {
                            idReferencia = Convert.ToInt32(objReader["id"]),
                            title = objReader["title"].ToString(),
                            body = objReader["body"].ToString(),
                            icon = objReader["icon"].ToString(),
                            tipoNotificacion = Convert.ToInt16(objReader["tipoNotificacion"]),
                            nombreNotificacion = objReader["notificacion"].ToString(),
                            color = objReader["color"].ToString(),
                            url = objReader["url"].ToString(),
                            fechaHoraRegistro = objReader["fechaHoraRegistro"].ToString(),
                        });
                    }
                    objReader.Close();
                }
                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objMisNotificaciones;
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("AlumnosDataAccess-ListadoMisNotificaciones. Error al obtener el listado de mis notifacaciones: ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }
            return miObjetoRespuesta;
        }

        public object CambiarEstatusNotificacion(Guid token,int idReferencia, int tipoNotificacion, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_CAMBIAR_ESTATUS_NOTIFICACION", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@ID_REFERENCIA", SqlDbType.VarChar).Value = idReferencia;
                ObjCommand.Parameters.Add("@TIPO_NOTIFICACION", SqlDbType.VarChar).Value = tipoNotificacion;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                connection.Open();

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
            catch (Exception e)
            {
                EscrituraLog.guardar("NotificacionesDataAccess-CambiarEstatusNotificacion. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }
    }
}
