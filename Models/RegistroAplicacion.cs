using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace protecta.laft.api.Models
{
    [Table("TBL_CFG_REGISTRO_APP", Schema = "LAFT")]
    public class RegistroAplicacion
    {
        [Key]
        [Column("NID")]
        public int nId { get; set; }
        [Column("NIDREGISTRO")]
        public int nIdRegistro { get; set; }
        [Column("NIDAPLICACION")]
        public int nIdAplicacion { get; set; }
        [Column("DREGISTRO")]
        public DateTime dRegistro { get; set; }
        [Column("SUSUARIO")]
        public string sUsuario { get; set; }
        [Column("NESTADO")]
        public Estado nEstado { get; set; }
    }
}