
using System.Collections.Generic;

namespace PRASYDE.ControlEscolar.Entities
{
    public class Conceptos
    {
        public string idConcepto { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
    }

    public class ListadoConceptos: Conceptos
    {
        public int tipo { get; set; }
        public int estatus { get; set; }
    }

    public class ListadoPermisosConceptos
    {
        public List<ListaGenericaCatalogos> permisos { get; set; }
        public List<ListadoConceptos> conceptos { get; set; }
    }
}
