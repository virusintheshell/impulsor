
using System.Collections.Generic;

namespace PRASYDE.ControlEscolar.Entities
{
    public class FormularioCambioGrupo
    {
      public List<ListaGenericaCatalogos> programaEducativos { get; set; }
    }

    public class DatosCambioGrupo {

        public string idProgramaEducativo { get; set; }
        public string nombrePrograma { get; set; }
        public string idGrupo { get; set; }
        public List<ListaGruposDisponibles> listaGrupos { get; set; }
    }

    public class ListaGruposDisponibles {

        public string idGrupo { get; set; }
        public string nombreGrupo { get; set; }
        public int nivelActual { get; set; }
        public string idPlanEducativo { get; set; }
        public string nombrePlan { get; set; }
        public int cupo { get; set; }
    }

    public class ListaProgramasDisponibles
    {
        public DatosCambioGrupo programaEducativo { get; set; }
        
    }

    public class CambioGrupo
    {
        public string idGrupo { get; set; }
        public string idAlumno { get; set; }
        public string idPrograma { get; set; }
        public MateriasOrigenDestino origin { get; set; }
        public MateriasOrigenDestino destiny { get; set; }
    }


    public class ListasAsignaturasCambioGrupo { 
    
        public MateriasOrigenDestino origin { get; set; }
        public MateriasOrigenDestino destiny { get; set; }
    }

    public class MateriasOrigenDestino
    {
        public int idGrupo { get; set; }
        public List<AsigunaturasListaCambioGrupo>  materiasLists { get; set; }

    }

    public class AsigunaturasListaCambioGrupo {
     
        public string idMateria { get; set; }
        public string materia { get; set; }
        public decimal calificacion { get; set; }
        public string status { get; set; }
    }

    //OBJETOS LA ACTUALIZACION DE CALIFICACIONES HISTORICAS

    public class PropiedadesCalificacionesHistoricas {
        public string idGrupo { get; set; }
        public int nivel { get; set; }
        public int idAsignatura { get; set; }
    }

    public class AsignaturasAlumnoHistorico : PropiedadesCalificacionesHistoricas
    {
        public List<CalificacionesAlumnoHistorico> alumnos { get; set; }
    }

    public class CalificacionesAlumnoHistorico {

        public int idAlumno { get; set; }
        public float calificacion { get; set; }
        public int tipo { get; set; }
    }
    //TERMINA OBJETOS LA ACTUALIZACION DE CALIFICACIONES HISTORICAS
}
