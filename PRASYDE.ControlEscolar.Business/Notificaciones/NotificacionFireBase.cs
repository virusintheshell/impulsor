
//AUTOR: DIEGO OLVEA 
//FECHA: 12-07-2020
//DESCRIPCION: CLASE QUE ENVIA LAS NOTIFIACIONES A LOS USUARIOS ATRAVES DE FIREBASE

using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using PRASYDE.ControlEscolar.Entities;
using PRASYDE.ControlEscolar.DataAcess.Framework;

namespace PRASYDE.ControlEscolar.Business.Notificaciones
{
    public class NotificacionFireBase
    {
        
         public static respuestaNotificacion EnviarNotificacionFireBase(tokenFirebase objetoMensaje)
        //public static respuestaNotificacion EnviarNotificacionFireBase(PropiedadesMensajeEnvioNotificacion objetoMensaje)
        {
            respuestaNotificacion objRepuesta = new respuestaNotificacion();
            respuestaResults objResultado = new respuestaResults();

            HttpWebRequest httpRequest = HttpWebRequest.Create("https://fcm.googleapis.com/fcm/send") as HttpWebRequest;
            httpRequest.ContentType = "application/json";
            httpRequest.Method = "POST";
            httpRequest.Headers.Add("Authorization", "key=AAAAxu8LEeI:APA91bGqBTtrLosoWpd_Ydmx1BJQf7OwyYpoiX6EahrQhmYGURVioOF8-zrhAa19h2hpUzFI8DK1bE9O2b9BZoxg9lCLgFYdhG3AcmbOCqQPrGAT3CdV1XdZ51Euv0B8bNZ3R2TKf9K5");
            propiedadesData objpropiedadesData = new propiedadesData();

            objpropiedadesData.idReferencia = objetoMensaje.id;
            objpropiedadesData.title = objetoMensaje.title;
            objpropiedadesData.body = objetoMensaje.body;
            objpropiedadesData.icon = objetoMensaje.icon;

            PropiedadesEnvioNotificacion objEnvioNotifiacion = new PropiedadesEnvioNotificacion();
            objEnvioNotifiacion.to = objetoMensaje.token; //destinatario
            objEnvioNotifiacion.data = objpropiedadesData;

            string sb = JsonConvert.SerializeObject(objEnvioNotifiacion);
            Byte[] bt = Encoding.UTF8.GetBytes(sb);

            Stream st = httpRequest.GetRequestStream();
            st.Write(bt, 0, bt.Length);
            st.Close();

            using (HttpWebResponse response = httpRequest.GetResponse() as HttpWebResponse)
            {

                try
                {
                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    string strsb = sr.ReadToEnd();
                }
                catch (Exception e) {
                    EscrituraLog.guardar("NotificacionFireBase-EnviarNotificacionFireBase. ", e.Message.ToString());
                }
     
            }
            return objRepuesta;
        }
    }
}
