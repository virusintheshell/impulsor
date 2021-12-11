

using System.Collections.Generic;

namespace PRASYDE.ControlEscolar.Entities
{

    public class InfoDetalleUsuario
    {
        public string detalleFechaNacimiento { get; set; }
        public string detalleGenero { get; set; }
        public string detallEstadoCivil { get; set; }
        public string detalleGradoEscolar { get; set; }
        public string detalleOcupacion { get; set; }
    }

    public class Usuarios : InfoDetalleUsuario
    {
        public string idUsuario { get; set; }
        public int idGradoEstudios { get; set; }
        public string matriculaOficial { get; set; }
        public int idRol { get; set; }
        public string nombre { get; set; }
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }
        public string edad { get; set; }
        public string sexo { get; set; }
        public string telefono { get; set; }
        public string celular { get; set; }
        public string direccion { get; set; }
        public string fechaNacimiento { get; set; }
        public int idEstadoCivil { get; set; }
        public string correoElectronico { get; set; }
        public string cedula { get; set; }
        public int ocupacion { get; set; }
        public string CURP { get; set; }
        public string urlImagen { get; set; }
        public int idJerarquia { get; set; }
    }

    public class ListadoUsarios
    {

        public string idUsuario { get; set; }
        public string nombre { get; set; }
        public string rol { get; set; }
        public string correo { get; set; }
        public string celular { get; set; }
        public int estatus { get; set; }
    }

    public class FormularioUsuarios
    {
        public List<ListaGenericaCatalogosTresValores> roles { get; set; }
        public List<ListaGenericaCatalogos> ocupacion { get; set; }
    }

    public class ListadoPermisosUsuarios
    {

        public List<ListaGenericaCatalogos> permisos { get; set; }
        public List<ListadoUsarios> usuarios { get; set; }
    }


    public class UsuariosPreInscripcion
    {
        public string nombre { get; set; }
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }
        public string correoElectronico { get; set; }
        public string celular { get; set; }
        public string idGradoEducativo { get; set; }
        public string idProgramaEducativo { get; set; }
    }

    public class propiedadContasena
    {
        public string contrasena { get; set; }
    }

}
