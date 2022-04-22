using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRASYDE.ControlEscolar.Entities
{
    public class CatalogoDocumentos
    {
        public int idCatalogoExpedienteDigital { get; set; }
        public string nombre { get; set; }
    }
    public class ListadoPermisosDocumentos
    {
        public List<ListaGenericaCatalogos> permisos { get; set; }
        public List<CatalogoDocumentos> documentos { get; set; }
    }
}
