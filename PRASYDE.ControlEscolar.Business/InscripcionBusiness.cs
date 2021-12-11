
//AUTOR: DIEGO OLVERA
//FECHA: 05-06-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA LA INSCRIPCION

namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using DataAcess;
    using Framework;
    using System.Net;
    using System.Web;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;

    public class InscripcionBusiness
    {
        //REGRESAMOS EL LISTADO DE GRADOS DUCATIVOS Y VALIDAMOS QUE TENGA PERMISOS PARA GUARDAR
        public static RespuestaGeneral ObtenerDatosFormulario(ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();
            string pagina = string.Empty;

            PropiedadesCatalogos filtroGeneral;
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            CatalogosDataAccess objGradoEducativo = new CatalogosDataAccess();
            FormularioInscripcion objListaInscripcion = new FormularioInscripcion();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                filtroGeneral = new PropiedadesCatalogos() { id = 0, cadenaIds = "", nombrePagina = "Inscripcion" };
                ObjetoRespuesta = objGradoEducativo.ObtenerCatalogo(token, filtroGeneral, ref Codigo_Respuesta);
                objListaInscripcion.gradoEducativo = (List<ListaGenericaCatalogosTresValores>)ObjetoRespuesta.result;

                if (objGradoEducativo.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))

                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = objListaInscripcion;
                }
                else {
                    mensaje = objGradoEducativo.textoRespuesta;
                    codigoRespuesta = objGradoEducativo.codigoRespuesta;
                    Resultado = new object { };
                }
            }
            ObjetoRespuesta.status = codigoRespuesta;
            ObjetoRespuesta.message = mensaje;
            ObjetoRespuesta.result = Resultado;

            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Obtener(string idPrograma, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            InscripcionDataAccess objGrupos = new InscripcionDataAccess();
            DocumentosDataAccess objDocumentos = new DocumentosDataAccess();

            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            ListaGruposPlantel objListaPlantelesGrupos = new ListaGruposPlantel();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                objListaPlantelesGrupos.planteles = (List<GruposPlantel>)objGrupos.Obtener(token, idPrograma, ref Codigo_Respuesta);
                if (objGrupos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    objListaPlantelesGrupos.documentos = (List<ListaDocumentosPrograma>)objDocumentos.Obtener(token, idPrograma, ref Codigo_Respuesta);
                    if (objGrupos.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                        codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                        Resultado = objListaPlantelesGrupos;
                    }
                    else {
                        mensaje = objDocumentos.textoRespuesta;
                        codigoRespuesta = objDocumentos.codigoRespuesta;
                        Resultado = new object { };
                    }
                }
                else {
                    mensaje = objGrupos.textoRespuesta;
                    codigoRespuesta = objGrupos.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Guardar(Inscripcion propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            InscripcionDataAccess objInscripcion = new InscripcionDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objInscripcion.Guardar(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objInscripcion.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objInscripcion.textoRespuesta;
                    codigoRespuesta = objInscripcion.codigoRespuesta;
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
