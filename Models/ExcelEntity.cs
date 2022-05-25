using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace protecta.laft.api.Models
{
    [DataContract]
    [Serializable]
    public class ExcelEntity
    {
        [DataMember]
        public string rutaExcel { get; set; } 
        [DataMember]
        public string validator { get; set; } 
        [DataMember]
        public int? idGrupo { get; set; } 
        [DataMember]
        public int? idSubGrupo { get; set; }
    }
}
