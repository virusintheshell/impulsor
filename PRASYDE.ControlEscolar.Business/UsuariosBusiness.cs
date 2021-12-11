
//AUTOR: DIEGO OLVERA
//FECHA: 06-04-2019
//DESCRIPCIÓN: CLASE QUE CONTIENE LA LOGICA PARA EL MODULO DE USUARIOS

namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using Framework;
    using DataAcess;
    using System.Net;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;

    public class UsuariosBusiness
    {
        public static RespuestaGeneral ObtenerDatosFormulario(ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            CatalogosDataAccess objRoles = new CatalogosDataAccess();
            CatalogosDataAccess objOcupacion = new CatalogosDataAccess();

            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            FormularioUsuarios objListaRoles = new FormularioUsuarios();

            PropiedadesCatalogos filtroGeneral;

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                filtroGeneral = new PropiedadesCatalogos() { id = 0, cadenaIds = "", nombrePagina = "Roles" };
                ObjetoRespuesta = objRoles.ObtenerCatalogo(token, filtroGeneral, ref Codigo_Respuesta);

                objListaRoles.roles = (List<ListaGenericaCatalogosTresValores>)ObjetoRespuesta.result;
                if (objRoles.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    filtroGeneral = new PropiedadesCatalogos() { id = 0, cadenaIds = "", nombrePagina = "Ocupacion" };
                    ObjetoRespuesta = objOcupacion.ObtenerCatalogo(token, filtroGeneral, ref Codigo_Respuesta);

                    objListaRoles.ocupacion = (List<ListaGenericaCatalogos>)ObjetoRespuesta.result;
                    if (objOcupacion.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                        codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                        Resultado = objListaRoles;
                    }
                    else {
                        mensaje = objOcupacion.textoRespuesta;
                        codigoRespuesta = objOcupacion.codigoRespuesta;
                        Resultado = new object { };
                    }
                }
                else {
                    mensaje = objRoles.textoRespuesta;
                    codigoRespuesta = objRoles.codigoRespuesta;
                    Resultado = new object { };
                }
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Guardar(Usuarios propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            UsuariosDataAccess objUsuarios = new UsuariosDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objUsuarios.Guardar(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objUsuarios.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objUsuarios.textoRespuesta;
                    codigoRespuesta = objUsuarios.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }

            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Listado(ref HttpStatusCode Codigo_Respuesta, int estatus)
        {
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            string pagina = string.Empty;
            string paginaOrigen = string.Empty;

            UsuariosDataAccess objUsuarios = new UsuariosDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            GeneralDataAccess objetoGeneral = new GeneralDataAccess();

            ListadoPermisosUsuarios objListaUsuarios = new ListadoPermisosUsuarios();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina, ref paginaOrigen))
            {
                objListaUsuarios.permisos = (List<ListaGenericaCatalogos>)objetoGeneral.Permisos(token, pagina, paginaOrigen, ref Codigo_Respuesta);
                if (objetoGeneral.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    objListaUsuarios.usuarios = (List<ListadoUsarios>)objUsuarios.Listado(token, estatus, ref Codigo_Respuesta);
                    if (objUsuarios.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                        codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                        Resultado = objListaUsuarios;
                    }
                    else {
                        mensaje = objUsuarios.textoRespuesta;
                        codigoRespuesta = objUsuarios.codigoRespuesta;
                        Resultado = new object { };
                    }
                }
                else {
                    mensaje = objetoGeneral.textoRespuesta;
                    codigoRespuesta = objetoGeneral.codigoRespuesta;
                    Resultado = new object { };
                }
                // ObjetoRespuesta.result = objUsuarios.Listado(token, estatus, ref Codigo_Respuesta);
                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Obtener(ref HttpStatusCode Codigo_Respuesta, string idUsuario)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            UsuariosDataAccess objUsuario = new UsuariosDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objUsuario.Obtener(token, pagina, idUsuario, ref Codigo_Respuesta);

                if (objUsuario.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objUsuario.textoRespuesta;
                    codigoRespuesta = objUsuario.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral Editar(Usuarios propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            UsuariosDataAccess objUsuario = new UsuariosDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token, ref pagina))
            {
                ObjetoRespuesta.result = objUsuario.Editar(propiedades, token, pagina, ref Codigo_Respuesta);

                if (objUsuario.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objUsuario.textoRespuesta;
                    codigoRespuesta = objUsuario.codigoRespuesta;
                    Resultado = new object { };
                }

                ObjetoRespuesta.status = codigoRespuesta;
                ObjetoRespuesta.message = mensaje;
                ObjetoRespuesta.result = Resultado;
            }
            return ObjetoRespuesta;
        }

        public static RespuestaGeneral GuardarPreInscripcion(UsuariosPreInscripcion propiedades, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            UsuariosDataAccess objUsuarios = new UsuariosDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            ObjetoRespuesta.result = objUsuarios.GuardarPreInscripcion(propiedades, ref Codigo_Respuesta);

            if (objUsuarios.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
            {
                Codigo_Respuesta = HttpStatusCode.OK;
                mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                Resultado = ObjetoRespuesta.result;
            }
            else {
                mensaje = objUsuarios.textoRespuesta;
                codigoRespuesta = objUsuarios.codigoRespuesta;
                Resultado = new object { };
            }
            ObjetoRespuesta.status = codigoRespuesta;
            ObjetoRespuesta.message = mensaje;
            ObjetoRespuesta.result = Resultado;

            return ObjetoRespuesta;
        }

        public static RespuestaGeneral CambiarContrasena(propiedadContasena propiedad, ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string pagina = string.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            UsuariosDataAccess objUsuario = new UsuariosDataAccess();
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                ObjetoRespuesta.result = objUsuario.CambiarContrasena(token, propiedad, ref Codigo_Respuesta);

                if (objUsuario.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                {
                    Codigo_Respuesta = HttpStatusCode.OK;
                    mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                    codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                    Resultado = ObjetoRespuesta.result;
                }
                else {
                    mensaje = objUsuario.textoRespuesta;
                    codigoRespuesta = objUsuario.codigoRespuesta;
                    Resultado = new object { };
                }
            }
            ObjetoRespuesta.status = codigoRespuesta;
            ObjetoRespuesta.message = mensaje;
            ObjetoRespuesta.result = Resultado;

            return ObjetoRespuesta;
        }

        public static bool ValidarDatos(Usuarios propiedades, int tipo)
        {
            try
            {
                bool respuesta = true;
                if (propiedades == null) { respuesta = false; return respuesta; }
                if (tipo == 2)
                {
                    if (propiedades.idUsuario == "") { respuesta = false; return respuesta; }
                }
            
                if (propiedades.idRol == 0) { respuesta = false; }
                if (propiedades.matriculaOficial != "") {
                    if (!Seguridad.NumerosObligatorios(propiedades.matriculaOficial, 9, 9)) { respuesta = false; return respuesta; }
                }
                if (!Seguridad.letrasObligatorias(propiedades.nombre.Trim(), 1, 100)) { respuesta = false; return respuesta; }
                if (!Seguridad.letrasObligatorias(propiedades.primerApellido.Trim(), 1, 100)) { respuesta = false; return respuesta; }
                if (!Seguridad.letrasNoObligatorias(propiedades.segundoApellido.Trim(), 1, 100)) { respuesta = false; return respuesta; }
                if (!Seguridad.NumeroTelefonicoNoObligatorio(propiedades.telefono)) { respuesta = false; return respuesta; }
                if (!Seguridad.NumeroTelefonicoObligatorio(propiedades.celular)) { respuesta = false; return respuesta; }
                if (!Seguridad.TextoSimpleNoObligatorio(propiedades.direccion.Trim(), 1, 250)) { respuesta = false; return respuesta; }
                if (!Seguridad.NumerosNoObligatorios(propiedades.idEstadoCivil.ToString(), 1, 1)) { respuesta = false; return respuesta; }
                if (!Seguridad.ValidaCorreo(propiedades.correoElectronico.Trim())) { respuesta = false; return respuesta; }
                if (!Seguridad.NumerosNoObligatorios(propiedades.ocupacion.ToString(), 1, 1)) { respuesta = false; return respuesta; }
                
                return respuesta;
            }
            catch (Exception exception) { throw exception; }
        }

        public static bool ValidarDatosPreInscripcion(UsuariosPreInscripcion propiedades)
        {
            try
            {
                bool respuesta = true;
               
                if (!Seguridad.letrasObligatorias(propiedades.nombre, 1, 100)) { respuesta = false; return respuesta; }
                if (!Seguridad.letrasNoObligatorias(propiedades.primerApellido, 1, 100)) { respuesta = false; return respuesta; }
                if (!Seguridad.letrasNoObligatorias(propiedades.segundoApellido, 1, 100)) { respuesta = false; return respuesta; }
                if (!Seguridad.NumeroTelefonicoObligatorio(propiedades.celular)) { respuesta = false; return respuesta; }
                if (!Seguridad.ValidaCorreo(propiedades.correoElectronico)) { respuesta = false; return respuesta; }

                return respuesta;
            }
            catch (Exception exception) { throw exception; }
        }

        public static string Prueba(string cadena)
        {
            UsuariosDataAccess objUsuarios = new UsuariosDataAccess();
            string respusta = string.Empty;
            respusta = objUsuarios.xxx(cadena);
            return respusta;
        }
    }
}



