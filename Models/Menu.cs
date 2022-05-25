using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace protecta.laft.api.Models {

    [Table ("LAFT_RESOURCE", Schema = "LAFT")]
    public class Menu {     

      
        [Key]
        [Column ("NIDRESOURCE")]
        public int nResourceId { get; set; }

        [Column ("NIDFATHER")]
        public int nFatherId { get; set; }

        [Column ("SNAME")]
        public string sName { get; set; }

        [Column ("SDESCRIPTION")]
        public string sDescription { get; set; }

        [Column ("NTYPERESOURCE")]
        public int nResourceType { get; set; }

        [Column ("SHTML")]
        public string sHtml { get; set; }

        [Column ("NORDER")]
        public int nOrder { get; set; }

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

        [Column ("STAG")]
        public string sRouterLink { get; set; }

        [Column ("NIDSYSTEM")]
        public int sSystemId { get; set; }    

        [Column ("NIDPROFILE")]
        public int nProfileId { get; set; } 

    }
}