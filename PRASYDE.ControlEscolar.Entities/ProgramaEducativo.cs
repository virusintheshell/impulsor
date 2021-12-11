
using System.Collections.Generic;

namespace PRASYDE.ControlEscolar.Entities
{
    public class ListadoProgramaEducativo
    {
        //LISTADO
        public string idProgramaUnique { get; set; }
        public string gradoEducativo { get; set; }
        public string programaEducativo { get; set; }
        public string perfilIngreso { get; set; }
        public string perfilEgreso { get; set; }
        public string requisitos { get; set; }
        public string urlImagen { get; set; }
        public int estatus { get; set; }
    }

    public class ProgramaEducativo
    {
        public string idProgramaEducativo { get; set; }
        public int gradoEducativo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string perfilIngreso { get; set; }
        public string perfilEgreso { get; set; }
        public string requisitos { get; set; }
        public string urlImagen { get; set; }
    }
   
    public class FormularioProgramaEducativo
    {
        public List<GradosEcutivosPrograma> gradosEducativos { get; set; }
        public List<ListadoAsingaturas> listaAsignaturas { get; set; }
        public List<ListaDocumentosPrograma> listaDocumentos { get; set; }
    }

    public class ListadoPermisosProgramaEducativo
    {
        public List<ListaGenericaCatalogos> permisos { get; set; }
        public List<ListadoProgramaEducativo> programaEducativo { get; set; }
    }

    public class ListadoPermisosProgramaEducativoMovil
    {
       public List<ListadoProgramaEducativo> programaEducativo { get; set; }
    }
    
    public class ProgramaObtener
    {
        public string idProgramaEducativo { get; set; }
        public int gradoEducativo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string perfilIngreso { get; set; }
        public string perfilEgreso { get; set; }
        public string requisitos { get; set; }
        public string urlImagen { get; set; }
    }

    public class GradosEcutivosPrograma : GradosEcutivosProgramaExtra
    {

        public int id { get; set; }
        public string nombre { get; set; }
        public string valor { get; set; }
    }

    public class GradosEcutivosProgramaExtra
    {

        public string pagina { get; set; }
        public string paginaOrigen { get; set; }
    }
}
