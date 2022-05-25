using System;
using System.Collections.Generic;
using System.Linq;

namespace protecta.laft.api.DTO
{
    public class RegistroDTO
    {
        public int id { get; set; }
        public int secuencia { get; set; }
        public string numero { get; set; }
        public int idCarga{get; set;}
        public MaestroDTO persona{get;set;}
        public MaestroDTO pais{get;set;}
        public SenialDTO senial{get;set;}
        public MaestroDTO documento{get;set;}
        public ConfigRegistroDTO configRegistro{get;set;}
        public List<ConfigAplicacionDTO> aplicaciones{get;set;}
        public string numeroDocumento{get;set;}
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string nombre { get; set; }
        public string observacion { get; set; }
        public string fechaRegistro { get; set; }
        public string usuario { get; set; }
        public bool activo {get;set;}
        public bool editado {get; set;}

        public bool liberado    {get;set;}
       public string fechaCarga { get; set; }

       public string categoriaNombre {get;set;}
       public string fechaVigencia {get; set;}
        
    }
}