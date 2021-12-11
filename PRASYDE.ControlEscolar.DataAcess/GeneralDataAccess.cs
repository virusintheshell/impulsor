
//AUTOR: DIEGO OLVERA
//FECHA: 17-04-2019
//DESCRIPCIÓN: CLASE QUE GESTIONA METODOS GENERALES QUE SE UTILIZAN EN TODO EL PROYECTO

namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using Entities;
    using Framework;
    using System.Net;
    using System.Data;
    using Newtonsoft.Json;
    using System.Data.SqlClient;
    using System.Collections.Generic;

    public class GeneralDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        public object Cambiar(Guid token, string pagina, string id, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_CAMBIAR_ESTATUS_REGISTRO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;
                ObjCommand.Parameters.Add("@ID", SqlDbType.VarChar).Value = id.Trim();
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
                EscrituraLog.guardar("GeneralDataAccess-Cambiar. ", e.Message.ToString());
            }
            finally { connection.Close();  connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Eliminar(Guid token, string pagina, string id, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_ELIMINAR_REGISTRO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;
                ObjCommand.Parameters.Add("@ID", SqlDbType.VarChar).Value = id.Trim();
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
                EscrituraLog.guardar("GeneralDataAccess-Eliminar. ", e.Message.ToString());
            }
            finally { connection.Close();  connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Permisos(Guid token, string pagina, string paginaOrigen, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_PERMISOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = paginaOrigen;
                ObjCommand.Parameters.Add("@PAGINA_ORIGEN", SqlDbType.VarChar).Value = pagina;

                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<ListaGenericaCatalogos> objPermisos = new List<ListaGenericaCatalogos>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objPermisos.Add(new ListaGenericaCatalogos
                        {
                            id = Convert.ToInt16(objReader["id"]),
                            nombre = objReader["nombre"].ToString()
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objPermisos;
            }
            catch (Exception e) {
                EscrituraLog.guardar("GeneralDataAccess-Permisos. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object AccesoPagina(Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_VALIDA_ACCESO_PAGINA", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;
              
                connection.Open();
                int accesoPagina = Convert.ToInt16(ObjCommand.ExecuteScalar());

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = accesoPagina == 1 ? Convert.ToInt16(Enumerados.Codigos_Respuesta.OK) : Convert.ToInt16(Enumerados.Codigos_Respuesta.NoPermission);
                textoRespuesta = "OK";
                miObjetoRespuesta = accesoPagina == 1 ? true : false;
            }
            catch (Exception e) {
                EscrituraLog.guardar("GeneralDataAccess-AccesoPagina. ", e.Message.ToString());
            }
            finally { connection.Dispose(); connection.Close(); }

            return miObjetoRespuesta;
        }

        public object ReenviarCorreoRegistro(propiedadesReenvioCorreo propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_REENVIAR_CORREO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                string idUsuario = string.Empty;
                idUsuario = propiedades.idUsuario == null ? "" : propiedades.idUsuario;

                ObjCommand.Parameters.Add("@ID_USUARIOUNIQUE", SqlDbType.VarChar).Value = idUsuario;
                ObjCommand.Parameters.Add("@CORREO", SqlDbType.VarChar).Value = propiedades.correoElectronico;

                ObjCommand.Parameters.Add("@NOMBRE_OUT", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@CONTRASENA_OUT", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;

                connection.Open();
                ObjCommand.ExecuteNonQuery();

                string nombreOut = Convert.ToString(ObjCommand.Parameters["@NOMBRE_OUT"].Value);
                string contrasenaOut = Convert.ToString(ObjCommand.Parameters["@CONTRASENA_OUT"].Value);
                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                if (respuesta == 200)
                {
                    ObjetoCorreoElectronico objeto = new ObjetoCorreoElectronico();
                    objeto.tipoCorreo = Convert.ToInt16(Enumerados.TipoEnvioCorreo.CambioContrasena);
                    objeto.nombre = nombreOut;
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
                EscrituraLog.guardar("GeneralDataAccess-ReenviarCorreoRegistro. ", e.Message.ToString());
            }
            finally { connection.Close();  connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object ObtenerComponente(Guid token, string nombreComponente)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_CONFIGURACION_COMPONENTES", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@NOMBRE_COMPONENTE", nombreComponente));
                connection.Open();

                Componente objComponente = new Componente();
                SqlDataReader objReader = ObjCommand.ExecuteReader();

                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objComponente.idComponenteUnique = objReader["idComponenteUnique"].ToString();
                        objComponente.nombre = objReader["nombre"].ToString();
                        objComponente.texto = objReader["texto"].ToString();
                    }
                }
                objReader.Close();
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objComponente;
            }
            catch (Exception e) {
                EscrituraLog.guardar("GeneralDataAccess-ObtenerComponente. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }
    }
}
