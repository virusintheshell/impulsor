
//AUTOR: DIEGO OLVERA
//FECHA: 06-05-2019
//DESCRIPCIÓN: CLASE QUE GESTIONA EL CRUD DE AVISOS

namespace PRASYDE.ControlEscolar.DataAcess
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

    public class AvisosDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        #region "METODOS PARA EL CRUD DE AVISOS"

        public object Listado(Guid token, int estatus,int tipo, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_LISTADO_AVISOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@TIPO", tipo));
                ObjCommand.Parameters.Add(new SqlParameter("@ESTATUS", estatus));
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<ListadoAvisos> objAvisos = new List<ListadoAvisos>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objAvisos.Add(new ListadoAvisos
                        {
                            idAviso = objReader["idAviso"].ToString(),
                            nombre = objReader["nombre"].ToString(),
                            descripcion = objReader["descripcion"].ToString(),
                            autor = objReader["autor"].ToString(),
                            imagen = objReader["imagen"].ToString(),
                            fecha = objReader["fecha"].ToString(),
                            estatus = Convert.ToInt16(objReader["estatus"])
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objAvisos;


            }
            catch (Exception e) {
                EscrituraLog.guardar("AvisosDataAccess-Listado. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Guardar(Avisos propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_AVISOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = propiedades.nombre.Trim();
                ObjCommand.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = propiedades.descripcion.Trim();
                ObjCommand.Parameters.Add("@AUTOR", SqlDbType.VarChar).Value = propiedades.autor.Trim();

                ObjCommand.Parameters.Add("@ID_AVISO_OUT", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_EMPRESA_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;

                connection.Open();
                ObjCommand.ExecuteNonQuery();

                string idAvisoOut = Convert.ToString(ObjCommand.Parameters["@ID_AVISO_OUT"].Value);
                int idEmpresaOut = Convert.ToInt16(ObjCommand.Parameters["@ID_EMPRESA_OUT"].Value);
                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK) && propiedades.imagen != "")
                {
                    int respuestaImagen = 0;
                    var nombreArchivo = string.Format(@"{0}", idAvisoOut);
                    string rutaArchivo = HttpContext.Current.Server.MapPath(string.Concat("~/Img/Avisos/", idEmpresaOut, "/", nombreArchivo, ".jpg"));
                    General.GuardarImagen(rutaArchivo, propiedades.imagen, ref respuestaImagen);

                    if (respuestaImagen == Convert.ToInt16(200))
                    {
                        string rutaBaseDatos = string.Empty;
                        rutaBaseDatos = string.Concat("/Img/Avisos/", idEmpresaOut, "/", nombreArchivo, ".jpg");
                        UrlImagen.Actualizar(connection, pagina, Convert.ToString(idAvisoOut), rutaBaseDatos);
                    }
                }
                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = new object();


            }
            catch (Exception e) {
                EscrituraLog.guardar("AvisosDataAccess-Guardar. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Editar(Avisos propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_EDITAR_AVISO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@ID_AVISO_UNIQUE", SqlDbType.VarChar).Value = propiedades.idAviso;

                ObjCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = propiedades.nombre.Trim();
                ObjCommand.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = propiedades.descripcion.Trim();
                ObjCommand.Parameters.Add("@AUTOR", SqlDbType.VarChar).Value = propiedades.autor.Trim();

                ObjCommand.Parameters.Add("@ID_EMPRESA_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;

                connection.Open();
                ObjCommand.ExecuteNonQuery();

                string idUAvisoOut = propiedades.idAviso;
                int idEmpresaOut = Convert.ToInt16(ObjCommand.Parameters["@ID_EMPRESA_OUT"].Value);
                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);
                int respuestaImagen = 0;

                var nombreArchivo = string.Format(@"{0}", idUAvisoOut);
                string rutaArchivo = HttpContext.Current.Server.MapPath(string.Concat("~/Img/Avisos/", idEmpresaOut, "/", nombreArchivo, ".jpg"));

                if (propiedades.imagen == "" || propiedades.imagen == null)
                {
                    General.EliminarImagen(rutaArchivo, ref respuestaImagen);
                    UrlImagen.Actualizar(connection, pagina, Convert.ToString(idUAvisoOut), "");
                }

                else if (propiedades.imagen.StartsWith("data:image", System.StringComparison.OrdinalIgnoreCase))
                {
                    General.GuardarImagen(rutaArchivo, propiedades.imagen, ref respuestaImagen);
                    if (respuestaImagen == 200)
                    {
                        string rutaBaseDatos = string.Empty;
                        rutaBaseDatos = string.Concat("/Img/Avisos/", idEmpresaOut, "/", nombreArchivo, ".jpg");

                        UrlImagen.Actualizar(connection, pagina, Convert.ToString(idUAvisoOut), rutaBaseDatos);
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
                EscrituraLog.guardar("AvisosDataAccess-Editar. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Obtener(Guid token, string pagina, string idAviso, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_AVISO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@PAGINA", pagina));
                ObjCommand.Parameters.Add(new SqlParameter("@ID_AVISO", idAviso));

                connection.Open();
                Avisos objAviso = new Avisos();
                SqlDataReader objReader = ObjCommand.ExecuteReader();

                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objAviso.idAviso = objReader["idAviso"].ToString();
                        objAviso.nombre = objReader["nombre"].ToString();
                        objAviso.descripcion = objReader["descripcion"].ToString();
                        objAviso.autor = objReader["autor"].ToString();
                        objAviso.imagen = objReader["imagen"].ToString();
                    }
                }
                objReader.Close();

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objAviso;

               
            }
            catch (Exception e) {
                EscrituraLog.guardar("AvisosDataAccess-Obtener. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object ListadoElementosEnvio(Guid token, int tipo,string idAviso, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_ELEMENTOS_ENVIO", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@ID_AVISO", idAviso.ToUpper()));
                ObjCommand.Parameters.Add(new SqlParameter("@TIPO", tipo));

                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<ElementosEnvio> obj = new List<ElementosEnvio>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        obj.Add(new ElementosEnvio
                        {
                            id = Convert.ToInt16(objReader["id"]),
                            nombre = objReader["nombre"].ToString(),
                            enviado = Convert.ToInt16(objReader["enviado"])
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = obj;
            }
            catch (Exception e) {
                EscrituraLog.guardar("ListadoElementosEnvio-Obtener. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        #endregion

        #region "METODOS PARA EL ENVIO DE AVISOS"

        public object GuardarEnvio(PropiedadesEnvio propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_ENVIOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@ID_AVISO", SqlDbType.VarChar).Value = propiedades.idAviso.Trim();
                ObjCommand.Parameters.Add("@TIPO_AVISO", SqlDbType.Int).Value = propiedades.tipoAviso;
                ObjCommand.Parameters.Add("@ENVIAR_A_TODOS", SqlDbType.Int).Value = propiedades.enviarAtodos;

                string cadenaIDs = string.Empty;
                int respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);

                if (propiedades.enviarAtodos == 0) { cadenaIDs = ObtenerIDs(propiedades, ref respuesta); }

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError))
                {
                    Codigo_Respuesta = HttpStatusCode.InternalServerError;
                    codigoRespuesta = respuesta;
                    textoRespuesta = "Internal Server Error";
                    return miObjetoRespuesta;
                }
                ObjCommand.Parameters.Add("@CADENA_IDS", SqlDbType.VarChar).Value = cadenaIDs;

                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_REFERENCIA", SqlDbType.Int).Direction = ParameterDirection.Output;
                connection.Open();
                ObjCommand.ExecuteNonQuery();
                respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);
                int idAviso = Convert.ToInt16(ObjCommand.Parameters["@ID_REFERENCIA"].Value);

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = idAviso;
            }
            catch (Exception e) {
                EscrituraLog.guardar("ListadoElementosEnvio-GuardarEnvio. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private string ObtenerIDs(PropiedadesEnvio propiedades, ref int respuesta)
        {
            string resultado = string.Empty;
            respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            ArrayList arr = new ArrayList();
            arr = propiedades.cadenaIds;

            try
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    resultado =  propiedades.cadenaIds[i].ToString() + ',' + resultado.ToString();
                }
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("ListadoElementosEnvio-ObtenerIDs. ", e.Message.ToString());
            }
            return resultado;
        }

        #endregion

    }
}
