
//AUTOR: DIEGO OLVERA
//FECHA: 27-08-2019
//DESCRIPCIÓN: CLASE QUE GESTIONA EL CRUD DEL MODULO DE TAREAS

namespace PRASYDE.ControlEscolar.DataAcess
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

    public class TareasDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        public object Guardar(Tareas propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_TAREAS_PROFESOR", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@IDGRUPO_UNIQUE", SqlDbType.VarChar).Value = propiedades.idGrupo;
                ObjCommand.Parameters.Add("@ID_ASIGNATURA", SqlDbType.Int).Value = propiedades.idAsignatura;
                ObjCommand.Parameters.Add("@TIPO", SqlDbType.Int).Value = propiedades.tipo;
                ObjCommand.Parameters.Add("@TITULO", SqlDbType.VarChar).Value = propiedades.titulo.Trim();
                ObjCommand.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = propiedades.descripcion.Trim();
                ObjCommand.Parameters.Add("@FECHA_ENTREGA", SqlDbType.VarChar).Value = propiedades.fechaEntrega.Trim();

                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_TAREA_OUT", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_EMPRESA_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;

                connection.Open();
                ObjCommand.ExecuteNonQuery();

                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);
                int idTareaOut = Convert.ToInt16(ObjCommand.Parameters["@ID_TAREA_OUT"].Value);
                int idEmpresaOut = Convert.ToInt16(ObjCommand.Parameters["@ID_EMPRESA_OUT"].Value);
                int tipoEvidencia = 2; //TAREAS PROFESOR

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK) && propiedades.archivo.Count > 0)
                {
                    for (int i = 0; i < propiedades.archivo.Count; i++)
                    {
                        string extension = System.IO.Path.GetExtension(propiedades.archivo[i].FileName);
                        string nombreArchivo = string.Concat(idTareaOut, extension);

                        MemoryStream fileStream = General.getStreamFromFile(propiedades.archivo[i]);
                        string filePath = HttpContext.Current.Server.MapPath(string.Concat("~/ExpendienteDigital/", idEmpresaOut, "/", tipoEvidencia, "/", nombreArchivo));
                        bool saveFile = General.saveAttachment(fileStream, filePath);

                        if (saveFile)
                        {
                            string rutaBaseDatos = string.Empty;
                            rutaBaseDatos = string.Concat("/ExpendienteDigital/", idEmpresaOut, "/", tipoEvidencia, "/", nombreArchivo);
                            UrlImagen.Actualizar(connection, "asignarTareas", idTareaOut.ToString(), rutaBaseDatos);
                        }
                    }
                }

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = idTareaOut;
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("TareasDataAccess-Guardar. " + "idUsuario: " + token + " idGrupo: " + propiedades.idGrupo + " idAsignatura: " + propiedades.idAsignatura, e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object ObtenerTareas(Guid token, int tipo, int plataforma, int estatus, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_TAREAS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@TIPO", tipo));
                ObjCommand.Parameters.Add(new SqlParameter("@PLATAFORMA", plataforma));
                ObjCommand.Parameters.Add(new SqlParameter("@ESTATUS", estatus));

                connection.Open();
                List<ListaTareas> tareas = new List<ListaTareas>();
                SqlDataReader objReader = ObjCommand.ExecuteReader();

                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        tareas.Add(new ListaTareas
                        {
                            idTareaProfesor = objReader["idTarea"].ToString(),
                            titulo = objReader["titulo"].ToString(),
                            descripcion = objReader["descripcion"].ToString(),
                            urlDocumento = objReader["urlDocumento"].ToString(),
                            fechaEntrega = objReader["fechaEntrega"].ToString(),
                            idGrupo = Convert.ToInt32(objReader["idGrupo"]),
                            idGrupoUnique = objReader["idGrupoUnique"].ToString(),
                            nombreGrupo = objReader["nombreGrupo"].ToString(),
                            idAsignatura = Convert.ToInt32(objReader["idAsignatura"]),
                            nombreAsignatura = objReader["nombreAsignatura"].ToString(),
                            tipo = Convert.ToInt32(objReader["tipo"]),
                            tipoTarea = objReader["tipoTarea"].ToString(),
                            fechaHoraRegistro = objReader["fechaHoraRegistro"].ToString(),
                            nombre = objReader["nombre"].ToString(),
                            tareaContestada = Convert.ToInt32(objReader["tareaContestada"]),
                            comentarioProfesor = objReader["comentarioProfesor"].ToString(),
                        });
                    }
                }
                objReader.Close();

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt32(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = tareas;
            }
            catch (Exception e)
            {
                codigoRespuesta = Convert.ToInt32(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("TareasDataAccess-ObtenerTareas. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object EditarTareaProfesor(Tareas propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_EDITAR_TAREAS_PROFESOR", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@ID_TAREA_UNIQUE", SqlDbType.VarChar).Value = propiedades.idTarea;
                ObjCommand.Parameters.Add("@IDGRUPO_UNIQUE", SqlDbType.VarChar).Value = propiedades.idGrupo;
                ObjCommand.Parameters.Add("@ID_ASIGNATURA", SqlDbType.Int).Value = propiedades.idAsignatura;
                ObjCommand.Parameters.Add("@TIPO", SqlDbType.Int).Value = propiedades.tipo;
                ObjCommand.Parameters.Add("@TITULO", SqlDbType.VarChar).Value = propiedades.titulo.Trim();
                ObjCommand.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = propiedades.descripcion.Trim();
                ObjCommand.Parameters.Add("@FECHA_ENTREGA", SqlDbType.VarChar).Value = propiedades.fechaEntrega.Trim();

                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_TAREA_OUT", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_EMPRESA_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                connection.Open();
                ObjCommand.ExecuteNonQuery();

                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);
                int idTareaOut = Convert.ToInt16(ObjCommand.Parameters["@ID_TAREA_OUT"].Value);
                int idEmpresaOut = Convert.ToInt16(ObjCommand.Parameters["@ID_EMPRESA_OUT"].Value);

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                string rutaArchivo = string.Empty;

                string nombrArchivoGeneral = idTareaOut.ToString();

                string nombreArchivoWord = string.Concat(nombrArchivoGeneral, ".docx");
                string nombreArchivoPDF = string.Concat(nombrArchivoGeneral, ".pdf");
                int tipoEvidencia = 2; //TAREAS

                if (propiedades.archivo.Count > 0)
                {
                    for (int i = 0; i < propiedades.archivo.Count; i++)
                    {
                        rutaArchivo = HttpContext.Current.Server.MapPath(string.Concat("~/ExpendienteDigital/", idEmpresaOut, "/", tipoEvidencia, "/", nombreArchivoWord));
                        EliminarDocumento(rutaArchivo, ref Codigo_Respuesta_servicio);

                        if (Codigo_Respuesta_servicio == 0)
                        {
                            rutaArchivo = HttpContext.Current.Server.MapPath(string.Concat("~/ExpendienteDigital/", idEmpresaOut, "/", tipoEvidencia, "/", nombreArchivoPDF));
                            EliminarDocumento(rutaArchivo, ref Codigo_Respuesta_servicio);

                        }
                        UrlImagen.Actualizar(connection, "asignarTareas", Convert.ToString(nombrArchivoGeneral), "");

                        string extension = System.IO.Path.GetExtension(propiedades.archivo[0].FileName);
                        string nombreArchivo = string.Concat(idTareaOut, extension);

                        MemoryStream fileStream = General.getStreamFromFile(propiedades.archivo[i]);
                        string filePath = HttpContext.Current.Server.MapPath(string.Concat("~/ExpendienteDigital/", idEmpresaOut, "/", tipoEvidencia, "/", nombreArchivo));
                        bool saveFile = General.saveAttachment(fileStream, filePath);

                        if (saveFile)
                        {
                            string rutaBaseDatos = string.Empty;
                            rutaBaseDatos = string.Concat("/ExpendienteDigital/", idEmpresaOut, "/", tipoEvidencia, "/", nombreArchivo);
                            UrlImagen.Actualizar(connection, "asignarTareas", idTareaOut.ToString(), rutaBaseDatos);
                        }
                    }
                }
                if (propiedades.contieneDocumento == 0)
                {
                    rutaArchivo = HttpContext.Current.Server.MapPath(string.Concat("~/ExpendienteDigital/", idEmpresaOut, "/", tipoEvidencia, "/", nombreArchivoWord));
                    EliminarDocumento(rutaArchivo, ref Codigo_Respuesta_servicio);

                    if (Codigo_Respuesta_servicio == 0)
                    {
                        rutaArchivo = HttpContext.Current.Server.MapPath(string.Concat("~/ExpendienteDigital/", idEmpresaOut, "/", tipoEvidencia, "/", nombreArchivoPDF));
                        EliminarDocumento(rutaArchivo, ref Codigo_Respuesta_servicio);

                    }
                    UrlImagen.Actualizar(connection, "asignarTareas", Convert.ToString(nombrArchivoGeneral), "");
                }

                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = new object();
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("TareasDataAccess-EditarTareaProfesor. ", e.Message.ToString());
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return miObjetoRespuesta;
        }

        public static void EliminarDocumento(string rutaArchivo, ref int respuesta)
        {
            try
            {
                if (File.Exists(rutaArchivo))
                {
                    File.Delete(rutaArchivo);
                    respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                }
                else
                {
                    respuesta = Convert.ToInt16(0);
                }
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("TareasDataAccess-EliminarDocumento. ", e.Message.ToString());
            }
        }

        public object GuardarTareasAlumno(TareasAlumno propiedades, Guid token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            string filePath = String.Empty;
            int respuesta = 0;
            int idEmpresaOut = 0;
            int idTareaOut = 0;
            
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_TAREA_ALUMNO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;

                ObjCommand.Parameters.Add("@ID_TAREA_UNIQUE", SqlDbType.VarChar).Value = propiedades.idTarea;
                ObjCommand.Parameters.Add("@COMENTARIOS", SqlDbType.VarChar).Value = propiedades.comentarios.Trim();

                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_EMPRESA_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_DETALLE_TAREA", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                connection.Open();
                ObjCommand.ExecuteNonQuery();

                respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);
                idEmpresaOut = Convert.ToInt16(ObjCommand.Parameters["@ID_EMPRESA_OUT"].Value);
                idTareaOut = Convert.ToInt32(ObjCommand.Parameters["@ID_DETALLE_TAREA"].Value);

                EscrituraLog.guardar("RESPUESTAS.-  ID_TAREA_UNIQUE: " + propiedades.idTarea + " RESPUESTA: " + respuesta.ToString() + " ID_DETALLE_TAREA:  " + idTareaOut.ToString(), " FECHA:" + DateTime.Now);

                int tipoEvidencia = 3; //TAREAS ALUMNO

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK) && propiedades.archivo.Count > 0)
                {
                    for (int i = 0; i < propiedades.archivo.Count; i++)
                    {
                        string extension = System.IO.Path.GetExtension(propiedades.archivo[i].FileName);
                        string nombreArchivo = string.Concat(idTareaOut, extension);

                        MemoryStream fileStream = General.getStreamFromFile(propiedades.archivo[i]);
                        filePath = HttpContext.Current.Server.MapPath(string.Concat("~/ExpendienteDigital/", idEmpresaOut, "/", tipoEvidencia, "/", nombreArchivo));
                        bool saveFile = General.saveAttachment(fileStream, filePath);
                        
                        if (saveFile)
                        {
                            string rutaBaseDatos = string.Empty;
                            rutaBaseDatos = string.Concat("/ExpendienteDigital/", idEmpresaOut, "/", tipoEvidencia, "/", nombreArchivo);
                            UrlImagen.Actualizar(connection, "guardarTareasAlumnos", idTareaOut.ToString(), rutaBaseDatos);
                        }
                    }
                }

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = idTareaOut;
            }
            catch (Exception e)
            {
               EscrituraLog.guardar("RESPUESTA_ARCHIVOS.-  ID_TAREA_UNIQUE: " + propiedades.idTarea + " RESPUESTA: " + respuesta.ToString() + " ID_DETALLE_TAREA:  " + idTareaOut.ToString() + "FILEPATH: " + filePath , " FECHA:" + DateTime.Now);
               EscrituraLog.guardar("TareasDataAccess-GuardarTareasAlumno. idTarea: " + propiedades.idTarea + " idUsuario: " + token, e.Message.ToString());
               EscrituraLog.guardar("ERRORES.-  ID_TAREA_UNIQUE: " + propiedades.idTarea + " RESPUESTA: " + respuesta.ToString() + " ID_DETALLE_TAREA:  " + idTareaOut.ToString(), " FECHA:" + DateTime.Now);
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return miObjetoRespuesta;
        }

        public object ObtenerRevisionTareas(Guid token, TareasFiltroRevision propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_TAREAS_REVISION", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@ID_GRUPO_UNIQUE", SqlDbType.VarChar).Value = propiedades.idGrupo;
                ObjCommand.Parameters.Add("@ID_ALUMNO", SqlDbType.Int).Value = propiedades.idAlumno;

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
                EscrituraLog.guardar("TareasDataAccess-ObtenerRevisionTareas. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private List<ListaTareasRevision> ArmarObjetoGeneral(DataSet ds, ref int respuesta)
        {
            List<ListaTareasRevision> objAsignaturas = new List<ListaTareasRevision>();
            try
            {
                DataTable datosGenerales = ds.Tables[0];

                string[] columna = { "idAsignatura", "nombreAsignatura" };
                DataTable dataTable = RegistrosDiferentes.Obtener(datosGenerales, columna);

                foreach (DataRow item in dataTable.Rows)
                {
                    objAsignaturas.Add(new ListaTareasRevision
                    {
                        Asignatura = Convert.ToString(item["nombreAsignatura"]),
                        tareas = ObtenerListaTareas(Convert.ToInt32(item["idAsignatura"]), datosGenerales)

                    });
                }
                respuesta = Convert.ToInt32(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt32(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("TareasDataAccess-ArmarObjetoGeneral. ", e.Message.ToString());
            }
            return objAsignaturas;
        }

        private List<TareasAsignadas> ObtenerListaTareas(int idAsignatura, DataTable datosGenerales)
        {
            List<TareasAsignadas> objLista = new List<TareasAsignadas>();
            try
            {
                DataRow[] resultado = datosGenerales.Select("idAsignatura ='" + idAsignatura + "'");

                foreach (DataRow item in resultado)
                {
                    objLista.Add(new TareasAsignadas
                    {
                        tituloTarea = Convert.ToString(item["tituloTarea"]),
                        observaciones = Convert.ToString(item["descripcion"]),
                        fechaSolicitada = Convert.ToString(item["fechaSolicitada"]),
                        tareaEntregada = ObtenerTareaEntregada(Convert.ToInt32(item["idAsignatura"]), Convert.ToInt32(item["idTarea"]), datosGenerales)
                    });
                }
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("TareasDataAccess-ObtenerListaTareas. ", e.Message.ToString());
            }
            return objLista;
        }

        private TareasEntregadas ObtenerTareaEntregada(int idAsignatura, int idTarea, DataTable datosGenerales)
        {
            TareasEntregadas objTarea = new TareasEntregadas();
            try
            {
                DataRow[] resultado = datosGenerales.Select("idAsignatura ='" + idAsignatura + "' AND idTarea = '" + idTarea + "'");

                foreach (DataRow item in resultado)
                {
                    objTarea.idDetalleTarea = Convert.ToInt32(item["idDetalleTarea"]);
                    objTarea.urlDocumento = Convert.ToString(item["urlDocumento"]);
                    objTarea.comentarios = Convert.ToString(item["comentarios"]);
                    objTarea.fechaEntrega = Convert.ToString(item["fechaEntregada"]);
                    objTarea.comentarioProfesor = Convert.ToString(item["comentario"]);
                }
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("TareasDataAccess-ObtenerTareaEntregada. ", e.Message.ToString());
            }
            return objTarea;
        }


        public object GuardrComentariosTareasProfesor(ObjetoComentariosProfesor propiedades, Guid token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_TAREAS_PROFESOR_COMENTARIOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;

                ObjCommand.Parameters.Add("@ID_DETALLE_TAREA", SqlDbType.Int).Value = propiedades.idDetalleTarea;
                ObjCommand.Parameters.Add("@COMENTARIO", SqlDbType.VarChar).Value = propiedades.comentarioTarea;

                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;

                connection.Open();
                ObjCommand.ExecuteNonQuery();
                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

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
                EscrituraLog.guardar("TareasDataAccess-GuardrComentariosTareasProfesor. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

    }
}
