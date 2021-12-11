
//AUTOR: DIEGO OLVERA
//FECHA: 07-05-2020
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA EL CAMBIO DE GRUPOS

namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using Framework;
    using DataAcess;
    using System.Net;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;

    public class CambioGrupoBusiness
    {
        public static RespuestaGeneral ObtenerProgramasEducativos(string idAlumno, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            CatalogosDataAccess objProgramasEducativos = new CatalogosDataAccess();
            FormularioCambioGrupo objListaCambioGrupo = new FormularioCambioGrupo();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            PropiedadesCatalogos filtroGeneral;

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                filtroGeneral = new PropiedadesCatalogos() { id = 0, cadenaIds = idAlumno, nombrePagina = "ProgramasAlumnos" };
                ObjetoRespuesta = objProgramasEducativos.ObtenerCatalogo(token, filtroGeneral, ref Codigo_Respuesta);

                objListaCambioGrupo.programaEducativos = (List<ListaGenericaCatalogos>)ObjetoRespuesta.result;
                if (objProgramasEducativos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = objListaCambioGrupo;
                }
                else
                {
                    mensaje = objProgramasEducativos.textoRespuesta;
                    codigoRespuesta = objProgramasEducativos.codigoRespuesta;
                    Resultado = new object { };
                }
                Codigo_Respuesta = HttpStatusCode.OK;
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ListadoProgramasDisponibles(int idPrograma, string idAlumno, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            CambioGrupoDataAccess obj = new CambioGrupoDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            GeneralDataAccess objetoGeneral = new GeneralDataAccess();
            ListaProgramasDisponibles objLista = new ListaProgramasDisponibles();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                objLista.programaEducativo = (DatosCambioGrupo)obj.ObtenerGruposDisponibles(token, idPrograma, idAlumno);
                if (obj.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = objLista;
                }
                else
                {
                    mensaje = obj.textoRespuesta;
                    codigoRespuesta = obj.codigoRespuesta;
                    Resultado = new object { };
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral GuardarCambioGrupo(CambioGrupo propiedades)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            CambioGrupoDataAccess objCambioGrupo = new CambioGrupoDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objCambioGrupo.GuardarCambioGrupo(propiedades, token);
                if (objCambioGrupo.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else
                {
                    mensaje = objCambioGrupo.textoRespuesta;
                    codigoRespuesta = objCambioGrupo.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }

            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ObtenerAsignaturasCambio(CambioGrupo propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();
            object ResultadoLista = new object(); 

            CambioGrupoDataAccess objCambioGrupo = new CambioGrupoDataAccess(); 
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ResultadoLista = (ListasAsignaturasCambioGrupo)objCambioGrupo.ObtenerAsignaturasCambio(propiedades, token); 
                if (objCambioGrupo.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ResultadoLista;
                }
                else
                {
                    mensaje = objCambioGrupo.textoRespuesta;
                    codigoRespuesta = objCambioGrupo.codigoRespuesta;
                    Resultado = new object { };
                }
                Codigo_Respuesta = HttpStatusCode.OK;
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        // METODOS PARA LA ACTUALIZACION DE CALIFICACIONES HISTORICAS

        public static RespuestaGeneral GuardarCalifiacionesHistoricas(AsignaturasAlumnoHistorico propiedades)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            CambioGrupoDataAccess objCambioGrupo = new CambioGrupoDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = objCambioGrupo.CalificacionesHistoricas(propiedades, token);
                if (objCambioGrupo.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else
                {
                    mensaje = objCambioGrupo.textoRespuesta;
                    codigoRespuesta = objCambioGrupo.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }

            return ObjetoRespuesta;
        }
        
        public static bool ValidarDatos(AsignaturasAlumnoHistorico propiedades)
        {
            try
            {
                bool respuesta = true;

                if (propiedades.idGrupo == "") { respuesta = false; return respuesta; }
                if (propiedades.nivel == 0) { respuesta = false; return respuesta; }
                if (propiedades.idAsignatura == 0) { respuesta = false; return respuesta; }
                if (propiedades.alumnos.Count == 0) { respuesta = false; return respuesta; }

                return respuesta;
            }
            catch (Exception exception) { throw exception; }
        }

        public static RespuestaGeneral EliminarCalificacionesHistoricas(PropiedadesCalificacionesHistoricas propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            CambioGrupoDataAccess objCambioGrupo = new CambioGrupoDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                 objCambioGrupo.EliminarCalificacionesHistoricas(propiedades, token, ref Codigo_Respuesta);
                if (objCambioGrupo.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = new object { };
                }
                else
                {
                    mensaje = objCambioGrupo.textoRespuesta;
                    codigoRespuesta = objCambioGrupo.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static bool ValidarDatosHistoricos(PropiedadesCalificacionesHistoricas propiedades)
        {
            try
            {
                bool respuesta = true;

                if (propiedades.idGrupo == "") { respuesta = false; return respuesta; }
                if (propiedades.nivel == 0) { respuesta = false; return respuesta; }
                if (propiedades.idAsignatura == 0) { respuesta = false; return respuesta; }
                
                return respuesta;
            }
            catch (Exception exception) { throw exception; }
        }

        // TERMINO DE METODOS PARA LA ACTUALIZACION DE CALIFICACIONES HISTORICAS
    }
}
