
namespace PRASYDE.ControlEscolar.WebAPI.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using PRASYDE.ControlEscolar.Business;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Business.Framework;
    
    [RoutePrefix("api")]
    public class ConceptosController : ApiController
    {
        [Route("ListadoConceptos/{estatus}")]
        [HttpGet]
        public HttpResponseMessage ObtenerListado(int estatus)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = ConceptosBusiness.Listado(ref Codigo_Respuesta, estatus);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("AgregarConcepto")]
        [HttpPost]
        public HttpResponseMessage Guardar(Conceptos propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                if (ConceptosBusiness.ValidarDatos(propiedades, 1))
                {
                    Respuesta = ConceptosBusiness.Guardar(propiedades, ref Codigo_Respuesta);
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

        [Route("EditarConcepto")]
        [HttpPost]
        public HttpResponseMessage Editar(Conceptos propiedades)
        {

            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                if (ConceptosBusiness.ValidarDatos(propiedades, 2))
                {
                    Respuesta = ConceptosBusiness.Editar(propiedades, ref Codigo_Respuesta);
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

        [Route("ObtenerConcepto/{idConcepto}")]
        [HttpGet]
        public HttpResponseMessage ObtenerPlantel(string idConcepto)
        {

            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = ConceptosBusiness.Obtener(ref Codigo_Respuesta, idConcepto);
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
