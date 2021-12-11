
//AUTOR: DIEGO OLVERA
//FECHA: 08-04-2019
//DESCRIPCIÓN: CLASE QUE GESTIONA LA OFERTA EDUCATIVA

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
   
    public class OfertaEducativaDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        public object Listado(Guid token, int estatus,int tipo, int idPlantel, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                int tipoListado = 1;
                if (idPlantel != 0) { tipoListado = 2; }
                SqlCommand ObjCommand = new SqlCommand("USP_LISTADO_OFERTA_EDUCATIVA", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@TIPO", tipo));
                ObjCommand.Parameters.Add(new SqlParameter("@TIPO_LISTADO", tipoListado));
                ObjCommand.Parameters.Add(new SqlParameter("@ID_PLANTEL", idPlantel));
                ObjCommand.Parameters.Add(new SqlParameter("@ESTATUS", estatus));

                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<ListadoOfertaEducativa> objOferta = new List<ListadoOfertaEducativa>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objOferta.Add(new ListadoOfertaEducativa
                        {
                            idOfertaEducativa = objReader["idOfertaEducativa"].ToString(),
                            gradoEducativo = objReader["gradoEducativo"].ToString(),
                            programaEducativo = objReader["programaEducativo"].ToString(),
                            modalidad = objReader["modalidad"].ToString(),
                            detalleModalidad = objReader["detalleModalidad"].ToString(),
                            estatus = Convert.ToInt16(objReader["estatus"])
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objOferta;
            }
            catch (Exception e) {
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("OfertaEducativaDataAccess-Listado. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Guardar(OfertaEducativa propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_OFERTA_EDUCATIVA", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@ID_PROGRAMA_EDUCATIVO", SqlDbType.VarChar).Value = propiedades.idProgramaEducativo.Trim();
                ObjCommand.Parameters.Add("@ID_DETALLE_MODALIDAD", SqlDbType.Int).Value = propiedades.idDetalleModalidad;
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
            catch (Exception e)
            {
                EscrituraLog.guardar("OfertaEducativaDataAccess-Guardar. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Obtener(Guid token, string pagina, string idOfertaEducativa, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_OFERTA_EDUCATIVA", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@PAGINA", pagina));
                ObjCommand.Parameters.Add(new SqlParameter("@ID_OFERTA_UNIQUE", idOfertaEducativa));

                connection.Open();
                OfertaEducativa objOferta = new OfertaEducativa();
                SqlDataReader objReader = ObjCommand.ExecuteReader();

                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objOferta.idOfertaEducativa = objReader["idOfertaEducativa"].ToString();
                        objOferta.idProgramaEducativo = objReader["idProgramaEducativo"].ToString();
                        objOferta.idDetalleModalidad = Convert.ToInt16(objReader["idDetalleModalidad"]);
                        objOferta.idGradoEducativo = Convert.ToInt16(objReader["idGradoEducativo"]);
                        objOferta.idModalidad = Convert.ToInt16(objReader["idModalidad"]);
                        objOferta.nombreGradoEducativo = objReader["nombreGradoEducativo"].ToString();
                        objOferta.nombreProgramaEducativo = objReader["nombreProgramaEducativo"].ToString();
                        objOferta.nombreModalidad = objReader["nombreModalidad"].ToString();
                        objOferta.nombreDetalleModalidad = objReader["nombreDetalleModalidad"].ToString();
                    }
                }
                objReader.Close();
                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objOferta;
            }
            catch (Exception e)
            {
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("OfertaEducativaDataAccess-Obtener. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Editar(OfertaEducativa propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_EDITAR_OFERTA_EDUCATIVA", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;
                ObjCommand.Parameters.Add("@ID_OFERTA_EDUCATIVA", SqlDbType.VarChar).Value = propiedades.idOfertaEducativa;
                ObjCommand.Parameters.Add("@ID_PROGRAMA_EDUCATIVO", SqlDbType.Int).Value = propiedades.idProgramaEducativo;
                ObjCommand.Parameters.Add("@ID_DETALLE_MODALIDAD", SqlDbType.Int).Value = propiedades.idDetalleModalidad;
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
            catch (Exception e)
            {
                EscrituraLog.guardar("OfertaEducativaDataAccess-Editar. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        #region "METODDOS PARA ARMAR EL FORMULARIO DE OFERTAS EDUCATIVAS"

        public object ArmarFormualario(Guid token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            int respuesta = 200;
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_LISTAS_OFERTA_EDUCATIVA", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;

                connection.Open();
                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(ObjCommand);

                DataSet ds = new DataSet();
                adapter.Fill(ds);
                miObjetoRespuesta = ObtenerListaGrados(ds, ref respuesta);

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
            }
            catch (Exception e) {
                EscrituraLog.guardar("OfertaEducativaDataAccess-ArmarFormualario. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private ObtenerListaOferta ObtenerListaGrados(DataSet ds, ref int respuesta)
        {
            ObtenerListaOferta objPropiedades = new ObtenerListaOferta();

            DataTable tablaGrados = ds.Tables[0];
            DataTable tablaModalidades = ds.Tables[1];

            try
            {
                objPropiedades.gradoEducativo = ObtenerGradosEducativos(tablaGrados);
                objPropiedades.modalidades = ObtenerModalidades(tablaModalidades);
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e) {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("OfertaEducativaDataAccess-ObtenerListaGrados. ", e.Message.ToString());
            }
            return objPropiedades;
        }

        private List<ListaGradoEducativo> ObtenerGradosEducativos(DataTable tablaGrados)
        {
            List<ListaGradoEducativo> objLista = new List<ListaGradoEducativo>();
            try
            {
                string[] columna = { "idGradoEducativo", "nombreGrado" };
                DataTable dataTable = General.ObtenerRegistrosDiferentes(tablaGrados, columna);

                foreach (DataRow item in dataTable.Rows)
                {
                    objLista.Add(new ListaGradoEducativo
                    {
                        idGradoEducativo = Convert.ToInt16(item["idGradoEducativo"]),
                        nombreGrado = item["nombreGrado"].ToString(),
                        programaEducativo = ObtenerPrograma(tablaGrados, Convert.ToInt16(item["idGradoEducativo"]))
                    });
                }

            }
            catch (Exception e) {
                EscrituraLog.guardar("OfertaEducativaDataAccess-ObtenerGradosEducativos. ", e.Message.ToString());
            }
            return objLista;
        }

        private List<ListaProgramaEducativo> ObtenerPrograma(DataTable tablaGrados, int idGradoEducativo)
        {
            List<ListaProgramaEducativo> objLista = new List<ListaProgramaEducativo>();
            try
            {
                DataRow[] resultado = tablaGrados.Select("idGradoEducativo =" + idGradoEducativo);

                foreach (DataRow registro in resultado)
                {
                    objLista.Add(new ListaProgramaEducativo
                    {
                        idGradoEducativo = Convert.ToInt16(registro["idGradoEducativo"]),
                        idProgramaEducativo = registro["idProgramaEducativo"].ToString(),
                        nombrePrograma = registro["nombrePrograma"].ToString(),
                        requisitos = registro["requisitos"].ToString()
                    });
                }
            }
            catch (Exception e) {
                EscrituraLog.guardar("OfertaEducativaDataAccess-ObtenerPrograma. ", e.Message.ToString());
            }
            return objLista;
        }
        
        private List<ListaModalidades> ObtenerModalidades(DataTable tablaModalidad)
        {
            List<ListaModalidades> objLista = new List<ListaModalidades>();
            try
            {
                string[] columna = { "idModalidad", "nombreModalidad" };
                DataTable dataTable = General.ObtenerRegistrosDiferentes(tablaModalidad, columna);

                foreach (DataRow item in dataTable.Rows)
                {
                    objLista.Add(new ListaModalidades
                    {
                        idModalidad = Convert.ToInt16(item["idModalidad"]),
                        nombreModalidad = item["nombreModalidad"].ToString(),
                        detalleModalidad = ObtenerDetalleModalidad(tablaModalidad, Convert.ToInt16(item["idModalidad"]))
                    });
                }
            }
            catch (Exception e) {
                EscrituraLog.guardar("OfertaEducativaDataAccess-ObtenerModalidades. ", e.Message.ToString());
            }
            return objLista;
        }

        private List<ListaDetalleModalidad> ObtenerDetalleModalidad(DataTable tablaModalidad, int idModalidad)
        {
            List<ListaDetalleModalidad> objLista = new List<ListaDetalleModalidad>();
            try
            {
                DataRow[] resultado = tablaModalidad.Select("idModalidad =" + idModalidad);

                foreach (DataRow registro in resultado)
                {
                    objLista.Add(new ListaDetalleModalidad
                    {
                        idDetalleModalidad = Convert.ToInt16(registro["idDetalleModalidad"]),
                        nombreDetalle = registro["nombreDetalle"].ToString(),
                        idModalidad = Convert.ToInt16(registro["idModalidad"]),
                    });
                }
            }
            catch (Exception e) {
                EscrituraLog.guardar("OfertaEducativaDataAccess-ObtenerDetalleModalidad. ", e.Message.ToString());
            }
            return objLista;
        }

        #endregion
     
    }
}
