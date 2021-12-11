
//AUTOR: DIEGO OLVERA
//FECHA: 06-05-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA EL MODULO DE AVISOS

namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using Framework;
    using DataAcess;
    using System.Net;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Business.Notificaciones;

    public class AvisosBusiness
    {
        #region "METODOS PARA EL CRUD DE AVISOS"

        public static RespuestaGeneral Listado(ref HttpStatusCode Codigo_Respuesta, int estatus, int tipo)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            string pagina = string.Empty;
            string paginaOrigen = string.Empty;

            AvisosDataAccess objAvisos = new AvisosDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            GeneralDataAccess objetoGeneral = new GeneralDataAccess();
            ListadoPermisosAvisos objListaAvisos = new ListadoPermisosAvisos();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina, ref paginaOrigen))
            {
                objListaAvisos.permisos = (List<ListaGenericaCatalogos>)objetoGeneral.Permisos(token, pagina, paginaOrigen, ref Codigo_Respuesta);
                if (objetoGeneral.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    objListaAvisos.avisos = (List<ListadoAvisos>)objAvisos.Listado(token, estatus, tipo, ref Codigo_Respuesta);
                    if (objAvisos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                        codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                        Resultado = objListaAvisos;
                    }
                    else {
                        mensaje = objAvisos.textoRespuesta;
                        codigoRespuesta = objAvisos.codigoRespuesta;
                        Resultado = new object { };
                    }
                }
                else {
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

        public static RespuestaGeneral Guardar(Avisos propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            AvisosDataAccess objAvisos = new AvisosDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objAvisos.Guardar(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objAvisos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objAvisos.textoRespuesta;
                    codigoRespuesta = objAvisos.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Editar(Avisos propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            AvisosDataAccess objAvisos = new AvisosDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objAvisos.Editar(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objAvisos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objAvisos.textoRespuesta;
                    codigoRespuesta = objAvisos.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Obtener(ref HttpStatusCode Codigo_Respuesta, string idAviso)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            AvisosDataAccess objAvisos = new AvisosDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objAvisos.Obtener(token, pagina, idAviso, ref Codigo_Respuesta);

                if (objAvisos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objAvisos.textoRespuesta;
                    codigoRespuesta = objAvisos.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral ListadoElementosEnvio(ref HttpStatusCode Codigo_Respuesta, int tipoEnvio,string idAviso)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            AvisosDataAccess objAvisos = new AvisosDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            object miObjetoRespuesta = new object();

            GeneralDataAccess objetoGeneral = new GeneralDataAccess();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                miObjetoRespuesta = (List<ElementosEnvio>)objAvisos.ListadoElementosEnvio(token, tipoEnvio, idAviso, ref Codigo_Respuesta);
                if (objAvisos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = miObjetoRespuesta;
                }
                else {
                    mensaje = objAvisos.textoRespuesta;
                    codigoRespuesta = objAvisos.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static bool ValidarDatos(Avisos propiedades, int tipo)
        {
            try
            {
                bool respusta = true;
                if (tipo == 2)
                {
                    if (propiedades.idAviso == "") { respusta = false; return respusta; }
                }

                if (Seguridad.TextoSimpleObligatorio(propiedades.nombre, 1, 50) == false) { respusta = false; return respusta; }
                if (Seguridad.TextoSimpleObligatorio(propiedades.autor, 1, 50) == false) { respusta = false; return respusta; }
                if (Seguridad.TextoSimpleObligatorio(propiedades.descripcion, 1, 800) == false) { respusta = false; return respusta; }

                return respusta;
            }
            catch (Exception exception) { throw exception; }
        }

        #endregion

        #region "METODOS PARA EL ENVIO DE AVISOS"

        public static RespuestaGeneral GuardarEnvio(PropiedadesEnvio propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            EnvioNotificacion objNotificacion = new EnvioNotificacion();
            AvisosDataAccess objEnvios = new AvisosDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objEnvios.GuardarEnvio(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objEnvios.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;

                    //ENVIO DE NOTIFICACION 
                    objNotificacion.EnviarNotificacion(Convert.ToInt32(Resultado), Convert.ToInt16(Enumerados.TipoNotificacion.Avisos), ref Codigo_Respuesta);
                }
                else {
                    mensaje = objEnvios.textoRespuesta;
                    codigoRespuesta = objEnvios.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static bool ValidarDatosEnvio(PropiedadesEnvio propiedades)
        {
            try
            {
                bool respusta = true;

                if (propiedades.idAviso == "") { respusta = false; return respusta; }
                if (propiedades.tipoAviso == 0) { respusta = false; return respusta; }
                if (propiedades.enviarAtodos > 1) { respusta = false; return respusta; }
                
                return respusta;
            }
            catch (Exception exception) { throw exception; }
        }
        #endregion
    }
}
