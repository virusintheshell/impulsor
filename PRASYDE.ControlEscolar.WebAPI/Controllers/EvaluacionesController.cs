
namespace PRASYDE.ControlEscolar.WebAPI.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using PRASYDE.ControlEscolar.Business;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Business.Framework;
    using System.Net.Http.Headers;
    using System;

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


        [Route("reporteEvaluacionesPorGrupo/{idGrupo}/{nivel}")]
        [HttpGet]
        public HttpResponseMessage obtenerDiagnosticoExcel(string idGrupo, int nivel)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            if (ValidarSesion.tokenValido())
            {
                HttpResponseMessage responseMessage = null;

                var response = EvaluacionesBusiness.ExportarEvaluaciones(idGrupo, nivel);
                responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                responseMessage.Content = new ByteArrayContent(response.FileArray);
                responseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

                responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                responseMessage.Content.Headers.ContentDisposition.FileName = string.Concat("ReporteEventos", DateTime.Now.ToString(), ".xlsx");
                responseMessage.Content.Headers.ContentLength = response.FileArray.Length;

                return responseMessage;
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }
    }
}
