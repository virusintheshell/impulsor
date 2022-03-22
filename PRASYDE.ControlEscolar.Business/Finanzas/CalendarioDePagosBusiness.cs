
//AUTOR: DIEGO OLVERA
//FECHA: 03-03-2022
//DESCRIPCIÓN: CLASE QUE GESTIONA LOS METOOOS PARA CREAR EL CALENDARAIO DE PAGOS

namespace PRASYDE.ControlEscolar.Business.Finanzas
{
    using System;
    using Framework;
    using DataAcess;
    using System.Net;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Entities.Finanzas;
    using PRASYDE.ControlEscolar.DataAcess.Finanzas;

    public class CalendarioDePagosBusiness
    {
        public static RespuestaGeneral Listado(CalendarioDePagos propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            string pagina = string.Empty;
            string paginaOrigen = string.Empty;

            CalendariosDePagosDataAccess objCalendario = new CalendariosDePagosDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            GeneralDataAccess objetoGeneral = new GeneralDataAccess();
            formularioCalendarioPagos objFormularioCalendario = new formularioCalendarioPagos();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina, ref paginaOrigen))
            {
                objFormularioCalendario.permisos = (List<ListaGenericaCatalogos>)objetoGeneral.Permisos(token, pagina, paginaOrigen, ref Codigo_Respuesta);
                if (objetoGeneral.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    objFormularioCalendario.listaCalendarioPagos = (List<ListadoCalendarioPagos>)objCalendario.ListadoCalendario(token, propiedades, ref Codigo_Respuesta);
                    if (objCalendario.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                        codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                        Resultado = objFormularioCalendario;
                    }
                    else
                    {
                        mensaje = objCalendario.textoRespuesta;
                        codigoRespuesta = objCalendario.codigoRespuesta;
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
    }
}
