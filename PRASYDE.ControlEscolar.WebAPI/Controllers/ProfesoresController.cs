
namespace PRASYDE.ControlEscolar.WebAPI.Controllers
{
    using System;
    using System.Web;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Newtonsoft.Json;
    using PRASYDE.ControlEscolar.Business;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Business.Framework;
       
    [RoutePrefix("api")]
    public class ProfesoresController : ApiController
    {
        [Route("ListadoProfesores/{estatus}")]
        [HttpGet]
        public HttpResponseMessage ObtenerListado(int estatus)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = ProfesoresBusiness.Listado(ref Codigo_Respuesta, estatus);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("AgregarAsignacion")]
        [HttpPost]
        public HttpResponseMessage Guardar(AsignacionProfesorAsignatura propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = ProfesoresBusiness.GuardarAsingaturas(propiedades, ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);

                //if (AsignaturasBusiness.ValidarDatos(propiedades, 1))
                //{

                //}
                //else {
                //    Respuesta = General.DatosInvalidos(ref Codigo_Respuesta);
                //    return Request.CreateResponse(Codigo_Respuesta, Respuesta);
                //}
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("ObtenerProfesorAsingaturas/{idrofesor}")]
        [HttpGet]
        public HttpResponseMessage ObtenerListadoAsignacion(string idrofesor)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = ProfesoresBusiness.ObtenerDatosAsingaturas(idrofesor, ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("HorarioProfesor/{idGrupo}")]
        [HttpGet]
        public HttpResponseMessage ObtenerHorarioProfesor(string idGrupo)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = ProfesoresBusiness.ObtenerDetalleGrupoProfesor(idGrupo, ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("EvaluacionesProfesor")]
        [HttpPost]
        public HttpResponseMessage ObtenerEvaluaciones(filtrosCalificaion propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = ProfesoresBusiness.Evaluaciones(propiedades, ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("GuardarEvaluaciones")]
        [HttpPost]
        public HttpResponseMessage Evaluaciones(Evaluaciones propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = ProfesoresBusiness.GuardarEvaluaciones(propiedades, ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("EditarEvaluaciones")]
        [HttpPost]
        public HttpResponseMessage editarEvaluaciones(EdicionEvaluaciones propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                if (ProfesoresBusiness.ValidarDatosEditarEvaluacion(propiedades))
                {
                    Respuesta = ProfesoresBusiness.EditarEvaluaciones(propiedades, ref Codigo_Respuesta);
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
        
        [Route("AsistenciasProfesor")]
        [HttpPost]
        public HttpResponseMessage ObtenerAsistencias(filtrosCalificaion propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = ProfesoresBusiness.Asistencias(propiedades, ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("GuardarAsistencia")]
        [HttpPost]
        public HttpResponseMessage Asistencias(Asistencia propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                if (ProfesoresBusiness.ValidarDatos(propiedades))
                {
                    Respuesta = ProfesoresBusiness.GuardarAsistencias(propiedades, ref Codigo_Respuesta);
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

        [Route("MisAsignaturas")]
        [HttpGet]
        public HttpResponseMessage ObtenerMisAsignaturas()
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = ProfesoresBusiness.ListadoMisAsignaturas(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("AgregarDocumento")]
        [HttpPost]
        public HttpResponseMessage GuardarArchivo()
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
                    var request = JsonConvert.DeserializeObject<PropiedadesDocumentosProfesor>(jsonString);
                    request.archivo = HttpContext.Current.Request.Files;

                    Respuesta = ProfesoresBusiness.GuardarDocumentos(request, ref Codigo_Respuesta);
                    return Request.CreateResponse(Codigo_Respuesta, Respuesta);
                }
                else
                {
                    return Request.CreateResponse(Codigo_Respuesta, RespuestaError);
                }
            }
        }

        [Route("EditarDocumento")]
        [HttpPost]
        public HttpResponseMessage EditarArchivo()
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
                    var request = JsonConvert.DeserializeObject<PropiedadesDocumentosProfesor>(jsonString);
                    request.archivo = HttpContext.Current.Request.Files;

                    Respuesta = ProfesoresBusiness.EditarDocumentos(request, ref Codigo_Respuesta);
                    return Request.CreateResponse(Codigo_Respuesta, Respuesta);
                }
                else
                {
                    return Request.CreateResponse(Codigo_Respuesta, RespuestaError);
                }
            }
        }

        [Route("ListadoTareasEntregadas/{idTarea}")]
        [HttpGet]
        public HttpResponseMessage ObtenerTareasEntregas(string idTarea)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = ProfesoresBusiness.ListadoTareasEntregadas(idTarea, ref Codigo_Respuesta);
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