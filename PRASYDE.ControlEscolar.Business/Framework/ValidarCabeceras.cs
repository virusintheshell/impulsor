

namespace PRASYDE.ControlEscolar.Business.Framework
{
    using System;
    using System.Web;
    using PRASYDE.ControlEscolar.Entities;

    class ValidarCabeceras
    {
        public static bool ContieneCabeceras(ref RespuestaGeneral RespuestaGral, ref Guid token, ref int Plataforma)

        {
            bool respuesta = false;
            string valor = string.Empty;

            if (HttpContext.Current.Request.Headers["Authorization"] != null)
            {
                if (Guid.TryParse(HttpContext.Current.Request.Headers["Authorization"], out token))
                {
                    if (HttpContext.Current.Request.Headers["Plataforma"] != null)
                    {
                        valor = Convert.ToString(HttpContext.Current.Request.Headers["Plataforma"]);

                        if (!Seguridad.IsNumeric(valor.ToString()))
                        { respuesta = false; }
                        else
                        {
                            Plataforma = Convert.ToUInt16(valor);
                            respuesta = true;
                        }
                    }
                    else { respuesta = false; }
                }
                else { respuesta = false; }
            }
            else { respuesta = false; }

            ///SI ALGUNA DE LAS CABECERAS ESTA VACIA O NO CONTIENE EL FORMATO ADECUADO REGRESA UNA RESPUESTA GENERAL DE ERROR
            if (respuesta == false)
            {
                RespuestaGral.status = Convert.ToInt16(Enumerados.Codigos_Respuesta.NoHeaders);
                RespuestaGral.message = "Revisar que las cabeceras contengan el formato adecuado o que no esten vacias";
                RespuestaGral.result = new object();
            }

            return respuesta;
        }

        public static bool ContieneCabeceras(ref RespuestaGeneral RespuestaGral, ref Guid token)

        {
            bool respuesta = false;

            if (HttpContext.Current.Request.Headers["Authorization"] != null)
            {
                if (Guid.TryParse(HttpContext.Current.Request.Headers["Authorization"], out token))
                {
                    respuesta = true;
                }
                else { respuesta = false; }
            }
            else { respuesta = false; }

            ///SI ALGUNA DE LAS CABECERAS ESTA VACIA O NO CONTIENE EL FORMATO ADECUADO REGRESA UNA RESPUESTA GENERAL DE ERROR
            if (respuesta == false)
            {
                RespuestaGral.status = Convert.ToInt16(Enumerados.Codigos_Respuesta.NoHeaders);
                RespuestaGral.message = "Revisar que las cabeceras contengan el formato adecuado o que no esten vacias";
                RespuestaGral.result = new object();
            }
            return respuesta;
        }

        public static bool ContieneCabeceras(ref RespuestaGeneral RespuestaGral, ref int Plataforma)

        {
            bool respuesta = false;
            string valor = string.Empty;

            if (HttpContext.Current.Request.Headers["Plataforma"] != null)
            {
                valor = Convert.ToString(HttpContext.Current.Request.Headers["Plataforma"]);

                if (!Seguridad.IsNumeric(valor.ToString()))
                { respuesta = false; }
                else
                {
                    Plataforma = Convert.ToUInt16(valor);
                    respuesta = true;
                }
            }
            else { respuesta = false; }



            ///SI ALGUNA DE LAS CABECERAS ESTA VACIA O NO CONTIENE EL FORMATO ADECUADO REGRESA UNA RESPUESTA GENERAL DE ERROR
            if (respuesta == false)
            {
                RespuestaGral.status = Convert.ToInt16(Enumerados.Codigos_Respuesta.NoHeaders);
                RespuestaGral.message = "Revisar que las cabeceras contengan el formato adecuado o que no esten vacias";
                RespuestaGral.result = new object();
            }
            return respuesta;
        }

        public static bool ContieneCabeceras(ref Guid token)

        {
            bool respuesta = false;
            string valor = string.Empty;

            if (HttpContext.Current.Request.Headers["Authorization"] != null)
            {
                if (Guid.TryParse(HttpContext.Current.Request.Headers["Authorization"], out token))
                {
                    respuesta = true;
                }
                else { respuesta = false; }
            }
            else { respuesta = false; }

            return respuesta;
        }

        public static bool ContieneCabeceras(ref RespuestaGeneral RespuestaGral, ref Guid token, ref string pagina)

        {
            bool respuesta = false;
            string valor = string.Empty;

            if (HttpContext.Current.Request.Headers["Authorization"] != null)
            {
                if (Guid.TryParse(HttpContext.Current.Request.Headers["Authorization"], out token))
                {
                    if (HttpContext.Current.Request.Headers["Pagina"] != null)
                    {
                        pagina = Convert.ToString(HttpContext.Current.Request.Headers["Pagina"]);

                        if (pagina != "")
                        {
                            respuesta = true;
                        }
                        else { respuesta = false; }
                    }
                    else { respuesta = false; }
                }
                else { respuesta = false; }
            }
            else { respuesta = false; }

            ///SI ALGUNA DE LAS CABECERAS ESTA VACIA O NO CONTIENE EL FORMATO ADECUADO REGRESA UNA RESPUESTA GENERAL DE ERROR
            if (respuesta == false)
            {
                RespuestaGral.status = Convert.ToInt16(Enumerados.Codigos_Respuesta.NoHeaders);
                RespuestaGral.message = "Revisar que las cabeceras contengan el formato adecuado o que no esten vacias";
                RespuestaGral.result = new object();
            }
            return respuesta;
        }

        public static bool ContieneCabeceras(ref RespuestaGeneral RespuestaGral, ref Guid token, ref string pagina, ref string paginaOrigen)

        {
            bool respuesta = false;
            string valor = string.Empty;

            if (HttpContext.Current.Request.Headers["Authorization"] != null)
            {
                if (Guid.TryParse(HttpContext.Current.Request.Headers["Authorization"], out token))
                {
                    if (HttpContext.Current.Request.Headers["Pagina"] != null)
                    {
                        pagina = Convert.ToString(HttpContext.Current.Request.Headers["Pagina"]);

                        if (pagina != "")
                        {
                            if (HttpContext.Current.Request.Headers["paginaOrigen"] != null)
                            {
                                paginaOrigen = Convert.ToString(HttpContext.Current.Request.Headers["paginaOrigen"]);

                                if (paginaOrigen != "")
                                {
                                    respuesta = true;
                                }
                                else { respuesta = false; }
                            }
                            else { respuesta = false; }
                        }
                        else { respuesta = false; }
                    }
                    else { respuesta = false; }
                }
                else { respuesta = false; }
            }
            else { respuesta = false; }

            ///SI ALGUNA DE LAS CABECERAS ESTA VACIA O NO CONTIENE EL FORMATO ADECUADO REGRESA UNA RESPUESTA GENERAL DE ERROR
            if (respuesta == false)
            {
                RespuestaGral.status = Convert.ToInt16(Enumerados.Codigos_Respuesta.NoHeaders);
                RespuestaGral.message = "Revisar que las cabeceras contengan el formato adecuado o que no esten vacias";
                RespuestaGral.result = new object();
            }
            return respuesta;
        }

        public static bool ContieneCabeceras(ref RespuestaGeneral RespuestaGral, ref Guid token, ref string pagina, ref string paginaOrigen, ref int tipoListado)

        {
            bool respuesta = false;
            string valor = string.Empty;

            if (HttpContext.Current.Request.Headers["Authorization"] != null)
            {
                if (Guid.TryParse(HttpContext.Current.Request.Headers["Authorization"], out token))
                {
                    if (HttpContext.Current.Request.Headers["Pagina"] != null)
                    {
                        pagina = Convert.ToString(HttpContext.Current.Request.Headers["Pagina"]);

                        if (pagina != "")
                        {
                            if (HttpContext.Current.Request.Headers["paginaOrigen"] != null)
                            {
                                paginaOrigen = Convert.ToString(HttpContext.Current.Request.Headers["paginaOrigen"]);

                                if (paginaOrigen != "")
                                {
                                    if (HttpContext.Current.Request.Headers["tipoListado"] != null)
                                    {
                                        tipoListado = Convert.ToInt16(HttpContext.Current.Request.Headers["tipoListado"]);

                                        if (tipoListado != 0)
                                        {
                                            respuesta = true;
                                        }
                                        else { respuesta = false; }
                                    }
                                    else { respuesta = false; }
                                }
                                else { respuesta = false; }
                            }
                            else { respuesta = false; }
                        }
                        else { respuesta = false; }
                    }
                    else { respuesta = false; }
                }
                else { respuesta = false; }
            }
            else { respuesta = false; }

            ///SI ALGUNA DE LAS CABECERAS ESTA VACIA O NO CONTIENE EL FORMATO ADECUADO REGRESA UNA RESPUESTA GENERAL DE ERROR
            if (respuesta == false)
            {
                RespuestaGral.status = Convert.ToInt16(Enumerados.Codigos_Respuesta.NoHeaders);
                RespuestaGral.message = "Revisar que las cabeceras contengan el formato adecuado o que no esten vacias";
                RespuestaGral.result = new object();
            }
            return respuesta;
        }
    }
}
