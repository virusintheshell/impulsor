
//AUTOR: DIEGO OLVERA
//FECHA: 07-04-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA EL MODULO DE ALUMNOS

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

    public class AlumnosDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        public object Listado(Guid token, int estatus, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_LISTADO_ALUMNOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@ESTATUS", estatus));
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<ListadoAlumnos> objAlumnos = new List<ListadoAlumnos>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objAlumnos.Add(new ListadoAlumnos
                        {
                            idUsuario = objReader["idUsuarioUnique"].ToString(),
                            idAlumno = objReader["idAlumnoUnique"].ToString(),
                            matricula = objReader["matricula"].ToString(),
                            matriculaOficial = objReader["matriculaOficial"].ToString(),
                            nombre = objReader["nombre"].ToString(),
                            correo = objReader["correo"].ToString(),
                            celular = objReader["celular"].ToString(),
                            estatusAlumno = objReader["estatusAlumno"].ToString(),
                            urlImagen = objReader["urlImagen"].ToString(),
                            inscripciones = objReader["inscripciones"].ToString(),
                            estatus = Convert.ToInt16(objReader["estatus"])
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objAlumnos;
            }
            catch (Exception e) {
                EscrituraLog.guardar("AlumnosDataAccess-Listado. Error al obtener el listado de alumnos: ", e.Message.ToString());
            }
            finally { connection.Close();  connection.Dispose(); }
            return miObjetoRespuesta;
        }

        public object ListadoAlumnosGrupo(Guid token, string idGrupo, int tipoConsulta, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_ALUMNOS_GRUPO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@ID_GRUPOUNIQUE", idGrupo));
                ObjCommand.Parameters.Add(new SqlParameter("@TIPO_CONSULTA", tipoConsulta));
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<PropiedadesGrupoAlumnos> objAlumnos = new List<PropiedadesGrupoAlumnos>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objAlumnos.Add(new PropiedadesGrupoAlumnos
                        {
                            idAlumnoUnique = objReader["idAlumnoUnique"].ToString(),
                            idAlumno = Convert.ToInt16(objReader["idAlumno"]),
                            matricula = objReader["matricula"].ToString(),
                            nombre = objReader["nombre"].ToString(),
                            imagen = objReader["imagen"].ToString(),
                            correo = objReader["correo"].ToString(),
                            celular = objReader["celular"].ToString(),
                            estatusAlumno = objReader["estatusAlumno"].ToString(),
                            estatus = Convert.ToInt16(objReader["estatus"])
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK); 
                textoRespuesta = "OK";
                miObjetoRespuesta = objAlumnos;
            }
            catch (Exception e) {
                EscrituraLog.guardar("AlumnosDataAccess-ListadoAlumnosGrupo. Error al obtener el listado de alumnos por grupo: ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }


        #region "CLASES PARA MOSTRAR EL DETALLE DEL ALUMNO"

        public object DetalleInscripcion(Guid token,int plataforma, string idUsuario, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_DETALLE_PROGRAMA_ALUMNO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@TIPO", 2));
                ObjCommand.Parameters.Add(new SqlParameter("@PLATAFORMA", plataforma));
                ObjCommand.Parameters.Add(new SqlParameter("@ID_USUARIO", idUsuario.ToUpper()));
                connection.Open();
                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(ObjCommand);

                DataSet ds = new DataSet();
                adapter.Fill(ds);

                int respuesta = 0;
                object objetoFinal = new object();
                objetoFinal = ArmarObjetoGeneralIncripciones(ds, ref respuesta);

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objetoFinal;
            }

            catch (Exception e) {
                EscrituraLog.guardar("AlumnosDataAccess-DetalleInscripcion. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private List<AlumnoDetalleInscripcion> ArmarObjetoGeneralIncripciones(DataSet ds, ref int respuesta)
        {
            List<AlumnoDetalleInscripcion> obj = new List<AlumnoDetalleInscripcion>();
            try
            {
                DataTable ListaProgramas = ds.Tables[0];
                DataTable ListaDetalle = ds.Tables[1];
                DataTable ListaNiveles = ds.Tables[2];

                foreach (DataRow item in ListaProgramas.Rows)
                {
                    int idPrograma = Convert.ToInt32(item["idPrograma"]);

                    obj.Add(new AlumnoDetalleInscripcion
                    {
                        idProgramaEducativo = item["idProgramaEducativo"].ToString(),
                        nombrePrograma = item["nombrePrograma"].ToString(),
                        urlImagen = item["urlImagen"].ToString(),
                        modalidad = item["modalidad"].ToString(),
                        plantel = item["plantel"].ToString(),
                        dias = item["dias"].ToString(),
                        porcentajeMaterias = Convert.ToDecimal(item["porcentajeMaterias"]),
                        porcentajeAsistencia = Convert.ToDecimal(item["porcentajeAsistencia"]),
                        idGrupo = item["idGrupo"].ToString(),
                        nivelActual =Convert.ToInt16(item["nivelActual"]),
                        calificaciones = ObtenerCalificaciones(idPrograma, ListaDetalle, ListaNiveles, ref respuesta)
                    });
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("AlumnosDataAccess-ArmarObjetoGeneralIncripciones. ", e.Message.ToString());
            }
            return obj;
        }

        private List<CalificacionesPrograma> ObtenerCalificaciones(int idPrograma, DataTable ListaDetalle, DataTable ListaNiveles, ref int respuesta)
        {
            List<CalificacionesPrograma> obj = new List<CalificacionesPrograma>();
            try
            {
                string[] columna = { "nivel", "idProgramaEducativo" };
                DataTable dataTable = General.ObtenerRegistrosDiferentes(ListaDetalle, columna);

                DataRow[] resultado = dataTable.Select("idProgramaEducativo ='" + idPrograma + "'");


                foreach (DataRow item in resultado)
                {
                    ArrayList arrParciales = new ArrayList();

                    DataRow[] resultadoNiveles = ListaNiveles.Select("nivel ='" + Convert.ToInt32(item["nivel"]) + "' AND idProgramaEducativo='" + idPrograma + "'");
                    foreach (DataRow itemA in resultadoNiveles)
                    {
                        arrParciales.Add(Convert.ToString(itemA["categoriaEvaluacion"]));
                    }

                    obj.Add(new CalificacionesPrograma
                    {
                        nivel = Convert.ToInt32(item["nivel"]),
                        asignaturas = ObtenerAsignaturas(Convert.ToInt32(item["nivel"]), idPrograma, ListaDetalle, ref respuesta),
                        parcialesNivel = arrParciales

                    });
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("AlumnosDataAccess-ObtenerCalificaciones. ", e.Message.ToString());
            }
            return obj;
        }

        private List<DetalleAsignaturasParciales> ObtenerAsignaturas(int nivel, int idPrograma, DataTable ListaDetalle, ref int respuesta)
        {
            List<DetalleAsignaturasParciales> obj = new List<DetalleAsignaturasParciales>();
            try
            {
                string[] columna = { "nivel", "idAsignatura", "clave", "nombre","idProgramaEducativo" };
                DataTable dataTable = General.ObtenerRegistrosDiferentes(ListaDetalle, columna);

                DataRow[] resultado = dataTable.Select("nivel ='" + nivel + "' AND idProgramaEducativo='"+ idPrograma + "'");

                string tipoCalif = string.Empty;
                double califFinal = 0.0;

                foreach (DataRow item in resultado)
                {
                    obj.Add(new DetalleAsignaturasParciales
                    {
                        idAsignatura = Convert.ToInt32(item["idAsignatura"]),
                        clave = Convert.ToString(item["clave"]),
                        asignatura = Convert.ToString(item["nombre"]),
                        parciales = ObtenerParciales(Convert.ToInt32(item["nivel"]), Convert.ToInt32(item["idProgramaEducativo"]), Convert.ToInt32(item["idAsignatura"]), ListaDetalle, ref tipoCalif, ref califFinal, ref respuesta),
                        tipoCalif = tipoCalif,
                        califFinal = califFinal
                    });
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("AlumnosDataAccess-ObtenerAsignaturas. ", e.Message.ToString());
            }
            return obj;
        }

        private List<DetalleParciales> ObtenerParciales(int nivel,int idPrograma, int idAsignatura, DataTable ListaDetalle, ref string tipoCalif, ref double califFinal, ref int respuesta)
        {
            List<DetalleParciales> obj = new List<DetalleParciales>();
            try
            {
                DataRow[] resultado = ListaDetalle.Select("nivel ='" + nivel + "' AND idProgramaEducativo = '"+ idPrograma  + "' AND idAsignatura ='" + idAsignatura + "'");

                foreach (DataRow item in resultado)
                {
                    if (Convert.ToInt32(item["idCategoriaEvaluacion"]) == 1)
                    {
                        obj.Add(new DetalleParciales
                        {
                            parcial = Convert.ToString(item["categoriaEvaluacion"]),
                            calif = Convert.ToDouble(item["calificacion"]),
                        });
                    }
                    else {
                        tipoCalif = Convert.ToString(item["categoriaEvaluacion"]);
                        califFinal = Convert.ToDouble(item["calificacion"]);
                    }
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("AlumnosDataAccess-ObtenerParciales. ", e.Message.ToString());
            }
            return obj;
        }
        
        ///-------------------------------------------------------------------------------------------------------//

        public object DetallePagos(Guid token, string idUsuario, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_DETALLE_PAGOS_ALUMNO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@ID_USUARIO", idUsuario.ToUpper()));

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
            catch (Exception e) {
                EscrituraLog.guardar("AlumnosDataAccess-DetallePagos. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private List<ListaPagosAlumno> ArmarObjetoGeneral(DataSet ds, ref int respuesta)
        {
            List<ListaPagosAlumno> objListaPagos = new List<ListaPagosAlumno>();
            try
            {
                DataTable ListaPagos = ds.Tables[0];
                DataTable ListaDetalle = ds.Tables[1];

                foreach (DataRow item in ListaPagos.Rows)
                {
                    objListaPagos.Add(new ListaPagosAlumno
                    {
                        nombrePrograma = Convert.ToString(item["nombrePrograma"]),
                        proximoPago = Convert.ToDecimal(item["proximoPago"]),
                        estatus = Convert.ToString(item["estatus"]),
                        listaPagos = ObtenerDetallePagos(Convert.ToString(item["idProgramaEducativo"]), ListaDetalle, ref respuesta)
                    });
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("AlumnosDataAccess-ArmarObjetoGeneral. ", e.Message.ToString());
            }
            return objListaPagos;
        }

        private List<ListaDetallePagosAlumno> ObtenerDetallePagos(string idProgramaEducativo, DataTable ListaDetalle, ref int respuesta)
        {
            List<ListaDetallePagosAlumno> objDetalleListaPagos = new List<ListaDetallePagosAlumno>();
            try
            {
                DataRow[] resultado = ListaDetalle.Select("idProgramaEducativo ='" + idProgramaEducativo + "'");
                foreach (DataRow item in resultado)
                {
                    objDetalleListaPagos.Add(new ListaDetallePagosAlumno
                    {
                        concepto = Convert.ToString(item["concepto"]),
                        descripcion = Convert.ToString(item["descripcion"]),
                        monto = Convert.ToDecimal(item["monto"]),
                        fecha = Convert.ToString(item["fecha"]),
                    });
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("AlumnosDataAccess-objDetalleListaPagos. ", e.Message.ToString());
            }
            return objDetalleListaPagos;
        }
        #endregion
    }
}
