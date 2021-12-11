
using System.Collections.Generic;
using System.Web;

namespace PRASYDE.ControlEscolar.Entities
{
    public class Asignaturas
    {
        public string idAsignatura { get; set; }
        public string clave { get; set; }
        public string nombre { get; set; }
        public string urlDocumento { get; set; }
        public int contieneDocumento { get; set; }
        public HttpFileCollection archivo { get; set; }
    }

    public class ListadoAsingaturas : Asignaturas
    {
        public string temario { get; set; }
        public int estatus { get; set; }
    }

    public class ListadoPermisosAsignaturas
    {
        public List<ListaGenericaCatalogos> permisos { get; set; }
        public List<ListadoAsingaturas> asignaturas { get; set; }
    }
}
