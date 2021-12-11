
//AUTOR: DIEGO OLVERA
//FECHA: 04-04-2019
//DESCRIPCIÓN: CLASE QUE REGRESA UNA LISTA GENRICA DE CATALOGOS

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

    public class CatalogosDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        public RespuestaGeneral ObtenerCatalogo(Guid token, PropiedadesCatalogos propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            RespuestaGeneral miObjetoRespuesta = new RespuestaGeneral();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_CATALOGOS", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = propiedades.nombrePagina;
                ObjCommand.Parameters.Add("@ID", SqlDbType.Int).Value = propiedades.id;
                ObjCommand.Parameters.Add("@IDS", SqlDbType.VarChar).Value = propiedades.cadenaIds;

                DataTable objDataTable = new DataTable();
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(ObjCommand))
                {
                    dataAdapter.Fill(objDataTable);

                    if (propiedades.nombrePagina == "Roles" || propiedades.nombrePagina == "Permisos" || propiedades.nombrePagina == "ProgramaEducativo" || propiedades.nombrePagina == "DetalleModalidad" || propiedades.nombrePagina == "Inscripcion" || propiedades.nombrePagina == "GradoEducativo") { miObjetoRespuesta.result = ListaTresValoresGenerica(objDataTable); }
                    else { miObjetoRespuesta.result = ListaGenerica(objDataTable); }
                }
                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
            }
            catch (Exception e)
            {
                Codigo_Respuesta = HttpStatusCode.InternalServerError;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                textoRespuesta = "Internal Server Error";
                EscrituraLog.guardar("CatalogosDataAccess-ObtenerCatalogo. ", e.Message.ToString());
            }
            finally { connection.Close();  connection.Dispose(); }

            return miObjetoRespuesta;
        }

        //LISTA DE DOS VALORES 
        private List<ListaGenericaCatalogos> ListaGenerica(DataTable datosGenerales)
        {
            List<ListaGenericaCatalogos> ListaCatalgosGeneral = new List<ListaGenericaCatalogos>();
            try
            {
                foreach (DataRow row in datosGenerales.Rows)
                {
                    ListaCatalgosGeneral.Add(new ListaGenericaCatalogos
                    {
                        id = Convert.ToInt16(row["id"]),
                        nombre = row["nombre"].ToString()
                    });
                }
            }
            catch (Exception e) {
                EscrituraLog.guardar("CatalogosDataAccess-ListaGenerica. ", e.Message.ToString());
            }

            return ListaCatalgosGeneral;
        }

        //LISTA DE TRES VALORES 
        private List<ListaGenericaCatalogosTresValores> ListaTresValoresGenerica(DataTable datosGenerales)
        {
            List<ListaGenericaCatalogosTresValores> ListaCatalgosGeneral = new List<ListaGenericaCatalogosTresValores>();
            try
            {
                foreach (DataRow row in datosGenerales.Rows)
                {
                    ListaCatalgosGeneral.Add(new ListaGenericaCatalogosTresValores
                    {
                        id = Convert.ToInt16(row["id"]),
                        nombre = row["nombre"].ToString(),
                        valor = row["valor"].ToString()
                    });
                }
            }
            catch (Exception e) {
                EscrituraLog.guardar("CatalogosDataAccess-ListaCatalgosGeneral. ", e.Message.ToString());
            }

            return ListaCatalgosGeneral;
        }
    }
}
