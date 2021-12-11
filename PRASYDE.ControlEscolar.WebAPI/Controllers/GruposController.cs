
namespace PRASYDE.ControlEscolar.WebAPI.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using PRASYDE.ControlEscolar.Business;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Business.Framework;

    [RoutePrefix("api")]
    public class GruposController : ApiController
    {
        [Route("FormularioGrupos/{idPlantel}")]
        [HttpGet]
        public HttpResponseMessage CargaFormulario(string idPlantel)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = GruposBusiness.ObtenerDatosFormulario(idPlantel, ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("ListadoGrupos/{estatus}/{tipo}")]
        [HttpGet]
        public HttpResponseMessage ObtenerListado(int estatus, int tipo)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = GruposBusiness.Listado(ref Codigo_Respuesta, estatus, tipo);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("AgregarGrupo")]
        [HttpPost]
        public HttpResponseMessage Guardar(Grupos propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = GruposBusiness.Guardar(propiedades, ref Codigo_Respuesta);
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

        [Route("EditarGrupo")]
        [HttpPost]
        public HttpResponseMessage Editar(GruposEdicion propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                if (GruposBusiness.ValidarDatos(propiedades))
                {
                    Respuesta = GruposBusiness.Editar(propiedades, ref Codigo_Respuesta);
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

        [Route("AgregarHorarioPorNivel")]
        [HttpPost]
        public HttpResponseMessage Guardar(HorariosPorNivel propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                if (GruposBusiness.ValidarDatosHorarioPorNivel(propiedades))
                {
                    Respuesta = GruposBusiness.GuardarHorarioPorNivel(propiedades, ref Codigo_Respuesta);
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
        
        [Route("AgregarAsignacionGrupoProfesor")]
        [HttpPost]
        public HttpResponseMessage GuardarAsignacionGrupoProfesor(PropiedadesGruposProfesor propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = GruposBusiness.GuardarAsignacionGrupoProfesor(propiedades, ref Codigo_Respuesta);
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
        
        [Route("ResumenGrupo/{idGrupo}/{nivel=0}")]
        [HttpGet]
        public HttpResponseMessage ObtenerResumenGrupo(string idGrupo,int nivel)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = GruposBusiness.ObtenerDetalleGrupo(ref Codigo_Respuesta, idGrupo, nivel);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("CerrarGrupo/{idGrupo}")]
        [HttpPost]
        public HttpResponseMessage Cerrar(string idGrupo)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = GruposBusiness.CerrarGrupo(idGrupo, ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("AsignaturasPorNivel/{idGrupo}")]
        [HttpGet]
        public HttpResponseMessage ObtenerListadoAsignaturasPorNivel(string idGrupo)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = GruposBusiness.ListadoAsignaturasPornivel(idGrupo, ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        #region "CONTROLADORES PRA EL CAMBIO DE GRUPOS"
        
        [Route("ListaProgramasEducativos/{idAlumno}")]
        [HttpGet]
        public HttpResponseMessage ObtenerProgramas(string idAlumno)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = CambioGrupoBusiness.ObtenerProgramasEducativos(idAlumno, ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("GruposDisponibles/{idAlumno}/{idPrograma}")]
        [HttpGet]
        public HttpResponseMessage ObtenerProgramasDisponibles(string idAlumno, int idPrograma)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = CambioGrupoBusiness.ListadoProgramasDisponibles(idPrograma, idAlumno, ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }


        [Route("ObtenerListaAsignaturas")]
        [HttpPost]
        public HttpResponseMessage ObtenerListaAsignaturasCAmbio(CambioGrupo propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = CambioGrupoBusiness.ObtenerAsignaturasCambio(propiedades, ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        [Route("CambiarGrupo")]
        [HttpPost]
        public HttpResponseMessage CambioGrupo(CambioGrupo propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                Respuesta = CambioGrupoBusiness.GuardarCambioGrupo(propiedades);
                return Request.CreateResponse(HttpStatusCode.OK, Respuesta);
            }
            else
            {
                Respuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                return Request.CreateResponse(Codigo_Respuesta, Respuesta);
            }
        }

        #endregion

        #region "CONTROLADORES PRA GUARDAR LAS CALIFIACIONES HISTORICAS"
        
        [Route("GuardarHistoricos")]
        [HttpPost]
        public HttpResponseMessage GuardarHistoricos(AsignaturasAlumnoHistorico propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                if (CambioGrupoBusiness.ValidarDatos(propiedades))
                {
                    Respuesta = CambioGrupoBusiness.GuardarCalifiacionesHistoricas(propiedades);
                    return Request.CreateResponse(HttpStatusCode.OK, Respuesta);
                }
                else
                {
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

        [Route("EliminarHistoricos")]
        [HttpPost]
        public HttpResponseMessage EliminarCalificaciones(PropiedadesCalificacionesHistoricas propiedades)
        {
            HttpStatusCode Codigo_Respuesta = new HttpStatusCode();
            RespuestaGeneral Respuesta = new RespuestaGeneral();

            if (ValidarSesion.tokenValido())
            {
                if (CambioGrupoBusiness.ValidarDatosHistoricos(propiedades))
                {
                    Respuesta = CambioGrupoBusiness.EliminarCalificacionesHistoricas(propiedades,ref Codigo_Respuesta);
                    return Request.CreateResponse(HttpStatusCode.OK, Respuesta);
                }
                else
                {
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
      
        #endregion
    }
}
