using System.Collections.Generic;

namespace protecta.laft.api.DTO
{
    public class CargaDTO
    {
        public int id{get; set;}
        public string fechaRegistro{get;set;}
        public string usuario{get; set;}
        public bool activo{get;set;}
        public List<RegistroDTO> registros {get; set;}
    }
}
