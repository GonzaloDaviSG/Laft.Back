using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace protecta.laft.api.DTO
{
    public class ResourceProfileDTO
    {
        public string sNameMenu { get; set; }
        public string sNameSubMenu { get; set; }
        public int nIdResource { get; set; }
        public int nIdFather { get; set; }
        public string sDescription { get; set; }
        public bool sActive { get; set; }
        public string sNameProfiles { get; set; }
        public int nIdProfile { get; set; }
    }
}
