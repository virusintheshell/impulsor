

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

    [RoutePrefix("api")]
    public class AsignaturaController : ApiController
    {
        [Route("ListadoAsignatura/{estatus}")]
        [HttpGet]
        public HttpResponseMessage ObtenerListado(int estatus)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = AsignaturasBusiness.Listado(ref Codigo_Respuesta, estatus);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("AgregarAsignatura")]
        [HttpPost]
        public HttpResponseMessage Guardar()
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();
            RespuestaGeneral RespuestaError = new RespuestaGeneral()
            {
                status = Convert.ToInt16(Enumerados.Codigos_Respuesta.ErrorData),
                message = "Formato de archivo incorrecto",
                result = new object { }
            };

            if (ValidarSesion.tokenValido())
            {
                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    return Request.CreateResponse(Codigo_Respuesta, RespuestaError);
                }
                else
                {
                    if (HttpContext.Current.Request.Form.GetValues("objetoAsignatura") != null)
                    {
                        var jsonString = HttpContext.Current.Request.Form.GetValues("objetoAsignatura")[0];
                        var propiedades = JsonConvert.DeserializeObject<Asignaturas>(jsonString);
                        propiedades.archivo = HttpContext.Current.Request.Files;

                        if (AsignaturasBusiness.ValidarDatos(propiedades, 1))
                        {
                            Respuesta = AsignaturasBusiness.Guardar(propiedades, ref Codigo_Respuesta);
                            return Request.CreateResponse(Codigo_Respuesta, Respuesta);
                        }
                        else {
                            Respuesta = General.DatosInvalidos(ref Codigo_Respuesta);
                            return Request.CreateResponse(Codigo_Respuesta, Respuesta);
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(Codigo_Respuesta, RespuestaError);
                    }
                }
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }


        [Route("ObtenerAsignatura/{idAsignatura}")]
        [HttpGet]
        public HttpResponseMessage ObtenerPlantel(string idAsignatura)
        {

            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = AsignaturasBusiness.Obtener(ref Codigo_Respuesta, idAsignatura);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("EditarAsignatura")]
        [HttpPost]
        public HttpResponseMessage Editar()
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();
            RespuestaGeneral RespuestaError = new RespuestaGeneral()
            {
                status = Convert.ToInt16(Enumerados.Codigos_Respuesta.ErrorData),
                message = "Formato de archivo incorrecto",
                result = new object { }
            };

            if (ValidarSesion.tokenValido())
            {
                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    return Request.CreateResponse(Codigo_Respuesta, RespuestaError);
                }
                else
                {
                    if (HttpContext.Current.Request.Form.GetValues("objetoAsignatura") != null)
                    {
                        var jsonString = HttpContext.Current.Request.Form.GetValues("objetoAsignatura")[0];
                        var propiedades = JsonConvert.DeserializeObject<Asignaturas>(jsonString);
                        propiedades.archivo = HttpContext.Current.Request.Files;

                        if (AsignaturasBusiness.ValidarDatos(propiedades, 2))
                        {
                            Respuesta = AsignaturasBusiness.Editar(propiedades, ref Codigo_Respuesta);
                            return Request.CreateResponse(Codigo_Respuesta, Respuesta);
                        }
                        else {
                            Respuesta = General.DatosInvalidos(ref Codigo_Respuesta);
                            return Request.CreateResponse(Codigo_Respuesta, Respuesta);
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(Codigo_Respuesta, RespuestaError);
                    }
                }
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("EditarAsignatura1")]
        [HttpPost]
        public HttpResponseMessage Editar1(Asignaturas propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                if (AsignaturasBusiness.ValidarDatos(propiedades, 2))
                {
                    Respuesta = AsignaturasBusiness.Editar(propiedades, ref Codigo_Respuesta);
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
