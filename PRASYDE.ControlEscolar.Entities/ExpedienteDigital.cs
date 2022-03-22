
using System.Web;
using System.Collections;
using System.Collections.Generic;

namespace PRASYDE.ControlEscolar.Entities
{

    public class propiedadesExpediente
    {
        public string idAlumno { get; set; }
        public int idDocumento { get; set; }
        public int TipoEvidencia { get; set; }
        public HttpFileCollection archivos { get; set; }
        public ArrayList urlArchivosEdicion { get; set; }
        public ArrayList urlArchivosEliminados { get; set; }
    }

    public class listadoArchivosAlumnos
    {
        public int idExpediente { get; set; }
        public string urlDocumento { get; set; }
    }

    public class ListadoPermisosArchivos
    {
        public List<listadoArchivosAlumnos> archivos { get; set; }
    }


}
