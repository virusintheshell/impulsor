

using System.Collections.Generic;

namespace PRASYDE.ControlEscolar.Entities
{
    public class Mensajes
    {
        public string idUsuarioRecibe { get; set; }
        public string mensaje { get; set; }
    }

    public class PropiedadesMensajeria
    {
        public string idMensajeUnique { get; set; }
        public int idUsuarioEnvia { get; set; }
        public int idUsuarioRecibe { get; set; }
        public string mensaje { get; set; }
        public string fecha { get; set; }
        public string fechaSinFormato { get; set; }
        public int mensajePropio { get; set; }
    }


    public class nivelUsuarios {
        public string nivel { get; set; }
        public List<listaUsuariosChat> usuarios { get; set; }
    }

    public class listaUsuariosChat {

        public string idUsuario { get; set; }
        public string nombre { get; set; }
        public string imagen { get; set; }
    }
}
