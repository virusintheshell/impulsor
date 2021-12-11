
namespace PRASYDE.ControlEscolar.WebAPI.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using PRASYDE.ControlEscolar.Business;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Business.Framework;

    [RoutePrefix("api")]
    public class EvaluacionesController : ApiController
    {
        [Route("EditarEvaluacionFinal")]
        [HttpPost]
        public HttpResponseMessage Guardar(EdicionCalificacionFinal propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                if (EvaluacionesBusiness.ValidarDatos(propiedades))
                {
                    Respuesta = EvaluacionesBusiness.Editar(propiedades, ref Codigo_Respuesta);
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
