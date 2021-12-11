using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRASYDE.ControlEscolar.Entities
{
    public class Modalidades
    {
        public int idModalidad { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
    }

    public class ListadoModalidades : Modalidades
    {
        public int totalElementos { get; set; }
        public int estatus { get; set; }
    }

    public class DetalleModalidad {

        public int idDetalleModalidad { get; set; }
        public int idModalidad { get; set; }
        public string modalidad { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
    }

    public class ListadoDetalleModalidades : DetalleModalidad
    {
       
        public int estatus { get; set; }
    }

    public class FormularioDetalleModalidad
    {
       
        public List<ListaGenericaCatalogos> modalidades { get; set; }

    }

    public class ListadoPermisosModalidades
    {
        public List<ListaGenericaCatalogos> permisos { get; set; }
        public List<ListadoModalidades> modalidades { get; set; }
    }

}
