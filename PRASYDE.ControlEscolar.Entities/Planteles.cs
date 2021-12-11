

using System.Collections;
using System.Collections.Generic;

namespace PRASYDE.ControlEscolar.Entities
{
    public class PlantelesListado : Planteles
    {

    }

    public class Planteles
    {
        public string idPlantel { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public decimal latitud { get; set; }
        public decimal longitud { get; set; }
        public string referencia { get; set; }
        public string contacto { get; set; }
        public string correoElectronico { get; set; }
        public string telefono { get; set; }
        public string extension { get; set; }
        public string urlImagen { get; set; }
        public int estatus { get; set; }
    }

    public class ListadoPermisosPlanteles
    {
        public bool accesoPagina { get; set; }
        public List<ListaGenericaCatalogos> permisos { get; set; }
        public List<Planteles> planteles { get; set; }
    }
    public class ListadoPermisosPlantelesMovil
    {
        public List<Planteles> planteles { get; set; }
    }
    
    public class FormularioAsignacion {
        public string nombrePlantel { get; set; }
        public List<FormularioAsignacionOfertas> ofertas { get; set; }
        public List<FormularioAsignacionOfertas> asignadas { get; set; }
    }

    public class FormularioAsignacionOfertas {
        public int idGradoEducativo { get; set; }
        public string nombre { get; set; }
        public List<DetalleFormularioAsignacion> ofertas { get; set; }
    }
    
    public class DetalleFormularioAsignacion
    {
        public int idOfertaEducativa { get; set; }
        public string programaEducativo { get; set; }
        public string modalidad { get; set; }
        public string  detalleModalidad { get; set; }
    }

    public class PropiedadesAsignacion {

        public string idPlantel { get; set; }
        public ArrayList idOfertas { get; set; }
    }
}
