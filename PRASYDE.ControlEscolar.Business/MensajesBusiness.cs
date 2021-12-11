
//AUTOR: DIEGO OLVERA
//FECHA: 25-08-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA EL MODULO DE MENSAJES

namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using Framework;
    using DataAcess;
    using System.Net;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Business.Notificaciones;

    public class MensajesBusiness
    {
        public static RespuestaGeneral Guardar(Mensajes propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            EnvioNotificacion objNotificacion = new EnvioNotificacion(); 
            MensajesDataAccess objMensajes = new MensajesDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = objMensajes.Guardar(propiedades, token, ref Codigo_Respuesta);

                if (objMensajes.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;

                    //ENVIO DE NOTIFICACION 
                    objNotificacion.EnviarNotificacion(Convert.ToInt32(Resultado), Convert.ToInt16(Enumerados.TipoNotificacion.Chat), ref Codigo_Respuesta);
                }
                else {
                    mensaje = objMensajes.textoRespuesta;
                    codigoRespuesta = objMensajes.codigoRespuesta;
                    Resultado = new object { };
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }

            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ObtenerListaUsuarios(ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            MensajesDataAccess objUsuarios = new MensajesDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = objUsuarios.ObtenerListaUsuarios(token,ref Codigo_Respuesta);

                if (objUsuarios.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objUsuarios.textoRespuesta;
                    codigoRespuesta = objUsuarios.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ObtenerMensajes(string idUsuario, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            MensajesDataAccess objMensajes = new MensajesDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = objMensajes.ObtenerMensjes(token,idUsuario, ref Codigo_Respuesta);

                if (objMensajes.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objMensajes.textoRespuesta;
                    codigoRespuesta = objMensajes.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }
        
        public static bool ValidarDatos(Mensajes propiedades)
        {
            try
            {
                bool respusta = true;
                if (propiedades == null) { respusta = false; return respusta; }
                if (Seguridad.TextoSimpleObligatorio(propiedades.mensaje, 1, 800) == false) { respusta = false; return respusta; }
                return respusta;
            }
            catch (Exception exception) { throw exception; }
        }
    }
}
