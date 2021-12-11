
//AUTOR: DIEGO OLVERA
//FECHA: 06-04-2019
//DESCRIPCIÓN: CLASE QUE GUARDA UN USUARIO Y DEPENDIENDO EL ROL SERA: ALUMNO, ADMINSITRATIVO O PROFESOR

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
    using System.Security.Cryptography;
    using PRASYDE.ControlEscolar.Entities;

    public class UsuariosDataAccess : BaseDataAccess
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
                SqlCommand ObjCommand = new SqlCommand("USP_LISTADO_USUARIOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@ESTATUS", estatus));
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<ListadoUsarios> objUsuarios = new List<ListadoUsarios>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objUsuarios.Add(new ListadoUsarios
                        {
                            idUsuario = objReader["idUsuario"].ToString(),
                            nombre = objReader["nombre"].ToString(),
                            rol = objReader["rol"].ToString(),
                            correo = objReader["correo"].ToString(),
                            celular = objReader["celular"].ToString(),
                            estatus = Convert.ToInt16(objReader["estatus"])
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objUsuarios;
            }
            catch (Exception e) {
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("UsuariosDataAccess-Listado. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Guardar(Usuarios propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_USUARIOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@IDGRADOESTUDIOS", SqlDbType.Int).Value = propiedades.idGradoEstudios;
                ObjCommand.Parameters.Add("@IDROL", SqlDbType.Int).Value = propiedades.idRol;
                ObjCommand.Parameters.Add("@MATRICULA_OFICAL", SqlDbType.VarChar).Value = propiedades.matriculaOficial.Trim();
                ObjCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = propiedades.nombre.Trim();
                ObjCommand.Parameters.Add("@PRIMERAPELLIDO", SqlDbType.VarChar).Value = propiedades.primerApellido.Trim();
                ObjCommand.Parameters.Add("@SEGUNDOAPELLIDO", SqlDbType.VarChar).Value = propiedades.segundoApellido.Trim();
                ObjCommand.Parameters.Add("@EDAD", SqlDbType.Int).Value = propiedades.edad.Trim();
                ObjCommand.Parameters.Add("@SEXO", SqlDbType.VarChar).Value = propiedades.sexo;
                ObjCommand.Parameters.Add("@TELEFONO", SqlDbType.VarChar).Value = propiedades.telefono.Trim();
                ObjCommand.Parameters.Add("@CELULAR", SqlDbType.VarChar).Value = propiedades.celular.Trim();
                ObjCommand.Parameters.Add("@DIRECCION", SqlDbType.VarChar).Value = propiedades.direccion.Trim();
                ObjCommand.Parameters.Add("@FECHANACIMIENTO", SqlDbType.VarChar).Value = propiedades.fechaNacimiento;
                ObjCommand.Parameters.Add("@IDESTADOCIVIL", SqlDbType.Int).Value = propiedades.idEstadoCivil;
                ObjCommand.Parameters.Add("@CORREOELECTRONICO", SqlDbType.VarChar).Value = propiedades.correoElectronico.Trim();
                ObjCommand.Parameters.Add("@CEDULA", SqlDbType.VarChar).Value = propiedades.cedula.Trim();
                ObjCommand.Parameters.Add("@OCUPACION", SqlDbType.Int).Value = propiedades.ocupacion;
                ObjCommand.Parameters.Add("@CURP", SqlDbType.VarChar).Value = propiedades.CURP.Trim();
                ObjCommand.Parameters.Add("@ID_USUARIO_OUT", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@CONTRASENA_OUT", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_EMPRESA_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;

                connection.Open();
                ObjCommand.ExecuteNonQuery();

                string idUsuarioOut = Convert.ToString(ObjCommand.Parameters["@ID_USUARIO_OUT"].Value);
                string contrasenaOut = Convert.ToString(ObjCommand.Parameters["@CONTRASENA_OUT"].Value);
                int idEmpresaOut = Convert.ToInt16(ObjCommand.Parameters["@ID_EMPRESA_OUT"].Value);
                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                if (respuesta == 200 && propiedades.urlImagen != "")
                {
                    int respuestaImagen = 0;
                    var nombreArchivo = string.Format(@"{0}", idUsuarioOut);
                    string rutaArchivo = HttpContext.Current.Server.MapPath(string.Concat("~/Img/ImagesUsers/", idEmpresaOut, "/", nombreArchivo, ".jpg"));
                    General.GuardarImagen(rutaArchivo, propiedades.urlImagen, ref respuestaImagen);

                    if (respuestaImagen == Convert.ToInt16(200))
                    {
                        string rutaBaseDatos = string.Empty;
                        rutaBaseDatos = string.Concat("/Img/ImagesUsers/", idEmpresaOut, "/", nombreArchivo, ".jpg");
                        UrlImagen.Actualizar(connection, pagina, Convert.ToString(idUsuarioOut), rutaBaseDatos);
                    }
                }
          
                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK)) {

                    ObjetoCorreoElectronico objeto = new ObjetoCorreoElectronico();
                    objeto.tipoCorreo = Convert.ToInt16(Enumerados.TipoEnvioCorreo.CambioContrasena);
                    objeto.nombre = string.Concat(propiedades.nombre, " ", propiedades.primerApellido); 
                    objeto.correo = propiedades.correoElectronico;
                    objeto.contrasena = contrasenaOut;
                    objeto.asunto = "Cambio de contraseña";

                    EnvioCorreoElectronico.Enviar(objeto);
                }

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = new object();
            }
            catch (Exception e) {
                EscrituraLog.guardar("UsuariosDataAccess-Guardar. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Obtener(Guid token, string pagina, string idUsuario, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_USUARIO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@PAGINA", pagina));
                ObjCommand.Parameters.Add(new SqlParameter("@ID_USUARIO", idUsuario));

                connection.Open();
                Usuarios objUsuario = new Usuarios();
                SqlDataReader objReader = ObjCommand.ExecuteReader();

                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objUsuario.idUsuario = objReader["idUsuario"].ToString();
                        objUsuario.idGradoEstudios = Convert.ToInt16(objReader["idGradoEstudios"]);
                        objUsuario.idRol = Convert.ToInt16(objReader["idRol"]);
                        objUsuario.matriculaOficial = objReader["matriculaOficial"].ToString();
                        objUsuario.nombre = objReader["nombre"].ToString();
                        objUsuario.primerApellido = objReader["primerApellido"].ToString();
                        objUsuario.segundoApellido = objReader["segundoApellido"].ToString();
                        objUsuario.edad = objReader["edad"].ToString();
                        objUsuario.sexo = objReader["sexo"].ToString();
                        objUsuario.telefono = objReader["telefono"].ToString();
                        objUsuario.celular = objReader["celular"].ToString();
                        objUsuario.direccion = objReader["direccion"].ToString();
                        objUsuario.fechaNacimiento = objReader["fechaNacimiento"].ToString();
                        objUsuario.idEstadoCivil = Convert.ToInt16(objReader["idEstadoCivil"]);
                        objUsuario.correoElectronico = objReader["correoElectronico"].ToString();
                        objUsuario.cedula = objReader["cedula"].ToString();
                        objUsuario.ocupacion = Convert.ToInt16(objReader["ocupacion"]);
                        objUsuario.CURP = objReader["CURP"].ToString();
                        objUsuario.urlImagen = objReader["urlImagen"].ToString();
                        objUsuario.idJerarquia = Convert.ToInt16(objReader["idJerarquia"]);
                        objUsuario.detalleFechaNacimiento = objReader["detalleFechaNacimiento"].ToString();
                        objUsuario.detalleGenero = objReader["detalleGenero"].ToString();
                        objUsuario.detallEstadoCivil = objReader["detallEstadoCivil"].ToString();
                        objUsuario.detalleGradoEscolar = objReader["detalleGradoEscolar"].ToString();
                        objUsuario.detalleOcupacion = objReader["detalleOcupacion"].ToString();
                    }
                }
                objReader.Close();

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objUsuario;
            }
            catch (Exception e) {
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("UsuariosDataAccess-Obtener. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public string ObtenerIdUsuario(Guid token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            string id = string.Empty;
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_IDALUMNO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                connection.Open();

                id = ObjCommand.ExecuteScalar().ToString();
            }
            catch (Exception e) {
                EscrituraLog.guardar("UsuariosDataAccess-ObtenerIdUsuario. ", e.Message.ToString());
            }
            finally { connection.Dispose(); connection.Close(); }

            return id;
        }

        public object Editar(Usuarios propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_EDITAR_USUARIO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar).Value = propiedades.idUsuario;
                ObjCommand.Parameters.Add("@IDGRADOESTUDIOS", SqlDbType.Int).Value = propiedades.idGradoEstudios;
                ObjCommand.Parameters.Add("@MATRICULA_OFICAL", SqlDbType.VarChar).Value = propiedades.matriculaOficial.Trim();
                ObjCommand.Parameters.Add("@IDROL", SqlDbType.Int).Value = propiedades.idRol;
                ObjCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = propiedades.nombre.Trim();
                ObjCommand.Parameters.Add("@PRIMERAPELLIDO", SqlDbType.VarChar).Value = propiedades.primerApellido.Trim();
                ObjCommand.Parameters.Add("@SEGUNDOAPELLIDO", SqlDbType.VarChar).Value = propiedades.segundoApellido.Trim();
                ObjCommand.Parameters.Add("@EDAD", SqlDbType.Int).Value = propiedades.edad.Trim();
                ObjCommand.Parameters.Add("@SEXO", SqlDbType.VarChar).Value = propiedades.sexo;
                ObjCommand.Parameters.Add("@TELEFONO", SqlDbType.VarChar).Value = propiedades.telefono.Trim();
                ObjCommand.Parameters.Add("@CELULAR", SqlDbType.VarChar).Value = propiedades.celular.Trim();
                ObjCommand.Parameters.Add("@DIRECCION", SqlDbType.VarChar).Value = propiedades.direccion.Trim();
                ObjCommand.Parameters.Add("@FECHANACIMIENTO", SqlDbType.VarChar).Value = propiedades.fechaNacimiento;
                ObjCommand.Parameters.Add("@IDESTADOCIVIL", SqlDbType.Int).Value = propiedades.idEstadoCivil;
                ObjCommand.Parameters.Add("@CORREOELECTRONICO", SqlDbType.VarChar).Value = propiedades.correoElectronico.Trim();
                ObjCommand.Parameters.Add("@CEDULA", SqlDbType.VarChar).Value = propiedades.cedula.Trim();
                ObjCommand.Parameters.Add("@OCUPACION", SqlDbType.Int).Value = propiedades.ocupacion;
                ObjCommand.Parameters.Add("@CURP", SqlDbType.VarChar).Value = propiedades.CURP.Trim();
                
                ObjCommand.Parameters.Add("@ID_USUARIO_OUT", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_EMPRESA_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;

                connection.Open();
                ObjCommand.ExecuteNonQuery();

                string idUsuarioOut = Convert.ToString(ObjCommand.Parameters["@ID_USUARIO_OUT"].Value);
                int idEmpresaOut = Convert.ToInt16(ObjCommand.Parameters["@ID_EMPRESA_OUT"].Value);
                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);
                int respuestaImagen = 0;


                var nombreArchivo = string.Format(@"{0}", idUsuarioOut);
                string rutaArchivo = HttpContext.Current.Server.MapPath(string.Concat("~/Img/ImagesUsers/", idEmpresaOut, "/", nombreArchivo, ".jpg"));

                if (propiedades.urlImagen == "" || propiedades.urlImagen == null)
                {
                    General.EliminarImagen(rutaArchivo, ref respuestaImagen);
                    UrlImagen.Actualizar(connection, pagina, Convert.ToString(idUsuarioOut), "");
                }

                else if (propiedades.urlImagen.StartsWith("data:image", System.StringComparison.OrdinalIgnoreCase))
                {
                    General.GuardarImagen(rutaArchivo, propiedades.urlImagen, ref respuestaImagen);
                    if (respuestaImagen == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        string rutaBaseDatos = string.Empty;
                        rutaBaseDatos = string.Concat("/Img/ImagesUsers/", idEmpresaOut, "/", nombreArchivo, ".jpg");

                        UrlImagen.Actualizar(connection,pagina, Convert.ToString(idUsuarioOut), rutaBaseDatos);
                    }
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
                EscrituraLog.guardar("UsuariosDataAccess-Editar. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object GuardarPreInscripcion(UsuariosPreInscripcion propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_PRE_INSCRIPCION", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = propiedades.nombre.Trim();
                ObjCommand.Parameters.Add("@PRIMERAPELLIDO", SqlDbType.VarChar).Value = propiedades.primerApellido.Trim();
                ObjCommand.Parameters.Add("@SEGUNDOAPELLIDO", SqlDbType.VarChar).Value = propiedades.segundoApellido.Trim();
                ObjCommand.Parameters.Add("@CORREOELECTRONICO", SqlDbType.VarChar).Value = propiedades.correoElectronico.Trim();
                ObjCommand.Parameters.Add("@CELULAR", SqlDbType.VarChar).Value = propiedades.celular.Trim();
                ObjCommand.Parameters.Add("@ID_GRADO_EDUCATIVO", SqlDbType.VarChar).Value = propiedades.idGradoEducativo.Trim();
                ObjCommand.Parameters.Add("@ID_PROGRAMA", SqlDbType.VarChar).Value = propiedades.idProgramaEducativo.Trim();
                                
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
                miObjetoRespuesta = new object();
            }
            catch (Exception e) {
                EscrituraLog.guardar("UsuariosDataAccess-GuardarPreInscripcion. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object CambiarContrasena(Guid token, propiedadContasena propiedad, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_CAMBIAR_CONTASENA", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                
                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@CONTRASENA", SqlDbType.VarChar).Value = propiedad.contrasena;
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
                miObjetoRespuesta = new object();
            }
            catch (Exception e) {
                EscrituraLog.guardar("UsuariosDataAccess-CambiarContrasena. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }
        
        public string xxx(string cadena)
        {
            var strEncryptred = Cryptography.TripleDesDecrypt(cadena.Trim());
            return strEncryptred;
        }
    }
}
