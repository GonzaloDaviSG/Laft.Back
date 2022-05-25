using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace protecta.laft.api.Models
{
    [Table("TBL_MAE_PAIS", Schema = "LAFT")]
    public class Pais
    {
        [Key]
        [Column("NID")]
        public int nId { get; set; }
        [Column("SDESCRIPCION")]
        public string sDescripcion { get; set; }
        [Column("DREGISTRO")]
        public DateTime dRegistro { get; set; }
        [Column("SUSUARIO")]
        public string sUsuario { get; set; }
        [Column("NESTADO")]
        public Estado nEstado { get; set; }
    }
}