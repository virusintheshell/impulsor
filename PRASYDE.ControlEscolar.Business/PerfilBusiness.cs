
//AUTOR: DIEGO OLVERA
//FECHA: 29-03-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA ARMAR EL PERFIL DEL USUARIO


namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using DataAcess;
    using Framework;
    using System.Net;
    using PRASYDE.ControlEscolar.Entities;
  
    public class PerfilBusiness
    {
        public static RespuestaGeneral ObtenerPerfil(ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            PerfilDataAccess objPerfil = new PerfilDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = objPerfil.ObtenerMiPerfil(Convert.ToString(token).Trim(), ref Codigo_Respuesta);

                if (objPerfil.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objPerfil.textoRespuesta;
                    codigoRespuesta = objPerfil.codigoRespuesta;
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
