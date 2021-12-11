
namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using System.Net;
    using System.Data;
    using Newtonsoft.Json;
    using System.Data.SqlClient;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.DataAcess.Framework;

    public class SesionDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        public object obtenerToken(CredencialesAcceso credenciales, int Plataforma, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            int respuesta = 0;
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_INICIAR_SESION", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = credenciales.Usuario;
                ObjCommand.Parameters.Add("@CONTRASENA", SqlDbType.VarChar).Value = credenciales.Contrasena;
                ObjCommand.Parameters.Add("@PLATAFORMA", SqlDbType.Int).Value = Plataforma;

                if (string.IsNullOrEmpty(credenciales.IMEI)) { credenciales.IMEI = ""; };
                ObjCommand.Parameters.Add("@IMEI", SqlDbType.VarChar).Value = credenciales.IMEI;

                ObjCommand.Parameters.Add("@TOKEN_OUTPUT", SqlDbType.VarChar,200).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@RESTABLECER_CONTRASENA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_JERARQUIA", SqlDbType.Int).Direction = ParameterDirection.Output;
                connection.Open();
                ObjCommand.ExecuteNonQuery();

                PropidadesUsuarioLogueado obj = new PropidadesUsuarioLogueado();
                obj.token = Convert.ToString(ObjCommand.Parameters["@TOKEN_OUTPUT"].Value).ToUpper();
                obj.restablecerContrasena = Convert.ToInt16(ObjCommand.Parameters["@RESTABLECER_CONTRASENA"].Value);
                obj.idJerarquia = Convert.ToInt16(ObjCommand.Parameters["@ID_JERARQUIA"].Value);

                respuesta = (string.IsNullOrEmpty(obj.token)) ? respuesta = 204 : respuesta = 200;

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = obj;
            }
            catch (Exception e)
            {
                Codigo_Respuesta = HttpStatusCode.InternalServerError;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                textoRespuesta = "Internal Server Error";
                EscrituraLog.guardar("SesionDataAccess-obtenerToken. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object guardarTokenFireBase(Guid token,string tokenFireBase, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            int respuesta = 0;
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_DETALLE_SESION", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@TOKEN_FIREBASE", SqlDbType.VarChar).Value = tokenFireBase.ToString();
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                
                connection.Open();
                ObjCommand.ExecuteNonQuery();
                respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = new object { };
            }
            catch (Exception e)
            {
                Codigo_Respuesta = HttpStatusCode.InternalServerError;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                textoRespuesta = "Internal Server Error";
                EscrituraLog.guardar("SesionDataAccess-guardarTokenFireBase. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }
    }

    public class PropidadesUsuarioLogueado
    {
        public string token { get; set; }
        public int restablecerContrasena { get; set; }
        public int idJerarquia { get; set; }
    }
}
