
//AUTOR: DIEGO OLVERA
//FECHA: 06-05-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA EL MODULO DE PAGOS

namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using Framework;
    using DataAcess;
    using System.Net;
    using System.Web;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Business.Notificaciones;

    public class PagosBusiness
    {
        public static RespuestaGeneral Listado(ref HttpStatusCode Codigo_Respuesta, int estatus)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            string pagina = string.Empty;
            string paginaOrigen = string.Empty;

            PagosDataAccess objPagos = new PagosDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            GeneralDataAccess objetoGeneral = new GeneralDataAccess();
            ListadoPermisoPagos objListaPagos = new ListadoPermisoPagos();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina, ref paginaOrigen))
            {
                objListaPagos.permisos = (List<ListaGenericaCatalogos>)objetoGeneral.Permisos(token, pagina, paginaOrigen, ref Codigo_Respuesta);
                if (objetoGeneral.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    objListaPagos.pagos = (List<ListadoPagos>)objPagos.Listado(token, estatus, ref Codigo_Respuesta);
                    if (objPagos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                        codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                        Resultado = objListaPagos;
                    }
                    else {
                        mensaje = objPagos.textoRespuesta;
                        codigoRespuesta = objPagos.codigoRespuesta;
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

        public static RespuestaGeneral Guardar(Pagos propiedades, ref HttpStatusCode Codigo_Respuesta)
        {

            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (HttpContext.Current.Request.Headers["Plataforma"] != null)
            {
                ObjetoRespuesta = GuardarMovil(propiedades, ref Codigo_Respuesta);
            }
            else {

                if (ValidarSesion.tokenValido())
                {
                    ObjetoRespuesta = GuardarWeb(propiedades, ref Codigo_Respuesta);
                }
                else {
                    ObjetoRespuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                }
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral GuardarWeb(Pagos propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;
            int plataforma = 2;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();
            
            PagosDataAccess objPagos = new PagosDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            EnvioNotificacion objNotificacion = new EnvioNotificacion();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objPagos.Guardar(propiedades, token, pagina, plataforma, ref Codigo_Respuesta);

                if (objPagos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;

                    //ENVIO DE NOTIFICACION 
                    objNotificacion.EnviarNotificacion(Convert.ToInt32(Resultado), Convert.ToInt16(Enumerados.TipoNotificacion.Pago), ref Codigo_Respuesta);
                }
                else {
                    mensaje = objPagos.textoRespuesta;
                    codigoRespuesta = objPagos.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }

            return ObjetoRespuesta;
        }

        public static RespuestaGeneral GuardarMovil(Pagos propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = "pagos";
            int plataforma = 0;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            PagosDataAccess objPagos = new PagosDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref plataforma))
            {
                ObjetoRespuesta.result = objPagos.Guardar(propiedades, token, pagina, plataforma, ref Codigo_Respuesta);

                if (objPagos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objPagos.textoRespuesta;
                    codigoRespuesta = objPagos.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }

            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ValidarPagos(string idPago, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;
        
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            PagosDataAccess objPagos = new PagosDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objPagos.ValidarPagos(idPago, token, pagina, ref Codigo_Respuesta);

                if (objPagos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objPagos.textoRespuesta;
                    codigoRespuesta = objPagos.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }

            return ObjetoRespuesta;
        }

        public static bool ValidarDatos(string idPago)
        {
            try
            {
                bool respuesta = true;
                if (idPago == "") { respuesta = false; }
                return respuesta;
            }
            catch (Exception exception) { throw exception; }
        }

    }
}
