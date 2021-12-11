
//AUTOR: DIEGO OLVERA
//FECHA: 25-08-2019
//DESCRIPCIÓN: CLASE QUE GESTIONA EL CRUD DE LOS MENSAJES

namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using Framework;
    using System.Net;
    using System.Data;
    using Newtonsoft.Json;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;

    public class MensajesDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        public object Guardar(Mensajes propiedades, Guid token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_MENSAJES", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@ID_USUARIO_RECIBE", SqlDbType.VarChar).Value = propiedades.idUsuarioRecibe.Trim();
                ObjCommand.Parameters.Add("@MENSAJE", SqlDbType.VarChar).Value = propiedades.mensaje.Trim();
                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;
                ObjCommand.Parameters.Add("@ID_REFERENCIA", SqlDbType.Int).Direction = ParameterDirection.Output;
                connection.Open();
                ObjCommand.ExecuteNonQuery();
                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);
                int idReferencia = Convert.ToInt32(ObjCommand.Parameters["@ID_REFERENCIA"].Value);

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = idReferencia;
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("MensajesDataAccess-Guardar. ", e.Message.ToString());
            }
            finally { connection.Dispose(); connection.Close(); }

            return miObjetoRespuesta;
        }

        public object ObtenerMensjes(Guid token, string idUsuario, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_MENSAJES", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@ID_USUARIO", idUsuario));
                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<PropiedadesMensajeria> objMensajes = new List<PropiedadesMensajeria>();

                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objMensajes.Add(new PropiedadesMensajeria
                        {
                            idMensajeUnique = objReader["idMensajeUnique"].ToString(),
                            idUsuarioEnvia = Convert.ToInt16(objReader["idUsuarioEnvia"]),
                            idUsuarioRecibe = Convert.ToInt16(objReader["idUsuarioRecibe"]),
                            mensaje = objReader["mensaje"].ToString(),
                            fecha = objReader["fecha"].ToString(),
                            fechaSinFormato = objReader["fechaSinFormato"].ToString(),
                            mensajePropio = Convert.ToInt16(objReader["mensajePropio"])
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objMensajes;
            }
            catch (Exception e)
            {
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("MensajesDataAccess-ObtenerMensjes. ", e.Message.ToString());
            }
            finally { connection.Dispose(); connection.Close(); }

            return miObjetoRespuesta;
        }

        public object ObtenerListaUsuarios(Guid token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_USUARIOS_CHAT", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
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
                EscrituraLog.guardar("MensajesDataAccess-ObtenerListaUsuarios. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private List<nivelUsuarios> ArmarObjetoGeneral(DataSet ds, ref int respuesta)
        {
            respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            List<nivelUsuarios> objNiveles = new List<nivelUsuarios>();

            try
            {
                string nombreNivel = string.Empty;

                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    if (i == 0) { nombreNivel = "Administradores"; }
                    if (i == 1) { nombreNivel = "Profesores"; }
                    if (i == 2) { nombreNivel = "Alumnos"; }

                    objNiveles.Add(new nivelUsuarios
                    {
                        nivel = nombreNivel,
                        usuarios = ObtenerUsuarios(ds, i)
                    });
                }
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("MensajesDataAccess-ArmarObjetoGeneral. ", e.Message.ToString());
            }
            return objNiveles;
        }

        private List<listaUsuariosChat> ObtenerUsuarios(DataSet dataSet, int nivel)
        {
            List<listaUsuariosChat> objLista = new List<listaUsuariosChat>();
            DataTable listaGeneral = dataSet.Tables[nivel];
            try
            {
                foreach (DataRow item in listaGeneral.Rows)
                {
                    objLista.Add(new listaUsuariosChat
                    {
                        idUsuario = item["idUsuarioUnique"].ToString(),
                        nombre = item["nombre"].ToString(),
                        imagen = item["imagen"].ToString()
                    });
                }
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("MensajesDataAccess-ObtenerUsuarios. ", e.Message.ToString());
            }
            return objLista;
        }
    }
}
