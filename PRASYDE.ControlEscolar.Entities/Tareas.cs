
namespace PRASYDE.ControlEscolar.Entities
{
    using System.Collections.Generic;
    using System.Web;

    #region "CLASE PARA GUARDAR TAREAS QUE EL PROFESOR ASIGNA A SUS ALUMNOS
    
    public class Tareas
    {
        public string idTarea { get; set; }
        public string idGrupo { get; set; }
        public int idAsignatura { get; set; }
        public int tipo { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string fechaEntrega { get; set; }
        public int contieneDocumento { get; set; }
        public HttpFileCollection archivo { get; set; }

     
    }
    #endregion

    #region "CLASESS PARA GUARDAR LAS TAREAS (ALUMNOS)

    public class TareasAlumno {
        public string idTarea { get; set; }
        public string comentarios { get; set; }
        public HttpFileCollection archivo { get; set; }
    }
    #endregion




    #region "CLASESS PARA OBTNER EL LISTADO DE TAREAS ENTREGADAS (REVISON PROFESOR)

    //FILTRO PARA OBTENER EL LISTADO DE REVISION DE TAREAS
    public class TareasFiltroRevision
    {
        public string idGrupo { get; set; }
        public int idAlumno { get; set; }
    }


    public class ListaGeneralTareas {
        public List<ListaTareasRevision> asignaturas { get; set; }
    }
    public class ListaTareasRevision {
        public string Asignatura { get; set; }
        public List<TareasAsignadas> tareas{ get; set; }
    }

    public class TareasAsignadas
    {
        public string tituloTarea { get; set; }
        public string observaciones { get; set; }
        public string fechaSolicitada { get; set; }
        public TareasEntregadas tareaEntregada { get; set; }
    }

    public class TareasEntregadas {
        public int idDetalleTarea { get; set; }
        public string urlDocumento { get; set; }
        public string comentarios { get; set; }
        public string fechaEntrega { get; set; }
        public string comentarioProfesor { get; set; }
    }

    public class ObjetoComentariosProfesor {
        public int idDetalleTarea { get; set; }
        public string comentarioTarea { get; set; }
    }
    #endregion

    //LISTADO GENERA DE TAREAS PARA ALUMNOS Y PROFESORES 
    public class ListaTareas {

        public string idTareaProfesor { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string urlDocumento { get; set; }
        public string fechaEntrega { get; set; }
        public int idGrupo { get; set; }
        public string idGrupoUnique { get; set; }
        public string nombreGrupo { get; set; }
        public int idAsignatura { get; set; }
        public string nombreAsignatura { get; set; }
        public int tipo { get; set; }
        public string tipoTarea { get; set; }
        public string fechaHoraRegistro { get; set; }
        public string nombre { get; set; }
        public int tareaContestada { get; set; }
        public string comentarioProfesor { get; set; }

    }
}
