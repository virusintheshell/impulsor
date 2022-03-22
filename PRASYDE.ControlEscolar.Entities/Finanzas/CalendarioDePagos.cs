using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRASYDE.ControlEscolar.Entities.Finanzas
{
   public  class CalendarioDePagos
    {
        public string fechaInicial { get; set; }
        public string periodoDuracion { get; set; }
    }

    public class ListadoCalendarioPagos
    {
        public string fecha { get; set; }
        public string concepto { get; set; }
    }

    public class formularioCalendarioPagos
    {
        public List<ListaGenericaCatalogos> permisos { get; set; }
        public List<ListadoCalendarioPagos> listaCalendarioPagos { get; set; }
    }
}
