
//AUTOR: DIEGO OLVERA
//FECHA: 06-04-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA EL MODULO DE PROGRAMA EDUCATIVO

namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using Framework;
    using DataAcess;
    using System.Net;
    using System.Web;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;
   
    public class ProgramaEducativoBusiness
    {
        public static RespuestaGeneral ObtenerDatosFormulario(ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            CatalogosDataAccess objProgramaEducativo = new CatalogosDataAccess();
            AsignaturaDataAccess objAsignatura = new AsignaturaDataAccess();
            DocumentosDataAccess objDocumentos = new DocumentosDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            ProgramaEducativoDataAccess objGradosEducativos = new ProgramaEducativoDataAccess();

            FormularioProgramaEducativo objListaPrograma = new FormularioProgramaEducativo();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                objListaPrograma.gradosEducativos = (List<GradosEcutivosPrograma>)objGradosEducativos.ObtenerGradosEducativos(token);
                if (objGradosEducativos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    objListaPrograma.listaAsignaturas = (List<ListadoAsingaturas>)objAsignatura.Listado(token, 1, ref Codigo_Respuesta);
                    if (objAsignatura.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        objListaPrograma.listaDocumentos = (List<ListaDocumentosPrograma>)objDocumentos.Obtener(token, "", ref Codigo_Respuesta);
                        if (objDocumentos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                        {
                            Codigo_Respuesta = HttpStatusCode.OK;
                            mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                            codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                            Resultado = objListaPrograma;
                        }
                        else {
                            mensaje = objDocumentos.textoRespuesta;
                            codigoRespuesta = objDocumentos.codigoRespuesta;
                            Resultado = new object { };
                        }
                    }
                    else {
                        mensaje = objAsignatura.textoRespuesta;
                        codigoRespuesta = objAsignatura.codigoRespuesta;
                        Resultado = new object { };
                    }
                }
                else {
                    mensaje = objProgramaEducativo.textoRespuesta;
                    codigoRespuesta = objProgramaEducativo.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Listado(ref HttpStatusCode Codigo_Respuesta, int estatus,string idAlumno)
        {
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (HttpContext.Current.Request.Headers["Plataforma"] != null)
            {
                int idEmpresa = Convert.ToInt16(HttpContext.Current.Request.Headers["Empresa"]);
                ObjetoRespuesta = ListadoMovil(ref Codigo_Respuesta, idEmpresa, estatus);
            }
            else {

                if (ValidarSesion.tokenValido())
                {
                    ObjetoRespuesta = ListadoWeb(ref Codigo_Respuesta, estatus, idAlumno);
                }
                else {
                    ObjetoRespuesta = General.SinAutorizacion(ref Codigo_Respuesta);
                }
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ListadoWeb(ref HttpStatusCode Codigo_Respuesta, int estatus,string idAlumno)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            string pagina = string.Empty;
            string paginaOrigen = string.Empty;
            int plataforma = 1;
            int tipoListado = 0;
            ProgramaEducativoDataAccess objPrograma = new ProgramaEducativoDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            GeneralDataAccess objetoGeneral = new GeneralDataAccess();
            ListadoPermisosProgramaEducativo objListaPrograma = new ListadoPermisosProgramaEducativo();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina, ref paginaOrigen, ref tipoListado))
            {
                objListaPrograma.permisos = (List<ListaGenericaCatalogos>)objetoGeneral.Permisos(token, pagina, paginaOrigen, ref Codigo_Respuesta);

                if (objetoGeneral.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    objListaPrograma.programaEducativo = (List<ListadoProgramaEducativo>)objPrograma.Listado(token, estatus,0, tipoListado, plataforma,ref Codigo_Respuesta, idAlumno);
                    if (objPrograma.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                        codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                        Resultado = objListaPrograma;
                    }
                    else {
                        mensaje = objPrograma.textoRespuesta;
                        codigoRespuesta = objPrograma.codigoRespuesta;
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

        public static RespuestaGeneral ListadoMovil(ref HttpStatusCode Codigo_Respuesta,int idEmpresa, int estatus)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();
            int plataforma = 0;

            int tipoListado = 0;
            ProgramaEducativoDataAccess objPrograma = new ProgramaEducativoDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            ListadoPermisosProgramaEducativoMovil objListaPrograma = new ListadoPermisosProgramaEducativoMovil();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref plataforma))
            {
                plataforma = 2;
                objListaPrograma.programaEducativo = (List<ListadoProgramaEducativo>)objPrograma.Listado(token, estatus, idEmpresa, tipoListado, plataforma, ref Codigo_Respuesta);
                if (objPrograma.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = objListaPrograma;
                }
                else {
                    mensaje = objPrograma.textoRespuesta;
                    codigoRespuesta = objPrograma.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Guardar(ProgramaEducativo propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            ProgramaEducativoDataAccess objPrograma = new ProgramaEducativoDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (propiedades != null)
            {

                if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
                {
                    ObjetoRespuesta.result = objPrograma.Guardar(propiedades, token, pagina, ref Codigo_Respuesta);

                    if (objPrograma.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                        codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                        Resultado = ObjetoRespuesta.result;
                    }
                    else {
                        mensaje = objPrograma.textoRespuesta;
                        codigoRespuesta = objPrograma.codigoRespuesta;
                        Resultado = new object { };
                    }

                    ObjetoRespuesta.status = codigoRespuesta;
                    ObjetoRespuesta.message = mensaje;
                    ObjetoRespuesta.result = Resultado;
                }
            }
            else {

            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Obtener(ref HttpStatusCode Codigo_Respuesta, string idPrograma)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            ProgramaEducativoDataAccess objPrograma = new ProgramaEducativoDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objPrograma.Obtener(token, pagina, idPrograma, ref Codigo_Respuesta);

                if (objPrograma.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objPrograma.textoRespuesta;
                    codigoRespuesta = objPrograma.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Editar(ProgramaEducativo propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            ProgramaEducativoDataAccess objPrograma = new ProgramaEducativoDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objPrograma.Editar(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objPrograma.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objPrograma.textoRespuesta;
                    codigoRespuesta = objPrograma.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static bool ValidarDatos(ProgramaEducativo propiedades, int tipo)
        {
            try
            {
                bool respusta = true;
                if (tipo == 2)
                {
                    if (propiedades.idProgramaEducativo == "") { respusta = false; return respusta; }
                }
                if (propiedades.gradoEducativo == 0) { respusta = false; return respusta; }
                if (Seguridad.TextoSimpleObligatorio(propiedades.nombre, 1, 50) == false) { respusta = false; return respusta; }
                if (Seguridad.TextoSimpleNoObligatorio(propiedades.descripcion, 1, 250) == false) { respusta = false; return respusta; }
                if (Seguridad.TextoSimpleNoObligatorio(propiedades.perfilIngreso, 1, 250) == false) { respusta = false; return respusta; }
                if (Seguridad.TextoSimpleNoObligatorio(propiedades.perfilEgreso, 1, 250) == false) { respusta = false; return respusta; }
                if (Seguridad.TextoSimpleNoObligatorio(propiedades.requisitos, 1, 250) == false) { respusta = false; return respusta; }

                return respusta;
            }
            catch (Exception exception) { throw exception; }
        }
    }
}
