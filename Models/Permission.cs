using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace protecta.laft.api.Models {

    [Table ("TBL_LAFT_PROFILES", Schema = "LAFT")]
    public class Permission {

        [Key]
        [Column ("NIDPROFILE")]
        public int nProfileId { get; set; }

        [Column ("SNAME")]
        public string sName { get; set; }

        [Column ("SDESCRIPTION")]
        public string sDescription { get; set; }

        [Column ("SACTIVE")]
        public string sActive { get; set; }

        [Column ("NUSERCODE")]
        public int nUserCode { get; set; }

        [Column ("DCOMPDATE")]
        public DateTime? dCompDate { get; set; }

        [Column ("DEFFECDATE")]
        public DateTime? dEffecDate { get; set; }

        [Column ("DNULLDATE")]
        public DateTime? dNullDate { get; set; }

        [Column ("NIDSYSTEM")]
        public int sSystemId { get; set; }

    }
}