
using System.Collections;
using System.Collections.Generic;

namespace PRASYDE.ControlEscolar.Entities
{

    public class Roles {

        public string idRol { get; set; }
        public int idJerarquia { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public ArrayList cadenaModuloPermiso { get; set; }
    }

    public class ObtenerRol {
        public string idRol { get; set; }
        public int idJerarquia { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public List<DetalleObtenerRol> cadenaModuloPermiso { get; set; }
        public ArrayList detalleModulo { get; set; }

    }

    public class DetalleObtenerRol {
        public int idModulo { get; set; }
        public ArrayList permisos { get; set; }
    }

    public class ListadoRoles
    {
        public string idRol { get; set; }
        public string nombre { get; set; }
        public string jerarquia { get; set; }
        public string descripcion { get; set; }
        public int estatus { get; set; }
    }

   
    public class FormularioRoles
    {
      
        public List<ListaGenericaCatalogos> jerarquias { get; set; }
        public List<ListaGenericaCatalogosTresValores> permisos{ get; set; }
        public List<listaModulos> modulos { get; set; }
    }
    
   

    public class listaModulos
    {
        public int idModuloPrincipal { get; set; }
        public string Modulo { get; set; }
        public int idPlataforma { get; set; }
        public string estilo { get; set; }
        public List<PropiedadesSubModulos> submodulos { get; set; }
    }
    
    public class PropiedadesSubModulos
    {
        public int idModuloPrincipal { get; set; }
        public int idSubModulo { get; set; }
        public string nombreSubModulo { get; set; }
        public List<PropiedadesPermisos> permisos { get; set; }
    }

    public class PropiedadesPermisos
    {
        public int idDetalleModuloPermiso { get; set; }
        public int idPermiso { get; set; }
        public string permiso { get; set; }
    }

    public class ListadoPermisosRoles
    {
        public List<ListaGenericaCatalogos> permisos { get; set; }
        public List<ListadoRoles> roles { get; set; }
    }
}
