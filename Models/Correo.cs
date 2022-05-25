using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace protecta.laft.api.Models
{
    [Table("TBL_LAFT_CONFIGURACION_CORREO", Schema = "LAFT")]
    public class Correo
    {

        [Key]
        [Column("NIDCORREO")]
        public int NIDCORREO { get; set; }

        [Column("NCANTIDAD_DIAS")]
        public int NCANTIDAD_DIAS { get; set; }

        [Column("NIDACCION")]
        public int NIDACCION { get; set; }
        



    }
}
