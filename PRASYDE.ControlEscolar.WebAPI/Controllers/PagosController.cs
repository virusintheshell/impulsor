

namespace PRASYDE.ControlEscolar.WebAPI.Controllers
{
    using System;
    using System.Web;
    using System.Net;
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Web.Http;
    using PRASYDE.ControlEscolar.Business;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Business.Framework;

    [RoutePrefix("api")]
    public class PagosController : ApiController
    {
        [Route("ListadoPagos/{estatus}")]
        [HttpGet]
        public HttpResponseMessage ObtenerListado(int estatus)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = PagosBusiness.Listado(ref Codigo_Respuesta, estatus);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("AgregarPagos")]
        [HttpPost]
        public HttpResponseMessage GuardarReporteResultados()
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            RespuestaGeneral RespuestaError = new RespuestaGeneral()
            {
                status = Convert.ToInt16(Enumerados.Codigos_Respuesta.ErrorData),
                message = "Formato de archivo incorrecto",
                result = new object { }
            };

            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return Request.CreateResponse(Codigo_Respuesta, RespuestaError);
            }
            else {
                if (HttpContext.Current.Request.Form.GetValues("Request") != null)
                {
                    var jsonString = HttpContext.Current.Request.Form.GetValues("Request")[0];
                    var request = JsonConvert.DeserializeObject<Pagos>(jsonString);
                    request.archivos = HttpContext.Current.Request.Files;

                    Respuesta = PagosBusiness.Guardar(request, ref Codigo_Respuesta);
                    return Request.CreateResponse(Codigo_Respuesta, Respuesta);
                }
                else {
                    return Request.CreateResponse(Codigo_Respuesta, RespuestaError);
                }
            }
        }

        [Route("ValidarPagos/{idPago}")]
        [HttpGet]
        public HttpResponseMessage VlidarPagos(string idPago)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                if (PagosBusiness.ValidarDatos(idPago))
                {
                    Respuesta = PagosBusiness.ValidarPagos(idPago, ref Codigo_Respuesta);
                    return Request.CreateResponse(Codigo_Respuesta, Respuesta);
                }
                else {
                    Respuesta = General.DatosInvalidos(ref Codigo_Respuesta);
                    return Request.CreateResponse(Codigo_Respuesta, Respuesta);
                }
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }
    }
}
