
//AUTOR: DIEGO OLVERA
//FECHA: 09-04-2019
//DESCRIPCIÓN: CLASE PARA METODOS GENERALES QUE SE UTILIZAN EN TODO EL SISTEMA. EJEMPLO: FILTRO CATALOGOS

namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using DataAcess;
    using Framework;
    using System.Net;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;

    public class GeneralBusiness
    {
        public static RespuestaGeneral ObtenerLista(ref HttpStatusCode Codigo_Respuesta, int id)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            PropiedadesCatalogos filtroGeneral;
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            CatalogosDataAccess objProgramaEducativo = new CatalogosDataAccess();
            FormularioCatalogoGeneral objListaGeneral = new FormularioCatalogoGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                filtroGeneral = new PropiedadesCatalogos() { id = id, cadenaIds = "", nombrePagina = pagina };
                ObjetoRespuesta = objProgramaEducativo.ObtenerCatalogo(token, filtroGeneral, ref Codigo_Respuesta);

                objListaGeneral.listado = (List<ListaGenericaCatalogos>)ObjetoRespuesta.result;
                if (objProgramaEducativo.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = objListaGeneral;
                }
                else {
                    mensaje = objProgramaEducativo.textoRespuesta;
                    codigoRespuesta = objProgramaEducativo.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ObtenerListaTresValores(ref HttpStatusCode Codigo_Respuesta, int id)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            PropiedadesCatalogos filtroGeneral;
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            CatalogosDataAccess objProgramaEducativo = new CatalogosDataAccess();
            FormularioCatalogoGeneralTresValores objListaGeneral = new FormularioCatalogoGeneralTresValores();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                filtroGeneral = new PropiedadesCatalogos() { id = id, cadenaIds = "", nombrePagina = pagina };
                ObjetoRespuesta = objProgramaEducativo.ObtenerCatalogo(token, filtroGeneral, ref Codigo_Respuesta);

                objListaGeneral.listado = (List<ListaGenericaCatalogosTresValores>)ObjetoRespuesta.result;
                if (objProgramaEducativo.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = objListaGeneral;
                }
                else {
                    mensaje = objProgramaEducativo.textoRespuesta;
                    codigoRespuesta = objProgramaEducativo.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Cambiar(ref HttpStatusCode Codigo_Respuesta, string id)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            GeneralDataAccess objGeneral = new GeneralDataAccess();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objGeneral.Cambiar(token, pagina, id, ref Codigo_Respuesta);
                if (objGeneral.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = new object { };
                }
                else {
                    mensaje = objGeneral.textoRespuesta;
                    codigoRespuesta = objGeneral.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Eliminar(ref HttpStatusCode Codigo_Respuesta, string id)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            GeneralDataAccess objGeneral = new GeneralDataAccess();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objGeneral.Eliminar(token, pagina, id, ref Codigo_Respuesta);
                if (objGeneral.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = new object { };
                }
                else {
                    mensaje = objGeneral.textoRespuesta;
                    codigoRespuesta = objGeneral.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral AccesoFormulario(ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();
            string pagina = string.Empty;

            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            GeneralDataAccess objetoGeneral = new GeneralDataAccess();
            bool accesoPagina;

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                accesoPagina = (bool)objetoGeneral.AccesoPagina(token, pagina, ref Codigo_Respuesta);
                if (objetoGeneral.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = accesoPagina;
                }
                else {
                    mensaje = objetoGeneral.textoRespuesta;
                    codigoRespuesta = objetoGeneral.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ReenvioCorreoContrasena(propiedadesReenvioCorreo propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            GeneralDataAccess objGeneral = new GeneralDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            ObjetoRespuesta.result = objGeneral.ReenviarCorreoRegistro(propiedades, ref Codigo_Respuesta);

            if (objGeneral.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
            {
                Codigo_Respuesta = HttpStatusCode.OK;
                mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                Resultado = ObjetoRespuesta.result;
            }
            else {
                mensaje = objGeneral.textoRespuesta;
                codigoRespuesta = objGeneral.codigoRespuesta;
                Resultado = new object { };
            }
            ObjetoRespuesta.status = codigoRespuesta;
            ObjetoRespuesta.message = mensaje;
            ObjetoRespuesta.result = Resultado;

            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ObtenerComponentes(ref HttpStatusCode Codigo_Respuesta, string nombreComponente)
        {
            Guid token = Guid.Empty;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            GeneralDataAccess objComponente = new GeneralDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            ComponenteFormulario componenteLista = new ComponenteFormulario();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                componenteLista.componente = (Componente)objComponente.ObtenerComponente(token, nombreComponente);

                if (objComponente.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = componenteLista.componente.idComponenteUnique != null ? componenteLista : new object { };
                }
                else
                {
                    mensaje = objComponente.textoRespuesta;
                    codigoRespuesta = objComponente.codigoRespuesta;
                    Resultado = new object { };
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

    }
}
