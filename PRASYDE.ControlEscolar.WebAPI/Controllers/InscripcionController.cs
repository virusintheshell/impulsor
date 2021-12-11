
namespace PRASYDE.ControlEscolar.WebAPI.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using PRASYDE.ControlEscolar.Business;
    using PRASYDE.ControlEscolar.Entities;
    
    [RoutePrefix("api")]
    public class InscripcionController : ApiController
    {
        [Route("GradosInscripcion")]
        [HttpGet]
        public HttpResponseMessage CargaFormulario()
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            Respuesta = InscripcionBusiness.ObtenerDatosFormulario(ref Codigo_Respuesta);
            return Request.CreateResponse(Codigo_Respuesta, Respuesta);
        }

        [Route("ObtenerGruposPlantel/{idPrograma}")]
        [HttpGet]
        public HttpResponseMessage ObtenerGrupos(string idPrograma)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            Respuesta = InscripcionBusiness.Obtener(idPrograma,ref Codigo_Respuesta);
            return Request.CreateResponse(Codigo_Respuesta, Respuesta);
        }

        [Route("AgregarInscripcion")]
        [HttpPost]
        public HttpResponseMessage Guardar(Inscripcion propiedades)
        {

            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            Respuesta = InscripcionBusiness.Guardar(propiedades, ref Codigo_Respuesta);
            return Request.CreateResponse(Codigo_Respuesta, Respuesta);

            //if (ValidarSesion.tokenValido())
            //{

            //}
            //else
            //{
            //    Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
            //    return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            //}
        }

    }
}
