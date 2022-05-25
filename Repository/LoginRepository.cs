using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using protecta.laft.api.DTO;
using protecta.laft.api.Models;

namespace protecta.laft.api.Repository {
    public class LoginRepository : Interfaces.ILoginRepository {
        private DB.ApplicationDbContext context;

        public LoginRepository () {
            this.context = new DB.ApplicationDbContext (DB.ApplicationDB.UsarOracle ());
        }

        public userResponseDTO ValExistUser (string username, string password) {
            try {
                userResponseDTO response = new userResponseDTO ();
                OracleParameter SUSUARIO = new OracleParameter ("SUSUARIO", OracleDbType.Varchar2, username, System.Data.ParameterDirection.Input);
                OracleParameter SPASSWORD = new OracleParameter ("SPASSWORD", OracleDbType.Varchar2, password, System.Data.ParameterDirection.Input);
                OracleParameter p_IDPERFIL = new OracleParameter ("P_ID_PERFIL", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_IDUSUARIO = new OracleParameter ("P_ID_USUARIO", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_NOM_USUARIO = new OracleParameter ("P_NOM_USUARIO", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                OracleParameter p_STIPO_USUARIO = new OracleParameter ("P_STIPO_USUARIO", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                OracleParameter P_NCODE = new OracleParameter ("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter ("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);

                P_NCODE.Size = 200;
                P_SMESSAGE.Size = 4000;
                p_IDPERFIL.Size = 200;
                p_IDUSUARIO.Size = 200;
                p_NOM_USUARIO.Size = 200;
                p_STIPO_USUARIO.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { SUSUARIO, SPASSWORD, p_IDPERFIL, p_IDUSUARIO,p_NOM_USUARIO, p_STIPO_USUARIO, P_NCODE, P_SMESSAGE };

                var sqlQuery = @"
                    BEGIN 
                    PKG_LAFT_USUARIO.SP_GET_EXISTEUSUARIO(:SUSUARIO, :SPASSWORD, :P_ID_PERFIL, :P_ID_USUARIO,:P_NOM_USUARIO, :P_STIPO_USUARIO, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";
                
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (sqlQuery, parameters);
                var codigo = P_NCODE.Value;
                //var mensaje = P_SMESSAGE.Value;
                response.ingreso = (codigo.ToString () == "0") ? true : false;
                response.idPerfil = (response.ingreso == true) ? Int32.Parse (p_IDPERFIL.Value.ToString ()) : 0;
                response.idUsuario = (response.ingreso == true) ? Int32.Parse (p_IDUSUARIO.Value.ToString ()) : 0;
                response.username = username;
                response.fullName = p_NOM_USUARIO.Value.ToString();
                response.message = P_SMESSAGE.Value.ToString();
                response.tipoUsuario = p_STIPO_USUARIO.Value.ToString();
                this.context.Database.CloseConnection ();
                return response;
                
            } catch (Exception ex) {
                
                throw ex;
                //Utils.ExceptionManager.resolve (ex);
                //return null;
            }
        }
    }
}