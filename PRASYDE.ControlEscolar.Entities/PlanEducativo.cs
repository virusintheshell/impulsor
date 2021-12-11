
using System.Collections;
using System.Collections.Generic;

namespace PRASYDE.ControlEscolar.Entities
{
    public class PlanEducativo
    {
        public string idPlanEducativo { get; set; }
        public string nombre { get; set; }
        public string idProgramaEducativo { get; set; }
        public int generarNiveles { get; set; }
        public int niveles { get; set; }
        public List<DetalleProgramaEducativo> listadoAsignaturas { get; set; }
        public ArrayList listadoDocumentos { get; set; }
    }

    public class DetalleProgramaEducativo
    {
        public int idNivel { get; set; }
        public ArrayList idAsignatura { get; set; }
    }

    //CLASES PARA EL METODO DE OBTENER 

    public class PlanObtener
    {
        public string nombre { get; set; }
        public string idProgramaEducativo { get; set; }
        public string idPlanEducativo { get; set; }
        public int gradoEducativo { get; set; }
        public int generarNiveles { get; set; }
        public int niveles { get; set; }
        public List<ProgramaAsignaturasObtener> listadoAsignaturas { get; set; }
        public ArrayList listadoDocumentos { get; set; }
    }

    public class ProgramaAsignaturasObtener
    {
        public int idNivel { get; set; }
        public List<DetalleProgramaObtener> Asignaturas { get; set; }
    }


    public class DetalleProgramaObtener
    {
        public string nombre { get; set; }
        public string idAsignatura { get; set; }
    }

    //CLASE PARA EL LISTADO
    public class ListadoPlanEducativo
    {
        public string idPlanEducativo { get; set; }
        public string nombre { get; set; }
        public string nombrePrograma { get; set; }
        public string generarNiveles { get; set; }
        public string fechaRegistro { get; set; }
    }

    public class ListadoPermisosPlanesEducativos
    {
        public List<ListaGenericaCatalogos> permisos { get; set; }
        public List<ListadoPlanEducativo> planesEducativos { get; set; }
    }
}




