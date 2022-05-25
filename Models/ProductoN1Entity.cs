using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace protecta.laft.api.Models
{
    [DataContract]
    [Serializable]
    public class ProductoN1Entity
    {
        [DataMember]
        public string producto { get; set; }
        [DataMember]
        public int numeroPolizas { get; set; }
        [DataMember]
        public int numeroContratantes { get; set; }
        [DataMember]
        public int numeroAsegurados { get; set; }
        [DataMember]
        public int numeroBeneficiarios { get; set; }
        [DataMember]
        public int clienteReforzado { get; set; }
        [DataMember]
        public double montoPrima { get; set; }
    }
}
