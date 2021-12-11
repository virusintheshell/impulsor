
using System;
using System.Net;
using PRASYDE.ControlEscolar.Entities;

namespace PRASYDE.ControlEscolar.Business.Framework
{
    public class General
    {
        public static RespuestaGeneral SinAutorizacion(ref HttpStatusCode Codigo_Respuesta)
        {
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            Codigo_Respuesta = HttpStatusCode.OK;
            ObjetoRespuesta.status = Convert.ToInt16(Enumerados.Codigos_Respuesta.Unauthorized);
            ObjetoRespuesta.message = "No está autorizado para ver el recurso";
            ObjetoRespuesta.result = new object { };

            return ObjetoRespuesta;
        }

        public static RespuestaGeneral DatosInvalidos(ref HttpStatusCode Codigo_Respuesta)
        {
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            Codigo_Respuesta = HttpStatusCode.OK;
            ObjetoRespuesta.status = Convert.ToInt16(Enumerados.Codigos_Respuesta.ErrorData);
            ObjetoRespuesta.message = "Datos invalidos";
            ObjetoRespuesta.result = new object { };

            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Error(ref HttpStatusCode Codigo_Respuesta, string mensajeError)
        {
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            Codigo_Respuesta = HttpStatusCode.InternalServerError;
            ObjetoRespuesta.status = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
            ObjetoRespuesta.message = mensajeError;
            ObjetoRespuesta.result = new object { };

            return ObjetoRespuesta;
        }
        
        public static RespuestaGeneral NoContent(ref HttpStatusCode Codigo_Respuesta)
        {
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            Codigo_Respuesta = HttpStatusCode.OK;
            ObjetoRespuesta.status = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            ObjetoRespuesta.message = "Sin datos";
            ObjetoRespuesta.result = new object { };

            return ObjetoRespuesta;
        }

        public static RespuestaGeneral UnsupportedMediaType()
        {
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            ObjetoRespuesta.status = Convert.ToInt16(HttpStatusCode.UnsupportedMediaType);
            ObjetoRespuesta.message = "UnsupportedMediaType";
            ObjetoRespuesta.result = new object { };

            return ObjetoRespuesta;
        }
    }
}