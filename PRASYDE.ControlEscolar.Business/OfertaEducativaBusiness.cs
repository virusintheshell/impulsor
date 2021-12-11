
//AUTOR: DIEGO OLVERA
//FECHA: 08-04-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA LA ORFETA EDUCATIVA

namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using DataAcess;
    using Framework;
    using System.Net;
    using System.Web;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;

    public class OfertaEducativaBusiness
    {
        public static RespuestaGeneral Formulario(ref HttpStatusCode Codigo_Respuesta)
        {
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (HttpContext.Current.Request.Headers["Plataforma"] != null)
            {
                int idEmpresa = Convert.ToInt16(HttpContext.Current.Request.Headers["Empresa"]);
                ObjetoRespuesta = ObtenerDatosFormularioMovil(ref Codigo_Respuesta, idEmpresa);
            }
            else {

                if (ValidarSesion.tokenValido())
                {
                    ObjetoRespuesta = ObtenerDatosFormularioWeb(ref Codigo_Respuesta);
                }
                else {
                    ObjetoRespuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                }
            }
            return ObjetoRespuesta;
        }
        
        public static RespuestaGeneral ObtenerDatosFormularioWeb(ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            CatalogosDataAccess objDetalleModalidad = new CatalogosDataAccess();
            CatalogosDataAccess objGradoEducativo = new CatalogosDataAccess();

            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            FormularioOfertaEducativa objListaOferta = new FormularioOfertaEducativa();
            PropiedadesCatalogos filtroGeneral;

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                filtroGeneral = new PropiedadesCatalogos() { id = 0, cadenaIds = "", nombrePagina = "Modalidad" };
                ObjetoRespuesta = objDetalleModalidad.ObtenerCatalogo(token, filtroGeneral, ref Codigo_Respuesta);

                objListaOferta.modalidades = (List<ListaGenericaCatalogos>)ObjetoRespuesta.result;
                if (objDetalleModalidad.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    filtroGeneral = new PropiedadesCatalogos() { id = 0, cadenaIds = "", nombrePagina = "GradoEducativo" };
                    ObjetoRespuesta = objGradoEducativo.ObtenerCatalogo(token, filtroGeneral, ref Codigo_Respuesta);

                    objListaOferta.gradoEducativo = (List<ListaGenericaCatalogosTresValores>)ObjetoRespuesta.result;
                    if (objGradoEducativo.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))

                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                        codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                        Resultado = objListaOferta;
                    }
                    else {
                        mensaje = objGradoEducativo.textoRespuesta;
                        codigoRespuesta = objGradoEducativo.codigoRespuesta;
                        Resultado = new object { };
                    }
                }
                else {
                    mensaje = objDetalleModalidad.textoRespuesta;
                    codigoRespuesta = objDetalleModalidad.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ObtenerDatosFormularioMovil(ref HttpStatusCode Codigo_Respuesta, int idEmpresa)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();
            int idPlataforma = 0;
            
            CatalogosDataAccess objGradoEducativo = new CatalogosDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            FormularioOfertaEducativaMovil objListaOferta = new FormularioOfertaEducativaMovil();

            PropiedadesCatalogos filtroGeneral;

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref idPlataforma))
            {
                //SE ENVIA LA PLATAFORMA EN EL PARAMATRO ID Y COMO TOKEN SE ENVIA NEW GUID()
                Guid tokenParammetro = new Guid();
                
                filtroGeneral = new PropiedadesCatalogos() { id = idPlataforma, cadenaIds = idEmpresa.ToString(), nombrePagina = "GradoEducativo" };
                ObjetoRespuesta = objGradoEducativo.ObtenerCatalogo(tokenParammetro, filtroGeneral, ref Codigo_Respuesta);

                objListaOferta.gradoEducativo = (List<ListaGenericaCatalogosTresValores>)ObjetoRespuesta.result;
                if (objGradoEducativo.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = objListaOferta;
                }
                else {
                    mensaje = objGradoEducativo.textoRespuesta;
                    codigoRespuesta = objGradoEducativo.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ListaElementos(ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            string pagina = string.Empty;
            string paginaOrigen = string.Empty;

            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            OfertaEducativaDataAccess objOferta = new OfertaEducativaDataAccess();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina, ref paginaOrigen))
            {
                ObjetoRespuesta.result = objOferta.ArmarFormualario(token, ref Codigo_Respuesta);
                if (objOferta.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                }
                else {
                    mensaje = objOferta.textoRespuesta;
                    codigoRespuesta = objOferta.codigoRespuesta;
                    ObjetoRespuesta.result = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;

            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Listado(ref HttpStatusCode Codigo_Respuesta, int estatus, int idPlantel)
        {

            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (HttpContext.Current.Request.Headers["Plataforma"] != null)
            {
                ObjetoRespuesta = ListadoMovil(ref Codigo_Respuesta, estatus);
            }
            else {

                if (ValidarSesion.tokenValido())
                {
                    ObjetoRespuesta = ListadoWeb(ref Codigo_Respuesta, estatus, idPlantel);
                }
                else {
                    ObjetoRespuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                }
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ListadoWeb(ref HttpStatusCode Codigo_Respuesta, int estatus, int idPlantel)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            string pagina = string.Empty;
            string paginaOrigen = string.Empty;

            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            OfertaEducativaDataAccess objOferta = new OfertaEducativaDataAccess();

            GeneralDataAccess objetoGeneral = new GeneralDataAccess();
            ListadoPermisoOfertaEducativa objListaOfertas = new ListadoPermisoOfertaEducativa();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina, ref paginaOrigen))
            {
                objListaOfertas.permisos = (List<ListaGenericaCatalogos>)objetoGeneral.Permisos(token, pagina, paginaOrigen, ref Codigo_Respuesta);
                if (objetoGeneral.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    objListaOfertas.ofertas = (List<ListadoOfertaEducativa>)objOferta.Listado(token, estatus, 0, idPlantel, ref Codigo_Respuesta);
                    if (objOferta.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                        codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                        Resultado = objListaOfertas;
                    }
                    else {
                        mensaje = objOferta.textoRespuesta;
                        codigoRespuesta = objOferta.codigoRespuesta;
                        Resultado = new object { };
                    }
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

        public static RespuestaGeneral ListadoMovil(ref HttpStatusCode Codigo_Respuesta, int estatus)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();
            int idPlataforma = 0;

            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            OfertaEducativaDataAccess objOferta = new OfertaEducativaDataAccess();

            GeneralDataAccess objetoGeneral = new GeneralDataAccess();
            ListadoPermisoOfertaEducativaMovil objListaOfertas = new ListadoPermisoOfertaEducativaMovil();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref idPlataforma))
            {
                objListaOfertas.ofertas = (List<ListadoOfertaEducativa>)objOferta.Listado(token, estatus, idPlataforma,0, ref Codigo_Respuesta);
                if (objOferta.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = objListaOfertas;
                }
                else {
                    mensaje = objOferta.textoRespuesta;
                    codigoRespuesta = objOferta.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Guardar(OfertaEducativa propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            OfertaEducativaDataAccess objOferta = new OfertaEducativaDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objOferta.Guardar(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objOferta.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objOferta.textoRespuesta;
                    codigoRespuesta = objOferta.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Obtener(ref HttpStatusCode Codigo_Respuesta, string idOfertaUnique)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            OfertaEducativaDataAccess objOferta = new OfertaEducativaDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objOferta.Obtener(token, pagina, idOfertaUnique, ref Codigo_Respuesta);

                if (objOferta.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objOferta.textoRespuesta;
                    codigoRespuesta = objOferta.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Editar(OfertaEducativa propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            OfertaEducativaDataAccess objOferta = new OfertaEducativaDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objOferta.Editar(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objOferta.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objOferta.textoRespuesta;
                    codigoRespuesta = objOferta.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static bool ValidarDatos(OfertaEducativa propiedades, int tipo)
        {
            try
            {
                bool respusta = true;
                if (tipo == 2)
                {
                    if (Seguridad.TextoSimpleObligatorio(propiedades.idOfertaEducativa, 1, 60) == false) { respusta = false; return respusta; }
                }

                if (Seguridad.TextoSimpleObligatorio(propiedades.idProgramaEducativo, 1, 50) == false) { respusta = false; return respusta; }
                if (propiedades.idDetalleModalidad == 0) { respusta = false; return respusta; }

                return respusta;
            }
            catch (Exception exception) { throw exception; }
        }
    }
}
