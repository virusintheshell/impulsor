
namespace PRASYDE.ControlEscolar.Entities
{
    using System.Web;
    using System.Collections;
    using System.Collections.Generic;
    
    public class ListadoProfesores
    {
        public string idUsuario { get; set; }
        public string idProfesor { get; set; }
        public string matricula { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public string celular { get; set; }
        public string gradoEstudios { get; set; }
        public int estatus { get; set; }
    }
    
    public class ListadoPermisosProfesores
    {
        public List<ListaGenericaCatalogos> permisos { get; set; }
        public List<ListadoProfesores> profesores { get; set; }
    }

    public class AsignacionProfesorAsignatura {

        public string idProfesor { get; set; }
        public ArrayList asignaturas { get; set; }
    }


    public class AsignaturasAsignadasProfesor {

        public string idAsignaturaUnique { get; set; }
        public string nombre { get; set; }
        public int idProfesor { get; set; }
        public string urlDocumento { get; set; }
    }

    #region "CLASES PARA MOSTRAR EL HORARIO DEL PROFESOR, LISTADO DE ALUMNOS"

    public class PropiedadesHorarioProfesor
    {
     
        public string idAsignatura { get; set; }
        public string asignatura { get; set; }
        public List<PropiedadesHorarioDetalleProfesor> dias { get; set; }
    }

    public class PropiedadesHorarioDetalleProfesor
    {

        public string nombre { get; set; }
        public string inicio { get; set; }
        public string final { get; set; }
    }


    public class FormularioDetalleGruposProfesor
    {
        public List<PropiedadesHorarioProfesor> asignaturas { get; set; }
        public List<PropiedadesGrupoAlumnos> alumnos { get; set; }
    }
    #endregion


    #region "CLASES PARA MOSTRAR EL HORARIO DEL PROFESOR, LISTADO DE CALIFICACIONES"

    public class ParcialesAlumnos {
        public int finalGradesSaved { get; set; }
        public List<Parciales> alumnos { get; set; }
    }

    public class Parciales {
        public string matricula { get; set; }
        public int idAlumno { get; set; }
        public string nombre { get; set; }
        public List<DetalleParcial> parciales { get; set; }
        public Tipo tipo { get; set; }
        public double promedio { get; set; }
        public double final { get; set; }
        public int idDetalleEvaluacion { get; set; }
    }

    public class DetalleParcial {
        public string nombre { get; set; }
        public double calif { get; set; }
    }
    
    public class Tipo {
        public int id { get; set; }
        public string nombreTipo { get; set; }

    }

    public class filtrosCalificaion {
        public string idGrupo { get; set; }
        public int idAsignatura { get; set; }
        public int tipo { get; set; }
        public string idAlumno { get; set; }
    }

    #endregion


    #region "CLASES PARA MOSTRAR EL HORARIO DEL PROFESERO, LISTADO DE ASISTENCIA"

    public class ListadoAsistenciaAlumnos
    {
        public List<AsistenciaAlumnos> alumnos { get; set; }
    }

    public class AsistenciaAlumnos {
        public int idAlumno { get; set; }
        public string matricula { get; set; }
        public string nombre { get; set; }
        public string fechaHoraRegistro { get; set; }
        public List<DetalleAsistenciaLista> asistencias { get; set; }

    }
    public class DetalleAsistenciaLista {

        public string fecha { get; set; }
        public int asistencia { get; set; }
    }

    #endregion

    #region "CLASES PARA EL LISTADO DE MIS ASIGNATURAS"


    public class PropiedadesDocumentosProfesor {
             
        public int idDetalleDocumento { get; set; }
        public int idAsignatura { get; set; }
        public int contieneDocumento { get; set; }
        public HttpFileCollection archivo { get; set; }
    }

    public class PropiedadesMisAsignaturas {
              
        public int idDetalleDocumento { get; set; }
        public int idAsignatura { get; set; }
        public string clave { get; set; }
        public string nombre { get; set; }
        public string temarioPlantel { get; set; }
        public string temarioProfesor { get; set; }
        public int gruposAsignados { get; set; }
    }
    
    public class ListadoPropiedadesMisAsignaturas
    {
        public List<PropiedadesMisAsignaturas> misAsignaturas { get; set; }
    }
    #endregion
}

