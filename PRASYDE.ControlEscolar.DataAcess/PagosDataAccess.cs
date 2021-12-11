
//AUTOR: DIEGO OLVERA
//FECHA: 06-05-2019
//DESCRIPCIÓN: CLASE QUE GESTIONA EL CRUD DE PAGOS

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

    public class PagosDataAccess : BaseDataAccess
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
                SqlCommand ObjCommand = new SqlCommand("USP_LISTADO_PAGOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@ESTATUS", estatus));
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<ListadoPagos> objPagos = new List<ListadoPagos>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objPagos.Add(new ListadoPagos
                        {
                            idPago = objReader["idPago"].ToString(),
                            nombreAlumno = objReader["nombreAlumno"].ToString(),
                            concepto = objReader["concepto"].ToString(),
                            descripcion = objReader["comentarios"].ToString(),
                            monto = Convert.ToDecimal(objReader["monto"]),
                            fecha = objReader["fecha"].ToString(),
                            urlImagen = objReader["urlImagen"].ToString(),
                            nombrePrograma = objReader["nombrePrograma"].ToString(),
                            estatusPago = Convert.ToInt16(objReader["estatusPago"]),
                            estatus = Convert.ToInt16(objReader["estatus"])
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objPagos;
            }
            catch (Exception e) {
                EscrituraLog.guardar("PagosDataAccess-Listado. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Guardar(Pagos propiedades, Guid token, string pagina, int plataforma, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_PAGOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;
                ObjCommand.Parameters.Add("@PLATAFORMA", SqlDbType.Int).Value = plataforma;

                string usuario = string.Empty;
                if (plataforma == 2) { usuario = propiedades.idUsuario; } //WEB              
                ObjCommand.Parameters.Add("@ID_USUARIO_WEB", SqlDbType.VarChar).Value = usuario;

                ObjCommand.Parameters.Add("@ID_PROGRAMA", SqlDbType.VarChar).Value = propiedades.idPrograma;
                ObjCommand.Parameters.Add("@ID_CONCEPTO", SqlDbType.VarChar).Value = propiedades.concepto.Trim();
                ObjCommand.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = propiedades.descripcion.Trim();
                ObjCommand.Parameters.Add("@MONTO", SqlDbType.Decimal).Value = propiedades.monto;

                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_PAGO_OUT", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_EMPRESA_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_REFERENCIA", SqlDbType.Int).Direction = ParameterDirection.Output;

                connection.Open();
                ObjCommand.ExecuteNonQuery();

                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);
                string idPagoOut = ObjCommand.Parameters["@ID_PAGO_OUT"].Value.ToString();
                int idEmpresa = Convert.ToInt16(ObjCommand.Parameters["@ID_EMPRESA_OUT"].Value);
                int referencia = Convert.ToInt32(ObjCommand.Parameters["@ID_REFERENCIA"].Value);

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                if (respuesta == 200 && propiedades.archivos.Count > 0)
                {
                    string extension = System.IO.Path.GetExtension(propiedades.archivos[0].FileName);
                    string nombrArchivo = System.IO.Path.GetFileNameWithoutExtension(propiedades.archivos[0].FileName);
                    string nombreArchivoBase = idPagoOut;

                    MemoryStream fileStream = General.getStreamFromFile(propiedades.archivos[0]);

                    string filePath = HttpContext.Current.Server.MapPath(string.Concat("~/Img/Pagos/", idEmpresa, "/", idPagoOut, extension));
                    bool saveFile = General.saveAttachment(fileStream, filePath);
                    if (saveFile)
                    {
                        string rutaBaseDatos = string.Empty;
                        rutaBaseDatos = string.Concat("/Img/Pagos/", idEmpresa, "/", idPagoOut, extension);
                        UrlImagen.Actualizar(connection, pagina, Convert.ToString(idPagoOut), rutaBaseDatos);
                    }
                }

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = referencia;

            }
            catch (Exception e) {
                EscrituraLog.guardar("PagosDataAccess-Guardar. ", e.Message.ToString());
            }
            finally { connection.Close();  connection.Dispose(); }

            return miObjetoRespuesta;
        }
        
        public object ValidarPagos(string idPago, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_VALIDAR_PAGOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;
                ObjCommand.Parameters.Add("@ID_PAGO", SqlDbType.VarChar).Value = idPago;

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
                EscrituraLog.guardar("PagosDataAccess-ValidarPagos. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }
    }
}
