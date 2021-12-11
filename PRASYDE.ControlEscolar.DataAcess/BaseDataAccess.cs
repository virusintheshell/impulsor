

namespace PRASYDE.ControlEscolar.DataAcess
{
    using System.Data;
    using System.Web.Configuration;

    public class BaseDataAccess
    {
        protected string ConnectionString;

        public BaseDataAccess()
        {
            this.ConnectionString = WebConfigurationManager.ConnectionStrings["ImpulsorIntelectual"].ConnectionString;
        }
    }

    public class RegistrosDiferentes
    {
        public static DataTable Obtener(DataTable dt, string[] columnas)
        {
            DataTable dtRegistrosUnicos = new DataTable();
            dtRegistrosUnicos = dt.DefaultView.ToTable(true, columnas);
            return dtRegistrosUnicos;
        }
    }
}
