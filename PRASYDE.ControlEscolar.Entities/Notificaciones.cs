using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRASYDE.ControlEscolar.Entities
{
    class Notificaciones
    {
    }

    //CLASE PARA LA RESPUESTA DEL ENVIO DE LA NOTIFICACION
    public class respuestaNotificacion
    {
        public string multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
        public int canonical_ids { get; set; }
        public List<respuestaResults> results { get; set; }
    }
    public class respuestaResults
    {
        public string message_id { get; set; }
    }


    //PROPIEDADES QUE MANEJAN INTERNAMENTE EN LA API
    //public class PropiedadesMensajeEnvioNotificacion
    //{
    //    public int id { get; set; }
    //    public string destinatario { get; set; }
    //    public string title { get; set; }
    //    public string body { get; set; }
    //    public string icon { get; set; }
    //}

    //PROPIEDADES QUE SE REGRESAN AL BO
    public class PropiedadesEnvioNotificacion
    {
        public string to { get; set; }
        public propiedadesData data { get; set; }
    }

    public class propiedadesData
    {
        public int idReferencia { get; set; }
       // public string destinatario { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string icon { get; set; }
    }

    //CLASES QUE SE UTILIZAN AL ENVIAR UNA NOTIFICACION
    public class tokenFirebase
    {
        public int id { get; set; }
        public string token { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string icon { get; set; }
    }

    public class PropiedadesMisNotificaciones: propiedadesData {

        public int tipoNotificacion { get; set; }
        public string nombreNotificacion { get; set; }
        //public string mensaje { get; set; }
        public string color { get; set; }
        public string url { get; set; }
        public string fechaHoraRegistro { get; set; }
    }

    public class ListadoMisNotificaciones
    {
        public List<PropiedadesMisNotificaciones> misNotificaciones { get; set; }
        
    }
}
