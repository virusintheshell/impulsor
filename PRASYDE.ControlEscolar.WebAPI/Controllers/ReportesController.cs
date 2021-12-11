
namespace PRASYDE.ControlEscolar.WebAPI.Controllers
{
    using System;
    using System.Net;
    using System.Web;
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Web.Http;
    using PRASYDE.ControlEscolar.Business;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Business.Framework;
    using PRASYDE.ControlEscolar.Business.Reportes;

    [RoutePrefix("api")]
    public class ReportesController : ApiController
    {
        [Route("ReporteGeneral")]
        [HttpGet]
        public HttpResponseMessage ObtenerDatosGenerales()
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = ReporteGeneralBusiness.Obtener(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("ReportePagosMensuales")]
        [HttpGet]
        public HttpResponseMessage ObtenerPagosMensuales()
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = ReporteGeneralBusiness.ObtenerReportePagos(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }
        
        [Route("ReporteInscripcionesMensuales")]
        [HttpGet]
        public HttpResponseMessage ObtenerInscripcionesMensuales()
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = ReporteGeneralBusiness.ObtenerReporteInscripciones(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }
    }
}
