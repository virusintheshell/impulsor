
//AUTOR: DIEGO OLVERA
//FECHA: 03-03-2022
//DESCRIPCIÓN: CLASE QUE GESTIONA LOS METOOOS PARA CREAR EL CALENDARAIO DE PAGOS

namespace PRASYDE.ControlEscolar.DataAcess.Finanzas
{
    using System;
    using Framework;
    using System.Net;
    using System.Web;
    using System.Data;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Entities.Finanzas;

    public class CalendariosDePagosDataAccess: BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }


        public object ListadoCalendario(Guid token, CalendarioDePagos propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_CALENDARIO_PAGOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@FECHA_INICIAL", propiedades.fechaInicial));
                ObjCommand.Parameters.Add(new SqlParameter("@PERIODO_DURACION", propiedades.periodoDuracion));
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<ListadoCalendarioPagos> obj = new List<ListadoCalendarioPagos>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        obj.Add(new ListadoCalendarioPagos
                        {
                            fecha = objReader["fecha"].ToString(),
                            concepto = objReader["concepto"].ToString(), 
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = obj;
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("CalendariosDePagosDataAccess-ListadoCalendario. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }
    }
}
