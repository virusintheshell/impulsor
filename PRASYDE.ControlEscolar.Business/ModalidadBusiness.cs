
//AUTOR: DIEGO OLVERA
//FECHA: 06-04-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA EL MODULO DE MODALIDADES


namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using Framework;
    using DataAcess;
    using System.Net;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;
    
    public class ModalidadBusiness
    {
        public static RespuestaGeneral Listado(ref HttpStatusCode Codigo_Respuesta, int estatus)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            string pagina = string.Empty;
            string paginaOrigen = string.Empty;

            ModalidadDataAccess objModalidad = new ModalidadDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            GeneralDataAccess objetoGeneral = new GeneralDataAccess();
            ListadoPermisosModalidades objListaModalidades = new ListadoPermisosModalidades();
            
            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina, ref paginaOrigen))
            {
                objListaModalidades.permisos = (List<ListaGenericaCatalogos>)objetoGeneral.Permisos(token, pagina, paginaOrigen, ref Codigo_Respuesta);
                if (objetoGeneral.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    objListaModalidades.modalidades = (List<ListadoModalidades>)objModalidad.Listado(token, estatus, ref Codigo_Respuesta);
                    if (objModalidad.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                        codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                        Resultado = objListaModalidades;
                    }
                    else {
                        mensaje = objModalidad.textoRespuesta;
                        codigoRespuesta = objModalidad.codigoRespuesta;
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

        public static RespuestaGeneral Guardar(Modalidades propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            ModalidadDataAccess objModaliddes = new ModalidadDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (propiedades != null)
            {

                if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
                {
                    ObjetoRespuesta.result = objModaliddes.Guardar(propiedades, token, pagina, ref Codigo_Respuesta);

                    if (objModaliddes.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                        codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                        Resultado = ObjetoRespuesta.result;
                    }
                    else {
                        mensaje = objModaliddes.textoRespuesta;
                        codigoRespuesta = objModaliddes.codigoRespuesta;
                        Resultado = new object { };
                    }

                    ObjetoRespuesta.status = codigoRespuesta;
                    ObjetoRespuesta.message = mensaje;
                    ObjetoRespuesta.result = Resultado;
                }
            }
            else {

            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Obtener(ref HttpStatusCode Codigo_Respuesta, int idPlantel)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            ModalidadDataAccess objModalidad = new ModalidadDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objModalidad.Obtener(token, pagina, idPlantel, ref Codigo_Respuesta);

                if (objModalidad.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objModalidad.textoRespuesta;
                    codigoRespuesta = objModalidad.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Editar(Modalidades propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            ModalidadDataAccess objModalidad = new ModalidadDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objModalidad.Editar(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objModalidad.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objModalidad.textoRespuesta;
                    codigoRespuesta = objModalidad.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static bool ValidarDatos(Modalidades propiedades, int tipo)
        {
            try
            {
                bool respusta = true;
                if (tipo == 2)
                {
                    if (propiedades.idModalidad == 0) { respusta = false; return respusta; }
                }

                if (Seguridad.TextoSimpleObligatorio(propiedades.nombre, 1, 50) == false) { respusta = false; return respusta; }
                if (Seguridad.TextoSimpleNoObligatorio(propiedades.descripcion, 1, 250) == false) { respusta = false; return respusta; }

                return respusta;
            }
            catch (Exception exception) { throw exception; }
        }

    }
}
