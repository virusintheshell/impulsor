
//AUTOR: DIEGO OLVERA
//FECHA: 01-04-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA GUARDAR, OBTENER, EDITAR Y LISTADO GENERAL

namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using Framework;
    using System.Web;
    using System.Net;
    using System.Data;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;
   
    public class PlantelDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }
        
        #region "METODOS PARA EL CRUD DE PLANTELES"
       
        public object Guardar(Planteles propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_PLANTELES", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = propiedades.nombre;
                ObjCommand.Parameters.Add("@DIRECCION", SqlDbType.VarChar).Value = propiedades.direccion;
                ObjCommand.Parameters.Add("@LATITUD", SqlDbType.Decimal).Value = propiedades.latitud;
                ObjCommand.Parameters.Add("@LONGITUD", SqlDbType.Decimal).Value = propiedades.longitud;
                ObjCommand.Parameters.Add("@REFERENCIA", SqlDbType.VarChar).Value = propiedades.referencia;
                ObjCommand.Parameters.Add("@CONTACTO", SqlDbType.VarChar).Value = propiedades.contacto;
                ObjCommand.Parameters.Add("@CORREO", SqlDbType.VarChar).Value = propiedades.correoElectronico;
                ObjCommand.Parameters.Add("@TELEFONO", SqlDbType.VarChar).Value = propiedades.telefono;
                ObjCommand.Parameters.Add("@EXTENSION", SqlDbType.VarChar).Value = propiedades.extension;
                
                ObjCommand.Parameters.Add("@ID_PLANTEL_OUT", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_EMPRESA_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                connection.Open();
                ObjCommand.ExecuteNonQuery();

                string idPlantelOut = Convert.ToString(ObjCommand.Parameters["@ID_PLANTEL_OUT"].Value);
                int idEmpresaOut = Convert.ToInt16(ObjCommand.Parameters["@ID_EMPRESA_OUT"].Value);
                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK) && propiedades.urlImagen != "")
                {
                    int respuestaImagen = 0;
                    var nombreArchivo = string.Format(@"{0}", idPlantelOut);
                    string rutaArchivo = HttpContext.Current.Server.MapPath(string.Concat("~/Img/Planteles/", idEmpresaOut, "/", nombreArchivo, ".jpg"));
                    General.GuardarImagen(rutaArchivo, propiedades.urlImagen, ref respuestaImagen);

                    if (respuestaImagen == Convert.ToInt16(Convert.ToInt16(Enumerados.Codigos_Respuesta.OK)))
                    {
                        string rutaBaseDatos = string.Empty;
                        rutaBaseDatos = string.Concat("/Img/Planteles/", idEmpresaOut, "/", nombreArchivo, ".jpg");
                        UrlImagen.Actualizar(connection, pagina, Convert.ToString(idPlantelOut), rutaBaseDatos);
                    }
                }

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = new object();
            }
            catch (Exception e) {
                EscrituraLog.guardar("PlantelDataAccess-Guardar. ", e.Message.ToString());
            }
            finally { connection.Close();  connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Listado(Guid token, int tipo, int idEmpresa, int estatus, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_LISTADO_PLANTELES", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                Guid tokenParammetro;

                if (tipo == 1) { tokenParammetro = new Guid(); } //MOVIL
                else { tokenParammetro = token; }

                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", tokenParammetro));
                ObjCommand.Parameters.Add(new SqlParameter("@EMPRESA", idEmpresa));
                ObjCommand.Parameters.Add(new SqlParameter("@ESTATUS", estatus));

                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<Planteles> objPlantel = new List<Planteles>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objPlantel.Add(new Planteles
                        {
                            idPlantel = objReader["idPlantel"].ToString(),
                            nombre = objReader["nombre"].ToString(),
                            direccion = objReader["direccion"].ToString(),
                            latitud = Convert.ToDecimal(objReader["latitud"]),
                            longitud = Convert.ToDecimal(objReader["longitud"]),
                            telefono = objReader["telefono"].ToString(),
                            extension = objReader["extension"].ToString(),
                            contacto = objReader["contacto"].ToString(),
                            urlImagen = objReader["urlImagen"].ToString(),
                            estatus = Convert.ToInt16(objReader["estatus"])
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objPlantel;
            }
            catch (Exception e) {
                EscrituraLog.guardar("PlantelDataAccess-Listado. ", e.Message.ToString());
            }
            finally { connection.Close();  connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object ObtenerPlantel(Guid token, string pagina, string idPlantel, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_PLANTEL", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@PAGINA", pagina));
                ObjCommand.Parameters.Add(new SqlParameter("@ID_PLANTEL", idPlantel));

                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                Planteles objPlantel = new Planteles();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objPlantel.idPlantel = objReader["idPlantel"].ToString();
                        objPlantel.nombre = objReader["nombre"].ToString();
                        objPlantel.direccion = objReader["direccion"].ToString();
                        objPlantel.latitud = Convert.ToDecimal(objReader["latitud"]);
                        objPlantel.longitud = Convert.ToDecimal(objReader["longitud"]);
                        objPlantel.referencia = objReader["referencia"].ToString();
                        objPlantel.contacto = objReader["contacto"].ToString();
                        objPlantel.correoElectronico = objReader["correo"].ToString();
                        objPlantel.telefono = objReader["telefono"].ToString();
                        objPlantel.extension = objReader["extension"].ToString();
                        objPlantel.urlImagen = objReader["urlImagen"].ToString();
                    }
                }
                objReader.Close();

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objPlantel;
            }
            catch (Exception e) {
                EscrituraLog.guardar("PlantelDataAccess-ObtenerPlantel. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Editar(Planteles propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_EDITAR_PLANTEL", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@ID_PLANTELUNIQUE", SqlDbType.VarChar).Value = propiedades.idPlantel;
                ObjCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = propiedades.nombre;
                ObjCommand.Parameters.Add("@DIRECCION", SqlDbType.VarChar).Value = propiedades.direccion;
                ObjCommand.Parameters.Add("@LATITUD", SqlDbType.Decimal).Value = propiedades.latitud;
                ObjCommand.Parameters.Add("@LONGITUD", SqlDbType.Decimal).Value = propiedades.longitud;
                ObjCommand.Parameters.Add("@REFERENCIA", SqlDbType.VarChar).Value = propiedades.referencia;
                ObjCommand.Parameters.Add("@CONTACTO", SqlDbType.VarChar).Value = propiedades.contacto;
                ObjCommand.Parameters.Add("@CORREO", SqlDbType.VarChar).Value = propiedades.correoElectronico;
                ObjCommand.Parameters.Add("@TELEFONO", SqlDbType.VarChar).Value = propiedades.telefono;
                ObjCommand.Parameters.Add("@EXTENSION", SqlDbType.VarChar).Value = propiedades.extension;

                ObjCommand.Parameters.Add("@ID_EMPRESA_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;

                connection.Open();
                ObjCommand.ExecuteNonQuery();

                int idEmpresaOut = Convert.ToInt16(ObjCommand.Parameters["@ID_EMPRESA_OUT"].Value);
                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);

                int respuestaImagen = 0;
                var nombreArchivo = string.Format(@"{0}", propiedades.idPlantel);
                string rutaArchivo = HttpContext.Current.Server.MapPath(string.Concat("~/Img/Planteles/", idEmpresaOut, "/", nombreArchivo, ".jpg"));

                if (propiedades.urlImagen == "" || propiedades.urlImagen == null)
                {
                    General.EliminarImagen(rutaArchivo, ref respuestaImagen);
                    UrlImagen.Actualizar(connection, pagina, Convert.ToString(propiedades.idPlantel), "");
                }

                else if (propiedades.urlImagen.StartsWith("data:image", System.StringComparison.OrdinalIgnoreCase))
                {
                    General.GuardarImagen(rutaArchivo, propiedades.urlImagen, ref respuestaImagen);
                    if (respuestaImagen == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        string rutaBaseDatos = string.Empty;
                        rutaBaseDatos = string.Concat("/Img/Planteles/", idEmpresaOut, "/", nombreArchivo, ".jpg");

                        UrlImagen.Actualizar(connection, pagina, Convert.ToString(propiedades.idPlantel), rutaBaseDatos);
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
                EscrituraLog.guardar("PlantelDataAccess-Editar. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }
        
        #endregion

        #region "METODOS PARA OBTENER LAS LISTAS QUE SE UTILIZARAN PARA ASIGNAR OFERTAS A PLANTELES"

        public object ArmarFormularioAsignacion(Guid token, string idPlantel, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            int respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_LISTADO_PLANTEL_OFERTA", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@ID_PLANTEL", SqlDbType.VarChar).Value = idPlantel;

                connection.Open();
                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(ObjCommand);

                DataSet ds = new DataSet();
                adapter.Fill(ds);
                miObjetoRespuesta = ObtenerLista(ds, ref respuesta);

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    textoRespuesta = "OK";
                }
                else {
                    Codigo_Respuesta = HttpStatusCode.InternalServerError;
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                    textoRespuesta = "Error Intrnal Server";
                }
                connection.Close();
            }
            catch (Exception e) {
                EscrituraLog.guardar("PlantelDataAccess-ArmarFormularioAsignacion. ", e.Message.ToString());
            }
            finally { connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private FormularioAsignacion ObtenerLista(DataSet ds, ref int respuesta)
        {
            FormularioAsignacion objPropiedades = new FormularioAsignacion();

            DataTable tablaOfertas = ds.Tables[0];
            DataTable tablaAsignadas = ds.Tables[1];
            DataTable nombrePlantel = ds.Tables[2];
            try
            {   objPropiedades.nombrePlantel = nombrePlantel.Rows[0].ItemArray[0].ToString();
                objPropiedades.ofertas = ObtenerOfertas(tablaOfertas);
                objPropiedades.asignadas = ObtenerOfertas(tablaAsignadas);
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e) {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("PlantelDataAccess-ObtenerLista. ", e.Message.ToString());
            }
            return objPropiedades;
        }

        private List<FormularioAsignacionOfertas> ObtenerOfertas(DataTable tabla)
        {
            List<FormularioAsignacionOfertas> objLista = new List<FormularioAsignacionOfertas>();
            try
            {
                string[] columna = { "idGradoEducativo", "gradoEducativo" };
                DataTable dataTable = General.ObtenerRegistrosDiferentes(tabla, columna);

                foreach (DataRow item in dataTable.Rows)
                {
                    objLista.Add(new FormularioAsignacionOfertas
                    {
                        idGradoEducativo = Convert.ToInt32(item["idGradoEducativo"]),
                        nombre = item["gradoEducativo"].ToString(),
                        ofertas = Obtener(tabla, Convert.ToInt32(item["idGradoEducativo"]))
                    });
                }
            }
            catch (Exception e) {
                EscrituraLog.guardar("PlantelDataAccess-ObtenerOfertas. ", e.Message.ToString());
            }
            return objLista;
        }

        private List<DetalleFormularioAsignacion> Obtener(DataTable tabla, int idGradoEducativo)
        {
            List<DetalleFormularioAsignacion> objLista = new List<DetalleFormularioAsignacion>();
            try
            {
                DataRow[] resultado = tabla.Select("idGradoEducativo =" + idGradoEducativo);

                foreach (DataRow registro in resultado)
                {
                    objLista.Add(new DetalleFormularioAsignacion
                    {
                        idOfertaEducativa = Convert.ToInt16(registro["id"]),
                        programaEducativo = registro["programaEducativo"].ToString(),
                        modalidad = registro["modalidad"].ToString(),
                        detalleModalidad = registro["detalleModalidad"].ToString()
                    });
                }
            }
            catch (Exception e) {
                EscrituraLog.guardar("PlantelDataAccess-Obtener. ", e.Message.ToString());
            }
            return objLista;
        }

        #endregion

        #region "METODO PARA GUARDAR O EDITAR LA ASIGNACION DE PLANTELES-OFERTAS"

        public object GuardarAsignacion(PropiedadesAsignacion propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                int respuestaObtrenerCadena = 0;
                string cadenaIDs = string.Empty;

                cadenaIDs = ObtenerCadenaIDs(propiedades.idOfertas, ref respuestaObtrenerCadena);

                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_ASIGNACION_OFERTAS_PLANTELES", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@ID_PLANTEL_UNIQUE", SqlDbType.VarChar).Value = propiedades.idPlantel;
                ObjCommand.Parameters.Add("@ID_OFERTA_EDUCATIVA", SqlDbType.VarChar).Value = cadenaIDs;

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
                EscrituraLog.guardar("PlantelDataAccess-GuardarAsignacion. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private static string ObtenerCadenaIDs(ArrayList propiedades, ref int respuesta)
        {
            string resultado = string.Empty;
            respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            try
            {
                if (propiedades.Count > 0)
                {
                    for (int i = 0; i < propiedades.Count; i++)
                    {
                        if (Convert.ToInt16(propiedades[i]) != 0) { resultado = propiedades[i].ToString() + ',' + resultado; }
                        else { respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError); break; }
                    }
                }
                else { respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.ErrorData); }
            }
            catch (Exception e) {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("PlantelDataAccess-ObtenerCadenaIDs. ", e.Message.ToString());
            }

            resultado = resultado.TrimEnd(',');
            return resultado;
        }

        #endregion
    }
}
