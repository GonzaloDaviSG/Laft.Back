using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace protecta.laft.api.Models
{
    [Table("TBL_MAE_SENIAL", Schema = "LAFT")]
    public class Senial
    {
        [Key]
        [Column("NID")]
        public int nId { get; set; }
        [Column("SDESCRIPCION")]
        public string sDescripcion { get; set; }
        [Column("SCOLOR")]
        public string sColor { get; set; }
        [Column("DREGISTRO")]
        public DateTime dRegistro { get; set; }
        [Column("SUSUARIO")]
        public string sUsuario { get; set; }
        [Column("NESTADO")]
        public Estado nEstado { get; set; }
        [Column("NINDALERT")]
        public int nindalert {get;set;}
        [Column("NINDERROR")]
        public int ninderror {get;set;}

    }
}