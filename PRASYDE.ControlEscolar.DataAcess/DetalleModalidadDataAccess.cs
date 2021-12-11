
//AUTOR: DIEGO OLVERA
//FECHA: 08-04-2019
//DESCRIPCIÓN: CLASE QUE GESTIONA EL CRUD DEL DETALLE DE MODALIDADDES

namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using System.Net;
    using System.Data;
    using Newtonsoft.Json;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.DataAcess.Framework;

    public class DetalleModalidadDataAccess : BaseDataAccess
    {

        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        public object Listado(Guid token, int estatus,int idModalidad, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_LISTADO_DETALLE_MODALIDADES", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@ID_MODALIDAD", idModalidad));
                ObjCommand.Parameters.Add(new SqlParameter("@ESTATUS", estatus));

                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<ListadoDetalleModalidades> objModalidad = new List<ListadoDetalleModalidades>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objModalidad.Add(new ListadoDetalleModalidades
                        {
                            idDetalleModalidad = Convert.ToInt16(objReader["idDetalleModalidad"]),
                            idModalidad = Convert.ToInt16(objReader["idModalidad"].ToString()),
                            modalidad = objReader["modalidad"].ToString(),
                            nombre = objReader["detalleModalidad"].ToString(),
                            descripcion = objReader["descripcion"].ToString(),
                            estatus = Convert.ToInt16(objReader["estatus"])
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objModalidad;
            }
            catch (Exception e) {
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("DetalleModalidadDataAccess-Listado. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Guardar(DetalleModalidad propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_DETALLE_MODALIDADES", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@ID_MODALIDAD", SqlDbType.Int).Value = propiedades.idModalidad;
                ObjCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = propiedades.nombre.Trim();
                ObjCommand.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = propiedades.descripcion.Trim();
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
                EscrituraLog.guardar("DetalleModalidadDataAccess-Guardar. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Obtener(Guid token, string pagina, int idDetalleModalidad, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_DETALLE_MODALIDAD", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@PAGINA", pagina));
                ObjCommand.Parameters.Add(new SqlParameter("@ID_DETALLE_MODALIDAD", idDetalleModalidad));

                connection.Open();
                DetalleModalidad objModalidad = new DetalleModalidad();
                SqlDataReader objReader = ObjCommand.ExecuteReader();

                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objModalidad.idDetalleModalidad = Convert.ToInt32(objReader["idDetalleModalidad"]);
                        objModalidad.idModalidad = Convert.ToInt16(objReader["idModalidad"]);
                        objModalidad.nombre = objReader["nombre"].ToString();
                        objModalidad.descripcion = objReader["descripcion"].ToString();
                    }
                }
                objReader.Close();

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objModalidad;
            }
            catch (Exception e) {
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("DetalleModalidadDataAccess-Obtener. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Editar(DetalleModalidad propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_EDITAR_DETALLE_MODALIDAD", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@ID_DETALLE_MODALIDAD", SqlDbType.Int).Value = propiedades.idDetalleModalidad;
                ObjCommand.Parameters.Add("@ID_MODALIDAD", SqlDbType.Int).Value = propiedades.idModalidad;
                ObjCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = propiedades.nombre.Trim();
                ObjCommand.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = propiedades.descripcion.Trim();
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
                EscrituraLog.guardar("DetalleModalidadDataAccess-Editar. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }
    }
}
