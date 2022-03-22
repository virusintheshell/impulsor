
//AUTOR: DIEGO OLVERA
//FECHA DE CREACION: 21-07-2017
//DESCRIPCION: CLASE QUE CONTIENE LA LOGICA PARA EL ENVIO DE CORREO ELECTRONICO

namespace PRASYDE.ControlEscolar.DataAcess.Framework
{
    using System;
    using System.Text;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Configuration;
    using PRASYDE.ControlEscolar.Entities;

    public class EnvioCorreoElectronico
    {
        public static int Enviar(ObjetoCorreoElectronico propiedades)
        {
            string PathFile = string.Empty;
            string cuentaCorreo = string.Empty;
            string contrasena = string.Empty;
            
            if (propiedades.tipoCorreo == Convert.ToInt16(Enumerados.TipoEnvioCorreo.CambioContrasena))
            {
                cuentaCorreo = ConfigurationManager.AppSettings["cuentaCorreoNuevoUsuario"].ToString();
                contrasena = ConfigurationManager.AppSettings["contrasenaNuevoUsuario"].ToString();
            }
            else {
                cuentaCorreo = ConfigurationManager.AppSettings["cuentaCorreoBienvenida"].ToString();
                contrasena = ConfigurationManager.AppSettings["contrasenaBienvenida"].ToString();
            }
            
            RespuestaGeneral objRespuestaGeneral = new RespuestaGeneral();
            int respuestaGeneral = Convert.ToInt16(200);

            string destino = propiedades.correo;
            string[] destinatario = destino.Split(';');

            foreach (string destinos in destinatario)
            {
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

                msg.To.Add(destinos);
                msg.From = new MailAddress(cuentaCorreo, cuentaCorreo, System.Text.Encoding.UTF8);
                msg.Subject = propiedades.asunto;

                //TIPO DE CODIFICACIÓN DEL ASUNTO
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                string nombreEmpresa = ConfigurationManager.AppSettings["nombreEmpresa"].ToString();
                string html = string.Empty;

                if (propiedades.tipoCorreo == Convert.ToInt16(Enumerados.TipoEnvioCorreo.CambioContrasena))
                {
                    html = MensajeCorreo(propiedades, nombreEmpresa);
                }
                else { html = MensajeBienvenida(propiedades, nombreEmpresa); }
                
                //MensajeBienvenida(url, nombre, token, nombreEmpresa); 
                AlternateView htmlView =
                    AlternateView.CreateAlternateViewFromString(html,
                                            Encoding.UTF8,
                                            MediaTypeNames.Text.Html);
                msg.IsBodyHtml = false;
                msg.AlternateViews.Add(htmlView);

                if (!string.IsNullOrEmpty(PathFile))
                {
                    //SE AGREGA EL ARCHIVO Y SU TIPO MediaTypeNames.Application.Zip
                    Attachment Data = new Attachment(PathFile);

                    //SE OBTIENEN LAS PROPIEDADES DEL ARCIVO
                    ContentDisposition disposition = Data.ContentDisposition;
                    disposition.CreationDate = System.IO.File.GetCreationTime(PathFile);
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime(PathFile);
                    disposition.ReadDate = System.IO.File.GetLastAccessTime(PathFile);
                    msg.Attachments.Add(Data);
                }

                //SE CREA UN TIPO DE CLIENTE DE CORREO (POR DONDE SE ENVIARA EL CORREO)
                SmtpClient client = new SmtpClient();

                //CREDENCIALES PARA EVITAR EL SPAM

                // OUTLOOK = 25 GMAIL = 587
                // IMPULSOR = CON SSL 465 SIN SSL 587 
                
                client.Port = Convert.ToInt16(ConfigurationManager.AppSettings["puertoEnvioCorreo"].ToString()); 
                client.Credentials = new System.Net.NetworkCredential(cuentaCorreo, contrasena);

                //SE IDENTIFICA EL CLIENTE QUE SE VA A UTILIZAR 
                //"smtpout.secureserver.net";  outlook
                //"smtp.gmail.com";  gmail
                //"mail.impulsorintelectual.com.mx";  

                //EnableSsl
                // gmail = true

                client.Host = ConfigurationManager.AppSettings["clienteHost"].ToString();
                client.EnableSsl =Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                
                try
                {
                    client.Send(msg);
                }
                catch (System.Net.Mail.SmtpException ex)
                {
                    string mensajeError = ex.Message;
                    respuestaGeneral = Convert.ToInt16(500);
                    EscrituraLog.guardar("EnvioCorreoElectronicoContrasena-Enviar. ", ex.ToString());
                }
            }
            return respuestaGeneral;
        }

        private static string MensajeCorreo(ObjetoCorreoElectronico propiedades, string nombreCompania)
        {
            string mensaje = string.Empty;
            string imagenBienvenida = ConfigurationManager.AppSettings["ImagenCorreoBienvenida"].ToString();
            string urlPlataforma = ConfigurationManager.AppSettings["urlPlataforma"].ToString();

            mensaje = "<div style='text-align: center; padding: 25px; border: 1px solid #e2e2e2; text-shadow: 2px 2px #e2e2e2'>" +
                      " <img src = '" + imagenBienvenida + "' style='width:30px; height:30px'><br/>" +
                      "  <header><br/>" +
                      "     <strong></strong><i>" + propiedades.nombre + "</i>" +
                      "  </header><br/><br/>" +
                      "  <section>" +
                      "     <p>Se ha generado una contraseña temporal, favor de cambiarla lo antes posible. " +
                      "     <br/><p>Contraseña temporal<p> " +
                      " <strong>" + propiedades.contrasena + "</strong>" +
                      "  </section><br/><br/>" +
                      "  <footer>" +
                      "     <strong>Atentamente</strong><br/>" +
                      "     " + nombreCompania + "" +
                      " <br/><a href="+ urlPlataforma  + ">" + urlPlataforma  + " </a>   " + 
                      "  </footer>" +
                      "</div>";

            return mensaje;
        }

        private static string MensajeBienvenida(ObjetoCorreoElectronico propiedades, string nombreCompania)
        {
            string mensaje = string.Empty;
            string imagen = ConfigurationManager.AppSettings["ImagenCorreoBienvenida"].ToString();
            string urlPlataforma = ConfigurationManager.AppSettings["urlPlataforma"].ToString();

            mensaje = "<!DOCTYPE html>" +
                      "<div style='text-align:center; padding:25px; border:1px solid #e2e2e2; text-shadow:2px 2px #e2e2e2'>" +
                      " <img src = '" + imagen + "' style='width:30px; height:30px'>" +

                      "   <section>" +
                      "     <p><b>CONFIRMACIÓN DE INSCRIPCIÓN</b></p>" +
                      "   </section><br/>" +
                      "   <div style ='text-align:left; margin-left:70px'><section>" +
                      "      <p><b> Estimado(a) alumno: </b><span>" + propiedades.nombre.ToUpper() + "</span></p>" +
                      "      <p> Reciba un cordial saludo. A través de la presente confirmamos su inscripción a la actividad academica.</p>" +
                      "   </section></div><br>" +
                      "   <section style=text-align:center; font-style:italic>" +
                      "     <p>" + propiedades.gradoEducativo + "</p>" +
                      "     <p>" + propiedades.programaEducativo + "</p>" +
                      "   </section>" +
                      "   <div style ='text-align:left; margin-left:70px'><section>" +
                      "      <p><span><b> Grupo: </b></span>" + propiedades.nombreGrupo + "</p>" +
                      "      <p><span><b> Modalidad: </b></span>" + propiedades.modalidad + "(" + propiedades.dias + ")" + "</p>" +
                      "      <p><span><b> Plantel: </b></span>" + propiedades.plantel + "(" + propiedades.direccionPlantel + ")" + "</p>" +
                      "   </section></div><br/><br/>" +
                      "   <footer style='text-align:center; font-style:italic'>" +
                      "      <strong> Atentamente </strong><br>" +
                      "     " + nombreCompania + "" +
                      " <br/><a href=" + urlPlataforma + ">" + urlPlataforma + " </a>   " +
                      "   </footer>" +
                          "</div>";

            return mensaje;
        }
    }

    public class ObjetoCorreoElectronico
    {

        public int tipoCorreo { get; set; }
        public string asunto { get; set; }
        public string correo { get; set; }
        public string nombre { get; set; }
        public string contrasena { get; set; }
        //PROPIEDADES PARA EL CORREO DE BIENVENIDA

        public string gradoEducativo { get; set; }
        public string programaEducativo { get; set; }
        public string nombreGrupo { get; set; }
        public string modalidad { get; set; }
        public string dias { get; set; }
        public string plantel { get; set; }
        public string direccionPlantel { get; set; }
    }

}

