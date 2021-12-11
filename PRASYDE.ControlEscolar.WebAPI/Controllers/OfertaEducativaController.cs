

namespace PRASYDE.ControlEscolar.WebAPI.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using PRASYDE.ControlEscolar.Business;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Business.Framework;

    [RoutePrefix("api")]
    public class OfertaEducativaController : ApiController
    {
        [Route("FormularioOfertaEducativa")]
        [HttpGet]
        public HttpResponseMessage CargaFormulario()
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            Respuesta = OfertaEducativaBusiness.Formulario(ref Codigo_Respuesta);
            return Request.CreateResponse(Codigo_Respuesta, Respuesta);
        }

        [Route("ListadoOferta/{estatus}/{idPlantel}")]
        [HttpGet]
        public HttpResponseMessage ObtenerListado(int estatus,int idPlantel)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            Respuesta = OfertaEducativaBusiness.Listado(ref Codigo_Respuesta, estatus, idPlantel);
            return Request.CreateResponse(Codigo_Respuesta, Respuesta);
        }

        [Route("AgregarOfertaEducativa")]
        [HttpPost]
        public HttpResponseMessage Guardar(OfertaEducativa propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                if (OfertaEducativaBusiness.ValidarDatos(propiedades, 1))
                {
                    Respuesta = OfertaEducativaBusiness.Guardar(propiedades, ref Codigo_Respuesta);
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


        [Route("ObtenerOferta/{idOferta}")]
        [HttpGet]
        public HttpResponseMessage Obtener(string idOferta)
        {

            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = OfertaEducativaBusiness.Obtener(ref Codigo_Respuesta, idOferta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }


        [Route("EditarOferta")]
        [HttpPost]
        public HttpResponseMessage Editar(OfertaEducativa propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                if (OfertaEducativaBusiness.ValidarDatos(propiedades, 2))
                {
                    Respuesta = OfertaEducativaBusiness.Editar(propiedades, ref Codigo_Respuesta);
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
