
using System.Web;

namespace PRASYDE.ControlEscolar.Entities
{

    public class propiedadesExpediente
    {
        public string idAlumno { get; set; }
        public int idDocumento { get; set; }
        public int TipoEvidencia { get; set; }
        public HttpFileCollection archivos { get; set; }
    }

   
}
