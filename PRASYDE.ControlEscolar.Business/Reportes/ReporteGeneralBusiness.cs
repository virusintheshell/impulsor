
//AUTOR: DIEGO OLVERA
//FECHA: 07-05-2020
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA EL CAMBIO DE GRUPOS

namespace PRASYDE.ControlEscolar.Business.Reportes
{
    using System;
    using Framework;
    using System.Net;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.DataAcess.Reportes;

    public class ReporteGeneralBusiness
    {
        public static RespuestaGeneral Obtener(ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            ReporteGeneralDataAccess obj = new ReporteGeneralDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = obj.ObtenerReportGeneral(token, ref Codigo_Respuesta);

                if (obj.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else
                {
                    mensaje = obj.textoRespuesta;
                    codigoRespuesta = obj.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ObtenerReportePagos(ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            ReporteGeneralDataAccess obj = new ReporteGeneralDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = obj.ObtenerReportePagosMensuales(token, ref Codigo_Respuesta);

                if (obj.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else
                {
                    mensaje = obj.textoRespuesta;
                    codigoRespuesta = obj.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ObtenerReporteInscripciones(ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            ReporteGeneralDataAccess obj = new ReporteGeneralDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = obj.ObtenerReporteInscripciones(token, ref Codigo_Respuesta);

                if (obj.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else
                {
                    mensaje = obj.textoRespuesta;
                    codigoRespuesta = obj.codigoRespuesta;
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
