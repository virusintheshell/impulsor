
//AUTOR: DIEGO OLVERA
//FECHA: 08-04-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA EL MODULO DETALLE DE MODALIDADES

namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using Framework;
    using DataAcess;
    using System.Net;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;
    
   public class DetalleModalidadesBusiness
    {

        public static RespuestaGeneral ObtenerDatosFormulario(ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            CatalogosDataAccess objDetalleModalidad = new CatalogosDataAccess();

            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            FormularioDetalleModalidad objListaModalidad = new FormularioDetalleModalidad();

            PropiedadesCatalogos filtroGeneral;

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                filtroGeneral = new PropiedadesCatalogos() { id = 0, cadenaIds = "", nombrePagina = "Modalidad" };
                ObjetoRespuesta = objDetalleModalidad.ObtenerCatalogo(token, filtroGeneral, ref Codigo_Respuesta);

                objListaModalidad.modalidades = (List<ListaGenericaCatalogos>)ObjetoRespuesta.result;
                if (objDetalleModalidad.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = objListaModalidad;
                }
                else {
                    mensaje = objDetalleModalidad.textoRespuesta;
                    codigoRespuesta = objDetalleModalidad.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Listado(ref HttpStatusCode Codigo_Respuesta, int estatus, int idModalidad)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            DetalleModalidadDataAccess objModalidad = new DetalleModalidadDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = objModalidad.Listado(token, estatus, idModalidad, ref Codigo_Respuesta);

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

        public static RespuestaGeneral ListadoPermisos(ref HttpStatusCode Codigo_Respuesta, int estatus)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();


            string pagina = string.Empty;
            string paginaOrigen = string.Empty;
         
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            GeneralDataAccess objetoGeneral = new GeneralDataAccess();


            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina, ref paginaOrigen))
            {
                ObjetoRespuesta.result = (List<ListaGenericaCatalogos>)objetoGeneral.Permisos(token, pagina, paginaOrigen, ref Codigo_Respuesta);

                if (objetoGeneral.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
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

        public static RespuestaGeneral Guardar(DetalleModalidad propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            DetalleModalidadDataAccess objModaliddes = new DetalleModalidadDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

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

            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Obtener(ref HttpStatusCode Codigo_Respuesta, int idDetalleModalidad)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            DetalleModalidadDataAccess objModalidad = new DetalleModalidadDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objModalidad.Obtener(token, pagina, idDetalleModalidad, ref Codigo_Respuesta);

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

        public static RespuestaGeneral Editar(DetalleModalidad propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            DetalleModalidadDataAccess objModalidad = new DetalleModalidadDataAccess();
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

        public static bool ValidarDatos(DetalleModalidad propiedades, int tipo)
        {
            try
            {
                bool respusta = true;
                if (tipo == 2)
                {
                    if (propiedades.idDetalleModalidad == 0) { respusta = false; return respusta; }
                }
                if (propiedades.idModalidad == 0) { respusta = false; return respusta; }
                if (Seguridad.TextoSimpleObligatorio(propiedades.nombre, 1, 50) == false) { respusta = false; return respusta; }
                if (Seguridad.TextoSimpleNoObligatorio(propiedades.descripcion, 1, 250) == false) { respusta = false; return respusta; }

                return respusta;
            }
            catch (Exception exception) { throw exception; }
        }
    }
}
