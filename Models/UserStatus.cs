using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace protecta.laft.api.Models {

    [Table ("TBL_TMAE_STATUS_USER", Schema = "LAFT")]
    public class UserStatus {

        [Key]
        [Column ("NSTATUSUSER")]
        public string sUserStatus { get; set; }

        [Column ("SDESCRIPTION")]
        public string sDescriptionStatus { get; set; }

        [Column ("DCOMPDATE")]
        public DateTime? dCompDate { get; set; }

        [Column ("NUSERCODE")]
        public string sUserCode { get; set; }  

    }
}