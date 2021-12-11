
//AUTOR: DIEGO OLVERA
//FECHA: 29-03-2019
//DESCRIPCIÓN: CLASE QUE REGRESA UNA LISTA CON LOS ELEMENTOS PARA ARMAR EN MENU 

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

    public class MenuDataAcceess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        public object ObtenerMenu(string token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_MENU", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));

                connection.Open();
                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(ObjCommand);

                DataSet ds = new DataSet();
                adapter.Fill(ds);

                int respuesta = 0;
                object objetoFinal = new object();
                objetoFinal = ArmarObjetoGeneral(ds, ref respuesta);

                if (respuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    textoRespuesta = "OK";
                    miObjetoRespuesta = objetoFinal;
                }
                else {
                    Codigo_Respuesta = HttpStatusCode.InternalServerError;
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                    textoRespuesta = "Internal Server Error";
                }
            }
            catch (Exception e) {
                EscrituraLog.guardar("MenuDataAcceess-ObtenerMenu. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private ObjetoMenu ArmarObjetoGeneral(DataSet ds, ref int respuesta)
        {
            ObjetoMenu objPropiedades = new ObjetoMenu();

            try
            {
                DataTable nombreMenu = ds.Tables[0];
                DataTable modulos = ds.Tables[1];

                objPropiedades.menu = Obtener(nombreMenu, modulos);
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("MenuDataAcceess-ArmarObjetoGeneral. ", e.Message.ToString());
            }
            return objPropiedades;
        }

        private List<PropiedadesMenu> Obtener(DataTable headerMenu, DataTable modulos)
        {
            List<PropiedadesMenu> objeto = new List<PropiedadesMenu>();
            try
            {
                //SE ARMA EL PRRIMER ELEMENTO DEL MENU "INICOO" GENERAL PARA TODOS
                objeto.Add(new PropiedadesMenuIcon
                {
                    url = "/",
                    name = "Inicio",
                    slug = "home",
                    icon = "insert_chart"
                });

                objeto.Add(new PropiedadesMenuIcon
                {
                    url = "/listaAvisos",
                    name = "Avisos",
                    slug = "avisos",
                    icon = "view_headline"
                });

                objeto.Add(new HeaderMenu { header = (headerMenu.Rows[0].ItemArray[0]).ToString() });

              
                DataRow[] resultado = modulos.Select("idSubModulo = 0");

                foreach (DataRow registro in resultado)
                {
                    objeto.Add(new PropiedadesMenuIcon
                    {
                        name = registro["nombreModulo"].ToString(),
                        url = registro["urlPagina"].ToString(),
                        slug = registro["nombreModulo"].ToString(),
                        icon = registro["estilo"].ToString(),
                        submenu = ObtenerSubMenu(modulos, Convert.ToInt16(registro["idModulo"]))
                    });
                }
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("MenuDataAcceess-Obtener. ", e.Message.ToString());
            }
            return objeto;
        }

        private List<PropiedadesMenu> ObtenerSubMenu(DataTable modulos, int idModulo)
        {
            List<PropiedadesMenu> objSubMenu = new List<PropiedadesMenu>();
            DataRow[] resultado = modulos.Select("idSubModulo =" + idModulo);

            foreach (DataRow registro in resultado)
            {
                objSubMenu.Add(new PropiedadesMenu
                {
                    name = registro["nombreModulo"].ToString(),
                    url = registro["urlPagina"].ToString(),
                    slug = registro["nombreModulo"].ToString()
                });
            }
            return objSubMenu;
        }
    }
}
