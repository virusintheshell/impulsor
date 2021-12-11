
using System.Collections.Generic;

namespace PRASYDE.ControlEscolar.Entities
{
    public class PropiedadesCatalogos
    {
        public string nombrePagina { get; set; }
        public int id { get; set; }
        public string cadenaIds { get; set; }
    }

    public class ListaGenericaCatalogos
    {
        public int id { get; set; }
        public string nombre { get; set; }
    }

    public class ListaGenericaCatalogosTresValores : ListaGenericaCatalogos
    {
        public string valor { get; set; }
    }


    public class PermisosLista
    {
        public string modulo { get; set; }
        public List<DetallePermisos> permisosModulo { get; set; }
    }

    public class DetallePermisos
    {
        public int idPermiso { get; set; }
        public string permiso { get; set; }
        public string estilo { get; set; }
    }


    public class FormularioCatalogoGeneral
    {
        public List<ListaGenericaCatalogos> listado { get; set; }
    }
    public class FormularioCatalogoGeneralTresValores
    {
        public List<ListaGenericaCatalogosTresValores> listado { get; set; }
    }
}
