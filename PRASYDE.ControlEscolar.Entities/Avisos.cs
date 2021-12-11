
namespace PRASYDE.ControlEscolar.Entities
{
    using System.Collections;
    using System.Collections.Generic;


    public class Avisos
    {
        public string idAviso { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string autor { get; set; }
        public string imagen { get; set; }
    }

    public class ListadoAvisos : Avisos
    {
        public string fecha { get; set; }
        public int estatus { get; set; }
    }

    public class ListadoPermisosAvisos
    {
        public List<ListaGenericaCatalogos> permisos { get; set; }
        public List<ListadoAvisos> avisos { get; set; }
    }

    public class ElementosEnvio {
        public int id { get; set; }
        public string nombre { get; set; }
        public int enviado { get; set; }
    }

    #region "CLASES PARA LOS METODOS DE ENVIO DE AVISOS"

    public class PropiedadesEnvio {
        public string idAviso { get; set; }
        public int tipoAviso { get; set; }
        public int enviarAtodos { get; set; }
        public ArrayList cadenaIds { get; set; }
    }
    #endregion
}
