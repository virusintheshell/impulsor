


namespace PRASYDE.ControlEscolar.Entities
{
    using System.Collections.Generic;

     
    public class ObjetoMenu
    {
        public List<PropiedadesMenu> menu { get; set; }
    }

    public class PropiedadesMenu    
    {
        public string url { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
    }

    public class PropiedadesMenuIcon: PropiedadesMenu
    {
        public string icon { get; set; }
        public List<PropiedadesMenu> submenu { get; set; }
    }


    public class HeaderMenu: PropiedadesMenuIcon
    {
        public string header { get; set; }
    }
}
