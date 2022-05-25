using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using protecta.laft.api.DTO;
using protecta.laft.api.Models;

namespace protecta.laft.api.Repository {
    public class MenuConfigRepository : Interfaces.IMenuConfigRepository {
        private DB.ApplicationDbContext context;

        public MenuConfigRepository () {
            this.context = new DB.ApplicationDbContext (DB.ApplicationDB.UsarOracle ());
        }

        public List<MenuListResponseDTO> GetOptionList (MenuListParametersDTO param) {
            List<MenuListResponseDTO> result = new List<MenuListResponseDTO> ();
            try {

                OracleParameter P_NPROFILE = new OracleParameter ("P_NPROFILE", OracleDbType.Int32, param.profileId, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter ("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { P_NPROFILE, RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_USUARIO.SP_GET_MENU(:P_NPROFILE, :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) RC1.Value).GetDataReader ();
                while (odr.Read ()) {
                    MenuListResponseDTO item = new MenuListResponseDTO ();

                    
                    
                    item.nResourceId = Convert.ToInt32(odr["NIDRESOURCE"].ToString());
                    item.sName = odr["SNAME"] == DBNull.Value ? string.Empty : odr["SNAME"].ToString ();
                    item.sDescription = odr["SDESCRIPTION"] == DBNull.Value ? string.Empty : odr["SDESCRIPTION"].ToString ();
                    item.sHtml = odr["SHTML"] == DBNull.Value ? string.Empty : odr["SHTML"].ToString ();
                    item.nResourceType = Convert.ToInt32 (odr["NTYPERESOURCE"].ToString ());
                    item.nOrder = Convert.ToInt32 (odr["NORDER"].ToString ());
                    item.sActive = odr["SACTIVE"] == DBNull.Value ? string.Empty : odr["SACTIVE"].ToString ();
                    item.sRouterLink = odr["STAG"] == DBNull.Value ? string.Empty : odr["STAG"].ToString ();
                    item.nFatherId = Convert.ToInt32 (odr["NIDFATHER"].ToString ());
                    item.nTieneHijo = Convert.ToInt32 (odr["TIENE_HIJO"].ToString ());
                    result.Add (item);

                }
                odr.Close ();
                this.context.Database.CloseConnection ();
            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }

        public List<MenuListResponseDTO> GetSubOptionList (SubmenuListParametersDTO param) {
            List<MenuListResponseDTO> result = new List<MenuListResponseDTO> ();
            try {

                OracleParameter P_NIDRESOURCE = new OracleParameter ("P_NIDRESOURCE", OracleDbType.Int32, param.nResourceId, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter ("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { P_NIDRESOURCE, RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_USUARIO.SP_GET_SUBMENU(:P_NIDRESOURCE, :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection ();
                this.context.Database.ExecuteSqlCommand (query, parameters);
                OracleDataReader odr = ((OracleRefCursor) RC1.Value).GetDataReader ();
                while (odr.Read ()) {
                    MenuListResponseDTO item = new MenuListResponseDTO ();

                    item.sName = odr["SNAME"] == DBNull.Value ? string.Empty : odr["SNAME"].ToString ();
                    item.sDescription = odr["SDESCRIPTION"] == DBNull.Value ? string.Empty : odr["SDESCRIPTION"].ToString ();
                    item.sHtml = odr["SHTML"] == DBNull.Value ? string.Empty : odr["SHTML"].ToString ();
                    item.nResourceType = Convert.ToInt32 (odr["NTYPERESOURCE"].ToString ());
                    item.nOrder = Convert.ToInt32 (odr["NORDER"].ToString ());
                    item.sActive = odr["SACTIVE"] == DBNull.Value ? string.Empty : odr["SACTIVE"].ToString ();
                    item.sRouterLink = odr["STAG"] == DBNull.Value ? string.Empty : odr["STAG"].ToString ();
                    item.nFatherId = Convert.ToInt32 (odr["NIDFATHER"].ToString ());
                    result.Add (item);

                }
                odr.Close ();
                this.context.Database.CloseConnection ();
            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }

    }
}