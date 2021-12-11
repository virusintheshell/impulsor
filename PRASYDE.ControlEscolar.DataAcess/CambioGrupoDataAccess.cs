
//AUTOR: DIEGO OLVERA
//FECHA: 07-05-2020
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA EL CAMBIO DE GRUPOS

namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using Framework;
    using System.Net;
    using System.Data;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;

    public class CambioGrupoDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        static SqlTransaction objMyTransaction = null;

        #region "METODOS PARA ARMAR EL FORMULARIO PARA REALIAR EL CAMBIO DE GRUPOS"

        public object ObtenerGruposDisponibles(Guid token, int idPrograma, string idAlumno)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_GRUPOS_DISPONIBLES", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@ID_PROGRAMA", idPrograma));
                ObjCommand.Parameters.Add(new SqlParameter("@ID_ALUMNO", idAlumno));
                connection.Open();
                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(ObjCommand);

                DataSet ds = new DataSet();
                adapter.Fill(ds);

                int respuesta = 0;
                object objetoFinal = new object();
                objetoFinal = ArmarObjetoGeneral(ds, ref respuesta);


                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objetoFinal;

            }
            catch (Exception e) {
                EscrituraLog.guardar("CambioGrupoDataAccess-ObtenerGruposDisponibles. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private DatosCambioGrupo ArmarObjetoGeneral(DataSet ds, ref int respuesta)
        {
            DatosCambioGrupo obj = new DatosCambioGrupo();
            try
            {
                DataTable ListaGrupos = ds.Tables[0];

                string idGrupoInscrito = string.Empty;
                obj.idProgramaEducativo = ListaGrupos.Rows[0].ItemArray[0].ToString();
                obj.nombrePrograma = ListaGrupos.Rows[0].ItemArray[1].ToString();

                obj.listaGrupos = ObtenerListadoGrupos(ListaGrupos, ref idGrupoInscrito, ref respuesta);
                obj.idGrupo = idGrupoInscrito;

                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("CambioGrupoDataAccess-ArmarObjetoGeneral. ", e.Message.ToString());
            }
            return obj;
        }

        private List<ListaGruposDisponibles> ObtenerListadoGrupos(DataTable datatable, ref string idGrupoInscrito, ref int respuesta)
        {
            List<ListaGruposDisponibles> obj = new List<ListaGruposDisponibles>();
            try
            {
                foreach (DataRow item in datatable.Rows)
                {
                    if (Convert.ToInt16(item["inscrito"]) == 1)
                    {
                        idGrupoInscrito = item["idGrupoUnique"].ToString();
                    }

                    obj.Add(new ListaGruposDisponibles
                    {
                        idGrupo = item["idGrupoUnique"].ToString(),
                        nombreGrupo = item["nombreGrupo"].ToString(),
                        nivelActual = Convert.ToInt16(item["nivel"]),
                        idPlanEducativo = item["idPlanUnique"].ToString(),
                        nombrePlan = item["planEducativo"].ToString(),
                        cupo = Convert.ToInt16(item["cupo"])
                    });
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("CambioGrupoDataAccess-ObtenerListadoGrupos. ", e.Message.ToString());
            }
            return obj;
        }

        #endregion

        #region "METODOS PARA EL CAMBIO DE GRUPO"

        public object ObtenerAsignaturasCambio(CambioGrupo propiedades, Guid token)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_ASIGNATURAS_A_SETEAR", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));

                ObjCommand.Parameters.Add("@ID_ALUMNO", SqlDbType.VarChar).Value = propiedades.idAlumno;
                ObjCommand.Parameters.Add("@ID_PROGRAMA", SqlDbType.VarChar).Value = propiedades.idPrograma;
                ObjCommand.Parameters.Add("@ID_GRUPO", SqlDbType.VarChar).Value = propiedades.idGrupo;
                connection.Open();

                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(ObjCommand);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                int respuesta = 0;
                object objetoFinal = new object();
                objetoFinal = ArmarObjeto(ds, ref respuesta);

                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objetoFinal;
            }
            catch (Exception e) {
                EscrituraLog.guardar("CambioGrupoDataAccess-ObtenerAsignaturasCambio. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private ListasAsignaturasCambioGrupo ArmarObjeto(DataSet ds, ref int respuesta)
        {
            ListasAsignaturasCambioGrupo obj = new ListasAsignaturasCambioGrupo();

            try
            {
                int esGrupoOrigen = 1;
                obj.origin = ArmarLista(ds, esGrupoOrigen, ref respuesta);
                esGrupoOrigen = 0;
                obj.destiny = ArmarLista(ds, esGrupoOrigen, ref respuesta);

                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("CambioGrupoDataAccess-ArmarObjeto. ", e.Message.ToString());
            }
            return obj;
        }

        private MateriasOrigenDestino ArmarLista(DataSet ds, int esGrupoOrigen, ref int respuesta)
        {
            MateriasOrigenDestino obj = new MateriasOrigenDestino();
            try
            {
                List<AsigunaturasListaCambioGrupo> objLista = new List<AsigunaturasListaCambioGrupo>();

                DataTable dataTable = ds.Tables[0];
                DataRow[] resultado = dataTable.Select("esGrupoOrigen ='" + esGrupoOrigen + "'");

                foreach (DataRow item in resultado)
                {
                    obj.idGrupo = Convert.ToInt16(item["idGrupo"]);

                    objLista.Add(new AsigunaturasListaCambioGrupo
                    {
                        idMateria = Convert.ToString(item["idAsignaturaUnique"]),
                        materia = Convert.ToString(item["nombreMateria"]),
                        calificacion = Convert.ToDecimal(item["calificacion"]),
                        status = "Pendiente",
                    });
                }
                obj.materiasLists = objLista;
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("CambioGrupoDataAccess-ArmarLista. ", e.Message.ToString());
            }
            return obj;
        }

        public object GuardarCambioGrupo(CambioGrupo propiedades, Guid token)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();

            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_CAMBIO_GRUPO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = "";

                ObjCommand.Parameters.Add("@ID_ALUMNO", SqlDbType.VarChar).Value = propiedades.idAlumno;
                ObjCommand.Parameters.Add("@ID_PROGRAMA", SqlDbType.VarChar).Value = propiedades.idPrograma;
                ObjCommand.Parameters.Add("@ID_GRUPO", SqlDbType.VarChar).Value = propiedades.idGrupo;

                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.ExecuteNonQuery();

                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = new object();

            }
            catch (Exception e) {
                EscrituraLog.guardar("CambioGrupoDataAccess-GuardarCambioGrupo. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            GuardarSeteoAsignaturas(propiedades, token);

            return miObjetoRespuesta;
        }

        //**********************************************************************************************************

        public object GuardarSeteoAsignaturas(CambioGrupo propiedades, Guid token)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            int respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);

            object miObjetoRespuesta = new object();
            try
            {
                objMyTransaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                respuesta = GuardarSeteoAsignaturasOrigen(connection, objMyTransaction, propiedades, token);

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    respuesta = GuardarSeteoAsignaturasDestino(connection, objMyTransaction, propiedades, token);

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK)) { objMyTransaction.Commit(); }
            }
            catch (Exception e) {
                objMyTransaction.Rollback();
                EscrituraLog.guardar("CambioGrupoDataAccess-GuardarSeteoAsignaturas. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public int GuardarSeteoAsignaturasOrigen(SqlConnection connection, SqlTransaction objMyTransaction, CambioGrupo propiedades, Guid token)
        {
            int respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            object miObjetoRespuesta = new object();
            try
            {
                int idGrupo = propiedades.origin.idGrupo;
                bool continuarProceso = true;
                foreach (AsigunaturasListaCambioGrupo item in propiedades.origin.materiasLists)
                {
                    if (continuarProceso == false) { break; }
                    string idMateria = item.idMateria.ToString();
                    Double calificaicon = Convert.ToDouble(item.calificacion);

                    SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_SETEO_EVALUACIONES_ORIGEN", connection);
                    ObjCommand.CommandType = CommandType.StoredProcedure;
                    ObjCommand.Transaction = objMyTransaction;
                    ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                    ObjCommand.Parameters.Add("@ALUMNO", SqlDbType.VarChar).Value = propiedades.idAlumno;
                    ObjCommand.Parameters.Add("@ASIGNATURA", SqlDbType.VarChar).Value = idMateria;
                    ObjCommand.Parameters.Add("@CALIFICACION", SqlDbType.Float).Value = calificaicon;
                    ObjCommand.Parameters.Add("@ID_GRUPO", SqlDbType.VarChar).Value = idGrupo;
                    ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;

                    ObjCommand.ExecuteNonQuery();
                    respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

                    if (respuesta != Convert.ToInt16(Enumerados.Codigos_Respuesta.OK)) { continuarProceso = false; }
                }
            }
            catch (Exception e) {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("CambioGrupoDataAccess-GuardarSeteoAsignaturasOrigen. ", e.Message.ToString());
            }
            return respuesta;
        }

        public int GuardarSeteoAsignaturasDestino(SqlConnection connection, SqlTransaction objMyTransaction, CambioGrupo propiedades, Guid token)
        {
            int respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            object miObjetoRespuesta = new object();

            try
            {
                int idGrupo = propiedades.destiny.idGrupo;
                bool continuarProceso = true;
                foreach (AsigunaturasListaCambioGrupo item in propiedades.destiny.materiasLists)
                {

                    if (continuarProceso == false) { break; }
                    string idMateria = item.idMateria.ToString();
                    Double calificaicon = Convert.ToDouble(item.calificacion);

                    SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_SETEO_EVALUACIONES_DESTINO", connection);
                    ObjCommand.CommandType = CommandType.StoredProcedure;
                    ObjCommand.Transaction = objMyTransaction;
                    ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;

                    ObjCommand.Parameters.Add("@ALUMNO", SqlDbType.VarChar).Value = propiedades.idAlumno;
                    ObjCommand.Parameters.Add("@ASIGNATURAS", SqlDbType.VarChar).Value = idMateria;
                    ObjCommand.Parameters.Add("@CALIFICACION_ASING", SqlDbType.Float).Value = calificaicon;
                    ObjCommand.Parameters.Add("@ID_GRUPO", SqlDbType.VarChar).Value = idGrupo;
                    ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;

                    ObjCommand.ExecuteNonQuery();
                    respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

                    if (respuesta != Convert.ToInt16(Enumerados.Codigos_Respuesta.OK)) { continuarProceso = false; }
                }
            }
            catch (Exception e) {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("CambioGrupoDataAccess-GuardarSeteoAsignaturasDestino. ", e.Message.ToString());
            }
            return respuesta;
        }

        private int ObtenerCadenaIDs(CambioGrupo propiedades, int tipoObjeto, ref string asignaturas, ref string calificaciones, ref int idGrupo)
        {
            int resultado = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            ArrayList arrAsignaturas = new ArrayList();
            ArrayList arrCalificaciones = new ArrayList();

            try
            {
                MateriasOrigenDestino objMaterias = new MateriasOrigenDestino();

                objMaterias = tipoObjeto == 0 ? propiedades.origin : propiedades.destiny;
                idGrupo = objMaterias.idGrupo;

                double calificacion = 0;

                foreach (AsigunaturasListaCambioGrupo item in objMaterias.materiasLists)
                {
                    asignaturas = asignaturas + ',' + item.idMateria.ToString();
                    calificacion = Convert.ToDouble(item.calificacion);
                    calificaciones = calificaciones + ',' + calificacion.ToString();
                }

                asignaturas = asignaturas.Remove(0, 1);
                calificaciones = calificaciones.Remove(0, 1);

            }
            catch (Exception e) {
                resultado = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("CambioGrupoDataAccess-ObtenerCadenaIDs. ", e.Message.ToString());
            }
            return resultado;
        }

        #endregion

        #region "METODOS PARA LA ACTUALIZACION DE CALIFICACIONES HISTORICAS"

        public object CalificacionesHistoricas(AsignaturasAlumnoHistorico propiedades, Guid token)
        {
            object miObjetoRespuesta = new object();
            int respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);

            SqlConnection connection = new SqlConnection(this.ConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                objMyTransaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                int idEvaluacion = 0;
                respuesta = GuardarCalificacionesHistoricas(connection, objMyTransaction, propiedades, token, ref idEvaluacion);

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = new object();

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    respuesta = GuardarDetalleCalificacionesHistoricas(connection, objMyTransaction, propiedades, idEvaluacion);

                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = new object();

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK)) { objMyTransaction.Commit(); }
            }
            catch (Exception e) {
                objMyTransaction.Rollback();
                EscrituraLog.guardar("CambioGrupoDataAccess-CalificacionesHistoricas. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); objMyTransaction.Dispose(); }

            return miObjetoRespuesta;
        }

        public int GuardarCalificacionesHistoricas(SqlConnection connection, SqlTransaction objMyTransaction, AsignaturasAlumnoHistorico propiedades, Guid token, ref int idEvaluacion)
        {
            int respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);

            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_EVALUACIONES_HISTORICAS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Transaction = objMyTransaction;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@ID_GRUPO", SqlDbType.VarChar).Value = propiedades.idGrupo;
                ObjCommand.Parameters.Add("@NIVEL", SqlDbType.Int).Value = propiedades.nivel;
                ObjCommand.Parameters.Add("@ID_ASIGNATURA", SqlDbType.Int).Value = propiedades.idAsignatura;

                ObjCommand.Parameters.Add("@ID_EVALUACION", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.ExecuteNonQuery();

                respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);
                idEvaluacion = Convert.ToInt32(ObjCommand.Parameters["@ID_EVALUACION"].Value);
            }
            catch (Exception e) {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("CambioGrupoDataAccess-GuardarCalificacionesHistoricas. ", e.Message.ToString());
            }
            return respuesta;
        }

        public int GuardarDetalleCalificacionesHistoricas(SqlConnection connection, SqlTransaction objMyTransaction, AsignaturasAlumnoHistorico propiedades, int idEvaluacion)
        {
            int respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            bool continuarProceso = true;

            if (connection.State != ConnectionState.Open)
                connection.Open();
            try
            {
                foreach (CalificacionesAlumnoHistorico item in propiedades.alumnos)
                {
                    if (continuarProceso == false) { break; }

                    SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_DETALLE_EVALUACIONES_HISTORICAS", connection);
                    ObjCommand.CommandType = CommandType.StoredProcedure;
                    ObjCommand.Transaction = objMyTransaction;

                    ObjCommand.Parameters.Add("@ID_EVALUACION", SqlDbType.Int).Value = idEvaluacion;
                    ObjCommand.Parameters.Add("@ID_ASIGNATURA", SqlDbType.Int).Value = propiedades.idAsignatura;

                    ObjCommand.Parameters.Add("@ID_ALUMNO", SqlDbType.Int).Value = item.idAlumno;
                    ObjCommand.Parameters.Add("@CALIFICACION", SqlDbType.Float).Value = item.calificacion;
                    ObjCommand.Parameters.Add("@TIPO", SqlDbType.Int).Value = item.tipo;

                    ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                    ObjCommand.ExecuteNonQuery();
                    respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

                    if (respuesta != Convert.ToInt16(Enumerados.Codigos_Respuesta.OK)) { continuarProceso = false; }
                }
            }
            catch (Exception e) {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("CambioGrupoDataAccess-GuardarDetalleCalificacionesHistoricas. ", e.Message.ToString());
            }
            return respuesta;
        }

        public object EliminarCalificacionesHistoricas(PropiedadesCalificacionesHistoricas propiedades, Guid token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();

            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_ELIMINAR_EVALUACIONES_HISTORICAS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@ID_GRUPO", SqlDbType.VarChar).Value = propiedades.idGrupo;
                ObjCommand.Parameters.Add("@NIVEL", SqlDbType.Int).Value = propiedades.nivel;
                ObjCommand.Parameters.Add("@ID_ASIGNATURA", SqlDbType.Int).Value = propiedades.idAsignatura;

                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
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
            catch (Exception e) {
                EscrituraLog.guardar("CambioGrupoDataAccess-EliminarCalificacionesHistoricas. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }
      
        #endregion
    }
}
