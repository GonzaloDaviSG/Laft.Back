using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace protecta.laft.api.Models
{
    [DataContract]
    [Serializable]
    public class Es10
    {
        [DataMember]
        public int
        codigo {get; set;}
        [DataMember]
        public string mensaje {get; set;}
        [DataMember]
        public int fila {get; set;}
        
        [DataMember]
        public ItemEs10 items { get; set; }

        [DataMember]
        public int cantidad
        { get; set; }

    }

    public class ItemEs10
    {
        [DataMember]
        public List<int> periodoProceso { get; set; }
        [DataMember]
        public List<string> ramo { get; set; }
        [DataMember]
        public List<string> riesgo { get; set; }
        [DataMember]
        public List<int> codRiesgo { get; set; }
        [DataMember]
        public List<string> codRegistro { get; set; }
        [DataMember]
        public List<string> nomComercial { get; set; }
        [DataMember]
        public List<string> moneda { get; set; }
        [DataMember]
        public List<DateTime?> fecini { get; set; }
        [DataMember]
        public List<int> numAsegurados { get; set; }
        [DataMember]
        public List<string> sRegimen { get; set; }
    }
}
