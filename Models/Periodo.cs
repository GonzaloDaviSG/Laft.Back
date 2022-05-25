using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace protecta.laft.api.Models
{
    [Table("TBL_LAFT_ALERTA_FRECUENCIA", Schema = "LAFT")]
    public class Periodo
    {

        [Key]
        [Column("NIDFRECUENCIA")]
        public int NIDFRECUENCIA { get; set; }

        [Column("DFECINI")]
        public DateTime DFECINI { get; set; }

        [Column("SESTADO")]
        public int SESTADO { get; set; }

        [Column("NPERIODO_PROCESO")]
        public int NPERIODO_PROCESO { get; set; }

        
    }
}
