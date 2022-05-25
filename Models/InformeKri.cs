using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace protecta.laft.api.Models
{
    [DataContract]
    [Serializable]
    public class InformeKri
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public string mesagge { get; set; }
        [DataMember]
        public List<Es10Entity> Es10 { get; set; }
        [DataMember]
        public List<Dictionary<string, dynamic>> Es10Cuadro { get; set; }
        [DataMember]
        public List<Dictionary<string, dynamic>> ActividadEconomicaCuadro { get; set; }
        [DataMember]
        public List<Dictionary<string, dynamic>> ZonasGeograficas { get; set; }
        [DataMember]
        public List<Dictionary<string, dynamic>> ZonaGeograficaCuadro { get; set; }
    }
}
