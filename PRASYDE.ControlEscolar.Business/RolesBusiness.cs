//AUTOR: DIEGO OLVERA
//FECHA: 04-04-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA ARMAR EL FORMULARIO DE ROLES

namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using Framework;
    using DataAcess;
    using System.Net;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;

    public class RolesBusiness
    {
        public static RespuestaGeneral ObtenerDatosFormulario(ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            CatalogosDataAccess objJerarquias = new CatalogosDataAccess();
            CatalogosDataAccess objPermisos = new CatalogosDataAccess();
            RolesDataAccess objModulos = new RolesDataAccess();

            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            FormularioRoles objListaRoles = new FormularioRoles();

            PropiedadesCatalogos filtroGeneral;

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                filtroGeneral = new PropiedadesCatalogos() { id = 0, cadenaIds = "", nombrePagina = "Jerarquias" };
                ObjetoRespuesta = objJerarquias.ObtenerCatalogo(token, filtroGeneral, ref Codigo_Respuesta);

                objListaRoles.jerarquias = (List<ListaGenericaCatalogos>)ObjetoRespuesta.result;
                if (objJerarquias.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    filtroGeneral = new PropiedadesCatalogos() { id = 0, cadenaIds = "", nombrePagina = "Permisos" };
                    ObjetoRespuesta = objPermisos.ObtenerCatalogo(token, filtroGeneral, ref Codigo_Respuesta);

                    objListaRoles.permisos = (List<ListaGenericaCatalogosTresValores>)ObjetoRespuesta.result;
                    if (objPermisos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        objListaRoles.modulos = objModulos.Obtener(token, ref Codigo_Respuesta);
                        if (objJerarquias.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                        {
                            Codigo_Respuesta = HttpStatusCode.OK;
                            mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                            codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                            Resultado = objListaRoles;
                        }
                        else {
                            mensaje = objModulos.textoRespuesta;
                            codigoRespuesta = objModulos.codigoRespuesta;
                            Resultado = new object { };
                        }
                    }
                    else {
                        mensaje = objPermisos.textoRespuesta;
                        codigoRespuesta = objPermisos.codigoRespuesta;
                        Resultado = new object { };
                    }
                }
                else {
                    mensaje = objJerarquias.textoRespuesta;
                    codigoRespuesta = objJerarquias.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Guardar(Roles propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            RolesDataAccess objRoles = new RolesDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objRoles.GuardarRol(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objRoles.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objRoles.textoRespuesta;
                    codigoRespuesta = objRoles.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Listado(ref HttpStatusCode Codigo_Respuesta, int estatus)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            string pagina = string.Empty;
            string paginaOrigen = string.Empty;

            RolesDataAccess objRoles = new RolesDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            GeneralDataAccess objetoGeneral = new GeneralDataAccess();

            ListadoPermisosRoles objListaRoles = new ListadoPermisosRoles();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina, ref paginaOrigen))
            {
                objListaRoles.permisos = (List<ListaGenericaCatalogos>)objetoGeneral.Permisos(token, pagina, paginaOrigen, ref Codigo_Respuesta);
                if (objetoGeneral.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    objListaRoles.roles = (List<ListadoRoles>)objRoles.Listado(token, estatus, ref Codigo_Respuesta);
                    if (objRoles.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                        codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                        Resultado = objListaRoles;
                    }
                    else {
                        mensaje = objRoles.textoRespuesta;
                        codigoRespuesta = objRoles.codigoRespuesta;
                        Resultado = new object { };
                    }
                }
                else {
                    mensaje = objetoGeneral.textoRespuesta;
                    codigoRespuesta = objetoGeneral.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.result = objRoles.Listado(token, estatus, ref Codigo_Respuesta);

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Obtener(ref HttpStatusCode Codigo_Respuesta, string idRol)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            RolesDataAccess objRol = new RolesDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objRol.ObtenerRol(token, pagina, idRol, ref Codigo_Respuesta);

                if (objRol.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objRol.textoRespuesta;
                    codigoRespuesta = objRol.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Editar(Roles propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            RolesDataAccess objRoles = new RolesDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objRoles.Editar(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objRoles.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objRoles.textoRespuesta;
                    codigoRespuesta = objRoles.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static bool ValidarDatos(Roles propiedades, int tipo)
        {
            try
            {
                bool respusta = true;
                if (propiedades == null) { respusta = false; return respusta; }
                if (tipo == 2)
                {
                    if (propiedades.idRol == "") { respusta = false; return respusta; }
                }
                if (Seguridad.TextoSimpleObligatorio(propiedades.nombre, 1, 50) == false) { respusta = false; return respusta; }
                if (Seguridad.TextoSimpleNoObligatorio(propiedades.descripcion, 1, 250) == false) { respusta = false; return respusta; }
                return respusta;
            }
            catch (Exception exception) { throw exception; }
        }
    }
}
