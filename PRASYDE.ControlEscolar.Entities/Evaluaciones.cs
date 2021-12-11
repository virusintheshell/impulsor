
using System.Collections;
using System.Collections.Generic;

namespace PRASYDE.ControlEscolar.Entities
{
   public class Evaluaciones {
       
        public string idGrupo { get; set; }
        public int categoriaEvaluacion { get; set; }
        public int idAsignatura { get; set; }
        public List<DetalleEvaluaciones> evaluaciones { get; set; }
    }

    public class DetalleEvaluaciones {

        public int tipo { get; set; }
        public int idAlumno { get; set; }
        public decimal calificacion { get; set; }
    }

    public class EdicionEvaluaciones {

        public string idGrupo { get; set; }
        public int idAlumno { get; set; }
        public int idAsignatura { get; set; }
        public ArrayList calificaciones { get; set; }
    }

    public class EdicionCalificacionFinal {

        public int idDetalleEvaluacion { get; set; }
        public int tipo { get; set; }
        public float calificacionFinal { get; set; }

    }
}
