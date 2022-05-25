using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace protecta.laft.api.Models
{
    [DataContract]
    [Serializable]
    public class ZonaGeografica
    {
        [DataMember]
        public int
        codigo {get; set;}
        [DataMember]
        public string mensaje {get; set;}
        [DataMember]
        public int fila {get; set;}
        
        [DataMember]
        public ItemZonaGeografica items { get; set; }

        [DataMember]
        public int cantidad
        { get; set; }

    }

    public class ItemZonaGeografica
    {
        [DataMember]
        public List<int> periodoProceso { get; set; }
        [DataMember]
        public List<string> producto { get; set; }
        [DataMember]
        public List<string> tipDoc { get; set; }
        [DataMember]
        public List<string> numDoc { get; set; }
        [DataMember]
        public List<string> primerNombre { get; set; }
        [DataMember]
        public List<string> segundoNombre { get; set; }
        [DataMember]
        public List<string> apellidoParterno { get; set; }
        [DataMember]
        public List<string> apellidoMaterno { get; set; }
        [DataMember]
        public List<string> region { get; set; }
    }
}
