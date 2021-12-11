//AUTOR: DIEGO OLVERA
//FECHA: 29-03-2019
//DESCRIPCIÓN: CLASE QUE REGRESA UNA LISTA CON LOS ELEMENTOS PARA ARMAR EL PERFIL DEL USUARIO 

namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using System.Net;
    using System.Data;
    using Newtonsoft.Json;
    using System.Data.SqlClient;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.DataAcess.Framework;

    public class PerfilDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        public object ObtenerMiPerfil(string token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_PERFIL", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));

                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                Perfil objPerfil = new Perfil();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objPerfil.idUsuario = Convert.ToInt16(objReader["idUsuario"]);
                        objPerfil.imagen = objReader["imagen"].ToString();
                        objPerfil.nombre = objReader["nombre"].ToString();
                        objPerfil.idRol = Convert.ToInt32(objReader["idRol"]);
                        objPerfil.nombreRol = objReader["nombreRol"].ToString();
                    }
                }
                objReader.Close();

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objPerfil;
            }
            catch (Exception e) {
             
                Codigo_Respuesta = HttpStatusCode.InternalServerError;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                textoRespuesta = "Internal Server Error";
                EscrituraLog.guardar("PerfilDataAccess-ObtenerMiPerfil. ", e.Message.ToString());

            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }
    }
}
