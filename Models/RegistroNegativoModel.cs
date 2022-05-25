using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace protecta.laft.api.Models
{
    [DataContract]
    [Serializable]
    public class RegistroNegativoModel
    {
        [DataMember]
        public int
        codigo {get; set;}
        [DataMember]
        public string mensaje {get; set;}
        [DataMember]
        public int fila {get; set;}
        
        [DataMember]
        public ItemRegistroNegativo items { get; set; }

        [DataMember]
        public int cantidad
        { get; set; }

    }

    public class ItemRegistroNegativo
    {
        [DataMember]
        public string [] numero { get; set; }
        [DataMember]
        public string[] tipoPersona { get; set; }
        [DataMember]
        public string[] pais { get; set; }
        [DataMember]
        public string[] tipoDocumento { get; set; }
        [DataMember]
        public string[] numeroDocumento { get; set; }
        [DataMember]
        public string[] apellidoParteno { get; set; }
        [DataMember]
        public string[] apellidoMaterno { get; set; }
        [DataMember]
        public string[] nombre { get; set; }
        [DataMember]
        public string[] senalLaft { get; set; }
        [DataMember]
        public string[] filtro { get; set; }
        [DataMember]
        public string[] fechaNacimiento { get; set; }
        [DataMember]
        public string[] documentoReferencia { get; set; }
        [DataMember]
        public string[] tipoLista { get; set; }
        [DataMember]
        public string[] numeroDocumento2 { get; set; }
        [DataMember]
        public string[] nombreCompleto { get; set; }
    }
}
