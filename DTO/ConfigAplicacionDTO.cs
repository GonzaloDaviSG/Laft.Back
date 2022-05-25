using System.Collections.Generic;

namespace protecta.laft.api.DTO
{
    public class ConfigAplicacionDTO
    {
        public int id{get;set;}
        public string fechaRegistro { get; set; }
        public string usuario { get; set; }
        public bool activo {get;set;}
        public MaestroDTO aplicacion{get;set;}
        public List<ConfigProductoDTO> productos{get;set;}
    }
}