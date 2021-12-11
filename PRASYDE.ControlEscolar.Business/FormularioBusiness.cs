
//AUTOR: DIEGO OLVERA
//FECHA: 18-04-2020
//DESCRIPCIÓN: CLASE QUE OBTIENE LOS DATOS PARA ARMAR UN FORMULARIO

namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using Framework;
    using DataAcess;
    using System.Net;
    using PRASYDE.ControlEscolar.Entities;

    public class FormularioBusiness
    {
        public static RespuestaGeneral Obtener(ref HttpStatusCode Codigo_Respuesta, string nombreFomulario)
        {
            Guid token = Guid.Empty;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            FormularioDataAccess objFormulario = new FormularioDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            FormularioListado formularioLista = new FormularioListado(); 

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                formularioLista.formulario = (Formulario)objFormulario.Obtener(token, nombreFomulario);

                if (objFormulario.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = formularioLista.formulario.idFormularioUnique != null ? formularioLista: new object { };
                }
                else
                {
                    mensaje = objFormulario.textoRespuesta;
                    codigoRespuesta = objFormulario.codigoRespuesta;
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
