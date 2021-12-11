//AUTOR: DIEGO OLVERA
//FECHA: 20-07-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA GUARDAR LAS ASISTENCIAS

namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using Framework;
    using System.Net;
    using System.Data;
    using Newtonsoft.Json;
    using System.Data.SqlClient;
    using PRASYDE.ControlEscolar.Entities;

    public class AsistenciaDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        static SqlTransaction objMyTransaction = null;

        public object Guardar(Asistencia propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            connection.Open();
            object miObjetoRespuesta = new object();
            try
            {
                objMyTransaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_ASISTENCIA", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Transaction = objMyTransaction;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@ID_GRUPO_UNIQUE", SqlDbType.VarChar).Value = propiedades.idGrupo;
                ObjCommand.Parameters.Add("@ID_ASIGNATURA", SqlDbType.Int).Value = propiedades.idAsignatura;
                ObjCommand.Parameters.Add("@FECCHA_ASISTENCIA", SqlDbType.VarChar).Value = propiedades.fechaAsistencia;
                
                ObjCommand.Parameters.Add("@ID_ASISTENCIA_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                
                ObjCommand.ExecuteNonQuery();

                int idAsisntencia = Convert.ToInt16(ObjCommand.Parameters["@ID_ASISTENCIA_OUT"].Value);
                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    GuardarDetalle(connection, idAsisntencia, propiedades, ref respuesta);
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
            catch (Exception e) {
                objMyTransaction.Rollback();
                EscrituraLog.guardar("AsistenciaDataAccess-Guardar. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private void GuardarDetalle(SqlConnection connection, int idAsisteincia, Asistencia propiedades, ref int respuesta)
        {
            respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            try
            {
                foreach (DetalleAsistencia item in propiedades.asistencias)
                {
                    SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_DETALLE_ASISTENCIA", connection);
                    ObjCommand.CommandType = CommandType.StoredProcedure;
                    ObjCommand.Transaction = objMyTransaction;

                    ObjCommand.Parameters.Add("@ID_ASISTENCIA", SqlDbType.Int).Value = idAsisteincia;
                    ObjCommand.Parameters.Add("@ID_ALUMNO", SqlDbType.Int).Value = item.idAlumno;
                    ObjCommand.Parameters.Add("@ASISTENCIA", SqlDbType.Decimal).Value = item.asistencia;
                    ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;

                    int filasAfectadas = ObjCommand.ExecuteNonQuery();
                    if (filasAfectadas == 0) { objMyTransaction.Rollback(); respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError); break; }
                    ObjCommand.Dispose();
                }
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("AsistenciaDataAccess-GuardarDetalle. ", e.Message.ToString());
            }
        }
    }
}
