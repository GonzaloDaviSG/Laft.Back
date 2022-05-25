
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace protecta.laft.api.Models
{
    [Table("TBL_TRX_REGISTRO", Schema = "LAFT")]
    public class Registro
    {
        [Key]
        [Column("NID")]
        public int nId { get; set; }

        [Column("NSEQ")]
        public int nSeq { get; set; }

        [Column("SNUMERO")]
        public string sNumero { get; set; }
        
        [Column("NIDCARGA")]
        public int nIdCarga{get; set;}

        [Column("NIDPERSONA")]
        public int nIdPersona{get;set;}
        [Column("SPERSONA")]
        public string sPersona { get; set; }
        [Column("NIDPAIS")]
        public int nIdPais{get;set;}
        [Column("SPAIS")]
        public string sPais { get; set; }
        [Column("NIDSENIAL")]
        public int nIdSenial{get;set;}
        [Column("SSENIAL")]
        public string sSenial { get; set; }
        [Column("NIDDOCUMENTO")]
        public int nIdDocumento{get;set;}
        [Column("SDOCUMENTO")]
        public string sDocumento { get; set; }

        [Column("SNUMDOC")]
        public string sNumDoc{get;set;}
        [Column("SAPEPAT")]
        public string sApePat { get; set; }
        [Column("SAPEMAT")]
        public string sApeMat { get; set; }
        [Column("SNOMBRE")]
        public string sNombre { get; set; }
        [Column("SOBSERVACION")]
        public string sObservacion { get; set; }
        [Column("DREGISTRO")]
        public DateTime? dRegistro { get; set; }
        [Column("SUSUARIO")]
        public string sUsuario { get; set; }
        [Column("NESTADO")]
        public Estado nEstado {get;set;}
        [Column("NEDITADO")]
        public int nEditado {get;set;}
        [Column("NLIBERADO")]
         public int nLiberado {get;set;}
        [Column("DCARGA")]
         public DateTime? dfechaCarga {get;set;}
         [Column("SCATNOMBRE")]
         public string ScategoriaNombre {get;set;}

         [Column("DVIGREGISTRO")]
         public DateTime? dfechaVigencia{get;set;}
    }
}