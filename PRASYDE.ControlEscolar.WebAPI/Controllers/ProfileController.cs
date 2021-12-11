
namespace PRASYDE.ControlEscolar.WebAPI.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using PRASYDE.ControlEscolar.Business;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Business.Framework;

    [RoutePrefix("api")]
    public class ProfileController : ApiController
    {
        [Route("obtenerPerfil")]
        [HttpGet]
        public HttpResponseMessage CalificarPublicacion()
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();
            
            if (ValidarSesion.tokenValido())
            {
                Respuesta = PerfilBusiness.ObtenerPerfil(ref Codigo_Respuesta);
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
