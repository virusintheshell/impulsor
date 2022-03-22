//AUTOR: DIEGO OLVERA
//FECHA: 25-11-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA LA EDICION DE EVALUACIONES FINALES


namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using Framework;
    using DataAcess;
    using System.Net;
    using PRASYDE.ControlEscolar.Entities;

    public class EvaluacionesBusiness
    {
        public static RespuestaGeneral Editar(EdicionCalificacionFinal propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            EvaluacionesDataAccess objEvaluacionFinal = new EvaluacionesDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = objEvaluacionFinal.EditarEvaluacionFinal(propiedades, token, ref Codigo_Respuesta);

                if (objEvaluacionFinal.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objEvaluacionFinal.textoRespuesta;
                    codigoRespuesta = objEvaluacionFinal.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static bool ValidarDatos(EdicionCalificacionFinal propiedades)
        {
            try
            {
                bool respuesta = true;
                if (propiedades == null) { respuesta = false; return respuesta; }

                if (propiedades.idDetalleEvaluacion == 0) { respuesta = false; return respuesta; }
                if (propiedades.tipo> 2) { respuesta = false; return respuesta; }
                if (!Seguridad.NumerosDecimales(propiedades.calificacionFinal.ToString())) { respuesta = false; return respuesta; }

                return respuesta;
            }
            catch (Exception exception) { throw exception; }
        }

        public static ExcelSheetResponse ExportarEvaluaciones(string idGrupo, int nivel)
        {
            Guid token = Guid.Empty;

            EvaluacionesDataAccess obj = new EvaluacionesDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            ExcelSheetResponse response = new ExcelSheetResponse();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                response = obj.ExportarCalificacionesExcel(idGrupo, nivel);
            }

            return response;
        }
    }
}
