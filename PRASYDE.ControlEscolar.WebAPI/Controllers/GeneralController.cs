

namespace PRASYDE.ControlEscolar.WebAPI.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Business;
    using PRASYDE.ControlEscolar.Business.Framework;
    using PRASYDE.ControlEscolar.Business.Notificaciones;

    [RoutePrefix("api")]
    public class GeneralController : ApiController
    {
        [Route("login")]
        [HttpPost]
        public HttpResponseMessage IniciarSesion(CredencialesAcceso credenciales)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (credenciales == null)
            {
                Respuesta = General.DatosInvalidos(ref Codigo_Respuesta);
                return Request.CreateResponse(HttpStatusCode.BadRequest, Respuesta);
            }
            else {
                Respuesta = SesionBusiness.Iniciar(credenciales, ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }
        
        [Route("detalleSesion")]
        [HttpGet]
        public HttpResponseMessage GuardarDetalle()
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = SesionBusiness.GuardarDetalleSesion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }
        
        [Route("obtenerMenu")]
        [HttpGet]
        public HttpResponseMessage Menu()
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = MenuBusuniness.ObtenerMenu(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }
        
        [Route("FiltroCatalogo/{id}")]
        [HttpGet]
        public HttpResponseMessage ObtenerDatos(int id)
        {

            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = GeneralBusiness.ObtenerListaTresValores(ref Codigo_Respuesta, id);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("CambiarStatus/{id}")]
        [HttpPost]
        public HttpResponseMessage CambioEstatus(string id)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = GeneralBusiness.Cambiar(ref Codigo_Respuesta, id);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("EliminarRegistro/{id}")]
        [HttpPost]
        public HttpResponseMessage Eliminar(string id)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = GeneralBusiness.Eliminar(ref Codigo_Respuesta, id);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("ValidaAccesoFormulario")]
        [HttpGet]
        public HttpResponseMessage AccesoFormulario()
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = GeneralBusiness.AccesoFormulario(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("ReenviarCorreo")]
        [HttpPost]
        public HttpResponseMessage ReenviarCorreoCambioContrasena(propiedadesReenvioCorreo propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            Respuesta = GeneralBusiness.ReenvioCorreoContrasena(propiedades, ref Codigo_Respuesta);
            return Request.CreateResponse(Codigo_Respuesta, Respuesta);
        }

        [Route("obtenerFormulario/{nombreFomulario}")]
        [HttpGet]
        public HttpResponseMessage ObtenerFomularioDinamico(string nombreFomulario)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = FormularioBusiness.Obtener(ref Codigo_Respuesta, nombreFomulario);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }
        
        [Route("obtenerComponente/{nombreComponente}")]
        [HttpGet]
        public HttpResponseMessage ObtenerComponenteDinamico(string nombreComponente)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = GeneralBusiness.ObtenerComponentes(ref Codigo_Respuesta, nombreComponente);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }
        
        [Route("MisNotificaciones")]
        [HttpGet]
        public HttpResponseMessage ObtenerMisNotificaciones()
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta =  EnvioNotificacion.ListadoNotificaciones(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }
        
        [Route("LeerNotificacion/{idReferencia}/{tipoNotificacion}")]
        [HttpGet]
        public HttpResponseMessage LeerNotificaciones(int idReferencia,int tipoNotificacion)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = EnvioNotificacion.CambiarEstatusNotificaciones(idReferencia, tipoNotificacion, ref Codigo_Respuesta);
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
