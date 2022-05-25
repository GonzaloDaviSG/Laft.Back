using System.Collections.Generic;
namespace protecta.laft.api.DTO
{
    public class ConfigSenialDTO
    {
        public SenialDTO senial { get; set; }
        public List<ConfigAplicacionDTO> aplicaciones{get;set;}
    }
}