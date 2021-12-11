
namespace PRASYDE.ControlEscolar.WebAPI.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using PRASYDE.ControlEscolar.Business;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Business.Framework;
 
    [RoutePrefix("api")]
    public class UsuariosController : ApiController
    {
        [Route("FormularioUsuarios")]
        [HttpGet]
        public HttpResponseMessage CargaFormulario()
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = UsuariosBusiness.ObtenerDatosFormulario(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("AgregarUsuario")]
        [HttpPost]
        public HttpResponseMessage Guardar(Usuarios propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                if (UsuariosBusiness.ValidarDatos(propiedades, Convert.ToInt16(Models.Enumerados.TipoValidacion.Agregar)))
                {
                    Respuesta = UsuariosBusiness.Guardar(propiedades, ref Codigo_Respuesta);
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

        [Route("ListadoUsuarios/{estatus}")]
        [HttpGet]
        public HttpResponseMessage ObtenerListado(int estatus)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = UsuariosBusiness.Listado(ref Codigo_Respuesta, estatus);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("ObtenerUsuario/{idUsuario}")]
        [HttpGet]
        public HttpResponseMessage ObtenerPlantel(string idUsuario)
        {

            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = UsuariosBusiness.Obtener(ref Codigo_Respuesta, idUsuario);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("EditarUsuario")]
        [HttpPost]
        public HttpResponseMessage Editar(Usuarios propiedades)
        {

            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                if (UsuariosBusiness.ValidarDatos(propiedades, Convert.ToInt16(Models.Enumerados.TipoValidacion.Editar)))
                {
                    Respuesta = UsuariosBusiness.Editar(propiedades, ref Codigo_Respuesta);
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

        [Route("AgregarPreInscripcion")]
        [HttpPost]
        public HttpResponseMessage GuardaPreInscripcion(UsuariosPreInscripcion propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (UsuariosBusiness.ValidarDatosPreInscripcion(propiedades))
            {
                Respuesta = UsuariosBusiness.GuardarPreInscripcion(propiedades, ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else {
                Respuesta = General.DatosInvalidos(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }
        

        [Route("CambiaTuContrasena")]
        [HttpPost] //[FromUri] string Aut
        public HttpResponseMessage CambiaContrasena(propiedadContasena propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            Respuesta = UsuariosBusiness.CambiarContrasena(propiedades, ref Codigo_Respuesta);
            return Request.CreateResponse(Codigo_Respuesta, Respuesta);
        }
        
        [Route("Dscencirpta/{cadena}")]
        [HttpGet]
        public HttpResponseMessage rolb(string cadena)
        {
            string respuesta = string.Empty;
            respuesta = UsuariosBusiness.Prueba(cadena);
            return Request.CreateResponse(HttpStatusCode.OK, respuesta);
        }



    }
}
