using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRASYDE.ControlEscolar.Entities
{
   public class OfertaEducativa
    {
        public string idOfertaEducativa { get; set; }
        public string idProgramaEducativo { get; set; }
        public int idDetalleModalidad { get; set; }
        public int idGradoEducativo { get; set; }
        public int idModalidad { get; set; }
        public string nombreGradoEducativo { get; set; }
        public string nombreProgramaEducativo { get; set; }
        public string nombreModalidad { get; set; }
        public string nombreDetalleModalidad { get; set; }
    }

    public class FormularioOfertaEducativa
    {
     
        public List<ListaGenericaCatalogosTresValores> gradoEducativo { get; set; }
        public List<ListaGenericaCatalogos> modalidades { get; set; }
    }

    public class FormularioOfertaEducativaMovil
    {
        public List<ListaGenericaCatalogosTresValores> gradoEducativo { get; set; }
    }

    public class ObtenerListaOferta {

        public List<ListaGradoEducativo> gradoEducativo { get; set; }
        public List<ListaModalidades> modalidades { get; set; }
    }
    
    public class ListaGradoEducativo {

        public int idGradoEducativo { get; set; }
        public string nombreGrado { get; set; }
        public List<ListaProgramaEducativo> programaEducativo { get; set; }
    }


    public class ListaProgramaEducativo {

        public int idGradoEducativo { get; set; }
        public string idProgramaEducativo { get; set; }
        public string nombrePrograma { get; set; }
        public string requisitos { get; set; }
    }
    
    public class ListaModalidades {
        public int idModalidad { get; set; }
        public string nombreModalidad { get; set; }
        public List<ListaDetalleModalidad> detalleModalidad { get; set; }
    }

    public class ListaDetalleModalidad {
        public int idDetalleModalidad { get; set; }
        public int idModalidad { get; set; }
        public string nombreDetalle { get; set; }
    }


    #region "CLASES PARA EL LISTADO DE OFERTA EDUCATIVA"

    public class ListadoOfertaEducativa {

        public string idOfertaEducativa { get; set; }
        public string gradoEducativo { get; set; }
        public string programaEducativo { get; set; }
        public string modalidad { get; set; }
        public string detalleModalidad { get; set; }
        public int estatus { get; set; }
    }
    
    public class ListadoPermisoOfertaEducativa
    {
        public List<ListaGenericaCatalogos> permisos { get; set; }
        public List<ListadoOfertaEducativa> ofertas { get; set; }
    }

    public class ListadoPermisoOfertaEducativaMovil
    {
        public List<ListadoOfertaEducativa> ofertas { get; set; }
    }
    #endregion
}
