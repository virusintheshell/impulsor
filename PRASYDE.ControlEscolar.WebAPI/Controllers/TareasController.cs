

namespace PRASYDE.ControlEscolar.WebAPI.Controllers
{
    using System;
    using System.Web;
    using System.Net;
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Web.Http;
    using PRASYDE.ControlEscolar.Business;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Business.Framework;

    [RoutePrefix("api")]
    public class TareasController : ApiController
    {
        [Route("ObtenerTareas/{tipo}/{estatus}")]
        [HttpGet]
        public HttpResponseMessage ObtenerTareas(int tipo, int estatus)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = TareasBusiness.ObtenerTareas(tipo, estatus, ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("AsignarTareas")]
        [HttpPost]
        public HttpResponseMessage AsignarTareas()
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            RespuestaGeneral RespuestaError = new RespuestaGeneral()
            {
                status = Convert.ToInt16(Enumerados.Codigos_Respuesta.ErrorData),
                message = "Formato de archivo incorrecto",
                result = new object { }
            };

            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return Request.CreateResponse(Codigo_Respuesta, RespuestaError);
            }
            else
            {
                if (HttpContext.Current.Request.Form.GetValues("objeto") != null)
                {
                    var jsonString = HttpContext.Current.Request.Form.GetValues("objeto")[0];
                    var request = JsonConvert.DeserializeObject<Tareas>(jsonString);
                    request.archivo = HttpContext.Current.Request.Files;

                    if (TareasBusiness.ValidarDatosTareasProfesor(request, Convert.ToInt16(Models.Enumerados.TipoValidacion.Agregar)))
                    {
                        Respuesta = TareasBusiness.Guardar(request, ref Codigo_Respuesta);
                        return Request.CreateResponse(Codigo_Respuesta, Respuesta);
                    }
                    else
                    {
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

        [Route("EditarTareas")]
        [HttpPost]
        public HttpResponseMessage EditarTareas()
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            RespuestaGeneral RespuestaError = new RespuestaGeneral()
            {
                status = Convert.ToInt16(Enumerados.Codigos_Respuesta.ErrorData),
                message = "Formato de archivo incorrecto",
                result = new object { }
            };

            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return Request.CreateResponse(Codigo_Respuesta, RespuestaError);
            }
            else
            {
                if (HttpContext.Current.Request.Form.GetValues("objeto") != null)
                {
                    var jsonString = HttpContext.Current.Request.Form.GetValues("objeto")[0];
                    var request = JsonConvert.DeserializeObject<Tareas>(jsonString);
                    request.archivo = HttpContext.Current.Request.Files;

                    if (TareasBusiness.ValidarDatosTareasProfesor(request, Convert.ToInt16(Models.Enumerados.TipoValidacion.Editar)))
                    {
                        Respuesta = TareasBusiness.EditarTareasProfesor(request, ref Codigo_Respuesta);
                        return Request.CreateResponse(Codigo_Respuesta, Respuesta);
                    }
                    else
                    {
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

        [Route("GuardarTareasAlumnos")]
        [HttpPost]
        public HttpResponseMessage GuardarTareasAlumnos()
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                RespuestaGeneral RespuestaError = new RespuestaGeneral()
                {
                    status = Convert.ToInt16(Enumerados.Codigos_Respuesta.ErrorData),
                    message = "Formato de archivo incorrecto",
                    result = new object { }
                };

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    return Request.CreateResponse(Codigo_Respuesta, RespuestaError);
                }
                else
                {
                    if (HttpContext.Current.Request.Form.GetValues("objeto") != null)
                    {
                        var jsonString = HttpContext.Current.Request.Form.GetValues("objeto")[0];
                        var request = JsonConvert.DeserializeObject<TareasAlumno>(jsonString);
                        request.archivo = HttpContext.Current.Request.Files;

                        if (TareasBusiness.ValidarDatosTareasAlumno(request))
                        {
                            Respuesta = TareasBusiness.GuardarTareasAlumnos(request, ref Codigo_Respuesta);
                            return Request.CreateResponse(Codigo_Respuesta, Respuesta);
                        }
                        else
                        {
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

        [Route("ObtenerTareasRevision")]
        [HttpPost]
        public HttpResponseMessage ObtenerTareas(TareasFiltroRevision propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = TareasBusiness.ObtenerRevisionTareas(propiedades, ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }


        [Route("ComentariosProfesor")]
        [HttpPost]
        public HttpResponseMessage GuardarComentarios(ObjetoComentariosProfesor propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = TareasBusiness.GuardrComentariosTareas(propiedades, ref Codigo_Respuesta);
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
