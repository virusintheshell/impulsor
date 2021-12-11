
//AUTOR: DIEGO OLVERA
//FECHA: 18-04-2020
//DESCRIPCIÓN: CLASE QUE OBTIENE LOS DATOS PARA ARMAR UN FORMULARIO

namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using Framework;
    using System.Data;
    using Newtonsoft.Json;
    using System.Data.SqlClient;
    using PRASYDE.ControlEscolar.Entities;

    public class FormularioDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }
        
        public object Obtener(Guid token, string nombreFormulario)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_FORMULARIO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@NOMBRE_FORMULARIO", nombreFormulario));
                connection.Open();

                Formulario objFormulario = new Formulario();
                SqlDataReader objReader = ObjCommand.ExecuteReader();
                
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objFormulario.idFormularioUnique = objReader["idFormularioUnique"].ToString();
                        objFormulario.nombre = objReader["nombre"].ToString();
                        objFormulario.texto = objReader["texto"].ToString();
                    }
                }
                objReader.Close();
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objFormulario;
            }
            catch (Exception e) {
                EscrituraLog.guardar("FormularioDataAccess-Obtener. ", e.Message.ToString());
            }
            finally { connection.Close();  connection.Dispose(); }

            return miObjetoRespuesta;
        }
    }
}
