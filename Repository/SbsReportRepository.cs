using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using protecta.laft.api.DTO;
using protecta.laft.api.Models;

namespace protecta.laft.api.Repository {
    public class SbsReportRepository : Interfaces.ISbsReportRepository {
        private DB.ApplicationDbContext context;
        private DB.ApplicationDbContext context2;

        public SbsReportRepository () {
            this.context = new DB.ApplicationDbContext (DB.ApplicationDB.UsarPrincipalGestor ());
            this.context2 = new DB.ApplicationDbContext (DB.ApplicationDB.UsarOracle ());
        }

        public List<CommentListResponseDTO> GetCommentList (int alertId, int periodId) {
            List<CommentListResponseDTO> result = new List<CommentListResponseDTO> ();
            try {

                OracleParameter P_NIDALERTA = new OracleParameter ("P_NIDALERTA", OracleDbType.Int32, alertId, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter ("P_NPERIODO_PROCESO", OracleDbType.Int32, periodId, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter ("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_COMENT_ALERTA_PERIODO(:P_NIDALERTA,:P_NPERIODO_PROCESO, :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) RC1.Value).GetDataReader ();
                while (odr.Read ()) {
                    CommentListResponseDTO item = new CommentListResponseDTO ();
                    item.regDate = odr["DFECHA_REGISTRO"] == DBNull.Value ? string.Empty : odr["DFECHA_REGISTRO"].ToString ();
                    item.fullName = odr["NOMBRECOMPLETO"] == DBNull.Value ? string.Empty : odr["NOMBRECOMPLETO"].ToString ();
                    item.comment = odr["SCOMENTARIO"] == DBNull.Value ? string.Empty : odr["SCOMENTARIO"].ToString ();
                    result.Add (item);
                }
                odr.Close ();
                this.context.Database.CloseConnection ();

            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }

        public UpdateCommentListResponseDTO UpdateCommentList (int alertId, int periodId, string comment, int userId) {

            try {

                UpdateCommentListResponseDTO result = new UpdateCommentListResponseDTO ();
                OracleParameter P_NIDALERTA = new OracleParameter ("P_NIDALERTA", OracleDbType.Int32, alertId, System.Data.ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter ("P_NPERIODO_PROCESO", OracleDbType.Int32, periodId, System.Data.ParameterDirection.Input);
                OracleParameter P_SCOMENTARIO = new OracleParameter ("P_SCOMENTARIO", OracleDbType.Varchar2, comment, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_MODIFICA = new OracleParameter ("P_NIDUSUARIO_MODIFICA", OracleDbType.Int32, userId, System.Data.ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter ("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter ("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, P_SCOMENTARIO, P_NIDUSUARIO_MODIFICA, P_NCODE, P_SMESSAGE };

                var sqlQuery = @"
                    BEGIN 
                    LAFT.PKG_LAFT_GESTION_ALERTAS.SP_INS_COMENT_ALERTA_PERIODO(:P_NIDALERTA, :P_NPERIODO_PROCESO,:P_SCOMENTARIO,:P_NIDUSUARIO_MODIFICA,:P_NCODE, :P_SMESSAGE);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (sqlQuery, parameters);
                result.code = Convert.ToInt32 (P_NCODE.Value.ToString ());
                result.message = P_SMESSAGE.Value.ToString ();
                this.context.Database.CloseConnection ();
                return result;

            } catch (Exception ex) {               
                Utils.ExceptionManager.resolve (ex);
                return null;
            }
        }

        public List<QuestionsByAlertResponseDTO> GetQuestionsByAlert (QuestionsByAlertParametersDTO param) {
            List<QuestionsByAlertResponseDTO> result = new List<QuestionsByAlertResponseDTO> ();
            try {

                OracleParameter p_NIDALERTA = new OracleParameter ("P_NIDALERTA", OracleDbType.Int32, param.alertId, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter ("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { p_NIDALERTA, RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_PREGUNTA_X_ALERTA(:P_NIDALERTA, :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) RC1.Value).GetDataReader ();
                while (odr.Read ()) {
                    QuestionsByAlertResponseDTO item = new QuestionsByAlertResponseDTO ();

                    item.alertId = Convert.ToInt32 (odr["NIDALERTA"].ToString ());
                    item.questionId = Convert.ToInt32 (odr["NIDPREGUNTA"].ToString ());
                    item.originId = Convert.ToInt32 (odr["NIDORIGEN"].ToString ());
                    item.questionDescription = odr["SPREGUNTA"] == DBNull.Value ? string.Empty : odr["SPREGUNTA"].ToString ();
                    item.statusId = odr["SESTADO"] == DBNull.Value ? string.Empty : odr["SESTADO"].ToString ();
                    item.regDate = odr["DFECHA_REGISTRO"] == DBNull.Value ? string.Empty : odr["DFECHA_REGISTRO"].ToString ();
                    item.userUpdate = Convert.ToInt32 (odr["NIDUSUARIO_MODIFICA"].ToString ());
                    item.statusDescription = odr["SDESESTADO"] == DBNull.Value ? string.Empty : odr["SDESESTADO"].ToString ();
                    item.originDescription = odr["SDESORIGEN"] == DBNull.Value ? string.Empty : odr["SDESORIGEN"].ToString ();
                    item.transactionType = odr["STIPO"] == DBNull.Value ? string.Empty : odr["STIPO"].ToString ();
                    item.validComment = odr["NIDINDICAOBLCOMEN"] == DBNull.Value ? 0 : Convert.ToInt32(odr["NIDINDICAOBLCOMEN"].ToString ());
                    result.Add (item);

                }
                odr.Close ();
                this.context.Database.CloseConnection ();

            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }

        public UpdateAlertResponseDTO UpdateAlertStatus (int alertId, string alertName, string alertDescription, string alertStatus, int userId, int bussinessDays, string reminderSender, string operType, int idgrupo, string regimenSim, string regimenGen) {

            try {

                UpdateAlertResponseDTO result = new UpdateAlertResponseDTO ();
                OracleParameter p_NIDALERTA = new OracleParameter ("P_NIDALERTA", OracleDbType.Int32, alertId, System.Data.ParameterDirection.Input);
                OracleParameter p_SNOMBRE_ALERTA = new OracleParameter ("P_SNOMBRE_ALERTA", OracleDbType.Varchar2, alertName, System.Data.ParameterDirection.Input);
                OracleParameter p_SDESCRIPCION_ALERTA = new OracleParameter ("P_SDESCRIPCION_ALERTA", OracleDbType.Varchar2, alertDescription, System.Data.ParameterDirection.Input);
                OracleParameter p_SESTADO = new OracleParameter ("P_SESTADO", OracleDbType.Varchar2, alertStatus, System.Data.ParameterDirection.Input);
                OracleParameter p_NIDUSUARIO_MODIFICA = new OracleParameter ("P_NIDUSUARIO_MODIFICA", OracleDbType.Int32, userId, System.Data.ParameterDirection.Input);
                OracleParameter p_NDIASUTILREENVIO = new OracleParameter ("P_NDIASUTILREENVIO", OracleDbType.Int32, bussinessDays, System.Data.ParameterDirection.Input);
                OracleParameter p_SACTIVA_REENVIO = new OracleParameter ("P_SACTIVA_REENVIO", OracleDbType.Varchar2, reminderSender, System.Data.ParameterDirection.Input);
                OracleParameter p_STIPO_OPE = new OracleParameter ("P_STIPO_OPE", OracleDbType.Varchar2, operType, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter ("P_NIDGRUPOSENAL", OracleDbType.Int32, idgrupo, System.Data.ParameterDirection.Input);
                OracleParameter P_NINDICA_REGIMEN_SIMP  = new OracleParameter ("P_NINDICA_REGIMEN_SIMP", OracleDbType.Varchar2, regimenSim, System.Data.ParameterDirection.Input);
                OracleParameter P_NINDICA_REGIMEN_GRAL = new OracleParameter ("P_NINDICA_REGIMEN_GRAL", OracleDbType.Varchar2, regimenGen, System.Data.ParameterDirection.Input);
                OracleParameter P_OUT_NIDALERTA = new OracleParameter ("P_OUT_NIDALERTA", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_NCODE = new OracleParameter ("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_SMESSAGE = new OracleParameter ("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                Console.Write("entro en el repository");
                p_NCODE.Size = 4000;
                p_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { p_NIDALERTA, p_SNOMBRE_ALERTA, p_SDESCRIPCION_ALERTA, p_SESTADO, p_NIDUSUARIO_MODIFICA, p_NDIASUTILREENVIO, p_SACTIVA_REENVIO, p_STIPO_OPE , P_NIDGRUPOSENAL, P_NINDICA_REGIMEN_SIMP, P_NINDICA_REGIMEN_GRAL, P_OUT_NIDALERTA, p_NCODE, p_SMESSAGE };

                var sqlQuery = @"
                    BEGIN 
                    LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_ALERTA(:P_NIDALERTA, :P_SNOMBRE_ALERTA,:P_SDESCRIPCION_ALERTA,:P_SESTADO,:P_NIDUSUARIO_MODIFICA,:P_NDIASUTILREENVIO,:P_SACTIVA_REENVIO,:P_STIPO_OPE, :P_NIDGRUPOSENAL, :P_NINDICA_REGIMEN_SIMP , :P_NINDICA_REGIMEN_GRAL, :P_OUT_NIDALERTA, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (sqlQuery, parameters);
                result.error = Convert.ToInt32 (p_NCODE.Value.ToString ());
                result.message = p_SMESSAGE.Value.ToString ();
                result.alertId = Convert.ToInt32 (P_OUT_NIDALERTA.Value.ToString ());
                
                this.context.Database.CloseConnection ();
                 Console.Write("entro en el repository2");
                return result;

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return null;
            }
        }

        public UpdateQuestionResponseDTO UpdateQuestion (int alertId, int questionId, int originId, string questionName, string questionStatus, int userId, string transactionType, int validComment) {

            try {

                UpdateQuestionResponseDTO result = new UpdateQuestionResponseDTO ();
                OracleParameter p_NIDALERTA = new OracleParameter ("P_NIDALERTA", OracleDbType.Int32, alertId, System.Data.ParameterDirection.Input);
                OracleParameter p_NIDPREGUNTA = new OracleParameter ("P_NIDPREGUNTA", OracleDbType.Int32, questionId, System.Data.ParameterDirection.Input);
                OracleParameter p_NIDORIGEN = new OracleParameter ("P_NIDORIGEN", OracleDbType.Int32, originId, System.Data.ParameterDirection.Input);
                OracleParameter p_SPREGUNTA = new OracleParameter ("P_SPREGUNTA", OracleDbType.Varchar2, questionName, System.Data.ParameterDirection.Input);
                OracleParameter p_SESTADO = new OracleParameter ("P_SESTADO", OracleDbType.Varchar2, questionStatus, System.Data.ParameterDirection.Input);
                OracleParameter p_NIDUSUARIO_MODIFICA = new OracleParameter ("P_NIDUSUARIO_MODIFICA", OracleDbType.Int32, userId, System.Data.ParameterDirection.Input);
                OracleParameter p_STIPO_OPE = new OracleParameter ("P_STIPO_OPE", OracleDbType.Varchar2, transactionType, System.Data.ParameterDirection.Input);
                OracleParameter p_NIDINDICAOBLCOMEN = new OracleParameter ("P_NIDINDICAOBLCOMEN", OracleDbType.Int32, validComment, System.Data.ParameterDirection.Input);
                OracleParameter p_NCODE = new OracleParameter ("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_SMESSAGE = new OracleParameter ("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);

                p_NCODE.Size = 4000;
                p_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { p_NIDALERTA, p_NIDPREGUNTA, p_NIDORIGEN, p_SPREGUNTA, p_SESTADO, p_NIDUSUARIO_MODIFICA, p_STIPO_OPE, p_NIDINDICAOBLCOMEN, p_NCODE, p_SMESSAGE };

                var sqlQuery = @"
                    BEGIN 
                    LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_PREGUNTA_X_ALERTA(:P_NIDALERTA, :P_NIDPREGUNTA,:P_NIDORIGEN,:P_SPREGUNTA,:P_SESTADO,:P_NIDUSUARIO_MODIFICA,:P_STIPO_OPE, :P_NIDINDICAOBLCOMEN, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (sqlQuery, parameters);
                result.error = Convert.ToInt32 (p_NCODE.Value.ToString ());
                result.message = p_SMESSAGE.Value.ToString ();
                this.context.Database.CloseConnection ();
                return result;

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return null;
            }
        }

        public List<AlertListResponseDTO> GetAlertList () {
            List<AlertListResponseDTO> result = new List<AlertListResponseDTO> ();
            try {

                OracleParameter RC1 = new OracleParameter ("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_ALERTA(:RC1);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) RC1.Value).GetDataReader ();
                while (odr.Read ()) {
                    AlertListResponseDTO item = new AlertListResponseDTO ();

                    item.alertId = Convert.ToInt32 (odr["NIDALERTA"].ToString ());
                    item.alertName = odr["SNOMBRE_ALERTA"] == DBNull.Value ? string.Empty : odr["SNOMBRE_ALERTA"].ToString ();
                    item.alertDescription = odr["SDESCRIPCION_ALERTA"] == DBNull.Value ? string.Empty : odr["SDESCRIPCION_ALERTA"].ToString ();
                    item.statusDescription = odr["SDESESTADO"] == DBNull.Value ? string.Empty : odr["SDESESTADO"].ToString ();
                    item.registerDate = odr["DFECHA_REGISTRO"] == DBNull.Value ? string.Empty : odr["DFECHA_REGISTRO"].ToString ();
                    item.userName = odr["NIDUSUARIO_MODIFICA"] == DBNull.Value ? string.Empty : odr["NIDUSUARIO_MODIFICA"].ToString ();
                    item.alertStatus = odr["SESTADO"] == DBNull.Value ? string.Empty : odr["SESTADO"].ToString ();
                    item.bussinessDays = Convert.ToInt32 (odr["NDIASUTILREENVIO"].ToString ());
                    item.reminderSender = odr["SACTIVA_REENVIO"] == DBNull.Value ? string.Empty : odr["SACTIVA_REENVIO"].ToString ();
                    item.userFullName = odr["SNOMUSUARIO"] == DBNull.Value ? string.Empty : odr["SNOMUSUARIO"].ToString ();
                    item.sennalDescripcion = odr["SDESGRUPO_SENAL"] == DBNull.Value ? string.Empty : odr["SDESGRUPO_SENAL"].ToString ();
                    item.regimenSim = odr["NINDICA_REGIMEN_SIMP"] == DBNull.Value ? string.Empty : odr["NINDICA_REGIMEN_SIMP"].ToString ();
                    item.regimenGen = odr["NINDICA_REGIMEN_GRAL"] == DBNull.Value ? string.Empty : odr["NINDICA_REGIMEN_GRAL"].ToString ();
                    result.Add (item);
                }
                odr.Close ();
                this.context.Database.CloseConnection ();

            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }

        public SuspendStatusResponseDTO SuspendFrequencyStatus (int frequencyId, string suspensionId) {

            try {

                SuspendStatusResponseDTO frequencyStatus = new SuspendStatusResponseDTO ();
                OracleParameter p_NIDFRECUENCIA = new OracleParameter ("P_NIDFRECUENCIA", OracleDbType.Int32, frequencyId, System.Data.ParameterDirection.Input);
                OracleParameter p_SINDSUSPENSION = new OracleParameter ("P_SINDSUSPENSION", OracleDbType.Varchar2, suspensionId, System.Data.ParameterDirection.Input);
                OracleParameter p_NCODE = new OracleParameter ("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_SMESSAGE = new OracleParameter ("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);

                p_NCODE.Size = 4000;
                p_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { p_NIDFRECUENCIA, p_SINDSUSPENSION, p_NCODE, p_SMESSAGE };

                var sqlQuery = @"
                    BEGIN 
                    LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_FRECUENCIA_SUSPENSION(:P_NIDFRECUENCIA, :P_SINDSUSPENSION, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (sqlQuery, parameters);
                frequencyStatus.error = Convert.ToInt32 (p_NCODE.Value.ToString ());
                frequencyStatus.message = p_SMESSAGE.Value.ToString ();
                this.context.Database.CloseConnection ();
                return frequencyStatus;

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return null;
            }
        }

        #region Consumo del servicio experian
        public Dictionary<string,dynamic> ExperianInvoker (dynamic param) {
            //experianServiceResponseDTO creditHistory = new experianServiceResponseDTO ();
            Dictionary<string,dynamic> creditHistory = new Dictionary<string,dynamic>();
            try {
                Console.WriteLine("LOS PARAM : "+param);
                executeWSExperia (param);
                double score = GetExperianScore (param.sclient);
                Dictionary<string,dynamic> config = new Dictionary<string,dynamic>();
                //dynamic config;// = new experianServiceConfigResponseDTO ();
                config["sClient"] = param.sclient;
                config["userCode"] = param.userCode;
                config["score"] = score.ToString ();
                creditHistory = GetExperianConfigCreditHistory (config);
                creditHistory["score"] = config["score"];
                this.context.Database.CloseConnection();
                return creditHistory;
            } catch (Exception ex) {
                creditHistory["nFlag"] = -2;
                creditHistory["score"] = ex.Message;
                Console.WriteLine("error en el invoker : "+ex);
                return creditHistory;
            }
            
        }

        public void executeWSExperia (dynamic param) {
            try {
                string arguments = string.Format (@"{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}", "", "", "ToExperia",
                 param.documenType, param.documentId, param.lastName, param.sclient, param.userCode, "3");

                using (var process = new Process { StartInfo = new ProcessStartInfo { FileName = "C:\\XperianService\\PrintPolicyDesktop.exe", Arguments = arguments } }) {
                    process.Start ();
                    process.WaitForExit ();
                }
            } catch (Exception ex) {
                throw ex;
            }
        }

        public double GetExperianScore (dynamic sclient) {
            double experianScore = 0;
            try {

                OracleParameter p_SCLIENT = new OracleParameter ("P_SCLIENT", OracleDbType.Varchar2, sclient, ParameterDirection.Input);
                OracleParameter p_NSCORE = new OracleParameter ("P_NSCORE", OracleDbType.Decimal, experianScore, ParameterDirection.Output);

                p_NSCORE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { p_SCLIENT, p_NSCORE };

                var sqlQuery = @"
                    BEGIN 
                    PKG_PV_COTIZACION.REA_SCORE_EXPERIA(:P_SCLIENT, :P_NSCORE);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (sqlQuery, parameters);
                experianScore = double.Parse (p_NSCORE.Value.ToString ());
                this.context.Database.CloseConnection ();

            } catch (Exception ex) {
                //throw ex;
                Console.WriteLine("error en el get experian score : "+ex);
                return experianScore;
            }
            return experianScore;
        }

        public Dictionary<string,dynamic> GetExperianConfigCreditHistory (Dictionary<string,dynamic> param) {
            //experianServiceResponseDTO config = new experianServiceResponseDTO ();
            Dictionary<string,dynamic> config = new Dictionary<string,dynamic>();
            try {
                OracleParameter p_SCLIENT = new OracleParameter ("P_SCLIENT", OracleDbType.Varchar2, param["sClient"], ParameterDirection.Input);
                OracleParameter p_SCORE_CLIENT = new OracleParameter ("P_SCORE_CLIENT", OracleDbType.Double, double.Parse (param["score"]), ParameterDirection.Input);
                OracleParameter p_NUSERCODE = new OracleParameter ("P_NUSERCODE", OracleDbType.Int32, param["userCode"], ParameterDirection.Input);
                OracleParameter p_RISKTYPE = new OracleParameter ("P_NTYPERISK", OracleDbType.Decimal, ParameterDirection.Output);
                OracleParameter p_SDESCRIPT = new OracleParameter ("P_SDESCRIPT", OracleDbType.Varchar2, ParameterDirection.Output);
                OracleParameter p_FLAG = new OracleParameter ("P_NFLAG_CRE", OracleDbType.Decimal, ParameterDirection.Output);

                p_RISKTYPE.Size = 9000;
                p_SDESCRIPT.Size = 9000;
                p_FLAG.Size = 2000;

                OracleParameter[] parameters = new OracleParameter[] { p_SCLIENT, p_SCORE_CLIENT, p_NUSERCODE, p_RISKTYPE, p_SDESCRIPT, p_FLAG };

                var sqlQuery = @"
                    BEGIN 
                    PKG_PV_COTIZACION.REA_POINTS_CLIENT(:P_SCLIENT, :P_SCORE_CLIENT, :P_NUSERCODE, :P_NTYPERISK, :P_SDESCRIPT, :P_NFLAG_CRE);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (sqlQuery, parameters);
                config["nRiskType"] = int.Parse (p_RISKTYPE.Value.ToString ());
                config["sDescript"] = p_SDESCRIPT.Value.ToString ();
                config["nFlag"] = int.Parse (p_FLAG.Value.ToString ());
                this.context.Database.CloseConnection ();
                return config;

            } catch (Exception ex) {
                //throw ex;
                Console.WriteLine("error en el get credit history : "+ex);
                Dictionary<string,dynamic> respObj = new Dictionary<string,dynamic>();
                respObj["code"] = 2;
                respObj["mensaje"] = ex.ToString();
                respObj["mensajeError"] = ex;
                return respObj;
            }
            
        }

        #endregion

        public ExchangeRateResponseDTO GetExchangeRate () {

            try {
                ExchangeRateResponseDTO eRate = new ExchangeRateResponseDTO ();
                OracleParameter p_TIPOCAMBIO = new OracleParameter ("P_TIPOCAMBIO", OracleDbType.Decimal, System.Data.ParameterDirection.Output);
                //SP_OBT_TIPO_CAMBIO
                p_TIPOCAMBIO.Size = 2000;
                OracleParameter[] parameters = new OracleParameter[] { p_TIPOCAMBIO };

                var sqlQuery = @"
                    BEGIN 
                    PKG_LAFT_REPORTE_SBS_.SP_OBT_TIPO_CAMBIO(:P_TIPOCAMBIO);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (sqlQuery, parameters);
                eRate.exchangeRate = Convert.ToDecimal (p_TIPOCAMBIO.Value.ToString ());
                this.context.Database.CloseConnection ();
                return eRate;

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return null;
            }
        }

        public AmountResponseDTO GetAmount () {

            try {
                AmountResponseDTO result = new AmountResponseDTO ();
                OracleParameter P_MONTO = new OracleParameter ("P_MONTO", OracleDbType.Decimal, System.Data.ParameterDirection.Output);
                OracleParameter P_MONTO_AT_RT = new OracleParameter ("P_MONTO_AT_RT", OracleDbType.Decimal, System.Data.ParameterDirection.Output);
                OracleParameter P_MONTO_UNICA_MULTI = new OracleParameter ("P_MONTO_UNICA_MULTI", OracleDbType.Decimal, System.Data.ParameterDirection.Output);
                OracleParameter P_MONTO_FREC_MULTI = new OracleParameter ("P_MONTO_FREC_MULTI", OracleDbType.Decimal, System.Data.ParameterDirection.Output);
                //SP_OBT_TIPO_CAMBIO
                P_MONTO.Size = 2000;
                P_MONTO_AT_RT.Size = 2000;
                P_MONTO_UNICA_MULTI.Size = 2000;
                P_MONTO_FREC_MULTI.Size = 2000;
                OracleParameter[] parameters = new OracleParameter[] { P_MONTO, P_MONTO_AT_RT, P_MONTO_UNICA_MULTI, P_MONTO_FREC_MULTI };

                var sqlQuery = @"
                    BEGIN 
                    PKG_LAFT_REPORTE_SBS_2.SP_OBT_MONTO(:P_MONTO,:P_MONTO_AT_RT,:P_MONTO_UNICA_MULTI,:P_MONTO_FREC_MULTI);
                    END;
                    ";

                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (sqlQuery, parameters);
                result.amount = Convert.ToDecimal (P_MONTO.Value.ToString ());
                result.amountrtat = Convert.ToDecimal (P_MONTO_AT_RT.Value.ToString ());
                result.amountUniMulti = Convert.ToDecimal (P_MONTO_UNICA_MULTI.Value.ToString ());
                result.amountFrecMulti = Convert.ToDecimal (P_MONTO_FREC_MULTI.Value.ToString ());
                Console.Write ("result  , " + result);
                this.context.Database.CloseConnection ();
                return result;

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return null;
            }
        }

        public SbsReportGenResponseDTO GenerateSbsReport (int operType, decimal exchangeType, int ammount, string startDate, string endDate, string nameReport, string sbsFileType) {

            try {
                SbsReportGenResponseDTO sbsReport = new SbsReportGenResponseDTO ();
                OracleParameter p_OPE = new OracleParameter ("P_OPE", OracleDbType.Int32, operType, System.Data.ParameterDirection.Input);
                OracleParameter p_TC = new OracleParameter ("P_TC", OracleDbType.Decimal, exchangeType, System.Data.ParameterDirection.Input);
                OracleParameter p_MONTO = new OracleParameter ("P_MONTO", OracleDbType.Int32, ammount, System.Data.ParameterDirection.Input);
                OracleParameter p_FECINI = new OracleParameter ("P_FECINI", OracleDbType.Varchar2, startDate, System.Data.ParameterDirection.Input);
                OracleParameter p_FECFIN = new OracleParameter ("P_FECFIN", OracleDbType.Varchar2, endDate, System.Data.ParameterDirection.Input);
                OracleParameter p_EST_REPORTES = new OracleParameter ("P_EST_REPORTES", OracleDbType.Varchar2, nameReport, System.Data.ParameterDirection.Input);
                OracleParameter p_TIPO_ARCHIVO = new OracleParameter ("P_TIPO_ARCHIVO", OracleDbType.Varchar2, sbsFileType, System.Data.ParameterDirection.Input);
                OracleParameter p_SRUTA = new OracleParameter ("P_SRUTA", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                OracleParameter p_ID_REPORTE = new OracleParameter ("P_ID_REPORTE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                OracleParameter p_NCODE = new OracleParameter ("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_SMESSAGE = new OracleParameter ("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                OracleParameter p_C_TABLE = new OracleParameter ("C_TABLE", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                p_SRUTA.Size = 4000;
                p_ID_REPORTE.Size = 4000;
                p_NCODE.Size = 4000;
                p_SMESSAGE.Size = 4000;
                p_C_TABLE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { p_OPE, p_TC, p_MONTO, p_FECINI, p_FECFIN, p_EST_REPORTES, p_TIPO_ARCHIVO, p_SRUTA, p_ID_REPORTE, p_NCODE, p_SMESSAGE, p_C_TABLE };

                var sqlQuery = @"
                    BEGIN 
                    PKG_LAFT_REPORTE_SBS_.GENERAR_REP_SBS(:P_OPE, :P_TC,:P_MONTO,:P_FECINI,:P_FECFIN,:P_EST_REPORTES,:P_TIPO_ARCHIVO,:P_SRUTA,:P_ID_REPORTE, :P_NCODE, :P_SMESSAGE, :C_TABLE);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (sqlQuery, parameters);
                sbsReport.route = p_SRUTA.Value.ToString ();
                sbsReport.message = p_SMESSAGE.Value.ToString ();
                sbsReport.reportSbsId = p_ID_REPORTE.Value.ToString ();
                this.context.Database.CloseConnection ();
                return sbsReport;

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return null;
            }
        }

        public UserDataResponseDTO GetUser (int userId) {
            try {
                UserDataResponseDTO user = new UserDataResponseDTO ();
                OracleParameter p_USUARIO = new OracleParameter ("P_USUARIO", OracleDbType.Int32, userId, System.Data.ParameterDirection.Input);
                OracleParameter p_SEMAIL = new OracleParameter ("P_SEMAIL", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                OracleParameter p_SNAME = new OracleParameter ("P_SNAME", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                OracleParameter p_NCODE = new OracleParameter ("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_MESSAGE = new OracleParameter ("P_MESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);

                p_SEMAIL.Size = 4000;
                p_SNAME.Size = 4000;
                p_NCODE.Size = 4000;
                p_MESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { p_USUARIO, p_SEMAIL, p_SNAME, p_NCODE, p_MESSAGE };

                var sqlQuery = @"
                    BEGIN 
                    PKG_LAFT_USUARIO.SPS_OBT_CORREO_ENVIO(:P_USUARIO, :P_SEMAIL, :P_SNAME, :P_NCODE, :P_MESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection ();
                this.context2.Database.ExecuteSqlCommand (sqlQuery, parameters);
                user.name = p_SNAME.Value.ToString ();
                user.email = p_SEMAIL.Value.ToString ();
                this.context.Database.CloseConnection ();
                return user;

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return null;
            }
        }
        public List<Dictionary<string,dynamic>> GetListReports (SbsReportGenListParametersDTO param) {
            List<Dictionary<string,dynamic>> result = new List<Dictionary<string,dynamic>> ();
            try {

                OracleParameter p_FECEJE_INI = new OracleParameter ("P_FECEJE_INI", OracleDbType.Varchar2, param.startDate, ParameterDirection.Input);
                OracleParameter p_FECEJE_FIN = new OracleParameter ("P_FECEJE_FIN", OracleDbType.Varchar2, param.endDate, ParameterDirection.Input);
                OracleParameter p_SID = new OracleParameter ("P_SID", OracleDbType.Varchar2, param.reportId, ParameterDirection.Input);
                OracleParameter p_TI_BUSQUEDA = new OracleParameter ("P_TI_BUSQUEDA", OracleDbType.Varchar2, param.searchType, ParameterDirection.Input);
                //OracleParameter p_NCODE = new OracleParameter ("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                //OracleParameter p_SMESSAGE = new OracleParameter ("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                OracleParameter C_TABLE = new OracleParameter ("C_TABLE", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                //p_NCODE.Size = 4000;
                //p_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { p_FECEJE_INI, p_FECEJE_FIN, p_SID, p_TI_BUSQUEDA, C_TABLE };
                var query = @"
                    BEGIN
                        INSUDB.PKG_LAFT_REPORTE_SBS_.SP_MONITOREO_REP_SBS(:P_FECEJE_INI,:P_FECEJE_FIN, :P_SID, :P_TI_BUSQUEDA, :C_TABLE);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) C_TABLE.Value).GetDataReader ();
                while (odr.Read ()) {
                    Dictionary<string,dynamic> item = new Dictionary<string,dynamic> ();

                    item["id"] = odr["SID"] == DBNull.Value ? string.Empty : odr["SID"].ToString ();
                    item["fechaInicioReporte"] = odr["DINIREP"] == DBNull.Value ? string.Empty : odr["DINIREP"].ToString ();
                    item["fechaFinReporte"] = odr["DFINREP"] == DBNull.Value ? string.Empty : odr["DFINREP"].ToString ();
                    item["tipoCambio"] = odr["NTIPCAMBIO"] == DBNull.Value ? 0 : Convert.ToDecimal (odr["NTIPCAMBIO"].ToString ());
                    item["tipoOperacion"] = odr["STIPOPE"] == DBNull.Value ? string.Empty : odr["STIPOPE"].ToString ();
                    item["origen"] = odr["SORIGEN"] == DBNull.Value ? string.Empty : odr["SORIGEN"].ToString ();
                    item["estadoReporte"] = odr["SDESCRIPTION"] == DBNull.Value ? string.Empty : odr["SDESCRIPTION"].ToString ();
                    item["fechaProcesoEjecucion"] = odr["FECINIEJEC"] == DBNull.Value ? string.Empty : odr["FECINIEJEC"].ToString ();
                    item["fechaProcesoEjecucion"] = odr["FECFINEJEC"] == DBNull.Value ? string.Empty : odr["FECFINEJEC"].ToString ();
                    item["tipoDeArchivo"] = odr["STIPOARCH"] == DBNull.Value ? string.Empty : odr["STIPOARCH"].ToString ();
                    item["estado"] = odr["SESTADO"] == DBNull.Value ? string.Empty : odr["SESTADO"].ToString ();
                    result.Add (item);
                }
                odr.Close ();
                this.context.Database.CloseConnection ();

            } catch (Exception ex) {
                Console.WriteLine("EL ERROR EN LA CONSULTA DE REPORTES : "+ex);
                throw ex;
            }
            return result;
        }
        public List<AlertMonitoringResponseDTO> GetListAlerts (AlertMonitoringParametersDTO param) {
            List<AlertMonitoringResponseDTO> result = new List<AlertMonitoringResponseDTO> ();
            try {

                OracleParameter p_FECEJE_INI = new OracleParameter ("P_FECEJE_INI", OracleDbType.Varchar2, param.startDate, ParameterDirection.Input);
                OracleParameter p_FECEJE_FIN = new OracleParameter ("P_FECEJE_FIN", OracleDbType.Varchar2, param.endDate, ParameterDirection.Input);
                OracleParameter p_SID = new OracleParameter ("P_SID", OracleDbType.Varchar2, param.reportId, ParameterDirection.Input);
                OracleParameter p_TI_BUSQUEDA = new OracleParameter ("P_TI_BUSQUEDA", OracleDbType.Varchar2, param.searchType, ParameterDirection.Input);
                OracleParameter p_NCODE = new OracleParameter ("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_SMESSAGE = new OracleParameter ("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                OracleParameter RC1 = new OracleParameter ("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                p_NCODE.Size = 4000;
                p_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { p_FECEJE_INI, p_FECEJE_FIN, p_SID, p_TI_BUSQUEDA, p_NCODE, p_SMESSAGE, RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_MONI_GESTION_ALERTA(:P_FECEJE_INI,:P_FECEJE_FIN, :P_SID, :P_TI_BUSQUEDA,:P_NCODE,:P_SMESSAGE, :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) RC1.Value).GetDataReader ();
                while (odr.Read ()) {
                    AlertMonitoringResponseDTO item = new AlertMonitoringResponseDTO ();
                    
                    item.estadoAlerta = odr["SDESCRIPTION"] == DBNull.Value ? string.Empty : odr["SDESCRIPTION"].ToString ();
                    item.id = odr["SID"] == DBNull.Value ? string.Empty : odr["SID"].ToString ();
                    item.fechaInicioPeriodo = odr["DINIPROC"] == DBNull.Value ? string.Empty : odr["DINIPROC"].ToString ();
                    item.fechaFinPeriodo = odr["DFINPROC"] == DBNull.Value ? string.Empty : odr["DFINPROC"].ToString ();
                    item.fechaInicioEjecucion = odr["DRANGOINI"] == DBNull.Value ? string.Empty : odr["DRANGOINI"].ToString ();
                    item.fechaFinEjecucion = odr["DRANGOFIN"] == DBNull.Value ? string.Empty : odr["DRANGOFIN"].ToString ();
                    item.resultado = odr["SMENSAJE"] == DBNull.Value ? string.Empty : odr["SMENSAJE"].ToString ();
                    item.descripcion = odr["SDESCRIPTION"] == DBNull.Value ? string.Empty : odr["SDESCRIPTION"].ToString ();
                    result.Add (item);
                }
                odr.Close ();
                this.context.Database.CloseConnection ();

            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }

        public List<SbsReportFileResponseDTO> GetReport (string id, int tipo_archivo) {

            try {
                //var route = id;
                List<SbsReportFileResponseDTO> result = new List<SbsReportFileResponseDTO> ();
                OracleParameter p_ID = new OracleParameter ("P_ID", OracleDbType.Varchar2, id, System.Data.ParameterDirection.Input);
                OracleParameter p_RUTA = new OracleParameter ("P_RUTA", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);

                p_RUTA.Size = 4000;
                OracleParameter[] parameters = new OracleParameter[] { p_ID, p_RUTA };

                var sqlQuery = @"
                    BEGIN 
                    PKG_LAFT_REPORTE_SBS_.SP_OBT_RUTA(:P_ID, :P_RUTA);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (sqlQuery, parameters);
                this.context.Database.CloseConnection ();
                //var ruta  = "";
                var modalidadUnica = "01";
                var modalidadMultiple = "02";
                if (tipo_archivo == 2) {
                    var rutaUnicaTxt = p_RUTA.Value.ToString () + modalidadUnica + id + ".txt";
                    byte[] AsBytesUnicaTxt = File.ReadAllBytes (rutaUnicaTxt);
                    var fileUnicaTxt = Convert.ToBase64String (AsBytesUnicaTxt);
                    SbsReportFileResponseDTO rt1txt = new SbsReportFileResponseDTO ();
                    rt1txt.report = fileUnicaTxt;
                    rt1txt.modalidad = modalidadUnica;

                    //

                    var rutaMultipleTxt = p_RUTA.Value.ToString () + modalidadMultiple + id + ".txt";
                    byte[] AsBytesMultipleTxt = File.ReadAllBytes (rutaMultipleTxt);
                    var fileMultipleTxt = Convert.ToBase64String (AsBytesMultipleTxt);
                    SbsReportFileResponseDTO rt2txt = new SbsReportFileResponseDTO ();
                    rt2txt.report = fileMultipleTxt;
                    rt2txt.modalidad = modalidadMultiple;
                    result.Add (rt1txt);
                    result.Add (rt2txt);
                    return result;
                }

                var unica = "";
                var multiple = "";

                if (tipo_archivo == 1) {
                    unica = "_" + modalidadUnica + ".xls";
                    multiple = "_" + modalidadMultiple + ".xls";

                } else if (tipo_archivo == 2) {
                    unica = "_" + modalidadUnica + ".txt";
                    multiple = "_" + modalidadMultiple + ".txt";
                } else {
                    unica = "_" + modalidadUnica + ".xls";
                    multiple = "_" + modalidadMultiple + ".xls";
                }
               
                var rutaUnica = p_RUTA.Value.ToString () + id + unica;
                var rutaMultiple = p_RUTA.Value.ToString () + id + multiple;

                Console.Write ("rutaUnica   :  " + rutaUnica, Environment.NewLine);
                Console.Write ("/n rutaMultiple   :  " + rutaMultiple, Environment.NewLine);
                //\\172.23.2.145\CONCILIACIONES\REPORTE_SBS_2020102712424666_01.xls
                Console.Write ("paso 1  :  ", Environment.NewLine);
                byte[] AsBytesUnica = File.ReadAllBytes (rutaUnica);
                var fileUnica = Convert.ToBase64String (AsBytesUnica);
                SbsReportFileResponseDTO rt1 = new SbsReportFileResponseDTO ();
                Console.Write ("paso 2  :  ", Environment.NewLine);
                rt1.report = fileUnica;
                rt1.modalidad = modalidadUnica;

                ////

                byte[] AsBytesMultiple = File.ReadAllBytes (rutaMultiple);
                var fileMultiple = Convert.ToBase64String (AsBytesMultiple);
                SbsReportFileResponseDTO rt2 = new SbsReportFileResponseDTO ();

                rt2.report = fileMultiple;
                rt2.modalidad = modalidadMultiple;

                result.Add (rt1);
                result.Add (rt2);
                this.context.Database.CloseConnection();
                //Console.Write ("result   :  " + result);

                return result;

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return null;
            }
        }

        public List<ReportTypeResponseDTO> GetReportTypes () {
            List<ReportTypeResponseDTO> result = new List<ReportTypeResponseDTO> ();
            try {

                OracleParameter RC1 = new OracleParameter ("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { RC1 };
                var query = @"
                    BEGIN
                        PKG_LAFT_REPORTE_SBS_.SP_OBT_TIPOS_REPORTE(:RC1);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) RC1.Value).GetDataReader ();
                while (odr.Read ()) {
                    ReportTypeResponseDTO item = new ReportTypeResponseDTO ();

                    item.reportTypeId = Convert.ToInt32 (odr["NIDREPORT"].ToString ());
                    item.reportTypeName = odr["SNAME"] == DBNull.Value ? string.Empty : odr["SNAME"].ToString ();
                    result.Add (item);

                }
                odr.Close ();
                this.context.Database.CloseConnection ();

            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }

        public List<GafiListResponseDTO> GetGafiList () {
            List<GafiListResponseDTO> result = new List<GafiListResponseDTO> ();
            try {

                OracleParameter RC1 = new OracleParameter ("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_GAFI(:RC1);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) RC1.Value).GetDataReader ();
                while (odr.Read ()) {
                    GafiListResponseDTO item = new GafiListResponseDTO ();
                    item.gafiId = Convert.ToInt32 (odr["NIDGAFI"].ToString ());
                    item.countryGafiName = odr["SNOMBRE_PAIS_GAFI"] == DBNull.Value ? string.Empty : odr["SNOMBRE_PAIS_GAFI"].ToString ();
                    item.userName = odr["NOMBRECOMPLETO"] == DBNull.Value ? string.Empty : odr["NOMBRECOMPLETO"].ToString ();
                    item.regDate = odr["DFECHA_REGISTRO"] == DBNull.Value ? string.Empty : odr["DFECHA_REGISTRO"].ToString ();
                    result.Add (item);
                }
                odr.Close ();
                this.context.Database.CloseConnection ();

            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }

        public UpdateCountryResponseDTO UpdateCountry (int gafiId, string countryGafiName, string state, int regUser, string operType) {

            try {
                UpdateCountryResponseDTO countryUpdate = new UpdateCountryResponseDTO ();

                OracleParameter p_NIDGAFI = new OracleParameter ("P_NIDGAFI", OracleDbType.Int32, gafiId, System.Data.ParameterDirection.Input);
                OracleParameter p_SNOMBRE_PAIS_GAFI = new OracleParameter ("P_SNOMBRE_PAIS_GAFI", OracleDbType.Varchar2, countryGafiName.ToUpper (), System.Data.ParameterDirection.Input);
                OracleParameter p_SESTADO = new OracleParameter ("P_SESTADO", OracleDbType.Varchar2, state, System.Data.ParameterDirection.Input);
                OracleParameter p_NIDUSUARIO_REGISTRO = new OracleParameter ("P_NIDUSUARIO_REGISTRO", OracleDbType.Int32, regUser, System.Data.ParameterDirection.Input);
                OracleParameter p_STIPO_OPE = new OracleParameter ("P_STIPO_OPE", OracleDbType.Varchar2, operType, System.Data.ParameterDirection.Input);
                OracleParameter p_NCODE = new OracleParameter ("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_SMESSAGE = new OracleParameter ("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);

                p_NCODE.Size = 4000;
                p_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { p_NIDGAFI, p_SNOMBRE_PAIS_GAFI, p_SESTADO, p_NIDUSUARIO_REGISTRO, p_STIPO_OPE, p_NCODE, p_SMESSAGE };

                var sqlQuery = @"
                    BEGIN 
                    LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_PAIS_GAFI(:P_NIDGAFI, :P_SNOMBRE_PAIS_GAFI, :P_SESTADO, :P_NIDUSUARIO_REGISTRO, :P_STIPO_OPE, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (sqlQuery, parameters);
                countryUpdate.error = Convert.ToInt32 (p_NCODE.Value.ToString ());
                countryUpdate.message = p_SMESSAGE.Value.ToString ();
                this.context.Database.CloseConnection ();
                return countryUpdate;

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return null;
            }
        }
        public List<FrequencyResponseDTO> GetFrequency () {
            List<FrequencyResponseDTO> result = new List<FrequencyResponseDTO> ();
            try {

                OracleParameter RC1 = new OracleParameter ("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_TIPO_FRECUENCIA(:RC1);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) RC1.Value).GetDataReader ();
                while (odr.Read ()) {
                    FrequencyResponseDTO item = new FrequencyResponseDTO ();
                    item.frequencyType = Convert.ToInt32 (odr["NTIPOFRECUENCIA"].ToString ());
                    item.frequencyName = odr["SDESFRECUENCIA"] == DBNull.Value ? string.Empty : odr["SDESFRECUENCIA"].ToString ();
                    result.Add (item);

                }
                odr.Close ();
                this.context.Database.CloseConnection ();

            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }

        public List<FrequencyListResponseDTO> GetFrequencyList () {
            List<FrequencyListResponseDTO> result = new List<FrequencyListResponseDTO> ();
            try {

                OracleParameter RC1 = new OracleParameter ("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_FRECUENCIA(:RC1);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) RC1.Value).GetDataReader ();
                while (odr.Read ()) {
                    FrequencyListResponseDTO item = new FrequencyListResponseDTO ();
                    item.frequencyId = Convert.ToInt32 (odr["NIDFRECUENCIA"].ToString ());
                    item.startDate = odr["DFEEJECUTAPROCINI"] == DBNull.Value ? string.Empty : odr["DFEEJECUTAPROCINI"].ToString ();
                    item.endDate = odr["DFEEJECUTAPROCFIN"] == DBNull.Value ? string.Empty : odr["DFEEJECUTAPROCFIN"].ToString ();
                    item.status = odr["SESTADO"] == DBNull.Value ? string.Empty : odr["SESTADO"].ToString ();
                    item.frequencyType = Convert.ToInt32 (odr["NTIPOFRECUENCIA"].ToString ());
                    item.frequencyName = odr["SDESFRECUENCIA"] == DBNull.Value ? string.Empty : odr["SDESFRECUENCIA"].ToString ();
                    item.suspendStatus = odr["SESTADOSUSP"] == DBNull.Value ? string.Empty : odr["SESTADOSUSP"].ToString ();
                    item.regDate = odr["DFECHA_REGISTRO"] == DBNull.Value ? string.Empty : odr["DFECHA_REGISTRO"].ToString ();
                    item.user = odr["USUARIO"] == DBNull.Value ? string.Empty : odr["USUARIO"].ToString ();
                    item.NPERIODO_PROCESO = odr["NPERIODO_PROCESO"] == DBNull.Value ? string.Empty : odr["NPERIODO_PROCESO"].ToString ();
                    result.Add (item);
                }

                odr.Close ();
                this.context.Database.CloseConnection ();
            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }
        public List<FrequencyActiveResponseDTO> GetFrequencyActive () {
            List<FrequencyActiveResponseDTO> result = new List<FrequencyActiveResponseDTO> ();
            try {

                OracleParameter RC1 = new OracleParameter ("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_FRECUENCIA_ACTIVO(:RC1);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) RC1.Value).GetDataReader ();
                while (odr.Read ()) {
                    FrequencyActiveResponseDTO item = new FrequencyActiveResponseDTO ();
                    item.frequencyId = Convert.ToInt32 (odr["NIDFRECUENCIA"].ToString ());
                    item.startDate = odr["DFECINI"] == DBNull.Value ? string.Empty : odr["DFECINI"].ToString ();
                    item.endDate = odr["DFECFIN"] == DBNull.Value ? string.Empty : odr["DFECFIN"].ToString ();
                    item.status = odr["SESTADO"] == DBNull.Value ? string.Empty : odr["SESTADO"].ToString ();
                    item.frequencyType = Convert.ToInt32 (odr["NTIPOFRECUENCIA"].ToString ());
                    item.frequencyName = odr["SDESFRECUENCIA"] == DBNull.Value ? string.Empty : odr["SDESFRECUENCIA"].ToString ();
                    item.suspendStatus = odr["SINDSUSPENSION"] == DBNull.Value ? string.Empty : odr["SINDSUSPENSION"].ToString ();
                    result.Add (item);

                }
                odr.Close ();
                this.context.Database.CloseConnection ();

            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }

        public UpdateFrequencyResponseDTO UpdateFrequency (int frequencyId, int frequencyType, string startDate, int userId) {

            try {
                UpdateFrequencyResponseDTO frequencyUpdate = new UpdateFrequencyResponseDTO ();

                OracleParameter p_NIDFRECUENCIA = new OracleParameter ("P_NIDFRECUENCIA", OracleDbType.Int32, frequencyId, System.Data.ParameterDirection.Input);
                OracleParameter p_NTIPOFRECUENCIA = new OracleParameter ("P_NTIPOFRECUENCIA", OracleDbType.Int32, frequencyType, System.Data.ParameterDirection.Input);
                OracleParameter p_DFECINI = new OracleParameter ("P_DFECINI", OracleDbType.Varchar2, startDate, System.Data.ParameterDirection.Input);
                OracleParameter p_NIDUSUARIO_MODIFICA = new OracleParameter ("P_NIDUSUARIO_MODIFICA", OracleDbType.Int32, userId, System.Data.ParameterDirection.Input);
                OracleParameter p_NCODE = new OracleParameter ("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_SMESSAGE = new OracleParameter ("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);

                p_NCODE.Size = 4000;
                p_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { p_NIDFRECUENCIA, p_NTIPOFRECUENCIA, p_DFECINI, p_NIDUSUARIO_MODIFICA, p_NCODE, p_SMESSAGE };

                var sqlQuery = @"
                    BEGIN 
                    LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_FRECUENCIA_PROCESO(:P_NIDFRECUENCIA, :P_NTIPOFRECUENCIA, :P_DFECINI, :P_NIDUSUARIO_MODIFICA, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (sqlQuery, parameters);
                frequencyUpdate.error = Convert.ToInt32 (p_NCODE.Value.ToString ());
                frequencyUpdate.message = p_SMESSAGE.Value.ToString ();
                this.context.Database.CloseConnection ();
                return frequencyUpdate;

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return null;
            }
        }

        public List<ProfileListResponseDTO> GetProfileList () {
            List<ProfileListResponseDTO> result = new List<ProfileListResponseDTO> ();
            try {

                OracleParameter RC1 = new OracleParameter ("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_PERFIL_ALERTA(:RC1);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) RC1.Value).GetDataReader ();
                while (odr.Read ()) {
                    ProfileListResponseDTO item = new ProfileListResponseDTO ();
                    item.profileId = odr["NIDPROFILE"] == DBNull.Value ? string.Empty : odr["NIDPROFILE"].ToString ();
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


         public List<ProfileListResponseDTO> GetPerfilList () {
            List<ProfileListResponseDTO> result = new List<ProfileListResponseDTO> ();
            try {

                OracleParameter RC1 = new OracleParameter ("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_PERFIL(:RC1);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) RC1.Value).GetDataReader ();
                while (odr.Read ()) {
                    ProfileListResponseDTO item = new ProfileListResponseDTO ();
                    item.profileId = odr["NIDPROFILE"] == DBNull.Value ? string.Empty : odr["NIDPROFILE"].ToString ();
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


        public List<ProfileListResponseDTO> GetUserByProfileList () {
            List<ProfileListResponseDTO> result = new List<ProfileListResponseDTO> ();
            try {

                OracleParameter RC1 = new OracleParameter ("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_PERFIL_X_USUARIO(:RC1);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) RC1.Value).GetDataReader ();
                while (odr.Read ()) {
                    ProfileListResponseDTO item = new ProfileListResponseDTO ();
                    item.profileId = odr["NIDPROFILE"] == DBNull.Value ? string.Empty : odr["NIDPROFILE"].ToString ();
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

        public List<RegimeResponseDTO> GetRegimeList (RegimeParametersDTO param) {
            List<RegimeResponseDTO> result = new List<RegimeResponseDTO> ();
            try {

                OracleParameter p_ID_ROL = new OracleParameter ("P_ID_ROL", OracleDbType.Int32, param.profileId, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter ("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { p_ID_ROL, RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_REGIMEN_PERFIL(:P_ID_ROL, :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) RC1.Value).GetDataReader ();
                while (odr.Read ()) {
                    RegimeResponseDTO item = new RegimeResponseDTO ();

                    item.regimeId = Convert.ToInt32 (odr["NIDREGIMEN"].ToString ());
                    item.regimeName = odr["SDESREGIMEN"] == DBNull.Value ? string.Empty : odr["SDESREGIMEN"].ToString ();
                    result.Add (item);

                }
                odr.Close ();
                this.context.Database.CloseConnection ();

            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }

        public List<GrupoPerfilListResponseDTO> GetGrupoxPerfilList (RegimeParametersDTO param) {
            List<GrupoPerfilListResponseDTO> result = new List<GrupoPerfilListResponseDTO> ();
            try {

                OracleParameter P_NIDGRUPOSENAL  = new OracleParameter ("P_NIDGRUPOSENAL ", OracleDbType.Int32, param.profileId, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter ("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { P_NIDGRUPOSENAL , RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_PERFIL_X_GRUPO_SENAL(:P_NIDGRUPOSENAL , :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) RC1.Value).GetDataReader ();
                while (odr.Read ()) {
                    GrupoPerfilListResponseDTO item = new GrupoPerfilListResponseDTO ();

                    item.idperfil = Convert.ToInt32 (odr["NIDPROFILE"].ToString ());
                    item.perfilname = odr["SNAME"] == DBNull.Value ? string.Empty : odr["SNAME"].ToString ();
                    result.Add (item);

                }
                odr.Close ();
                this.context.Database.CloseConnection ();

            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }

        public List<Dictionary<string,dynamic>> GetAlertByProfileList (dynamic param) {
            //List<AlertByProfileResponseDTO> result = new List<AlertByProfileResponseDTO> ();
            List<Dictionary<string,dynamic>> result = new List<Dictionary<string,dynamic>>();
            try {

                OracleParameter P_ID_ROL = new OracleParameter ("P_ID_ROL", OracleDbType.Int32, param.profileId, ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL  = new OracleParameter ("P_NIDGRUPOSENAL", OracleDbType.Int32, param.grupoid, ParameterDirection.Input);
                OracleParameter P_NIDREGIMEN = new OracleParameter ("P_NIDREGIMEN", OracleDbType.Int32, param.regimeId, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter ("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { P_ID_ROL,P_NIDGRUPOSENAL, P_NIDREGIMEN, RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_ALERTA_X_PERFIL(:P_ID_ROL,:P_NIDGRUPOSENAL ,:P_NIDREGIMEN, :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) RC1.Value).GetDataReader ();
                while (odr.Read ()) {
                    Dictionary<string,dynamic> item = new Dictionary<string,dynamic> ();

                    item["alertId"] = Convert.ToInt32 (odr["NIDALERTA"].ToString ());
                    item["alertName"] = odr["SNOMBRE_ALERTA"] == DBNull.Value ? string.Empty : odr["SNOMBRE_ALERTA"].ToString ();
                    item["alertDescription"] = odr["SDESCRIPCION_ALERTA"] == DBNull.Value ? string.Empty : odr["SDESCRIPCION_ALERTA"].ToString ();
                    item["profileId"] = odr["NIDPROFILE"] == DBNull.Value ? 0 : Convert.ToInt32 (odr["NIDPROFILE"].ToString ());
                    item["profileName"] = odr["SNAME"] == DBNull.Value ? string.Empty : odr["SNAME"].ToString ();
                    item["alertStatus"] = odr["SESTADO"] == DBNull.Value ? string.Empty : odr["SESTADO"].ToString ();
                    item["regimeId"] = odr["NIDREGIMEN"].ToString ();//Convert.ToInt32 ();
                    item["regimeName"] = odr["SDESREGIMEN"] == DBNull.Value ? string.Empty : odr["SDESREGIMEN"].ToString ();
                    result.Add (item);

                }
                odr.Close ();
                this.context.Database.CloseConnection ();

            } catch (Exception ex) {
                Dictionary<string,dynamic> item = new Dictionary<string,dynamic> ();
                item["code"] = 3;
                item["messageError"] = ex.Message.ToString();
                item["messageErrorDetalle"] = ex;
                result.Add (item);
                //throw ex;
            }
            return result;
        }

        public UpdateAlertByProfileResponseDTO UpdateAlertByProfile (int profileId, int regimeId, int alertId, string alertStatus) {

            try {
                UpdateAlertByProfileResponseDTO alertByProfileUpdate = new UpdateAlertByProfileResponseDTO ();
                OracleParameter p_ID_ROL = new OracleParameter ("P_ID_ROL", OracleDbType.Int32, profileId, System.Data.ParameterDirection.Input);
                OracleParameter p_NIDREGIMEN = new OracleParameter ("P_NIDREGIMEN", OracleDbType.Int32, regimeId, System.Data.ParameterDirection.Input);
                OracleParameter p_NIDALERTA = new OracleParameter ("P_NIDALERTA", OracleDbType.Int32, alertId, System.Data.ParameterDirection.Input);
                OracleParameter p_SESTADO = new OracleParameter ("P_SESTADO", OracleDbType.Varchar2, alertStatus, System.Data.ParameterDirection.Input);
                OracleParameter p_NCODE = new OracleParameter ("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_SMESSAGE = new OracleParameter ("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);

                p_NCODE.Size = 4000;
                p_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { p_ID_ROL, p_NIDREGIMEN, p_NIDALERTA, p_SESTADO, p_NCODE, p_SMESSAGE };

                var sqlQuery = @"
                    BEGIN 
                    LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_ALERTA_X_PERFIL(:P_ID_ROL,:P_NIDREGIMEN, :P_NIDALERTA, :P_SESTADO, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (sqlQuery, parameters);
                alertByProfileUpdate.error = Convert.ToInt32 (p_NCODE.Value.ToString ());
                alertByProfileUpdate.message = p_SMESSAGE.Value.ToString ();
                this.context.Database.CloseConnection ();
                return alertByProfileUpdate;

            } catch (Exception ex) {
                 this.context.Database.CloseConnection ();
                Console.WriteLine("Error :" + ex);
                Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                item["menesajeError"] = ex.Message.ToString();
                item["mensajeErrorDEtalle"] = ex;
                Utils.ExceptionManager.resolve (ex);
                return null;
            }
        }

        public List<UsersByProfileResponseDTO> GetUsersByProfile (UsersByProfileParametersDTO param) {
            List<UsersByProfileResponseDTO> result = new List<UsersByProfileResponseDTO> ();
            try {

                OracleParameter p_ID_PROFILE = new OracleParameter ("P_ID_PERFIL", OracleDbType.Int32, param.profileId, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter ("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { p_ID_PROFILE, RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_USUARIOS_X_PERFIL(:P_ID_PERFIL, :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) RC1.Value).GetDataReader ();
                while (odr.Read ()) {
                    UsersByProfileResponseDTO item = new UsersByProfileResponseDTO ();

                    //item.idUsuario = Convert.ToInt32 (odr["ID_USUARIO"].ToString ());
                    item.nombreCompleto = odr["NOMBRECOMPLETO"] == DBNull.Value ? string.Empty : odr["NOMBRECOMPLETO"].ToString ();
                    item.correo = odr["SEMAIL"] == DBNull.Value ? string.Empty : odr["SEMAIL"].ToString ();
                    item.cargo = odr["SCARGO"] == DBNull.Value ? string.Empty : odr["SCARGO"].ToString ();
                    item.perfil = odr["SNAME"] == DBNull.Value ? string.Empty : odr["SNAME"].ToString ();
                    item.estUsuario = odr["ESTADO"] == DBNull.Value ? string.Empty : odr["ESTADO"].ToString();
                    result.Add (item);

                }
                odr.Close ();
                this.context.Database.CloseConnection ();

            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }
    
        public Dictionary<string, dynamic> GetCursorSiniestros (dynamic param, int numIndicador,List<SbsReportEnity> result) {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            //List<Dictionary<string, dynamic>> result = new List<Dictionary<string, dynamic>> ();
            //List<SbsReportEnity> result = new List<SbsReportEnity> ();
            try {

                OracleParameter P_OPE = new OracleParameter ("P_OPE", OracleDbType.Int32, param.OPE, ParameterDirection.Input);
                OracleParameter P_TC = new OracleParameter ("P_TC", OracleDbType.Int32, param.TC, ParameterDirection.Input);
                OracleParameter P_MONTO = new OracleParameter ("P_MONTO", OracleDbType.Int32, param.MONTO, ParameterDirection.Input);
                OracleParameter P_MONTO_UNICO = new OracleParameter ("P_MONTO_UNICO", OracleDbType.Int32, param.MONTOATRT, ParameterDirection.Input);
                OracleParameter P_FECINI = new OracleParameter ("P_FECINI", OracleDbType.Varchar2, param.FECINI, ParameterDirection.Input);
                OracleParameter P_FECFIN = new OracleParameter ("P_FECFIN", OracleDbType.Varchar2, param.FECFIN, ParameterDirection.Input);
                OracleParameter C_TABLE = new OracleParameter ("C_TABLE", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters;
                var query = "";
                if(param["busqueda"] == "SIN"){
                    parameters = new OracleParameter[] { P_OPE, P_TC, P_MONTO, P_FECINI, P_FECFIN, C_TABLE };
                    query = @"
                    BEGIN
                        INSUDB.PKG_LAFT_REPORTE_SBS_2.SINIESTROS_MAYORES(:P_OPE, :P_TC, :P_MONTO, :P_FECINI, :P_FECFIN, :C_TABLE);
                    END;
                    ";
                }else if(param["busqueda"] == "VIDA"){
                    parameters = new OracleParameter[] { P_OPE, P_TC, P_MONTO, P_FECINI, P_FECFIN, C_TABLE };
                    query = @"
                    BEGIN
                        INSUDB.PKG_LAFT_REPORTE_SBS_2.NC_VIDA_MAYORES_2(:P_OPE, :P_TC, :P_MONTO, :P_FECINI, :P_FECFIN, :C_TABLE);
                    END;
                    ";
                }else if(param["busqueda"] == "RT"){
                    parameters = new OracleParameter[] { P_FECINI, P_FECFIN, P_MONTO_UNICO, P_TC, C_TABLE };
                    query = @"
                    BEGIN
                        INSUDB.PKG_LAFT_REPORTE_SBS_2.RENTAS_MAYORES(:P_FECINI, :P_FECFIN, :P_MONTO_UNICO, :P_TC, :C_TABLE);
                    END;
                    ";
                }else{
                    this.context.Database.CloseConnection ();
                    return null;
                }


                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) C_TABLE.Value).GetDataReader ();
                while (odr.Read ()) {
                    SbsReportEnity sbsEnti = new SbsReportEnity();
                    sbsEnti.FILA = odr["FILA"].ToString ();
                    sbsEnti.OFICINA = odr["OFICINA"].ToString ();
                    sbsEnti.OPERACION = odr["OPERACION"].ToString ();
                    sbsEnti.INTERNO = odr["INTERNO"].ToString ();
                    sbsEnti.MODALIDAD = odr["MODALIDAD"].ToString ();
                    sbsEnti.OPE_UBIGEO = odr["OPE_UBIGEO"].ToString ();
                    sbsEnti.OPE_FECHA = odr["OPE_FECHA"].ToString ();
                    sbsEnti.OPE_HORA = odr["OPE_HORA"].ToString ();
                    sbsEnti.EJE_RELACION = odr["EJE_RELACION"].ToString ();
                    sbsEnti.EJE_CONDICION = odr["EJE_CONDICION"].ToString ();
                    sbsEnti.EJE_TIPPER = odr["EJE_TIPPER"].ToString ();
                    sbsEnti.EJE_TIPDOC = odr["EJE_TIPDOC"].ToString ();
                    sbsEnti.EJE_NUMDOC = odr["EJE_NUMDOC"].ToString ();
                    sbsEnti.EJE_NUMRUC = odr["EJE_NUMRUC"].ToString ();
                    sbsEnti.EJE_APEPAT = odr["EJE_APEPAT"].ToString ();
                    sbsEnti.EJE_APEMAT = odr["EJE_APEMAT"].ToString ();
                    sbsEnti.EJE_NOMBRES = odr["EJE_NOMBRES"].ToString ();
                    sbsEnti.EJE_OCUPACION = odr["EJE_OCUPACION"].ToString ();
                    sbsEnti.EJE_PAIS = odr["EJE_PAIS"].ToString ();
                    sbsEnti.EJE_CARGO = odr["EJE_CARGO"].ToString ();
                    sbsEnti.EJE_PEP = odr["EJE_PEP"].ToString ();
                    sbsEnti.EJE_DOMICILIO = odr["EJE_DOMICILIO"].ToString ();
                    sbsEnti.EJE_DEPART = odr["EJE_DEPART"].ToString ();
                    sbsEnti.EJE_PROV = odr["EJE_PROV"].ToString ();
                    sbsEnti.EJE_DIST = odr["EJE_DIST"].ToString ();
                    sbsEnti.EJE_TELEFONO = odr["EJE_TELEFONO"].ToString ();
                    sbsEnti.ORD_RELACION = odr["ORD_RELACION"].ToString ();
                    sbsEnti.ORD_CONDICION = odr["ORD_CONDICION"].ToString ();
                    sbsEnti.ORD_TIPPER = odr["ORD_TIPPER"].ToString ();
                    sbsEnti.ORD_TIPDOC = odr["ORD_TIPDOC"].ToString ();
                    sbsEnti.ORD_NUMDOC = odr["ORD_NUMDOC"].ToString ();
                    sbsEnti.ORD_NUMRUC = odr["ORD_NUMRUC"].ToString ();
                    sbsEnti.ORD_APEPAT = odr["ORD_APEPAT"].ToString ();
                    sbsEnti.ORD_APEMAT = odr["ORD_APEMAT"].ToString ();
                    sbsEnti.ORD_NOMBRES = odr["ORD_NOMBRES"].ToString ();
                    sbsEnti.ORD_OCUPACION = odr["ORD_OCUPACION"].ToString ();
                    sbsEnti.ORD_PAIS = odr["ORD_PAIS"].ToString ();
                    sbsEnti.ORD_CARGO = odr["ORD_CARGO"].ToString ();
                    sbsEnti.ORD_PEP = odr["ORD_PEP"].ToString ();
                    sbsEnti.ORD_DOMICILIO = odr["ORD_DOMICILIO"].ToString ();
                    sbsEnti.ORD_DEPART = odr["ORD_DEPART"].ToString ();
                    sbsEnti.ORD_PROV = odr["ORD_PROV"].ToString ();
                    sbsEnti.ORD_DIST = odr["ORD_DIST"].ToString ();
                    sbsEnti.ORD_TELEFONO = odr["ORD_TELEFONO"].ToString ();
                    sbsEnti.BEN_RELACION = odr["BEN_RELACION"].ToString ();
                    sbsEnti.BEN_CONDICION = odr["BEN_CONDICION"].ToString ();
                    sbsEnti.BEN_TIP_PER = odr["BEN_TIP_PER"].ToString ();
                    sbsEnti.BEN_TIP_DOC = odr["BEN_TIP_DOC"].ToString ();
                    sbsEnti.BEN_NUM_DOC = odr["BEN_NUM_DOC"].ToString ();
                    sbsEnti.BEN_NUM_RUC = odr["BEN_NUM_RUC"].ToString ();
                    sbsEnti.BEN_APEPAT = odr["BEN_APEPAT"].ToString ();
                    sbsEnti.BEN_APEMAT = odr["BEN_APEMAT"].ToString ();
                    sbsEnti.BEN_NOMBRES = odr["BEN_NOMBRES"].ToString ();
                    sbsEnti.BEN_OCUPACION = odr["BEN_OCUPACION"].ToString ();
                    sbsEnti.BEN_PAIS = odr["BEN_PAIS"].ToString ();
                    sbsEnti.BEN_CARGO = odr["BEN_CARGO"].ToString ();
                    sbsEnti.BEN_PEP = odr["BEN_PEP"].ToString ();
                    sbsEnti.BEN_DOMICILIO = odr["BEN_DOMICILIO"].ToString ();
                    sbsEnti.BEN_DEPART = odr["BEN_DEPART"].ToString ();
                    sbsEnti.BEN_PROV = odr["BEN_PROV"].ToString ();
                    sbsEnti.BEN_DIST = odr["BEN_DIST"].ToString ();
                    sbsEnti.BEN_TELEFONO = odr["BEN_TELEFONO"].ToString ();
                    sbsEnti.DAT_TIPFON = odr["DAT_TIPFON"].ToString ();
                    sbsEnti.DAT_TIPOPE = odr["DAT_TIPOPE"].ToString ();
                    sbsEnti.DAT_DESOPE = odr["DAT_DESOPE"].ToString ();
                    sbsEnti.DAT_ORIFON = odr["DAT_ORIFON"].ToString ();
                    sbsEnti.DAT_MONOPE = odr["DAT_MONOPE"].ToString ();
                    sbsEnti.DAT_MONOPE_A = odr["DAT_MONOPE_A"].ToString ();
                    sbsEnti.DAT_MTOOPE = odr["DAT_MTOOPE"].ToString ();
                    sbsEnti.DAT_MTOOPEA = odr["DAT_MTOOPEA"].ToString ();
                    sbsEnti.DAT_COD_ENT_INVO = odr["DAT_COD_ENT_INVO"].ToString ();
                    sbsEnti.DAT_COD_TIP_CTAO = odr["DAT_COD_TIP_CTAO"].ToString ();
                    sbsEnti.DAT_COD_CTAO = odr["DAT_COD_CTAO"].ToString ();
                    sbsEnti.DAT_ENT_FNC_EXTO = odr["DAT_ENT_FNC_EXTO"].ToString ();
                    sbsEnti.DAT_COD_ENT_INVB = odr["DAT_COD_ENT_INVB"].ToString ();
                    sbsEnti.DAT_COD_TIP_CTAB = odr["DAT_COD_TIP_CTAB"].ToString ();
                    sbsEnti.DAT_COD_CTAB = odr["DAT_COD_CTAB"].ToString ();
                    sbsEnti.DAT_ENT_FNC_EXTB = odr["DAT_ENT_FNC_EXTB"].ToString ();
                    sbsEnti.DAT_ALCANCE = odr["DAT_ALCANCE"].ToString ();
                    sbsEnti.DAT_COD_PAISO = odr["DAT_COD_PAISO"].ToString ();
                    sbsEnti.DAT_COD_PAISD = odr["DAT_COD_PAISD"].ToString ();
                    sbsEnti.DAT_INTOPE = odr["DAT_INTOPE"].ToString ();
                    sbsEnti.DAT_FORMA = odr["DAT_FORMA"].ToString ();
                    sbsEnti.DAT_INFORM = odr["DAT_INFORM"].ToString ();
                    result.Add (sbsEnti);
                }
                odr.Close ();
                this.context.Database.CloseConnection ();
                resp["data"] = result;
                return resp;
            } catch (Exception ex) {
                Console.WriteLine("el erro en el repository cursor siniestros : "+ex);
                Dictionary<string,dynamic> objError = new Dictionary<string,dynamic>();
                objError["code"] = "1";
                objError["message"] = ex.ToString();
                return objError;
            }
            //return result;
        }
    
    
        public DataTable SetColumnsDataTable (DataTable dt){

            dt.Columns.Add("FILA",typeof(string));
                    dt.Columns.Add("OFICINA",typeof(string));
                    dt.Columns.Add("OPERACION",typeof(string));
                    
                    dt.Columns.Add("INTERNO",typeof(string));
                    dt.Columns.Add("MODALIDAD",typeof(string));
                    dt.Columns.Add("OPE_UBIGEO",typeof(string));
                    dt.Columns.Add("OPE_FECHA",typeof(string));
                    dt.Columns.Add("OPE_HORA",typeof(string));
                    dt.Columns.Add("EJE_RELACION",typeof(string));
                    dt.Columns.Add("EJE_CONDICION",typeof(string));
                    dt.Columns.Add("EJE_TIPPER",typeof(string));
                    dt.Columns.Add("EJE_TIPDOC",typeof(string));
                    dt.Columns.Add("EJE_NUMDOC",typeof(string));
                    dt.Columns.Add("EJE_NUMRUC",typeof(string));
                    dt.Columns.Add("EJE_APEPAT",typeof(string));
                    dt.Columns.Add("EJE_APEMAT",typeof(string));
                    dt.Columns.Add("EJE_NOMBRES",typeof(string));
                    dt.Columns.Add("EJE_OCUPACION",typeof(string));
                    dt.Columns.Add("EJE_PAIS",typeof(string));
                    dt.Columns.Add("EJE_CARGO",typeof(string));
                    dt.Columns.Add("EJE_PEP",typeof(string));
                    dt.Columns.Add("EJE_DOMICILIO",typeof(string));
                    dt.Columns.Add("EJE_DEPART",typeof(string));
                    dt.Columns.Add("EJE_PROV",typeof(string));
                    dt.Columns.Add("EJE_DIST",typeof(string));
                    dt.Columns.Add("EJE_TELEFONO",typeof(string));
                    dt.Columns.Add("ORD_RELACION",typeof(string));
                    dt.Columns.Add("ORD_CONDICION",typeof(string));
                    dt.Columns.Add("ORD_TIPPER",typeof(string));
                    dt.Columns.Add("ORD_TIPDOC",typeof(string));
                    dt.Columns.Add("ORD_NUMDOC",typeof(string));
                    dt.Columns.Add("ORD_NUMRUC",typeof(string));
                    dt.Columns.Add("ORD_APEPAT",typeof(string));
                    dt.Columns.Add("ORD_APEMAT",typeof(string));
                    dt.Columns.Add("ORD_NOMBRES",typeof(string));
                    dt.Columns.Add("ORD_OCUPACION",typeof(string));
                    dt.Columns.Add("ORD_PAIS",typeof(string));
                    dt.Columns.Add("ORD_CARGO",typeof(string));
                    dt.Columns.Add("ORD_PEP",typeof(string));
                    dt.Columns.Add("ORD_DOMICILIO",typeof(string));
                    dt.Columns.Add("ORD_DEPART",typeof(string));
                    dt.Columns.Add("ORD_PROV",typeof(string));
                    dt.Columns.Add("ORD_DIST",typeof(string));
                    //dt.Columns.Add("ORD_UBIGEO",typeof(string));
                    dt.Columns.Add("ORD_TELEFONO",typeof(string));
                    dt.Columns.Add("BEN_RELACION",typeof(string));
                    dt.Columns.Add("BEN_CONDICION",typeof(string));
                    dt.Columns.Add("BEN_TIP_PER",typeof(string));
                    dt.Columns.Add("BEN_TIP_DOC",typeof(string));
                    dt.Columns.Add("BEN_NUM_DOC",typeof(string));
                    dt.Columns.Add("BEN_NUM_RUC",typeof(string));
                    dt.Columns.Add("BEN_APEPAT",typeof(string));
                    dt.Columns.Add("BEN_APEMAT",typeof(string));
                    dt.Columns.Add("BEN_NOMBRES",typeof(string));
                    dt.Columns.Add("BEN_OCUPACION",typeof(string));
                    dt.Columns.Add("BEN_PAIS",typeof(string));
                    dt.Columns.Add("BEN_CARGO",typeof(string));
                    dt.Columns.Add("BEN_PEP",typeof(string));
                    dt.Columns.Add("BEN_DOMICILIO",typeof(string));
                    dt.Columns.Add("BEN_DEPART",typeof(string));
                    dt.Columns.Add("BEN_PROV",typeof(string));
                    dt.Columns.Add("BEN_DIST",typeof(string));
                    //dt.Columns.Add("BEN_UBIGEO",typeof(string));
                    dt.Columns.Add("BEN_TELEFONO",typeof(string));
                    dt.Columns.Add("DAT_TIPFON",typeof(string));
                    dt.Columns.Add("DAT_TIPOPE",typeof(string));
                    dt.Columns.Add("DAT_DESOPE",typeof(string));
                    dt.Columns.Add("DAT_ORIFON",typeof(string));
                    dt.Columns.Add("DAT_MONOPE",typeof(string));
                    dt.Columns.Add("DAT_MONOPE_A",typeof(string));
                    dt.Columns.Add("DAT_MTOOPE",typeof(string));
                    dt.Columns.Add("DAT_MTOOPEA",typeof(string));
                    dt.Columns.Add("DAT_COD_ENT_INVO",typeof(string));
                    dt.Columns.Add("DAT_COD_TIP_CTAO",typeof(string));
                    dt.Columns.Add("DAT_COD_CTAO",typeof(string));
                    dt.Columns.Add("DAT_ENT_FNC_EXTO",typeof(string));
                    dt.Columns.Add("DAT_COD_ENT_INVB",typeof(string));
                    dt.Columns.Add("DAT_COD_TIP_CTAB",typeof(string));
                    dt.Columns.Add("DAT_COD_CTAB",typeof(string));
                    dt.Columns.Add("DAT_ENT_FNC_EXTB",typeof(string));
                    dt.Columns.Add("DAT_ALCANCE",typeof(string));
                    dt.Columns.Add("DAT_COD_PAISO",typeof(string));
                    dt.Columns.Add("DAT_COD_PAISD",typeof(string));
                    dt.Columns.Add("DAT_INTOPE",typeof(string));
                    dt.Columns.Add("DAT_FORMA",typeof(string));
                    dt.Columns.Add("DAT_INFORM",typeof(string));

            return dt;
        }
        
        public DataTable SetRowsDataTable (DataTable dt,SbsReportEnity sbsEnti){

            dt.Rows.Add(sbsEnti.FILA,sbsEnti.OFICINA,sbsEnti.OPERACION,
                    sbsEnti.INTERNO,sbsEnti.MODALIDAD,sbsEnti.OPE_UBIGEO,
                    sbsEnti.OPE_FECHA,sbsEnti.OPE_HORA,sbsEnti.EJE_RELACION,
                    sbsEnti.EJE_CONDICION,sbsEnti.EJE_TIPPER,sbsEnti.EJE_TIPDOC,
                    sbsEnti.EJE_NUMDOC,sbsEnti.EJE_NUMRUC,sbsEnti.EJE_APEPAT,
                    sbsEnti.EJE_APEMAT,sbsEnti.EJE_NOMBRES,sbsEnti.EJE_OCUPACION,
                    sbsEnti.EJE_PAIS,sbsEnti.EJE_CARGO,sbsEnti.EJE_PEP,
                    sbsEnti.EJE_DOMICILIO,sbsEnti.EJE_DEPART,sbsEnti.EJE_PROV,
                    sbsEnti.EJE_DIST,sbsEnti.EJE_TELEFONO,sbsEnti.ORD_RELACION,
                    sbsEnti.ORD_CONDICION,sbsEnti.ORD_TIPPER,sbsEnti.ORD_TIPDOC,
                    sbsEnti.ORD_NUMDOC,sbsEnti.ORD_NUMRUC,sbsEnti.ORD_APEPAT,
                    sbsEnti.ORD_APEMAT,sbsEnti.ORD_NOMBRES,sbsEnti.ORD_OCUPACION,
                    sbsEnti.ORD_PAIS,sbsEnti.ORD_CARGO,sbsEnti.ORD_PEP,
                    sbsEnti.ORD_DOMICILIO,sbsEnti.ORD_DEPART,
                    sbsEnti.ORD_PROV,sbsEnti.ORD_DIST,sbsEnti.ORD_TELEFONO,
                    sbsEnti.BEN_RELACION,sbsEnti.BEN_CONDICION,sbsEnti.BEN_TIP_PER,
                    sbsEnti.BEN_TIP_DOC,sbsEnti.BEN_NUM_DOC,sbsEnti.BEN_NUM_RUC,
                    sbsEnti.BEN_APEPAT,sbsEnti.BEN_APEMAT,sbsEnti.BEN_NOMBRES,
                    sbsEnti.BEN_OCUPACION,sbsEnti.BEN_PAIS,sbsEnti.BEN_CARGO,
                    sbsEnti.BEN_PEP,sbsEnti.BEN_DOMICILIO,sbsEnti.BEN_DEPART,
                    sbsEnti.BEN_PROV,sbsEnti.BEN_DIST,sbsEnti.BEN_TELEFONO,
                    sbsEnti.DAT_TIPFON,sbsEnti.DAT_TIPOPE,sbsEnti.DAT_DESOPE,
                    sbsEnti.DAT_ORIFON,sbsEnti.DAT_MONOPE,sbsEnti.DAT_MONOPE_A,
                    sbsEnti.DAT_MTOOPE,sbsEnti.DAT_MTOOPEA,sbsEnti.DAT_COD_ENT_INVO,
                    sbsEnti.DAT_COD_TIP_CTAO,sbsEnti.DAT_COD_CTAO,sbsEnti.DAT_ENT_FNC_EXTO,
                    sbsEnti.DAT_COD_ENT_INVB,sbsEnti.DAT_COD_TIP_CTAB,sbsEnti.DAT_COD_CTAB,
                    sbsEnti.DAT_ENT_FNC_EXTB,sbsEnti.DAT_ALCANCE,sbsEnti.DAT_COD_PAISO,
                    sbsEnti.DAT_COD_PAISD,sbsEnti.DAT_INTOPE,sbsEnti.DAT_FORMA,sbsEnti.DAT_INFORM);

            return dt;
        }
        

        public Dictionary<string, dynamic> InsertDataSUNAT (SbsCargaSUNATDTO param) {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            //List<Dictionary<string, dynamic>> result = new List<Dictionary<string, dynamic>> ();
            //List<SbsReportEnity> result = new List<SbsReportEnity> ();
            try {

                OracleParameter P_NID_CODIGO = new OracleParameter ("P_NID_CODIGO", OracleDbType.Varchar2, param.NID_CODIGO, ParameterDirection.Input);
                OracleParameter P_NUM_RUC = new OracleParameter ("P_NUM_RUC", OracleDbType.Varchar2, param.NUM_RUC, ParameterDirection.Input);
                OracleParameter P_NOM_RAZON = new OracleParameter ("P_NOM_RAZON", OracleDbType.Varchar2, param.NOM_RAZON, ParameterDirection.Input);
                OracleParameter P_TIPO_CONTRIBU = new OracleParameter ("P_TIPO_CONTRIBU", OracleDbType.Varchar2, param.TIPO_CONTRIBU, ParameterDirection.Input);
                OracleParameter P_PROFESION_OFI = new OracleParameter ("P_PROFESION_OFI", OracleDbType.Varchar2, param.PROFESION_OFI, ParameterDirection.Input);
                OracleParameter P_NOM_COMERCIAL = new OracleParameter ("P_NOM_COMERCIAL", OracleDbType.Varchar2, param.NOM_COMERCIAL, ParameterDirection.Input);
                OracleParameter P_CONDICION_CONTRIBUYENTE = new OracleParameter ("P_CONDICION_CONTRIBUYENTE", OracleDbType.Varchar2, param.CONDICION_CONTRIBUYENTE, ParameterDirection.Input);
                OracleParameter P_ESTADO_CONTRIBUYENTE = new OracleParameter ("P_ESTADO_CONTRIBUYENTE", OracleDbType.Varchar2, param.ESTADO_CONTRIBUYENTE, ParameterDirection.Input);
                OracleParameter P_FECHA_INSCRIP = new OracleParameter ("P_FECHA_INSCRIP", OracleDbType.Varchar2, param.FECHA_INSCRIP, ParameterDirection.Input);
                OracleParameter P_FECHA_INICIO_ACTIVIDAD = new OracleParameter ("P_FECHA_INICIO_ACTIVIDAD", OracleDbType.Varchar2, param.FECHA_INICIO_ACTIVIDAD, ParameterDirection.Input);
                OracleParameter P_DEPARTAMENTO = new OracleParameter ("P_DEPARTAMENTO", OracleDbType.Varchar2, param.DEPARTAMENTO, ParameterDirection.Input);
                OracleParameter P_PROVINCIA = new OracleParameter ("P_PROVINCIA", OracleDbType.Varchar2, param.PROVINCIA, ParameterDirection.Input);
                OracleParameter P_DISTRITO = new OracleParameter ("P_DISTRITO", OracleDbType.Varchar2, param.DISTRITO, ParameterDirection.Input);
                OracleParameter P_DIRECCION = new OracleParameter ("P_DIRECCION", OracleDbType.Varchar2, param.DIRECCION, ParameterDirection.Input);
                OracleParameter P_TELEFONO = new OracleParameter ("P_TELEFONO", OracleDbType.Varchar2, param.TELEFONO, ParameterDirection.Input);
                OracleParameter P_FAX = new OracleParameter ("P_FAX", OracleDbType.Varchar2, param.FAX, ParameterDirection.Input);
                OracleParameter P_ACTIVID_COMERCIO_EXTERIOR = new OracleParameter ("P_ACTIVID_COMERCIO_EXTERIOR", OracleDbType.Varchar2, param.ACTIVID_COMERCIO_EXTERIOR, ParameterDirection.Input);
                OracleParameter P_PRINCIPAL_CIIU = new OracleParameter ("P_PRINCIPAL_CIIU", OracleDbType.Varchar2, param.PRINCIPAL_CIIU, ParameterDirection.Input);
                OracleParameter P_SECUNDARIO_1_CIIU = new OracleParameter ("P_SECUNDARIO_1_CIIU", OracleDbType.Varchar2, param.SECUNDARIO_1_CIIU, ParameterDirection.Input);
                OracleParameter P_SECUNDARIO_2_CIIU = new OracleParameter ("P_SECUNDARIO_2_CIIU", OracleDbType.Varchar2, param.SECUNDARIO_2_CIIU, ParameterDirection.Input);
                OracleParameter P_AFECTO_NUEVO_RUS = new OracleParameter ("P_AFECTO_NUEVO_RUS", OracleDbType.Varchar2, param.AFECTO_NUEVO_RUS, ParameterDirection.Input);
                OracleParameter P_BUEN_CONTRIBUYENTE = new OracleParameter ("P_BUEN_CONTRIBUYENTE", OracleDbType.Varchar2, param.BUEN_CONTRIBUYENTE, ParameterDirection.Input);
                OracleParameter P_AGENTE_RETENCION = new OracleParameter ("P_AGENTE_RETENCION", OracleDbType.Varchar2, param.AGENTE_RETENCION, ParameterDirection.Input);
                OracleParameter P_AGENTE_PERCEPCION_VTAINT = new OracleParameter ("P_AGENTE_PERCEPCION_VTAINT", OracleDbType.Varchar2, param.AGENTE_PERCEPCION_VTAINT, ParameterDirection.Input);
                OracleParameter P_AGENTE_PERCEPCION_COMLIQ = new OracleParameter ("P_AGENTE_PERCEPCION_COMLIQ", OracleDbType.Varchar2, param.AGENTE_PERCEPCION_COMLIQ, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter ("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter ("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                P_NCODE.Size = 5000;
                P_SMESSAGE.Size = 5000;
                
                OracleParameter[] parameters;
                var query = "";
                parameters = new OracleParameter[] { P_NID_CODIGO, P_NUM_RUC, P_NOM_RAZON, P_TIPO_CONTRIBU, 
                        P_PROFESION_OFI, P_NOM_COMERCIAL,
                        P_CONDICION_CONTRIBUYENTE, P_ESTADO_CONTRIBUYENTE, 
                        P_FECHA_INSCRIP, P_FECHA_INICIO_ACTIVIDAD, 
                        P_DEPARTAMENTO, P_PROVINCIA, P_DISTRITO,
                        P_DIRECCION, P_TELEFONO, P_FAX, P_ACTIVID_COMERCIO_EXTERIOR, 
                        P_PRINCIPAL_CIIU, P_SECUNDARIO_1_CIIU, P_SECUNDARIO_2_CIIU,
                        P_AFECTO_NUEVO_RUS, P_BUEN_CONTRIBUYENTE, P_AGENTE_RETENCION, 
                        P_AGENTE_PERCEPCION_VTAINT, P_AGENTE_PERCEPCION_COMLIQ, P_NCODE, P_SMESSAGE };
                query = @"
                    BEGIN
                        INSUDB.PKG_LAFT_REPORTE_SBS_.INS_SBS_REPORT(:P_NID_CODIGO, :P_NUM_RUC, :P_NOM_RAZON, :P_TIPO_CONTRIBU, 
                        :P_PROFESION_OFI, :P_NOM_COMERCIAL,
                        :P_CONDICION_CONTRIBUYENTE, :P_ESTADO_CONTRIBUYENTE, 
                        :P_FECHA_INSCRIP, :P_FECHA_INICIO_ACTIVIDAD, 
                        :P_DEPARTAMENTO, :P_PROVINCIA, :P_DISTRITO,
                        :P_DIRECCION, :P_TELEFONO, :P_FAX, :P_ACTIVID_COMERCIO_EXTERIOR, 
                        :P_PRINCIPAL_CIIU, :P_SECUNDARIO_1_CIIU, :P_SECUNDARIO_2_CIIU,
                        :P_AFECTO_NUEVO_RUS, :P_BUEN_CONTRIBUYENTE, :P_AGENTE_RETENCION, 
                        :P_AGENTE_PERCEPCION_VTAINT, :P_AGENTE_PERCEPCION_COMLIQ, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";


                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                resp["error"] = Convert.ToInt32 (P_NCODE.Value.ToString ());
                resp["message"] = P_SMESSAGE.Value.ToString ();
                this.context.Database.CloseConnection ();
                return resp;
            } catch (Exception ex) {
                Console.WriteLine("el erro en el repository cursor siniestros : "+ex);
                Dictionary<string,dynamic> objError = new Dictionary<string,dynamic>();
                objError["code"] = "1";
                objError["message"] = ex.ToString();
                return objError;
            }
            //return result;
        }
    
public Dictionary<string, dynamic> InsertDataPAGOSMANUALES (SbsCargaPagosManualesDTO param) {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            //List<Dictionary<string, dynamic>> result = new List<Dictionary<string, dynamic>> ();
            //List<SbsReportEnity> result = new List<SbsReportEnity> ();
            try {

                OracleParameter P_NCODIGO_IDEN = new OracleParameter ("P_NCODIGO_IDEN", OracleDbType.Varchar2, param.NCODIGO_IDEN, ParameterDirection.Input);
                OracleParameter P_NCORRELATIVO = new OracleParameter ("P_NCORRELATIVO", OracleDbType.Varchar2, param.NCORRELATIVO, ParameterDirection.Input);
                OracleParameter P_NMEMO = new OracleParameter ("P_NMEMO", OracleDbType.Varchar2, param.NMEMO, ParameterDirection.Input);
                OracleParameter P_SMOTIVO = new OracleParameter ("P_SMOTIVO", OracleDbType.Varchar2, param.SMOTIVO, ParameterDirection.Input);
                OracleParameter P_SCANAL_AREA = new OracleParameter ("P_SCANAL_AREA", OracleDbType.Varchar2, param.SCANAL_AREA, ParameterDirection.Input);
                OracleParameter P_SASUNTO = new OracleParameter ("P_SASUNTO", OracleDbType.Varchar2, param.SASUNTO, ParameterDirection.Input);
                OracleParameter P_SFECHA_SOLICITUD = new OracleParameter ("P_SFECHA_SOLICITUD", OracleDbType.Varchar2, param.SFECHA_SOLICITUD, ParameterDirection.Input);
                OracleParameter P_SUSUARIO_SOLICITANTE = new OracleParameter ("P_SUSUARIO_SOLICITANTE", OracleDbType.Varchar2, param.SUSUARIO_SOLICITANTE, ParameterDirection.Input);
                OracleParameter P_POLIZA_CERTIFICADO_SINIESTRO = new OracleParameter ("P_POLIZA_CERTIFICADO_SINIESTRO", OracleDbType.Varchar2, param.POLIZA_CERTIFICADO_SINIESTRO, ParameterDirection.Input);
                OracleParameter P_DNI_BENEF_PAGO = new OracleParameter ("P_DNI_BENEF_PAGO", OracleDbType.Varchar2, param.DNI_BENEF_PAGO, ParameterDirection.Input);
                OracleParameter P_SMONEDA = new OracleParameter ("P_SMONEDA", OracleDbType.Varchar2, param.SMONEDA, ParameterDirection.Input);
                OracleParameter P_NMONTO = new OracleParameter ("P_NMONTO", OracleDbType.Decimal, param.NMONTO, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter ("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter ("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                P_NCODE.Size = 5000;
                P_SMESSAGE.Size = 5000;
                
                OracleParameter[] parameters;
                var query = "";
                parameters = new OracleParameter[] { P_NCODIGO_IDEN, P_NCORRELATIVO, P_NMEMO, P_SMOTIVO, 
                        P_SCANAL_AREA, P_SASUNTO, P_SFECHA_SOLICITUD, P_SUSUARIO_SOLICITANTE, 
                        P_POLIZA_CERTIFICADO_SINIESTRO, P_DNI_BENEF_PAGO, P_SMONEDA, P_NMONTO, 
                        P_NCODE, P_SMESSAGE };
                query = @"
                    BEGIN
                        INSUDB.PKG_LAFT_REPORTE_SBS_.INS_SBS_RPT_PAGOS_MANUALES(:P_NCODIGO_IDEN, :P_NCORRELATIVO, :P_NMEMO, :P_SMOTIVO, 
                        :P_SCANAL_AREA, :P_SASUNTO, :P_SFECHA_SOLICITUD, :P_SUSUARIO_SOLICITANTE, 
                        :P_POLIZA_CERTIFICADO_SINIESTRO, :P_DNI_BENEF_PAGO, :P_SMONEDA, :P_NMONTO, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";


                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                resp["code"] = Convert.ToInt32 (P_NCODE.Value.ToString ());
                resp["message"] = P_SMESSAGE.Value.ToString ();
                this.context.Database.CloseConnection ();
                return resp;
            } catch (Exception ex) {
                Console.WriteLine("el erro en el repository INS_SBS_RPT_PAGOS_MANUALES : "+ex);
                Dictionary<string,dynamic> objError = new Dictionary<string,dynamic>();
                objError["code"] = 2;
                resp["message"] = "error";
                objError["messageError"] = ex.ToString();
                objError["messageErrorDetalle"] = ex;
                return objError;
            }
            //return result;
        }
    

        public Dictionary<string,dynamic> UpdateEstadoMonitoreoSbs (string id,string fechaIniReport,string fechaFinReport,decimal tipocambio,string tipoOperacion, string tipoBusqueda, string estadoReporte) {
                Dictionary<string,dynamic> objRespuesta = new Dictionary<string,dynamic>();
            try {
                

                OracleParameter P_SID = new OracleParameter ("P_SID", OracleDbType.Varchar2, id, System.Data.ParameterDirection.Input);
                OracleParameter P_DINIREP = new OracleParameter ("P_DINIREP", OracleDbType.Varchar2, fechaIniReport, System.Data.ParameterDirection.Input);
                OracleParameter P_DFINREP = new OracleParameter ("P_DFINREP", OracleDbType.Varchar2, fechaFinReport, System.Data.ParameterDirection.Input);
                OracleParameter P_NTIPOCAMBIO = new OracleParameter ("P_NTIPOCAMBIO", OracleDbType.Decimal, tipocambio, System.Data.ParameterDirection.Input);
                OracleParameter P_STIPOPE = new OracleParameter ("P_STIPOPE", OracleDbType.Varchar2, tipoOperacion, System.Data.ParameterDirection.Input);
                OracleParameter P_STIPO_BUSQUEDA = new OracleParameter ("P_STIPO_BUSQUEDA", OracleDbType.Varchar2, tipoBusqueda, System.Data.ParameterDirection.Input);
                OracleParameter P_SESTADO = new OracleParameter ("P_SESTADO", OracleDbType.Varchar2, estadoReporte, System.Data.ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter ("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter ("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_SID, P_DINIREP, P_DFINREP, P_NTIPOCAMBIO, P_STIPOPE, P_STIPO_BUSQUEDA, P_SESTADO, P_NCODE, P_SMESSAGE };

                var sqlQuery = @"
                    BEGIN 
                    INSUDB.PKG_LAFT_REPORTE_SBS_2.SP_UPD_MONITOREO_REP_SBS(:P_SID, :P_DINIREP, :P_DFINREP, :P_NTIPOCAMBIO, :P_STIPOPE, :P_STIPO_BUSQUEDA, :P_SESTADO, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (sqlQuery, parameters);
                objRespuesta["code"] = Convert.ToInt32 (P_NCODE.Value.ToString ());
                objRespuesta["message"] = P_SMESSAGE.Value.ToString ();
                this.context.Database.CloseConnection ();
                return objRespuesta;

            } catch (Exception ex) {
                Console.WriteLine("El error del update monitoreo : "+ex);
                objRespuesta["code"] = 2;
                objRespuesta["messageError"] = ex.Message.ToString ();
                objRespuesta["messageErrorDetalle"] = ex;
                return objRespuesta;
            }
        }


    }
}

