using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace protecta.laft.api.Models {

    [Table ("LAFT_PROFILES_RESOURCE", Schema = "LAFT")]
    public class Profile {

        [Key]
        [Column ("NIDPROFILE")]
        public int nProfileId { get; set; }

        [Column ("NIDRESOURCE")]
        public int nResourceId { get; set; }

        [Column ("NUSERCODE")]
        public int nUserCode { get; set; }

        [Column ("DCOMPDATE")]
        public DateTime? dCompDate { get; set; }

        [Column ("DEFFECDATE")]
        public DateTime? dEffecDate { get; set; }

        [Column ("DNULLDATE")]
        public DateTime? dNullDate { get; set; }

        [Column ("NIDUSER")]
        public int nUserId { get; set; }

    }
}