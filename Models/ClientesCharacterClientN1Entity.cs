using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace protecta.laft.api.Models
{
    [DataContract]
    [Serializable]
    public class ClientesCharacterClientN1Entity
    {
        [DataMember]
        public string tipoClientes { get; set; }
        [DataMember]
        public int numeroClientes { get; set; }
        [DataMember]
        public int numeroClienteReforzado { get; set; }
        [DataMember]
        public double montoPrima { get; set; }
    }
}
