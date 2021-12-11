
//AUTOR: DIEGO OLVERA
//FECHA: 23-05-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA EL MODULO DE GRUPOS

namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using Framework;
    using DataAcess;
    using System.Net;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;

    public class GruposBusiness
    {
        public static RespuestaGeneral Listado(ref HttpStatusCode Codigo_Respuesta, int estatus, int tipo)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            string pagina = string.Empty;
            string paginaOrigen = string.Empty;

            GruposDataAccess objGrupos = new GruposDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            GeneralDataAccess objetoGeneral = new GeneralDataAccess();
            ListadoPermisosGrupos objListaGrupos = new ListadoPermisosGrupos();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina, ref paginaOrigen))
            {
                objListaGrupos.permisos = (List<ListaGenericaCatalogos>)objetoGeneral.Permisos(token, pagina, paginaOrigen, ref Codigo_Respuesta);
                if (objetoGeneral.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    objListaGrupos.grupos = (List<ListadoGrupos>)objGrupos.Listado(token, estatus, tipo, ref Codigo_Respuesta);
                    if (objGrupos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                        codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                        Resultado = objListaGrupos;
                    }
                    else {
                        mensaje = objGrupos.textoRespuesta;
                        codigoRespuesta = objGrupos.codigoRespuesta;
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

        public static RespuestaGeneral ListadoAsignaturasPornivel(string idGrupo, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            string pagina = string.Empty;
            string paginaOrigen = string.Empty;

            GruposDataAccess objGrupos = new GruposDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            GeneralDataAccess objetoGeneral = new GeneralDataAccess();
            ListadoAsignaturasPorNivel objAsignaturas = new ListadoAsignaturasPorNivel();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                objAsignaturas.asignaturas = (List<PropiedadesAsignaturasPorNivel>)objGrupos.ListadoAsignaturasPorNivel(token, idGrupo, ref Codigo_Respuesta);
                if (objGrupos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = objAsignaturas;
                }
                else {
                    mensaje = objGrupos.textoRespuesta;
                    codigoRespuesta = objGrupos.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Guardar(Grupos propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            GruposDataAccess objGrupos = new GruposDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objGrupos.Guardar(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objGrupos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objGrupos.textoRespuesta;
                    codigoRespuesta = objGrupos.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Editar(GruposEdicion propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            GruposDataAccess objGrupos = new GruposDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objGrupos.Editar(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objGrupos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objGrupos.textoRespuesta;
                    codigoRespuesta = objGrupos.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }
        
        public static RespuestaGeneral GuardarHorarioPorNivel(HorariosPorNivel propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            GruposDataAccess objGrupos = new GruposDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objGrupos.GuardarHorarioPorNivel(propiedades, ref Codigo_Respuesta);

                if (objGrupos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objGrupos.textoRespuesta;
                    codigoRespuesta = objGrupos.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }
        
        public static RespuestaGeneral ObtenerDatosFormulario(string idPlantel, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            GruposDataAccess objGrupos = new GruposDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            FormularioGrupos objListaGrupos = new FormularioGrupos();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = objGrupos.ArmarFormularioGrupos(token, idPlantel, ref Codigo_Respuesta);

                objListaGrupos.gradosEducativos = (List<GruposGrados>)ObjetoRespuesta.result;
                if (objGrupos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = objListaGrupos;
                }
                else {
                    mensaje = objGrupos.textoRespuesta;
                    codigoRespuesta = objGrupos.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral GuardarAsignacionGrupoProfesor(PropiedadesGruposProfesor propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            GruposDataAccess objGrupos = new GruposDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = objGrupos.GuardarAsignacionGrupoProfesor(propiedades, token, ref Codigo_Respuesta);

                if (objGrupos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objGrupos.textoRespuesta;
                    codigoRespuesta = objGrupos.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ObtenerDetalleGrupo(ref HttpStatusCode Codigo_Respuesta, string idGrupo, int nivel)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            GruposDataAccess objGrupos = new GruposDataAccess();
            AlumnosDataAccess objAlumnos = new AlumnosDataAccess();

            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            FormularioDetalleGrupos objListadoDetalleGrupo = new FormularioDetalleGrupos();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                objListadoDetalleGrupo.asignaturas = (List<PropiedadesHorarioAsignaturas>)objGrupos.ListadoHorario(token, idGrupo, nivel, ref Codigo_Respuesta);
                if (objGrupos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    int mostrarAlumnosCambio = 1;
                    objListadoDetalleGrupo.alumnos = (List<PropiedadesGrupoAlumnos>)objAlumnos.ListadoAlumnosGrupo(token, idGrupo, mostrarAlumnosCambio, ref Codigo_Respuesta);
                    if (objGrupos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        objListadoDetalleGrupo.asignaturasProfesor = (GrupoAsingaturas)objGrupos.ObtenerListaAsignaturasProfesor(token, idGrupo, nivel, ref Codigo_Respuesta);
                        if (objGrupos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                        {
                            Codigo_Respuesta = HttpStatusCode.OK;
                            mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                            codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                            Resultado = objListadoDetalleGrupo;
                        }
                        else {
                            mensaje = objGrupos.textoRespuesta;
                            codigoRespuesta = objGrupos.codigoRespuesta;
                            Resultado = new object { };
                        }
                    }
                    else {
                        mensaje = objAlumnos.textoRespuesta;
                        codigoRespuesta = objAlumnos.codigoRespuesta;
                        Resultado = new object { };
                    }
                }
                else {
                    mensaje = objGrupos.textoRespuesta;
                    codigoRespuesta = objGrupos.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral CerrarGrupo(string idGrupo, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            GruposDataAccess objGrupos = new GruposDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = objGrupos.CerrarGrupo(token, idGrupo, ref Codigo_Respuesta);

                if (objGrupos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objGrupos.textoRespuesta;
                    codigoRespuesta = objGrupos.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static bool ValidarDatosHorarioPorNivel(HorariosPorNivel propiedades)
        {
            try
            {
                bool respuesta = true;
                if (propiedades == null) { respuesta = false; return respuesta; }
                if (propiedades.idGrupo == "") { respuesta = false; return respuesta; }
                if (propiedades.horario.Count == 0) { respuesta = false; return respuesta; }
               
                return respuesta;
            }
            catch (Exception exception) { throw exception; }
        }

        public static bool ValidarDatos(GruposEdicion propiedades)
        {
            try
            {
                bool respuesta = true;
                if (propiedades == null) { respuesta = false; return respuesta; }

                if (propiedades.idGrupo == "") { respuesta = false; return respuesta; }
                if (!Seguridad.TextoSimpleObligatorio(propiedades.nombreGrupo, 1, 90)) { respuesta = false; return respuesta; }
                if (!Seguridad.NumerosObligatorios(propiedades.cupo.ToString(), 1, 3)) { respuesta = false; return respuesta; }
                
                return respuesta;
            }
            catch (Exception exception) { throw exception; }
        }
    }
}
