using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace protecta.laft.api.DTO
{
    public class ResourceProfileHistoryDTO
    {
        public int nIdHistorial { get; set; }
        public string sProfileName { get; set; }
        public string sOpcion { get; set; }
        public string sMenu { get; set; }
        public string sSubMenu { get; set; }
        public string sAccion { get; set; }
        public string dFechaRegistro { get; set; }
        public string sUsuarioName { get; set; }
    }
}
