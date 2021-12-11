
using System.Collections.Generic;

namespace PRASYDE.ControlEscolar.Entities
{
    public class FormularioInscripcion
    {
        public List<ListaGenericaCatalogos> permisos { get; set; }
        public List<ListaGenericaCatalogosTresValores> gradoEducativo { get; set; }
    }

    public class ListaGruposPlantel
    {
        public List<GruposPlantel> planteles { get; set; }
        public List<ListaDocumentosPrograma> documentos { get; set; }
    }

    public class GruposPlantel {

        public int idPlantel { get; set; }
        public string nombrePlantel { get; set; }
        public string direccion { get; set; }
        public List<DetalleGrupoPlantel> grupos { get; set; }
    }

    public class DetalleGrupoPlantel {
        public string idGrupo { get; set; }
        public string claveGrupo { get; set; }
        public int cupo { get; set; }
        public int inscritos { get; set; }
        public string modalidad { get; set; }
        public string detalleModalidad { get; set; }
        public int estatusGrupo { get; set; }
    }

    public class Inscripcion {

        public string idAlumno { get; set; }
        public string idProgramaEducativo { get; set; }
        public string idgrupo { get; set; }
        public string correoAlumno { get; set; }
        public string nombreAlumno { get; set; }
        public string gradoEducativo { get; set; }
        public string programaEducativo { get; set; }
        public string nombreGrupo { get; set; }
        public string modalidad { get; set; }
        public string dias { get; set; }
        public string plantel { get; set; }
        public string direccionPlantel { get; set; }
        public DetalleInscripcion pago { get; set; }
    }

    public class DetalleInscripcion {
        public double montoInscripcion { get; set; }
        public double colegiatura { get; set; }
        public int periodicidad { get; set; }
        public string fechaInicio { get; set; }
    }
}
