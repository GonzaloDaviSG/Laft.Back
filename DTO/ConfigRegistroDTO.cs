using System.Collections.Generic;

namespace protecta.laft.api.DTO
{
    public class ConfigRegistroDTO
    {
        public int idRegistro{get;set;}
         public List<ConfigAplicacionDTO> aplicaciones{get;set;}
    }
}