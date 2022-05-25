using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace protecta.laft.api.Models
{
    [DataContract]
    [Serializable]
    public class Es10Response
    {
        
        [DataMember]
        public List<Es10Entity> Es10 { get; set; }
        [DataMember]
        public List<string> riesgosFilter { get; set; }
        [DataMember]
        public List<string> polizaFilter { get; set; }
        [DataMember]
        public List<string> monedaFilter { get; set; }
    }

    public class Es10Entity
    {
        [DataMember]
        public int nPeriodoProceso { get; set; }
        [DataMember]
        public string sRamo { get; set; }
        [DataMember]
        public string sRiesgo { get; set; }
        [DataMember]
        public int nCodRiesgo { get; set; }
        [DataMember]
        public string sCodRegistro { get; set; }
        [DataMember]
        public string sNomComercial { get; set; }
        [DataMember]
        public string sMoneda { get; set; }
        [DataMember]
        public string sFechaIniComercial { get; set; }
        [DataMember]
        public int nCantAsegurados { get; set; }
        [DataMember]
        public string sRegimen { get; set; }
    }
}
