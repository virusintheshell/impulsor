
using System.Collections.Generic;

namespace PRASYDE.ControlEscolar.Entities
{
    public class Asistencia
    {
        public string idGrupo { get; set; }
        public int idAsignatura { get; set; }
        public string fechaAsistencia { get; set; }
        public List<DetalleAsistencia> asistencias { get; set; }
    }

    public class DetalleAsistencia {
        public int idAlumno { get; set; }
        public int asistencia { get; set; }
    }
}
