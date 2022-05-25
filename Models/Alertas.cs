using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace protecta.laft.api.Models
{
    [Table("TBL_LAFT_ALERTA_CAB_USUARIO", Schema = "LAFT")]
    public class Alertas
    {

        [Key]
        [Column("NIDALERTA_CAB_USUARIO")]
        public int NIDALERTA_CAB_USUARIO { get; set; }

        [Column("NPERIODO_PROCESO")]
        public int NPERIODO_PROCESO { get; set; }

        [Column("NIDALERTA")]
        public int NIDALERTA { get; set; }

        [Column("NCONTADOR_REENVIO")]
        public int NCONTADOR_REENVIO { get; set; }
        




    }
}
