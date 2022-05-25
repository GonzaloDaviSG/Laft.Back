using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace protecta.laft.api.Models
{
    [DataContract]
    [Serializable]
    public class informeN1
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public string mesagge { get; set; }
        [DataMember]
        public List<ZonaGeograficaN1Entity> zonaGeograficas { get; set; }
        [DataMember]
        public List<ProductoN1Entity> productos { get; set; }
        [DataMember]
        public List<ClientesTypeRegimenN1Entity> clientesType { get; set; }
        [DataMember]
        public List<ClientesCharacterClientN1Entity> clientesCharacter { get; set; }
    }
}
