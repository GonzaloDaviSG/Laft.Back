using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using protecta.laft.api.DTO;
using protecta.laft.api.Models;

namespace protecta.laft.api.Repository {
    public class UserConfigurationRepository : Interfaces.IUserConfigRespository {
        private DB.ApplicationDbContext context;

        public UserConfigurationRepository () {
            this.context = new DB.ApplicationDbContext (DB.ApplicationDB.UsarOracle ());
        }

        public List<User> GetAllUsers () {
            try {

                //return this.context.Users.Where (x => x.sState == "1").OrderByDescending (x => x.nUserId).ToList ();
                return this.context.Users.OrderByDescending(x => x.nUserId).ToList();

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return null;
            }
        }
        public List<UserStatus> GetAllUserStatus () {
            try {

                return this.context.UsersStatus.OrderByDescending (x => x.sUserStatus).ToList ();

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return null;
            }
        }

        public UserListResponseDTO GetUserData (int userId) {

            try {
                UserListResponseDTO userDataList = new UserListResponseDTO ();

                OracleParameter p_ID_USUARIO = new OracleParameter ("P_ID_USUARIO", OracleDbType.Int32, userId, System.Data.ParameterDirection.Input);
                OracleParameter p_USUARIO = new OracleParameter ("P_USUARIO", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                OracleParameter p_NOMBRECOMPLETO = new OracleParameter ("P_NOMBRECOMPLETO", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                OracleParameter p_PASSWORD = new OracleParameter ("P_PASSWORD", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                OracleParameter p_ESTADO = new OracleParameter ("P_ESTADO", OracleDbType.Char, System.Data.ParameterDirection.Output);
                OracleParameter p_CANT_ACCESFAIL = new OracleParameter ("P_CANT_ACCESFAIL", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_FEC_INICIO_PASS = new OracleParameter ("P_FEC_INICIO_PASS", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                OracleParameter p_FEC_FIN_PASS = new OracleParameter ("P_FEC_FIN_PASS", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                OracleParameter p_ID_ROL = new OracleParameter ("P_ID_ROL", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_ID_CARGO = new OracleParameter ("P_ID_CARGO", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_SEMAIL = new OracleParameter ("P_SEMAIL", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);

                p_ID_USUARIO.Size = 2000;
                p_USUARIO.Size = 2000;
                p_NOMBRECOMPLETO.Size = 2000;
                p_PASSWORD.Size = 2000;
                p_ESTADO.Size = 2000;
                p_CANT_ACCESFAIL.Size = 2000;
                p_FEC_INICIO_PASS.Size = 2000;
                p_FEC_FIN_PASS.Size = 2000;
                p_ID_ROL.Size = 2000;
                p_ID_CARGO.Size = 2000;
                p_SEMAIL.Size = 2000;

                OracleParameter[] parameters = new OracleParameter[] { p_ID_USUARIO, p_USUARIO, p_NOMBRECOMPLETO, p_PASSWORD, p_ESTADO, p_CANT_ACCESFAIL, p_FEC_INICIO_PASS, p_FEC_FIN_PASS, p_ID_ROL, p_ID_CARGO, p_SEMAIL, };

                var sqlQuery = @"
                    BEGIN 
                    PKG_LAFT_USUARIO.SP_GET_USUARIO(:P_ID_USUARIO,:P_USUARIO,:P_NOMBRECOMPLETO,:P_PASSWORD,:P_ESTADO,:P_CANT_ACCESFAIL,:P_FEC_INICIO_PASS,:P_FEC_FIN_PASS,:P_ID_ROL,:P_ID_CARGO,:P_SEMAIL);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (sqlQuery, parameters);
                userDataList.userId = Convert.ToInt32 (p_ID_USUARIO.Value.ToString ());
                userDataList.userName = p_USUARIO.Value.ToString ();
                userDataList.userFullName = p_NOMBRECOMPLETO.Value.ToString ();
                userDataList.pass = p_PASSWORD.Value.ToString ();
                userDataList.userState = p_ESTADO.Value.ToString ().Trim ();
                userDataList.accessAttempts = Convert.ToInt32 (p_CANT_ACCESFAIL.Value.ToString ());
                userDataList.startDatepass = p_FEC_INICIO_PASS.Value.ToString ();
                userDataList.endDatepass = p_FEC_FIN_PASS.Value.ToString ();
                userDataList.userRolId = Convert.ToInt32 (p_ID_ROL.Value.ToString ());
                userDataList.cargoId = Convert.ToInt32 (p_ID_CARGO.Value.ToString ());
                userDataList.userEmail = p_SEMAIL.Value.ToString () == "null" ? string.Empty : p_SEMAIL.Value.ToString ();
                this.context.Database.CloseConnection ();
                return userDataList;

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return null;
            }
        }

        public List<ProfileResponseDTO> GetProfiles () {
            List<ProfileResponseDTO> result = new List<ProfileResponseDTO> ();
            try {

                OracleParameter RC1 = new OracleParameter ("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_PERFIL_USUARIO(:RC1);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) RC1.Value).GetDataReader ();
                while (odr.Read ()) {
                    ProfileResponseDTO item = new ProfileResponseDTO ();
                    item.profileId = Convert.ToInt32 (odr["NIDPROFILE"].ToString ());
                    item.profileName = odr["SNAME"] == DBNull.Value ? string.Empty : odr["SNAME"].ToString ();
                    result.Add (item);
                }
                odr.Close ();
                this.context.Database.CloseConnection ();
            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }

        public List<CargoResponseDTO> GetLisCargo (int profileId) {
            List<CargoResponseDTO> result = new List<CargoResponseDTO> ();
            try {

                OracleParameter p_NIDPROFILE = new OracleParameter ("P_NIDPROFILE", OracleDbType.Int32, profileId, ParameterDirection.Input);             
                OracleParameter RC1 = new OracleParameter ("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] {p_NIDPROFILE, RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_CARGO(:P_NIDPROFILE,:RC1);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) RC1.Value).GetDataReader ();
                while (odr.Read ()) {
                    CargoResponseDTO item = new CargoResponseDTO ();
                    item.cargoId = Convert.ToInt32 (odr["NIDCARGO"].ToString ());
                    item.cargoName = odr["SDESCARGO"] == DBNull.Value ? string.Empty : odr["SDESCARGO"].ToString ();
                    result.Add (item);
                }
                odr.Close ();
                this.context.Database.CloseConnection ();
            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }
        public UpdateUserResponseDTO UpdateUser (int userId, string userName, string userFullName, string pass, int userUpd, string endDatepass, int userRolName, string systemId, string userEmail, int cargoId , string modifico, int state) {

            try {
                UpdateUserResponseDTO userUpdate = new UpdateUserResponseDTO ();

                OracleParameter p_NID_USUARIO = new OracleParameter ("NID_USUARIO", OracleDbType.Int32, userId, System.Data.ParameterDirection.Input);
                OracleParameter p_SUSUARIO = new OracleParameter ("SUSUARIO", OracleDbType.Varchar2, userName, System.Data.ParameterDirection.Input);
                OracleParameter p_SNOMBRECOMPLETO = new OracleParameter ("SNOMBRECOMPLETO", OracleDbType.Varchar2, userFullName, System.Data.ParameterDirection.Input);
                OracleParameter p_SPASSWORD = new OracleParameter ("P_SPASSWORD", OracleDbType.Varchar2, pass, System.Data.ParameterDirection.Input);
                OracleParameter p_NUSER_UPD = new OracleParameter ("NUSER_UPD", OracleDbType.Int32, userUpd, System.Data.ParameterDirection.Input);
                OracleParameter p_DFEC_FIN_PASS = new OracleParameter ("DFEC_FIN_PASS", OracleDbType.Varchar2, endDatepass, System.Data.ParameterDirection.Input);
                OracleParameter p_NID_ROL = new OracleParameter ("NID_ROL", OracleDbType.Int32, userRolName, System.Data.ParameterDirection.Input);
                OracleParameter p_NID_SISTEMA = new OracleParameter ("NID_SISTEMA", OracleDbType.Varchar2, systemId, System.Data.ParameterDirection.Input);
                OracleParameter p_SEMAIL = new OracleParameter ("P_SEMAIL", OracleDbType.Varchar2, userEmail, System.Data.ParameterDirection.Input);
                //OracleParameter p_SDNI = new OracleParameter ("P_SDNI", OracleDbType.Varchar2, dni, System.Data.ParameterDirection.Input);
                OracleParameter p_NID_CARGO = new OracleParameter ("NID_CARGO", OracleDbType.Int32, cargoId, System.Data.ParameterDirection.Input);
                OracleParameter p_MODIFICO = new OracleParameter("P_MODIFICO", OracleDbType.Varchar2, modifico, System.Data.ParameterDirection.Input);
                OracleParameter NESTADO = new OracleParameter("NESTADO", OracleDbType.Int32, state, System.Data.ParameterDirection.Input);

                OracleParameter p_NCODE = new OracleParameter ("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_SMESSAGE = new OracleParameter ("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);

                p_NCODE.Size = 4000;
                p_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { p_NID_USUARIO, p_SUSUARIO, p_SNOMBRECOMPLETO, p_SPASSWORD, p_NUSER_UPD, p_DFEC_FIN_PASS, p_NID_ROL, p_NID_SISTEMA, p_SEMAIL, p_NID_CARGO, p_MODIFICO, NESTADO, p_NCODE, p_SMESSAGE };

                var sqlQuery = @"
                    BEGIN 
                    PKG_LAFT_USUARIO.SP_UDP_USUARIO(:NID_USUARIO, :SUSUARIO,:SNOMBRECOMPLETO,:P_SPASSWORD,:NUSER_UPD,:DFEC_FIN_PASS,:NID_ROL,:NID_SISTEMA,:P_SEMAIL,:NID_CARGO , :P_MODIFICO, :NESTADO, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (sqlQuery, parameters);
                userUpdate.error = Convert.ToInt32 (p_NCODE.Value.ToString ());
                userUpdate.message = p_SMESSAGE.Value.ToString ();
                this.context.Database.CloseConnection ();
                return userUpdate;

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return null;
            }
        }

        public CreateUserResponseDTO CreateUser (string userName, string userFullName, string pass, string userReg, int userUpd, string startDatepass, string endDatepass, int userRolName, string systemId, string userEmail, int cargoId) {

            try {
                CreateUserResponseDTO userCreated = new CreateUserResponseDTO ();

                //OracleParameter p_NID_USUARIO = new OracleParameter ("NID_USUARIO", OracleDbType.Int32, 0 , System.Data.ParameterDirection.Input);
                OracleParameter p_SUSUARIO = new OracleParameter ("P_SUSUARIO", OracleDbType.Varchar2, userName, System.Data.ParameterDirection.Input);
                OracleParameter p_SNOMBRECOMPLETO = new OracleParameter ("P_SNOMBRECOMPLETO", OracleDbType.Varchar2, userFullName, System.Data.ParameterDirection.Input);
                OracleParameter p_SPASSWORD = new OracleParameter ("P_SPASSWORD", OracleDbType.Varchar2, pass, System.Data.ParameterDirection.Input);
                OracleParameter p_NUSER_REG = new OracleParameter ("NUSER_REG", OracleDbType.Int32, userReg, System.Data.ParameterDirection.Input);
                OracleParameter p_DFEC_INICIO_PASS = new OracleParameter ("DFEC_INICIO_PASS", OracleDbType.Varchar2, startDatepass, System.Data.ParameterDirection.Input);
                OracleParameter p_DFEC_FIN_PASS = new OracleParameter ("DFEC_FIN_PASS", OracleDbType.Varchar2, endDatepass, System.Data.ParameterDirection.Input);
                OracleParameter p_NID_ROL = new OracleParameter ("NID_ROL", OracleDbType.Int32, userRolName, System.Data.ParameterDirection.Input);
                OracleParameter p_NUSER_UPD = new OracleParameter ("NUSER_UPD", OracleDbType.Int32, userUpd, System.Data.ParameterDirection.Input);
                OracleParameter p_NID_SISTEMA = new OracleParameter ("NID_SISTEMA", OracleDbType.Varchar2, systemId, System.Data.ParameterDirection.Input);
                OracleParameter p_SEMAIL = new OracleParameter ("P_SEMAIL", OracleDbType.Varchar2, userEmail, System.Data.ParameterDirection.Input);
                OracleParameter p_NID_CARGO = new OracleParameter ("NID_CARGO", OracleDbType.Int32, cargoId, System.Data.ParameterDirection.Input);
                OracleParameter p_NCODE = new OracleParameter ("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_SMESSAGE = new OracleParameter ("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);

                p_NCODE.Size = 4000;
                p_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { p_SUSUARIO, p_SNOMBRECOMPLETO, p_SPASSWORD, p_NUSER_REG, p_DFEC_INICIO_PASS, p_DFEC_FIN_PASS, p_NID_ROL, p_NUSER_UPD, p_NID_SISTEMA, p_SEMAIL, p_NID_CARGO, p_NCODE, p_SMESSAGE };

                var sqlQuery = @"
                    BEGIN 
                    PKG_LAFT_USUARIO.SP_INS_USUARIO(:P_SUSUARIO,:P_SNOMBRECOMPLETO,:P_SPASSWORD,:NUSER_REG,:DFEC_INICIO_PASS, :DFEC_FIN_PASS,:NID_ROL,:NUSER_UPD,:NID_SISTEMA,:P_SEMAIL,:NID_CARGO, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (sqlQuery, parameters);
                userCreated.error = Convert.ToInt32 (p_NCODE.Value.ToString ());
                userCreated.message = p_SMESSAGE.Value.ToString ();
                this.context.Database.CloseConnection ();
                return userCreated;

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return null;
            }
        }

              public Dictionary<string, dynamic> GetHistorialUser(dynamic inputParam)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_USUARIO = new OracleParameter ("P_USUARIO", OracleDbType.Varchar2, inputParam.idUser, System.Data.ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_USUARIO, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_HISTORIAL_USUARIO(:P_USUARIO,:RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["ID_USUARIO"] = odr["ID_USUARIO"];
                    item["USUARIO"] = odr["USUARIO"];
                    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"];
                    item["SPERFIL"] = odr["SPERFIL"];
                    item["SCARGO"] = odr["SCARGO"];
                    item["SEMAIL"] = odr["SEMAIL"];
                    item["SESTADO"] = odr["SESTADO"];
                    item["SUSU_MODIFICA"] = odr["SUSU_MODIFICA"];
                    item["DFECHA_REGISTRO"] = odr["DFECHA_REGISTRO"];
                    item["SMODIFICADO"] = odr["SMODIFICADO"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
                objRespuesta["code"] = 0;
                objRespuesta["mensaje"] = "";
                objRespuesta["data"] = lista;
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = "Hubo un error.";
                objRespuesta["mensajeError"] = ex.Message;
                objRespuesta["mensajeErrorDetalle"] = ex;
                return objRespuesta;
                //throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            //return lista;
        }
    }
}