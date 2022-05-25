using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace protecta.laft.api.Models
{
    [Table("TBL_LAFT_CORREO_OC", Schema = "LAFT")]
    public class Correo_OC
    {

        [Key]
        [Column("CORREO")]
        public string CORREO { get; set; }

        [Column("PASSWORD")]
        public string PASSWORD { get; set; }

       




    }
}
