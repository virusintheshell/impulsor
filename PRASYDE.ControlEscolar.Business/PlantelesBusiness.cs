
//AUTOR: DIEGO OLVERA
//FECHA: 01-04-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA GUARDAR LOS PANTELES

namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using DataAcess;
    using Framework;
    using System.Web;
    using System.Net;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;

    public class PlantelesBusiness
    {
        public static RespuestaGeneral Listado(ref HttpStatusCode Codigo_Respuesta, int estatus)
        {

            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (HttpContext.Current.Request.Headers["Plataforma"] != null)
            {
                int idEmpresa = Convert.ToInt16(HttpContext.Current.Request.Headers["Empresa"]);
                ObjetoRespuesta = ListadoMovil(ref Codigo_Respuesta, idEmpresa,estatus);
            }
            else {

                if (ValidarSesion.tokenValido())
                {
                    ObjetoRespuesta = ListadoWeb(ref Codigo_Respuesta, estatus);
                }
                else {
                    ObjetoRespuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                }
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ListadoWeb(ref HttpStatusCode Codigo_Respuesta, int estatus)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            string pagina = string.Empty;
            string paginaOrigen = string.Empty;

            PlantelDataAccess objPlanteles = new PlantelDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            GeneralDataAccess objetoGeneral = new GeneralDataAccess();
            ListadoPermisosPlanteles objListaPlanteles = new ListadoPermisosPlanteles();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina, ref paginaOrigen))
            {
                objListaPlanteles.permisos = (List<ListaGenericaCatalogos>)objetoGeneral.Permisos(token, pagina, paginaOrigen, ref Codigo_Respuesta);
                if (objetoGeneral.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    objListaPlanteles.planteles = (List<Planteles>)objPlanteles.Listado(token, 0,0, estatus, ref Codigo_Respuesta);
                    if (objPlanteles.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                        codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                        Resultado = objListaPlanteles;
                    }
                    else {
                        mensaje = objPlanteles.textoRespuesta;
                        codigoRespuesta = objPlanteles.codigoRespuesta;
                        Resultado = new object { };
                    }
                }
                else {
                    mensaje = objetoGeneral.textoRespuesta;
                    codigoRespuesta = objetoGeneral.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ListadoMovil(ref HttpStatusCode Codigo_Respuesta, int idEmpresa, int estatus)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            int idPlataforma = 0;

            PlantelDataAccess objPlanteles = new PlantelDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            GeneralDataAccess objetoGeneral = new GeneralDataAccess();
            ListadoPermisosPlantelesMovil objListaPlanteles = new ListadoPermisosPlantelesMovil();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref idPlataforma))
            {
                objListaPlanteles.planteles = (List<Planteles>)objPlanteles.Listado(token, idPlataforma, idEmpresa, estatus, ref Codigo_Respuesta);
                if (objPlanteles.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = objListaPlanteles;
                }
                else {
                    mensaje = objPlanteles.textoRespuesta;
                    codigoRespuesta = objPlanteles.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Guardar(Planteles propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            PlantelDataAccess objPlanteles = new PlantelDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objPlanteles.Guardar(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objPlanteles.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objPlanteles.textoRespuesta;
                    codigoRespuesta = objPlanteles.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Obtener(ref HttpStatusCode Codigo_Respuesta, string idPlantel)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            PlantelDataAccess objCampus = new PlantelDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objCampus.ObtenerPlantel(token, pagina, idPlantel, ref Codigo_Respuesta);

                if (objCampus.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objCampus.textoRespuesta;
                    codigoRespuesta = objCampus.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Editar(Planteles propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            PlantelDataAccess objPlantel = new PlantelDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objPlantel.Editar(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objPlantel.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objPlantel.textoRespuesta;
                    codigoRespuesta = objPlantel.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ListadoAsignacion(ref HttpStatusCode Codigo_Respuesta, string idPlantel)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            PlantelDataAccess objPlantel = new PlantelDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = objPlantel.ArmarFormularioAsignacion(token, idPlantel, ref Codigo_Respuesta);

                if (objPlantel.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objPlantel.textoRespuesta;
                    codigoRespuesta = objPlantel.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral GuardaAsignacion(PropiedadesAsignacion propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            PlantelDataAccess objPlanteles = new PlantelDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objPlanteles.GuardarAsignacion(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objPlanteles.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objPlanteles.textoRespuesta;
                    codigoRespuesta = objPlanteles.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static bool ValidarDatos(Planteles propiedades, int tipo)
        {
            try
            {
                bool respuesta = true;
                if (propiedades == null) { respuesta = false; return respuesta; }
                if (tipo == 2)
                {
                    if (propiedades.idPlantel == "") { respuesta = false; return respuesta; }
                }

                if (!Seguridad.TextoSimpleObligatorio(propiedades.nombre, 1, 150)) { respuesta = false; return respuesta; }
                if (!Seguridad.TextoSimpleObligatorio(propiedades.direccion, 1, 250)) { respuesta = false; return respuesta; }
                if (!Seguridad.TextoSimpleNoObligatorio(propiedades.referencia, 1, 250)) { respuesta = false; return respuesta; }
                if (!Seguridad.letrasNoObligatorias(propiedades.contacto, 1, 150)) { respuesta = false; return respuesta; }
                if (!string.IsNullOrEmpty(propiedades.correoElectronico) && !Seguridad.ValidaCorreo(propiedades.correoElectronico) == true) { respuesta = false; return respuesta; }
                if (!Seguridad.NumeroTelefonicoNoObligatorio(propiedades.telefono)) { respuesta = false; return respuesta; }
                if (!Seguridad.NumerosNoObligatorios(propiedades.extension, 1, 5)) { respuesta = false; return respuesta; }

                return respuesta;
            }
            catch (Exception exception) { throw exception; }
        }

    }
}
