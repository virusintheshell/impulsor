
using System;
using System.Data;
using System.Data.SqlClient;
using PRASYDE.ControlEscolar.Entities;

namespace PRASYDE.ControlEscolar.DataAcess.Framework
{
    public class UrlImagen
    {
        public static RespuestaGeneral Actualizar(SqlConnection objMyConnection, string pagina, string idUnique, string url)
        {
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            SqlCommand objMyCommand = new SqlCommand();
            objMyCommand.CommandText = "USP_ACTUALIZAR_URL";
            objMyCommand.CommandType = CommandType.StoredProcedure;
            objMyCommand.Connection = objMyConnection;
            try
            {
                objMyCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina.Trim();
                objMyCommand.Parameters.Add("@ID", SqlDbType.VarChar).Value = idUnique;
                objMyCommand.Parameters.Add("@URL", SqlDbType.VarChar).Value = url.Trim();
                objMyCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                objMyCommand.ExecuteNonQuery();

                ObjetoRespuesta.status = 200;
                ObjetoRespuesta.message = "OK";
                ObjetoRespuesta.result = new object { };

            }
            catch (Exception e)
            {
                ObjetoRespuesta.status = 500;
                ObjetoRespuesta.message = e.ToString();
                ObjetoRespuesta.result = new object { };
            }
            finally
            {
                objMyCommand.Dispose();
            }

            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Actualizar(SqlConnection objMyConnection, SqlTransaction objMyTransaction, string pagina, string idUnique, string url)
        {
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            SqlCommand objMyCommand = new SqlCommand();
            objMyCommand.CommandText = "USP_ACTUALIZAR_URL";
            objMyCommand.CommandType = CommandType.StoredProcedure;

            objMyCommand.Connection = objMyConnection;
            objMyCommand.Transaction = objMyTransaction;

            try
            {
                objMyCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina.Trim();
                objMyCommand.Parameters.Add("@ID", SqlDbType.VarChar).Value = idUnique;
                objMyCommand.Parameters.Add("@URL", SqlDbType.VarChar).Value = url.Trim();
                objMyCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                objMyCommand.ExecuteNonQuery();


                ObjetoRespuesta.status = 200;
                ObjetoRespuesta.message = "OK";
                ObjetoRespuesta.result = new object { };

            }
            catch (Exception e)
            {
                ObjetoRespuesta.status = 500;
                ObjetoRespuesta.message = e.ToString();
                ObjetoRespuesta.result = new object { };
            }
            finally
            {
                objMyCommand.Dispose();
            }

            return ObjetoRespuesta;
        }

    }
}
