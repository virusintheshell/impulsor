
//AUTOR: DIEGO OLVERA
//FECHA:  22-06-2021
//DESCRIPCIÓN: CLASE QUE GESTIONA EL REPORTE GENERAL 

namespace PRASYDE.ControlEscolar.DataAcess.Reportes
{
    using System;
    using System.IO;
    using Framework;
    using System.Web;
    using System.Net;
    using System.Data;
    using Newtonsoft.Json;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;

    public class ReporteGeneralDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        #region "FUNCIONES PARA OBTENER LOS DATOS GENERALES"

        public object ObtenerReportGeneral(Guid token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_REPORTE_GENERAL", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                connection.Open();

                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(ObjCommand);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                int respuesta = 0;
                object objetoFinal = new object();
                objetoFinal = ArmarObjetoGeneral(ds, ref respuesta);

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objetoFinal;
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("ReporteGeneralDataAccess-ObtenerReportGeneral. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private ReporteGeneral ArmarObjetoGeneral(DataSet ds, ref int respuesta)
        {
            ReporteGeneral obj = new ReporteGeneral();
            try
            {
                DataTable dtTotalAlumnos = ds.Tables[0];
                DataTable dtTotalProfesores = ds.Tables[1];
                DataTable dtTotalGrupos = ds.Tables[2];
                DataTable dtTotalPagoMensual = ds.Tables[3];

                obj.totalAlumnos = Convert.ToInt32(dtTotalAlumnos.Rows[0].ItemArray[0]);
                obj.totalProfesores = Convert.ToInt32(dtTotalProfesores.Rows[0].ItemArray[0]);
                obj.totalGrupos = Convert.ToInt32(dtTotalGrupos.Rows[0].ItemArray[0]);
                obj.totalPagoMensual = Convert.ToDouble(dtTotalPagoMensual.Rows[0].ItemArray[0]);
                obj.mesCurso = Convert.ToString(dtTotalPagoMensual.Rows[0].ItemArray[1]);
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("ReporteGeneralDataAccess-ArmarObjetoGeneral. ", e.Message.ToString());
            }
            return obj;
        }

        #endregion

        #region "FUNCIONES PARA OBTENER LOS DATOS PARA LA GRAFICA DE PAGO POR MESES"

        public object ObtenerReportePagosMensuales(Guid token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            List<ReportePagosMensuales> obj = new List<ReportePagosMensuales>();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_REPORTE_PAGOS_MENSUALES", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        obj.Add(new ReportePagosMensuales
                        {
                            totalMonto = Convert.ToDouble(objReader["totalMonto"]),
                            fecha = objReader["fecha"].ToString(),
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
                EscrituraLog.guardar("ReporteGeneralDataAccess-ObtenerReportePagosMensuales. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        #endregion


        #region "FUNCIONES PARA OBTENER LOS DATOS PARA LA GRAFICA DE TOTAL DE INSCRIPCIONES POR MES"

        public object ObtenerReporteInscripciones(Guid token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            List<ReporteInscripcionesMensuales> obj = new List<ReporteInscripcionesMensuales>();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_REPORTE_INSCRIPCIONES_MENSUALES", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        obj.Add(new ReporteInscripcionesMensuales
                        {
                            totalInscripcion = Convert.ToDouble(objReader["totalInscripciones"]),
                            fecha = objReader["fecha"].ToString(),
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
                EscrituraLog.guardar("ReporteGeneralDataAccess-ObtenerReporteInscripciones. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }


        #endregion
    }
}
