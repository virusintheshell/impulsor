
namespace PRASYDE.ControlEscolar.WebAPI.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Business.Framework;
    using PRASYDE.ControlEscolar.Entities.Finanzas;
    using PRASYDE.ControlEscolar.Business.Finanzas;

    [RoutePrefix("api")]
    public class FinanzasController : ApiController
    {
        [Route("calendarioPagos")]
        [HttpPost]
        public HttpResponseMessage ObtenerListado(CalendarioDePagos propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = CalendarioDePagosBusiness.Listado(propiedades, ref Codigo_Respuesta);
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
