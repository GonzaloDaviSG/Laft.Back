using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace protecta.laft.api.DTO
{
    public class Consulta360
    {
        public int Ramo { get; set; }
        public int Producto { get; set; }
        public int Poliza { get; set; }
        public int Certificado { get; set; }
        public string FechaConsulta { get; set; }
        public string Endoso { get; set; }
    }
}
