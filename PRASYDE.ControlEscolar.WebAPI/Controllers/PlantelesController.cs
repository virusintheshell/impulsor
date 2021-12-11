
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
    public class PlantelesController : ApiController
    {
       
        [Route("ListadoPlanteles/{estatus}")]
        [HttpGet]
        public HttpResponseMessage ObtenerListado(int estatus)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            Respuesta = PlantelesBusiness.Listado(ref Codigo_Respuesta, estatus);
            return Request.CreateResponse(Codigo_Respuesta, Respuesta);
        }

        [Route("AgregarPlantel")]
        [HttpPost]
        public HttpResponseMessage Guardar(Planteles propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                if (PlantelesBusiness.ValidarDatos(propiedades, Convert.ToInt16(Models.Enumerados.TipoValidacion.Agregar)))
                {
                    Respuesta = PlantelesBusiness.Guardar(propiedades, ref Codigo_Respuesta);
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

        [Route("ObtenerPlantel/{idPlantel}")]
        [HttpGet]
        public HttpResponseMessage ObtenerPlantel(string idPlantel)
        {

            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();
            
            if (ValidarSesion.tokenValido())
            {
                Respuesta = PlantelesBusiness.Obtener(ref Codigo_Respuesta, idPlantel);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("EditarPlantel")]
        [HttpPost]
        public HttpResponseMessage Editar(Planteles propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                if (PlantelesBusiness.ValidarDatos(propiedades, Convert.ToInt16(Models.Enumerados.TipoValidacion.Editar)))
                {
                    Respuesta = PlantelesBusiness.Editar(propiedades, ref Codigo_Respuesta);
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


        [Route("ListadoAsignacion/{idPlatel}")]
        [HttpGet]
        public HttpResponseMessage ObtenerListadoAsignacion(string idPlatel)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {

                Respuesta = PlantelesBusiness.ListadoAsignacion(ref Codigo_Respuesta, idPlatel);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }


        [Route("GuardarAsignacion")]
        [HttpPost]
        public HttpResponseMessage Asignacion(PropiedadesAsignacion propiedades)
        {

            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = PlantelesBusiness.GuardaAsignacion(propiedades, ref Codigo_Respuesta);
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
