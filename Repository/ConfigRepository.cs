
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using protecta.laft.api.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace protecta.laft.api.Repository
{
    public class ConfigRepository : Interfaces.IConfigRepository
    {
        private DB.ApplicationDbContext context;
        public ConfigRepository()
        {
            this.context = new DB.ApplicationDbContext(DB.ApplicationDB.UsarPrincipal());
        }


        public List<ResourceProfileDTO> ListResourceProfile(ResourceProfileRequestDTO dto)
        {
            List<ResourceProfileDTO> list = new List<ResourceProfileDTO>();
            try
            {

                OracleParameter P_LISTA = new OracleParameter("P_LISTA", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter p_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);

                p_NCODE.Size = 4000;
                p_SMESSAGE.Size = 4000;
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LIST_RESOURCE_PROFILE(:P_LISTA,:p_NCODE,:p_SMESSAGE);
                    END;
                    ";
                OracleParameter[] parameters = new OracleParameter[] { P_LISTA, p_NCODE, p_SMESSAGE };
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)P_LISTA.Value).GetDataReader();
                ResourceProfileDTO item = null;
                while (odr.Read())
                {
                    item = new ResourceProfileDTO();
                    item.sNameMenu = odr["SNAMEMENU"].ToString();
                    item.sNameSubMenu = odr["SNAMESUBMENU"].ToString();
                    item.nIdResource = Convert.ToInt32(odr["NIDRESOURCE"].ToString());
                    item.nIdFather = Convert.ToInt32(odr["NIDFATHER"].ToString());
                    item.sDescription = odr["SDESCRIPTION"].ToString();
                    item.sActive = (odr["SACTIVE"].ToString() == "0" ? false : true);
                    //item.sNameProfiles = odr["SNAMEPROFILE"].ToString();
                    item.nIdProfile = Convert.ToInt32(odr["NIDPROFILE"].ToString());
                    list.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public List<ResourceProfileHistoryDTO> ListResourceProfileHistory(ResourceProfileRequestDTO dto)
        {
            List<ResourceProfileHistoryDTO> list = new List<ResourceProfileHistoryDTO>();
            try
            {
                OracleParameter P_NIDPROFILE = new OracleParameter("P_NIDPROFILE", OracleDbType.Int32, dto.nIdProfile, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_HISTORIAL_RESOURCE_PROFILE(:P_NIDPROFILE,:RC1);
                    END;
                    ";
                OracleParameter[] parameters = new OracleParameter[] { P_NIDPROFILE, RC1 };
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                ResourceProfileHistoryDTO item = null;
                while (odr.Read())
                {
                    item = new ResourceProfileHistoryDTO();
                    //item.nIdHistorial = Convert.ToInt32(odr["NIDHISTORIAL"].ToString() == "" ? 0 : odr["NIDHISTORIAL"].ToString());
                    item.sProfileName = odr["PROFILENAME"].ToString();
                    item.sOpcion = odr["SOPCION"].ToString();
                    item.sAccion = odr["SACCION"].ToString();
                    item.dFechaRegistro = odr["DFECHA_REGISTRO"].ToString();
                    item.sUsuarioName = odr["USUARIONAME"].ToString();
                    list.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }


        public List<ResourceProfileHistoryDTO> UpdateResourceProfile(ResourceProfileParametersDTO dto)
        {
            List<ResourceProfileHistoryDTO> list = new List<ResourceProfileHistoryDTO>();
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                for (int i = 0; i < dto.items2.Count; i++)
                {
                    OracleParameter P_NIDPROFILE = new OracleParameter("P_NIDPROFILE", OracleDbType.Int32, dto.items2[i].nIdProfile, ParameterDirection.Input);
                    OracleParameter P_NIDPROFILEUSER = new OracleParameter("P_NIDPROFILEUSER", OracleDbType.Int32, dto.nIdProfile, ParameterDirection.Input);
                    OracleParameter P_NIDRESOURCE = new OracleParameter("P_NIDRESOURCE", OracleDbType.Int32, (dto.items2[i].nIdFather > 0 ? dto.items2[i].nIdFather : dto.items2[i].nIdResource) , ParameterDirection.Input);
                    OracleParameter P_NIDUSER = new OracleParameter("P_NIDUSER", OracleDbType.Int32, dto.nIdUser, ParameterDirection.Input);
                    OracleParameter P_SMENU = new OracleParameter("P_SMENU", OracleDbType.Varchar2, dto.items2[i].sMenu, ParameterDirection.Input);
                    OracleParameter P_SSUBMENU = new OracleParameter("P_SSUBMENU", OracleDbType.Varchar2, dto.items2[i].sSubMenu, ParameterDirection.Input);
                    OracleParameter P_SOPCION = new OracleParameter("P_SOPCION", OracleDbType.Varchar2, dto.items2[i].nResourceName, ParameterDirection.Input);
                    OracleParameter P_SACCION = new OracleParameter("P_SACCION", OracleDbType.Varchar2, (dto.items2[i].isChecked ? "Activar opción" : "Desactivar opción"), ParameterDirection.Input);
                    OracleParameter P_ISCHECK = new OracleParameter("P_ISCHECK", OracleDbType.Int32, (dto.items2[i].isChecked ?1 :0), ParameterDirection.Input);
                    OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                    OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                    P_NCODE.Size = 4000;
                    P_SMESSAGE.Size = 4000;
                    OracleParameter[] parameters = new OracleParameter[] {
                        P_NIDPROFILE,
                        P_NIDPROFILEUSER,
                        P_NIDRESOURCE,
                        P_NIDUSER,
                        P_SMENU,
                        P_SSUBMENU,
                        P_SOPCION,
                        P_SACCION,
                        P_ISCHECK,
                        P_NCODE,
                        P_SMESSAGE
                    };
                    var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_INS_RESOURCE_PROFILE(:P_NIDPROFILE,:P_NIDPROFILEUSER, :P_NIDRESOURCE,
                        :P_NIDUSER , :P_SMENU, :P_SSUBMENU, :P_SOPCION, :P_SACCION, :P_ISCHECK, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";
                    this.context.Database.OpenConnection();
                    this.context.Database.ExecuteSqlCommand(query, parameters);
                    output["nCode"] = P_NCODE.Value;
                    if (output["nCode"] != 0)
                    {
                        output["sMessage"] = P_SMESSAGE.Value;
                    }
                    this.context.Database.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
    }
}
