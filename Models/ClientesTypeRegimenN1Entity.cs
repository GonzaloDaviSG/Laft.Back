using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace protecta.laft.api.Models
{
    [DataContract]
    [Serializable]
    public class ClientesTypeRegimenN1Entity
    {
        [DataMember]
        public string tipoRegimen { get; set; }
        [DataMember]
        public int numeroClientes { get; set; }
    }
}
