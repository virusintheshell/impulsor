
namespace PRASYDE.ControlEscolar.WebAPI.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using PRASYDE.ControlEscolar.Business;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Business.Framework;

    [RoutePrefix("api")]
    public class ProgramaEducativoController : ApiController
    {

        [Route("FormularioPrograma")]
        [HttpGet]
        public HttpResponseMessage CargaFormulario()
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = ProgramaEducativoBusiness.ObtenerDatosFormulario(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }
        
        [Route("ListadoProgramaEducativo/{estatus}/{idAlumno=ND}")]
        [HttpGet]
        public HttpResponseMessage ObtenerListado(int estatus,string idAlumno)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            Respuesta = ProgramaEducativoBusiness.Listado(ref Codigo_Respuesta, estatus, idAlumno);
            return Request.CreateResponse(Codigo_Respuesta, Respuesta);
        }

        [Route("AgregarPrograma")]
        [HttpPost]
        public HttpResponseMessage Guardar(ProgramaEducativo propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                if (ProgramaEducativoBusiness.ValidarDatos(propiedades,1))
                {
                    Respuesta = ProgramaEducativoBusiness.Guardar(propiedades, ref Codigo_Respuesta);
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
        
        [Route("ObtenerPrograma/{idPrograma}")]
        [HttpGet]
        public HttpResponseMessage ObtenerProgramaEducativo(string idPrograma)
        {

            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = ProgramaEducativoBusiness.Obtener(ref Codigo_Respuesta, idPrograma);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("EditarPrograma")]
        [HttpPost]
        public HttpResponseMessage Editar(ProgramaEducativo propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                if (ProgramaEducativoBusiness.ValidarDatos(propiedades, 2))
                {
                    Respuesta = ProgramaEducativoBusiness.Editar(propiedades, ref Codigo_Respuesta);
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
