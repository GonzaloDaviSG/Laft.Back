using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace protecta.laft.api.Models {
    
    [Table ("TBL_TRX_MONITOREO_REP_SBS", Schema = "INSUDB")]
    public class SbsReport {

        [Key]
        [Column ("SID")]
        public string sId { get; set; }

        [Column ("DINIREP")]
        public DateTime? dStartDate { get; set; }

        [Column ("DFINREP")]
        public DateTime? dEndDate { get; set; }

        [Column ("NTIPCAMBIO")]
        public decimal nExchangeRate { get; set; }

        [Column ("STIPOPE")]
        public int sOperType { get; set; }

        [Column ("NMONTO")]
        public decimal dAmount { get; set; }

        [Column ("NUSERCODE")]
        public string nUserId { get; set; }

        [Column ("NSTATUSPROC")]
        public int nStatusProc { get; set; }

        [Column ("DCOMPDATE")]
        public DateTime? dCompDate { get; set; }

        [Column ("DINIPROC")]
        public DateTime? dStartDateProc { get; set; }

        [Column ("DFINPROC")]
        public DateTime? dEndDateProc { get; set; }

        [Column ("SORIGEN")]
        public string sNameReport { get; set; }

        [Column ("SMENSAJE")]
        public string sMessage { get; set; }
        
        [Column ("STIPOARCH")]
        public string sfileType { get; set; }
    }
}