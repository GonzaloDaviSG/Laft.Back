using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace protecta.laft.api.Models
{
    [Table("TBL_HOLIDAYS", Schema = "LAFT")]
    public class Feriado
    {

        [Key]
        [Column("ANIO")]
        public int ANIO { get; set; }

        [Column("FECHA")]
        public DateTime FECHA { get; set; }

        

    }
}
