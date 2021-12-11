

namespace PRASYDE.ControlEscolar.Business.Framework
{
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using DataAcess;
    using System.Data;
    using System.Web.Configuration;

    public class ValidarSesion
    {
        ///VALIDA QUE EL TOKEN ESTE ACTIVO Y QUE NO TENGA MAS DE 20 MIN DE USO (VALOR CONFIGURABLE EN EL WEB.CONFIG)
        public static bool tokenValido()
        {

            bool sesionValida = false;
            Guid token = Guid.Empty;
            string valor = ConfigurationManager.AppSettings["ValorSesion"].ToString();
            int respuesta = 0;

            if (ValidarCabeceras.ContieneCabeceras(ref token))
            {
                SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ImpulsorIntelectual"].ConnectionString);
                try
                {
                    SqlCommand ObjCommand = new SqlCommand("USP_VALIDA_SESION", connection);
                    ObjCommand.CommandType = CommandType.StoredProcedure;

                    ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                    ObjCommand.Parameters.Add("@VALOR", SqlDbType.Int).Value = valor;

                    connection.Open();
                    respuesta = Convert.ToInt16(ObjCommand.ExecuteScalar());

                    sesionValida = (respuesta == 1) ? true : false;

                }
                catch (Exception exception) { throw exception; }
                finally { connection.Dispose(); }
            }
            return sesionValida;
        }
    }
}