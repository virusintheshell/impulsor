
//AUTOR: DIEGO OLVERA
//FECHA: 05-04-2019
//DESCRIPCIÓN: CLASE QUE REGRESA UNA LISTA DE LOS MODULOS GENERALES PARA ARMAR EL FORMULARIO DE ROLES


namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using Framework;
    using System.Net;
    using System.Data;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;
    
    public class RolesDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        #region "METODOS PARA ARMAR EL FORMULARIO DE ROLES"

        public List<listaModulos> Obtener(Guid token, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            RespuestaGeneral miObjetoRespuesta = new RespuestaGeneral();

            List<listaModulos> objRespuesta = new List<listaModulos>();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_MENU_ROLES", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;

                DataTable objDataTable = new DataTable();
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(ObjCommand))
                {
                    dataAdapter.Fill(objDataTable);
                    if (objDataTable.Rows.Count != 0)
                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                        textoRespuesta = "OK";
                        objRespuesta = obtenerListaModulos(objDataTable);
                    }
                }
            }
            catch (Exception e)
            {
                Codigo_Respuesta = HttpStatusCode.InternalServerError;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                textoRespuesta = "Internal Server Error";
                EscrituraLog.guardar("RolesDataAccess-Obtener. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return objRespuesta;
        }

        private List<listaModulos> obtenerListaModulos(DataTable DataTableInfo)
        {
            List<listaModulos> objListaModulos = new List<listaModulos>();

            try
            {
                string[] columna = { "idModuloPrincipal", "idModulo", "nombreModulo", "idPlataforma", "estilo" };
                DataTable dataTable = RegistrosDiferentes.Obtener(DataTableInfo, columna);

                DataRow[] modulosPrincipales = dataTable.Select("idModuloPrincipal = 0");
                int idModulo = 0;

                foreach (DataRow row in modulosPrincipales)
                {
                    idModulo = Convert.ToInt32(row["idModulo"]);

                    objListaModulos.Add(new listaModulos
                    {
                        idModuloPrincipal = idModulo,
                        Modulo = row["nombreModulo"].ToString(),
                        idPlataforma = Convert.ToInt16(1),
                        estilo = row["estilo"].ToString(),
                        submodulos = ObtenerSubModulos(DataTableInfo, idModulo)
                    });
                }
            }
            catch (Exception e) {
                EscrituraLog.guardar("RolesDataAccess-obtenerListaModulos. ", e.Message.ToString());
            }

            return objListaModulos;
        }

        //FUNCION QUE OBTIEN LOS SUBMODULOS Y SE LLAMA OTRA FUNCION PARA OBTENER LOS PERMISOS POR SUBMODULO
        private List<PropiedadesSubModulos> ObtenerSubModulos(DataTable DataTableInfo, int idModulo)
        {
            List<PropiedadesSubModulos> objPropiedadesSubModulos = new List<PropiedadesSubModulos>();

            DataRow[] subModulos = DataTableInfo.Select("idModuloPrincipal =" + idModulo + "");

            int idSubModulo = 0;
            foreach (DataRow row1 in subModulos)
            {
                if (idSubModulo != Convert.ToInt32(row1[0])) //SE ARMA UNA LISTA CON EL NOMBRE DEL SUBMODULO 
                {
                    idSubModulo = Convert.ToInt32(row1[0]);
                    objPropiedadesSubModulos.Add(new PropiedadesSubModulos
                    {
                        idModuloPrincipal = idModulo,
                        idSubModulo = Convert.ToInt32(row1[0]),
                        nombreSubModulo = row1["nombreModulo"].ToString(),
                        permisos = ObtenerPermisos(DataTableInfo, idSubModulo) //SE OBTIENEN LOS PERMISOS POR SUBMODULO
                    });
                }
            }
            return objPropiedadesSubModulos;
        }

        //FUNCION QUE REGRESA LOS PERMISOS POR MODULO Y SUBMODULO
        //SE PASA COMO PARAMETRO EL DATATABLE Y EL ID DEL SUBMODULO 
        private List<PropiedadesPermisos> ObtenerPermisos(DataTable DataTableInfo, int idModulo)
        {
            List<PropiedadesPermisos> objPropiedadesPermisos = new List<PropiedadesPermisos>();
            DataRow[] subModulos = DataTableInfo.Select("idModulo =" + idModulo + "");

            foreach (DataRow row1 in subModulos)
            {
                objPropiedadesPermisos.Add(new PropiedadesPermisos //PERMISOS POR SUBMODULO
                {
                    idDetalleModuloPermiso = Convert.ToInt32(row1["idDetalleModuloPermiso"]),
                    idPermiso = Convert.ToInt32(row1["idPermiso"]),
                    permiso = row1["nombrePermiso"].ToString()
                });
            }
            return objPropiedadesPermisos;
        }

        #endregion

        #region "METODOS PARA EL CRUD DE ROLES"

        public object GuardarRol(Roles propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {

            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_ROLES", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@ID_JERARQUIA", SqlDbType.Int).Value = propiedades.idJerarquia;
                ObjCommand.Parameters.Add("@NOMBRE_ROL", SqlDbType.VarChar).Value = propiedades.nombre;
                ObjCommand.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = propiedades.descripcion;

                string cadenaPermisos = string.Empty;
                ArmarCadena(propiedades.cadenaModuloPermiso, ref cadenaPermisos);
                ObjCommand.Parameters.Add("@CADENA_MODULOPERMISO", SqlDbType.VarChar).Value = cadenaPermisos;

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
                EscrituraLog.guardar("RolesDataAccess-GuardarRol. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object Listado(Guid token, int estatus, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_LISTADO_ROLES", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add(new SqlParameter("@TOKEN", token));
                ObjCommand.Parameters.Add(new SqlParameter("@ESTATUS", estatus));

                connection.Open();

                SqlDataReader objReader = ObjCommand.ExecuteReader();
                List<ListadoRoles> objRoles = new List<ListadoRoles>();
                if (objReader.HasRows)
                {
                    while (objReader.Read())
                    {
                        objRoles.Add(new ListadoRoles
                        {
                            idRol = objReader["idRol"].ToString(),
                            nombre = objReader["nombre"].ToString(),
                            jerarquia = objReader["jerarquia"].ToString(),
                            descripcion = objReader["descripcion"].ToString(),
                            estatus = Convert.ToInt16(objReader["estatus"])
                        });
                    }
                    objReader.Close();
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objRoles;
            }
            catch (Exception e) {
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("RolesDataAccess-Listado. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        public object ObtenerRol(Guid token, string pagina, string idRol, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_OBTENER_ROL", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;
                ObjCommand.Parameters.Add("@ID_ROL", SqlDbType.VarChar).Value = idRol;
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
            catch (Exception e) {
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("RolesDataAccess-ObtenerRol. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }
        
        private ObtenerRol ArmarObjetoGeneral(DataSet ds, ref int respuesta)
        {
            ObtenerRol objRol = new ObtenerRol();
            try
            {
                DataTable datosGenerales = ds.Tables[0];

                objRol.idRol = datosGenerales.Rows[0].ItemArray[0].ToString();
                objRol.idJerarquia = Convert.ToInt16(datosGenerales.Rows[0].ItemArray[1]);
                objRol.nombre = datosGenerales.Rows[0].ItemArray[2].ToString();
                objRol.descripcion = datosGenerales.Rows[0].ItemArray[3].ToString();
                objRol.cadenaModuloPermiso = ObtenerDetalle(datosGenerales);
                objRol.detalleModulo = ObtenerListaDetalle(datosGenerales);
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("RolesDataAccess-ArmarObjetoGeneral. ", e.Message.ToString());
            }
            return objRol;
        }

        private List<DetalleObtenerRol> ObtenerDetalle(DataTable roles)
        {
            List<DetalleObtenerRol> objLista = new List<DetalleObtenerRol>();
         
            try
            {
                string[] columna = { "idModulo", };
                DataTable dataTable = General.ObtenerRegistrosDiferentes(roles, columna);

                if (dataTable.Rows.Count >= 1)
                {
                    foreach (DataRow item in dataTable.Rows)
                    {
                        objLista.Add(new DetalleObtenerRol
                        {
                            idModulo = Convert.ToInt16(item["idModulo"]),
                            permisos = ObtenerListaPermisos(roles, Convert.ToInt16(item["idModulo"])),
                        });
                    }
                }
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("RolesDataAccess-ObtenerDetalle. ", e.Message.ToString());
            }
          
            return objLista;
        }
        
        private ArrayList ObtenerListaPermisos(DataTable permisos, int idModulo)
        {
            ArrayList objLista = new ArrayList();
            try
            {
                DataRow[] resultado = permisos.Select("idModulo =" + idModulo);

                foreach (DataRow registro in resultado)
                {
                    objLista.Add(Convert.ToInt16(registro["idPermiso"]));
                }
            }
            catch (Exception e) {
                EscrituraLog.guardar("RolesDataAccess-ObtenerListaPermisos. ", e.Message.ToString());
            }
            return objLista;
        }

        private ArrayList ObtenerListaDetalle(DataTable permisos)
        {
            ArrayList objLista = new ArrayList();
            try
            {
                if (permisos.Rows.Count > 1)
                {
                    foreach (DataRow registro in permisos.Rows)
                    {
                        objLista.Add(Convert.ToInt16(registro["idDetalleModuloPermiso"]));
                    }
                }
            }
            catch (Exception e) {
                EscrituraLog.guardar("RolesDataAccess-ObtenerListaDetalle. ", e.Message.ToString());
            }
            return objLista;
        }

        public object Editar(Roles propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_EDITAR_ROL", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@ID_ROL", SqlDbType.VarChar).Value = propiedades.idRol;
                ObjCommand.Parameters.Add("@ID_JERARQUIA", SqlDbType.Int).Value = propiedades.idJerarquia;
                ObjCommand.Parameters.Add("@NOMBRE_ROL", SqlDbType.VarChar).Value = propiedades.nombre;
                ObjCommand.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = propiedades.descripcion;

                string cadenaPermisos = string.Empty;
                ArmarCadena(propiedades.cadenaModuloPermiso, ref cadenaPermisos);
                ObjCommand.Parameters.Add("@CADENA_MODULOPERMISO", SqlDbType.VarChar).Value = cadenaPermisos;

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
                EscrituraLog.guardar("RolesDataAccess-ObtenerListaDetalle. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private void ArmarCadena(ArrayList LsitaPermisos, ref string cadenaPermisos)
        {
            try
            {
                if (LsitaPermisos.Count > 0)
                {
                    for (int i = 0; i < LsitaPermisos.Count; i++)
                    {
                        if (Convert.ToInt16(LsitaPermisos[i]) != 0)
                        {
                            cadenaPermisos = LsitaPermisos[i].ToString() + ',' + cadenaPermisos;
                        }
                        else { break; }
                    }
                }
                cadenaPermisos = cadenaPermisos.TrimEnd(',');
            }
            catch (Exception e) {
                EscrituraLog.guardar("RolesDataAccess-ArmarCadena. ", e.Message.ToString());
            }
        }
        
        #endregion
    }
}
