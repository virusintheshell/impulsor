
//AUTOR: DIEGO OLVERA
//FECHA: 28-05-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA EL MODULO DE PROFESROES

namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using Framework;
    using DataAcess;
    using System.Net;
    using System.Collections;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Business.Notificaciones;

    public class ProfesoresBusiness
    {
        public static RespuestaGeneral Listado(ref HttpStatusCode Codigo_Respuesta, int estatus)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            string pagina = string.Empty;
            string paginaOrigen = string.Empty;

            ProfesoresDataAccess objProfesores = new ProfesoresDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            GeneralDataAccess objetoGeneral = new GeneralDataAccess();
            ListadoPermisosProfesores objListaProfesores = new ListadoPermisosProfesores();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina, ref paginaOrigen))
            {
                objListaProfesores.permisos = (List<ListaGenericaCatalogos>)objetoGeneral.Permisos(token, pagina, paginaOrigen, ref Codigo_Respuesta);

                if (objetoGeneral.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    objListaProfesores.profesores = (List<ListadoProfesores>)objProfesores.Listado(token, estatus, ref Codigo_Respuesta);
                    if (objProfesores.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                        codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                        Resultado = objListaProfesores;
                    }
                    else
                    {
                        mensaje = objProfesores.textoRespuesta;
                        codigoRespuesta = objProfesores.codigoRespuesta;
                        Resultado = new object { };
                    }
                }
                else
                {
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

        public static RespuestaGeneral GuardarAsingaturas(AsignacionProfesorAsignatura propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            ProfesoresDataAccess objProfesores = new ProfesoresDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objProfesores.GuardarAsignacion(token, pagina, propiedades, ref Codigo_Respuesta);

                if (objProfesores.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else
                {
                    mensaje = objProfesores.textoRespuesta;
                    codigoRespuesta = objProfesores.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ObtenerDatosAsingaturas(string idProfesor, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();
            object ResultadoConsulta = new object();

            ProfesoresDataAccess objPorofesores = new ProfesoresDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            AsignacionProfesorAsignatura obj = new AsignacionProfesorAsignatura();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ResultadoConsulta = (List<AsignaturasAsignadasProfesor>)objPorofesores.ListadoAsignaturasAsignadas(token, idProfesor, ref Codigo_Respuesta);
                if (objPorofesores.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ResultadoConsulta;
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ObtenerDetalleGrupoProfesor(string idGrupo, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            ProfesoresDataAccess objAsignaturas = new ProfesoresDataAccess();
            AlumnosDataAccess objAlumnos = new AlumnosDataAccess();

            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            FormularioDetalleGruposProfesor objListadoDetalleGrupoProfesor = new FormularioDetalleGruposProfesor();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                objListadoDetalleGrupoProfesor.asignaturas = (List<PropiedadesHorarioProfesor>)objAsignaturas.ListadoHorario(token, idGrupo, ref Codigo_Respuesta);
                if (objAsignaturas.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    int mostrarAlumnosCambio = 0;
                    objListadoDetalleGrupoProfesor.alumnos = (List<PropiedadesGrupoAlumnos>)objAlumnos.ListadoAlumnosGrupo(token, idGrupo, mostrarAlumnosCambio, ref Codigo_Respuesta);
                    if (objAlumnos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                        codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                        Resultado = objListadoDetalleGrupoProfesor;
                    }
                    else
                    {
                        mensaje = objAlumnos.textoRespuesta;
                        codigoRespuesta = objAlumnos.codigoRespuesta;
                        Resultado = new object { };
                    }
                }
                else
                {
                    mensaje = objAsignaturas.textoRespuesta;
                    codigoRespuesta = objAsignaturas.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Evaluaciones(filtrosCalificaion propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            ProfesoresDataAccess objCalificaciones = new ProfesoresDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            ParcialesAlumnos objeto = new ParcialesAlumnos();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                int calificacionFinal = 0;

                objeto.alumnos = (List<Parciales>)objCalificaciones.ListadoEvaluaciones(token, propiedades, ref calificacionFinal, ref Codigo_Respuesta);
                if (objCalificaciones.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    objeto.finalGradesSaved = calificacionFinal;

                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = objeto;
                }
                else
                {
                    mensaje = objCalificaciones.textoRespuesta;
                    codigoRespuesta = objCalificaciones.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral GuardarEvaluaciones(Evaluaciones propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            EvaluacionesDataAccess objEvaluaciones = new EvaluacionesDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objEvaluaciones.Guardar(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objEvaluaciones.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else
                {
                    mensaje = objEvaluaciones.textoRespuesta;
                    codigoRespuesta = objEvaluaciones.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral EditarEvaluaciones(EdicionEvaluaciones propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            EvaluacionesDataAccess objEvaluaciones = new EvaluacionesDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objEvaluaciones.Editar(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objEvaluaciones.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else
                {
                    mensaje = objEvaluaciones.textoRespuesta;
                    codigoRespuesta = objEvaluaciones.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Asistencias(filtrosCalificaion propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            ProfesoresDataAccess objAsistencias = new ProfesoresDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            ListadoAsistenciaAlumnos objeto = new ListadoAsistenciaAlumnos();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                objeto.alumnos = (List<AsistenciaAlumnos>)objAsistencias.ListadoAsistencias(token, propiedades, ref Codigo_Respuesta);
                if (objAsistencias.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {

                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = objeto;
                }
                else
                {
                    mensaje = objAsistencias.textoRespuesta;
                    codigoRespuesta = objAsistencias.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral GuardarAsistencias(Asistencia propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            AsistenciaDataAccess objAsistencia = new AsistenciaDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objAsistencia.Guardar(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objAsistencia.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else
                {
                    mensaje = objAsistencia.textoRespuesta;
                    codigoRespuesta = objAsistencia.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ListadoMisAsignaturas(ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();
            string pagina = string.Empty;

            ProfesoresDataAccess objProfesores = new ProfesoresDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            ListadoPropiedadesMisAsignaturas objMisAsignaturas = new ListadoPropiedadesMisAsignaturas();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                objMisAsignaturas.misAsignaturas = (List<PropiedadesMisAsignaturas>)objProfesores.ListadoMisAsignaturas(token, pagina, ref Codigo_Respuesta);

                if (objProfesores.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = objMisAsignaturas;
                }
                else
                {
                    mensaje = objProfesores.textoRespuesta;
                    codigoRespuesta = objProfesores.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral GuardarDocumentos(PropiedadesDocumentosProfesor propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            EnvioNotificacion objNotificacion = new EnvioNotificacion();
            ProfesoresDataAccess objDocumento = new ProfesoresDataAccess();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = objDocumento.GuardarDetalleDocumento(propiedades, token, ref Codigo_Respuesta);

                if (objDocumento.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;

                    //ENVIO DE NOTIFICACION 
                    objNotificacion.EnviarNotificacion(Convert.ToInt32(Resultado), Convert.ToInt16(Enumerados.TipoNotificacion.Sistesis), ref Codigo_Respuesta);
                }
                else
                {
                    mensaje = objDocumento.textoRespuesta;
                    codigoRespuesta = objDocumento.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }

            return ObjetoRespuesta;
        }

        public static RespuestaGeneral EditarDocumentos(PropiedadesDocumentosProfesor propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            ProfesoresDataAccess objDocumento = new ProfesoresDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = objDocumento.EditarDetalleDocumento(propiedades, token, ref Codigo_Respuesta);

                if (objDocumento.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else
                {
                    mensaje = objDocumento.textoRespuesta;
                    codigoRespuesta = objDocumento.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }

            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ListadoTareasEntregadas(string idTarea, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            string pagina = string.Empty;
            string paginaOrigen = string.Empty;

            ProfesoresDataAccess objProfesores = new ProfesoresDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            GeneralDataAccess objetoGeneral = new GeneralDataAccess();

            List<object> miLista = new List<object>();
            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                miLista = (List<object>)objProfesores.ListadoTareasEntregadas(token, idTarea, ref Codigo_Respuesta);
                if (objProfesores.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = miLista;
                }
                else
                {
                    mensaje = objProfesores.textoRespuesta;
                    codigoRespuesta = objProfesores.codigoRespuesta;
                    Resultado = new object { };
                }


                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static bool ValidarDatos(Asistencia propiedades)
        {
            try
            {
                bool respuesta = true;
                if (propiedades == null) { respuesta = false; return respuesta; }

                if (propiedades.idAsignatura == 0) { respuesta = false; }
                if (propiedades.fechaAsistencia == "") { respuesta = false; }
                if (propiedades.asistencias.Count == 0) { respuesta = false; }

                return respuesta;
            }
            catch (Exception exception) { throw exception; }
        }

        public static bool ValidarDatosEditarEvaluacion(EdicionEvaluaciones propiedades)
        {
            try
            {
                bool respuesta = true;
                if (propiedades == null) { respuesta = false; return respuesta; }
                if (propiedades.idGrupo == "") { respuesta = false; }
                if (propiedades.idAlumno == 0) { respuesta = false; }
                if (propiedades.idAsignatura == 0) { respuesta = false; }
                if (propiedades.calificaciones.Count == 0) { respuesta = false; }

                for (int i = 0; i < propiedades.calificaciones.Count; i++)
                {
                    int calificacion = Convert.ToInt16(propiedades.calificaciones[i]);
                    if (calificacion < 0 || calificacion > 10)
                    {
                        respuesta = false;
                    }
                }



                return respuesta;
            }
            catch (Exception exception) { throw exception; }
        }

    }
}
