
namespace PRASYDE.ControlEscolar.Entities
{
   public class ReporteGeneral
    {
        public int totalAlumnos { get; set; }
        public int totalProfesores { get; set; }
        public int totalGrupos { get; set; }
        public double totalPagoMensual { get; set; }
        public string mesCurso { get; set; }
    }

    public class ReportePagosMensuales
    {
        public double totalMonto { get; set; }
        public string fecha { get; set; }
    }

    public class ReporteInscripcionesMensuales
    {
        public double totalInscripcion { get; set; }
        public string fecha { get; set; }
    }
}
