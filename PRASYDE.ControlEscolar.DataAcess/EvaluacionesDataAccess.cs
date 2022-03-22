//AUTOR: DIEGO OLVERA
//FECHA: 27-06-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA GUARDAR LAS EVALUACIONES 

namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using Framework;
    using System.Net;
    using System.Data;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Data.SqlClient;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.DataAcess.ClasesExcel;

    public class EvaluacionesDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        static SqlTransaction objMyTransaction = null;

        public object Guardar(Evaluaciones propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            connection.Open();
            object miObjetoRespuesta = new object();
            try
            {
                objMyTransaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_EVALUACIONES", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Transaction = objMyTransaction;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@ID_GRUPO_UNIQUE", SqlDbType.VarChar).Value = propiedades.idGrupo;
                ObjCommand.Parameters.Add("@ID_CATEGORIA_EVALUACION", SqlDbType.Int).Value = propiedades.categoriaEvaluacion;
                ObjCommand.Parameters.Add("@ID_ASIGNATURA", SqlDbType.Int).Value = propiedades.idAsignatura;

                ObjCommand.Parameters.Add("@ID_EVALUACION_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.ExecuteNonQuery();

                int idEvaluacion = Convert.ToInt16(ObjCommand.Parameters["@ID_EVALUACION_OUT"].Value);
                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    GuardarDetalle(connection, idEvaluacion, propiedades, ref respuesta);
                    if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK)) { objMyTransaction.Commit(); }
                }

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
                EscrituraLog.guardar("EvaluacionesDataAccess-Guardar. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Editar(EdicionEvaluaciones propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            connection.Open();
            object miObjetoRespuesta = new object();
            int respuesta = 0;

            try
            {
                string cadenaCalificaciones = string.Empty;
                cadenaCalificaciones = ObtenerCadenaCalificaciones(propiedades.calificaciones, ref respuesta);

                SqlCommand ObjCommand = new SqlCommand("USP_EDITAR_EVALUACION", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@ID_GRUPO_UNIQUE", SqlDbType.VarChar).Value = propiedades.idGrupo;
                ObjCommand.Parameters.Add("@ID_ALUMNO", SqlDbType.Int).Value = propiedades.idAlumno;
                ObjCommand.Parameters.Add("@ID_ASIGNATURA", SqlDbType.Int).Value = propiedades.idAsignatura;
                ObjCommand.Parameters.Add("@CALIFICACIONES", SqlDbType.VarChar).Value = cadenaCalificaciones;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;

                ObjCommand.ExecuteNonQuery();
                respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

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
                EscrituraLog.guardar("EvaluacionesDataAccess-Editar. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object EditarEvaluacionFinal(EdicionCalificacionFinal propiedades, Guid token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            connection.Open();
            object miObjetoRespuesta = new object();
            int respuesta = 0;

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_EDITAR_EVALUACIONES_FINALES", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;

                ObjCommand.Parameters.Add("@ID_DETALLE_EVALUACION", SqlDbType.Int).Value = propiedades.idDetalleEvaluacion;
                ObjCommand.Parameters.Add("@TIPO", SqlDbType.Int).Value = propiedades.tipo;
                ObjCommand.Parameters.Add("@CALIFICACION", SqlDbType.Float).Value = propiedades.calificacionFinal;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;

                ObjCommand.ExecuteNonQuery();
                respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

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
                EscrituraLog.guardar("EvaluacionesDataAccess-EditarEvaluacionFinal. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private static string ObtenerCadenaCalificaciones(ArrayList propiedades, ref int respuesta)
        {
            string resultado = string.Empty;
            ArrayList listaCalificaciones = new ArrayList();

            listaCalificaciones = (ArrayList)propiedades;
            listaCalificaciones.Reverse();

            respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            try
            {
                if (propiedades.Count > 0)
                {
                    for (int i = 0; i < propiedades.Count; i++)
                    {
                        string nuevaCalifacacion = listaCalificaciones[i].ToString().Replace(',', '.');
                        resultado = nuevaCalifacacion + ',' + resultado;
                    }
                }
                else { respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.ErrorData); }
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("EvaluacionesDataAccess-ObtenerCadenaCalificaciones. ", e.Message.ToString());
            }

            resultado = resultado.TrimEnd(',');
            return resultado;
        }

        //POR EL MOMENTO ESTE METODO NO SE ESTA UTILIZANDO
        //public object GuardarEvaluacionRetrasada(Evaluaciones propiedades, ref HttpStatusCode Codigo_Respuesta)
        //{
        //    SqlConnection connection = new SqlConnection(this.ConnectionString);
        //    connection.Open();
        //    object miObjetoRespuesta = new object();
        //    try
        //    {
        //        objMyTransaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

        //        SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_EVALUACION_ANTERIOR", connection);
        //        ObjCommand.CommandType = CommandType.StoredProcedure;
        //        ObjCommand.Transaction = objMyTransaction;

        //        ObjCommand.Parameters.Add("@ID_GRUPO_UNIQUE", SqlDbType.VarChar).Value = propiedades.idGrupo;
        //        ObjCommand.Parameters.Add("@ID_CATEGORIA_EVALUACION", SqlDbType.Int).Value = propiedades.categoriaEvaluacion;
        //        ObjCommand.Parameters.Add("@ID_ASIGNATURA", SqlDbType.Int).Value = propiedades.idAsignatura;
        //        ObjCommand.Parameters.Add("@ID_ALUMNO", SqlDbType.Int).Value = propiedades.evaluaciones[0].idAlumno;

        //        ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;

        //        int idEvaluacion = Convert.ToInt32(ObjCommand.ExecuteScalar());
        //        int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

        //        if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
        //        {
        //            GuardarDetalle(connection, idEvaluacion, propiedades, ref respuesta);
        //            if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK)) { objMyTransaction.Commit(); }
        //        }

        //        string textoRespuestaServicio = string.Empty;
        //        int Codigo_Respuesta_servicio = 0;
        //        Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

        //        Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
        //        codigoRespuesta = respuesta;
        //        textoRespuesta = textoRespuestaServicio;
        //        miObjetoRespuesta = new object();

        //        connection.Close();
        //    }
        //    catch (Exception exception) { objMyTransaction.Rollback(); throw exception; }
        //    finally { connection.Dispose(); }

        //    return miObjetoRespuesta;
        //}

        private void GuardarDetalle(SqlConnection connection, int idEvaluacion, Evaluaciones propiedades, ref int respuesta)
        {
            respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            try
            {
                foreach (DetalleEvaluaciones item in propiedades.evaluaciones)
                {
                    SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_DETALLE_EVALUACIONES", connection);
                    ObjCommand.CommandType = CommandType.StoredProcedure;
                    ObjCommand.Transaction = objMyTransaction;

                    ObjCommand.Parameters.Add("@ID_EVALUACION", SqlDbType.Int).Value = idEvaluacion;
                    ObjCommand.Parameters.Add("@ID_ALUMNO", SqlDbType.Int).Value = item.idAlumno;
                    ObjCommand.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = item.tipo;
                    ObjCommand.Parameters.Add("@CALIFICACION", SqlDbType.Decimal).Value = item.calificacion;
                    ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;

                    int filasAfectadas = ObjCommand.ExecuteNonQuery();
                    if (filasAfectadas == 0) { objMyTransaction.Rollback(); respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError); break; }
                    ObjCommand.Dispose();
                }
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("EvaluacionesDataAccess-GuardarDetalle. ", e.Message.ToString());
            }
        }


        #region "METODO PARA EXPORTAR LA INFORMACIÓN A EXCEL"

        public ExcelSheetResponse ExportarCalificacionesExcel(string idGrupo, int nivel)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            var response = new ExcelSheetResponse();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_EXPORTAR_CALIFICACIONES", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                
                ObjCommand.Parameters.Add("@ID_GRUPO", SqlDbType.VarChar).Value = idGrupo;
                ObjCommand.Parameters.Add("@NIVEL", SqlDbType.Int).Value = nivel;
                connection.Open();

                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(ObjCommand);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                response = ExportarCalificaciones.ExportReport(ds);
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("EvaluacionesDataAccess-ExportarCalificacionesExcel. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return response;
        }

        #endregion
    }
}
