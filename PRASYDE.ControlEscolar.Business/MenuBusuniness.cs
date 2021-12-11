
//AUTOR: DIEGO OLVERA
//FECHA: 29-03-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA ARMAR EN MENU 

namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using Framework;
    using DataAcess;
    using System.Net;
    using PRASYDE.ControlEscolar.Entities;
   
    public class MenuBusuniness
    {
        public static RespuestaGeneral ObtenerMenu(ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();
                        
            MenuDataAcceess objMenu = new MenuDataAcceess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            
            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = objMenu.ObtenerMenu(Convert.ToString(token).Trim(), ref Codigo_Respuesta);
               
                if (objMenu.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objMenu.textoRespuesta;
                    codigoRespuesta = objMenu.codigoRespuesta;
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
