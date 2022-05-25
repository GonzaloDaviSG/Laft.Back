using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace protecta.laft.api.Models
{
    [Table("TBL_CFG_REGISTRO_APP_PROD", Schema = "LAFT")]
    public class RegistroAplicacionProducto
    {
        [Key]
        [Column("NID")]
        public int nId { get; set; }
        [Column("NIDREGISTROAPP")]
        public int nIdRegistroApp { get; set; }
        [Column("NIDPRODUCTO")]
        public int nIdProducto { get; set; }
        [Column("DREGISTRO")]
        public DateTime dRegistro { get; set; }
        [Column("SUSUARIO")]
        public string sUsuario { get; set; }
        [Column("NESTADO")]
        public Estado nEstado { get; set; }
    }
}