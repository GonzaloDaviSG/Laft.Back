using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace protecta.laft.api.DTO
{
    public class ListaPolizas
    {
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Poliza { get; set; }
        public string CodAplicacion { get; set; }
        public string Producto { get; set; }
        public string FechaSolicitud { get; set; }
        public string Rol { get; set; }
        public string Tipo { get; set; }
        public string estado { get; set; }
        public string Ramo { get; set; }
        public int pagina { get; set; }
        public string NumeroResgistros { get; set; }
        public string Endoso { get; set; }
        public string Usuario { get; set; }

    }
}
