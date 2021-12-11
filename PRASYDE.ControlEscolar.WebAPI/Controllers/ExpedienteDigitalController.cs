
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
    public class ExpedienteDigitalController : ApiController
    {

        [Route("AgregarArchivo")]
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
                if (HttpContext.Current.Request.Form.GetValues("Request") != null)
                {
                    var jsonString = HttpContext.Current.Request.Form.GetValues("Request")[0];
                    var request = JsonConvert.DeserializeObject<propiedadesExpediente>(jsonString);
                    request.archivos = HttpContext.Current.Request.Files;

                    Respuesta = ExpedienteDigitalBusiness.Guardar(request, ref Codigo_Respuesta);
                    return Request.CreateResponse(Codigo_Respuesta, Respuesta);
                }
                else
                {
                    return Request.CreateResponse(Codigo_Respuesta, RespuestaError);
                }
            }
        }
    }
}
