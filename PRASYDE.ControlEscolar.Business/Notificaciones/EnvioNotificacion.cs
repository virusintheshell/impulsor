
//AUTOR: DIEGO OLVERA
//FECHA: 12-07-2020
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA ENVIAR LAS NOTIFICACIONES DE ACUERDO A LISTA DE USUARIOS OBTENIDA

using System;
using System.Net;
using System.Collections.Generic;
using PRASYDE.ControlEscolar.Entities;
using PRASYDE.ControlEscolar.Business.Framework;
using PRASYDE.ControlEscolar.DataAcess.Notificaciones;

namespace PRASYDE.ControlEscolar.Business.Notificaciones
{
    public class EnvioNotificacion
    {
        public RespuestaGeneral EnviarNotificacion(int idReferencia, int tipo, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                try
                {
                    TokenActivosDataAccess objData = new TokenActivosDataAccess();
                    List<tokenFirebase> ListaNotificacion = new List<tokenFirebase>();

                    ListaNotificacion = (List<tokenFirebase>)objData.ListadoTokenUsuarios(token, idReferencia, tipo, ref Codigo_Respuesta);
                    if (objData.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        foreach (tokenFirebase destinatario in ListaNotificacion)
                        {
                            //PropiedadesMensajeEnvioNotificacion objMensajeFireBase = new PropiedadesMensajeEnvioNotificacion();
                            tokenFirebase objMensajeFireBase = new tokenFirebase(); 

                            objMensajeFireBase.id = destinatario.id;
                            //objMensajeFireBase.destinatario = destinatario.token.ToString();
                            objMensajeFireBase.token = destinatario.token.ToString();
                            objMensajeFireBase.title = destinatario.title.ToString();
                            objMensajeFireBase.body = destinatario.body.ToString();
                            objMensajeFireBase.icon = destinatario.icon.ToString();
                            NotificacionFireBase.EnviarNotificacionFireBase(objMensajeFireBase);
                        }
                    }
                    codigoRespuesta = objData.codigoRespuesta;
                    mensaje = objData.textoRespuesta;
                    Resultado = new object { };
                }
                catch (Exception ex)
                {
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                    mensaje = ex.ToString();
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ListadoNotificaciones(ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            NotificacionesDataAccess objNotificaciones = new NotificacionesDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            ListadoMisNotificaciones objLista = new ListadoMisNotificaciones();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                objLista.misNotificaciones = (List<PropiedadesMisNotificaciones>)objNotificaciones.ListadoMisNotificaciones(token, ref Codigo_Respuesta);

                if (objNotificaciones.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = objLista;
                }
                else
                {
                    mensaje = objNotificaciones.textoRespuesta;
                    codigoRespuesta = objNotificaciones.codigoRespuesta;
                    Resultado = new object { };
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral CambiarEstatusNotificaciones(int idReferencia, int tipoNotificacion,ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            NotificacionesDataAccess objNotificaciones = new NotificacionesDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            
            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                objNotificaciones.CambiarEstatusNotificacion(token, idReferencia, tipoNotificacion, ref Codigo_Respuesta);

                if (objNotificaciones.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = new object { };
                }
                else
                {
                    mensaje = objNotificaciones.textoRespuesta;
                    codigoRespuesta = objNotificaciones.codigoRespuesta;
                    Resultado = new object { };
                }

                Codigo_Respuesta = HttpStatusCode.OK;
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }
    }
}
