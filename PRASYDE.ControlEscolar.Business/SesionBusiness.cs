
namespace PRASYDE.ControlEscolar.Business
{
    using System;
    using DataAcess;
    using System.Net;
    using System.Web;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.Business.Framework;

    public class SesionBusiness
    {
        #region "METODOS PARA INICIO DE SESION"

        public static RespuestaGeneral Iniciar(CredencialesAcceso credenciales, ref HttpStatusCode Codigo_Respuesta)
        {
            int Plataforma = 0;
            Guid token = Guid.Empty;

            int codigoRespuesta = 0;
            string mensaje = string.Empty;
            object Resultado = new object();

            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            SesionDataAccess objSesion = new SesionDataAccess();

            if (credenciales != null)
            {
                if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref Plataforma))
                {
                    //VALIDAMOS EL MODO DE ACCESO AL SISTEMA 
                    if (Plataforma == Convert.ToInt16(Enumerados.Plataforma.Movil))
                    {
                        if (string.IsNullOrEmpty(credenciales.Usuario) || Seguridad.ValidaCorreo(credenciales.Usuario) == false) { return General.DatosInvalidos(ref Codigo_Respuesta); }
                        if (string.IsNullOrEmpty(credenciales.Contrasena)) { return General.DatosInvalidos(ref Codigo_Respuesta); }

                        //SE VALIDA QUE EL IMEI TENGA EL FORMATO ADECUADO
                        if (Seguridad.NumerosObligatorios(credenciales.IMEI, 15, 15) == false) { return General.DatosInvalidos(ref Codigo_Respuesta); }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(credenciales.Usuario) || Seguridad.ValidaCorreo(credenciales.Usuario) == false) { return General.DatosInvalidos(ref Codigo_Respuesta); }
                        if (string.IsNullOrEmpty(credenciales.Contrasena)) { return General.DatosInvalidos(ref Codigo_Respuesta); }
                    }

                    if (Plataforma == Convert.ToInt16(Enumerados.Plataforma.Web) || Plataforma == Convert.ToInt16(Enumerados.Plataforma.Movil))
                    {
                        if (Credenciales_Correctas(ref ObjetoRespuesta, credenciales.Usuario, credenciales.Contrasena, ref Codigo_Respuesta) == false)
                        {
                            return ObjetoRespuesta;
                        }
                    }

                    ObjetoRespuesta.result = objSesion.obtenerToken(credenciales, Plataforma, ref Codigo_Respuesta);

                    if (objSesion.codigoRespuesta == Convert.ToInt16(Enumerados.Codigos_Respuesta.OK))
                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        mensaje = Enumerados.Textos_Respuesta.OK.ToString();
                        codigoRespuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
                        Resultado = ObjetoRespuesta.result;
                    }
                    else
                    {
                        Codigo_Respuesta = HttpStatusCode.OK;
                        mensaje = objSesion.textoRespuesta;
                        codigoRespuesta = objSesion.codigoRespuesta;
                        Resultado = new object { };
                    }

                    ObjetoRespuesta.status = codigoRespuesta;
                    ObjetoRespuesta.message = mensaje;
                    ObjetoRespuesta.result = Resultado;
                }
                else { Codigo_Respuesta = HttpStatusCode.Unauthorized; }
            }
            else
            {
                return General.DatosInvalidos(ref Codigo_Respuesta);
            }
            return ObjetoRespuesta;
        }

        /// SE VALIDA QUE EL USUARIO Y CONTRASEÑA NO ESTEN VACIO Y QUE EL USUARIO TENGA EL FORMATO CORRECTO DE E-MAIL
        private static bool Credenciales_Correctas(ref RespuestaGeneral ObjetoRespuesta, string Usuario, string Contrasena, ref HttpStatusCode Codigo_Respuesta)
        {
            bool Respuesta = true;

            if (Usuario != "" && Contrasena != "")
            {
                if (Seguridad.ValidaCorreo(Usuario) == false)
                {
                    Respuesta = false;
                    ObjetoRespuesta.status = Convert.ToInt16(Enumerados.Codigos_Respuesta.NoContent);
                    ObjetoRespuesta.message = "Usuario con formato no valido";
                    ObjetoRespuesta.result = new object { };
                }
            }

            else
            {
                Respuesta = false;
                ObjetoRespuesta.status = Convert.ToInt16(Enumerados.Codigos_Respuesta.NoContent);
                ObjetoRespuesta.message = "Usuario o contraseña no valido";
                ObjetoRespuesta.result = new object { };
            }
            Codigo_Respuesta = HttpStatusCode.OK;
            return Respuesta;
        }

        #endregion

        #region "METODO PARA GUARDAR EL DETALLE DE LA SESION (FIREBASE)"

        public static RespuestaGeneral GuardarDetalleSesion(ref HttpStatusCode Codigo_Respuesta)
        {
            Guid token = Guid.Empty;
            string tokeFB = string.Empty;
         
            RespuestaGeneral ObjetoRespuesta = new RespuestaGeneral();
            SesionDataAccess objSesion = new SesionDataAccess();

            if (ValidarCabeceras.ContieneCabeceras(ref ObjetoRespuesta, ref token))
            {
                if (HttpContext.Current.Request.Headers["tokenFireBase"] != null)
                {
                    tokeFB = HttpContext.Current.Request.Headers["tokenFireBase"];
                    objSesion.guardarTokenFireBase(token, tokeFB.ToString(), ref Codigo_Respuesta);

                    Codigo_Respuesta = HttpStatusCode.OK;
                    ObjetoRespuesta.status = objSesion.codigoRespuesta;
                    ObjetoRespuesta.message = objSesion.textoRespuesta;
                    ObjetoRespuesta.result = new object { };
                }
            }
            else { Codigo_Respuesta = HttpStatusCode.Unauthorized; }
            return ObjetoRespuesta;
        }

        #endregion
    }
}

