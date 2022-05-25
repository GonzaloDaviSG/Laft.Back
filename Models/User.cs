using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace protecta.laft.api.Models {

    [Table ("TBL_LAFT_USUARIO", Schema = "LAFT")]
    public class User {

        [Key]
        [Column ("ID_USUARIO")]
        public int nUserId { get; set; }

        [Column ("USUARIO")]
        public string sUser { get; set; }

        [Column ("PASSWORD")]
        public string sPass { get; set; }

        [Column ("NOMBRECOMPLETO")]
        public string sFullName { get; set; }

        [Column ("ESTADO")]
        public string sState { get; set; }

        [Column ("CANT_ACCESFAIL")]
        public int sAccessAttempts { get; set; }

        [Column ("USER_REG")]
        public int nUserReg { get; set; }

        [Column ("FECHA_REG")]
        public DateTime? dRegDate { get; set; }

        [Column ("USER_UPD")]
        public int nUserUpd { get; set; }

        [Column ("FECHA_UPD")]
        public DateTime? dUpdDate { get; set; }

        [Column ("FEC_INICIO_PASS")]
        public DateTime? dPassStartDate { get; set; }

        [Column ("FEC_FIN_PASS")]
        public DateTime? dPassEndDate { get; set; }

        [Column ("NIDPROFILE")]
        public int nRolId { get; set; }

         [Column ("SCARGO")]
        public string sCargo { get; set; }

        [Column ("ID_SISTEMA")]
        public string sSystemId { get; set; }

        [Column ("USUARIO_SISTEMA")]
        public string sSystemUser { get; set; }

        [Column ("SEMAIL")]
        public string sUserEmail { get; set; }

        [Column ("SDNI")]
        public string sDni { get; set; }

        [Column("SHASH")]
        public string sHash { get; set; }

        [Column("SENCRIP_USUARIO")]
        public string sEncrip_Usuario { get; set; }
        [Column("SENCRIP_CORREO")]
        public string sEncrip_Correo { get; set; }
        [Column("SDESENCRIP_ID")]
        public string sDesencrip_Id { get; set; }

        [Column("SENCRIP_ID")]
        public string sSencrip_Id { get; set; }

        [Column("FECHA_ENCRIP")]
        public DateTime? fecha_encrip { get; set; }

    }
}
