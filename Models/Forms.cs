using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace protecta.laft.api.Models {

    [Table ("LAFT_FORMS_DATABASE", Schema = "LAFT")]
    public class Forms {

        [Key]
        [Column ("SKEY")]
        public string sSkey { get; set; }

        [Column ("DFECHA_RESPUESTA")]
        public DateTime? dDateResponse { get; set; }

        [Column ("SRESPONSABLE")]
        public string sPersonCharge { get; set; }

        [Column ("SPERIODO")]
        public string sPeriod { get; set; }

        [Column ("STIPO_SIGNAL")]
        public string sSignalType { get; set; }

        [Column ("NSTATUS")]
        public int nStatus { get; set; }

         [Column ("SSUMMARY")]
        public string sSummary { get; set; }

    }
}