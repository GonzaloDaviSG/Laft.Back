using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace protecta.laft.api.Models
{
    [DataContract]
    [Serializable]
    public class ActividadEconomica
    {
        [DataMember]
        public int codigo { get; set; }
        [DataMember]
        public string mensaje { get; set; }
        [DataMember]
        public int fila { get; set; }
        [DataMember]
        public ItemsActividadEconomica items { get; set; }
        [DataMember]
        public int cantidad { get; set; }
    }

    public class ItemsActividadEconomica
    {
        [DataMember]
        public List<int> nPeriodoProceso { get; set; }
        [DataMember]
        public List<string> sDescription { get; set; }
        [DataMember]
        public List<string> sNumRuc { get; set; }
        [DataMember]
        public List<string> sActivityEconomy { get; set; }
        [DataMember]
        public List<string> sTipoContribuyente { get; set; }
        [DataMember]
        public List<string> sSector { get; set; }
    }
}
