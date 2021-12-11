
//AUTOR: DIEGO OLVERA
//FECHA: 07-04-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA EL MODULO DE ALUMNOS

namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using Framework;
    using DataAcess;
    using System.Net;
    using System.Web;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;

    public class AlumnosBusiness
    {
        public static RespuestaGeneral Listado(ref HttpStatusCode Codigo_Respuesta, int estatus)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            string pagina = string.Empty;
            string paginaOrigen = string.Empty;

            AlumnosDataAccess objAlumnos = new AlumnosDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            GeneralDataAccess objetoGeneral = new GeneralDataAccess();
            ListadoPermisosAlumnos objListaAlumnos = new ListadoPermisosAlumnos();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina, ref paginaOrigen))
            {
                objListaAlumnos.permisos = (List<ListaGenericaCatalogos>)objetoGeneral.Permisos(token, pagina, paginaOrigen, ref Codigo_Respuesta);

                if (objetoGeneral.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    objListaAlumnos.alumnos = (List<ListadoAlumnos>)objAlumnos.Listado(token, estatus, ref Codigo_Respuesta);
                    if (objAlumnos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                        codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                        Resultado = objListaAlumnos;
                    }
                    else {
                        mensaje = objAlumnos.textoRespuesta;
                        codigoRespuesta = objAlumnos.codigoRespuesta;
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

        public static RespuestaGeneral DetalleAlumno(ref HttpStatusCode Codigo_Respuesta, string idUsuario)
        {
            Guid token = Guid.Empty;
            int plataforma = 0;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            UsuariosDataAccess objUsuario = new UsuariosDataAccess();
            AlumnosDataAccess objAlumnos = new AlumnosDataAccess();

            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            ListadoDetalleAlumnos objListaDetalleAlumno = new ListadoDetalleAlumnos();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                if (HttpContext.Current.Request.Headers["Plataforma"] != null)
                {
                     plataforma = Convert.ToInt16(HttpContext.Current.Request.Headers["Plataforma"]);
                }

                if (plataforma == 2)
                {
                    idUsuario = objUsuario.ObtenerIdUsuario(token, ref Codigo_Respuesta);
                }

                objListaDetalleAlumno.usuario = (Usuarios)objUsuario.Obtener(token, "usuarios", idUsuario, ref Codigo_Respuesta);
                if (objUsuario.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    objListaDetalleAlumno.inscripciones = (List<AlumnoDetalleInscripcion>)objAlumnos.DetalleInscripcion(token, plataforma,idUsuario, ref Codigo_Respuesta);
                    if (objAlumnos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        objListaDetalleAlumno.pagos = (List<ListaPagosAlumno>)objAlumnos.DetallePagos(token, idUsuario, ref Codigo_Respuesta);
                        if (objAlumnos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                        {
                            Codigo_Respuesta = HttpStatusCode.OK;
                            mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                            codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                            Resultado = objListaDetalleAlumno;
                        }
                        else {
                            mensaje = objAlumnos.textoRespuesta;
                            codigoRespuesta = objAlumnos.codigoRespuesta;
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
                    mensaje = objUsuario.textoRespuesta;
                    codigoRespuesta = objUsuario.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

    }
}
