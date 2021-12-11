

namespace PRASYDE.ControlEscolar.Entities
{
    public class propiedadesReenvioCorreo
    {
        public string idUsuario { get; set; }
        public string correoElectronico { get; set; }
    }

    public class Componente
    {
        public string idComponenteUnique { get; set; }
        public string nombre { get; set; }
        public string texto { get; set; }
    }

    public class ComponenteFormulario
    {
        public Componente componente { get; set; }
    }
}
