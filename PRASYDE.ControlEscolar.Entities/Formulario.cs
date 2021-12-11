
namespace PRASYDE.ControlEscolar.Entities
{
    public class Formulario
    {
        public string idFormularioUnique { get; set; }
        public string nombre { get; set; }
        public string texto { get; set; }
    }

    public class FormularioListado {

        public Formulario formulario { get; set; }
    }
}
