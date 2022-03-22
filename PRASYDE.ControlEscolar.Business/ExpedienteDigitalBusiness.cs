

//AUTOR: DIEGO OLVERA
//FECHA: 11-06-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA GUARDAR ARCHIVOS

namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using Framework;
    using DataAcess;
    using System.Net;
    using PRASYDE.ControlEscolar.Entities;
    using System.Collections.Generic;

    public class ExpedienteDigitalBusiness
    {
        public static RespuestaGeneral Guardar(propiedadesExpediente propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            ExpedienteDigitalDataAccess objExpediente = new ExpedienteDigitalDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = objExpediente.Guardar(propiedades, token, ref Codigo_Respuesta);

                if (objExpediente.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else
                {
                    mensaje = objExpediente.textoRespuesta;
                    codigoRespuesta = objExpediente.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }

            return ObjetoRespuesta;
        }
              
        public static RespuestaGeneral ListadoArchivos(string idAlumno, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();
            string pagina = string.Empty;
        
            ExpedienteDigitalDataAccess objArchivos = new ExpedienteDigitalDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            GeneralDataAccess objetoGeneral = new GeneralDataAccess();
            ListadoPermisosArchivos objListaArchivos = new ListadoPermisosArchivos();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                objListaArchivos.archivos = (List<listadoArchivosAlumnos>)objArchivos.ListadoArchivos(token, idAlumno, ref Codigo_Respuesta);
                if (objArchivos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = objListaArchivos;
                }
                else
                {
                    mensaje = objArchivos.textoRespuesta;
                    codigoRespuesta = objArchivos.codigoRespuesta;
                    Resultado = new object { };
                }
            }
            else {
                codigoRespuesta = ObjetoRespuesta.status;
                mensaje = ObjetoRespuesta.message;
                ObjetoRespuesta.result = new object { };
            }

            Codigo_Respuesta = HttpStatusCode.OK;
            ObjetoRespuesta.status = codigoRespuesta;
            ObjetoRespuesta.message = mensaje;
            ObjetoRespuesta.result = Resultado;

            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Editar(propiedadesExpediente propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            ExpedienteDigitalDataAccess objExpediente = new ExpedienteDigitalDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = objExpediente.Editar(propiedades, token, ref Codigo_Respuesta);

                if (objExpediente.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else
                {
                    mensaje = objExpediente.textoRespuesta;
                    codigoRespuesta = objExpediente.codigoRespuesta;
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
