using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace protecta.laft.api.Models
{
    [DataContract]
    [Serializable]
    public class ZonaGeograficaResponse
    {
        
        [DataMember]
        public List<ZonaGeograficaResponseEntity> ZonaGeografica { get; set; }
        [DataMember]
        public List<string> departamentos { get; set; }
    }

    public class ZonaGeograficaResponseEntity
    {
        [DataMember]
        public string rowId { get; set; }
        [DataMember]
        public int nPeriodoProceso { get; set; }
        [DataMember]
        public string tipDoc { get; set; }
        [DataMember]
        public string sProducto { get; set; }
        [DataMember]
        public string numDoc { get; set; }
        [DataMember]
        public string primerNombre { get; set; }
        [DataMember]
        public string segundoNombre { get; set; }
        [DataMember]
        public string apellidoParterno { get; set; }
        [DataMember]
        public string apellidoMaterno { get; set; }
        [DataMember]
        public string region { get; set; }
    }
}
