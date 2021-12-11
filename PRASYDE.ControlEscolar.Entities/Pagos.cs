


namespace PRASYDE.ControlEscolar.Entities
{
   
    using System.Web;
    using System.Collections.Generic;
  
    public class Pagos
    {
        public string idPago { get; set; }
        public string idPrograma { get; set; }
        public string idUsuario { get; set; }
        public string concepto { get; set; }
        public string descripcion { get; set; }
        public decimal monto { get; set; }
        public HttpFileCollection archivos { get; set; }
    }

    public class ListadoPagos
    {
        public string idPago { get; set; }
        public string nombreAlumno { get; set; }
        public string concepto { get; set; }
        public string fecha { get; set; }
        public string descripcion { get; set; }
        public decimal monto { get; set; }
        public string nombrePrograma { get; set; }
        public string urlImagen { get; set; }
        public int estatusPago { get; set; }
        public int estatus { get; set; }
      
    }

    public class ListadoPermisoPagos
    {
        public List<ListaGenericaCatalogos> permisos { get; set; }
        public List<ListadoPagos> pagos { get; set; }
    }
}
