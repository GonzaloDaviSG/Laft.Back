using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace protecta.laft.api.DTO
{
    [DataContract]
    [Serializable]
    public class DemandaRequestDTO
    {
        [DataMember]
        public string P_SCODBUSQUEDA { get; set; }
        [DataMember]
        public string P_SNOMBREUSUARIO { get; set; }
        [DataMember]
        public string P_SNOMCOMPLETO { get; set; }
        [DataMember]
        public string P_SNUM_DOCUMENTO { get; set; }
        [DataMember]
        public int P_NPERIODO_PROCESO { get; set; }
        [DataMember]
        public int P_TIPOBUSQUEDA { get; set; }
        [DataMember]
        public int P_NOMBRE_RAZON { get; set; }
        [DataMember]
        public List<Proveedor> LFUENTES { get; set; }
    }
    [DataContract]
    [Serializable]
    public class Proveedor
    {

        [DataMember]
        public int NIDPROVEEDOR { get; set; }
        [DataMember]
        public string SDESPROVEEDOR { get; set; }
        [DataMember]
        public bool ISCHECK { get; set; }
    }
}
