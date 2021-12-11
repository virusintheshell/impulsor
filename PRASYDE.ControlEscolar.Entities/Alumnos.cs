

using System.Collections;
using System.Collections.Generic;

namespace PRASYDE.ControlEscolar.Entities
{
   public  class ListadoAlumnos
    {
        //LISTADO DE ALUMNOS
        public string idUsuario { get; set; }
        public string idAlumno { get; set; }
        public string matricula { get; set; }
        public string matriculaOficial { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public string celular { get; set; }
        public string estatusAlumno { get; set; }
        public string urlImagen { get; set; }
        public string inscripciones { get; set; }
        public int estatus { get; set; }
    }

    public class PropiedadesGrupoAlumnos
    {
        public string idAlumnoUnique { get; set; }
        public int idAlumno { get; set; }
        public string matricula { get; set; }
        public string nombre { get; set; }
        public string imagen { get; set; }
        public string correo { get; set; }
        public string celular { get; set; }
        public string estatusAlumno { get; set; }
        public int  estatus { get; set; }
    }


    public class ListadoPermisosAlumnos
    {
        public List<ListaGenericaCatalogos> permisos { get; set; }
        public List<ListadoAlumnos> alumnos { get; set; }
    }


    #region "CLASES PARA MOSTRAR EL DETALLE DEL ALUMNO"

    public class AlumnoDetalleInscripcion {

        public string idProgramaEducativo { get; set; }
        public string nombrePrograma { get; set; }
        public string urlImagen { get; set; }
        public string modalidad { get; set; }
        public string plantel { get; set; }
        public string dias { get; set; }
        public decimal porcentajeMaterias { get; set; }
        public decimal porcentajeAsistencia { get; set; }
        public string idGrupo { get; set; }
        public int nivelActual { get; set; }
        public List<CalificacionesPrograma> calificaciones { get; set; }
    }

    public class ListaPagosAlumno {
        public string nombrePrograma { get; set; }
        public decimal proximoPago { get; set; }
        public string estatus { get; set; }
        public List<ListaDetallePagosAlumno> listaPagos { get; set; }
    }

    public class ListaDetallePagosAlumno {
        public string concepto { get; set; }
        public string descripcion { get; set; }
        public decimal monto { get; set; }
        public string fecha { get; set; }
    }

    //DETALLE DE CALIFICACIONES POR PROGRAMA-ASIGNATURA 

    public class CalificacionesPrograma {
        public int nivel { get; set; }
        public ArrayList parcialesNivel { get; set; }
        public List<DetalleAsignaturasParciales> asignaturas { get; set; }
    }
    
    public class DetalleAsignaturasParciales {
        public int idAsignatura { get; set; }
        public string clave { get; set; }
        public string asignatura { get; set; }
        public string tipoCalif { get; set; }
        public double califFinal { get; set; }
        public List<DetalleParciales> parciales { get; set; }
    }

    public class DetalleParciales {
        public string parcial { get; set; }
        public double calif { get; set; }
    }
    
    public class ListadoDetalleAlumnos{
        public Usuarios usuario { get; set; }
        public List<AlumnoDetalleInscripcion> inscripciones { get; set; }
        public List<ListaPagosAlumno> pagos { get; set; }
    }

    #endregion
}
