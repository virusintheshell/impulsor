
//AUTOR: DIEGO OLVERA
//FECHA: 08-04-2019
//DESCRIPCIÓN: CLASE QUE CONTROLA LOS METODOS PARA GUARDAR, EDITAR, CONSULTAR, LISTADO DE LOS PROGRAMAS EDUCATIVOS

namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using Framework;
    using System.Web;
    using System.Net;
    using System.Data;
    using Newtonsoft.Json;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;

    public class ProgramaEducativoDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        static SqlTransaction objMyTransaction = null;

        public object Listado(Guid token, int estatus, int idEmpresa, int tipoListado, int plataforma, ref HttpStatusCode Codigo_Respuesta, string idAlumno = "")
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_LISTADO_PROGRAMA_EDUCATIVO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                Guid tokenParammetro;

                if (plataforma == 2) { tokenParammetro = new Guid(); } //MOVIL
                else { tokenParammetro = token; }

                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", tokenParammetro));
                ObjCommand.Parameters.Add(new SqlParameter("@PLATAFORMA", plataforma));
                ObjCommand.Parameters.Add(new SqlParameter("@ID_ALUMNO", idAlumno));
                ObjCommand.Parameters.Add(new SqlParameter("@EMPRESA", idEmpresa));
                ObjCommand.Parameters.Add(new SqlParameter("@TIPO_LISTADO", tipoListado));
                ObjCommand.Parameters.Add(new SqlParameter("@ESTATUS", estatus));
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<ListadoProgramaEducativo> objPrograma = new List<ListadoProgramaEducativo>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objPrograma.Add(new ListadoProgramaEducativo
                        {
                            idProgramaUnique = objReader["idProgramaUnique"].ToString(),
                            gradoEducativo = objReader["gradoEducativo"].ToString(),
                            programaEducativo = objReader["programaEducativo"].ToString(),
                            perfilEgreso = objReader["perfilEgreso"].ToString(),
                            perfilIngreso = objReader["perfilIngreso"].ToString(),
                            requisitos = objReader["requisitos"].ToString(),
                            urlImagen = objReader["urlImagen"].ToString(),
                            estatus = Convert.ToInt16(objReader["estatus"])
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objPrograma;
            }
            catch (Exception e) {
                EscrituraLog.guardar("ProgramaEducativoDataAccess-Listado. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Guardar(ProgramaEducativo propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();

            object miObjetoRespuesta = new object();

            try
            {
                objMyTransaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_PROGRAMA_EDUCATIVO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Transaction = objMyTransaction;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@IDGRADO_EDUCATIVO", SqlDbType.Int).Value = propiedades.gradoEducativo;
                ObjCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = propiedades.nombre.Trim();
                ObjCommand.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = propiedades.descripcion.Trim();
                ObjCommand.Parameters.Add("@PERFIL_INGRESO", SqlDbType.VarChar).Value = propiedades.perfilIngreso.Trim();
                ObjCommand.Parameters.Add("@PERFIL_EGRESO", SqlDbType.VarChar).Value = propiedades.perfilEgreso.Trim();
                ObjCommand.Parameters.Add("@REQUISITOS", SqlDbType.VarChar).Value = propiedades.requisitos.Trim();
                ObjCommand.Parameters.Add("@ID_PROGRAMA_OUT", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_PROGRAMA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_EMPRESA_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.ExecuteNonQuery();

                string idProgramaOut = Convert.ToString(ObjCommand.Parameters["@ID_PROGRAMA_OUT"].Value);
                int idPrograma = Convert.ToInt32(ObjCommand.Parameters["@ID_PROGRAMA"].Value);
                int idEmpresaOut = Convert.ToInt16(ObjCommand.Parameters["@ID_EMPRESA_OUT"].Value);
                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK) && (propiedades.urlImagen != "" && propiedades.urlImagen != null))
                {
                    int respuestaImagen = 0;
                    var nombreArchivo = string.Format(@"{0}", idProgramaOut);
                    string rutaArchivo = HttpContext.Current.Server.MapPath(string.Concat("~/Img/ProgramaEducativo/", idEmpresaOut, "/", nombreArchivo, ".jpg"));
                    General.GuardarImagen(rutaArchivo, propiedades.urlImagen, ref respuestaImagen);

                    if (respuestaImagen == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        string rutaBaseDatos = string.Empty;
                        rutaBaseDatos = string.Concat("/Img/ProgramaEducativo/", idEmpresaOut, "/", nombreArchivo, ".jpg");
                        UrlImagen.Actualizar(connection, objMyTransaction, pagina, Convert.ToString(idProgramaOut), rutaBaseDatos);
                    }
                }

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    objMyTransaction.Commit();
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
                EscrituraLog.guardar("ProgramaEducativoDataAccess-Guardar. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Obtener(Guid token, string pagina, string idPrograma, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_PROGRAMA_EDUCATIVO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;
                ObjCommand.Parameters.Add("@ID_PROGRAMA", SqlDbType.VarChar).Value = idPrograma;
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                ProgramaObtener objPrograma = new ProgramaObtener();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objPrograma.idProgramaEducativo = objReader["idProgramaEducativo"].ToString();
                        objPrograma.gradoEducativo = Convert.ToInt16(objReader["idGradoEducativo"]);
                        objPrograma.nombre = objReader["nombre"].ToString();
                        objPrograma.descripcion = objReader["descripcion"].ToString();
                        objPrograma.perfilIngreso = objReader["perfilIngreso"].ToString();
                        objPrograma.perfilEgreso = objReader["perfilEgreso"].ToString();
                        objPrograma.requisitos = objReader["requisitos"].ToString();
                        objPrograma.urlImagen = objReader["urlImagen"].ToString();
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objPrograma;
            }
            catch (Exception e) {
                EscrituraLog.guardar("ProgramaEducativoDataAccess-Obtener. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Editar(ProgramaEducativo propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();

            object miObjetoRespuesta = new object();

            try
            {
                objMyTransaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                SqlCommand ObjCommand = new SqlCommand("USP_EDITAR_PROGRAMA_EDUCATIVO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Transaction = objMyTransaction;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@ID_PROGRAMA_EDUCATIVO", SqlDbType.VarChar).Value = propiedades.idProgramaEducativo;
                ObjCommand.Parameters.Add("@IDGRADO_EDUCATIVO", SqlDbType.Int).Value = propiedades.gradoEducativo;
                ObjCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = propiedades.nombre.Trim();
                ObjCommand.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = propiedades.descripcion.Trim();
                ObjCommand.Parameters.Add("@PERFIL_INGRESO", SqlDbType.VarChar).Value = propiedades.perfilIngreso.Trim();
                ObjCommand.Parameters.Add("@PERFIL_EGRESO", SqlDbType.VarChar).Value = propiedades.perfilEgreso.Trim();
                ObjCommand.Parameters.Add("@REQUISITOS", SqlDbType.VarChar).Value = propiedades.requisitos.Trim();
                ObjCommand.Parameters.Add("@ID_PROGRAMA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_EMPRESA_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.ExecuteNonQuery();

                int idPrograma = Convert.ToInt16(ObjCommand.Parameters["@ID_PROGRAMA"].Value);
                int idEmpresaOut = Convert.ToInt16(ObjCommand.Parameters["@ID_EMPRESA_OUT"].Value);
                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);
                int respuestaImagen = 0;

                var nombreArchivo = propiedades.idProgramaEducativo;
                string rutaArchivo = HttpContext.Current.Server.MapPath(string.Concat("~/Img/ProgramaEducativo/", idEmpresaOut, "/", nombreArchivo, ".jpg"));

                if (propiedades.urlImagen == "" || propiedades.urlImagen == null)
                {
                    General.EliminarImagen(rutaArchivo, ref respuestaImagen);
                    UrlImagen.Actualizar(connection, objMyTransaction, pagina, Convert.ToString(propiedades.idProgramaEducativo), "");
                }
                else if (propiedades.urlImagen.StartsWith("data:image", System.StringComparison.OrdinalIgnoreCase))
                {
                    General.GuardarImagen(rutaArchivo, propiedades.urlImagen, ref respuestaImagen);
                    if (respuestaImagen == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        string rutaBaseDatos = string.Empty;
                        rutaBaseDatos = string.Concat("/Img/ProgramaEducativo/", idEmpresaOut, "/", nombreArchivo, ".jpg");
                        UrlImagen.Actualizar(connection, objMyTransaction, pagina, Convert.ToString(propiedades.idProgramaEducativo), rutaBaseDatos);
                    }
                }

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    objMyTransaction.Commit();
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
                EscrituraLog.guardar("ProgramaEducativoDataAccess-Editar. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }


        #region "METODOS PARA AGREGAR PAGINA Y PAGINAORIGEN AL OBJETO DE GRADOS EDUCATIVOS"


        public object ObtenerGradosEducativos(Guid token)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_CATALOGOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = "GradoEducativo";
                ObjCommand.Parameters.Add("@ID", SqlDbType.Int).Value = 0;
                ObjCommand.Parameters.Add("@IDS", SqlDbType.VarChar).Value = "";
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<GradosEcutivosPrograma> objGradosEducativos = new List<GradosEcutivosPrograma>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        GradosEcutivosProgramaExtra objeto = new GradosEcutivosProgramaExtra();
                        objeto = ObtenerPagina(objReader["nombre"].ToString());

                        objGradosEducativos.Add(new GradosEcutivosPrograma
                        {
                            id = Convert.ToInt16(objReader["id"]),
                            nombre = objReader["nombre"].ToString(),
                            valor = objReader["valor"].ToString(),
                            pagina = objeto.pagina,
                            paginaOrigen = objeto.paginaOrigen,
                        });
                    }
                    objReader.Close();
                }

                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objGradosEducativos;
            }
            catch (Exception e) {
                EscrituraLog.guardar("ProgramaEducativoDataAccess-ObtenerGradosEducativos. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private GradosEcutivosProgramaExtra ObtenerPagina(string nombre)
        {
            GradosEcutivosProgramaExtra objeto = new GradosEcutivosProgramaExtra();

            switch (nombre)
            {
                case "Cursos":
                    objeto.pagina = "cursos";
                    objeto.paginaOrigen = "ListadoCursos";
                    break;
                case "Diplomados":
                    objeto.pagina = "diplomados";
                    objeto.paginaOrigen = "ListadoDiplomados";
                    break;
                case "Licenciaturas":
                    objeto.pagina = "licenciaturas";
                    objeto.paginaOrigen = "listadoLicenciatura";
                    break;
                case "Maestrías":
                    objeto.pagina = "maestrias";
                    objeto.paginaOrigen = "listadoMaestrias";
                    break;
                case "Talleres":
                    objeto.pagina = "talleres";
                    objeto.paginaOrigen = "ListadoTalleres";
                    break;

                case "Bachillerato":
                    objeto.pagina = "bachillerato";
                    objeto.paginaOrigen = "listadoBachillerato";
                    break;
                case "Especialidades":
                    objeto.pagina = "especialidades";
                    objeto.paginaOrigen = "ListadoEspecialidades";
                    break;

            }
            return objeto;
        }
        #endregion

    }
}
