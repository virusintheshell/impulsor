
//AUTOR: DIEGO OLVERA
//FECHA: 27-08-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA EL MODULO DE TAREAS

namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using Framework;
    using DataAcess;
    using System.Net;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Business.Notificaciones;

    public class TareasBusiness
    {
        public static RespuestaGeneral Guardar(Tareas propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            EnvioNotificacion objNotificacion = new EnvioNotificacion();
            TareasDataAccess objTareas = new TareasDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objTareas.Guardar(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objTareas.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                    
                    //ENVIO DE NOTIFICACION 
                    objNotificacion.EnviarNotificacion(Convert.ToInt32(Resultado), Convert.ToInt16(Enumerados.TipoNotificacion.AsignacionTarea), ref Codigo_Respuesta);
                }
                else {
                    mensaje = objTareas.textoRespuesta;
                    codigoRespuesta = objTareas.codigoRespuesta;
                    Resultado = new object { };
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral EditarTareasProfesor(Tareas propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            TareasDataAccess objTareas = new TareasDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objTareas.EditarTareaProfesor(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objTareas.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objTareas.textoRespuesta;
                    codigoRespuesta = objTareas.codigoRespuesta;
                    Resultado = new object { };
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ObtenerTareas(int tipo,int estatus, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();
            int plataforma = 0;
            TareasDataAccess objTareas = new TareasDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
                      
            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref plataforma))
            {
                ObjetoRespuesta.result = objTareas.ObtenerTareas(token, tipo, plataforma, estatus, ref Codigo_Respuesta);

                if (objTareas.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objTareas.textoRespuesta;
                    codigoRespuesta = objTareas.codigoRespuesta;
                    Resultado = new object { };
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral GuardarTareasAlumnos(TareasAlumno propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            EnvioNotificacion objNotificacion = new EnvioNotificacion();
            TareasDataAccess objTareas = new TareasDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = objTareas.GuardarTareasAlumno(propiedades, token, ref Codigo_Respuesta);

                if (objTareas.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;

                    //ENVIO DE NOTIFICACION 
                    objNotificacion.EnviarNotificacion(Convert.ToInt32(Resultado), Convert.ToInt16(Enumerados.TipoNotificacion.TareaEntregada), ref Codigo_Respuesta);
                }
                else {
                    mensaje = objTareas.textoRespuesta;
                    codigoRespuesta = objTareas.codigoRespuesta;
                    Resultado = new object { };
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ObtenerRevisionTareas(TareasFiltroRevision propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            TareasDataAccess objTareas = new TareasDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = objTareas.ObtenerRevisionTareas(token, propiedades, ref Codigo_Respuesta);

                if (objTareas.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objTareas.textoRespuesta;
                    codigoRespuesta = objTareas.codigoRespuesta;
                    Resultado = new object { };
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }



        public static RespuestaGeneral GuardrComentariosTareas(ObjetoComentariosProfesor propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            TareasDataAccess objTareas = new TareasDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = objTareas.GuardrComentariosTareasProfesor(propiedades,token, ref Codigo_Respuesta);

                if (objTareas.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else
                {
                    mensaje = objTareas.textoRespuesta;
                    codigoRespuesta = objTareas.codigoRespuesta;
                    Resultado = new object { };
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static bool ValidarDatosTareasProfesor(Tareas propiedades, int tipo)
        {
            try
            {
                bool respuesta = true;
                if (propiedades == null) { respuesta = false; return respuesta; }
                if (tipo == 2)
                {
                    if (propiedades.idTarea == "" || propiedades.idTarea == null) { respuesta = false; return respuesta; }
                }
                if (propiedades.idGrupo == "" || propiedades.idGrupo == null) { respuesta = false; return respuesta; }
                if (propiedades.idAsignatura == 0) { respuesta = false; return respuesta; }
                if (propiedades.tipo == 0) { respuesta = false; return respuesta; }
                if (!Seguridad.TextoSimpleObligatorio(propiedades.titulo, 1, 800)) { respuesta = false; return respuesta; }
                if (!Seguridad.TextoSimpleNoObligatorio(propiedades.descripcion, 1, 800)) { respuesta = false; return respuesta; }
                if (propiedades.fechaEntrega == "") { respuesta = false; return respuesta; }

                return respuesta;
            }
            catch (Exception exception) { throw exception; }
        }

        public static bool ValidarDatosTareasAlumno(TareasAlumno propiedades)
        {
            try
            {
                bool respuesta = true;
                if (propiedades == null) { respuesta = false; return respuesta; }

                if (propiedades.idTarea == "" || propiedades.idTarea == null) { respuesta = false; return respuesta; }
                if (!Seguridad.TextoSimpleNoObligatorio(propiedades.comentarios, 1, 800)) { respuesta = false; return respuesta; }
                return respuesta;
            }
            catch (Exception exception) { throw exception; }
        }
    }
}
