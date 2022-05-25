using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace protecta.laft.api.Models
{
    public class BusquedaDemanda
    {
        public List<string> codBusqueda { get; set; }
        public List<string> nombreUsuario { get; set; }
        public List<string> nombreCliente { get; set; }
        public List<string> numeroRuc { get; set; }
        public List<string> tipoDocumento { get; set; }
    }
}
