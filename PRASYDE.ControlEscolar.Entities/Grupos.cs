

namespace PRASYDE.ControlEscolar.Entities
{
    using System.Collections.Generic;

    public class Grupos
    {
        public string idGrupo { get; set; }
        public string nombreGrupo { get; set; }
        public int idPlantel { get; set; }
        public string idOfertaEducativa { get; set; }
        public string idPlanEducativo { get; set; }
        public int cupo { get; set; }
        public int nivel { get; set; }
        public List<Horarios> horario { get; set; }
    }

    public class GruposEdicion
    {
        public string idGrupo { get; set; }
        public string nombreGrupo { get; set; }
        public int cupo { get; set; }
    }


    public class Horarios
    {
        public int dia { get; set; }
        public List<AsignaturasHorarios> asignaturas { get; set; }
    }

    public class AsignaturasHorarios
    {
        public string idAsignatura { get; set; }
        public string inicio { get; set; }
        public string fin { get; set; }
    }

    public class HorariosPorNivel
    {
        public string idGrupo { get; set; }
        public List<Horarios> horario { get; set; }
    }

    public class PropiedadesFiltro
    {
        public string idPlantel { get; set; }
        public string idGradoEducativo { get; set; }
        public int estatus { get; set; }
    }

    public class ListadoGrupos
    {
        public string idGrupo { get; set; }
        public string clave { get; set; }
        public int idOfertaEducativa { get; set; }
        public string planEducativo { get; set; }
        public string programaEducativo { get; set; }
        public string modalidad { get; set; }
        public string plantel { get; set; }
        public int cupo { get; set; }
        public int inscritos { get; set; }
        public int horarioGrupo { get; set; }
        public string estatusGrupo { get; set; }
        public int nivelActual { get; set; }
        public int generarNiveles { get; set; }
        public int estatus { get; set; }
    }


    public class ListadoPermisosGrupos
    {
        public List<ListaGenericaCatalogos> permisos { get; set; }
        public List<ListadoGrupos> grupos { get; set; }
    }

    public class PropiedadesAsignaturasPorNivel
    {
        public string idAsignatura { get; set; }
        public string clave { get; set; }
        public string nombreAsignatura { get; set; }
    }

    public class ListadoAsignaturasPorNivel
    {
        public List<PropiedadesAsignaturasPorNivel> asignaturas { get; set; }
    }




    #region "CLASES PARA ARMAR EL OBJETO DE LA ASIGNACION DE GRUPO - PROFESOR"

    public class GrupoAsingaturas
    {
        public string claveGrupo { get; set; }
        public int cupo { get; set; }
        public List<AsignaturaProfesor> asignaturas { get; set; }
    }

    public class AsignaturaProfesor
    {
        public string idAsignatura { get; set; }
        public string clave { get; set; }
        public string nombre { get; set; }
        public List<DetalleAsignaturaProfesor> profesores { get; set; }
    }
    public class DetalleAsignaturaProfesor
    {
        public int idProfesor { get; set; }
        public string matricula { get; set; }
        public string nombre { get; set; }
        public int asignado { get; set; }
    }

    //public class ListadoGruposAsignacion
    //{

    //    public GrupoAsingaturas grupos { get; set; }
    //}
    #endregion

    #region "CLASES PARA ARMAR EL FORMULARIO DE GRUPOS"


    public class GruposGrados
    {
        public int idGrado { get; set; }
        public string nombre { get; set; }
        public List<GruposPrograma> ProgramasEducativos { get; set; }
    }

    public class GruposPrograma
    {
        public int idProgramaEducativo { get; set; }
      //  public int nivelesGenerados { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public List<GruposModalidades> modalidades { get; set; }
        public List<PlanesEducativosLista> planesEducativos { get; set; }
    }

    public class PlanesEducativosLista
    {
        public string idPlanEducativo { get; set; }
        public string nombrePlan { get; set; }
        public int nivelesGenerados { get; set; }
        public List<GruposNiveles> niveles { get; set; }
        public List<GruposAsignaturas> asignaturasSinNivel { get; set; }
    }


    public class GruposNiveles
    {

        public int nivel { get; set; }
        public List<GruposAsignaturas> Asignaturas { get; set; }
    }

    public class GruposAsignaturas
    {
        public string idAsignatura { get; set; }
        public string clave { get; set; }
        public string nombreAsignatura { get; set; }
    }

    public class GruposModalidades
    {

        public int idModalidad { get; set; }
        public string nombreModalidad { get; set; }
        public List<GruposHorarios> Horario { get; set; }
    }

    public class GruposHorarios
    {
        public string idOfertaEducativa { get; set; }
        public int idHorario { get; set; }
        public string Horario { get; set; }

    }


    public class FormularioGrupos
    {
        public List<GruposGrados> gradosEducativos { get; set; }
    }


   

#endregion

#region "CLASES PARA GUARDAR LA ASIGNACION DE GRUPOS A PROFESORES"


public class PropiedadesGruposProfesor
{
    public string idgrupo { get; set; }
    public List<PropiedadesGruposProfesorDetalle> asignaturas { get; set; }
}
public class PropiedadesGruposProfesorDetalle
{

    public int idProfesor { get; set; }
    public string idAsignatura { get; set; }
}
#endregion

#region "CLASES PARA MOSTRAR EL HORARIO DEL GRUPO, LISTADO DE ALUMNOS"

    public class PropiedadesHorarioAsignaturas
    {
        public int id { get; set; }
        public string clave { get; set; }
        public string nombre { get; set; }
        public string profesor { get; set; }
        public List<PropiedadesHorarioDetalle> dias { get; set; }
    }

    public class PropiedadesHorarioDetalle
    {

        public string nombre { get; set; }
        public string inicio { get; set; }
        public string final { get; set; }
    }


    public class FormularioDetalleGrupos
    {

        public List<PropiedadesHorarioAsignaturas> asignaturas { get; set; }
        public List<PropiedadesGrupoAlumnos> alumnos { get; set; }
        public GrupoAsingaturas asignaturasProfesor { get; set; }

    }
    #endregion

}
