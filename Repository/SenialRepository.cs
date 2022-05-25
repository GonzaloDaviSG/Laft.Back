using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Types;
using protecta.laft.api.DTO;
using protecta.laft.api.Models;
using System.Net;
using Oracle.ManagedDataAccess.Client;

//using Oracle.DataAccess.Client;

namespace protecta.laft.api.Repository
{
    public class SenialRepository : Interfaces.IMaestroRepository<Senial>
    {
        private DB.ApplicationDbContext context;
        public SenialRepository()
        {
            this.context = new DB.ApplicationDbContext(DB.ApplicationDB.UsarPrincipal());
        }

        public List<Senial> GetAll()
        {
            try
            {
                return this.context.Seniales.ToList();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();

                Utils.ExceptionManager.resolve(ex);
                return new List<Senial>();
            }
        }
        public Senial Get(int Id)
        {
            try
            {
                return this.context.Seniales.Find(Id);
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Utils.ExceptionManager.resolve(ex);
                return new Senial();
            }
        }
        public void Update(Senial senial)
        {
            try
            {
                this.context.Seniales.Update(senial);
                this.context.SaveChanges();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Utils.ExceptionManager.resolve(ex);
            }
        }

        public List<Dictionary<string, dynamic>> GetOCEmail()
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_CORREO_OFICIAL(:RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["SEMAIL"] = odr["SEMAIL"];
                    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"];
                    item["SNOMPERFIL"] = odr["SNOMPERFIL"];
                    item["SDESCARGO"] = odr["SDESCARGO"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }

        internal object getClientsforRegimen(dynamic param)
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {
                var P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                var P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int64, param.NIDREGIMEN, ParameterDirection.Input);
                var P_NANO = new OracleParameter("P_NANO", OracleDbType.Int64, param.NANO, ParameterDirection.Input);
                var P_SNUM_DOCUMENTO = new OracleParameter("P_SNUM_DOCUMENTO", OracleDbType.NVarchar2, param.SNUM_DOCUMENTO, ParameterDirection.Input);
                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDREGIMEN, P_NANO, P_SNUM_DOCUMENTO, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTCLIENT_REGIMEN(:P_NPERIODO_PROCESO,:P_NIDREGIMEN,:P_NANO,:P_SNUM_DOCUMENTO, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["SRANGOPERIODO"] = odr["SRANGOPERIODO"];
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["SDESCRIPT"] = odr["SDESCRIPT"];
                    item["SCLINUMDOCU"] = odr["SCLINUMDOCU"];
                    item["SNAME"] = odr["SNAME"];
                    item["SDESREGIMEN"] = odr["SDESREGIMEN"];
                    item["SDESPRODUCTO"] = odr["SDESPRODUCTO"];
                    lista.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }

            return lista;
        }

        internal object GetSearchClientsPepSeacsa(dynamic param)
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {
                var P_FECHAINICIO = new OracleParameter("P_FECHAINICIO", OracleDbType.NVarchar2, param.FECHAINICIO, ParameterDirection.Input);
                var P_FECHAFIN = new OracleParameter("P_FECHAFIN", OracleDbType.NVarchar2, param.FECHAFIN, ParameterDirection.Input);
                var P_SNUM_DOCUMENTO = new OracleParameter("P_SNUM_DOCUMENTO", OracleDbType.NVarchar2, param.SNUM_DOCUMENTO, ParameterDirection.Input);
                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { P_FECHAINICIO, P_FECHAFIN, P_SNUM_DOCUMENTO, RC1 };

                var query = @"
                    BEGIN
                       INSUDB.PKG_BUSQ_COINCIDENCIAS_ALERTAS.SP_GET_BUSQUEDA_PEP_SEACSA(:P_FECHAINICIO,:P_FECHAFIN, :P_SNUM_DOCUMENTO, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["COD_TIPPROD"] = odr["COD_TIPPROD"];
                    item["NUM_POLIZA"] = odr["NUM_POLIZA"];
                    item["FEC_EMISION"] = odr["FEC_EMISION"] == DBNull.Value ? "-" : ((DateTime)(odr["FEC_EMISION"])).ToShortDateString();
                    item["ESTADO_POL"] = odr["ESTADO_POL"];
                    item["SNOMBRECOMPLETO"] = odr["SNOMBRECOMPLETO"];
                    item["SDESCRIPT"] = odr["SDESCRIPT"];
                    item["NUM_IDENBEN"] = odr["NUM_IDENBEN"];
                    item["PARENTESCO_PEP"] = odr["PARENTESCO_PEP"];
                    item["GLS_NOMBREPEP"] = odr["GLS_NOMBREPEP"];
                    item["TIP_DOCPEP"] = odr["TIP_DOCPEP"];
                    item["NUM_IDENPEP"] = odr["NUM_IDENPEP"];
                    item["GLS_NACIONALIDAD"] = odr["GLS_NACIONALIDAD"];
                    item["GLS_INSTITUCION"] = odr["GLS_INSTITUCION"];
                    item["TIP_ORGANIZACION"] = odr["TIP_ORGANIZACION"];
                    item["DESC_CARGO"] = odr["DESC_CARGO"];
                    item["PARENTESCO_FAMPEP"] = odr["PARENTESCO_FAMPEP"];
                    item["TIP_DOCFAMPEP"] = odr["TIP_DOCFAMPEP"];
                    item["NUM_IDENFAM"] = odr["NUM_IDENFAM"];
                    item["GLS_NOMBREFAM"] = odr["GLS_NOMBREFAM"];
                    item["GLS_NACIONALIDADFAM"] = odr["GLS_NACIONALIDADFAM"];
                    lista.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }

            return lista;
        }

        internal object GetSearchClientsPep(dynamic param)
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {
                var P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                var P_NANO = new OracleParameter("P_NANO", OracleDbType.Int64, param.NANO, ParameterDirection.Input);
                var P_NIDTIPOLISTA = new OracleParameter("P_NIDTIPOLISTA", OracleDbType.Int64, param.NIDTIPOLISTA, ParameterDirection.Input);
                var P_SNUM_DOCUMENTO = new OracleParameter("P_SNUM_DOCUMENTO", OracleDbType.NVarchar2, param.SNUM_DOCUMENTO, ParameterDirection.Input);
                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NANO, P_NIDTIPOLISTA, P_SNUM_DOCUMENTO, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_BUSQUEDA_PEP(:P_NPERIODO_PROCESO,:P_NANO,:P_NIDTIPOLISTA,:P_SNUM_DOCUMENTO, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["SRANGOPERIODO"] = odr["SRANGOPERIODO"];
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["STIPO_DOCUMENTO"] = odr["STIPO_DOCUMENTO"];
                    item["SNUM_DOCUMENTO"] = odr["SNUM_DOCUMENTO"];
                    item["SNOM_COMPLETO"] = odr["SNOM_COMPLETO"];
                    item["SDESTIPOLISTA"] = odr["SDESTIPOLISTA"];
                    item["SDESPRODUCTO"] = odr["SDESPRODUCTO"] == DBNull.Value ? string.Empty : odr["SDESPRODUCTO"].ToString();
                    lista.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }

            return lista;
        }

        public List<Dictionary<string, dynamic>> GetMovementHistory(dynamic param)
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {
                var P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                var P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int64, param.NIDGRUPOSENAL, ParameterDirection.Input);
                var P_NTIPOIDEN_BUSQ = new OracleParameter("P_NTIPOIDEN_BUSQ", OracleDbType.Varchar2, param.STIPOIDEN_BUSQ, ParameterDirection.Input);
                var P_SNUM_DOCUMENTO_BUSQ = new OracleParameter("P_SNUM_DOCUMENTO_BUSQ", OracleDbType.Varchar2, param.SNUM_DOCUMENTO_BUSQ, ParameterDirection.Input);
                var P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int64, param.NIDREGIMEN, ParameterDirection.Input);
                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDGRUPOSENAL, P_NTIPOIDEN_BUSQ, P_SNUM_DOCUMENTO_BUSQ, P_NIDREGIMEN, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_MOVIMIENTO(:P_NPERIODO_PROCESO,:P_NIDGRUPOSENAL,:P_NTIPOIDEN_BUSQ,:P_SNUM_DOCUMENTO_BUSQ,:P_NIDREGIMEN, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["SNOM_COMPLETO_BUSQ"] = odr["SNOM_COMPLETO_BUSQ"];
                    item["SDESTIPOLISTA"] = odr["SDESTIPOLISTA"];
                    item["NIDTIPOLISTA"] = odr["NIDTIPOLISTA"];
                    item["STIPO_BUSQUEDA"] = odr["STIPO_BUSQUEDA"];
                    item["USUARIO_APRUEBA"] = odr["USUARIO_APRUEBA"];
                    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"];
                    item["DFECHA_REVISADO"] = odr["DFECHA_REVISADO"];
                    item["DFECHA_REGISTRO"] = odr["DFECHA_REGISTRO"];
                    item["NTIPOCARGA"] = odr["NTIPOCARGA"];
                    item["NPORC_APROXIMA_BUSQ"] = odr["NPORC_APROXIMA_BUSQ"];
                    item["NACEPTA_COINCIDENCIA"] = odr["NACEPTA_COINCIDENCIA"];
                    item["DFECHA_REVISADO"] = odr["DFECHA_REVISADO"];
                    item["NIDPROVEEDOR"] = odr["NIDPROVEEDOR"];
                    lista.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }

            return lista;

        }


        public List<Dictionary<string, dynamic>> GetProfileList()
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {

                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_PERFIL(:RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["NIDPROFILE"] = odr["NIDPROFILE"];
                    item["SNAME"] = odr["SNAME"];

                    lista.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }

            return lista;

        }


        public List<Dictionary<string, dynamic>> GetListAction()
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {

                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_ACCIONES(:RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["NIDACCION"] = odr["NIDACCION"];
                    item["SDESCRIPT"] = odr["SDESCRIPT"];

                    lista.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }

            return lista;

        }


        public List<Dictionary<string, dynamic>> GetListPerfiles()
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {

                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_PERFILES(:RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["NIDPROFILE"] = odr["NIDPROFILE"];
                    item["SNAME"] = odr["SNAME"];
                    item["SDESCRIPTION"] = odr["SDESCRIPTION"];
                    item["SACTIVE"] = odr["SACTIVE"];
                    item["NUSERCODE"] = odr["NUSERCODE"];
                    item["DCOMPDATE"] = odr["DCOMPDATE"];
                    item["STIPO_USUARIO"] = odr["STIPO_USUARIO"];
                    item["NIDGRUPOSENAL"] = odr["NIDGRUPOSENAL"];
                    item["NIDREGIMEN"] = odr["NIDREGIMEN"];
                    item["DFECHA_REGISTRO"] = odr["DFECHA_REGISTRO"];
                    item["NIDUSUARIO_MODIFICA"] = odr["NIDUSUARIO_MODIFICA"];

                    lista.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }

            return lista;

        }





        public List<Dictionary<string, dynamic>> GetGrupoSenal()
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {

                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_GRUPO_SENAL(:RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["NIDGRUPOSENAL"] = odr["NIDGRUPOSENAL"];
                    item["SDESGRUPO_SENAL"] = odr["SDESGRUPO_SENAL"];

                    lista.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }

            return lista;

        }

        public List<Dictionary<string, dynamic>> GetListCorreo()
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {

                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_CONFIG_CORREO(:RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["NIDCORREO"] = odr["NIDCORREO"];
                    item["NIDGRUPOSENAL"] = odr["NIDGRUPOSENAL"];
                    item["SGRUPOSENAL"] = odr["SGRUPOSENAL"];
                    item["NIDPROFILE"] = odr["NIDPROFILE"];
                    item["DESPROFILE"] = odr["DESPROFILE"];
                    item["SASUNTO_CORREO"] = odr["SASUNTO_CORREO"];
                    item["SCUERPO_CORREO"] = odr["SCUERPO_CORREO"];
                    item["DFECHA_REGISTRO"] = odr["DFECHA_REGISTRO"];
                    item["SNOMBREUSUARIO"] = odr["SNOMBREUSUARIO"];
                    item["NIDUSUARIO_MODIFICA"] = odr["NIDUSUARIO_MODIFICA"];
                    item["NIDACCION"] = odr["NIDACCION"];
                    item["SDESACCION"] = odr["SDESACCION"];
                    item["SCUERPO_TEXTO"] = odr["SCUERPO_TEXTO"];
                    item["NCANTIDAD_DIAS"] = odr["NCANTIDAD_DIAS"];

                    lista.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }

            return lista;

        }


        public List<Dictionary<string, dynamic>> GetListaPerfiles()
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {

                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_PERFILES_GRUPO(:RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["NIDPROFILE"] = odr["NIDPROFILE"];
                    item["SNAME"] = odr["SNAME"];
                    item["SDESCRIPTION"] = odr["SDESCRIPTION"];
                    item["SACTIVE"] = odr["SACTIVE"];
                    item["NUSERCODE"] = odr["NUSERCODE"];
                    item["DCOMPDATE"] = odr["DCOMPDATE"];
                    item["STIPO_USUARIO"] = odr["STIPO_USUARIO"];
                    item["SDESGRUPO_SENAL"] = odr["SDESGRUPO_SENAL"];
                    item["NIDGRUPOSENAL"] = odr["NIDGRUPOSENAL"];
                    item["DFECHA_REGISTRO"] = odr["DFECHA_REGISTRO"];
                    item["NIDUSUARIO_MODIFICA"] = odr["NIDUSUARIO_MODIFICA"];


                    lista.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }

            return lista;

        }


        public List<Dictionary<string, dynamic>> GetListConifgCorreoDefault()
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {

                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_CONFIG_CORREO_DEF(:RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["NID"] = odr["NID"];
                    item["SNOMBRE"] = odr["SNOMBRE"];
                    item["NIDACCION"] = odr["NIDACCION"];

                    lista.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("Error" + ex);
                throw ex;
            }

            return lista;

        }


        public List<Dictionary<string, dynamic>> GetPolicyList(dynamic param)
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {
                var P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.P_NPERIODO_PROCESO, ParameterDirection.Input);
                var P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, param.P_NIDALERTA, ParameterDirection.Input);
                var P_NTIPOIDEN_BUSQ = new OracleParameter("P_NTIPOIDEN_BUSQ", OracleDbType.Varchar2, param.P_NTIPOIDEN_BUSQ, ParameterDirection.Input);
                var P_SNUM_DOCUMENTO_BUSQ = new OracleParameter("P_SNUM_DOCUMENTO_BUSQ", OracleDbType.Varchar2, param.P_SNUM_DOCUMENTO_BUSQ, ParameterDirection.Input);
                var P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int64, param.P_NIDREGIMEN, ParameterDirection.Input);
                var P_NTIPOCARGA = new OracleParameter("P_NTIPOCARGA", OracleDbType.Int64, param.P_NTIPOCARGA, ParameterDirection.Input);

                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);


                var parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDALERTA, P_NTIPOIDEN_BUSQ, P_SNUM_DOCUMENTO_BUSQ, P_NIDREGIMEN, P_NTIPOCARGA, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_POLIZAS_VIGENTES(:P_NPERIODO_PROCESO,:P_NIDALERTA, :P_NTIPOIDEN_BUSQ, :P_SNUM_DOCUMENTO_BUSQ, :P_NIDREGIMEN, :P_NTIPOCARGA, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["SNUM_POLIZA"] = odr["SNUM_POLIZA"];
                    item["NCERTIF"] = odr["NCERTIF"];
                    item["SPRODUCTO"] = odr["SPRODUCTO"];
                    item["PRIMA_POLIZA"] = odr["PRIMA_POLIZA"];
                    item["STIPO_CLIENTE"] = odr["STIPO_CLIENTE"];
                    item["FEC_PAGO"] = odr["FEC_PAGO"];
                    item["PLACA"] = odr["PLACA"];
                    item["RAMO"] = odr["RAMO"];
                    item["DFEC_INI_POLIZA"] = odr["DFEC_INI_POLIZA"];
                    item["DFEC_FIN_POLIZA"] = odr["DFEC_FIN_POLIZA"];
                    item["NIDREGIMEN"] = odr["NIDREGIMEN"];
                    item["SREGIMEN"] = odr["SREGIMEN"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }

            return lista;

        }

        internal List<Dictionary<string, dynamic>> GetSubGrupoSenal(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            Dictionary<string, dynamic> item = null;
            try
            {
                var P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int64, param.NIDGRUPOSENAL, ParameterDirection.Input);
                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);
                var parameters = new OracleParameter[] { P_NIDGRUPOSENAL, RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_SUB_GRUPO_SENAL(:P_NIDGRUPOSENAL, :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    item = new Dictionary<string, dynamic>();
                    item["NIDSUBGRUPOSEN"] = odr["NIDSUBGRUPOSEN"];
                    item["SDESSUBGRUPO_SENAL"] = odr["SDESSUBGRUPO_SENAL"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //EDUARDO
        public List<Dictionary<string, dynamic>> GetDetailQuestions(MonitoreoSenalesParamsDTO param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();

            try
            {
                OracleParameter P_NIDALERTA_CABECERA = new OracleParameter("P_NIDALERTA_CABECERA", OracleDbType.Int32, param.NIDALERTA_CABECERA, System.Data.ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter p_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                p_NCODE.Size = 4000;
                p_SMESSAGE.Size = 4000;
                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA_CABECERA, RC1, p_NCODE, p_SMESSAGE };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_DETALLE_RESPUESTAS(:P_NIDALERTA_CABECERA, :RC1, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NIDALERTA_CAB_USUARIO"] = odr["NIDALERTA_CAB_USUARIO"];
                    item["NIDALERTA_DETALLE"] = odr["NIDALERTA_DETALLE"];
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["NIDALERTA"] = odr["NIDALERTA"];
                    item["NIDALERTA_DET"] = odr["NIDALERTA_DET"];
                    item["NIDPREGUNTA_DET_PRE"] = odr["NIDPREGUNTA_DET_PRE"];
                    item["NIDORIGEN"] = odr["NIDORIGEN"];
                    item["SNOMBRE_CLIENTE"] = odr["SNOMBRE_CLIENTE"];
                    item["SPRODUCTO"] = odr["SPRODUCTO"];
                    item["SRUC"] = odr["SRUC"];
                    item["SPREGUNTA"] = odr["SPREGUNTA"];
                    item["NRESPUESTA"] = odr["NRESPUESTA"];
                    item["SCOMENTARIO"] = odr["SCOMENTARIO"];
                    item["NIDINDICAOBLCOMEN"] = odr["NIDINDICAOBLCOMEN"];

                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }

        internal List<Dictionary<string, dynamic>> getListProveedor()
        {
            List<Dictionary<string, dynamic>> List = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LIST_PROVEEDOR(:RC1);
                    END;
                    ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NIDPROVEEDOR"] = odr["NIDPROVEEDOR"].ToString();
                    item["SDESPROVEEDOR"] = odr["SDESPROVEEDOR"].ToString();
                    List.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return List;
        }

        public List<Dictionary<string, dynamic>> GetHeaderQuestions(MonitoreoSenalesParamsDTO param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDAGRUPA = new OracleParameter("P_NIDAGRUPA", OracleDbType.Int32, param.NIDAGRUPA, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_ASIGNADO = new OracleParameter("P_NIDUSUARIO_ASIGNADO", OracleDbType.Int32, param.NIDUSUARIO_ASIGNADO, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int32, param.NIDREGIMEN, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDALERTA_CAB_USUARIO = new OracleParameter("P_NIDALERTA_CAB_USUARIO", OracleDbType.Int32, param.NIDALERTA_CABECERA, System.Data.ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter p_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                p_NCODE.Size = 4000;
                p_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDUSUARIO_ASIGNADO, P_NIDAGRUPA, P_NIDALERTA, P_NIDREGIMEN, RC1, p_NCODE, p_SMESSAGE };

                var query = @"BEGIN
                LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_CABECERA_RESPUESTAS(:P_NPERIODO_PROCESO, :P_NIDUSUARIO_ASIGNADO, :P_NIDAGRUPA, :P_NIDALERTA, :P_NIDREGIMEN, :RC1, :P_NCODE,:P_SMESSAGE);
                END;
                ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NIDALERTA_CABECERA"] = odr["NIDALERTA_CABECERA"];
                    item["SPREGUNTA"] = odr["SPREGUNTA"];
                    item["NRESPUESTA"] = odr["NIND_RESPUESTA"];
                    item["SNOMBRE_ALERTA"] = odr["SNOMBRE_ALERTA"];
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"];
                    item["SCARGO"] = odr["SCARGO"];
                    item["SCOMENTARIO"] = odr["SCOMENTARIO"];
                    item["NIDPREGUNTA"] = odr["NIDPREGUNTA"];
                    item["NIDORIGEN"] = odr["NIDORIGEN"];
                    item["NIDALERTA"] = odr["NIDALERTA"];
                    item["NIDAGRUPA"] = odr["NIDAGRUPA"];
                    item["NIND_RESPUESTA"] = odr["NIND_RESPUESTA"];
                    item["SRUTA_PDF"] = odr["SRUTA_PDF"];
                    item["SEMAIL"] = odr["SEMAIL"];
                    item["ID_ROL"] = odr["ID_ROL"];
                    item["SOFICIAL_CUMPLIMIENTO"] = odr["USU_OC"];
                    item["SPERFIL"] = odr["PERFIL_OC"];
                    item["SRANGO_FECHA"] = odr["SRANGO_FECHA"];
                    item["SESTADO"] = odr["SESTADO"];
                    item["SNOMBRE_ESTADO"] = odr["SNOMBRE_ESTADO"];
                    item["SESTADO_PROC_ALERTA"] = odr["SESTADO_PROC_ALERTA"];
                    item["NIDINDICAOBLCOMEN"] = odr["NIDINDICAOBLCOMEN"];
                    item["SCOMENTARIO_OC"] = odr["SCOMENTARIO_OC"];
                    lista.Add(item);
                }
                odr.Close();
                /*
                odr = ((OracleRefCursor) RC2.Value).GetDataReader();
                if (odr.HasRows) {
                    while(odr.Read()) {
                    }
                }
                */
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine(" hola : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }

        internal object getClientWcEstado(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();

            try
            {
                OracleParameter P_SCLIENT = new OracleParameter("P_SCLIENT", OracleDbType.Varchar2, param.SCLIENT, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_SESTADO_TRAT = new OracleParameter("P_SESTADO_TRAT", OracleDbType.Varchar2, param.SESTADO_TRAT, ParameterDirection.Input);
                OracleParameter P_NIDRESULTADO = new OracleParameter("P_NIDRESULTADO", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_NIDPROVEEDOR = new OracleParameter("P_NIDPROVEEDOR", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SREPORT_COINCIDENCE = new OracleParameter("P_SREPORT_COINCIDENCE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_NIDRESULTADO.Size = 4000;
                P_NIDPROVEEDOR.Size = 4000;
                P_SREPORT_COINCIDENCE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] {
                    P_SCLIENT,
                    P_NPERIODO_PROCESO,
                    P_SESTADO_TRAT,
                    P_NIDRESULTADO,
                    P_NIDPROVEEDOR,
                    P_SREPORT_COINCIDENCE
                };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_CLIENT_WC_ESTADO(:P_SCLIENT,:P_NPERIODO_PROCESO, :P_SESTADO_TRAT, :P_NIDRESULTADO, :P_NIDPROVEEDOR, :P_SREPORT_COINCIDENCE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                output["NIDRESULTADO"] = Convert.ToInt64((P_NIDRESULTADO.Value).ToString());
                output["NIDPROVEEDOR"] = Convert.ToInt64((P_NIDPROVEEDOR.Value).ToString());
                output["SREPORT_COINCIDENCE"] = P_SREPORT_COINCIDENCE.Value.ToString() == "null" ? "" : P_SREPORT_COINCIDENCE.Value.ToString();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                output["nCode"] = 1;
                output["sMessage"] = "Ocurrio un error, contactar al soporte. " + ex.Message;
                return output;
            }
            return output;
        }

        internal object updateWebLink(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();

            try
            {
                OracleParameter P_SROWID = new OracleParameter("P_SROWID", OracleDbType.Varchar2, param["SROWID"], ParameterDirection.Input);
                OracleParameter P_SSTATE = new OracleParameter("P_SSTATE", OracleDbType.Varchar2, param["SSTATE"], ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] {
                    P_SROWID,
                    P_SSTATE,
                    P_NCODE,
                    P_SMESSAGE
                };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_WEBLINK_COINCIDENCE(:P_SROWID,:P_SSTATE, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                output["nCode"] = Convert.ToInt64((P_NCODE.Value).ToString());
                output["sMessage"] = P_SMESSAGE.Value;
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                output["nCode"] = 1;
                output["sMessage"] = "Ocurrio un error, contactar al soporte. " + ex.Message;
                return output;
            }
            return output;
        }

        internal InformeKri getInformacionActividadEconomica(dynamic param)
        {
            InformeKri response = new InformeKri();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, System.Data.ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter p_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                p_NCODE.Size = 4000;
                p_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, RC1, p_NCODE, p_SMESSAGE };

                var query = @"BEGIN
                LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_ACTIVIDAD_ECONOMICA_INFORME(:P_NPERIODO_PROCESO, :RC1, :P_NCODE, :P_SMESSAGE);
                END;
                ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                response.code = Convert.ToInt16((p_NCODE.Value).ToString());
                response.mesagge = p_SMESSAGE.Value.ToString();
                if (response.code == 1) {
                    return response;
                }
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                response.ActividadEconomicaCuadro = new List<Dictionary<string, dynamic>>();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["SSECTOR"] = odr["SSECTOR"].ToString();
                    item["NCANTIDAD"] = odr["NCANTIDAD"].ToString();
                    item["NPORCENTAJE"] = double.Parse(odr["NPORCENTAJE"].ToString());
                    response.ActividadEconomicaCuadro.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
                return response;
            }
            catch (Exception ex)
            {
                response.mesagge = ex.Message;
                response.code = 1;
                return response;
            }
        }

        internal InformeKri getInformacionZonaGeografica(dynamic param)
        {
            InformeKri response = new InformeKri();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, System.Data.ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter RC2 = new OracleParameter("RC2", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter p_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                p_NCODE.Size = 4000;
                p_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, RC1, RC2, p_NCODE, p_SMESSAGE };

                var query = @"BEGIN
                LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_ZONA_GEOGRAFICA_INFORME(:P_NPERIODO_PROCESO, :RC1, :RC2, :P_NCODE, :P_SMESSAGE);
                END;
                ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                response.code = Convert.ToInt16((p_NCODE.Value).ToString());
                response.mesagge = p_SMESSAGE.Value.ToString();
                if (response.code == 1) {
                    return response;
                }
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                response.ZonasGeograficas = new List<Dictionary<string, dynamic>>();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["GLS_REGION"] = odr["GLS_REGION"].ToString();
                    item["NAHORRO_TOTAL"] = odr["NAHORRO_TOTAL"].ToString();
                    item["NRENTA_TOTAL"] = odr["NRENTA_TOTAL"].ToString();
                    item["NVIDA_RENTA"] = odr["NVIDA_RENTA"].ToString();
                    item["NTOTAL"] = odr["NTOTAL"].ToString();
                    item["NPORCENTAJE"] = double.Parse(odr["NPORCENTAJE"].ToString());
                    item["BISNACIONAL"] = int.Parse(odr["BISNACIONAL"].ToString());
                    response.ZonasGeograficas.Add(item);
                }
                odr.Close();
                OracleDataReader odr2 = ((OracleRefCursor)RC2.Value).GetDataReader();
                response.ZonaGeograficaCuadro = new List<Dictionary<string, dynamic>>();
                while (odr2.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["SREGION"] = odr2["SREGION"].ToString();
                    item["TOTAL"] = odr2["TOTAL"].ToString();
                    item["NPORCENTAJE"] = double.Parse(odr2["NPORCENTAJE"].ToString());
                    response.ZonaGeograficaCuadro.Add(item);
                }
                odr2.Close();
                this.context.Database.CloseConnection();
                return response;
            }
            catch (Exception ex)
            {
                response.mesagge = ex.Message;
                response.code = 1;
                return response;
            }
        }

        internal InformeKri getInformacionEs10(dynamic param)
        {
            InformeKri response = new InformeKri();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, System.Data.ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter RC2 = new OracleParameter("RC2", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter p_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                p_NCODE.Size = 4000;
                p_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, RC1, RC2, p_NCODE, p_SMESSAGE };

                var query = @"BEGIN
                LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_ES10_INFORME(:P_NPERIODO_PROCESO, :RC1, :RC2, :P_NCODE, :P_SMESSAGE);
                END;
                ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                response.code = Convert.ToInt16((p_NCODE.Value).ToString());
                response.mesagge = p_SMESSAGE.Value.ToString();
                if (response.code == 1) {
                    return response;
                }
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                response.Es10 = new List<Es10Entity>();
                while (odr.Read())
                {
                    Es10Entity item = new Es10Entity();
                    item.sRamo = odr["SRAMO"].ToString();
                    item.nCodRiesgo = int.Parse(odr["NCOD_RIESGO"].ToString());
                    item.sCodRegistro = odr["SCOD_REGISTRO"].ToString();
                    item.sNomComercial = odr["SNOM_COMERCIAL"].ToString();
                    item.sMoneda = odr["SMONEDA"].ToString();
                    item.sFechaIniComercial = odr["DFEC_INI_COMERCIAL"].ToString();
                    item.nCantAsegurados = int.Parse(odr["NCANT_ASEGURADOS"].ToString());
                    response.Es10.Add(item);
                }
                odr.Close();
                OracleDataReader odr2 = ((OracleRefCursor)RC2.Value).GetDataReader();
                response.Es10Cuadro = new List<Dictionary<string, dynamic>>();
                while (odr2.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["SREGIMEN"] = odr2["SREGIMEN"].ToString();
                    item["NCANT_ASEGURADOS"] = odr2["NCANT_ASEGURADOS"].ToString();
                    item["NPORCENTAJE"] = double.Parse(odr2["NPORCENTAJE"].ToString());
                    response.Es10Cuadro.Add(item);
                }
                odr2.Close();
                this.context.Database.CloseConnection();
                return response;
            }
            catch (Exception ex)
            {
                response.mesagge = ex.Message;
                response.code = 1;
                return response;
            }
        }

        internal List<Dictionary<string, dynamic>> getListasPorProveedor()
        {
            List<Dictionary<string, dynamic>> List = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LIST_FOR_PROVEEDOR(:RC1);
                    END;
                    ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NIDTIPOLISTA"] = odr["NIDTIPOLISTA"].ToString();
                    item["SDESTIPOLISTA"] = odr["SDESTIPOLISTA"].ToString();
                    item["NIDPROVEEDOR"] = odr["NIDPROVEEDOR"].ToString();
                    item["SDESPROVEEDOR"] = odr["SDESPROVEEDOR"].ToString();
                    List.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return List;
        }

        internal object addWebLinkscliente(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();

            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int32, param.NIDGRUPOSENAL, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDSUBGRUPOSEN = new OracleParameter("P_NIDSUBGRUPOSEN", OracleDbType.Int32, param.NIDSUBGRUPOSEN, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDPROVEEDOR = new OracleParameter("P_NIDPROVEEDOR", OracleDbType.Int32, param.NIDPROVEEDOR, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDTIPOLISTA = new OracleParameter("P_NIDTIPOLISTA", OracleDbType.Int32, param.NIDTIPOLISTA, System.Data.ParameterDirection.Input);
                OracleParameter P_SNUM_DOCUMENTO = new OracleParameter("P_SNUM_DOCUMENTO", OracleDbType.NVarchar2, param.SNUM_DOCUMENTO, System.Data.ParameterDirection.Input);
                OracleParameter P_SURI = new OracleParameter("P_SURI", OracleDbType.Varchar2, param.SURI, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDGRUPOSENAL, P_NIDSUBGRUPOSEN, P_NIDPROVEEDOR, P_NIDTIPOLISTA, P_SNUM_DOCUMENTO, P_SURI, P_NCODE, P_SMESSAGE };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_INS_LINKS_CLIENTS(:P_NPERIODO_PROCESO,:P_NIDGRUPOSENAL,:P_NIDSUBGRUPOSEN,:P_NIDPROVEEDOR,:P_NIDTIPOLISTA,:P_SNUM_DOCUMENTO, :P_SURI ,:P_NCODE ,:P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                output["nCode"] = Convert.ToInt64((P_NCODE.Value).ToString());
                output["sMessage"] = P_SMESSAGE.Value;
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                output["nCode"] = 1;
                output["sMessage"] = "Ocurrio un error, contactar al soporte. " + ex.Message;
                return output;
            }
            return output;
        }

        internal object getDeleteWebLinksCoincidence(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();

            try
            {
                OracleParameter P_SROWID = new OracleParameter("P_SROWID", OracleDbType.Varchar2, param.SROWID, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] {
                    P_SROWID,
                    P_NCODE,
                    P_SMESSAGE
                };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_DEL_WEBLINK_COINCIDENCE(:P_SROWID, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                output["nCode"] = Convert.ToInt64((P_NCODE.Value).ToString());
                output["sMessage"] = P_SMESSAGE.Value;
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                output["nCode"] = 1;
                output["sMessage"] = "Ocurrio un error, contactar al soporte. " + ex.Message;
                return output;
            }
            return output;
        }

        internal List<Dictionary<string, dynamic>> getListWebLinksCliente(dynamic param)
        {
            List<Dictionary<string, dynamic>> List = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int32, param.NIDGRUPOSENAL, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDSUBGRUPOSEN = new OracleParameter("P_NIDSUBGRUPOSEN", OracleDbType.Int32, param.NIDSUBGRUPOSEN, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDPROVEEDOR = new OracleParameter("P_NIDPROVEEDOR", OracleDbType.Int32, param.NIDPROVEEDOR, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDTIPOLISTA = new OracleParameter("P_NIDTIPOLISTA", OracleDbType.Int32, param.NIDTIPOLISTA, System.Data.ParameterDirection.Input);
                OracleParameter P_SNUM_DOCUMENTO = new OracleParameter("P_SNUM_DOCUMENTO", OracleDbType.NVarchar2, param.SNUM_DOCUMENTO, System.Data.ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDGRUPOSENAL, P_NIDSUBGRUPOSEN, P_NIDPROVEEDOR, P_NIDTIPOLISTA, P_SNUM_DOCUMENTO, RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LINKS_CLIENTS(:P_NPERIODO_PROCESO,:P_NIDGRUPOSENAL,:P_NIDSUBGRUPOSEN,:P_NIDPROVEEDOR,:P_NIDTIPOLISTA,:P_SNUM_DOCUMENTO, :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["SROWID"] = odr["SROWID"];
                    item["SURI"] = odr["SURI"];
                    item["SSTATE"] = odr["SSTATE"];
                    item["SDESTIPOLISTA"] = odr["SDESTIPOLISTA"];
                    List.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return List;
        }

        internal List<Dictionary<string, dynamic>> GetListaTipo()
        {
            List<Dictionary<string, dynamic>> list = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_TIPOS_LISTA_ALL( :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NIDTIPOLISTA"] = odr["NIDTIPOLISTA"];
                    item["SDESTIPOLISTA"] = odr["SDESTIPOLISTA"];
                    item["SESTADO"] = odr["SESTADO"];
                    item["SDESCORTALISTA"] = odr["SDESCORTALISTA"];
                    list.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(" hola : " + ex);
                throw;
            }

        }

        //EDUARDO
        public Dictionary<string, dynamic> InsertQuestionDetail(QuestionDetailDTO param)
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_NIDALERTA_CAB_USUARIO = new OracleParameter("P_NIDALERTA_CAB_USUARIO", OracleDbType.Int32, param.NIDALERTA_CAB_USUARIO, ParameterDirection.Input);
                OracleParameter P_NIDALERTA_DET_USUARIO = new OracleParameter("P_NIDALERTA_DET_USUARIO", OracleDbType.Int32, param.NIDALERTA_DETALLE, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NIDALERTA_DET = new OracleParameter("P_NIDALERTA_DET", OracleDbType.Int32, param.NIDALERTA_DET, ParameterDirection.Input);
                OracleParameter P_NIDPREGUNTA = new OracleParameter("P_NIDPREGUNTA", OracleDbType.Int32, param.NIDPREGUNTA_DET_PRE, ParameterDirection.Input);
                OracleParameter P_NIDORIGEN = new OracleParameter("P_NIDORIGEN", OracleDbType.Varchar2, param.NIDORIGEN, ParameterDirection.Input);
                OracleParameter P_NIND_RESPUESTA = new OracleParameter("P_NIND_RESPUESTA", OracleDbType.Int32, param.NRESPUESTA, ParameterDirection.Input);
                OracleParameter P_SCOMENTARIO = new OracleParameter("P_SCOMENTARIO", OracleDbType.Varchar2, param.SCOMENTARIO, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;
                OracleParameter[] parameters = new OracleParameter[] {
                    P_NIDALERTA_CAB_USUARIO,
                    P_NIDALERTA_DET_USUARIO,
                    P_NPERIODO_PROCESO,
                    P_NIDALERTA,
                    P_NIDALERTA_DET,
                    P_NIDPREGUNTA,
                    P_NIDORIGEN,
                    P_NIND_RESPUESTA,
                    P_SCOMENTARIO,
                    P_NCODE,
                    P_SMESSAGE
                };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_RESPUESTA_DET(:P_NIDALERTA_CAB_USUARIO, :P_NIDALERTA_DET_USUARIO, :P_NPERIODO_PROCESO, :P_NIDALERTA, :P_NIDALERTA_DET, :P_NIDPREGUNTA, 
                                :P_NIDORIGEN, :P_NIND_RESPUESTA, :P_SCOMENTARIO, :P_NCODE, :P_SMESSAGE);
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
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return output;
        }

        internal List<Dictionary<string, dynamic>> GetProveedorCoincidencia(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int32, param.NIDGRUPOSENAL, ParameterDirection.Input);
                OracleParameter P_NIDSUBGRUPOSEN = new OracleParameter("P_NIDSUBGRUPOSEN", OracleDbType.Int32, param.NIDSUBGRUPOSEN, ParameterDirection.Input);
                OracleParameter P_SCLIENT = new OracleParameter("P_SCLIENT", OracleDbType.NVarchar2, param.SCLIENT, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDALERTA, P_NIDGRUPOSENAL, P_NIDSUBGRUPOSEN, P_SCLIENT, RC1 };
                var query = @"
                    BEGIN
                        INSUDB.PKG_BUSQ_COINCIDENCIAS_ALERTAS.SP_GET_PROVEEDOR_BEFORE_TRATAM(:P_NPERIODO_PROCESO,:P_NIDALERTA, :P_NIDGRUPOSENAL, :P_NIDSUBGRUPOSEN, :P_SCLIENT, :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NIDPROVEEDOR"] = odr["NIDPROVEEDOR"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }
            return lista;
        }

        //EDUARDO
        public Dictionary<string, dynamic> InsertQuestionHeader(MonitoreoSenalesParamsDTO param)
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_NIDALERTA_CAB_USUARIO = new OracleParameter("P_NIDALERTA_CAB_USUARIO", OracleDbType.Int32, param.NIDALERTA_CABECERA, ParameterDirection.Input);
                OracleParameter P_NIDPREGUNTA = new OracleParameter("P_NIDPREGUNTA", OracleDbType.Int32, param.NIDPREGUNTA, ParameterDirection.Input);
                OracleParameter P_NIDORIGEN = new OracleParameter("P_NIDORIGEN", OracleDbType.Int32, param.NIDORIGEN, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NIDAGRUPA = new OracleParameter("P_NIDAGRUPA", OracleDbType.Int32, param.NIDAGRUPA, ParameterDirection.Input);
                OracleParameter P_NIND_RESPUESTA = new OracleParameter("P_NIND_RESPUESTA", OracleDbType.Int32, param.NRESPUESTA, ParameterDirection.Input);
                OracleParameter P_SCOMENTARIO = new OracleParameter("P_SCOMENTARIO", OracleDbType.Varchar2, param.SCOMENTARIO, ParameterDirection.Input);
                OracleParameter P_SRUTA_PDF = new OracleParameter("P_SRUTA_PDF", OracleDbType.Int32, param.SRUTA_PDF, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;
                OracleParameter[] parameters = new OracleParameter[] {
                    P_NIDALERTA_CAB_USUARIO,
                    P_NIDPREGUNTA,
                    P_NIDORIGEN,
                    P_NIDALERTA,
                    P_NIDAGRUPA,
                    P_NIND_RESPUESTA,
                    P_SCOMENTARIO,
                    P_SRUTA_PDF,
                    P_NCODE,
                    P_SMESSAGE
                };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_RESPUESTA_CAB(:P_NIDALERTA_CAB_USUARIO, :P_NIDPREGUNTA, :P_NIDORIGEN, :P_NIDALERTA, :P_NIDAGRUPA, :P_NIND_RESPUESTA, :P_SCOMENTARIO,
                                :P_SRUTA_PDF, :P_NCODE, :P_SMESSAGE);
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
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return output;
        }
        //EDUARDO
        public List<Dictionary<string, dynamic>> GetAlertFormList(AlertFormParamDTO param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NIDUSUARIO_ASIGNADO = new OracleParameter("P_NIDUSUARIO_ASIGNADO", OracleDbType.Int32, param.NIDUSUARIO_ASIGNADO, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { P_NIDUSUARIO_ASIGNADO, P_NPERIODO_PROCESO, RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_BANDEJA_USUARIO(:P_NIDUSUARIO_ASIGNADO, :P_NPERIODO_PROCESO, :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["SNOMBRE_ALERTA"] = odr["SNOMBRE_ALERTA"];
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["NIDAGRUPA"] = "" + odr["NIDAGRUPA"];
                    item["DFECHA_ESTADO_MOVIMIENTO"] = odr["DFECHA_ESTADO_MOVIMIENTO"];
                    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"];
                    item["NIDUSUARIO_ASIGNADO"] = odr["NIDUSUARIO_ASIGNADO"];
                    item["NIDALERTA_CABECERA"] = odr["NIDALERTA_CABECERA"];
                    item["SNOMBRE_ESTADO"] = odr["SNOMBRE_ESTADO"];
                    item["NIDALERTA"] = odr["NIDALERTA"];
                    item["NIDREGIMEN"] = "" + odr["NIDREGIMEN"];
                    item["SDESCRIPCION_ALERTA"] = odr["SDESCRIPCION_ALERTA"];
                    item["SDESCRIPCION_ALERTA1_CORTA"] = odr["SDESCRIPCION_ALERTA1_CORTA"];
                    item["SDESREGIMEN"] = odr["SDESREGIMEN"];
                    item["SESTADO"] = odr["SESTADO"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;

            }
            return lista;
        }

        public List<Dictionary<string, dynamic>> GetOfficialAlertFormList(OfficialAlertFormParamDTO param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int32, param.NIDREGIMEN, ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int32, param.NIDGRUPOSENAL, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDREGIMEN, P_NIDGRUPOSENAL, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_BANDEJA_OFICIAL(:P_NPERIODO_PROCESO, :P_NIDREGIMEN, :P_NIDGRUPOSENAL, :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"];
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["SNOMBRE_ALERTA"] = odr["SNOMBRE_ALERTA"];
                    item["SNOMBRE_ESTADO"] = odr["SNOMBRE_ESTADO"];
                    item["DFECHA_ESTADO_MOVIMIENTO"] = odr["DFECHA_ESTADO_MOVIMIENTO"];
                    item["DFECHA_REVISADO"] = odr["DFECHA_REVISADO"];
                    item["NIDALERTA_CABECERA"] = odr["NIDALERTA_CABECERA"];
                    item["NIDAGRUPA"] = odr["NIDAGRUPA"];
                    item["NIDALERTA"] = odr["NIDALERTA"];
                    item["SESTADO_REVISADO"] = odr["SESTADO_REVISADO"];
                    item["NIDUSUARIO_ASIGNADO"] = odr["NIDUSUARIO_ASIGNADO"];
                    item["STIPO_USUARIO"] = odr["STIPO_USUARIO"];
                    item["SDESCRIPCION_ALERTA"] = odr["SDESCRIPCION_ALERTA"];
                    item["SDESCRIPCION_ALERTA1_CORTA"] = odr["SDESCRIPCION_ALERTA1_CORTA"];
                    item["SESTADO"] = odr["SESTADO"];
                    item["TIPO_FORM"] = odr["TIPO_FORM"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }
            return lista;
        }

        public List<Dictionary<string, dynamic>> GetListNcCompanies(NcCompaniesParamDTO param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.P_NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.P_NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int32, param.P_NIDREGIMEN, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, P_NIDREGIMEN, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_NC_EMPRESAS(:P_NIDALERTA, :P_NPERIODO_PROCESO, :P_NIDREGIMEN, :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NIDALERTA"] = odr["NIDALERTA"];
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["NDEVOLUCION"] = odr["NDEVOLUCION"];
                    item["NIDREGIMEN"] = odr["NIDREGIMEN"];
                    item["NOMCLIENTE"] = odr["NOMCLIENTE"];
                    item["DESTIPODOC"] = odr["DESTIPODOC"];
                    item["NRODOC"] = odr["NRODOC"];
                    item["DESPRODUCTO"] = odr["DESPRODUCTO"];
                    item["NPOLICY"] = odr["NPOLICY"];
                    item["DESTIPOCOMP"] = odr["DESTIPOCOMP"];
                    item["NROCOMP"] = odr["NROCOMP"];
                    item["NAMOUNT"] = odr["NAMOUNT"];
                    item["NIVA"] = odr["NIVA"];
                    item["NRIGHTISSUE"] = odr["NRIGHTISSUE"];
                    item["DBILLDATE"] = odr["DBILLDATE"];
                    item["SESTADOPAGEFEC"] = odr["SESTADOPAGEFEC"];
                    item["DESMOTIVO"] = odr["DESMOTIVO"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }
            return lista;
        }

        public Dictionary<string, dynamic> UpdateListNcCompanies(UpdateNcCompaniesParamDTO param)
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.P_NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.P_NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NDEVOLUCION = new OracleParameter("P_NDEVOLUCION", OracleDbType.Int32, param.P_NDEVOLUCION, ParameterDirection.Input);
                OracleParameter P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int32, param.P_NIDREGIMEN, ParameterDirection.Input);
                OracleParameter P_SESTADOPAGEFEC = new OracleParameter("P_SESTADOPAGEFEC", OracleDbType.Varchar2, param.P_SESTADOPAGEFEC, ParameterDirection.Input);
                OracleParameter P_DESMOTIVO = new OracleParameter("P_DESMOTIVO", OracleDbType.Varchar2, param.P_DESMOTIVO, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;
                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, P_NDEVOLUCION, P_NIDREGIMEN, P_SESTADOPAGEFEC, P_DESMOTIVO, P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_NC_EMPRESAS(:P_NIDALERTA, :P_NPERIODO_PROCESO,:P_NDEVOLUCION,:P_NIDREGIMEN,:P_SESTADOPAGEFEC,:P_DESMOTIVO,:P_NCODE, :P_SMESSAGE);
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
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return output;
        }

        public Dictionary<string, dynamic> UpdateStatusAlert(dynamic param)
        {
            try
            {
                //AlertStatusResponseDTO result = new AlertStatusResponseDTO ();
                Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.alertId, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.periodId, ParameterDirection.Input);
                OracleParameter P_SESTADO = new OracleParameter("P_SESTADO", OracleDbType.Varchar2, param.status, ParameterDirection.Input);
                OracleParameter P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int32, param.regimeId, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;
                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, P_SESTADO, P_NIDREGIMEN, P_NCODE, P_SMESSAGE };

                var query = @"
                 BEGIN
                     LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_ALERTA_PERIODO(:P_NIDALERTA, :P_NPERIODO_PROCESO,:P_SESTADO,:P_NIDREGIMEN,:P_NCODE, :P_SMESSAGE);
                 END;
                 ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                output["nCode"] = Int32.Parse(P_NCODE.Value.ToString());
                if (output["nCode"] != 0)
                {
                    output["sMessage"] = P_SMESSAGE.Value.ToString();
                }
                this.context.Database.CloseConnection();
                Console.WriteLine("/n El output : " + output);
                return output;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Utils.ExceptionManager.resolve(ex);
                return null;
            }
        }

        public Dictionary<string, dynamic> GetAnulacionAlerta(dynamic param)
        {
            try
            {
                //AlertStatusResponseDTO result = new AlertStatusResponseDTO ();
                Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int32, param.NIDREGIMEN, ParameterDirection.Input);
                OracleParameter P_IND_ANULA = new OracleParameter("P_IND_ANULA", OracleDbType.Int32, ParameterDirection.Output);
                P_IND_ANULA.Size = 4000;
                // P_SMESSAGE.Size = 4000;
                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NIDREGIMEN, P_IND_ANULA };

                var query = @"
                 BEGIN
                     LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_VAL_ANULACION_ALERTA(:P_NIDALERTA, :P_NIDREGIMEN, :P_IND_ANULA);
                 END;
                 ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                output["P_IND_ANULA"] = Int32.Parse(P_IND_ANULA.Value.ToString());
                if (output["P_IND_ANULA"] != 0)
                {
                    output["sMessage"] = "No grabo correctamente";
                }
                this.context.Database.CloseConnection();
                Console.WriteLine("/n El output : " + output);
                return output;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("Error :" + ex);
                Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                item["menesajeError"] =
                item["mensajeErrorDEtalle"] = ex;

                // this.context.Database.CloseConnection ();
                Utils.ExceptionManager.resolve(ex);
                return item;
            }
        }

        public Dictionary<string, dynamic> DeleteAdjuntosInformAlerta(dynamic param)
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                //AlertStatusResponseDTO result = new AlertStatusResponseDTO ();

                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NREGIMEN = new OracleParameter("P_NREGIMEN", OracleDbType.Int32, param.NREGIMEN, ParameterDirection.Input);
                //OracleParameter P_SESTADO = new OracleParameter("P_SESTADO", OracleDbType.Varchar2, param.status, ParameterDirection.Input);
                //OracleParameter P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int32, param.regimeId, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;
                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NREGIMEN, P_NCODE, P_SMESSAGE };

                var query = @"
                 BEGIN
                     LAFT.PKG_LAFT_GESTION_ALERTAS.SP_DEL_ADJ_INFORM_ALERTA(:P_NIDALERTA, :P_NREGIMEN,:P_NCODE, :P_SMESSAGE);
                 END;
                 ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                output["nCode"] = Int32.Parse(P_NCODE.Value.ToString());
                output["sMessage"] = P_SMESSAGE.Value.ToString();

                this.context.Database.CloseConnection();
                Console.WriteLine("/n El output : " + output);
                return output;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Utils.ExceptionManager.resolve(ex);
                output["nCode"] = 2;
                output["sMessageError"] = ex.Message.ToString();
                output["sMessageErrorDetalle"] = ex;
                return output;
            }
        }

        public Dictionary<string, dynamic> UpdateRevisedState(RevisedParamDTO param)
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_NIDALERTA_CAB_USUARIO = new OracleParameter("P_NIDALERTA_CAB_USUARIO", OracleDbType.Int32, param.NIDALERTA_CAB_USUARIO, ParameterDirection.Input);
                OracleParameter P_SESTADO_REVISADO = new OracleParameter("P_SESTADO_REVISADO", OracleDbType.Varchar2, param.SESTADO_REVISADO, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                OracleParameter P_DFECHA_REVISADO = new OracleParameter("P_DFECHA_REVISADO", OracleDbType.Date, ParameterDirection.Output);
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;
                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA_CAB_USUARIO, P_SESTADO_REVISADO, P_NCODE, P_DFECHA_REVISADO, P_SMESSAGE };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_EST_REVISADO_CAB(:P_NIDALERTA_CAB_USUARIO, :P_SESTADO_REVISADO, :P_DFECHA_REVISADO, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                output["nCode"] = P_NCODE.Value;
                if (output["nCode"] != 0)
                {
                    output["sMessage"] = P_SMESSAGE.Value;
                }
                else
                {
                    output["dFechaRevisado"] = P_DFECHA_REVISADO.Value;
                }
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return output;

        }

        public List<Dictionary<string, dynamic>> GetCommentsHeader(CommentsHeaderParamDTO param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NIDALERTA_CAB_USUARIO = new OracleParameter("P_NIDALERTA_CAB_USUARIO", OracleDbType.Int32, param.NIDALERTA_CAB_USUARIO, ParameterDirection.Input);
                OracleParameter P_STIPO_USUARIO = new OracleParameter("P_STIPO_USUARIO", OracleDbType.Varchar2, param.STIPO_USUARIO, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA_CAB_USUARIO, P_STIPO_USUARIO, RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_COMENTARIO_CABECERA(:P_NIDALERTA_CAB_USUARIO, :P_STIPO_USUARIO, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["DFECHA_REGISTRO"] = odr["DFECHA_REGISTRO"];
                    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"];
                    item["SCOMENTARIO"] = odr["SCOMENTARIO"];
                    item["STIPO_USUARIO"] = odr["STIPO_USUARIO"];
                    item["STIPO_MENSAJE"] = odr["STIPO_MENSAJE"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }

        public Dictionary<string, dynamic> InsertCommentHeader(CommentsHeaderParamDTO param)
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_NIDALERTA_CAB_USUARIO = new OracleParameter("P_NIDALERTA_CAB_USUARIO", OracleDbType.Int32, param.NIDALERTA_CAB_USUARIO, ParameterDirection.Input);
                OracleParameter P_SCOMENTARIO = new OracleParameter("P_SCOMENTARIO", OracleDbType.Varchar2, param.SCOMENTARIO, ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_MODIFICA = new OracleParameter("P_NIDUSUARIO_MODIFICA", OracleDbType.Int32, param.NIDUSUARIO_MODIFICA, ParameterDirection.Input);
                OracleParameter P_STIPO_USUARIO = new OracleParameter("P_STIPO_USUARIO", OracleDbType.Varchar2, param.STIPO_USUARIO, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;
                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA_CAB_USUARIO, P_SCOMENTARIO, P_NIDUSUARIO_MODIFICA, P_STIPO_USUARIO, P_NCODE, P_SMESSAGE };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_INS_COMENTARIO_CAB_USU(:P_NIDALERTA_CAB_USUARIO, :P_SCOMENTARIO, :P_NIDUSUARIO_MODIFICA, :P_STIPO_USUARIO, :P_NCODE, :P_SMESSAGE);
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
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return output;
        }

        public Dictionary<string, dynamic> UpdateAttachFiles(dynamic param)
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            return output;
        }

        public List<Dictionary<string, dynamic>> GetWorkModuleList(dynamic param)
        { //WorkModuleParamDTO param) {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int32, param.NIDREGIMEN, ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int32, param.NIDGRUPOSENAL, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDREGIMEN, P_NIDGRUPOSENAL, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_MODULO_TRABAJO(:P_NPERIODO_PROCESO,:P_NIDREGIMEN, :P_NIDGRUPOSENAL, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["SNOMBRE_ALERTA"] = odr["SNOMBRE_ALERTA"];
                    item["SDESCRIPCION_ALERTA"] = odr["SDESCRIPCION_ALERTA"];
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["SNOMBRE_ESTADO"] = odr["SNOMBRE_ESTADO"];
                    item["NIDALERTA"] = odr["NIDALERTA"];
                    item["SESTADO"] = odr["SESTADO"];
                    item["NIDREGIMEN"] = odr["NIDREGIMEN"];
                    item["ULTFECREV"] = odr["ULTFECREV"];

                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("EL EX : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }

        public List<Dictionary<string, dynamic>> GetWorkModuleDetail(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int32, param.NIDREGIMEN, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDALERTA, P_NIDREGIMEN, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_DETALLE_MODULO_TRABAJO(:P_NPERIODO_PROCESO, :P_NIDALERTA,:P_NIDREGIMEN, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"];
                    item["SCARGO"] = odr["SCARGO"];
                    item["DFECHA_ESTADO_MOVIMIENTO"] = odr["DFECHA_ESTADO_MOVIMIENTO"];
                    item["SNOMBRE_ESTADO"] = odr["SNOMBRE_ESTADO"];
                    item["NIDALERTA_CABECERA"] = odr["NIDALERTA_CAB_USUARIO"];
                    item["NIDAGRUPA"] = odr["NIDAGRUPA"];
                    item["NIDALERTA"] = odr["NIDALERTA"];
                    item["NIDUSUARIO_ASIGNADO"] = odr["NIDUSUARIO_ASIGNADO"];
                    item["SESTADO"] = odr["SESTADO"];
                    item["NRESPUESTA"] = odr["NRESPUESTA"];
                    item["SCOMENTARIO"] = odr["SCOMENTARIO"] == DBNull.Value ? string.Empty : odr["SCOMENTARIO"].ToString(); ;
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                throw ex;
            }
            return lista;
        }

        public List<Dictionary<string, dynamic>> GetProductsCompany()
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_PRODUCTOS(:RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NPRODUCT"] = odr["NPRODUCT"];
                    item["SDESCRIPT"] = odr["SDESCRIPT"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }

        public Dictionary<string, dynamic> InsertCompanyDetailUser(CompanyDetailUserDTO param)
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDALERTA_CAB_USUARIO = new OracleParameter("P_NIDALERTA_CAB_USUARIO", OracleDbType.Int32, param.NIDALERTA_CABECERA, ParameterDirection.Input);
                OracleParameter P_SNUMERO_DOC = new OracleParameter("P_SNUMERO_DOC", OracleDbType.Varchar2, param.SRUC, ParameterDirection.Input);
                OracleParameter P_SRAZON_SOCIAL = new OracleParameter("P_SRAZON_SOCIAL", OracleDbType.Varchar2, param.SNOMBRE_CLIENTE, ParameterDirection.Input);
                OracleParameter P_NPRODUCT = new OracleParameter("P_NPRODUCT", OracleDbType.Int32, param.NPRODUCT, ParameterDirection.Input);
                OracleParameter P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int32, param.NIDREGIMEN, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;
                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, P_NIDALERTA_CAB_USUARIO, P_SNUMERO_DOC, P_SRAZON_SOCIAL, P_NPRODUCT, P_NIDREGIMEN, RC1, P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_INS_EMPRESA_DETALLE_USUARIO(:P_NIDALERTA, :P_NPERIODO_PROCESO, :P_NIDALERTA_CAB_USUARIO, 1, :P_SNUMERO_DOC, NULL, NULL, NULL, NULL, 
                                :P_SRAZON_SOCIAL, NULL, NULL, :P_NPRODUCT, :P_NIDREGIMEN, :RC1, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();

                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NIDALERTA_DET"] = odr["NIDALERTA_DET"];
                    lista.Add(item);

                }
                odr.Close();
                output["nCode"] = P_NCODE.Value.ToString();
                output["lista"] = lista;
                output["sMessage"] = P_SMESSAGE.Value.ToString();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return output;
        }

        public Dictionary<string, dynamic> InsertAttachedFiles(dynamic param)
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_NIDALERTA_CAB_USUARIO = new OracleParameter("P_NIDALERTA_CAB_USUARIO", OracleDbType.Int64, param.NIDALERTA_CABECERA, ParameterDirection.Input);
                OracleParameter P_SRUTA_ADJUNTO = new OracleParameter("P_SRUTA_ADJUNTO", OracleDbType.Varchar2, param.SRUTA_ADJUNTO, ParameterDirection.Input);
                OracleParameter P_STIPO_USUARIO = new OracleParameter("P_STIPO_USUARIO", OracleDbType.Varchar2, param.STIPO_USUARIO, ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_MODIFICA = new OracleParameter("P_NIDUSUARIO_MODIFICA", OracleDbType.Int64, param.NIDUSUARIO_ASIGNADO, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA_CAB_USUARIO, P_SRUTA_ADJUNTO, P_STIPO_USUARIO, P_NIDUSUARIO_MODIFICA, P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_INS_ADJUNTO_CABECERA(:P_NIDALERTA_CAB_USUARIO, :P_SRUTA_ADJUNTO, :P_STIPO_USUARIO, :P_NIDUSUARIO_MODIFICA, :P_NCODE, :P_SMESSAGE);
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
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el exception en ins_adjunto_cabecea : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return output;
        }



        public List<Dictionary<string, dynamic>> GetAttachedFiles(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NIDALERTA_CAB_USUARIO = new OracleParameter("P_NIDALERTA_CAB_USUARIO", OracleDbType.Int32, param.NIDALERTA_CAB_USUARIO, ParameterDirection.Input);
                OracleParameter P_STIPO_USUARIO = new OracleParameter("P_STIPO_USUARIO", OracleDbType.Varchar2, param.STIPO_USUARIO, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA_CAB_USUARIO, P_STIPO_USUARIO, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_ADJUNTO_CABECERA(:P_NIDALERTA_CAB_USUARIO, :P_STIPO_USUARIO, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NIDALERTA_CAB_USUARIO"] = odr["NIDALERTA_CAB_USUARIO"];
                    item["NIDADJUNTO"] = odr["NIDADJUNTO"];
                    item["SRUTA_ADJUNTO"] = odr["SRUTA_ADJUNTO"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }

        public Dictionary<string, dynamic> SendComplimentary(dynamic param)
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_NIDALERTA_CAB_USUARIO = new OracleParameter("P_NIDALERTA_CAB_USUARIO", OracleDbType.Int32, param.NIDALERTA_CAB_USUARIO, ParameterDirection.Input);
                OracleParameter P_SCOMENTARIO = new OracleParameter("P_SCOMENTARIO", OracleDbType.Varchar2, param.SCOMENTARIO, ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_MODIFICA = new OracleParameter("P_NIDUSUARIO_MODIFICA", OracleDbType.Int32, param.NIDUSUARIO_MODIFICA, ParameterDirection.Input);
                OracleParameter P_STIPO_USUARIO = new OracleParameter("P_STIPO_USUARIO", OracleDbType.Varchar2, param.STIPO_USUARIO, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA_CAB_USUARIO, P_SCOMENTARIO, P_NIDUSUARIO_MODIFICA, P_STIPO_USUARIO, P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_INS_COMENTARIO_CAB_USU(:P_NIDALERTA_CAB_USUARIO, :P_SCOMENTARIO, :P_NIDUSUARIO_MODIFICA, :P_STIPO_USUARIO, :P_NCODE, :P_SMESSAGE);
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
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return output;
        }

        public List<Dictionary<string, dynamic>> GetGafiList()
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_GAFI(:RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["SNOMBRE_PAIS_GAFI"] = odr["SNOMBRE_PAIS_GAFI"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }

        public List<Dictionary<string, dynamic>> GetSignalList(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int32, param.NIDREGIMEN, ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int32, param.NIDGRUPOSENAL, ParameterDirection.Input);

                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDALERTA, P_NIDREGIMEN, P_NIDGRUPOSENAL, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_RESULTADO(:P_NPERIODO_PROCESO, :P_NIDALERTA, :P_NIDREGIMEN, :P_NIDGRUPOSENAL, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["STIPOIDEN"] = odr["STIPOIDEN"];
                    item["SNUM_DOCUMENTO"] = odr["SNUM_DOCUMENTO"];
                    item["SNOM_COMPLETO"] = odr["SNOM_COMPLETO"];
                    item["SDESTIPOLISTA"] = odr["SDESTIPOLISTA"];
                    item["SDESESTADO"] = odr["SDESESTADO"];
                    item["DFECHA_NACIMIENTO"] = odr["DFECHA_NACIMIENTO"];
                    item["EDAD"] = odr["EDAD"];
                    item["SOCUPACION"] = odr["SOCUPACION"];
                    item["SCARGO"] = odr["SCARGO"];
                    item["SZONA_GEO"] = odr["SZONA_GEO"];
                    item["NTIPO_DOCUMENTO"] = odr["NTIPO_DOCUMENTO"];
                    item["DFECHA_REVISADO"] = odr["DFECHA_REVISADO"];
                    item["SESTADO_REVISADO"] = odr["SESTADO_REVISADO"];
                    item["NIDSUBGRUPOSEN"] = odr["NIDSUBGRUPOSEN"];
                    item["SDESSUBGRUPO_SENAL"] = odr["SDESSUBGRUPO_SENAL"];
                    item["SNUM_DOCUMENTO_EMPRESA"] = odr["SNUM_DOCUMENTO_EMPRESA"];
                    item["SNOM_COMPLETO_EMPRESA"] = odr["SNOM_COMPLETO_EMPRESA"];
                    item["NACIONALIDAD"] = odr["NACIONALIDAD"];
                    item["CARGO"] = odr["CARGO"];

                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.Write("el ex 1224:" + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }

        public Dictionary<string, dynamic> UpdateStatusToReviewed(dynamic param)
        {
            var output = new Dictionary<string, dynamic>();
            try
            {
                var P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                var P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, param.NIDALERTA, ParameterDirection.Input);
                var P_NTIPOIDEN_BUSQ = new OracleParameter("P_NTIPOIDEN_BUSQ", OracleDbType.Varchar2, param.NTIPOIDEN_BUSQ, ParameterDirection.Input);
                var P_SNUM_DOCUMENTO_BUSQ = new OracleParameter("P_SNUM_DOCUMENTO_BUSQ", OracleDbType.Varchar2, param.SNUM_DOCUMENTO_BUSQ, ParameterDirection.Input);
                var P_SESTADO_REVISADO = new OracleParameter("P_SESTADO_REVISADO", OracleDbType.Varchar2, param.SESTADO_REVISADO, ParameterDirection.Input);
                var P_NIDUSUARIO_REVISADO = new OracleParameter("P_NIDUSUARIO_REVISADO", OracleDbType.Int64, param.NIDUSUARIO_REVISADO, ParameterDirection.Input);
                var P_DFECHA_REVISADO = new OracleParameter("P_DFECHA_REVISADO", OracleDbType.Date, ParameterDirection.Output);
                var P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                var P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_DFECHA_REVISADO.Size = 4000;
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                var parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDALERTA, P_NTIPOIDEN_BUSQ, P_SNUM_DOCUMENTO_BUSQ, P_SESTADO_REVISADO, P_NIDUSUARIO_REVISADO, P_DFECHA_REVISADO, P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_EST_REVISADO_RES(  :P_NPERIODO_PROCESO, :P_NIDALERTA, :P_NTIPOIDEN_BUSQ, :P_SNUM_DOCUMENTO_BUSQ, :P_SESTADO_REVISADO, :P_NIDUSUARIO_REVISADO,:P_DFECHA_REVISADO, :P_NCODE, :P_SMESSAGE );
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                output["nCode"] = P_NCODE.Value;
                if (output["nCode"] != 0)
                {
                    output["sMessage"] = P_SMESSAGE.Value;
                }
                else
                {
                    output["dfechaRevisado"] = P_DFECHA_REVISADO.Value;
                }
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }
            return output;
        }

        public List<Dictionary<string, dynamic>> GetRegimeList()
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_REGIMEN(:RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NIDREGIMEN"] = odr["NIDREGIMEN"];
                    item["SNOMBRE"] = odr["SDESREGIMEN"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }

        public Dictionary<string, dynamic> GetCurrentPeriod()
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_DFEEJECUTAPROCFIN = new OracleParameter("P_DFEEJECUTAPROCFIN", OracleDbType.Varchar2, ParameterDirection.Output);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;
                P_DFEEJECUTAPROCFIN.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_DFEEJECUTAPROCFIN, P_NCODE, P_SMESSAGE };

                var query = @"
               BEGIN
                   LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_PERIODO_VIGENTE(:P_NPERIODO_PROCESO, :P_DFEEJECUTAPROCFIN, :P_NCODE, :P_SMESSAGE);
               END;
               ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                output["periodo"] = Int32.Parse(P_NPERIODO_PROCESO.Value.ToString());

                output["fechaEjecFin"] = P_DFEEJECUTAPROCFIN.Value.ToString();
                output["nCode"] = Int32.Parse(P_NCODE.Value.ToString());
                if (output["nCode"] != 0)
                {
                    output["sMessage"] = P_SMESSAGE.Value.ToString();
                }
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el error : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return output;
        }

        public List<Dictionary<string, dynamic>> GetAlertReportList(dynamic param)
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {
                var P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, Convert.ToInt64(param.NIDALERTA), ParameterDirection.Input);
                var P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int64, Convert.ToInt64(param.NIDREGIMEN), ParameterDirection.Input);
                var P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, Convert.ToInt64(param.NPERIODO_PROCESO), ParameterDirection.Input);
                var P_NIDUSUARIO_ASIGNADO = new OracleParameter("P_NIDUSUARIO_ASIGNADO", OracleDbType.Int64, Convert.ToInt64(param.NIDUSUARIO_ASIGNADO), ParameterDirection.Input);
                var P_NIDALERTA_ORI = new OracleParameter("P_NIDALERTA_ORI", OracleDbType.Int64, Convert.ToInt64(param.P_NIDALERTA_ORI), ParameterDirection.Input);
                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);
                var parameters = new OracleParameter[] { P_NIDALERTA, P_NIDREGIMEN, P_NPERIODO_PROCESO, P_NIDUSUARIO_ASIGNADO, P_NIDALERTA_ORI, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_ALERTA_INFORME(:P_NIDALERTA, :P_NIDREGIMEN, :P_NPERIODO_PROCESO, :P_NIDUSUARIO_ASIGNADO, :P_NIDALERTA_ORI, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["SRESPUESTA"] = odr["SRESPUESTA"];
                    item["NIDUSUARIO_ASIGNADO"] = odr["NIDUSUARIO_ASIGNADO"];
                    item["ETIQUETA"] = odr["ETIQUETA"];
                    item["NIDREGIMEN"] = odr["NIDREGIMEN"];
                    item["SOBLIGA_USUARIO"] = odr["SOBLIGA_USUARIO"];
                    item["NCANTIDAD"] = odr["NCANTIDAD"];
                    item["SNOM_USUARIO_ASIG"] = odr["SNOM_USUARIO_ASIG"] == DBNull.Value ? string.Empty : odr["SNOM_USUARIO_ASIG"].ToString();
                    item["SPERFIL"] = odr["SPERFIL"] == DBNull.Value ? string.Empty : odr["SPERFIL"].ToString();
                    // item["SCOMENTARIO"] = odr["SCOMENTARIO"] == DBNull.Value ? string.Empty : odr["SCOMENTARIO"].ToString ();
                    lista.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }
            return lista;
        }
        public List<Dictionary<string, dynamic>> GetInternationalLists(dynamic param)
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {
                var P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, param.NIDALERTA, ParameterDirection.Input);
                var P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                var P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int64, param.NIDREGIMEN, ParameterDirection.Input);
                var P_STIPOIDEN_BUSQ = new OracleParameter("P_STIPOIDEN_BUSQ", OracleDbType.Varchar2, param.STIPOIDEN_BUSQ, ParameterDirection.Input);
                var P_SNUM_DOCUMENTO_BUSQ = new OracleParameter("P_SNUM_DOCUMENTO_BUSQ", OracleDbType.Varchar2, param.SNUM_DOCUMENTO_BUSQ, ParameterDirection.Input);
                var P_NIDSUBGRUPOSENAL = new OracleParameter("P_NIDSUBGRUPOSENAL", OracleDbType.Int32, param.NIDSUBGRUPOSENAL, ParameterDirection.Input);
                var P_NTIPOCARGA = new OracleParameter("P_NTIPOCARGA", OracleDbType.Int32, param.NTIPOCARGA, ParameterDirection.Input);
                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, P_NIDREGIMEN, P_STIPOIDEN_BUSQ, P_SNUM_DOCUMENTO_BUSQ, P_NIDSUBGRUPOSENAL, P_NTIPOCARGA, RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_RESULTADO_LI(:P_NIDALERTA, :P_NPERIODO_PROCESO, :P_NIDREGIMEN, :P_STIPOIDEN_BUSQ, :P_SNUM_DOCUMENTO_BUSQ,:P_NIDSUBGRUPOSENAL,:P_NTIPOCARGA, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["STIPOIDEN"] = odr["STIPOIDEN"];
                    item["SNUM_DOCUMENTO"] = odr["SNUM_DOCUMENTO"];
                    item["SNOM_COMPLETO"] = odr["SNOM_COMPLETO"];
                    item["STERMINO_COINCIDENCIA"] = odr["STERMINO_COINCIDENCIA"];
                    item["SORIGEN"] = odr["SORIGEN"];
                    item["NPORC_APROXIMA_BUSQ"] = odr["NPORC_APROXIMA_BUSQ"];
                    item["NACEPTA_COINCIDENCIA"] = odr["NACEPTA_COINCIDENCIA"];
                    item["NIDRESULTADO"] = odr["NIDRESULTADO"];
                    item["NIDTIPOLISTA"] = odr["NIDTIPOLISTA"];
                    item["NIDPROVEEDOR"] = odr["NIDPROVEEDOR"];
                    item["STIPO_BUSQUEDA"] = odr["STIPO_BUSQUEDA"];
                    item["SCLIENT"] = odr["SCLIENT"];
                    item["SESTADO_TRAT"] = odr["SESTADO_TRAT"];
                    item["NIDREGIMEN"] = odr["NIDREGIMEN"];
                    item["SDESREGIMEN"] = odr["SDESREGIMEN"];
                    item["NTIPOCARGA"] = odr["NTIPOCARGA"];
                    item["SESTADO_REVISADO"] = odr["SESTADO_REVISADO"];
                    lista.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }
            return lista;
        }

        public List<Dictionary<string, dynamic>> getListaResultadosCoincid(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, System.Data.ParameterDirection.Input);

                OracleParameter P_STIPOIDEN_BUSQ = new OracleParameter("P_STIPOIDEN_BUSQ", OracleDbType.Varchar2, param.STIPOIDEN_BUSQ, System.Data.ParameterDirection.Input);
                OracleParameter P_SNUM_DOCUMENTO_BUSQ = new OracleParameter("P_SNUM_DOCUMENTO_BUSQ", OracleDbType.Varchar2, param.SNUM_DOCUMENTO_BUSQ, System.Data.ParameterDirection.Input);

                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, P_STIPOIDEN_BUSQ, P_SNUM_DOCUMENTO_BUSQ, RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_RESULTADO_COINC_GC(:P_NIDALERTA, :P_NPERIODO_PROCESO, :P_STIPOIDEN_BUSQ, :P_SNUM_DOCUMENTO_BUSQ, :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["STIPOIDEN"] = odr["STIPOIDEN"];
                    item["SNUM_DOCUMENTO"] = odr["SNUM_DOCUMENTO"];
                    item["SNOM_COMPLETO"] = odr["SNOM_COMPLETO"];
                    item["STERMINO_COINCIDENCIA"] = odr["STERMINO_COINCIDENCIA"];
                    item["SORIGEN"] = odr["SORIGEN"];
                    item["NPORC_APROXIMA_BUSQ"] = odr["NPORC_APROXIMA_BUSQ"];
                    item["NACEPTA_COINCIDENCIA"] = odr["NACEPTA_COINCIDENCIA"];
                    item["SESTADO_REVISADO"] = odr["SESTADO_REVISADO"];
                    item["NIDRESULTADO"] = odr["NIDRESULTADO"];
                    item["NIDTIPOLISTA"] = odr["NIDTIPOLISTA"];
                    item["NIDPROVEEDOR"] = odr["NIDPROVEEDOR"];
                    item["SDESPROVEEDOR"] = odr["SDESPROVEEDOR"];
                    item["STIPO_BUSQUEDA"] = odr["STIPO_BUSQUEDA"];
                    item["SDESTIPOLISTA"] = odr["SDESTIPOLISTA"];
                    item["SCLIENT"] = odr["SCLIENT"];
                    item["SESTADO_TRAT"] = odr["SESTADO_TRAT"];
                    item["NIDREGIMEN"] = odr["NIDREGIMEN"];
                    item["SDESREGIMEN"] = odr["SDESREGIMEN"];
                    item["SNOMCARGO"] = odr["SNOMCARGO"];
                    item["NTIPOCARGA"] = odr["SNOMCARGO"];

                    lista.Add(item);
                }
                odr.Close();
                /*
                odr = ((OracleRefCursor) RC2.Value).GetDataReader();
                if (odr.HasRows) {
                    while(odr.Read()) {
                    }
                }
                */
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine(" hola : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }

        public List<Dictionary<string, dynamic>> GetPepList(dynamic param)
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {
                var P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, param.NIDALERTA, ParameterDirection.Input);
                var P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                var P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int64, param.NIDREGIMEN, ParameterDirection.Input);
                var P_STIPOIDEN_BUSQ = new OracleParameter("P_STIPOIDEN_BUSQ", OracleDbType.Varchar2, param.STIPOIDEN_BUSQ, ParameterDirection.Input);
                var P_SNUM_DOCUMENTO_BUSQ = new OracleParameter("P_SNUM_DOCUMENTO_BUSQ", OracleDbType.Varchar2, param.SNUM_DOCUMENTO_BUSQ, ParameterDirection.Input);
                var P_NIDSUBGRUPOSENAL = new OracleParameter("P_NIDSUBGRUPOSENAL", OracleDbType.Int32, param.NIDSUBGRUPOSENAL, ParameterDirection.Input);
                var P_NTIPOCARGA = new OracleParameter("P_NTIPOCARGA", OracleDbType.Int32, param.NTIPOCARGA, ParameterDirection.Input);
                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, P_NIDREGIMEN, P_STIPOIDEN_BUSQ, P_SNUM_DOCUMENTO_BUSQ, P_NIDSUBGRUPOSENAL, P_NTIPOCARGA, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_RESULTADO_PEP(:P_NIDALERTA, :P_NPERIODO_PROCESO, :P_NIDREGIMEN, :P_STIPOIDEN_BUSQ, :P_SNUM_DOCUMENTO_BUSQ,:P_NIDSUBGRUPOSENAL,:P_NTIPOCARGA, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["STIPOIDEN"] = odr["STIPOIDEN"];
                    item["SNUM_DOCUMENTO"] = odr["SNUM_DOCUMENTO"];
                    item["SNOM_COMPLETO"] = odr["SNOM_COMPLETO"];
                    item["STERMINO_COINCIDENCIA"] = odr["STERMINO_COINCIDENCIA"];
                    item["SORIGEN"] = odr["SORIGEN"];
                    item["NPORC_APROXIMA_BUSQ"] = odr["NPORC_APROXIMA_BUSQ"];
                    item["NACEPTA_COINCIDENCIA"] = odr["NACEPTA_COINCIDENCIA"];
                    item["NIDRESULTADO"] = odr["NIDRESULTADO"];
                    item["NIDTIPOLISTA"] = odr["NIDTIPOLISTA"];
                    item["NIDPROVEEDOR"] = odr["NIDPROVEEDOR"];
                    item["STIPO_BUSQUEDA"] = odr["STIPO_BUSQUEDA"];
                    item["SCLIENT"] = odr["SCLIENT"];
                    item["SESTADO_TRAT"] = odr["SESTADO_TRAT"];
                    item["NIDREGIMEN"] = odr["NIDREGIMEN"];
                    item["SDESREGIMEN"] = odr["SDESREGIMEN"];
                    item["NTIPOCARGA"] = odr["NTIPOCARGA"];
                    item["SESTADO_REVISADO"] = odr["SESTADO_REVISADO"];
                    item["SCARGO_PEP"] = odr["SCARGO_PEP"];
                    item["NIDCARGOPEP"] = odr["NIDCARGOPEP"];
                    item["SNOMCARGO"] = odr["SNOMCARGO"];



                    lista.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }
            return lista;
        }

        public List<Dictionary<string, dynamic>> GetFamiliesPepList(dynamic param)
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {
                var P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, param.NIDALERTA, ParameterDirection.Input);
                var P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                var P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int64, param.NIDREGIMEN, ParameterDirection.Input);
                var P_STIPOIDEN_BUSQ = new OracleParameter("P_STIPOIDEN_BUSQ", OracleDbType.Varchar2, param.STIPOIDEN_BUSQ, ParameterDirection.Input);
                var P_SNUM_DOCUMENTO_BUSQ = new OracleParameter("P_SNUM_DOCUMENTO_BUSQ", OracleDbType.Varchar2, param.SNUM_DOCUMENTO_BUSQ, ParameterDirection.Input);
                var P_NIDSUBGRUPOSENAL = new OracleParameter("P_NIDSUBGRUPOSENAL", OracleDbType.Int32, param.NIDSUBGRUPOSENAL, ParameterDirection.Input);
                var P_NTIPOCARGA = new OracleParameter("P_NTIPOCARGA", OracleDbType.Int32, param.NTIPOCARGA, ParameterDirection.Input);
                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, P_NIDREGIMEN, P_STIPOIDEN_BUSQ, P_SNUM_DOCUMENTO_BUSQ, P_NIDSUBGRUPOSENAL, P_NTIPOCARGA, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_RESULTADO_FPEP(:P_NIDALERTA, :P_NPERIODO_PROCESO, :P_NIDREGIMEN, :P_STIPOIDEN_BUSQ, :P_SNUM_DOCUMENTO_BUSQ,:P_NIDSUBGRUPOSENAL,:P_NTIPOCARGA,:RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["STIPOIDEN"] = odr["STIPOIDEN"];
                    item["SNUM_DOCUMENTO"] = odr["SNUM_DOCUMENTO"];
                    item["SNOM_COMPLETO"] = odr["SNOM_COMPLETO"];
                    item["SORIGEN"] = odr["SORIGEN"];
                    item["NPORC_APROXIMA_BUSQ"] = odr["NPORC_APROXIMA_BUSQ"];
                    item["NACEPTA_COINCIDENCIA"] = odr["NACEPTA_COINCIDENCIA"];
                    item["NIDRESULTADO"] = odr["NIDRESULTADO"];
                    item["NIDTIPOLISTA"] = odr["NIDTIPOLISTA"];
                    item["NIDPROVEEDOR"] = odr["NIDPROVEEDOR"];
                    item["STIPO_BUSQUEDA"] = odr["STIPO_BUSQUEDA"];
                    item["SCLIENT"] = odr["SCLIENT"];
                    item["SESTADO_TRAT"] = odr["SESTADO_TRAT"];
                    item["NIDREGIMEN"] = odr["NIDREGIMEN"];
                    item["SDESREGIMEN"] = odr["SDESREGIMEN"];
                    item["NTIPOCARGA"] = odr["NTIPOCARGA"];
                    item["SESTADO_REVISADO"] = odr["SESTADO_REVISADO"];
                    lista.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }

            return lista;

        }

        public List<Dictionary<string, dynamic>> GetSacList(dynamic param)
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {
                var P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, param.NIDALERTA, ParameterDirection.Input);
                var P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                var P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int64, param.NIDREGIMEN, ParameterDirection.Input);
                var P_STIPOIDEN_BUSQ = new OracleParameter("P_STIPOIDEN_BUSQ", OracleDbType.Varchar2, param.STIPOIDEN_BUSQ, ParameterDirection.Input);
                var P_SNUM_DOCUMENTO_BUSQ = new OracleParameter("P_SNUM_DOCUMENTO_BUSQ", OracleDbType.Varchar2, param.SNUM_DOCUMENTO_BUSQ, ParameterDirection.Input);
                var P_NIDSUBGRUPOSENAL = new OracleParameter("P_NIDSUBGRUPOSENAL", OracleDbType.Int32, param.NIDSUBGRUPOSENAL, ParameterDirection.Input);
                var P_NTIPOCARGA = new OracleParameter("P_NTIPOCARGA", OracleDbType.Int32, param.NTIPOCARGA, ParameterDirection.Input);
                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, P_NIDREGIMEN, P_STIPOIDEN_BUSQ, P_SNUM_DOCUMENTO_BUSQ, P_NIDSUBGRUPOSENAL, P_NTIPOCARGA, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_RESULTADO_SAC(:P_NIDALERTA, :P_NPERIODO_PROCESO, :P_NIDREGIMEN, :P_STIPOIDEN_BUSQ, :P_SNUM_DOCUMENTO_BUSQ, :P_NIDSUBGRUPOSENAL,:P_NTIPOCARGA, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["STIPOIDEN"] = odr["STIPOIDEN"];
                    item["SNUM_DOCUMENTO"] = odr["SNUM_DOCUMENTO"];
                    item["SNOM_COMPLETO"] = odr["SNOM_COMPLETO"];
                    item["SORIGEN"] = odr["SORIGEN"];
                    item["NPORC_APROXIMA_BUSQ"] = odr["NPORC_APROXIMA_BUSQ"];
                    item["NACEPTA_COINCIDENCIA"] = odr["NACEPTA_COINCIDENCIA"];
                    item["NIDRESULTADO"] = odr["NIDRESULTADO"];
                    item["NIDTIPOLISTA"] = odr["NIDTIPOLISTA"];
                    item["NIDPROVEEDOR"] = odr["NIDPROVEEDOR"];
                    item["STIPO_BUSQUEDA"] = odr["STIPO_BUSQUEDA"];
                    item["SCLIENT"] = odr["SCLIENT"];
                    item["SESTADO_TRAT"] = odr["SESTADO_TRAT"];
                    item["NIDREGIMEN"] = odr["NIDREGIMEN"];
                    item["SDESREGIMEN"] = odr["SDESREGIMEN"];
                    item["NTIPOCARGA"] = odr["NTIPOCARGA"];
                    item["SESTADO_REVISADO"] = odr["SESTADO_REVISADO"];
                    lista.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }

            return lista;

        }

        internal Es10Response GetListaEs10(dynamic param)
        {
            Es10Response item = new Es10Response();
            List<Es10Response> lista = new List<Es10Response>();
            try
            {
                var P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);
                var RC2 = new OracleParameter("RC2", OracleDbType.RefCursor, ParameterDirection.Output);
                var RC3 = new OracleParameter("RC3", OracleDbType.RefCursor, ParameterDirection.Output);
                var RC4 = new OracleParameter("RC4", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { P_NPERIODO_PROCESO, RC1, RC2, RC3, RC4 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_ES10(:P_NPERIODO_PROCESO, :RC1 , :RC2 , :RC3, :RC4);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                item.Es10 = new List<Es10Entity>();
                while (odr.Read())
                {
                    Es10Entity _item = new Es10Entity();
                    _item.nPeriodoProceso = int.Parse(odr["NPERIODO_PROCESO"].ToString());
                    _item.sRamo = odr["SRAMO"].ToString();
                    _item.sRiesgo = odr["SRIESGO"].ToString();
                    _item.nCodRiesgo = int.Parse(odr["NCOD_RIESGO"].ToString());
                    _item.sCodRegistro = odr["SCOD_REGISTRO"].ToString();
                    _item.sNomComercial = odr["SNOM_COMERCIAL"].ToString();
                    _item.sMoneda = odr["SMONEDA"].ToString();
                    _item.sFechaIniComercial = odr["DFEC_INI_COMERCIAL"].ToString();
                    _item.nCantAsegurados = int.Parse(odr["NCANT_ASEGURADOS"].ToString());
                    _item.sRegimen = odr["SREGIMEN"].ToString();
                    item.Es10.Add(_item);
                }
                item.riesgosFilter = new List<string>();
                var odr2 = ((OracleRefCursor)RC2.Value).GetDataReader();
                while (odr2.Read())
                {
                    item.riesgosFilter.Add(odr2["SRIESGO"].ToString());
                }
                item.polizaFilter = new List<string>();
                var odr3 = ((OracleRefCursor)RC3.Value).GetDataReader();
                while (odr3.Read())
                {
                    item.polizaFilter.Add(odr3["SNOM_COMERCIAL"].ToString());
                }
                item.monedaFilter = new List<string>();
                var odr4 = ((OracleRefCursor)RC4.Value).GetDataReader();
                while (odr4.Read())
                {
                    item.monedaFilter.Add(odr4["SMONEDA"].ToString());
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }

            return item;
        }

        internal List<Dictionary<string, dynamic>> GetKriListContratantes()
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {
                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { RC1 };

                var query = @"
                    BEGIN
                        INSUDB.PKG_BUSQ_COINCIDENCIAS_ALERTAS.SP_GET_KRI_LISTA_CONTRATANTES( :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["NRODOCUMENTO"] = odr["NRODOCUMENTO"];
                    item["NOMBRE_RAZONSOCIAL"] = odr["NOMBRE_RAZONSOCIAL"];
                    lista.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }

            return lista;
        }
        internal ZonaGeograficaResponse GetKriSearchZonaGeografica(dynamic param)
        {
            ZonaGeograficaResponse item = new ZonaGeograficaResponse();
            try
            {
                var P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);
                var RC2 = new OracleParameter("RC2", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { P_NPERIODO_PROCESO, RC1, RC2 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_ZONAS_GEOGRAFICA( :P_NPERIODO_PROCESO, :RC1 ,:RC2);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                item.ZonaGeografica = new List<ZonaGeograficaResponseEntity>();
                while (odr.Read())
                {
                    var _item = new ZonaGeograficaResponseEntity();
                    _item.rowId = odr["ROWID"].ToString();
                    _item.nPeriodoProceso = int.Parse(odr["NPERIODO_PROCESO"].ToString());
                    _item.sProducto = odr["COD_TIPPROD"].ToString();
                    _item.tipDoc = odr["TIP_DOC"].ToString();
                    _item.numDoc = odr["NUM_IDENBEN"].ToString();
                    _item.primerNombre = odr["NOMBEN"].ToString();
                    _item.segundoNombre = odr["NOMSEGBEN"].ToString();
                    _item.apellidoParterno = odr["PATBEN"].ToString();
                    _item.apellidoMaterno = odr["MATBEN"].ToString();
                    _item.region = odr["GLS_REGION"].ToString();
                    item.ZonaGeografica.Add(_item);
                }
                var odr2 = ((OracleRefCursor)RC2.Value).GetDataReader();
                item.departamentos = new List<string>();
                while (odr2.Read())
                {
                    item.departamentos.Add(odr2["GLS_REGION"].ToString());
                }
                odr.Close();
                odr2.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }

            return item;
        }
        internal List<Dictionary<string, dynamic>> GetKriListZonasGeograficas()
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {
                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { RC1 };

                var query = @"
                    BEGIN
                        INSUDB.PKG_BUSQ_COINCIDENCIAS_ALERTAS.SP_GET_LIST_ZONA_GEOGRAFICA( :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["COD_TIPPROD"] = odr["COD_TIPPROD"];
                    item["TIP_DOC"] = odr["TIP_DOC"];
                    item["NUM_IDENBEN"] = odr["NUM_IDENBEN"];
                    item["NOMBEN"] = odr["NOMBEN"];
                    item["NOMSEGBEN"] = odr["NOMSEGBEN"];
                    item["PATBEN"] = odr["PATBEN"];
                    item["MATBEN"] = odr["MATBEN"];
                    item["GLS_REGION"] = odr["GLS_REGION"];
                    lista.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }

            return lista;
        }

        public Dictionary<string, dynamic> delZonaGeografica(dynamic param)
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NCODE, P_SMESSAGE };

                var query = @"
               BEGIN
                   LAFT.PKG_LAFT_GESTION_ALERTAS.SP_DEL_ZONA_GEOGRAFICA(:P_NPERIODO_PROCESO, :P_NCODE, :P_SMESSAGE);
               END;
               ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                output["nCode"] = Int32.Parse(P_NCODE.Value.ToString());
                if (output["nCode"] != 0)
                {
                    output["sMessage"] = P_SMESSAGE.Value.ToString();
                }
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el error : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return output;
        }
        public Dictionary<string, dynamic> updateZonaGeografica(dynamic param)
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_SROWID = new OracleParameter("P_SROWID", OracleDbType.NVarchar2, param.SROWID, ParameterDirection.Input);
                OracleParameter P_SREGION = new OracleParameter("P_SREGION", OracleDbType.NVarchar2, param.SREGION, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_SROWID, P_SREGION, P_NCODE, P_SMESSAGE };

                var query = @"
               BEGIN
                   LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_ZONA_GEOGRAFICA( :P_SROWID, :P_SREGION, :P_NCODE, :P_SMESSAGE);
               END;
               ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                output["nCode"] = Int32.Parse(P_NCODE.Value.ToString());
                if (output["nCode"] != 0)
                {
                    output["sMessage"] = P_SMESSAGE.Value.ToString();
                }
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el error : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return output;
        }
        internal List<FrequencyListResponseDTO> GetPeriodoSemestral()
        {
            List<FrequencyListResponseDTO> result = new List<FrequencyListResponseDTO>();
            try
            {

                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_FRECUENCIA_SEMESTRAL(:RC1);
                    END;
                    ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    FrequencyListResponseDTO item = new FrequencyListResponseDTO();
                    item.frequencyId = Convert.ToInt32(odr["NIDFRECUENCIA"].ToString());
                    item.startDate = odr["DFEEJECUTAPROCINI"] == DBNull.Value ? string.Empty : odr["DFEEJECUTAPROCINI"].ToString();
                    item.endDate = odr["DFEEJECUTAPROCFIN"] == DBNull.Value ? string.Empty : odr["DFEEJECUTAPROCFIN"].ToString();
                    item.status = odr["SESTADO"] == DBNull.Value ? string.Empty : odr["SESTADO"].ToString();
                    item.frequencyType = Convert.ToInt32(odr["NTIPOFRECUENCIA"].ToString());
                    item.frequencyName = odr["SDESFRECUENCIA"] == DBNull.Value ? string.Empty : odr["SDESFRECUENCIA"].ToString();
                    item.suspendStatus = odr["SESTADOSUSP"] == DBNull.Value ? string.Empty : odr["SESTADOSUSP"].ToString();
                    item.regDate = odr["DFECHA_REGISTRO"] == DBNull.Value ? string.Empty : odr["DFECHA_REGISTRO"].ToString();
                    item.user = odr["USUARIO"] == DBNull.Value ? string.Empty : odr["USUARIO"].ToString();
                    item.NPERIODO_PROCESO = odr["NPERIODO_PROCESO"] == DBNull.Value ? string.Empty : odr["NPERIODO_PROCESO"].ToString();
                    result.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        internal List<Dictionary<string, dynamic>> GetListaActividadEconomica(dynamic param)
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {
                var P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { P_NPERIODO_PROCESO, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LIST_ACTIVITY_ECONOMY(:P_NPERIODO_PROCESO, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["SDESCRIPTION"] = odr["SDESCRIPTION"];
                    item["SNUM_RUC"] = odr["SNUM_RUC"];
                    item["STIPOCONTRIBUYENTE"] = odr["STIPOCONTRIBUYENTE"];
                    item["SACTIVITYECONOMY"] = odr["SACTIVITYECONOMY"];
                    item["SSECTOR"] = odr["SSECTOR"];
                    lista.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }

            return lista;
        }

        public List<Dictionary<string, dynamic>> GetListEspecial(dynamic param)
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {
                var P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, param.NIDALERTA, ParameterDirection.Input);
                var P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                var P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int64, param.NIDREGIMEN, ParameterDirection.Input);
                var P_STIPOIDEN_BUSQ = new OracleParameter("P_STIPOIDEN_BUSQ", OracleDbType.Varchar2, param.STIPOIDEN_BUSQ, ParameterDirection.Input);
                var P_SNUM_DOCUMENTO_BUSQ = new OracleParameter("P_SNUM_DOCUMENTO_BUSQ", OracleDbType.Varchar2, param.SNUM_DOCUMENTO_BUSQ, ParameterDirection.Input);
                var P_NIDSUBGRUPOSENAL = new OracleParameter("P_NIDSUBGRUPOSENAL", OracleDbType.Int32, param.NIDSUBGRUPOSENAL, ParameterDirection.Input);
                var P_NTIPOCARGA = new OracleParameter("P_NTIPOCARGA", OracleDbType.Int32, param.NTIPOCARGA, ParameterDirection.Input);
                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, P_NIDREGIMEN, P_STIPOIDEN_BUSQ, P_SNUM_DOCUMENTO_BUSQ, P_NIDSUBGRUPOSENAL, P_NTIPOCARGA, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_RESULTADO_ESP(:P_NIDALERTA, :P_NPERIODO_PROCESO, :P_NIDREGIMEN, :P_STIPOIDEN_BUSQ, :P_SNUM_DOCUMENTO_BUSQ,:P_NIDSUBGRUPOSENAL,:P_NTIPOCARGA, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["STIPOIDEN"] = odr["STIPOIDEN"];
                    item["SNUM_DOCUMENTO"] = odr["SNUM_DOCUMENTO"];
                    item["SNOM_COMPLETO"] = odr["SNOM_COMPLETO"];
                    item["SORIGEN"] = odr["SORIGEN"];
                    item["NPORC_APROXIMA_BUSQ"] = odr["NPORC_APROXIMA_BUSQ"];
                    item["NACEPTA_COINCIDENCIA"] = odr["NACEPTA_COINCIDENCIA"];
                    item["NIDRESULTADO"] = odr["NIDRESULTADO"];
                    item["NIDTIPOLISTA"] = odr["NIDTIPOLISTA"];
                    item["NIDPROVEEDOR"] = odr["NIDPROVEEDOR"];
                    item["STIPO_BUSQUEDA"] = odr["STIPO_BUSQUEDA"];
                    item["SCLIENT"] = odr["SCLIENT"];
                    item["SESTADO_TRAT"] = odr["SESTADO_TRAT"];
                    item["NIDREGIMEN"] = odr["NIDREGIMEN"];
                    item["SDESREGIMEN"] = odr["SDESREGIMEN"];
                    item["NTIPOCARGA"] = odr["NTIPOCARGA"];
                    item["SESTADO_REVISADO"] = odr["SESTADO_REVISADO"];
                    lista.Add(item);
                }

                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }

            return lista;

        }

        public Dictionary<string, dynamic> UpdateUnchecked(dynamic param)
        {
            var output = new Dictionary<string, dynamic>();
            try
            {
                var P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                var P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int64, param.NIDGRUPOSENAL, ParameterDirection.Input);
                var P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, param.NIDALERTA, ParameterDirection.Input);
                var P_NIDRESULTADO = new OracleParameter("P_NIDRESULTADO", OracleDbType.Int64, param.NIDRESULTADO, ParameterDirection.Input);
                var P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int64, param.NIDREGIMEN, ParameterDirection.Input);
                var P_NIDTIPOLISTA = new OracleParameter("P_NIDTIPOLISTA", OracleDbType.Int64, param.NIDTIPOLISTA, ParameterDirection.Input);
                var P_NIDPROVEEDOR = new OracleParameter("P_NIDPROVEEDOR", OracleDbType.Int64, param.NIDPROVEEDOR, ParameterDirection.Input);
                var P_NACEPTA_COINCIDENCIA = new OracleParameter("P_NACEPTA_COINCIDENCIA", OracleDbType.Int64, param.NACEPTA_COINCIDENCIA, ParameterDirection.Input);
                var P_SCLIENT = new OracleParameter("P_SCLIENT", OracleDbType.Varchar2, param.SCLIENT, ParameterDirection.Input);
                var P_NIDUSUARIO_REVISADO = new OracleParameter("P_NIDUSUARIO_REVISADO", OracleDbType.Int64, param.NIDUSUARIO_REVISADO, ParameterDirection.Input);
                var P_SESTADO_TRAT = new OracleParameter("P_SESTADO_TRAT", OracleDbType.Varchar2, param.SESTADO_TRAT, ParameterDirection.Input);
                var P_NTIPOCARGA = new OracleParameter("P_NTIPOCARGA", OracleDbType.Int64, param.NTIPOCARGA, ParameterDirection.Input);
                var P_STIPO_BUSQUEDA = new OracleParameter("P_STIPO_BUSQUEDA", OracleDbType.Varchar2, param.STIPO_BUSQUEDA, ParameterDirection.Input);
                var P_NIDCARGOPEP = new OracleParameter("P_NIDCARGOPEP", OracleDbType.Varchar2, param.NIDCARGOPEP, ParameterDirection.Input);
                var P_STIPO_DOCUMENTO = new OracleParameter("P_STIPO_DOCUMENTO", OracleDbType.Varchar2, param.STIPO_DOCUMENTO, ParameterDirection.Input);
                var P_NIDSUBGRUPOSEN = new OracleParameter("P_NIDSUBGRUPOSEN", OracleDbType.Int32, param.NIDSUBGRUPOSEN, ParameterDirection.Input);
                var P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                var P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                var parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDGRUPOSENAL, P_NIDALERTA, P_NIDRESULTADO, P_NIDREGIMEN, P_NIDTIPOLISTA, P_NIDPROVEEDOR, P_NACEPTA_COINCIDENCIA, P_SCLIENT, P_NIDUSUARIO_REVISADO, P_SESTADO_TRAT, P_NTIPOCARGA, P_STIPO_BUSQUEDA, P_NIDCARGOPEP, P_STIPO_DOCUMENTO, P_NIDSUBGRUPOSEN, P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_RESULTADO_COINCIDENCIA( :P_NPERIODO_PROCESO,:P_NIDGRUPOSENAL, :P_NIDALERTA, :P_NIDRESULTADO, :P_NIDREGIMEN, :P_NIDTIPOLISTA, :P_NIDPROVEEDOR, 
                                :P_NACEPTA_COINCIDENCIA,:P_SCLIENT, :P_NIDUSUARIO_REVISADO, :P_SESTADO_TRAT, :P_NTIPOCARGA, :P_STIPO_BUSQUEDA, :P_NIDCARGOPEP,:P_STIPO_DOCUMENTO,:P_NIDSUBGRUPOSEN, :P_NCODE, :P_SMESSAGE  );
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                output["nCode"] = Int32.Parse(P_NCODE.Value.ToString());
                if (output["nCode"] != 0)
                {
                    output["sMessage"] = P_SMESSAGE.Value.ToString();
                }
                this.context.Database.CloseConnection();
                return output;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;
            }

        }

        public List<Dictionary<string, dynamic>> GetAddressList(dynamic param)
        {
            var lista = new List<Dictionary<string, dynamic>>();
            try
            {
                var P_NIDDOC_TYPE = new OracleParameter("P_NIDDOC_TYPE", OracleDbType.Int64, param.NIDDOC_TYPE, ParameterDirection.Input);
                var P_SIDDOC = new OracleParameter("P_SIDDOC", OracleDbType.Varchar2, param.SIDDOC, ParameterDirection.Input);
                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                var parameters = new OracleParameter[] { P_NIDDOC_TYPE, P_SIDDOC, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_HIST_DIRECCCIONES(:P_NIDDOC_TYPE, :P_SIDDOC, :RC1 );
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                var odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    var item = new Dictionary<string, dynamic>();
                    item["DIRECCION"] = odr["DIRECCION"];
                    item["DCOMPDATE"] = odr["DCOMPDATE"];
                    item["DNULLDATE"] = odr["DNULLDATE"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
            }
            return lista;
        }

        public List<AlertGafiResponseDTO> getGafiByParams(AlertGafiDTO dtos)
        {
            //Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            List<AlertGafiResponseDTO> result = new List<AlertGafiResponseDTO>();
            try
            {
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, dtos.P_NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, dtos.P_NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                //OracleParameter P_NCODE = new OracleParameter ("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                //OracleParameter P_SMESSAGE = new OracleParameter ("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                //P_NCODE.Size = 4000;
                //P_SMESSAGE.Size = 4000;
                //P_DFEEJECUTAPROCFIN.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_DIR_PAIS_GAFI(:P_NIDALERTA, :P_NPERIODO_PROCESO, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    AlertGafiResponseDTO item = new AlertGafiResponseDTO();

                    item.nIdAlerta = Convert.ToInt32(odr["NIDALERTA"].ToString());
                    item.nPeriodoProceso = Convert.ToInt32(odr["NPERIODO_PROCESO"].ToString());
                    item.dCompdate = odr["DCOMPDATE"] == DBNull.Value ? string.Empty : odr["DCOMPDATE"].ToString();
                    item.sDesProducto = odr["DES_PRODUCTO"] == DBNull.Value ? string.Empty : odr["DES_PRODUCTO"].ToString();
                    item.sCliename = odr["SCLIENAME"] == DBNull.Value ? string.Empty : odr["SCLIENAME"].ToString();
                    item.nTipoDocumento = Convert.ToInt32(odr["NIDDOC_TYPE"].ToString());
                    item.sNumDocumento = odr["SIDDOC"] == DBNull.Value ? string.Empty : odr["SIDDOC"].ToString();
                    item.nPoliza = (int)Convert.ToInt64(odr["NPOLICY"].ToString());
                    item.sDireccion = odr["DIRECCION"] == DBNull.Value ? string.Empty : odr["DIRECCION"].ToString();
                    item.sPais = odr["PAIS"] == DBNull.Value ? string.Empty : odr["PAIS"].ToString();
                    item.sDesPais = odr["DES_PAIS"] == DBNull.Value ? string.Empty : odr["DES_PAIS"].ToString();
                    item.sDesTipoDocumento = odr["DES_TIPO_DOC"] == DBNull.Value ? string.Empty : odr["DES_TIPO_DOC"].ToString();
                    result.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
                return result;

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                ///Console.WriteLine("LA EXCEPCION  :::  "+ex.Message);
                throw ex;
            }

        }

        public List<AlertNotaCreditoResponseDTO> getNCByParams(AlertNCDTORequest dtos)
        {
            //Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            List<AlertNotaCreditoResponseDTO> result = new List<AlertNotaCreditoResponseDTO>();
            try
            {
                Console.WriteLine("LOS DTOS 1: " + dtos.P_NIDALERTA);
                Console.WriteLine("LOS DTOS 2: " + dtos.P_NPERIODO_PROCESO);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, dtos.P_NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, dtos.P_NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int32, dtos.P_NIDREGIMEN, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                //OracleParameter P_NCODE = new OracleParameter ("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                //OracleParameter P_SMESSAGE = new OracleParameter ("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                //P_NCODE.Size = 4000;
                //P_SMESSAGE.Size = 4000;
                //P_DFEEJECUTAPROCFIN.Size = 4000;
                Console.WriteLine("LOS DTOS 1: " + dtos.P_NIDALERTA);
                Console.WriteLine("LOS DTOS 2: " + dtos.P_NPERIODO_PROCESO);
                Console.WriteLine("LOS DTOS 2: " + dtos.P_NIDREGIMEN);
                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, P_NIDREGIMEN, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_NOTAS_CREDITO(:P_NIDALERTA, :P_NPERIODO_PROCESO, :P_NIDREGIMEN, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    AlertNotaCreditoResponseDTO item = new AlertNotaCreditoResponseDTO();

                    item.alerta = (int)Convert.ToInt64(odr["NIDALERTA"].ToString());
                    item.periodo = (int)Convert.ToInt64(odr["NPERIODO_PROCESO"].ToString());
                    item.fecha = odr["DCOMPDATE"] == DBNull.Value ? string.Empty : odr["DCOMPDATE"].ToString();
                    item.producto = odr["DESPRODUCTO"] == DBNull.Value ? string.Empty : odr["DESPRODUCTO"].ToString();
                    item.motivo = odr["DESMOTIVO"] == DBNull.Value ? string.Empty : odr["DESMOTIVO"].ToString();
                    item.codigoCliente = odr["SCLIENT"] == DBNull.Value ? string.Empty : odr["SCLIENT"].ToString();
                    item.nombreCliente = odr["NOMCLIENTE"] == DBNull.Value ? string.Empty : odr["NOMCLIENTE"].ToString();
                    item.tipoDocumento = odr["DESTIPODOC"] == DBNull.Value ? string.Empty : odr["DESTIPODOC"].ToString();
                    item.numeroDocumento = odr["NRODOC"] == DBNull.Value ? string.Empty : odr["NRODOC"].ToString();
                    item.tipoComprobante = odr["DESTIPOCOMP"] == DBNull.Value ? string.Empty : odr["DESTIPOCOMP"].ToString();
                    item.numeroComprobante = odr["NROCOMP"] == DBNull.Value ? string.Empty : odr["NROCOMP"].ToString();
                    item.amount = odr["NAMOUNT"] == DBNull.Value ? 0 : Convert.ToDecimal(odr["NAMOUNT"].ToString());
                    item.iva = odr["NIVA"] == DBNull.Value ? 0 : Convert.ToDecimal(odr["NIVA"].ToString());
                    item.rightTissue = odr["NRIGHTISSUE"] == DBNull.Value ? 0 : Convert.ToDecimal(odr["NRIGHTISSUE"].ToString());
                    item.poliza = (int)Convert.ToInt64(odr["NPOLICY"].ToString());
                    item.recibo = odr["NRECEIPT"] == DBNull.Value ? string.Empty : odr["NRECEIPT"].ToString();

                    result.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
                return result;

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("LA EXCEPCION  :::  " + ex);
                throw ex;
            }
        }

        public List<ClientAlertS2ResDTO> getClientsS2ByParams(ClientAlertS2ReqDTO dtos)
        {
            //Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            List<ClientAlertS2ResDTO> result = new List<ClientAlertS2ResDTO>();
            try
            {
                Console.WriteLine("LOS DTOS 1: " + dtos.P_NIDALERTA);
                Console.WriteLine("LOS DTOS 2: " + dtos.P_NPERIODO_PROCESO);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, dtos.P_NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, dtos.P_NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                Console.WriteLine("LOS DTOS 1: " + dtos.P_NIDALERTA);
                Console.WriteLine("LOS DTOS 2: " + dtos.P_NPERIODO_PROCESO);
                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_DIR_DUPL(:P_NIDALERTA, :P_NPERIODO_PROCESO, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    ClientAlertS2ResDTO item = new ClientAlertS2ResDTO();

                    item.alerta = (int)Convert.ToInt64(odr["NIDALERTA"].ToString());
                    item.periodo = (int)Convert.ToInt64(odr["NPERIODO_PROCESO"].ToString());
                    item.fecha = odr["DCOMPDATE"] == DBNull.Value ? string.Empty : odr["DCOMPDATE"].ToString();
                    item.producto = odr["DES_PRODUCTO"] == DBNull.Value ? string.Empty : odr["DES_PRODUCTO"].ToString();
                    item.nombreCliente = odr["SCLIENAME"] == DBNull.Value ? string.Empty : odr["SCLIENAME"].ToString();
                    item.codigoCliente = odr["SCLIENT"] == DBNull.Value ? string.Empty : odr["SCLIENT"].ToString();
                    item.tipoDocumento = odr["DES_TIPO_DOC"] == DBNull.Value ? string.Empty : odr["DES_TIPO_DOC"].ToString();
                    item.numeroDocumento = odr["SIDDOC"] == DBNull.Value ? string.Empty : odr["SIDDOC"].ToString();
                    item.poliza = (int)Convert.ToInt64(odr["NPOLICY"].ToString());
                    item.direccion = odr["DIRECCION"] == DBNull.Value ? string.Empty : odr["DIRECCION"].ToString();
                    item.departamento = odr["DES_DEPARTAMENTO"] == DBNull.Value ? string.Empty : odr["DES_DEPARTAMENTO"].ToString();
                    item.provincia = odr["DES_PROVINCIA"] == DBNull.Value ? string.Empty : odr["DES_PROVINCIA"].ToString();
                    item.distrito = odr["DES_DISTRITO"] == DBNull.Value ? string.Empty : odr["DES_DISTRITO"].ToString();
                    item.pais = odr["DES_PAIS"] == DBNull.Value ? string.Empty : odr["DES_PAIS"].ToString();

                    result.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
                //var serializado = JsonSerializer.ser

                return result;

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("LA EXCEPCION  :::  " + ex.Message);
                throw ex;
            }

        }

        public List<ClientAlertRG4ResDTO> getClientsRG4ByParams(ClientAlertRG4ReqDTO dtos)
        {
            //Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            List<ClientAlertRG4ResDTO> result = new List<ClientAlertRG4ResDTO>();
            try
            {
                Console.WriteLine("LOS DTOS 1: " + dtos.P_NIDALERTA);
                Console.WriteLine("LOS DTOS 2: " + dtos.P_NPERIODO_PROCESO);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, dtos.P_NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, dtos.P_NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                Console.WriteLine("LOS DTOS 1: " + dtos.P_NIDALERTA);
                Console.WriteLine("LOS DTOS 2: " + dtos.P_NPERIODO_PROCESO);
                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_CLI_ALERT_RENTA(:P_NIDALERTA, :P_NPERIODO_PROCESO, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    ClientAlertRG4ResDTO item = new ClientAlertRG4ResDTO();

                    item.alerta = (int)Convert.ToInt64(odr["NIDALERTA"].ToString());
                    item.periodo = dtos.P_NPERIODO_PROCESO;//(int)Convert.ToInt64(odr["NPERIODO_ALERTA"].ToString());
                    item.fecha = odr["DCOMPDATE"] == DBNull.Value ? string.Empty : odr["DCOMPDATE"].ToString();
                    item.producto = odr["SPRODUCTO"] == DBNull.Value ? string.Empty : odr["SPRODUCTO"].ToString();
                    item.nombreCliente = odr["SNOM_COMPLETO"] == DBNull.Value ? string.Empty : odr["SNOM_COMPLETO"].ToString();
                    item.codigoCliente = odr["SCOD_CLIENTE"] == DBNull.Value ? string.Empty : odr["SCOD_CLIENTE"].ToString();
                    item.tipoDocumento = odr["SDES_TIPO_DOC"] == DBNull.Value ? string.Empty : odr["SDES_TIPO_DOC"].ToString();
                    item.numeroDocumento = odr["SNUM_DOCUMENTO"] == DBNull.Value ? string.Empty : odr["SNUM_DOCUMENTO"].ToString();
                    item.poliza = odr["SNUM_DOCUMENTO"] == DBNull.Value ? string.Empty : odr["SNUM_POLIZA"].ToString();
                    item.fechaInicioPoliza = odr["DFEC_INI_POLIZA"] == DBNull.Value ? string.Empty : odr["DFEC_INI_POLIZA"].ToString();
                    item.fechaFinPoliza = odr["DFEC_FIN_POLIZA"] == DBNull.Value ? string.Empty : odr["DFEC_FIN_POLIZA"].ToString();
                    item.FechaInicioVigenciaPoliza = odr["DINICIO_VIG_CERT"] == DBNull.Value ? string.Empty : odr["DINICIO_VIG_CERT"].ToString();
                    item.FechaFinVigenciaPoliza = odr["DFIN_VIG_CERT"] == DBNull.Value ? string.Empty : odr["DFIN_VIG_CERT"].ToString();
                    item.riskType = odr["NRISKTYPE"] == DBNull.Value ? string.Empty : odr["NRISKTYPE"].ToString();
                    item.riesgoFinanciero = odr["SRIESGO_FINANCIERO"] == DBNull.Value ? string.Empty : odr["SRIESGO_FINANCIERO"].ToString();

                    result.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
                return result;

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                ///Console.WriteLine("LA EXCEPCION  :::  "+ex.Message);
                throw ex;
            }

        }

        //modificar para descargar listas internacionales por tipo
        public Dictionary<string, dynamic> getListasInternacionalesByType(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, System.Data.ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, System.Data.ParameterDirection.Input);
                //OracleParameter P_NIDREGIMEN = new OracleParameter ("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, System.Data.ParameterDirection.Input);
                //OracleParameter P_NIDPROVEEDOR = new OracleParameter ("P_NIDPROVEEDOR", OracleDbType.Int32, param.NIDPROVEEDOR, System.Data.ParameterDirection.Input);
                //OracleParameter P_NIDGRUPOSENAL = new OracleParameter ("P_NIDGRUPOSENAL", OracleDbType.Int32, param.NIDGRUPOSENAL, System.Data.ParameterDirection.Input);
                //OracleParameter P_NIDSUBGRUPOSEN = new OracleParameter ("P_NIDSUBGRUPOSEN", OracleDbType.Int32, param.NIDSUBGRUPOSEN, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDTIPOLISTA = new OracleParameter("P_NIDTIPOLISTA", OracleDbType.Int32, param.NIDTIPOLISTA, System.Data.ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, P_NIDTIPOLISTA, RC1 };
                //OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDALERTA, P_NIDREGIMEN, P_NIDPROVEEDOR, P_NIDGRUPOSENAL, P_NIDSUBGRUPOSEN, P_NIDTIPOLISTA, RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_INFO( :P_NIDALERTA,:P_NPERIODO_PROCESO, :P_NIDTIPOLISTA, :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["NIDALERTA"] = odr["NIDALERTA"];
                    item["NIDLISTAEXT"] = odr["NIDLISTAEXT"];
                    item["SORIGEN"] = odr["SORIGEN"];
                    item["DFECHA_CARGA"] = odr["DFECHA_CARGA"];
                    item["STIPO_DOCUMENTO"] = odr["STIPO_DOCUMENTO"];
                    item["SNUM_DOCUMENTO"] = odr["SNUM_DOCUMENTO"];
                    item["SNOM_COMPLETO"] = odr["SNOM_COMPLETO"];
                    item["SDESTIPOLISTA"] = odr["SDESTIPOLISTA"];
                    item["NIDTIPOLISTA"] = odr["NIDTIPOLISTA"];
                    item["NIDPROVEEDOR"] = odr["NIDPROVEEDOR"];
                    item["SDESPROVEEDOR"] = odr["SDESPROVEEDOR"];
                    lista.Add(item);
                }
                odr.Close();
                /*
                odr = ((OracleRefCursor) RC2.Value).GetDataReader();
                if (odr.HasRows) {
                    while(odr.Read()) {
                    }
                }
                */
                this.context.Database.CloseConnection();

                //return lista;
                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                objRespuesta["code"] = 0;
                objRespuesta["lista"] = lista;
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                //Console.WriteLine(" hola : " + ex);
                //throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
                Console.WriteLine("el ex : " + ex);
                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;
            }

        }

        public List<Dictionary<string, dynamic>> getListaInternacional(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int32, param.NIDGRUPOSENAL, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int32, param.NIDREGIMEN, System.Data.ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NIDGRUPOSENAL, P_NPERIODO_PROCESO, P_NIDREGIMEN, RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_TIPOS_LISTA(:P_NIDALERTA, :P_NIDGRUPOSENAL,:P_NPERIODO_PROCESO, :P_NIDREGIMEN, :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NIDTIPOLISTA"] = odr["NIDTIPOLISTA"];
                    item["SDESTIPOLISTA"] = odr["SDESTIPOLISTA"];
                    item["NCANTCLIXREV"] = odr["NCANTCLIXREV"];
                    item["DCOMPDATE"] = odr["DCOMPDATE"];
                    item["SESTADO_REVISADO"] = odr["SESTADO_REVISADO"];
                    item["NCANTCLIENTES"] = odr["NCANTCLIENTES"];
                    item["NIDREGIMEN"] = odr["NIDREGIMEN"];
                    item["NIDPROVEEDOR"] = odr["NIDPROVEEDOR"];
                    item["NIDSUBGRUPOSEN"] = odr["NIDSUBGRUPOSEN"];
                    item["SDESSUBGRUPO_SENAL"] = odr["SDESSUBGRUPO_SENAL"];
                    item["NIDGRUPOSENAL"] = odr["NIDGRUPOSENAL"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine(" hola : " + ex);
                throw ex;
            }
            return lista;
        }

        public List<Dictionary<string, dynamic>> getResultadosCoincidencias(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int32, param.NIDREGIMEN, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int32, param.NIDGRUPOSENAL, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDALERTA, P_NIDREGIMEN, P_NIDGRUPOSENAL, RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_RESULTADO(:P_NPERIODO_PROCESO, :P_NIDALERTA, :P_NIDREGIMEN, :P_NIDGRUPOSENAL, :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["STIPOIDEN"] = odr["STIPOIDEN"];
                    item["SNUM_DOCUMENTO"] = odr["SNUM_DOCUMENTO"];
                    item["SNOM_COMPLETO"] = odr["SNOM_COMPLETO"];
                    item["SDESTIPOLISTA"] = odr["SDESTIPOLISTA"];
                    item["SDESESTADO"] = odr["SDESESTADO"];
                    item["NTIPO_DOCUMENTO"] = odr["NTIPO_DOCUMENTO"];
                    item["DFECHA_NACIMIENTO"] = odr["DFECHA_NACIMIENTO"];
                    //item["DBIRTHDAT"] = odr["DBIRTHDAT"];
                    item["EDAD"] = odr["EDAD"];
                    item["SOCUPACION"] = odr["SOCUPACION"];
                    item["SCARGO"] = odr["SCARGO"];
                    item["SZONA_GEO"] = odr["SZONA_GEO"];
                    item["NTIPO_DOCUMENTO"] = odr["NTIPO_DOCUMENTO"];
                    item["DFECHA_REVISADO"] = odr["DFECHA_REVISADO"];
                    item["SESTADO_REVISADO"] = odr["SESTADO_REVISADO"];
                    item["SDESPROVEEDOR"] = odr["SDESPROVEEDOR"];
                    item["SORIGEN"] = odr["SORIGEN"];
                    item["SCLIENT"] = odr["SCLIENT"];
                    item["SESTADO_TRAT"] = odr["SESTADO_TRAT"];
                    item["NTIPOCARGA"] = odr["NTIPOCARGA"];
                    item["STIPO_BUSQUEDA"] = odr["STIPO_BUSQUEDA"];
                    item["NPORC_APROXIMA_BUSQ"] = odr["NPORC_APROXIMA_BUSQ"];
                    item["NIDPROVEEDOR"] = odr["NIDPROVEEDOR"];
                    item["NIDPROVEEDOR"] = odr["NIDPROVEEDOR"];
                    item["NIDSUBGRUPOSEN"] = odr["NIDSUBGRUPOSEN"];
                    item["SDESSUBGRUPO_SENAL"] = odr["SDESSUBGRUPO_SENAL"];
                    item["SNUM_DOCUMENTO_EMPRESA"] = odr["SNUM_DOCUMENTO_EMPRESA"];
                    item["SNOM_COMPLETO_EMPRESA"] = odr["SNOM_COMPLETO_EMPRESA"];
                    item["NACIONALIDAD"] = odr["NACIONALIDAD"];
                    item["CARGO"] = odr["CARGO"];


                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine(" hola : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }

        public Dictionary<string, dynamic> InsertAttachedFilesByAlert(dynamic param)
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Varchar2, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_SRUTA_ADJUNTO = new OracleParameter("P_SRUTA_ADJUNTO", OracleDbType.Varchar2, param.SRUTA_ADJUNTO, ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_MODIFICA = new OracleParameter("P_NIDUSUARIO_MODIFICA", OracleDbType.Varchar2, param.NIDUSUARIO_MODIFICA, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, P_SRUTA_ADJUNTO, P_NIDUSUARIO_MODIFICA, P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_INS_ADJUNTO_ALERTA_PERIODO(:P_NIDALERTA, :P_NPERIODO_PROCESO, :P_SRUTA_ADJUNTO, :P_NIDUSUARIO_MODIFICA, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                output["nCode"] = Convert.ToInt64((P_NCODE.Value).ToString());
                if (output["nCode"] != 0)
                {
                    output["sMessage"] = P_SMESSAGE.Value;
                }
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return output;
        }


        public Dictionary<string, dynamic> InsertAttachedFilesInformByAlert(dynamic param)
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                Console.WriteLine("data : " + param);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_SRUTA_ADJUNTO = new OracleParameter("P_SRUTA_ADJUNTO", OracleDbType.Varchar2, param.SRUTA_ADJUNTO, ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_MODIFICA = new OracleParameter("P_NIDUSUARIO_MODIFICA", OracleDbType.Int32, param.NIDUSUARIO_MODIFICA, ParameterDirection.Input);
                OracleParameter P_STIPO_CARGA = new OracleParameter("P_STIPO_CARGA", OracleDbType.Varchar2, param.STIPO_CARGA, ParameterDirection.Input);
                OracleParameter P_NREGIMEN = new OracleParameter("P_NREGIMEN", OracleDbType.Int32, param.NREGIMEN, ParameterDirection.Input);
                OracleParameter P_NIDALERTA_CABECERA = new OracleParameter("P_NIDALERTA_CABECERA", OracleDbType.Int32, param.NIDALERTA_CABECERA, ParameterDirection.Input);
                OracleParameter P_NID_USUARIO = new OracleParameter("P_NID_USUARIO", OracleDbType.Int32, param.NID_USUARIO, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, P_SRUTA_ADJUNTO, P_NIDUSUARIO_MODIFICA, P_STIPO_CARGA, P_NREGIMEN, P_NIDALERTA_CABECERA, P_NID_USUARIO, P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_INS_ADJ_ALR_INFORM_PERIODO(:P_NIDALERTA, :P_NPERIODO_PROCESO, :P_SRUTA_ADJUNTO, :P_NIDUSUARIO_MODIFICA, :P_STIPO_CARGA, :P_NREGIMEN, :P_NIDALERTA_CABECERA, :P_NID_USUARIO ,:P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                output["nCode"] = Convert.ToInt64((P_NCODE.Value).ToString());
                if (output["nCode"] != 0)
                {
                    output["sMessage"] = P_SMESSAGE.Value;
                }
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el mensaje del error : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return output;
        }


        public List<Dictionary<string, dynamic>> GetAttachedFilesByAlert(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Varchar2, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_ADJUNTO_ALERTA_PERIODO(:P_NIDALERTA, :P_NPERIODO_PROCESO, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["DFECHA_REGISTRO"] = odr["DFECHA_REGISTRO"];
                    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"];
                    item["SRUTA_ADJUNTO"] = odr["SRUTA_ADJUNTO"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }

        public List<Dictionary<string, dynamic>> GetAttachedFilesInformByAlert(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                //MODIFICAR PARAMETROS Y STORE
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Varchar2, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_STIPO_CARGA = new OracleParameter("P_STIPO_CARGA", OracleDbType.Varchar2, param.STIPO_CARGA, ParameterDirection.Input);
                OracleParameter P_NREGIMEN = new OracleParameter("P_NREGIMEN", OracleDbType.Int32, param.NREGIMEN, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, P_STIPO_CARGA, P_NREGIMEN, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_ADJ_INFORM_ALR_PERIODO(:P_NIDALERTA, :P_NPERIODO_PROCESO, :P_STIPO_CARGA, :P_NREGIMEN, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["DFECHA_REGISTRO"] = odr["DFECHA_REGISTRO"];
                    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"];
                    item["SRUTA_ADJUNTO"] = odr["SRUTA_ADJUNTO"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }

        public List<Dictionary<string, dynamic>> GetAttachedFilesInformByCabecera(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                //MODIFICAR PARAMETROS Y STORE
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Varchar2, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_STIPO_CARGA = new OracleParameter("P_STIPO_CARGA", OracleDbType.Varchar2, param.STIPO_CARGA, ParameterDirection.Input);
                OracleParameter P_NREGIMEN = new OracleParameter("P_NREGIMEN", OracleDbType.Int32, param.NREGIMEN, ParameterDirection.Input);
                OracleParameter P_NIDALERTA_CABECERA = new OracleParameter("P_NIDALERTA_CABECERA", OracleDbType.Int32, param.NIDALERTA_CABECERA, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, P_STIPO_CARGA, P_NREGIMEN, P_NIDALERTA_CABECERA, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_ADJ_INFORM_ALR_PERIODO_CAB(:P_NIDALERTA, :P_NPERIODO_PROCESO, :P_STIPO_CARGA, :P_NREGIMEN, :P_NIDALERTA_CABECERA, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["DFECHA_REGISTRO"] = odr["DFECHA_REGISTRO"];
                    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"];
                    item["SRUTA_ADJUNTO"] = odr["SRUTA_ADJUNTO"];
                    item["NIDALERTA_CABECERA"] = odr["NIDALERTA_CABECERA"];
                    item["NIDALERTA"] = param.NIDALERTA;
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                //throw ex;
                Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                item["mensajeError"] = ex.Message.ToString();
                item["mensajeErrorDetalle"] = ex;
                lista.Add(item);
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }


        public Dictionary<string, dynamic> UpdateTratamientoCliente(dynamic param)
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int32, param.NIDGRUPOSENAL, ParameterDirection.Input);

                OracleParameter P_SCLIENT = new OracleParameter("P_SCLIENT", OracleDbType.Varchar2, param.SCLIENT, ParameterDirection.Input);
                OracleParameter P_SESTADO_TRAT = new OracleParameter("P_SESTADO_TRAT", OracleDbType.Varchar2, param.SESTADO_TRAT, ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_MODIFICA = new OracleParameter("P_NIDUSUARIO_MODIFICA", OracleDbType.Varchar2, param.NIDUSUARIO_MODIFICA, ParameterDirection.Input);
                //OracleParameter P_NIDDOC_TYPE = new OracleParameter("P_NIDDOC_TYPE", OracleDbType.Varchar2, param.NIDDOC_TYPE, ParameterDirection.Input);
                //OracleParameter P_SIDDOC = new OracleParameter("P_SIDDOC", OracleDbType.Varchar2, param.SIDDOC, ParameterDirection.Input);
                OracleParameter P_DBIRTHDAT = new OracleParameter("P_DBIRTHDAT", OracleDbType.Varchar2, param.DBIRTHDAT, ParameterDirection.Input);
                OracleParameter P_SCLIENAME = new OracleParameter("P_SCLIENAME", OracleDbType.Varchar2, param.SCLIENAME, ParameterDirection.Input);
                OracleParameter P_STIPOIDEN = new OracleParameter("P_STIPOIDEN", OracleDbType.Varchar2, param.STIPOIDEN, ParameterDirection.Input);
                OracleParameter P_STIPO_PEP = new OracleParameter("P_STIPO_PEP", OracleDbType.Varchar2, param.STIPO_PEP, ParameterDirection.Input);
                OracleParameter P_SDES_PEP = new OracleParameter("P_SDES_PEP", OracleDbType.Varchar2, param.SDES_PEP, ParameterDirection.Input);
                OracleParameter P_SCOMENTARIO = new OracleParameter("P_SCOMENTARIO", OracleDbType.Varchar2, param.SCOMENTARIO, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int32, param.NIDREGIMEN, ParameterDirection.Input);
                OracleParameter P_TIPOACCION = new OracleParameter("P_TIPOACCION", OracleDbType.Varchar2, param.TIPOACCION, ParameterDirection.Input);
                OracleParameter P_STIPOACTRESULTADO = new OracleParameter("P_STIPOACTRESULTADO", OracleDbType.Varchar2, param.STIPOACTRESULTADO, ParameterDirection.Input);
                OracleParameter P_NTIPOCLIENTE = new OracleParameter("P_NTIPOCLIENTE", OracleDbType.Varchar2, param.NTIPOCLIENTE, ParameterDirection.Input);
                OracleParameter P_NIDTRATCLIEHIS = new OracleParameter("P_NIDTRATCLIEHIS", OracleDbType.Int32, param.NIDTRATCLIEHIS, ParameterDirection.Input);
                OracleParameter P_NTIPOCARGA = new OracleParameter("P_NTIPOCARGA", OracleDbType.Int32, param.NTIPOCARGA, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);


                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;
                P_SESTADO_TRAT.Size = 3;

                OracleParameter[] parameters = new OracleParameter[] {
                    P_NPERIODO_PROCESO,
                    P_NIDGRUPOSENAL,
                    P_NIDALERTA,
                    P_SCLIENT,
                    P_SESTADO_TRAT,
                    P_NIDUSUARIO_MODIFICA,
                    P_NTIPOCLIENTE,
                    P_STIPOACTRESULTADO,
                    P_DBIRTHDAT,
                    P_SCLIENAME,
                    P_STIPOIDEN,
                    P_STIPO_PEP,
                    P_SDES_PEP,
                    P_SCOMENTARIO,
                    P_NIDREGIMEN,
                    P_NIDTRATCLIEHIS,
                    P_NTIPOCARGA,
                    P_TIPOACCION,
                    P_NCODE,
                    P_SMESSAGE
                };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_TRATAMIENTO_CLIENTE(:P_NPERIODO_PROCESO, :P_NIDGRUPOSENAL,:P_NIDALERTA, :P_SCLIENT, :P_SESTADO_TRAT, :P_NIDUSUARIO_MODIFICA, :P_NTIPOCLIENTE, :P_STIPOACTRESULTADO, :P_DBIRTHDAT, :P_SCLIENAME, :P_STIPOIDEN, :P_STIPO_PEP, :P_SDES_PEP, :P_SCOMENTARIO, :P_NIDREGIMEN, :P_NIDTRATCLIEHIS, :P_NTIPOCARGA ,:P_TIPOACCION, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                output["nCode"] = Convert.ToInt64((P_NCODE.Value).ToString());
                output["sState"] = param.SESTADO_TRAT.ToString();
                if (output["nCode"] != 0)
                {
                    output["sMessage"] = P_SMESSAGE.Value;
                }
                this.context.Database.CloseConnection();
                return output;

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            //Console.WriteLine("el output : "+output);
            //return output;
        }

        public List<Dictionary<string, dynamic>> GetResultadoTratamiento(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Varchar2, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Varchar2, param.NIDREGIMEN, ParameterDirection.Input);
                OracleParameter P_SESTADO_TRAT = new OracleParameter("P_SESTADO_TRAT", OracleDbType.Varchar2, param.SESTADO_TRAT, ParameterDirection.Input);
                OracleParameter P_NUM_RESULT = new OracleParameter("P_NUM_RESULT", OracleDbType.Int32, 50, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDALERTA, P_NIDREGIMEN, P_SESTADO_TRAT, P_NUM_RESULT, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_RESULTADO_TRATAMIENTO(:P_NPERIODO_PROCESO, :P_NIDALERTA, :P_NIDREGIMEN, :P_SESTADO_TRAT,:P_NUM_RESULT, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["STIPOIDEN"] = odr["STIPOIDEN"];
                    item["SNUM_DOCUMENTO"] = odr["SNUM_DOCUMENTO"];
                    item["SNOM_COMPLETO"] = odr["SNOM_COMPLETO"];
                    item["SDESTIPOLISTA"] = odr["SDESTIPOLISTA"];
                    item["SDESESTADO"] = odr["SDESESTADO"];
                    item["NTIPO_DOCUMENTO"] = odr["NTIPO_DOCUMENTO"];
                    item["DFECHA_NACIMIENTO"] = odr["DFECHA_NACIMIENTO"];
                    item["EDAD"] = odr["EDAD"];
                    item["SDESPRODUCTO"] = odr["SDESPRODUCTO"];
                    item["NIDTIPOLISTA"] = odr["NIDTIPOLISTA"];
                    //  item["SDES_PEP"] = odr["SDES_PEP"];
                    //  item["SCOMENTARIO"] = odr["SCOMENTARIO"];
                    item["NIDREGIMEN"] = odr["NIDREGIMEN"];
                    item["SDESREGIMEN"] = odr["SDESREGIMEN"];
                    item["NIDTRATCLIEHIS"] = odr["NIDTRATCLIEHIS"];

                    //item["SCARGO"] = odr["SCARGO"];
                    //item["SOCUPACION"] = odr["SOCUPACION"];
                    //item["SZONA_GEO"] = odr["SZONA_GEO"];
                    item["SCLIENT"] = odr["SCLIENT"];
                    item["NTIPOCARGA"] = odr["NTIPOCARGA"];
                    item["NIDREGIMEN_VALID"] = odr["NIDREGIMEN_VALID"];
                    item["SFALTA_ACEPTAR_COINC"] = odr["SFALTA_ACEPTAR_COINC"] == DBNull.Value ? string.Empty : odr["SFALTA_ACEPTAR_COINC"].ToString();

                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("Error :" + ex);
                throw ex;

                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }

        public List<Dictionary<string, dynamic>> GetPerfilXGrupo(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int64, param.NIDGRUPOSENAL, ParameterDirection.Input);

                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NIDGRUPOSENAL, RC1 };



                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_PERFIL_X_GRUPO_SENAL(:P_NIDGRUPOSENAL, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NIDPROFILE"] = odr["NIDPROFILE"];
                    item["SNAME"] = odr["SNAME"];


                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("Error :" + ex);
                Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                item["menesajeError"] = ex.Message.ToString();
                item["mensajeErrorDEtalle"] = ex;
                lista.Add(item);
                //throw ex;

                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }


        public List<Dictionary<string, dynamic>> GetGrupoXPerfil(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_NIDPROFILE = new OracleParameter("P_NIDPROFILE", OracleDbType.Int64, param.NIDPROFILE, ParameterDirection.Input);

                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NIDPROFILE, RC1 };



                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_GRUPO_SENAL_X_PERFIL(:P_NIDPROFILE, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NIDGRUPOSENAL"] = odr["NIDGRUPOSENAL"];
                    item["SDESGRUPO_SENAL"] = odr["SDESGRUPO_SENAL"];


                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("Error :" + ex);
                Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                item["menesajeError"] = ex.Message.ToString();
                item["mensajeErrorDEtalle"] = ex;
                lista.Add(item);
                //throw ex;

                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }




        public List<Dictionary<string, dynamic>> GetResultsList(dynamic param)
        {
            var lista = new List<Dictionary<string, dynamic>>();
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                var P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                // var P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);
                var P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int32, param.NIDGRUPOSENAL, ParameterDirection.Input);
                var P_NIDSUBGRUPOSEN = new OracleParameter("P_NIDSUBGRUPOSEN", OracleDbType.Int32, param.NIDSUBGRUPOSENAL, ParameterDirection.Input);
                var P_NTIPOIDEN_BUSQ = new OracleParameter("P_NTIPOIDEN_BUSQ", OracleDbType.Int32, param.NTIPOIDEN_BUSQ, ParameterDirection.Input);
                var P_SNUM_DOCUMENTO_BUSQ = new OracleParameter("P_SNUM_DOCUMENTO_BUSQ", OracleDbType.Varchar2, param.SNUM_DOCUMENTO_BUSQ, ParameterDirection.Input);
                var P_SNOM_COMPLETO_BUSQ = new OracleParameter("P_SNOM_COMPLETO_BUSQ", OracleDbType.Varchar2, param.SNOM_COMPLETO_BUSQ, ParameterDirection.Input);
                var P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int64, ParameterDirection.Output);
                var P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                var parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDGRUPOSENAL, P_NIDSUBGRUPOSEN, P_NTIPOIDEN_BUSQ, P_SNUM_DOCUMENTO_BUSQ, P_SNOM_COMPLETO_BUSQ, RC1, P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_RESULTADO_GC(:P_NPERIODO_PROCESO, :P_NIDGRUPOSENAL,:P_NIDSUBGRUPOSEN, :P_NTIPOIDEN_BUSQ, :P_SNUM_DOCUMENTO_BUSQ, :P_SNOM_COMPLETO_BUSQ, :RC1, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                Console.WriteLine("P_NCODE.Value : " + P_NCODE.Value);
                Console.WriteLine("P_NCODE.Value.ToString() : " + P_NCODE.Value.ToString());
                output["nCode"] = Convert.ToInt64((P_NCODE.Value).ToString());

                if (output["nCode"] == 1)
                {
                    output["sMessage"] = P_SMESSAGE.Value.ToString();
                    Console.WriteLine("P_SMESSAGE.Value.ToString() : " + P_SMESSAGE.Value.ToString());
                }
                else
                {
                    OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                    while (odr.Read())
                    {
                        var item = new Dictionary<string, dynamic>();
                        item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                        item["NPERIODO_PROCESO_VALID"] = odr["NPERIODO_PROCESO_VALID"];
                        item["STIPOIDEN"] = odr["STIPOIDEN"];
                        item["SNUM_DOCUMENTO"] = odr["SNUM_DOCUMENTO"];
                        item["NIDTIPOLISTA"] = odr["NIDTIPOLISTA"];
                        item["SDESTIPOLISTA"] = odr["SDESTIPOLISTA"];
                        item["SNOM_COMPLETO"] = odr["SNOM_COMPLETO"];
                        item["SDESTIPOLISTA"] = odr["SDESTIPOLISTA"];
                        item["SDESESTADO"] = odr["SDESESTADO"];
                        item["NTIPO_DOCUMENTO"] = odr["NTIPO_DOCUMENTO"];
                        item["DFECHA_NACIMIENTO"] = odr["DFECHA_NACIMIENTO"];
                        item["EDAD"] = odr["EDAD"];
                        // item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                        item["SCLIENT"] = odr["SCLIENT"];
                        item["SESTADO_TRAT"] = odr["SESTADO_TRAT"];
                        item["SDESESTADO_TRAT"] = odr["SDESESTADO_TRAT"];
                        item["NIDREGIMEN"] = odr["NIDREGIMEN"];
                        item["SDESPRODUCTO"] = odr["SDESPRODUCTO"];
                        item["SDESREGIMEN"] = odr["SDESREGIMEN"];
                        item["SESTADO_REVISADO"] = odr["SESTADO_REVISADO"];
                        item["NTIPOCARGA"] = odr["NTIPOCARGA"];
                        item["NIDREGIMEN_VALID"] = odr["NIDREGIMEN_VALID"];
                        item["NIDGRUPOSENAL"] = odr["NIDGRUPOSENAL"];
                        item["SFALTA_ACEPTAR_COINC"] = odr["SFALTA_ACEPTAR_COINC"] == DBNull.Value ? string.Empty : odr["SFALTA_ACEPTAR_COINC"].ToString();
                        item["SCARGO"] = odr["SCARGO"];
                        item["NACIONALIDAD"] = odr["NACIONALIDAD"];
                        item["CARGO"] = odr["CARGO"];

                        lista.Add(item);
                    }


                }
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("Error en la BD : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }


        public Dictionary<dynamic, dynamic> UpdateListClienteRefor(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();

            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Varchar2, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Varchar2, param.NIDREGIMEN, ParameterDirection.Input);
                OracleParameter P_SESTADO_TRAT = new OracleParameter("P_SESTADO_TRAT", OracleDbType.Varchar2, param.SESTADO_TRAT, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] {
                    P_NPERIODO_PROCESO,
                    P_NIDALERTA,
                    P_NIDREGIMEN,
                    P_SESTADO_TRAT,
                    P_NCODE,
                    P_SMESSAGE
                };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_INS_LIST_CLI_REFOR(:P_NPERIODO_PROCESO, :P_NIDALERTA, :P_NIDREGIMEN, :P_SESTADO_TRAT, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                output["nCode"] = Convert.ToInt64((P_NCODE.Value).ToString());
                if (output["nCode"] != 0)
                {
                    output["sMessage"] = P_SMESSAGE.Value;
                }
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex: " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return output;
        }

        public Dictionary<dynamic, dynamic> GetUpdateCorreos(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();

            try
            {
                OracleParameter P_NIDCORREO = new OracleParameter("P_NIDCORREO", OracleDbType.Int32, param.NIDCORREO, ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int32, param.NIDGRUPOSENAL, ParameterDirection.Input);
                OracleParameter P_NIDPROFILE = new OracleParameter("P_NIDPROFILE", OracleDbType.Int32, param.NIDPROFILE, ParameterDirection.Input);
                OracleParameter P_SASUNTO_CORREO = new OracleParameter("P_SASUNTO_CORREO", OracleDbType.Varchar2, param.SASUNTO_CORREO, ParameterDirection.Input);
                OracleParameter P_SCUERPO_CORREO = new OracleParameter("P_SCUERPO_CORREO", OracleDbType.Varchar2, param.SCUERPO_CORREO, ParameterDirection.Input);
                OracleParameter P_SCUERPO_CORREO_DEF = new OracleParameter("P_SCUERPO_CORREO_DEF", OracleDbType.Varchar2, param.SCUERPO_CORREO_DEF, ParameterDirection.Input);

                OracleParameter P_NIDUSUARIO_MODIFICA = new OracleParameter("P_NIDUSUARIO_MODIFICA", OracleDbType.Int32, param.NIDUSUARIO_MODIFICA, ParameterDirection.Input);
                OracleParameter P_NIDACCION = new OracleParameter("P_NIDACCION", OracleDbType.Int32, param.NIDACCION, ParameterDirection.Input);
                OracleParameter P_SCUERPO_TEXTO = new OracleParameter("P_SCUERPO_TEXTO", OracleDbType.Varchar2, param.SCUERPO_TEXTO, ParameterDirection.Input);
                OracleParameter P_NCANTIDAD_DIAS = new OracleParameter("P_NCANTIDAD_DIAS", OracleDbType.Int32, param.NCANTIDAD_DIAS, ParameterDirection.Input);

                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                Console.WriteLine("Prueba :");
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] {
                    P_NIDCORREO,
                    P_NIDGRUPOSENAL,
                    P_NIDPROFILE,
                    P_SASUNTO_CORREO,
                    P_SCUERPO_CORREO,
                    P_SCUERPO_CORREO_DEF,
                    P_NIDUSUARIO_MODIFICA,
                     P_NIDACCION,
                     P_SCUERPO_TEXTO,
                     P_NCANTIDAD_DIAS,
                    P_NCODE,
                    P_SMESSAGE
                };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_CONFIG_CORREO(:P_NIDCORREO, :P_NIDGRUPOSENAL, :P_NIDPROFILE, :P_SASUNTO_CORREO, :P_SCUERPO_CORREO, :P_SCUERPO_CORREO_DEF, :P_NIDUSUARIO_MODIFICA, :P_NIDACCION, :P_SCUERPO_TEXTO, :P_NCANTIDAD_DIAS, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                output["code"] = Convert.ToInt64((P_NCODE.Value).ToString());
                output["mensaje"] = P_SMESSAGE.Value.ToString();
                Console.WriteLine("el ex: " + output["mensaje"]);
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                // this.context.Database.CloseConnection ();
                Console.WriteLine("el ex: " + ex);
                this.context.Database.CloseConnection();
                output["code"] = 2;
                output["mensaje"] = ex.Message.ToString();
                output["mensajeError"] = ex.ToString();
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return output;
        }







        public Dictionary<dynamic, dynamic> AnularResultadosCliente(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();

            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Varchar2, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_SCLIENT = new OracleParameter("P_SCLIENT", OracleDbType.Varchar2, param.SCLIENT, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] {
                    P_NPERIODO_PROCESO,
                    P_NIDALERTA,
                    P_SCLIENT,
                    P_NCODE,
                    P_SMESSAGE
                };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_ANULAR_RESULTADO(:P_NPERIODO_PROCESO, :P_NIDALERTA, :P_SCLIENT, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                output["code"] = Convert.ToInt64((P_NCODE.Value).ToString());
                output["mensaje"] = P_SMESSAGE.Value.ToString();

                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex: " + ex);
                this.context.Database.CloseConnection();
                output["code"] = 2;
                output["mensaje"] = ex.Message.ToString();
                output["mensajeError"] = ex.ToString();
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return output;
        }

        public List<Dictionary<dynamic, dynamic>> GetResultadoTratamientoHistorico(dynamic param)
        {
            List<Dictionary<dynamic, dynamic>> lista = new List<Dictionary<dynamic, dynamic>>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_SCLIENT = new OracleParameter("P_SCLIENT", OracleDbType.Varchar2, param.SCLIENT, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_SCLIENT, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_CLIENTE_TRAT_HIS(:P_NPERIODO_PROCESO, :P_SCLIENT, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    Dictionary<dynamic, dynamic> item = new Dictionary<dynamic, dynamic>();
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["SCLIENT"] = odr["SCLIENT"];
                    item["SESTADO_TRAT"] = odr["SESTADO_TRAT"];
                    item["NIDUSUARIO_MODIFICA"] = odr["NIDUSUARIO_MODIFICA"];
                    item["DCOMPDATE"] = odr["DCOMPDATE"];
                    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
                Console.WriteLine("el lista : " + lista);
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }
        public List<Dictionary<dynamic, dynamic>> GetCantidadResultadoTratamientoHistorico(dynamic param)
        {
            List<Dictionary<dynamic, dynamic>> lista = new List<Dictionary<dynamic, dynamic>>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_SESTADO_TRAT = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NCOUNT = new OracleParameter("RC1", OracleDbType.Int32, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDALERTA, P_SESTADO_TRAT, P_NCOUNT };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_CLIENTE_TRAT_HIS(:P_NPERIODO_PROCESO,:P_NIDALERTA, :P_SESTADO_TRAT , :P_NCOUNT);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                //OracleDataReader odr = ((int)P_NCOUNT.Value).GetDataReader();

                //while (odr.Read())
                //{
                //    Dictionary<dynamic, dynamic> item = new Dictionary<dynamic, dynamic>();
                //    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                //    item["SCLIENT"] = odr["SCLIENT"];
                //    item["SESTADO_TRAT"] = odr["SESTADO_TRAT"];
                //    item["NIDUSUARIO_MODIFICA"] = odr["NIDUSUARIO_MODIFICA"];
                //    item["DCOMPDATE"] = odr["DCOMPDATE"];
                //    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"];
                //    lista.Add(item);
                //}
                //odr.Close();
                this.context.Database.CloseConnection();
                Console.WriteLine("el lista : " + lista);
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }
        public Dictionary<dynamic, dynamic> UpdateStateSenialCabUsuario(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();

            try
            {
                OracleParameter P_NIDALERTA_CAB_USUARIO = new OracleParameter("P_NIDALERTA_CAB_USUARIO", OracleDbType.Int32, param.NIDALERTA_CAB_USUARIO, ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_MODIFICA = new OracleParameter("P_NIDUSUARIO_MODIFICA", OracleDbType.Varchar2, param.NIDUSUARIO_MODIFICA, ParameterDirection.Input);
                OracleParameter P_SESTADO = new OracleParameter("P_SESTADO", OracleDbType.Varchar2, param.SESTADO, ParameterDirection.Input);
                OracleParameter P_SESTADO_REV_TOTAL = new OracleParameter("P_SESTADO_REV_TOTAL", OracleDbType.Varchar2, ParameterDirection.Output);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                Console.WriteLine("Prueba :");
                P_NCODE.Size = 4000;
                P_SESTADO_REV_TOTAL.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] {
                    P_NIDALERTA_CAB_USUARIO,
                    P_NIDUSUARIO_MODIFICA,
                    P_SESTADO,
                    P_SESTADO_REV_TOTAL,
                    P_NCODE,
                    P_SMESSAGE

                };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_STATE_CAB_USU(:P_NIDALERTA_CAB_USUARIO, :P_NIDUSUARIO_MODIFICA, :P_SESTADO, :P_SESTADO_REV_TOTAL, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                Console.WriteLine("Prueba2 :");
                output["nCode"] = Convert.ToInt64((P_NCODE.Value).ToString());
                output["SESTADO_REV_TOTAL"] = (P_SESTADO_REV_TOTAL.Value).ToString();
                if (output["nCode"] != 0)
                {
                    output["sMessage"] = P_SMESSAGE.Value;
                }
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex: " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return output;
        }


        public Dictionary<dynamic, dynamic> BusquedaConcidenciaXNombre(dynamic param)
        {

            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_SORIGENARCHIVO = new OracleParameter("P_SORIGENARCHIVO", OracleDbType.Varchar2, param.SORIGENARCHIVO, ParameterDirection.Input);
                OracleParameter P_NIDTIPOLISTA = new OracleParameter("P_NIDTIPOLISTA", OracleDbType.Int32, param.NIDTIPOLISTA, ParameterDirection.Input);
                OracleParameter P_NIDPROVEEDOR = new OracleParameter("P_NIDPROVEEDOR", OracleDbType.Int32, param.NIDPROVEEDOR, ParameterDirection.Input);
                OracleParameter P_SNOMCOMPLETO = new OracleParameter("P_SNOMCOMPLETO", OracleDbType.Varchar2, param.SNOMCOMPLETO, ParameterDirection.Input);
                OracleParameter P_NTIPOCARGA = new OracleParameter("P_NTIPOCARGA", OracleDbType.Int32, param.NTIPOCARGA, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;


                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDALERTA, P_SORIGENARCHIVO, P_NIDTIPOLISTA, P_NIDPROVEEDOR, P_SNOMCOMPLETO, P_NTIPOCARGA, P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        INSUDB.PKG_BUSQ_COINCIDENCIAS_ALERTAS.SP_BUSQ_COINCIDENCIA_X_NOMBRE(:P_NPERIODO_PROCESO, :P_NIDALERTA, :P_SORIGENARCHIVO, :P_NIDTIPOLISTA, :P_NIDPROVEEDOR, :P_SNOMCOMPLETO, :P_NTIPOCARGA, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                // OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                dynamic respNcode = Convert.ToInt32(P_NCODE.Value.ToString());
                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                this.context.Database.CloseConnection();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = respNMESSAGE;

                //this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;
                //throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            //return lista;
        }

        public ResponseCoincidenciaDemanda BusquedaConcidenciaXNombreDemanda(dynamic param, int proveedor)
        {
            ResponseCoincidenciaDemanda response = new ResponseCoincidenciaDemanda();
            List<CoincidenciaDemanda> lista = new List<CoincidenciaDemanda>();
            try
            {
                OracleParameter P_SCODBUSQUEDA = new OracleParameter("P_SCODBUSQUEDA", OracleDbType.Varchar2, param.P_SCODBUSQUEDA, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.P_NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_SNOMCOMPLETO = new OracleParameter("P_SNOMCOMPLETO", OracleDbType.Varchar2, param.P_SNOMCOMPLETO, ParameterDirection.Input);
                //OracleParameter P_SCLIENT = new OracleParameter("P_SCLIENT", OracleDbType.Varchar2, param.SCLIENT, ParameterDirection.Input);
                OracleParameter P_SNOMBREUSUARIO = new OracleParameter("P_SNOMBREUSUARIO", OracleDbType.Varchar2, param.P_SNOMBREUSUARIO, ParameterDirection.Input);
                OracleParameter P_NIDPROVEEDOR = new OracleParameter("P_NIDPROVEEDOR", OracleDbType.Int32, proveedor, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;


                OracleParameter[] parameters = new OracleParameter[] { P_SCODBUSQUEDA, P_NPERIODO_PROCESO, P_SNOMCOMPLETO, P_SNOMBREUSUARIO, P_NIDPROVEEDOR, P_NCODE, P_SMESSAGE, RC1 };

                var query = @"
                    BEGIN
                        INSUDB.PKG_BUSQ_COINCIDENCIAS_ALERTAS.SP_BUSQ_COINCIDENCIA_X_NOMBRE_A_DEMANDA(:P_SCODBUSQUEDA, :P_NPERIODO_PROCESO, :P_SNOMCOMPLETO, :P_SNOMBREUSUARIO,:P_NIDPROVEEDOR, :P_NCODE, :P_SMESSAGE, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                // OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    CoincidenciaDemanda item = new CoincidenciaDemanda();
                    //item["SIDCODCLIENTE"] = odr["SIDCODCLIENTE"];
                    item.DFECHA_BUSQUEDA = odr["DFECHA_BUSQUEDA"].ToString();
                    item.SUSUARIO_BUSQUEDA = odr["SUSUARIO_BUSQUEDA"].ToString();
                    item.SNOMBRE_COMPLETO = odr["SNOMBRE_COMPLETO"].ToString();
                    item.SNOMBRE_BUSQUEDA = odr["SNOMBRE_BUSQUEDA"].ToString();//agregar parametro
                    item.SNUMDOC_BUSQUEDA = odr["SNUMDOC_BUSQUEDA"].ToString();
                    item.STIPO_DOCUMENTO = odr["STIPO_DOCUMENTO"].ToString();
                    item.SNUM_DOCUMENTO = odr["SNUM_DOCUMENTO"].ToString();
                    item.STIPO_PERSONA = odr["STIPO_PERSONA"].ToString();
                    item.SCARGO = odr["SCARGO"].ToString();
                    item.SPORCEN_COINCIDENCIA = odr["SPORCEN_COINCIDENCIA"].ToString();
                    item.NIDTIPOLISTA = int.Parse(odr["NIDTIPOLISTA"].ToString());
                    item.NIDPROVEEDOR = int.Parse(odr["NIDPROVEEDOR"].ToString());
                    item.SDESPROVEEDOR = odr["SDESPROVEEDOR"].ToString();
                    item.SDESTIPOLISTA = odr["SDESTIPOLISTA"].ToString();
                    item.SCOINCIDENCIA = odr["SCOINCIDENCIA"].ToString();
                    //item["DNULLDATE"] = odr["DNULLDATE"] == DBNull.Value ? string.Empty : odr["DNULLDATE"].ToString();
                    item.STIPOCOINCIDENCIA = odr["STIPOCOINCIDENCIA"].ToString();
                    lista.Add(item);
                }
                odr.Close();

                int respNcode = Convert.ToInt32(P_NCODE.Value.ToString());
                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                this.context.Database.CloseConnection();
                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                response.code = respNcode;
                response.mensaje = respNMESSAGE;
                response.Items = new List<CoincidenciaDemanda>();
                response.Items.AddRange(lista);
                Console.WriteLine("el lista : " + lista);


                //this.context.Database.CloseConnection();
                return response;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                response.code = 2;
                response.mensaje = ex.Message.ToString();
                response.mensajeError = ex.ToString();
                return response;
                //throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            //return lista;
        }

        public ResponseCoincidenciaDemanda BusquedaConcidenciaXNumeroDocDemanda(dynamic param, int proveedor)
        {
            ResponseCoincidenciaDemanda response = new ResponseCoincidenciaDemanda();
            List<CoincidenciaDemanda> lista = new List<CoincidenciaDemanda>();
            try
            {
                OracleParameter P_SCODBUSQUEDA = new OracleParameter("P_SCODBUSQUEDA", OracleDbType.Varchar2, param.P_SCODBUSQUEDA, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.P_NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_SNUM_DOCUMENTO = new OracleParameter("P_SNUM_DOCUMENTO", OracleDbType.Varchar2, param.P_SNUM_DOCUMENTO, ParameterDirection.Input);
                //OracleParameter P_SCLIENT = new OracleParameter("P_SCLIENT", OracleDbType.Varchar2, param.SCLIENT, ParameterDirection.Input);
                OracleParameter P_SNOMBREUSUARIO = new OracleParameter("P_SNOMBREUSUARIO", OracleDbType.Varchar2, param.P_SNOMBREUSUARIO, ParameterDirection.Input);
                OracleParameter P_NIDPROVEEDOR = new OracleParameter("P_NIDPROVEEDOR", OracleDbType.Int32, proveedor, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;


                OracleParameter[] parameters = new OracleParameter[] { P_SCODBUSQUEDA, P_NPERIODO_PROCESO, P_SNUM_DOCUMENTO, P_SNOMBREUSUARIO, P_NIDPROVEEDOR, P_NCODE, P_SMESSAGE, RC1 };

                var query = @"
                    BEGIN
                        INSUDB.PKG_BUSQ_COINCIDENCIAS_ALERTAS.SP_BUSQ_COINCIDENCIA_X_NRO_DOC_A_DEMANDA(:P_SCODBUSQUEDA, :P_NPERIODO_PROCESO, :P_SNUM_DOCUMENTO, :P_SNOMBREUSUARIO, :P_NIDPROVEEDOR, :P_NCODE, :P_SMESSAGE, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                // OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    CoincidenciaDemanda item = new CoincidenciaDemanda();
                    //item["SIDCODCLIENTE"] = odr["SIDCODCLIENTE"];
                    item.DFECHA_BUSQUEDA = odr["DFECHA_BUSQUEDA"].ToString();
                    item.SUSUARIO_BUSQUEDA = odr["SUSUARIO_BUSQUEDA"].ToString();
                    item.SNOMBRE_COMPLETO = odr["SNOMBRE_COMPLETO"].ToString();
                    item.SNOMBRE_BUSQUEDA = odr["SNOMBRE_BUSQUEDA"].ToString();
                    item.SNUMDOC_BUSQUEDA = odr["SNUMDOC_BUSQUEDA"].ToString();
                    item.STIPO_DOCUMENTO = odr["STIPO_DOCUMENTO"].ToString();
                    item.SNUM_DOCUMENTO = odr["SNUM_DOCUMENTO"].ToString();
                    item.STIPO_PERSONA = odr["STIPO_PERSONA"].ToString();
                    item.SCARGO = odr["SCARGO"].ToString();
                    item.SPORCEN_COINCIDENCIA = odr["SPORCEN_COINCIDENCIA"].ToString();
                    item.NIDTIPOLISTA = int.Parse(odr["NIDTIPOLISTA"].ToString());
                    item.NIDPROVEEDOR = int.Parse(odr["NIDPROVEEDOR"].ToString());
                    item.SDESPROVEEDOR = odr["SDESPROVEEDOR"].ToString();
                    item.SDESTIPOLISTA = odr["SDESTIPOLISTA"].ToString();
                    item.SCOINCIDENCIA = odr["SCOINCIDENCIA"].ToString();

                    //item["DNULLDATE"] = odr["DNULLDATE"] == DBNull.Value ? string.Empty : odr["DNULLDATE"].ToString();
                    item.STIPOCOINCIDENCIA = odr["STIPOCOINCIDENCIA"].ToString();
                    lista.Add(item);
                }
                odr.Close();

                dynamic respNcode = Convert.ToInt32(P_NCODE.Value.ToString());
                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                this.context.Database.CloseConnection();
                response.code = respNcode;
                response.mensaje = respNMESSAGE;
                response.Items = new List<CoincidenciaDemanda>();
                response.Items.AddRange(lista);


                //this.context.Database.CloseConnection();
                return response;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                response.code = 2;
                response.mensaje = ex.Message.ToString();
                response.mensajeError = ex.ToString();
                return response;
                //throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            //return lista;
        }

        public Dictionary<string, dynamic> getNombresADemanda(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_SCODBUSQUEDA = new OracleParameter("P_SCODBUSQUEDA", OracleDbType.Varchar2, param.P_SCODBUSQUEDA, ParameterDirection.Input);
                //OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.P_NPERIODO_PROCESO, ParameterDirection.Input);
                //OracleParameter P_SNUM_DOCUMENTO = new OracleParameter("P_SNUM_DOCUMENTO", OracleDbType.Varchar2, param.P_SNUM_DOCUMENTO, ParameterDirection.Input);
                ////OracleParameter P_SCLIENT = new OracleParameter("P_SCLIENT", OracleDbType.Varchar2, param.SCLIENT, ParameterDirection.Input);
                //OracleParameter P_SNOMBREUSUARIO = new OracleParameter("P_SNOMBREUSUARIO", OracleDbType.Varchar2, param.P_SNOMBREUSUARIO, ParameterDirection.Input);
                //OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, param.P_NCODE, ParameterDirection.Output);
                //OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, param.P_SMESSAGE, ParameterDirection.Output);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                //P_NCODE.Size = 4000;
                //P_SMESSAGE.Size = 4000;


                OracleParameter[] parameters = new OracleParameter[] { P_SCODBUSQUEDA, RC1 };

                var query = @"
                    BEGIN
                        INSUDB.PKG_BUSQ_COINCIDENCIAS_ALERTAS.SP_GET_NOMBRES_A_DEMANDA(:P_SCODBUSQUEDA, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                // OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    //item["SIDCODCLIENTE"] = odr["SIDCODCLIENTE"];
                    //item["DFECHA_BUSQUEDA"] = odr["DFECHA_BUSQUEDA"].ToString();
                    item["SCODBUSQUEDA"] = odr["SCODBUSQUEDA"].ToString();
                    item["SUSUARIO_BUSQUEDA"] = odr["SUSUARIO_BUSQUEDA"].ToString();
                    item["SNOMBRE_COMPLETO"] = odr["SNOMBRE_COMPLETO"].ToString();
                    item["STIPO_DOCUMENTO"] = odr["STIPO_DOCUMENTO"].ToString();
                    item["SNUM_DOCUMENTO"] = odr["SNUM_DOCUMENTO"].ToString();
                    //item["STIPO_PERSONA"] = odr["STIPO_PERSONA"];
                    //item["SCARGO"] = odr["SCARGO"].ToString();
                    //item["SPORCEN_COINCIDENCIA"] = odr["SPORCEN_COINCIDENCIA"].ToString();
                    //item["SLISTA"] = odr["SLISTA"].ToString();
                    //item["DNULLDATE"] = odr["DNULLDATE"] == DBNull.Value ? string.Empty : odr["DNULLDATE"].ToString();
                    lista.Add(item);
                }
                odr.Close();

                //dynamic respNcode = Convert.ToInt32(P_NCODE.Value.ToString());
                //string respNMESSAGE = P_SMESSAGE.Value.ToString();
                this.context.Database.CloseConnection();
                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                //objRespuesta["code"] = respNcode;
                //objRespuesta["mensaje"] = respNMESSAGE;
                objRespuesta["items"] = lista;
                Console.WriteLine("el lista : " + lista);


                //this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;
                //throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            //return lista;
        }

        public Dictionary<dynamic, dynamic> BusquedaConcidenciaXDoc(dynamic param)
        {

            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_SNUM_DOCUMENTO = new OracleParameter("P_SNUM_DOCUMENTO", OracleDbType.Varchar2, param.SNUM_DOCUMENTO, ParameterDirection.Input);
                OracleParameter P_NTIPOCARGA = new OracleParameter("P_NTIPOCARGA", OracleDbType.Int32, param.NTIPOCARGA, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, param.NCODE, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, param.SMESSAGE, ParameterDirection.Output);


                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDALERTA, P_SNUM_DOCUMENTO, P_NTIPOCARGA, P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        INSUDB.PKG_BUSQ_COINCIDENCIAS_ALERTAS.SP_BUSQ_COINCIDENCIA_X_DOC(:P_NPERIODO_PROCESO, :P_NIDALERTA, :P_SNUM_DOCUMENTO, :P_NTIPOCARGA, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                // OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                dynamic respNcode = Convert.ToInt32(P_NCODE.Value.ToString());
                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = respNMESSAGE;

                this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;
                //throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            //return lista;
        }

        public Dictionary<string, dynamic> BusquedaConcidenciaXDocXName(dynamic param)
        {

            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_SNUM_DOCUMENTO = new OracleParameter("P_SNUM_DOCUMENTO", OracleDbType.Varchar2, param.SNUM_DOCUMENTO, ParameterDirection.Input);
                OracleParameter P_SNOMCOMPLETO = new OracleParameter("P_SNOMCOMPLETO", OracleDbType.Varchar2, param.SNOMCOMPLETO, ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int32, param.NIDGRUPOSENAL, ParameterDirection.Input);
                OracleParameter P_NIDSUBGRUPOSEN = new OracleParameter("P_NIDSUBGRUPOSEN", OracleDbType.Int32, param.NIDSUBGRUPOSEN, ParameterDirection.Input);
                OracleParameter P_NTIPOCARGA = new OracleParameter("P_NTIPOCARGA", OracleDbType.Int32, param.NTIPOCARGA, ParameterDirection.Input);
                OracleParameter P_SCLIENT = new OracleParameter("P_SCLIENT", OracleDbType.Varchar2, param.SCLIENT, ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_MODIFICA = new OracleParameter("P_NIDUSUARIO_MODIFICA", OracleDbType.Int32, param.NIDUSUARIO_MODIFICA, ParameterDirection.Input);
                OracleParameter P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int32, param.NIDREGIMEN, ParameterDirection.Input);
                OracleParameter P_NIDTIPOLISTA = new OracleParameter("P_NIDTIPOLISTA", OracleDbType.Int32, param.NIDTIPOLISTA, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);


                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;
                P_SCLIENT.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDALERTA, P_SNUM_DOCUMENTO, P_SNOMCOMPLETO, P_NIDGRUPOSENAL, P_NIDSUBGRUPOSEN, P_NTIPOCARGA, P_SCLIENT, P_NIDUSUARIO_MODIFICA, P_NIDREGIMEN, P_NIDTIPOLISTA, P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        INSUDB.PKG_BUSQ_COINCIDENCIAS_ALERTAS.SP_BUSQ_COINCIDENCIA_MANUAL(:P_NPERIODO_PROCESO, :P_NIDALERTA, :P_SNUM_DOCUMENTO, :P_SNOMCOMPLETO, :P_NIDGRUPOSENAL,:P_NIDSUBGRUPOSEN, :P_NTIPOCARGA, :P_SCLIENT, :P_NIDUSUARIO_MODIFICA, :P_NIDREGIMEN, :P_NIDTIPOLISTA, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                // OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                Console.Write("el ncode : " + P_NCODE.Value);
                dynamic respNcode = Convert.ToInt32((P_NCODE.Value).ToString());
                Console.Write("el ncode2 : " + respNcode);
                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = respNMESSAGE;

                this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;
                //throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            //return lista;
        }


        public Dictionary<dynamic, dynamic> GetResultadoCoincidenciasPen(dynamic param)
        {
            List<Dictionary<dynamic, dynamic>> lista = new List<Dictionary<dynamic, dynamic>>();
            try
            {
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_SCLIENT = new OracleParameter("P_SCLIENT", OracleDbType.Varchar2, param.SCLIENT, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, P_SCLIENT, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_COINCIDENCIA_PEN(:P_NIDALERTA, :P_NPERIODO_PROCESO, :P_SCLIENT, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    Dictionary<dynamic, dynamic> item = new Dictionary<dynamic, dynamic>();
                    item["NTIPO_DOCUMENTO"] = odr["NTIPO_DOCUMENTO"] == DBNull.Value ? string.Empty : odr["NTIPO_DOCUMENTO"].ToString();
                    item["STIPOIDEN"] = odr["STIPOIDEN"] == DBNull.Value ? string.Empty : odr["STIPOIDEN"].ToString();
                    item["SNUM_DOCUMENTO"] = odr["SNUM_DOCUMENTO"] == DBNull.Value ? string.Empty : odr["SNUM_DOCUMENTO"].ToString();
                    item["SNOM_COMPLETO"] = odr["SNOM_COMPLETO"] == DBNull.Value ? string.Empty : odr["SNOM_COMPLETO"].ToString();
                    item["DFECHA_NACIMIENTO"] = odr["DFECHA_NACIMIENTO"] == DBNull.Value ? string.Empty : odr["DFECHA_NACIMIENTO"].ToString();
                    item["EDAD"] = odr["EDAD"];// == DBNull.Value ? string.Empty : odr["DFECHA_REGISTRO"].ToString ();
                    item["SDESESTADO"] = odr["SDESESTADO"] == DBNull.Value ? string.Empty : odr["SDESESTADO"].ToString();
                    item["SDESPRODUCTO"] = odr["SDESPRODUCTO"] == DBNull.Value ? string.Empty : odr["SDESPRODUCTO"].ToString();
                    item["SCLIENT"] = odr["SCLIENT"] == DBNull.Value ? string.Empty : odr["SCLIENT"].ToString();
                    item["SESTADO_TRAT"] = odr["SESTADO_TRAT"] == DBNull.Value ? string.Empty : odr["SESTADO_TRAT"].ToString();
                    item["SDESESTADO_TRAT"] = odr["SDESESTADO_TRAT"] == DBNull.Value ? string.Empty : odr["SDESESTADO_TRAT"].ToString();
                    item["NPERIODO_PROCESO"] = odr["NIDREGIMEN"];
                    item["SDESREGIMEN"] = odr["SDESREGIMEN"] == DBNull.Value ? string.Empty : odr["SDESREGIMEN"].ToString();
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["NIDTIPOLISTA"] = odr["NIDTIPOLISTA"];
                    item["SDESTIPOLISTA"] = odr["SDESTIPOLISTA"] == DBNull.Value ? string.Empty : odr["SDESTIPOLISTA"].ToString();
                    item["NTIPOCARGA"] = odr["NTIPOCARGA"];
                    item["NIDREGIMEN"] = odr["NIDREGIMEN"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
                Console.WriteLine("el lista : " + lista);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 0;
                objRespuesta["lista"] = lista;
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;
            }
        }


        public Dictionary<dynamic, dynamic> GetHistorialEstadoCli(dynamic param)
        {
            List<Dictionary<dynamic, dynamic>> lista = new List<Dictionary<dynamic, dynamic>>();
            try
            {

                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int64, param.NIDGRUPOSENAL, ParameterDirection.Input);
                OracleParameter P_NIDSUBGRUPOSEN = new OracleParameter("P_NIDSUBGRUPOSEN", OracleDbType.Int64, param.NIDSUBGRUPOSEN, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_SCLIENT = new OracleParameter("P_SCLIENT", OracleDbType.Varchar2, param.SCLIENT, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NIDGRUPOSENAL, P_NIDSUBGRUPOSEN, P_NIDALERTA, P_NPERIODO_PROCESO, P_SCLIENT, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_HISTORIAL_ESTADO_CLI(:P_NIDGRUPOSENAL,:P_NIDSUBGRUPOSEN, :P_NIDALERTA, :P_NPERIODO_PROCESO, :P_SCLIENT, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    Dictionary<dynamic, dynamic> item = new Dictionary<dynamic, dynamic>();
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["NIDALERTA"] = odr["NIDALERTA"];
                    item["NIDTRATCLIEHIS"] = odr["NIDTRATCLIEHIS"];
                    item["NIDREGIMEN"] = odr["NIDREGIMEN"];
                    item["SCLIENT"] = odr["SCLIENT"] == DBNull.Value ? string.Empty : odr["SCLIENT"].ToString();
                    item["SESTADO_TRAT"] = odr["SESTADO_TRAT"] == DBNull.Value ? string.Empty : odr["SESTADO_TRAT"].ToString();
                    item["NIDUSUARIO_MODIFICA"] = odr["NIDUSUARIO_MODIFICA"];
                    item["DCOMPDATE"] = odr["DCOMPDATE"] == DBNull.Value ? string.Empty : odr["DCOMPDATE"].ToString();
                    item["STIPO_PEP"] = odr["STIPO_PEP"] == DBNull.Value ? string.Empty : odr["STIPO_PEP"].ToString();
                    item["SDES_PEP"] = odr["SDES_PEP"] == DBNull.Value ? string.Empty : odr["SDES_PEP"].ToString();
                    item["SCOMENTARIO"] = odr["SCOMENTARIO"] == DBNull.Value ? string.Empty : odr["SCOMENTARIO"].ToString();
                    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"] == DBNull.Value ? string.Empty : odr["NOMBRECOMPLETO"].ToString();
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
                Console.WriteLine("el lista : " + lista);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 0;
                objRespuesta["lista"] = lista;
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;
            }
        }

        public Dictionary<string, dynamic> getCorreoCustom(Dictionary<string, dynamic> param)
        {
            //List<Dictionary<dynamic, dynamic>> lista = new List<Dictionary<dynamic, dynamic>>();
            try
            {
                OracleParameter P_NIDPROFILE = new OracleParameter("P_NIDPROFILE", OracleDbType.Int64, Convert.ToInt64(param["NIDPROFILE"]), ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int64, Convert.ToInt64(param["NIDGRUPOSENAL"]), ParameterDirection.Input);
                OracleParameter P_NIDACCION = new OracleParameter("P_NIDACCION", OracleDbType.Int64, Convert.ToInt64(param["NIDACCION"]), ParameterDirection.Input);
                OracleParameter P_SASUNTO_CORREO = new OracleParameter("P_SASUNTO_CORREO", OracleDbType.Varchar2, ParameterDirection.Output);
                OracleParameter P_SCUERPO_CORREO = new OracleParameter("P_SCUERPO_CORREO", OracleDbType.Varchar2, ParameterDirection.Output);
                //OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                P_SASUNTO_CORREO.Size = 4000;
                P_SCUERPO_CORREO.Size = 4000;


                OracleParameter[] parameters = new OracleParameter[] { P_NIDPROFILE, P_NIDGRUPOSENAL, P_NIDACCION, P_SASUNTO_CORREO, P_SCUERPO_CORREO };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_CORREO_CUSTOM(:P_NIDPROFILE, :P_NIDGRUPOSENAL, :P_NIDACCION, :P_SASUNTO_CORREO, :P_SCUERPO_CORREO);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);


                string V_SASUNTO_CORREO = P_SASUNTO_CORREO.Value.ToString();
                Console.WriteLine("el V_SASUNTO_CORREO : " + V_SASUNTO_CORREO);
                string V_SCUERPO_CORREO = P_SCUERPO_CORREO.Value.ToString();

                Console.WriteLine("el V_SCUERPO_CORREO : " + V_SCUERPO_CORREO);
                this.context.Database.CloseConnection();
                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                objRespuesta["code"] = 0;
                objRespuesta["mensaje"] = "Se consultó con exito";
                objRespuesta["SASUNTO_CORREO"] = V_SASUNTO_CORREO;
                objRespuesta["SCUERPO_CORREO"] = V_SCUERPO_CORREO;


                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex del correo custom: " + ex);
                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;
            }
        }

        public Dictionary<string, dynamic> getDataUsuarioByParams(Dictionary<string, dynamic> param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NIDPROFILE = new OracleParameter("P_NIDPROFILE", OracleDbType.Int64, param["NIDPROFILE"], ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO = new OracleParameter("P_NIDUSUARIO", OracleDbType.Int64, param["NIDUSUARIO"], ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NIDPROFILE, P_NIDUSUARIO, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_DATA_USUARIO(:P_NIDPROFILE, :P_NIDUSUARIO, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["ID_USUARIO"] = odr["ID_USUARIO"];
                    item["USUARIO"] = odr["USUARIO"].ToString();
                    item["NOMBRE_USUARIO"] = odr["NOMBRE_USUARIO"].ToString();
                    item["SEMAIL"] = odr["SEMAIL"].ToString();
                    item["NIDPROFILE"] = odr["NIDPROFILE"];
                    item["NOMBRE_PERFIL"] = odr["NOMBRE_PERFIL"].ToString();
                    item["SDESCRIPTION"] = odr["SDESCRIPTION"].ToString();
                    item["STIPO_USUARIO"] = odr["STIPO_USUARIO"].ToString();
                    item["NIDGRUPOSENAL"] = odr["NIDGRUPOSENAL"] == DBNull.Value ? string.Empty : odr["NIDGRUPOSENAL"].ToString(); ;
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
                Console.WriteLine("el lista : " + lista);
                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                objRespuesta["code"] = 0;
                objRespuesta["lista"] = lista;
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                objRespuesta["lista"] = lista;
                return objRespuesta;
            }
        }




        public Dictionary<dynamic, dynamic> InsertUpdateProfile(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {
                OracleParameter P_NIDPROFILE = new OracleParameter("P_NIDPROFILE", OracleDbType.Int64, param.NIDPROFILE, ParameterDirection.Input);
                OracleParameter P_SNAME = new OracleParameter("P_SNAME", OracleDbType.Varchar2, param.SNAME, ParameterDirection.Input);
                OracleParameter P_SDESCRIPTION = new OracleParameter("P_SDESCRIPTION", OracleDbType.Varchar2, param.SDESCRIPTION, ParameterDirection.Input);
                OracleParameter P_SACTIVE = new OracleParameter("P_SACTIVE", OracleDbType.Varchar2, param.SACTIVE, ParameterDirection.Input);
                OracleParameter P_NUSERCODE = new OracleParameter("P_NUSERCODE", OracleDbType.Int64, param.NUSERCODE, ParameterDirection.Input);
                OracleParameter P_STIPO_USUARIO = new OracleParameter("P_STIPO_USUARIO", OracleDbType.Varchar2, param.STIPO_USUARIO, ParameterDirection.Input);
                OracleParameter P_TIPOOPERACION = new OracleParameter("P_TIPOOPERACION", OracleDbType.Varchar2, param.TIPOOPERACION, ParameterDirection.Input);

                OracleParameter P_NIDPROFILE_OUT = new OracleParameter("P_NIDPROFILE_OUT", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_NIDPROFILE_OUT.Size = 4000;
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] {  P_NIDPROFILE,
                                                                        P_SNAME,
                                                                        P_SDESCRIPTION,
                                                                        P_SACTIVE,
                                                                        P_NUSERCODE,
                                                                        P_STIPO_USUARIO,
                                                                        P_TIPOOPERACION ,
                                                                        P_NIDPROFILE_OUT,
                                                                        P_NCODE,
                                                                        P_SMESSAGE };


                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_INS_UPDATE_PROFILE(:P_NIDPROFILE, :P_SNAME, :P_SDESCRIPTION, :P_SACTIVE, :P_NUSERCODE, :P_STIPO_USUARIO, :P_TIPOOPERACION,  :P_NIDPROFILE_OUT, :P_NCODE, :P_SMESSAGE );
                    END;
                    ";

                this.context.Database.OpenConnection();

                this.context.Database.ExecuteSqlCommand(query, parameters);
                // OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                //    dynamic respNcode  = Int32.Parse(P_NCODE.Value.ToString());
                Console.WriteLine("P_NCODE.Value : " + P_NCODE.Value);
                Console.WriteLine("P_NCODE.Value : " + P_NCODE.Value);
                Console.WriteLine("P_NIDPROFILE_OUT.Value.ToString() : " + P_NIDPROFILE_OUT.Value.ToString());
                dynamic respNcode = Int32.Parse(P_NCODE.Value.ToString());
                int respIDprofile = Int32.Parse(P_NIDPROFILE_OUT.Value.ToString());
                //   dynamic respNcode = P_NCODE.Value.ToString() == null ? 0 : 3;
                //   dynamic respNcode =  Convert.ToInt32(P_NCODE.Value.ToString())== null ? string.Empty : 0 ;

                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = respNMESSAGE;
                objRespuesta["id"] = respIDprofile;

                this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;

            }

        }



        public Dictionary<dynamic, dynamic> InsertUpdateProfileGrupos(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {
                OracleParameter P_NIDPROFILE = new OracleParameter("P_NIDPROFILE", OracleDbType.Int64, param.NIDPROFILE, ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int64, param.NIDGRUPOSENAL, ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_MODIFICA = new OracleParameter("P_NIDUSUARIO_MODIFICA", OracleDbType.Int64, param.NIDUSUARIO_MODIFICA, ParameterDirection.Input);
                OracleParameter P_TIPOOPERACION = new OracleParameter("P_TIPOOPERACION", OracleDbType.Varchar2, param.TIPOOPERACION, ParameterDirection.Input);

                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);


                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NIDPROFILE, P_NIDGRUPOSENAL, P_NIDUSUARIO_MODIFICA, P_TIPOOPERACION, P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_PROFILE_GRUPO(:P_NIDPROFILE, :P_NIDGRUPOSENAL, :P_NIDUSUARIO_MODIFICA, :P_TIPOOPERACION, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                // OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                //    dynamic respNcode  = Int32.Parse(P_NCODE.Value.ToString());
                Console.WriteLine("P_NCODE.Value : " + P_NCODE.Value);
                Console.WriteLine("P_NCODE.Value.ToString() : " + P_NCODE.Value.ToString());
                dynamic respNcode = Int32.Parse(P_NCODE.Value.ToString());
                //   dynamic respNcode = P_NCODE.Value.ToString() == null ? 0 : 3;
                //   dynamic respNcode =  Convert.ToInt32(P_NCODE.Value.ToString())== null ? string.Empty : 0 ;

                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = respNMESSAGE;

                this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;

            }

        }


        public List<Dictionary<string, dynamic>> GetListaResultadoGC(dynamic param)
        {
            var lista = new List<Dictionary<string, dynamic>>();
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                var P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                var P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int64, param.NIDGRUPOSENAL, ParameterDirection.Input);

                var P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int64, ParameterDirection.Output);
                var P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                var RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                var parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDGRUPOSENAL, RC1, P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_RESULTADO_GC_CONT_COLAB(:P_NPERIODO_PROCESO, :P_NIDGRUPOSENAL, :RC1, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                Console.WriteLine("P_NCODE.Value : " + P_NCODE.Value);
                Console.WriteLine("P_NCODE.Value.ToString() : " + P_NCODE.Value.ToString());
                output["nCode"] = Convert.ToInt64((P_NCODE.Value).ToString());

                if (output["nCode"] == 1)
                {
                    output["sMessage"] = P_SMESSAGE.Value.ToString();
                    Console.WriteLine("P_SMESSAGE.Value.ToString() : " + P_SMESSAGE.Value.ToString());
                }
                else
                {
                    OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                    while (odr.Read())
                    {
                        var item = new Dictionary<string, dynamic>();
                        item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                        item["STIPOIDEN"] = odr["STIPOIDEN"];
                        item["SNUM_DOCUMENTO"] = odr["SNUM_DOCUMENTO"];
                        item["NIDTIPOLISTA"] = odr["NIDTIPOLISTA"];
                        item["SDESTIPOLISTA"] = odr["SDESTIPOLISTA"];
                        item["SNOM_COMPLETO"] = odr["SNOM_COMPLETO"];
                        item["SDESTIPOLISTA"] = odr["SDESTIPOLISTA"];
                        item["SDESESTADO"] = odr["SDESESTADO"];
                        item["NTIPO_DOCUMENTO"] = odr["NTIPO_DOCUMENTO"];
                        item["DFECHA_NACIMIENTO"] = odr["DFECHA_NACIMIENTO"];
                        item["EDAD"] = odr["EDAD"];
                        // item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                        item["SCLIENT"] = odr["SCLIENT"];
                        item["SESTADO_TRAT"] = odr["SESTADO_TRAT"];
                        item["SDESESTADO_TRAT"] = odr["SDESESTADO_TRAT"];
                        item["NIDREGIMEN"] = odr["NIDREGIMEN"];
                        item["SDESPRODUCTO"] = odr["SDESPRODUCTO"];
                        item["SDESREGIMEN"] = odr["SDESREGIMEN"];
                        item["SESTADO_REVISADO"] = odr["SESTADO_REVISADO"];
                        item["NTIPOCARGA"] = odr["NTIPOCARGA"];
                        item["NIDREGIMEN_VALID"] = odr["NIDREGIMEN_VALID"];
                        item["SFALTA_ACEPTAR_COINC"] = odr["SFALTA_ACEPTAR_COINC"] == DBNull.Value ? string.Empty : odr["SFALTA_ACEPTAR_COINC"].ToString();
                        lista.Add(item);
                    }


                }
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("Error en la BD : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }

        public List<Dictionary<string, dynamic>> GetListaCargo()
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_CARGO_PEP(:RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();

                    item["id"] = odr["ID"];
                    item["label"] = odr["LABEL"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }

        public List<Dictionary<string, dynamic>> GetListaResultado(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int32, param.NIDREGIMEN, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDALERTA, P_NIDREGIMEN, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_TABLA_RESULTADO(:P_NPERIODO_PROCESO, :P_NIDALERTA, :P_NIDREGIMEN, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["SNUM_DOCUMENTO_BUSQ"] = odr["SNUM_DOCUMENTO_BUSQ"];
                    item["NIDTIPOLISTA"] = odr["NIDTIPOLISTA"];
                    item["SNOM_COMPLETO_BUSQ"] = odr["SNOM_COMPLETO_BUSQ"];
                    item["SDOC_REFERENCIA"] = odr["SDOC_REFERENCIA"];
                    item["STIPO_LISTA_REFERENCIA"] = odr["STIPO_LISTA_REFERENCIA"];
                    item["NIDREGIMEN"] = odr["NIDREGIMEN"];
                    item["RAMO"] = odr["RAMO"];
                    item["DESRAMO"] = odr["DESRAMO"];
                    item["STIPO_DOCUMENTO"] = odr["STIPO_DOCUMENTO"];
                    item["SDESTIPOLISTA"] = odr["SDESTIPOLISTA"];
                    item["NIDPROVEEDOR"] = odr["NIDPROVEEDOR"];




                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
                Console.WriteLine("el lista : " + lista);
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }


        public List<Dictionary<string, dynamic>> GetGrupoXSenal(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int64, param.NIDGRUPOSENAL, ParameterDirection.Input);

                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NIDGRUPOSENAL, RC1 };



                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_GRUPO_SENAL_X_ALERTA(:P_NIDGRUPOSENAL, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NIDALERTA"] = odr["NIDALERTA"];
                    item["SNOMBRE_ALERTA"] = odr["SNOMBRE_ALERTA"];


                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("Error :" + ex);
                Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                item["menesajeError"] = ex.Message.ToString();
                item["mensajeErrorDEtalle"] = ex;
                lista.Add(item);
                //throw ex;

                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }

        public List<Dictionary<string, dynamic>> GetListaAlertaComplemento()
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_ALERTA_COMPLEMENTO(:RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();

                    item["NIDCOMPLEMENTO"] = odr["NIDCOMPLEMENTO"];
                    item["SNOMBRE_COMPLEMENTO"] = odr["SNOMBRE_COMPLEMENTO"];
                    item["SDESCRIPCION"] = odr["SDESCRIPCION"];
                    item["SPREGUNTA"] = odr["SPREGUNTA"];
                    item["NIDALERTA"] = odr["NIDALERTA"];
                    item["NIDGRUPOSENAL"] = odr["NIDGRUPOSENAL"];
                    item["SESTADO"] = odr["SESTADO"];
                    item["NIDUSUARIO_MODIFICA"] = odr["NIDUSUARIO_MODIFICA"];
                    item["DFECHA_REGISTRO"] = odr["DFECHA_REGISTRO"];
                    item["SNOMBRE_ALERTA"] = odr["SNOMBRE_ALERTA"];
                    item["SDESGRUPO_SENAL"] = odr["SDESGRUPO_SENAL"];
                    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"];
                    item["SFILE_NAME"] = odr["SFILE_NAME"];
                    item["SRUTA_FILE_NAME"] = odr["SRUTA_FILE_NAME"];
                    item["SFILE_NAME_LARGO"] = odr["SFILE_NAME_LARGO"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }


        public Dictionary<dynamic, dynamic> InsertUpdateComplemento(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {
                OracleParameter P_NIDCOMPLEMENTO = new OracleParameter("P_NIDCOMPLEMENTO", OracleDbType.Int64, param.NIDCOMPLEMENTO, ParameterDirection.Input);
                OracleParameter P_SNOMBRE_COMPLEMENTO = new OracleParameter("P_SNOMBRE_COMPLEMENTO", OracleDbType.Varchar2, param.SNOMBRE_COMPLEMENTO, ParameterDirection.Input);
                OracleParameter P_SDESCRIPCION = new OracleParameter("P_SDESCRIPCION", OracleDbType.Varchar2, param.SDESCRIPCION, ParameterDirection.Input);
                OracleParameter P_SPREGUNTA = new OracleParameter("P_SPREGUNTA", OracleDbType.Varchar2, param.SPREGUNTA, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int64, param.NIDGRUPOSENAL, ParameterDirection.Input);
                OracleParameter P_SESTADO = new OracleParameter("P_SESTADO", OracleDbType.Varchar2, param.SESTADO, ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_MODIFICA = new OracleParameter("P_NIDUSUARIO_MODIFICA", OracleDbType.Varchar2, param.NIDUSUARIO_MODIFICA, ParameterDirection.Input);
                OracleParameter P_TIPOOPERACION = new OracleParameter("P_TIPOOPERACION", OracleDbType.Varchar2, param.TIPOOPERACION, ParameterDirection.Input);
                OracleParameter P_SFILE_NAME = new OracleParameter("P_SFILE_NAME", OracleDbType.Varchar2, param.SFILE_NAME, ParameterDirection.Input);
                OracleParameter P_SRUTA_FILE_NAME = new OracleParameter("P_SRUTA_FILE_NAME", OracleDbType.Varchar2, param.SRUTA_FILE_NAME, ParameterDirection.Input);
                OracleParameter P_SFILE_NAME_LARGO = new OracleParameter("P_SFILE_NAME_LARGO", OracleDbType.Varchar2, param.SFILE_NAME_LARGO, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);


                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NIDCOMPLEMENTO, P_SNOMBRE_COMPLEMENTO, P_SDESCRIPCION, P_SPREGUNTA, P_NIDALERTA, P_NIDGRUPOSENAL, P_SESTADO, P_NIDUSUARIO_MODIFICA, P_TIPOOPERACION, P_SFILE_NAME, P_SRUTA_FILE_NAME, P_SFILE_NAME_LARGO, P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_INS_UPDATE_ALERTA_COMPLEMENTO(:P_NIDCOMPLEMENTO, :P_SNOMBRE_COMPLEMENTO, :P_SDESCRIPCION, :P_SPREGUNTA, :P_NIDALERTA, :P_NIDGRUPOSENAL, :P_SESTADO, :P_NIDUSUARIO_MODIFICA, :P_TIPOOPERACION, :P_SFILE_NAME, :P_SRUTA_FILE_NAME, :P_SFILE_NAME_LARGO, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                Console.WriteLine("P_NCODE.Value : " + P_NCODE.Value);
                Console.WriteLine("P_NCODE.Value.ToString() : " + P_NCODE.Value.ToString());
                dynamic respNcode = Int32.Parse(P_NCODE.Value.ToString());


                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = respNMESSAGE;

                this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;

            }

        }


        public Dictionary<dynamic, dynamic> ValidarPolizaVigente(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int64, param.NIDGRUPOSENAL, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NIDREGIMEN = new OracleParameter("P_NIDREGIMEN", OracleDbType.Int64, param.NIDREGIMEN, ParameterDirection.Input);
                OracleParameter P_SCLIENT = new OracleParameter("P_SCLIENT", OracleDbType.Varchar2, param.SCLIENT, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);


                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDGRUPOSENAL, P_NIDALERTA, P_NIDREGIMEN, P_SCLIENT, P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        INSUDB.PKG_BUSQ_COINCIDENCIAS_ALERTAS.SP_VALIDAR_POLIZAS_VIGENTES(:P_NPERIODO_PROCESO, :P_NIDGRUPOSENAL, :P_NIDALERTA ,:P_NIDREGIMEN ,:P_SCLIENT, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                Console.WriteLine("P_NCODE.Value : " + P_NCODE.Value);
                Console.WriteLine("P_NCODE.Value.ToString() : " + P_NCODE.Value.ToString());
                dynamic respNcode = Int32.Parse(P_NCODE.Value.ToString());


                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = respNMESSAGE;

                this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;

            }

        }

        public List<Dictionary<string, dynamic>> GetListaComplementos(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int64, param.NIDGRUPOSENAL, ParameterDirection.Input);

                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NIDGRUPOSENAL, RC1 };



                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_COMPLEMENTO(:P_NIDALERTA, :P_NIDGRUPOSENAL, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NIDCOMPLEMENTO"] = odr["NIDCOMPLEMENTO"];
                    item["SNOMBRE_COMPLEMENTO"] = odr["SNOMBRE_COMPLEMENTO"];
                    item["SDESCRIPCION"] = odr["SDESCRIPCION"];
                    item["SPREGUNTA"] = odr["SPREGUNTA"];
                    item["NIDALERTA"] = odr["NIDALERTA"];
                    item["NIDGRUPOSENAL"] = odr["NIDGRUPOSENAL"];
                    item["SESTADO"] = odr["SESTADO"];
                    item["NIDUSUARIO_MODIFICA"] = odr["NIDUSUARIO_MODIFICA"];
                    item["DFECHA_REGISTRO"] = odr["DFECHA_REGISTRO"];



                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("Error :" + ex);
                Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                item["menesajeError"] = ex.Message.ToString();
                item["mensajeErrorDEtalle"] = ex;
                lista.Add(item);
                //throw ex;

                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }



        public List<Dictionary<string, dynamic>> GetListaPolizas(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_TIPO_DOC = new OracleParameter("P_TIPO_DOC", OracleDbType.Varchar2, param.P_TIPO_DOC, ParameterDirection.Input);
                OracleParameter P_NUMERO_DOC = new OracleParameter("P_NUMERO_DOC", OracleDbType.Varchar2, param.P_NUMERO_DOC, ParameterDirection.Input);
                OracleParameter P_NOMBRES = new OracleParameter("P_NOMBRES", OracleDbType.Varchar2, param.P_NOMBRES, ParameterDirection.Input);
                OracleParameter P_POLIZA = new OracleParameter("P_POLIZA", OracleDbType.Varchar2, param.P_POLIZA, ParameterDirection.Input);
                OracleParameter P_CODAPPLICATION = new OracleParameter("P_CODAPPLICATION", OracleDbType.Varchar2, param.P_CODAPPLICATION, ParameterDirection.Input);
                OracleParameter P_PRODUCTO = new OracleParameter("P_PRODUCTO", OracleDbType.Varchar2, param.P_PRODUCTO, ParameterDirection.Input);
                OracleParameter P_FECHA_SOLICITUD = new OracleParameter("P_FECHA_SOLICITUD", OracleDbType.Varchar2, param.P_FECHA_SOLICITUD, ParameterDirection.Input);
                OracleParameter P_ROL = new OracleParameter("P_ROL", OracleDbType.Varchar2, param.P_ROL, ParameterDirection.Input);
                OracleParameter P_TIPO = new OracleParameter("P_TIPO", OracleDbType.Varchar2, param.P_TIPO, ParameterDirection.Input);
                OracleParameter P_ESTADO = new OracleParameter("P_ESTADO", OracleDbType.Varchar2, param.P_ESTADO, ParameterDirection.Input);
                OracleParameter P_NBRANCH = new OracleParameter("P_NBRANCH", OracleDbType.Varchar2, param.P_NBRANCH, ParameterDirection.Input);
                OracleParameter P_NPAGENUM = new OracleParameter("P_NPAGENUM", OracleDbType.Int64, param.P_NPAGENUM, ParameterDirection.Input);
                OracleParameter P_NLIMITPERPAGE = new OracleParameter("P_NLIMITPERPAGE", OracleDbType.Int64, param.P_NLIMITPERPAGE, ParameterDirection.Input);
                OracleParameter P_NUSER = new OracleParameter("P_NUSER", OracleDbType.Int64, param.P_NUSER, ParameterDirection.Input);


                OracleParameter P_NTOTALROWS = new OracleParameter("P_NTOTALROWS", OracleDbType.Int64, System.Data.ParameterDirection.Output);
                OracleParameter C_TABLE = new OracleParameter("C_TABLE", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);


                OracleParameter[] parameters = new OracleParameter[] { P_TIPO_DOC, P_NUMERO_DOC, P_NOMBRES, P_POLIZA, P_CODAPPLICATION, P_PRODUCTO, P_FECHA_SOLICITUD, P_ROL, P_TIPO, P_ESTADO, P_NBRANCH, P_NPAGENUM, P_NLIMITPERPAGE, P_NUSER, P_NTOTALROWS, C_TABLE };

                P_NTOTALROWS.Size = 4000;

                var query = @"
                    BEGIN
                        INSUDB.PKG_BDU_INFO_CLIENTE.SPS_LIST_POLIZAS(:P_TIPO_DOC, :P_NUMERO_DOC, :P_NOMBRES, :P_POLIZA, :P_CODAPPLICATION, :P_PRODUCTO, :P_FECHA_SOLICITUD, :P_ROL, :P_TIPO, :P_ESTADO, :P_NBRANCH, :P_NPAGENUM, :P_NLIMITPERPAGE, :P_NUSER, :P_NTOTALROWS, :C_TABLE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)C_TABLE.Value).GetDataReader();

                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["IDRAMO"] = odr["IDRAMO"];
                    item["DES_RAMO"] = odr["DES_RAMO"];
                    item["DES_CORTA_RAMO"] = odr["DES_CORTA_RAMO"];
                    item["COD_PRODUCTO"] = odr["COD_PRODUCTO"];
                    item["DES_PRODUCTO"] = odr["DES_PRODUCTO"];
                    item["POLIZA"] = odr["POLIZA"];
                    item["NCERTIF"] = odr["NCERTIF"];
                    item["ROL"] = odr["ROL"];
                    item["DOCUMENTO"] = odr["DOCUMENTO"];
                    item["NRODOCUMENTO"] = odr["NRODOCUMENTO"];
                    item["NOMBRE"] = odr["NOMBRE"];
                    //item["DBIRTHDAT"] = odr["DBIRTHDAT"];
                    item["DIRECCION"] = odr["DIRECCION"];
                    item["INICIO_VIG_POLIZA"] = odr["INICIO_VIG_POLIZA"];
                    item["FIN_VIG_POLIZA"] = odr["FIN_VIG_POLIZA"];
                    item["INICIO_VIG_CERTIFICADO"] = odr["INICIO_VIG_CERTIFICADO"];
                    item["FEC_ANULACION"] = odr["FEC_ANULACION"];
                    item["ULTIMA_PRIMA"] = odr["ULTIMA_PRIMA"];
                    item["FEC_ULTIMA_PAGADA"] = odr["FEC_ULTIMA_PAGADA"];
                    item["PLACA"] = odr["PLACA"];
                    item["ESTADO"] = odr["ESTADO"];
                    item["TIPO"] = odr["TIPO"];
                    item["NUM_ENDOSO"] = odr["NUM_ENDOSO"];
                    item["REGISTRO"] = odr["REGISTRO"];
                    //item["MONEDA"] = odr["MONEDA"];



                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("Error :" + ex);
                Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                item["menesajeError"] = ex.Message.ToString();
                item["mensajeErrorDEtalle"] = ex;
                lista.Add(item);
                //throw ex;

                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }


        public List<Dictionary<string, dynamic>> ListaUsariosComp()
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_USUARIOS(:RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();

                    item["ID_USUARIO"] = odr["ID_USUARIO"];
                    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"];
                    item["NIDPROFILE"] = odr["NIDPROFILE"];
                    item["SNAME"] = odr["SNAME"];
                    item["NIDCARGO"] = odr["NIDCARGO"];
                    item["SDESCARGO"] = odr["SDESCARGO"];
                    item["SEMAIL"] = odr["SEMAIL"];
                    item["CONSULTA"] = 'C';
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }




        public Dictionary<dynamic, dynamic> GetUpdComplementoCab(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {
                OracleParameter P_NIDCOMP_CAB_USUARIO = new OracleParameter("P_NIDCOMP_CAB_USUARIO", OracleDbType.Int64, param.NIDCOMP_CAB_USUARIO, ParameterDirection.Input);
                OracleParameter P_NIDPREGUNTA = new OracleParameter("P_NIDPREGUNTA", OracleDbType.Int64, param.NIDPREGUNTA, ParameterDirection.Input);
                OracleParameter P_NIDORIGEN = new OracleParameter("P_NIDORIGEN", OracleDbType.Int64, param.NIDORIGEN, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NIDAGRUPA = new OracleParameter("P_NIDAGRUPA", OracleDbType.Int64, param.NIDAGRUPA, ParameterDirection.Input);
                OracleParameter P_NIND_RESPUESTA = new OracleParameter("P_NIND_RESPUESTA", OracleDbType.Int64, param.NIND_RESPUESTA, ParameterDirection.Input);
                OracleParameter P_SCOMENTARIO = new OracleParameter("P_SCOMENTARIO", OracleDbType.Varchar2, param.SCOMENTARIO, ParameterDirection.Input);
                OracleParameter P_SRUTA_PDF = new OracleParameter("P_SRUTA_PDF", OracleDbType.Varchar2, param.SRUTA_PDF, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);


                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NIDCOMP_CAB_USUARIO, P_SCOMENTARIO, P_SRUTA_PDF, P_NCODE, P_SMESSAGE };



                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_COMPLEMENTO_CAB(:P_NIDCOMP_CAB_USUARIO, :P_SCOMENTARIO, :P_SRUTA_PDF, :P_NCODE, :P_SMESSAGE );
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                Console.WriteLine("P_NCODE.Value : " + P_NCODE.Value);
                Console.WriteLine("P_NCODE.Value.ToString() : " + P_NCODE.Value.ToString());
                dynamic respNcode = Int32.Parse(P_NCODE.Value.ToString());


                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = respNMESSAGE;

                this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;

            }

        }




        public Dictionary<dynamic, dynamic> GetInsCormularioComplUsu(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NIDCOMPLEMENTO = new OracleParameter("P_NIDCOMPLEMENTO", OracleDbType.Int64, param.NIDCOMPLEMENTO, ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_RESPONSABLE = new OracleParameter("P_NIDUSUARIO_RESPONSABLE", OracleDbType.Int64, param.NIDUSUARIO_RESPONSABLE, ParameterDirection.Input);
                OracleParameter P_SNOMBRE_RESPONSABLE = new OracleParameter("P_SNOMBRE_RESPONSABLE", OracleDbType.Varchar2, param.SNOMBRE_RESPONSABLE, ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_ASIGNADO = new OracleParameter("P_NIDUSUARIO_ASIGNADO", OracleDbType.Int64, param.NIDUSUARIO_ASIGNADO, ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int64, param.NIDGRUPOSENAL, ParameterDirection.Input);
                OracleParameter SRUTA_PDF = new OracleParameter("SRUTA_PDF", OracleDbType.Varchar2, param.SRUTA_PDF, ParameterDirection.Input);
                OracleParameter P_SRUTA_FILE_NAME = new OracleParameter("P_SRUTA_FILE_NAME", OracleDbType.Varchar2, param.SRUTA_FILE_NAME, ParameterDirection.Input);
                OracleParameter P_NIDAGRUPA = new OracleParameter("P_NIDAGRUPA", OracleDbType.Int64, param.NIDAGRUPA, ParameterDirection.Input);
                OracleParameter P_SFILE_NAME = new OracleParameter("P_SFILE_NAME", OracleDbType.Varchar2, param.SFILE_NAME, ParameterDirection.Input);
                OracleParameter P_SFILE_NAME_LARGO = new OracleParameter("P_SFILE_NAME_LARGO", OracleDbType.Varchar2, param.SFILE_NAME_LARGO, ParameterDirection.Input);
                OracleParameter P_SNOM_COMPLEMENTO = new OracleParameter("P_SNOM_COMPLEMENTO", OracleDbType.Varchar2, param.SNOM_COMPLEMENTO, ParameterDirection.Input);
                OracleParameter P_SFILE_NAME_RE = new OracleParameter("P_SFILE_NAME_RE", OracleDbType.Varchar2, param.SFILE_NAME_RE, ParameterDirection.Input);
                OracleParameter P_SRUTA_FILE_NAME_RE = new OracleParameter("P_SRUTA_FILE_NAME_RE", OracleDbType.Varchar2, param.SRUTA_FILE_NAME_RE, ParameterDirection.Input);
                OracleParameter P_SFILE_NAME_LARGO_RE = new OracleParameter("P_SFILE_NAME_LARGO_RE", OracleDbType.Varchar2, param.SFILE_NAME_LARGO_RE, ParameterDirection.Input);

                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                OracleParameter P_ID_COMPL = new OracleParameter("P_ID_COMPL", OracleDbType.Int64, ParameterDirection.Output);


                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;


                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDALERTA, P_NIDCOMPLEMENTO, P_NIDUSUARIO_RESPONSABLE, P_SNOMBRE_RESPONSABLE, P_NIDUSUARIO_ASIGNADO, P_NIDGRUPOSENAL, SRUTA_PDF, P_SRUTA_FILE_NAME, P_NIDAGRUPA, P_SFILE_NAME, P_SFILE_NAME_LARGO, P_SNOM_COMPLEMENTO, P_SFILE_NAME_RE, P_SRUTA_FILE_NAME_RE, P_SFILE_NAME_LARGO_RE, P_NCODE, P_SMESSAGE, P_ID_COMPL };



                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_INS_FORMULARIO_COMPL_USU(:P_NPERIODO_PROCESO, :P_NIDALERTA, :P_NIDCOMPLEMENTO, :P_NIDUSUARIO_RESPONSABLE, :P_SNOMBRE_RESPONSABLE, :P_NIDUSUARIO_ASIGNADO, :P_NIDGRUPOSENAL, :SRUTA_PDF, :P_SRUTA_FILE_NAME, :P_NIDAGRUPA, :P_SFILE_NAME, :P_SFILE_NAME_LARGO, :P_SNOM_COMPLEMENTO, :P_SFILE_NAME_RE, :P_SRUTA_FILE_NAME_RE, :P_SFILE_NAME_LARGO_RE, :P_NCODE, :P_SMESSAGE, :P_ID_COMPL );
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                Console.WriteLine("P_NCODE.Value : " + P_NCODE.Value);
                Console.WriteLine("P_NCODE.Value.ToString() : " + P_NCODE.Value.ToString());
                dynamic respNcode = Int32.Parse(P_NCODE.Value.ToString());
                dynamic resID = Int32.Parse(P_ID_COMPL.Value.ToString());

                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = respNMESSAGE;
                objRespuesta["ID"] = resID;

                this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;

            }

        }



        public Dictionary<dynamic, dynamic> GetValFormularioCompl(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NIDCOMPLEMENTO = new OracleParameter("P_NIDCOMPLEMENTO", OracleDbType.Int64, param.NIDCOMPLEMENTO, ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_RESPONSABLE = new OracleParameter("P_NIDUSUARIO_RESPONSABLE", OracleDbType.Int64, param.NIDUSUARIO_RESPONSABLE, ParameterDirection.Input);
                OracleParameter P_NVALIDATE = new OracleParameter("P_NVALIDATE", OracleDbType.Int64, ParameterDirection.Output);


                P_NVALIDATE.Size = 4000;
                //P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDALERTA, P_NIDCOMPLEMENTO, P_NIDUSUARIO_RESPONSABLE, P_NVALIDATE };



                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_VAL_FORMULARIO_CCOMPL(:P_NPERIODO_PROCESO, :P_NIDALERTA, :P_NIDCOMPLEMENTO, :P_NIDUSUARIO_RESPONSABLE, :P_NVALIDATE );
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                Console.WriteLine("P_NCODE.Value : " + P_NVALIDATE.Value);
                Console.WriteLine("P_NCODE.Value.ToString() : " + P_NVALIDATE.Value.ToString());
                dynamic respNcode = Int32.Parse(P_NVALIDATE.Value.ToString());


                //string respNMESSAGE = P_SMESSAGE.Value.ToString();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = "Validacion";

                this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;

            }

        }


        public List<Dictionary<string, dynamic>> GetListaCompUsu(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int64, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NIDCOMPLEMENTO = new OracleParameter("P_NIDCOMPLEMENTO", OracleDbType.Int64, param.NIDCOMPLEMENTO, ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_RESPONSABLE = new OracleParameter("P_NIDUSUARIO_RESPONSABLE", OracleDbType.Int64, param.NIDUSUARIO_RESPONSABLE, ParameterDirection.Input);


                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDALERTA, P_NIDCOMPLEMENTO, P_NIDUSUARIO_RESPONSABLE, RC1 };



                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_COMPL_USU(:P_NPERIODO_PROCESO, :P_NIDALERTA, :P_NIDCOMPLEMENTO, :P_NIDUSUARIO_RESPONSABLE, :RC1 );
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["NIDCOMPLEMENTO"] = odr["NIDCOMPLEMENTO"];
                    item["NIDALERTA"] = odr["NIDALERTA"];
                    item["NIDUSUARIO_RESPONSABLE"] = odr["NIDUSUARIO_RESPONSABLE"];
                    item["NIDUSUARIO_ASIGNADO"] = odr["NIDUSUARIO_ASIGNADO"];
                    item["DFECHA_ESTADO_MOVIMIENTO"] = odr["DFECHA_ESTADO_MOVIMIENTO"];
                    item["NIDUSUARIO_ASIGNADO"] = odr["NIDUSUARIO_ASIGNADO"];
                    item["SRUTA_PDF"] = odr["SRUTA_PDF"];
                    item["SESTADO"] = odr["SESTADO"];
                    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"];
                    item["SNAME"] = odr["SNAME"];
                    item["SDESCARGO"] = odr["SDESCARGO"];



                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("Error :" + ex);
                Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                item["menesajeError"] = ex.Message.ToString();
                item["mensajeErrorDEtalle"] = ex;
                lista.Add(item);
                //throw ex;

                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }

        public List<Dictionary<string, dynamic>> GetListaComplementoUsuario(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);

                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, RC1 };



                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_COMPLEMENTOS_USU(:P_NPERIODO_PROCESO, :RC1 );
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["NIDCOMPLEMENTO"] = odr["NIDCOMPLEMENTO"];
                    item["NIDALERTA"] = odr["NIDALERTA"];
                    item["NIDUSUARIO_RESPONSABLE"] = odr["NIDUSUARIO_RESPONSABLE"];
                    item["SNOMBRE_RESPONSABLE"] = odr["SNOMBRE_RESPONSABLE"];
                    item["NIDUSUARIO_ASIGNADO"] = odr["NIDUSUARIO_ASIGNADO"];
                    item["DFECHA_ESTADO_MOVIMIENTO"] = odr["DFECHA_ESTADO_MOVIMIENTO"];
                    item["NIDUSUARIO_ASIGNADO"] = odr["NIDUSUARIO_ASIGNADO"];
                    item["SRUTA_PDF"] = odr["SRUTA_PDF"];
                    item["SESTADO"] = odr["SESTADO"];
                    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"];
                    item["SNAME"] = odr["SNAME"];
                    item["SDESCARGO"] = odr["SDESCARGO"];
                    item["SNOMBRE_ESTADO"] = odr["SNOMBRE_ESTADO"];
                    item["SEMAIL"] = odr["SEMAIL"];
                    item["SFILE_NAME"] = odr["SFILE_NAME"] == DBNull.Value ? string.Empty : odr["SFILE_NAME"].ToString();
                    item["SRUTA_FILE_NAME"] = odr["SRUTA_FILE_NAME"] == DBNull.Value ? string.Empty : odr["SRUTA_FILE_NAME"].ToString();
                    item["SFILE_NAME_LARGO"] = odr["SFILE_NAME_LARGO"] == DBNull.Value ? string.Empty : odr["SFILE_NAME_LARGO"].ToString();
                    item["CONSULTA"] = "STORE";
                    item["ARCHIVO"] = "";
                    item["SNOM_COMPLEMENTO"] = odr["SNOM_COMPLEMENTO"];
                    item["NIDCOMP_CAB_USUARIO"] = odr["NIDCOMP_CAB_USUARIO"];
                    item["SFILE_NAME_RE"] = odr["SFILE_NAME_RE"] == DBNull.Value ? string.Empty : odr["SFILE_NAME_RE"].ToString();
                    item["SRUTA_FILE_NAME_RE"] = odr["SRUTA_FILE_NAME_RE"] == DBNull.Value ? string.Empty : odr["SRUTA_FILE_NAME_RE"].ToString();
                    item["SFILE_NAME_LARGO_RE"] = odr["SFILE_NAME_LARGO_RE"] == DBNull.Value ? string.Empty : odr["SFILE_NAME_LARGO_RE"].ToString();
                    item["SCOMENTARIO"] = odr["SCOMENTARIO"] == DBNull.Value ? string.Empty : odr["SCOMENTARIO"].ToString();
                    item["DFECHA_RECEPCION"] = odr["DFECHA_RECEPCION"] == DBNull.Value ? string.Empty : odr["DFECHA_RECEPCION"].ToString();








                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("Error :" + ex);
                Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                item["menesajeError"] = ex.Message.ToString();
                item["mensajeErrorDEtalle"] = ex;
                lista.Add(item);
                //throw ex;

                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }



        public Dictionary<dynamic, dynamic> GetUpdPssUsuario(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {
                OracleParameter P_ID_USUARIO = new OracleParameter("P_ID_USUARIO", OracleDbType.Int64, param.ID_USUARIO, ParameterDirection.Input);
                OracleParameter P_PASSWORD = new OracleParameter("P_PASSWORD", OracleDbType.Varchar2, param.PASSWORD, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);


                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_ID_USUARIO, P_PASSWORD, P_NCODE, P_SMESSAGE };



                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_PASS_USUARIO(:P_ID_USUARIO, :P_PASSWORD, :P_NCODE, :P_SMESSAGE );
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                Console.WriteLine("P_NCODE.Value : " + P_NCODE.Value);
                Console.WriteLine("P_NCODE.Value.ToString() : " + P_NCODE.Value.ToString());
                dynamic respNcode = Int32.Parse(P_NCODE.Value.ToString());



                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = respNMESSAGE;

                this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;

            }

        }

        public Dictionary<dynamic, dynamic> GetActPassUsuario(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {
                OracleParameter P_ID_USUARIO = new OracleParameter("P_ID_USUARIO", OracleDbType.Int64, param.ID_USUARIO, ParameterDirection.Input);
                OracleParameter P_NINDICA_ACT = new OracleParameter("P_NINDICA_ACT", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_NINDICA_ACT.Size = 4000;
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_ID_USUARIO, P_NINDICA_ACT, P_NCODE, P_SMESSAGE };



                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_ACT_PASS_USUARIO(:P_ID_USUARIO, :P_NINDICA_ACT, :P_NCODE, :P_SMESSAGE );
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                Console.WriteLine("P_NCODE.Value : " + P_NCODE.Value);
                Console.WriteLine("P_NCODE.Value.ToString() : " + P_NCODE.Value.ToString());
                dynamic respNcode = Int32.Parse(P_NCODE.Value.ToString());
                dynamic indica_act = Int32.Parse(P_NINDICA_ACT.Value.ToString());


                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = respNMESSAGE;
                objRespuesta["indicador"] = indica_act;

                this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;

            }

        }



        public List<Dictionary<string, dynamic>> GetAlertaResupuesta(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, System.Data.ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int32, param.NIDGRUPOSENAL, System.Data.ParameterDirection.Input);
                OracleParameter P_VALIDADOR = new OracleParameter("P_VALIDADOR", OracleDbType.Int32, param.VALIDADOR, System.Data.ParameterDirection.Input);

                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter p_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter p_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                p_NCODE.Size = 4000;
                p_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDGRUPOSENAL, P_VALIDADOR, RC1, p_NCODE, p_SMESSAGE };

                var query = @"BEGIN
                LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_ALERTAS_RESPUESTA(:P_NPERIODO_PROCESO, :P_NIDGRUPOSENAL, :P_VALIDADOR , :RC1, :P_NCODE,:P_SMESSAGE);
                END;
                ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NIDALERTA_CABECERA"] = odr["NIDALERTA_CABECERA"];
                    item["NIDALERTA"] = odr["NIDALERTA"];
                    item["SNOMBRE_ALERTA"] = odr["SNOMBRE_ALERTA"];
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["NIDRESPUESTA"] = odr["NIDRESPUESTA"];
                    item["SESTADO"] = odr["SESTADO"];
                    item["SCOMENTARIO"] = odr["SCOMENTARIO"];
                    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"];
                    item["SPERFIL"] = odr["PERFIL"];
                    item["SCARGO"] = odr["SCARGO"];
                    item["SNOMBRE_ESTADO"] = odr["SNOMBRE_ESTADO"];
                    item["NIDREGIMEN"] = odr["NIDREGIMEN"];

                    lista.Add(item);
                }
                odr.Close();
                /*
                odr = ((OracleRefCursor) RC2.Value).GetDataReader();
                if (odr.HasRows) {
                    while(odr.Read()) {
                    }
                }
                */
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine(" Error : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }

        public Dictionary<dynamic, dynamic> InsCorreoUsuario(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {
                OracleParameter P_ID_USUARIO = new OracleParameter("P_ID_USUARIO", OracleDbType.Int64, param.ID_USUARIO, ParameterDirection.Input);
                OracleParameter P_NIDCORREO = new OracleParameter("P_NIDCORREO", OracleDbType.Int64, param.NIDCORREO, ParameterDirection.Input);
                OracleParameter P_NIDPROFILE = new OracleParameter("P_NIDPROFILE", OracleDbType.Int64, param.NIDPROFILE, ParameterDirection.Input);
                OracleParameter P_NIDACCION = new OracleParameter("P_NIDACCION", OracleDbType.Int64, param.NIDACCION, ParameterDirection.Input);
                OracleParameter P_TIPOOPERACION = new OracleParameter("P_TIPOOPERACION", OracleDbType.Varchar2, param.TIPOOPERACION, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);


                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_ID_USUARIO, P_NIDCORREO, P_NIDPROFILE, P_NIDACCION, P_TIPOOPERACION, P_NCODE, P_SMESSAGE };



                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_INS_CORREOS_USUARIOS_ASIGNADOS(:P_ID_USUARIO, :P_NIDCORREO, :P_NIDPROFILE, :P_NIDACCION, :P_TIPOOPERACION, :P_NCODE, :P_SMESSAGE );
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);


                dynamic respNcode = Int32.Parse(P_NCODE.Value.ToString());

                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = respNMESSAGE;


                this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;

            }

        }


        public List<Dictionary<string, dynamic>> getListaUsuarioCorreos(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NIDACCION = new OracleParameter("P_NIDACCION", OracleDbType.Int64, param.NIDACCION, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NIDACCION, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_USUARIOS_CORREOS(:P_NIDACCION, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();

                    item["userId"] = odr["ID_USUARIO"];
                    item["userFullName"] = odr["NOMBRECOMPLETO"];
                    item["userEmail"] = odr["SEMAIL"];
                    item["SASUNTO_CORREO"] = odr["SASUNTO_CORREO"];
                    item["SCUERPO_CORREO"] = odr["SCUERPO_CORREO"];



                    //item["CONSULTA"] = 'C';
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("Error :" + ex);
                Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                item["menesajeError"] = ex.Message.ToString();
                item["mensajeErrorDEtalle"] = ex;
                lista.Add(item);
            }
            return lista;
        }


        public List<Dictionary<string, dynamic>> getListaAdjuntos(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NIDACCION = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NIDACCION, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_ADJUNTOS(:P_NPERIODO_PROCESO, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();

                    item["NIDALERTA"] = odr["NIDALERTA"];
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["NIDADJUNTO"] = odr["NIDADJUNTO"];
                    item["SRUTA_ADJUNTO"] = odr["SRUTA_ADJUNTO"];
                    item["NIDUSUARIO_MODIFICA"] = odr["NIDUSUARIO_MODIFICA"];
                    item["NREGIMEN"] = odr["NREGIMEN"];
                    item["STIPO_CARGA"] = odr["STIPO_CARGA"];
                    item["NIDALERTA_CABECERA"] = odr["NIDALERTA_CABECERA"];
                    item["NID_USUARIO"] = odr["NID_USUARIO"];




                    //item["CONSULTA"] = 'C';
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("Error :" + ex);
                Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                item["menesajeError"] = ex.Message.ToString();
                item["mensajeErrorDEtalle"] = ex;
                lista.Add(item);
            }
            return lista;
        }



        public Dictionary<string, dynamic> getDeleteAdjuntos(dynamic param)
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {
                //AlertStatusResponseDTO result = new AlertStatusResponseDTO ();

                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NREGIMEN = new OracleParameter("P_NREGIMEN", OracleDbType.Int32, param.NREGIMEN, ParameterDirection.Input);
                OracleParameter P_STIPO_CARGA = new OracleParameter("P_STIPO_CARGA", OracleDbType.Varchar2, param.STIPO_CARGA, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;
                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA, P_NPERIODO_PROCESO, P_NREGIMEN, P_STIPO_CARGA, P_NCODE, P_SMESSAGE };

                var query = @"
                 BEGIN
                     LAFT.PKG_LAFT_GESTION_ALERTAS.SP_DEL_ADJUNTOS(:P_NIDALERTA, :P_NPERIODO_PROCESO, :P_NREGIMEN, :P_STIPO_CARGA, :P_NCODE, :P_SMESSAGE);
                 END;
                 ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                output["nCode"] = Int32.Parse(P_NCODE.Value.ToString());
                output["sMessage"] = P_SMESSAGE.Value.ToString();

                this.context.Database.CloseConnection();
                Console.WriteLine("/n El output : " + output);
                return output;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Utils.ExceptionManager.resolve(ex);
                output["nCode"] = 2;
                output["sMessageError"] = ex.Message.ToString();
                output["sMessageErrorDetalle"] = ex;
                return output;
            }
        }


        public Dictionary<dynamic, dynamic> GetRegistrarDatosExcelGC(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {
                //AlertStatusResponseDTO result = new AlertStatusResponseDTO ();
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NTIPO_DOCUMENTO = new OracleParameter("P_NTIPO_DOCUMENTO", OracleDbType.Varchar2, param.NTIPO_DOCUMENTO, ParameterDirection.Input);
                OracleParameter P_SNUM_DOCUMENTO = new OracleParameter("P_SNUM_DOCUMENTO", OracleDbType.Varchar2, param.SNUM_DOCUMENTO, ParameterDirection.Input);
                OracleParameter P_SNOM_COMPLETO = new OracleParameter("P_SNOM_COMPLETO", OracleDbType.Varchar2, param.SNOM_COMPLETO, ParameterDirection.Input);
                OracleParameter P_DFECHA_NACIMIENTO = new OracleParameter("P_DFECHA_NACIMIENTO", OracleDbType.Varchar2, param.DFECHA_NACIMIENTO, ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO = new OracleParameter("P_NIDUSUARIO", OracleDbType.Int32, param.NIDUSUARIO, ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int32, param.NIDGRUPOSENAL, ParameterDirection.Input);
                OracleParameter P_NIDSUBGRUPOSEN = new OracleParameter("P_NIDSUBGRUPOSEN", OracleDbType.Int32, param.NIDSUBGRUPOSEN, ParameterDirection.Input);
                OracleParameter P_SNUM_DOCUMENTO_EMPRESA = new OracleParameter("P_SNUM_DOCUMENTO_EMPRESA", OracleDbType.Varchar2, param.SNUM_DOCUMENTO_EMPRESA, ParameterDirection.Input);
                OracleParameter P_SNOM_COMPLETO_EMPRESA = new OracleParameter("P_SNOM_COMPLETO_EMPRESA", OracleDbType.Varchar2, param.SNOM_COMPLETO_EMPRESA, ParameterDirection.Input);
                OracleParameter P_NACIONALIDAD = new OracleParameter("P_NACIONALIDAD", OracleDbType.Varchar2, param.NACIONALIDAD, ParameterDirection.Input);
                OracleParameter P_CARGO = new OracleParameter("P_CARGO", OracleDbType.Varchar2, param.CARGO, ParameterDirection.Input);
                OracleParameter P_SACTUALIZA = new OracleParameter("P_SACTUALIZA", OracleDbType.Varchar2, param.SACTUALIZA, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;
                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NTIPO_DOCUMENTO, P_SNUM_DOCUMENTO, P_SNOM_COMPLETO, P_DFECHA_NACIMIENTO, P_NIDUSUARIO, P_NIDGRUPOSENAL, P_NIDSUBGRUPOSEN, P_SNUM_DOCUMENTO_EMPRESA, P_SNOM_COMPLETO_EMPRESA, P_NACIONALIDAD, P_CARGO, P_SACTUALIZA, P_NCODE, P_SMESSAGE };

                var query = @"
                 BEGIN
                     LAFT.PKG_LAFT_GESTION_ALERTAS.SP_CARGA_INFO_EXTERNA(:P_NPERIODO_PROCESO, :P_NTIPO_DOCUMENTO, :P_SNUM_DOCUMENTO, :P_SNOM_COMPLETO, :P_DFECHA_NACIMIENTO, :P_NIDUSUARIO, :P_NIDGRUPOSENAL, :P_NIDSUBGRUPOSEN, :P_SNUM_DOCUMENTO_EMPRESA, :P_SNOM_COMPLETO_EMPRESA, :P_NACIONALIDAD, :P_CARGO, :P_SACTUALIZA, :P_NCODE, :P_SMESSAGE );
                 END;
                 ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);




                output["nCode"] = Int32.Parse(P_NCODE.Value.ToString());
                output["sMessage"] = P_SMESSAGE.Value.ToString();

                this.context.Database.CloseConnection();
                Console.WriteLine("/n El output : " + output);
                return output;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Utils.ExceptionManager.resolve(ex);
                output["nCode"] = 2;
                output["sMessageError"] = ex.Message.ToString();
                output["sMessageErrorDetalle"] = ex;
                return output;
            }
        }


        public Dictionary<dynamic, dynamic> DelEliminarDemanda(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {
                //AlertStatusResponseDTO result = new AlertStatusResponseDTO ();
                OracleParameter P_SCODBUSQUEDA = new OracleParameter("P_SCODBUSQUEDA", OracleDbType.Varchar2, param.SCODBUSQUEDA, ParameterDirection.Input);

                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;
                OracleParameter[] parameters = new OracleParameter[] { P_SCODBUSQUEDA, P_NCODE, P_SMESSAGE };

                var query = @"
                 BEGIN
                     INSUDB.PKG_BUSQ_COINCIDENCIAS_ALERTAS.SP_DEL_CARGA_BUSQUEDA_DEMANDA( :P_SCODBUSQUEDA, :P_NCODE, :P_SMESSAGE );
                 END;
                 ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);




                output["nCode"] = Int32.Parse(P_NCODE.Value.ToString());
                output["sMessage"] = P_SMESSAGE.Value.ToString();

                this.context.Database.CloseConnection();
                Console.WriteLine("/n El output : " + output);
                return output;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Utils.ExceptionManager.resolve(ex);
                output["nCode"] = 2;
                output["sMessageError"] = ex.Message.ToString();
                output["sMessageErrorDetalle"] = ex;
                return output;
            }
        }


        //public List<User> GetValidarExisteCorreo(dynamic param)
        public Dictionary<dynamic, dynamic> GetValidarExisteCorreo(dynamic param)

        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();

            string usuario = param.USUARIO;
            string correo = param.CORREO;

            //List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                var ListUsuario = from r in this.context.Users
                                  where r.sUser == usuario && r.sUserEmail == correo
                                  select new { ID_USUARIO = r.nUserId, FECHA = r.fecha_encrip };

                if (ListUsuario.Count() > 0)
                {
                    output["CODE"] = 1;
                    output["MESSAGE"] = "Existe Usuario";
                    output["ID"] = ListUsuario.ToList();
                }
                else
                {
                    output["CODE"] = 0;
                    output["MESSAGE"] = "NO EXISTE USUARIO";
                }

                return output;
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                //return new List<User>();
                output["CODE"] = 2;
                output["MESSAGE"] = ex;
                return output;
            }
        }


        public Dictionary<dynamic, dynamic> GetUpdUsuarioEncriptado(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {
                OracleParameter P_USUARIO = new OracleParameter("P_USUARIO", OracleDbType.Varchar2, param.USUARIO, ParameterDirection.Input);
                OracleParameter P_SEMAIL = new OracleParameter("P_SEMAIL", OracleDbType.Varchar2, param.SEMAIL, ParameterDirection.Input);
                OracleParameter P_SENCRIP_USUARIO = new OracleParameter("P_SENCRIP_USUARIO", OracleDbType.Varchar2, param.SENCRIP_USUARIO, ParameterDirection.Input);
                OracleParameter P_SENCRIP_CORREO = new OracleParameter("P_SENCRIP_CORREO", OracleDbType.Varchar2, param.SENCRIP_CORREO, ParameterDirection.Input);
                OracleParameter P_SENCRIP_ID = new OracleParameter("P_SENCRIP_ID", OracleDbType.Varchar2, param.SENCRIP_ID, ParameterDirection.Input);
                OracleParameter P_SDESENCRIP_ID = new OracleParameter("P_SDESENCRIP_ID", OracleDbType.Varchar2, param.SDESENCRIP_ID, ParameterDirection.Input);
                OracleParameter P_SHASH = new OracleParameter("P_SHASH", OracleDbType.Varchar2, param.SHASH, ParameterDirection.Input);

                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);


                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_USUARIO, P_SEMAIL, P_SENCRIP_USUARIO, P_SENCRIP_CORREO, P_SENCRIP_ID, P_SDESENCRIP_ID, P_SHASH, P_NCODE, P_SMESSAGE };



                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_USUARIO_ENCRIPTADO(:P_USUARIO, :P_SEMAIL, :P_SENCRIP_USUARIO, :P_SENCRIP_CORREO, :P_SENCRIP_ID, :P_SDESENCRIP_ID, :P_SHASH, :P_NCODE, :P_SMESSAGE );
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                Console.WriteLine("P_NCODE.Value : " + P_NCODE.Value);
                Console.WriteLine("P_NCODE.Value.ToString() : " + P_NCODE.Value.ToString());
                dynamic respNcode = Int32.Parse(P_NCODE.Value.ToString());



                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = respNMESSAGE;

                this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;

            }

        }

        //public Dictionary<string, dynamic> GetValidarHash(dynamic param)
        public List<Dictionary<string, dynamic>> GetValidarHash(dynamic param)
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            string hash = param.HASH;



            //List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                var ListUsuario = from r in this.context.Users
                                  where r.sHash == hash
                                  //select r;
                                  select new { SENCRIP_USUARIO = r.sEncrip_Usuario, SENCRIP_CORREO = r.sEncrip_Correo, SDESENCRIP_ID = r.sDesencrip_Id, SENCRIP_ID = r.sSencrip_Id, ID_USER = r.nUserId };

                ListUsuario.ToList();
                //output["SENCRIP_USUARIO"] = ListUsuario;
                Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();

                item["RESPUESTA"] = ListUsuario;

                lista.Add(item);
                return lista;
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                output["CODE"] = 2;
                output["MESSAGE"] = ex;
                return lista;
            }

        }



        public Dictionary<dynamic, dynamic> GetInsertaHistorialUsuario(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {

                /*OracleParameter P_NID_USUARIO = new OracleParameter("P_NID_USUARIO", OracleDbType.Int32, param.NID_USUARIO, ParameterDirection.Input);
                OracleParameter P_SUSUARIO = new OracleParameter("P_SUSUARIO", OracleDbType.Varchar2, param.SUSUARIO, ParameterDirection.Input);
                OracleParameter P_SNOMBRECOMPLETO = new OracleParameter("P_SNOMBRECOMPLETO", OracleDbType.Varchar2, param.SNOMBRECOMPLETO, ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_MODIFICA = new OracleParameter("P_NIDUSUARIO_MODIFICA", OracleDbType.Int32, param.NIDUSUARIO_MODIFICA, ParameterDirection.Input);
                OracleParameter P_NIDPROFILE = new OracleParameter("P_NIDPROFILE", OracleDbType.Int32, param.NIDPROFILE, ParameterDirection.Input);
                OracleParameter P_SEMAIL = new OracleParameter("P_SEMAIL", OracleDbType.Varchar2, param.SEMAIL, ParameterDirection.Input);
                OracleParameter P_NID_CARGO = new OracleParameter("P_NID_CARGO", OracleDbType.Int32, param.NID_CARGO, ParameterDirection.Input);
                OracleParameter P_SCAMPOMODIF = new OracleParameter("P_SCAMPOMODIF", OracleDbType.Varchar2, param.SCAMPOMODIF, ParameterDirection.Input);
                OracleParameter P_NESTADO = new OracleParameter("P_NESTADO", OracleDbType.Varchar2, param.NESTADO, ParameterDirection.Input);
                
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;
                OracleParameter[] parameters = new OracleParameter[] { P_NID_USUARIO, P_SUSUARIO, P_SNOMBRECOMPLETO, P_NIDUSUARIO_MODIFICA, P_NIDPROFILE, P_SEMAIL, P_NID_CARGO, P_SCAMPOMODIF, P_NESTADO, P_NCODE, P_SMESSAGE };

                var query = @"
                 BEGIN
                     LAFT.PKG_LAFT_USUARIO.SP_UDP_HIS_USUARIO(:P_NID_USUARIO, :P_SUSUARIO, :P_SNOMBRECOMPLETO, :P_NIDUSUARIO_MODIFICA, :P_NIDPROFILE, :P_SEMAIL, :P_NID_CARGO, :P_SCAMPOMODIF, :P_NESTADO, :P_NCODE, :P_SMESSAGE );
                 END;
                 ";*/

                OracleParameter P_NID_USUARIO = new OracleParameter("P_NID_USUARIO", OracleDbType.Int32, param.userId, ParameterDirection.Input);
                OracleParameter P_SUSUARIO = new OracleParameter("P_SUSUARIO", OracleDbType.Varchar2, param.userName, ParameterDirection.Input);
                OracleParameter P_SNOMBRECOMPLETO = new OracleParameter("P_SNOMBRECOMPLETO", OracleDbType.Varchar2, param.userFullName, ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_MODIFICA = new OracleParameter("P_NIDUSUARIO_MODIFICA", OracleDbType.Int32, param.userUpd, ParameterDirection.Input);
                OracleParameter P_NIDPROFILE = new OracleParameter("P_NIDPROFILE", OracleDbType.Int32, param.userRolId, ParameterDirection.Input);
                OracleParameter P_SEMAIL = new OracleParameter("P_SEMAIL", OracleDbType.Varchar2, param.userEmail, ParameterDirection.Input);
                OracleParameter P_NID_CARGO = new OracleParameter("P_NID_CARGO", OracleDbType.Int32, param.cargoId, ParameterDirection.Input);
                OracleParameter P_SCAMPOMODIF = new OracleParameter("P_SCAMPOMODIF", OracleDbType.Varchar2, param.modificado, ParameterDirection.Input);
                OracleParameter P_NESTADO = new OracleParameter("P_NESTADO", OracleDbType.Varchar2, param.state, ParameterDirection.Input);

                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;
                OracleParameter[] parameters = new OracleParameter[] { P_NID_USUARIO, P_SUSUARIO, P_SNOMBRECOMPLETO, P_NIDUSUARIO_MODIFICA, P_NIDPROFILE, P_SEMAIL, P_NID_CARGO, P_SCAMPOMODIF, P_NESTADO, P_NCODE, P_SMESSAGE };

                var query = @"
                 BEGIN
                     LAFT.PKG_LAFT_USUARIO.SP_UDP_HIS_USUARIO(:P_NID_USUARIO, :P_SUSUARIO, :P_SNOMBRECOMPLETO, :P_NIDUSUARIO_MODIFICA, :P_NIDPROFILE, :P_SEMAIL, :P_NID_CARGO, :P_SCAMPOMODIF, :P_NESTADO, :P_NCODE, :P_SMESSAGE );
                 END;
                 ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);




                output["nCode"] = Int32.Parse(P_NCODE.Value.ToString());
                output["sMessage"] = P_SMESSAGE.Value.ToString();

                this.context.Database.CloseConnection();
                Console.WriteLine("/n El output : " + output);
                return output;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Utils.ExceptionManager.resolve(ex);
                output["nCode"] = 2;
                output["sMessageError"] = ex.Message.ToString();
                output["sMessageErrorDetalle"] = ex;
                return output;
            }
        }


        public Dictionary<string, dynamic> getCorreoCustomAction(Dictionary<string, dynamic> param)
        {
            //List<Dictionary<dynamic, dynamic>> lista = new List<Dictionary<dynamic, dynamic>>();
            try
            {
                OracleParameter P_NIDACCION = new OracleParameter("P_NIDACCION", OracleDbType.Int64, Convert.ToInt64(param["NIDACCION"]), ParameterDirection.Input);
                OracleParameter P_SASUNTO_CORREO = new OracleParameter("P_SASUNTO_CORREO", OracleDbType.Varchar2, ParameterDirection.Output);
                OracleParameter P_SCUERPO_CORREO = new OracleParameter("P_SCUERPO_CORREO", OracleDbType.Varchar2, ParameterDirection.Output);
                //OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                P_SASUNTO_CORREO.Size = 4000;
                P_SCUERPO_CORREO.Size = 4000;


                OracleParameter[] parameters = new OracleParameter[] { P_NIDACCION, P_SASUNTO_CORREO, P_SCUERPO_CORREO };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_CORREO_CUSTOM_ACTION(:P_NIDACCION, :P_SASUNTO_CORREO, :P_SCUERPO_CORREO);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);


                string V_SASUNTO_CORREO = P_SASUNTO_CORREO.Value.ToString();
                Console.WriteLine("el V_SASUNTO_CORREO : " + V_SASUNTO_CORREO);
                string V_SCUERPO_CORREO = P_SCUERPO_CORREO.Value.ToString();

                Console.WriteLine("el V_SCUERPO_CORREO : " + V_SCUERPO_CORREO);
                this.context.Database.CloseConnection();
                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                objRespuesta["code"] = 0;
                objRespuesta["mensaje"] = "Se consultó con exito";
                objRespuesta["SASUNTO_CORREO"] = V_SASUNTO_CORREO;
                objRespuesta["SCUERPO_CORREO"] = V_SCUERPO_CORREO;


                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex del correo custom: " + ex);
                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;
            }
        }

        public Dictionary<string, dynamic> getObtenerHash(Dictionary<string, dynamic> param)
        {
            //List<Dictionary<dynamic, dynamic>> lista = new List<Dictionary<dynamic, dynamic>>();
            try
            {
                OracleParameter P_ID_USUARIO = new OracleParameter("P_ID_USUARIO", OracleDbType.Int64, Convert.ToInt64(param["ID_USUARIO"]), ParameterDirection.Input);
                OracleParameter P_SHASH = new OracleParameter("P_SHASH", OracleDbType.Varchar2, ParameterDirection.Output);

                //OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                P_SHASH.Size = 4000;



                OracleParameter[] parameters = new OracleParameter[] { P_ID_USUARIO, P_SHASH };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_OBTENER_HASH(:P_ID_USUARIO, :P_SHASH);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);


                string V_SHASH = P_SHASH.Value.ToString();


                //Console.WriteLine("el V_SCUERPO_CORREO : " + V_SCUERPO_CORREO);
                this.context.Database.CloseConnection();
                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                objRespuesta["code"] = 0;
                objRespuesta["mensaje"] = "Se consultó con exito";
                objRespuesta["SHASH"] = V_SHASH;



                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex del correo custom: " + ex);
                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;
            }
        }


        public List<Dictionary<string, dynamic>> GetListaEmpresas(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();

            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);


                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, RC1 };
                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_EMPRESAS(:P_NPERIODO_PROCESO, :RC1);
                    END;
                    ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["SNUMERO_DOC"] = odr["SNUMERO_DOC"];
                    item["SRAZON_SOCIAL"] = odr["SRAZON_SOCIAL"];
                    item["SDESPRODUCTO"] = odr["SDESPRODUCTO"];
                    item["NIDREGIMEN"] = odr["NIDREGIMEN"];


                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();

            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }


        public List<Dictionary<string, dynamic>> GetListaResultadoProveedorContraparte(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_RESULTADO_PROVEEDOR_CONTRAPARTE(:P_NPERIODO_PROCESO, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["SNUM_DOCUMENTO_BUSQ"] = odr["SNUM_DOCUMENTO_BUSQ"];
                    item["NIDTIPOLISTA"] = odr["NIDTIPOLISTA"];
                    item["SNOM_COMPLETO_BUSQ"] = odr["SNOM_COMPLETO_BUSQ"];
                    item["SDOC_REFERENCIA"] = odr["SDOC_REFERENCIA"];
                    item["STIPO_LISTA_REFERENCIA"] = odr["STIPO_LISTA_REFERENCIA"];
                    item["NIDREGIMEN"] = odr["NIDREGIMEN"];
                    item["SDESTIPOLISTA"] = odr["SDESTIPOLISTA"];
                    item["NIDPROVEEDOR"] = odr["NIDPROVEEDOR"];
                    item["SNUM_DOCUMENTO_EMPRESA"] = odr["SNUM_DOCUMENTO_EMPRESA"];
                    item["SNOM_COMPLETO_EMPRESA"] = odr["SNOM_COMPLETO_EMPRESA"];
                    item["NIDGRUPOSENAL"] = odr["NIDGRUPOSENAL"];
                    item["NIDSUBGRUPOSEN"] = odr["NIDSUBGRUPOSEN"];
                    item["STIPOIDEN_BUSQ"] = odr["STIPOIDEN_BUSQ"];
                    item["SDESPROVEEDOR"] = odr["SDESPROVEEDOR"];
                    item["TD_SDESCRIP"] = odr["TD_SDESCRIP"];


                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.Write("el ex 1224:" + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }


        public Dictionary<dynamic, dynamic> GetFechaInicioPeriodo()

        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            //List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();

            try
            {
                var Periodo = from r in this.context.Periodos
                              where r.SESTADO == 1
                              select new { NPERIODO_PROCESO = r.NPERIODO_PROCESO, FECHA = r.DFECINI };

                Periodo.ToList();
                //output["SENCRIP_USUARIO"] = ListUsuario;
                //Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();

                //item["RESPUESTA"] = Periodo;
                //var Fecha = Periodo.ToList()[0].FECHA;
                //lista.Add(item);
                output["FechaInicioPeriodo"] = Periodo.ToList()[0].FECHA.ToString("dd/MM/yyyy");
                output["Periodo"] = Periodo.ToList()[0].NPERIODO_PROCESO;
                return output;
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                //return new List<User>();
                output["CODE"] = 2;
                output["MESSAGE"] = ex;
                return output;
            }
        }

        public Dictionary<dynamic, dynamic> GetObtenerCantidadEnvio()

        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            //List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();

            try
            {
                var Correo = from r in this.context.Correos
                             where r.NIDACCION == 4
                             select r;

                Correo.ToList();
                //output["SENCRIP_USUARIO"] = ListUsuario;
                //Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();

                //item["RESPUESTA"] = Periodo;
                //var Fecha = Periodo.ToList()[0].FECHA;
                //lista.Add(item);
                output["Cantidad"] = Correo.ToList()[0].NCANTIDAD_DIAS;

                return output;
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                //return new List<User>();
                output["CODE"] = 2;
                output["MESSAGE"] = ex;
                return output;
            }
        }


        public Dictionary<dynamic, dynamic> GetObtenerCantidadEnviados(dynamic param)

        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            //List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            int PeriodoActual = param.NPERIODO_PROCESO;
            try
            {
                var Alerta = from r in this.context.Alerta
                             where r.NPERIODO_PROCESO == PeriodoActual
                             select r;

                Alerta.ToList();
                //output["SENCRIP_USUARIO"] = ListUsuario;
                //Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();

                //item["RESPUESTA"] = Periodo;
                //var Fecha = Periodo.ToList()[0].FECHA;
                //lista.Add(item);
                output["CantidadEnviado"] = Alerta.ToList()[0].NCONTADOR_REENVIO;

                return output;
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                //return new List<User>();
                output["CODE"] = 2;
                output["MESSAGE"] = ex;
                return output;
            }
        }


        public List<Dictionary<string, dynamic>> GetListaResultadoEstadosCorreos(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_RESULTADO_ESTADOS_CORREOS(:P_NPERIODO_PROCESO, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"];
                    item["ID_USUARIO"] = odr["ID_USUARIO"];
                    item["NIDGRUPOSENAL"] = odr["NIDGRUPOSENAL"];
                    item["SDESGRUPO_SENAL"] = odr["SDESGRUPO_SENAL"];
                    item["SEMAIL"] = odr["SEMAIL"];
                    item["DFECHA_REENVIO"] = odr["DFECHA_REENVIO"];

                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.Write("el ex 1224:" + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }


        public List<Dictionary<string, dynamic>> GetFechaFeriado(dynamic param)

        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_FECHA = new OracleParameter("P_FECHA", OracleDbType.Varchar2, param.FECHA, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_FECHA, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_VALIDA_FERIADOS(:P_FECHA, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["ANIO"] = odr["ANIO"];
                    item["FECHA"] = odr["FECHA"];


                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.Write("el ex 1224:" + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;


        }


        public Dictionary<dynamic, dynamic> GetActualizarFechaEnvio(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);


                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NCODE, P_SMESSAGE };



                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_ACTUALIZAR_FECHA_ENVIO(:P_NPERIODO_PROCESO, :P_NCODE, :P_SMESSAGE );
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                Console.WriteLine("P_NCODE.Value : " + P_NCODE.Value);
                Console.WriteLine("P_NCODE.Value.ToString() : " + P_NCODE.Value.ToString());
                dynamic respNcode = Int32.Parse(P_NCODE.Value.ToString());



                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = respNMESSAGE;

                this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;

            }

        }


        public Dictionary<dynamic, dynamic> GetActualizarContadorCorreo(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_ASIGNADO = new OracleParameter("P_NIDUSUARIO_ASIGNADO", OracleDbType.Int64, param.NIDUSUARIO_ASIGNADO, ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int64, param.NIDGRUPOSENAL, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);


                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDUSUARIO_ASIGNADO, P_NIDGRUPOSENAL, P_NCODE, P_SMESSAGE };



                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_ACTUALIZAR_CONTADOR_ENVIO_CORREO(:P_NPERIODO_PROCESO, :P_NIDUSUARIO_ASIGNADO, :P_NIDGRUPOSENAL, :P_NCODE, :P_SMESSAGE );
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                Console.WriteLine("P_NCODE.Value : " + P_NCODE.Value);
                Console.WriteLine("P_NCODE.Value.ToString() : " + P_NCODE.Value.ToString());
                dynamic respNcode = Int32.Parse(P_NCODE.Value.ToString());



                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = respNMESSAGE;

                this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;

            }

        }


        public List<Dictionary<string, dynamic>> GetListaOtrosClientes(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int32, param.NIDGRUPOSENAL, ParameterDirection.Input);
                OracleParameter P_NIDSUBGRUPOSEN = new OracleParameter("P_NIDSUBGRUPOSEN", OracleDbType.Int32, param.NIDSUBGRUPOSEN, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDGRUPOSENAL, P_NIDSUBGRUPOSEN, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_OTROS_CLIENTES(:P_NPERIODO_PROCESO, :P_NIDGRUPOSENAL, :P_NIDSUBGRUPOSEN, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["SDESGRUPO_SENAL"] = odr["SDESGRUPO_SENAL"];
                    item["SDESSUBGRUPO_SENAL"] = odr["SDESSUBGRUPO_SENAL"];
                    item["SNUM_DOCUMENTO"] = odr["SNUM_DOCUMENTO"];
                    item["SNOM_COMPLETO"] = odr["SNOM_COMPLETO"];
                    item["TIPO_DOCUMENTO"] = odr["TIPO_DOCUMENTO"];
                    item["RUC"] = odr["RUC"];
                    item["RAZON_SOCIAL"] = odr["RAZON_SOCIAL"];
                    item["NTIPO_DOCUMENTO"] = odr["NTIPO_DOCUMENTO"];
                    item["IDDOC_TYPE"] = odr["IDDOC_TYPE"];
                    item["NACIONALIDAD"] = odr["NACIONALIDAD"];
                    item["CARGO"] = odr["CARGO"];



                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.Write("el ex 1224:" + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }


        public Dictionary<dynamic, dynamic> actualizarTratamiento(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NIDALERTA = new OracleParameter("P_NIDALERTA", OracleDbType.Int32, param.NIDALERTA, ParameterDirection.Input);//
                OracleParameter P_NIDGRUPOSENAL = new OracleParameter("P_NIDGRUPOSENAL", OracleDbType.Int64, param.NIDGRUPOSENAL, ParameterDirection.Input);
                OracleParameter P_NIDSUBGRUPOSEN = new OracleParameter("P_NIDSUBGRUPOSEN", OracleDbType.Int64, param.NIDSUBGRUPOSEN, ParameterDirection.Input);//
                OracleParameter P_NTIPOCARGA = new OracleParameter("P_NTIPOCARGA", OracleDbType.Int64, 2, ParameterDirection.Input);
                OracleParameter P_SCLIENT = new OracleParameter("P_SCLIENT", OracleDbType.NVarchar2, param.SCLIENT, ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_MODIFICA = new OracleParameter("P_NIDUSUARIO_MODIFICA", OracleDbType.Int64, param.NIDUSUARIO_MODIFICA, ParameterDirection.Input);
                OracleParameter P_NIDPROVEEDOR = new OracleParameter("P_NIDPROVEEDOR", OracleDbType.Int64, param.NIDPROVEEDOR, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;
                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NIDALERTA, P_NIDGRUPOSENAL, P_NIDSUBGRUPOSEN, P_NTIPOCARGA, P_SCLIENT, P_NIDUSUARIO_MODIFICA, P_NIDPROVEEDOR, P_NCODE, P_SMESSAGE };
                var query = @"
                    BEGIN
                        INSUDB.PKG_BUSQ_COINCIDENCIAS_ALERTAS.SP_INS_TRATAMIENTO_CLIENTE(: P_NPERIODO_PROCESO, :P_NIDALERTA, :P_NIDGRUPOSENAL, :P_NIDSUBGRUPOSEN, :P_NTIPOCARGA, :P_SCLIENT, :P_NIDUSUARIO_MODIFICA, :P_NIDPROVEEDOR, :P_NCODE, :P_SMESSAGE );
                    END;
                    ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                Console.WriteLine("P_NCODE.Value : " + P_NCODE.Value);
                Console.WriteLine("P_NCODE.Value.ToString() : " + P_NCODE.Value.ToString());
                dynamic respNcode = Int32.Parse(P_NCODE.Value.ToString());
                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = respNMESSAGE;

                this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;

            }

        }


        public List<Dictionary<string, dynamic>> GetListaInformes(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_VALIDADOR = new OracleParameter("P_VALIDADOR", OracleDbType.Int32, param.VALIDADOR, ParameterDirection.Input);
                OracleParameter P_INFORME = new OracleParameter("P_INFORME", OracleDbType.Varchar2, param.INFORME, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_VALIDADOR, P_INFORME, RC1 };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_INFORMES(:P_NPERIODO_PROCESO , :P_VALIDADOR, :P_INFORME, :RC1);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["NPERIODO_PROCESO"] = odr["NPERIODO_PROCESO"];
                    item["DFECHA_ESTADO"] = odr["DFECHA_ESTADO"];
                    item["SRUTA_ARCHIVO"] = odr["SRUTA_ARCHIVO"];
                    item["SESTADO"] = odr["SESTADO"];
                    item["DFECHA_REGISTRO"] = odr["DFECHA_REGISTRO"];
                    item["NIDUSUARIO_MODIFICA"] = odr["NIDUSUARIO_MODIFICA"];
                    item["SNOMBRE_ARCHIVO_CORTO"] = odr["SNOMBRE_ARCHIVO_CORTO"];
                    item["SNOMBRE_ARCHIVO"] = odr["SNOMBRE_ARCHIVO"];
                    item["NOMBRECOMPLETO"] = odr["NOMBRECOMPLETO"];
                    item["FECHA_PERIODO"] = odr["FECHA_PERIODO"];
                    item["FILE"] = "file";
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return lista;
        }



        public Dictionary<dynamic, dynamic> UpdInformes(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_SRUTA_ARCHIVO = new OracleParameter("P_SRUTA_ARCHIVO", OracleDbType.Varchar2, param.SRUTA_ARCHIVO, ParameterDirection.Input);
                OracleParameter P_NIDUSUARIO_MODIFICA = new OracleParameter("P_NIDUSUARIO_MODIFICA", OracleDbType.Int64, param.NIDUSUARIO_MODIFICA, ParameterDirection.Input);
                OracleParameter P_SNOMBRE_ARCHIVO_CORTO = new OracleParameter("P_SNOMBRE_ARCHIVO_CORTO", OracleDbType.Varchar2, param.SNOMBRE_ARCHIVO_CORTO, ParameterDirection.Input);
                OracleParameter P_SNOMBRE_ARCHIVO = new OracleParameter("P_SNOMBRE_ARCHIVO", OracleDbType.Varchar2, param.SNOMBRE_ARCHIVO, ParameterDirection.Input);
                OracleParameter P_VALIDADOR = new OracleParameter("P_VALIDADOR", OracleDbType.Varchar2, param.VALIDADOR, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);


                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_SRUTA_ARCHIVO, P_NIDUSUARIO_MODIFICA, P_SNOMBRE_ARCHIVO_CORTO, P_SNOMBRE_ARCHIVO, P_VALIDADOR, P_NCODE, P_SMESSAGE };



                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_INFORMES(:P_NPERIODO_PROCESO, :P_SRUTA_ARCHIVO, :P_NIDUSUARIO_MODIFICA, :P_SNOMBRE_ARCHIVO_CORTO, :P_SNOMBRE_ARCHIVO, :P_VALIDADOR, :P_NCODE, :P_SMESSAGE );
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);


                dynamic respNcode = Int32.Parse(P_NCODE.Value.ToString());

                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = respNMESSAGE;


                this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;

            }

        }

        public Dictionary<dynamic, dynamic> getCorreo_OC()

        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            //List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();

            try
            {
                var Alerta = from r in this.context.Correo_OC
                                 //where r.CORREO == PeriodoActual
                             select r;

                Alerta.ToList();

                output["correo"] = Alerta.ToList()[0].CORREO;

                return output;
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                //return new List<User>();
                output["CODE"] = 2;
                output["MESSAGE"] = ex;
                return output;
            }
        }

        public Dictionary<dynamic, dynamic> UpdActualizarCorreoOC(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {


                OracleParameter P_CORREO = new OracleParameter("P_CORREO", OracleDbType.Varchar2, param.CORREO, ParameterDirection.Input);
                OracleParameter P_PASSWORD = new OracleParameter("P_PASSWORD", OracleDbType.Varchar2, param.PASSWORD, ParameterDirection.Input);

                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);


                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_CORREO, P_PASSWORD, P_NCODE, P_SMESSAGE };



                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_ACTUALIZAR_CORREO_OC(:P_CORREO, :P_PASSWORD, :P_NCODE, :P_SMESSAGE );
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);


                dynamic respNcode = Int32.Parse(P_NCODE.Value.ToString());

                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = respNMESSAGE;


                this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;

            }

        }


        public Dictionary<dynamic, dynamic> getObtenerContrasennaCorreo()

        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            //List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();

            try
            {
                var Alerta = from r in this.context.Correo_OC
                                 //where r.CORREO == PeriodoActual
                             select r;

                Alerta.ToList();


                output["CORREO"] = Alerta.ToList()[0].CORREO;
                output["PASSWORD"] = Alerta.ToList()[0].PASSWORD;

                return output;
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                //return new List<User>();
                output["CODE"] = 2;
                output["MESSAGE"] = ex;
                return output;
            }
        }


        public Dictionary<dynamic, dynamic> UpdRutaComplementos(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {
                OracleParameter P_NIDALERTA_CAB_USUARIO = new OracleParameter("P_NIDALERTA_CAB_USUARIO", OracleDbType.Int64, param.NIDALERTA_CAB_USUARIO, ParameterDirection.Input);
                OracleParameter P_SFILE_NAME = new OracleParameter("P_SFILE_NAME", OracleDbType.Varchar2, param.SFILE_NAME, ParameterDirection.Input);
                OracleParameter P_SRUTA_FILE_NAME = new OracleParameter("P_SRUTA_FILE_NAME", OracleDbType.Varchar2, param.SRUTA_FILE_NAME, ParameterDirection.Input);
                OracleParameter P_SFILE_NAME_LARGO = new OracleParameter("P_SFILE_NAME_LARGO", OracleDbType.Varchar2, param.SFILE_NAME_LARGO, ParameterDirection.Input);
                OracleParameter P_VALIDADOR = new OracleParameter("P_VALIDADOR", OracleDbType.Varchar2, param.VALIDADOR, ParameterDirection.Input);

                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);


                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NIDALERTA_CAB_USUARIO, P_SFILE_NAME, P_SRUTA_FILE_NAME, P_SFILE_NAME_LARGO, P_VALIDADOR, P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_UPD_RUTA_COMPLEMENTO( :P_NIDALERTA_CAB_USUARIO, :P_SFILE_NAME, :P_SRUTA_FILE_NAME, :P_SFILE_NAME_LARGO, :P_VALIDADOR, :P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                Console.WriteLine("P_NCODE.Value : " + P_NCODE.Value);
                Console.WriteLine("P_NCODE.Value.ToString() : " + P_NCODE.Value.ToString());
                dynamic respNcode = Int32.Parse(P_NCODE.Value.ToString());


                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = respNMESSAGE;

                this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;

            }

        }


        public Dictionary<dynamic, dynamic> getCuerpoCorreo(dynamic param)
        {
            //List<Dictionary<dynamic, dynamic>> lista = new List<Dictionary<dynamic, dynamic>>();
            try
            {
                OracleParameter P_NIDACCION = new OracleParameter("P_NIDACCION", OracleDbType.Int64, param.NIDACCION, ParameterDirection.Input);
                OracleParameter P_SASUNTO_CORREO = new OracleParameter("P_SASUNTO_CORREO", OracleDbType.Varchar2, ParameterDirection.Output);
                OracleParameter P_SCUERPO_CORREO = new OracleParameter("P_SCUERPO_CORREO", OracleDbType.Varchar2, ParameterDirection.Output);
                //OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                P_SASUNTO_CORREO.Size = 4000;
                P_SCUERPO_CORREO.Size = 4000;


                OracleParameter[] parameters = new OracleParameter[] { P_NIDACCION, P_SASUNTO_CORREO, P_SCUERPO_CORREO };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_CORREO_CUSTOM_ACTION(:P_NIDACCION, :P_SASUNTO_CORREO, :P_SCUERPO_CORREO);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);


                string V_SASUNTO_CORREO = P_SASUNTO_CORREO.Value.ToString();
                Console.WriteLine("el V_SASUNTO_CORREO : " + V_SASUNTO_CORREO);
                string V_SCUERPO_CORREO = P_SCUERPO_CORREO.Value.ToString();

                Console.WriteLine("el V_SCUERPO_CORREO : " + V_SCUERPO_CORREO);
                this.context.Database.CloseConnection();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 0;
                objRespuesta["mensaje"] = "Se consultó con exito";
                objRespuesta["SASUNTO_CORREO"] = V_SASUNTO_CORREO;
                objRespuesta["SCUERPO_CORREO"] = V_SCUERPO_CORREO;


                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex del correo custom: " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;
            }
        }

        public Dictionary<dynamic, dynamic> DesenciptarPassUsuario(dynamic param)
        {
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {
                OracleParameter NID_USUARIO = new OracleParameter("NID_USUARIO", OracleDbType.Int64, param.NID_USUARIO, ParameterDirection.Input);
                OracleParameter PASS = new OracleParameter("PASS", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int64, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);


                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { NID_USUARIO, PASS, P_NCODE, P_SMESSAGE };



                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_DESENCRIPTAR_PASS(:NID_USUARIO, :PASS, :P_NCODE, :P_SMESSAGE );
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);


                dynamic respNcode = Int32.Parse(P_NCODE.Value.ToString());

                string respNMESSAGE = P_SMESSAGE.Value.ToString();
                string resPass = PASS.Value.ToString();
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = respNcode;
                objRespuesta["mensaje"] = respNMESSAGE;
                objRespuesta["pss"] = resPass;

                this.context.Database.CloseConnection();
                return objRespuesta;
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el ex : " + ex);
                Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;

            }

        }



        public RegistroNegativoModel GetRegistrarDatosExcelRegistronegativo(RegistroNegativoModel item)
        {
            try
            {
                OracleConnection connection = new OracleConnection(Utils.Config.AppSetting["ConnectionString:LAFT"]);
                connection.Open();
                OracleParameter P_SNID = new OracleParameter("P_SNID", OracleDbType.Varchar2, item.items.numero, ParameterDirection.Input);
                OracleParameter P_STIPOPERSONA = new OracleParameter("P_STIPOPERSONA", OracleDbType.Varchar2, item.items.tipoPersona, ParameterDirection.Input);
                OracleParameter P_STIPODOC_PAIS = new OracleParameter("P_STIPODOC_PAIS", OracleDbType.Varchar2, item.items.pais, ParameterDirection.Input);
                OracleParameter P_SNUMIDENTIDAD = new OracleParameter("P_SNUMIDENTIDAD", OracleDbType.Varchar2, item.items.numeroDocumento, ParameterDirection.Input);
                OracleParameter P_SAPE_PATERNO = new OracleParameter("P_SAPE_PATERNO", OracleDbType.Varchar2, item.items.apellidoParteno, ParameterDirection.Input);
                OracleParameter P_SAPE_MATERNO = new OracleParameter("P_SAPE_MATERNO", OracleDbType.Varchar2, item.items.apellidoMaterno, ParameterDirection.Input);
                OracleParameter P_SNOMBRES_RS = new OracleParameter("P_SNOMBRES_RS", OracleDbType.Varchar2, item.items.nombre, ParameterDirection.Input);
                OracleParameter P_SSENAL_LAFT = new OracleParameter("P_SSENAL_LAFT", OracleDbType.Varchar2, item.items.senalLaft, ParameterDirection.Input);
                OracleParameter P_SFILTRO1 = new OracleParameter("P_SFILTRO1", OracleDbType.Varchar2, item.items.filtro, ParameterDirection.Input);
                OracleParameter P_SFEDESCUBRIMIENTO = new OracleParameter("P_SFEDESCUBRIMIENTO", OracleDbType.Varchar2, item.items.fechaNacimiento, ParameterDirection.Input);
                OracleParameter P_SDOCREFERENCIA = new OracleParameter("P_SDOCREFERENCIA", OracleDbType.Varchar2, item.items.documentoReferencia, ParameterDirection.Input);
                OracleParameter P_STIPOLISTA = new OracleParameter("P_STIPOLISTA", OracleDbType.Varchar2, item.items.tipoLista, ParameterDirection.Input);
                OracleParameter P_SNUMDOCUMENTO = new OracleParameter("P_SNUMDOCUMENTO", OracleDbType.Varchar2, item.items.numeroDocumento2, ParameterDirection.Input);
                OracleParameter P_SNOM_COMPLETO = new OracleParameter("P_SNOM_COMPLETO", OracleDbType.Varchar2, item.items.nombreCompleto, ParameterDirection.Input);
                OracleParameter[] parameters = new OracleParameter[] { P_SNID, P_STIPOPERSONA, P_STIPODOC_PAIS, P_SNUMIDENTIDAD, P_SAPE_PATERNO, P_SAPE_MATERNO, P_SNOMBRES_RS, P_SSENAL_LAFT, P_SFILTRO1, P_SFEDESCUBRIMIENTO, P_SDOCREFERENCIA, P_STIPOLISTA, P_SNUMDOCUMENTO, P_SNOM_COMPLETO };


                // create command and set properties  
                OracleCommand cmd = connection.CreateCommand();
                //DateTime sysdate = DateTime.Now;
                cmd.CommandText = "INSERT INTO TBL_LAFT_CLIENTE_REG_NEGATIVO ( SNID, STIPOPERSONA, STIPODOC_PAIS, SNUMIDENTIDAD, SAPE_PATERNO, SAPE_MATERNO, SNOMBRES_RS, SSENAL_LAFT, SFILTRO1, SFEDESCUBRIMIENTO, SDOCREFERENCIA, STIPOLISTA, SNUMDOCUMENTO, SNOM_COMPLETO) " +
                                  "VALUES (:1, :2, :3, :4, :5, :6, :7, :8, :9, :10, :11, :12, :13, :14)";
                cmd.ArrayBindCount = item.items.numero.Length;
                cmd.Parameters.AddRange(parameters);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                item.mensaje = ex.Message.ToString();
                item.codigo = 2;
            }
            return item;
        }


        public List<Dictionary<string, dynamic>> GetListaRegistroNegativo(dynamic param)
        {
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            Dictionary<dynamic, dynamic> output = new Dictionary<dynamic, dynamic>();
            try
            {
                OracleParameter P_SNUMIDENTIDAD = new OracleParameter("P_SNUMIDENTIDAD", OracleDbType.Varchar2, param.SNUMIDENTIDAD, ParameterDirection.Input);
                OracleParameter P_SNOM_COMPLETO = new OracleParameter("P_SNOM_COMPLETO", OracleDbType.Varchar2, param.SNOM_COMPLETO, ParameterDirection.Input);
                OracleParameter P_SSENAL_LAFT = new OracleParameter("P_SSENAL_LAFT", OracleDbType.Varchar2, param.SSENAL_LAFT, ParameterDirection.Input);
                OracleParameter P_VALIDADOR = new OracleParameter("P_VALIDADOR", OracleDbType.Varchar2, param.VALIDADOR, ParameterDirection.Input);
                OracleParameter RC1 = new OracleParameter("RC1", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);
                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;
                OracleParameter[] parameters = new OracleParameter[] { P_SNUMIDENTIDAD, P_SNOM_COMPLETO, P_SSENAL_LAFT, P_VALIDADOR, RC1, P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_LISTA_REGISTO_NEGATIVO(:P_SNUMIDENTIDAD , :P_SNOM_COMPLETO, :P_SSENAL_LAFT, :P_VALIDADOR, :RC1 ,:P_NCODE ,:P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                OracleDataReader odr = ((OracleRefCursor)RC1.Value).GetDataReader();

                output["nCode"] = Int32.Parse(P_NCODE.Value.ToString());
                output["sMessage"] = P_SMESSAGE.Value.ToString();
                while (odr.Read())
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    item["SNID"] = odr["SNID"];
                    item["STIPOPERSONA"] = odr["STIPOPERSONA"];
                    item["STIPODOC_PAIS"] = odr["STIPODOC_PAIS"];
                    item["SNUMIDENTIDAD"] = odr["SNUMIDENTIDAD"];
                    item["SAPE_PATERNO"] = odr["SAPE_PATERNO"];
                    item["SAPE_MATERNO"] = odr["SAPE_MATERNO"];
                    item["SNOMBRES_RS"] = odr["SNOMBRES_RS"];
                    item["SSENAL_LAFT"] = odr["SSENAL_LAFT"];
                    item["SFILTRO1"] = odr["SFILTRO1"];
                    item["SFEDESCUBRIMIENTO"] = odr["SFEDESCUBRIMIENTO"];
                    item["SDOCREFERENCIA"] = odr["SDOCREFERENCIA"];
                    item["STIPOLISTA"] = odr["STIPOLISTA"];
                    item["SNUMDOCUMENTO"] = odr["SNUMDOCUMENTO"];
                    item["SNOM_COMPLETO"] = odr["SNOM_COMPLETO"];
                    lista.Add(item);
                }
                odr.Close();
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();

                Console.WriteLine("Error :" + ex);
                Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                item["menesajeError"] = ex.Message.ToString();
                item["mensajeErrorDEtalle"] = ex;
                lista.Add(item);
            }
            return lista;
        }



        public Dictionary<string, dynamic> getDeleteRegistrosNegativos()
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {

                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_DELETE_REGISTO_NEGATIVO(:P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                output["nCode"] = Convert.ToInt64((P_NCODE.Value).ToString());
                if (output["nCode"] != 0)
                {
                    output["sMessage"] = P_SMESSAGE.Value;
                }
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el mensaje del error : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return output;
        }
        internal int getPeriodoSemestralMax()
        {
            int periodo = 0;
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, ParameterDirection.Output);

                P_NPERIODO_PROCESO.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO };

                var query = @"
               BEGIN
                   LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_PERIODO_SEMESTRAL_MAX(:P_NPERIODO_PROCESO);
               END;
               ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                periodo = Int32.Parse(P_NPERIODO_PROCESO.Value.ToString());
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el error : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return periodo;
        }
        public Es10 GetRegistrarDatosExcelEs10(Es10 item)
        {
            try
            {
                OracleConnection connection = new OracleConnection(Utils.Config.AppSetting["ConnectionString:LAFT"]);
                connection.Open();
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, item.items.periodoProceso.ToArray(), ParameterDirection.Input);
                OracleParameter P_SRAMO = new OracleParameter("P_SRAMO", OracleDbType.Varchar2, item.items.ramo.ToArray(), ParameterDirection.Input);
                OracleParameter P_SRIESGO = new OracleParameter("P_SRIESGO", OracleDbType.Varchar2, item.items.riesgo.ToArray(), ParameterDirection.Input);
                OracleParameter P_NCOD_RIESGO = new OracleParameter("P_NCOD_RIESGO", OracleDbType.Int64, item.items.codRiesgo.ToArray(), ParameterDirection.Input);
                OracleParameter P_SCOD_REGISTRO = new OracleParameter("P_SCOD_REGISTRO", OracleDbType.Varchar2, item.items.codRegistro.ToArray(), ParameterDirection.Input);
                OracleParameter P_SNOM_COMERCIAL = new OracleParameter("P_SNOM_COMERCIAL", OracleDbType.Varchar2, item.items.nomComercial.ToArray(), ParameterDirection.Input);
                OracleParameter P_SMONEDA = new OracleParameter("P_SMONEDA", OracleDbType.Varchar2, item.items.moneda.ToArray(), ParameterDirection.Input);
                OracleParameter P_DFEC_INI_COMERCIAL = new OracleParameter("P_DFEC_INI_COMERCIAL", OracleDbType.Date, item.items.fecini.ToArray(), ParameterDirection.Input);
                OracleParameter P_NCANT_ASEGURADOS = new OracleParameter("P_NCANT_ASEGURADOS", OracleDbType.Int64, item.items.numAsegurados.ToArray(), ParameterDirection.Input);
                OracleParameter P_SREGIMEN = new OracleParameter("P_SREGIMEN", OracleDbType.Varchar2, item.items.sRegimen.ToArray(), ParameterDirection.Input);

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_SRAMO, P_SRIESGO, P_NCOD_RIESGO, P_SCOD_REGISTRO, P_SNOM_COMERCIAL, P_SMONEDA, P_DFEC_INI_COMERCIAL, P_NCANT_ASEGURADOS, P_SREGIMEN };


                // create command and set properties  
                OracleCommand cmd = connection.CreateCommand();
                //DateTime sysdate = DateTime.Now;
                cmd.CommandText = "INSERT INTO TBL_LAFT_PLANTILLA_ES10 ( NPERIODO_PROCESO, SRAMO, SRIESGO, NCOD_RIESGO, SCOD_REGISTRO, SNOM_COMERCIAL, SMONEDA, DFEC_INI_COMERCIAL, NCANT_ASEGURADOS, SREGIMEN) " +
                                  "VALUES (:1, :2, :3, :4, :5, :6, :7, :8, :9, :10)";
                cmd.ArrayBindCount = item.items.ramo.Count;
                cmd.Parameters.AddRange(parameters);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                item.mensaje = ex.Message.ToString();
                item.codigo = 2;
            }
            return item;
        }
        public Dictionary<string,string> insertMasiveDataDemanda(BusquedaDemanda item)
        {
            Dictionary<string, string> response = new Dictionary<string, string>();
            try
            {
                OracleConnection connection = new OracleConnection(Utils.Config.AppSetting["ConnectionString:LAFT"]);
                connection.Open();
                OracleParameter P_SCODBUSQUEDA = new OracleParameter("P_SCODBUSQUEDA", OracleDbType.Varchar2, item.codBusqueda.ToArray(), ParameterDirection.Input);
                OracleParameter P_SNOMBREUSUARIO = new OracleParameter("P_SNOMBREUSUARIO", OracleDbType.Varchar2, item.nombreUsuario.ToArray(), ParameterDirection.Input);
                OracleParameter P_SNOMBRE_COMPLETO = new OracleParameter("P_SNOMBRE_COMPLETO", OracleDbType.Varchar2, item.nombreCliente.ToArray(), ParameterDirection.Input);
                OracleParameter P_STIPO_DOCUMENTO = new OracleParameter("P_STIPO_DOCUMENTO", OracleDbType.Varchar2, item.tipoDocumento.ToArray(), ParameterDirection.Input);
                OracleParameter P_SNUM_DOCUMENTO = new OracleParameter("P_SNUM_DOCUMENTO", OracleDbType.Varchar2, item.numeroRuc.ToArray(), ParameterDirection.Input);
               
                OracleParameter[] parameters = new OracleParameter[] { P_SCODBUSQUEDA, P_SNOMBREUSUARIO, P_SNOMBRE_COMPLETO, P_STIPO_DOCUMENTO, P_SNUM_DOCUMENTO };


                // create command and set properties  
                OracleCommand cmd = connection.CreateCommand();
                //DateTime sysdate = DateTime.Now;
                cmd.CommandText = "INSERT INTO TBL_LAFT_ALERTA_CARGA_BUSQUEDA_DEMANDA ( SCODBUSQUEDA, SNOMBREUSUARIO, SNOMBRE_COMPLETO, STIPO_DOCUMENTO, SNUM_DOCUMENTO) " +
                                  "VALUES (:1, :2, :3, :4, :5)";
                cmd.ArrayBindCount = item.codBusqueda.Count;
                cmd.Parameters.AddRange(parameters);
                cmd.ExecuteNonQuery();
                response["mensaje"] = "Se ejecuto correctamente";
                response["codigo"] = "1";
            }
            catch (Exception ex)
            {
                response["mensaje"] = ex.Message.ToString();
                response["codigo"] = "2";
            }
            return response;
        }
        public Dictionary<string, dynamic> getDeleteEs10(dynamic param)
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {

                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_GET_DELETE_ES10(:P_NPERIODO_PROCESO ,:P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                output["nCode"] = Convert.ToInt64((P_NCODE.Value).ToString());
                if (output["nCode"] != 0)
                {
                    output["sMessage"] = P_SMESSAGE.Value;
                }
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el mensaje del error : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return output;
        }
        public Dictionary<string, dynamic> getDeleteActivity(dynamic param)
        {
            Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
            try
            {

                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, ParameterDirection.Input);
                OracleParameter P_NCODE = new OracleParameter("P_NCODE", OracleDbType.Int32, ParameterDirection.Output);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, ParameterDirection.Output);

                P_NCODE.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_NCODE, P_SMESSAGE };

                var query = @"
                    BEGIN
                        LAFT.PKG_LAFT_GESTION_ALERTAS.SP_DEL_KRI_ACTIVIDADECONOMICA(:P_NPERIODO_PROCESO ,:P_NCODE, :P_SMESSAGE);
                    END;
                    ";

                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);

                output["nCode"] = Convert.ToInt64((P_NCODE.Value).ToString());
                if (output["nCode"] != 0)
                {
                    output["sMessage"] = P_SMESSAGE.Value;
                }
                this.context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                this.context.Database.CloseConnection();
                Console.WriteLine("el mensaje del error : " + ex);
                throw ex;
                //Utils.ExceptionManager.resolve(ex);
                //return null;
            }
            return output;
        }
        public ActividadEconomica GetRegistrarDatosExcelActividadEconomico(ActividadEconomica item)
        {
            try
            {
                OracleConnection connection = new OracleConnection(Utils.Config.AppSetting["ConnectionString:LAFT"]);
                connection.Open();
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, item.items.nPeriodoProceso.ToArray(), ParameterDirection.Input);
                OracleParameter P_SDESCRIPTION = new OracleParameter("P_SDESCRIPTION", OracleDbType.Varchar2, item.items.sDescription.ToArray(), ParameterDirection.Input);
                OracleParameter P_SNUMRUC = new OracleParameter("P_SNUMRUC", OracleDbType.Varchar2, item.items.sNumRuc.ToArray(), ParameterDirection.Input);
                OracleParameter P_STIPOCONTRIBUYENTE = new OracleParameter("P_STIPOCONTRIBUYENTE", OracleDbType.Varchar2, item.items.sTipoContribuyente.ToArray(), ParameterDirection.Input);
                OracleParameter P_SACTIVITYECONOMY = new OracleParameter("P_SACTIVITYECONOMY", OracleDbType.Varchar2, item.items.sActivityEconomy.ToArray(), ParameterDirection.Input);
                OracleParameter P_SSECTOR = new OracleParameter("P_SSECTOR", OracleDbType.Varchar2, item.items.sSector.ToArray(), ParameterDirection.Input);

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_SDESCRIPTION, P_SNUMRUC, P_STIPOCONTRIBUYENTE, P_SACTIVITYECONOMY, P_SSECTOR };


                // create command and set properties  
                OracleCommand cmd = connection.CreateCommand();
                //DateTime sysdate = DateTime.Now;
                cmd.CommandText = "INSERT INTO TBL_LAFT_ACTIVIDAD_ECONOMICA ( NPERIODO_PROCESO, SDESCRIPTION, SNUM_RUC, STIPOCONTRIBUYENTE, SACTIVITYECONOMY, SSECTOR) " +
                                  "VALUES (:1, :2, :3, :4, :5, :6)";
                cmd.ArrayBindCount = item.items.nPeriodoProceso.Count;
                cmd.Parameters.AddRange(parameters);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                item.mensaje = ex.Message.ToString();
                item.codigo = 2;
            }
            return item;
        }
        public ZonaGeografica GetRegistrarDatosExcelZonaGeografica(ZonaGeografica item)
        {
            try
            {
                OracleConnection connection = new OracleConnection(Utils.Config.AppSetting["ConnectionString:LAFT"]);
                connection.Open();
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int64, item.items.periodoProceso.ToArray(), ParameterDirection.Input);
                OracleParameter P_COD_TIPPROD = new OracleParameter("P_COD_TIPPROD", OracleDbType.Varchar2, item.items.producto.ToArray(), ParameterDirection.Input);
                OracleParameter P_STIPODOCUMENTO = new OracleParameter("P_STIPODOCUMENTO", OracleDbType.Varchar2, item.items.tipDoc.ToArray(), ParameterDirection.Input);
                OracleParameter P_SNUMDOCUMENTO = new OracleParameter("P_SNUMDOCUMENTO", OracleDbType.Varchar2, item.items.numDoc.ToArray(), ParameterDirection.Input);
                OracleParameter P_SPRIMERNOMBRE = new OracleParameter("P_SPRIMERNOMBRE", OracleDbType.Varchar2, item.items.primerNombre.ToArray(), ParameterDirection.Input);
                OracleParameter P_SSEGUNDONOMBRE = new OracleParameter("P_SSEGUNDONOMBRE", OracleDbType.Varchar2, item.items.segundoNombre.ToArray(), ParameterDirection.Input);
                OracleParameter P_SAPELLIDOPATERNO = new OracleParameter("P_SAPELLIDOPATERNO", OracleDbType.Varchar2, item.items.apellidoParterno.ToArray(), ParameterDirection.Input);
                OracleParameter P_SAPELLIDOMATERNO = new OracleParameter("P_SAPELLIDOMATERNO", OracleDbType.Varchar2, item.items.apellidoMaterno.ToArray(), ParameterDirection.Input);
                OracleParameter P_SREGION = new OracleParameter("P_SREGION", OracleDbType.Varchar2, item.items.region.ToArray(), ParameterDirection.Input);

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_COD_TIPPROD, P_STIPODOCUMENTO, P_SNUMDOCUMENTO, P_SPRIMERNOMBRE, P_SSEGUNDONOMBRE, P_SAPELLIDOPATERNO, P_SAPELLIDOMATERNO, P_SREGION };


                // create command and set properties  
                OracleCommand cmd = connection.CreateCommand();
                //DateTime sysdate = DateTime.Now;
                cmd.CommandText = "INSERT INTO LAFT.TBL_LAFT_KRI_ZONA_GEOGRAFICA ( NPERIODO_PROCESO,COD_TIPPROD, TIP_DOC, NUM_IDENBEN, NOMBEN,  NOMSEGBEN, PATBEN, MATBEN, GLS_REGION) " +
                                  "VALUES (:1, :2, :3, :4, :5 , :6, :7, :8, :9)";
                cmd.ArrayBindCount = item.items.periodoProceso.Count;
                cmd.Parameters.AddRange(parameters);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                item.mensaje = ex.Message.ToString();
                item.codigo = 2;
            }
            return item;
        }

        public informeN1 getInformeN1(dynamic param)
        {
            informeN1 response = new informeN1();
            try
            {
                OracleParameter P_NPERIODO_PROCESO = new OracleParameter("P_NPERIODO_PROCESO", OracleDbType.Int32, param.NPERIODO_PROCESO, System.Data.ParameterDirection.Input);
                OracleParameter P_SMESSAGE = new OracleParameter("P_SMESSAGE", OracleDbType.Varchar2, System.Data.ParameterDirection.Output);
                OracleParameter P_NERROR = new OracleParameter("P_NERROR", OracleDbType.Int32, System.Data.ParameterDirection.Output);
                OracleParameter ZONAS_GEOGRAFICAS = new OracleParameter("ZONAS_GEOGRAFICAS", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter PRODUCTOS = new OracleParameter("PRODUCTOS", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter CLIENTES_TYPE_REGIMEN = new OracleParameter("CLIENTES_TYPE_REGIMEN", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                OracleParameter CLIENTES_CHARACTER_CLIENT = new OracleParameter("CLIENTES_CHARACTER_CLIENT", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                
                P_NERROR.Size = 4000;
                P_SMESSAGE.Size = 4000;

                OracleParameter[] parameters = new OracleParameter[] { P_NPERIODO_PROCESO, P_SMESSAGE, P_NERROR, ZONAS_GEOGRAFICAS, PRODUCTOS, CLIENTES_TYPE_REGIMEN, CLIENTES_CHARACTER_CLIENT };
                
                var query = @"
                BEGIN
                    INSUDB.PKG_LAFT_REPORTE_N1.SP_GET_ZGEOGRAFIC_PRODUCT_CLIENTES(:P_NPERIODO_PROCESO, :P_SMESSAGE, :P_NERROR, :ZONAS_GEOGRAFICAS, :PRODUCTOS, :CLIENTES_TYPE_REGIMEN, :CLIENTES_CHARACTER_CLIENT );
                END;
                ";
                this.context.Database.OpenConnection();
                this.context.Database.ExecuteSqlCommand(query, parameters);
                response.code = Convert.ToInt16((P_NERROR.Value).ToString());
                response.mesagge = P_SMESSAGE.Value.ToString();
                if (response.code == 1)
                {
                    return response;
                }
                OracleDataReader odr = ((OracleRefCursor)ZONAS_GEOGRAFICAS.Value).GetDataReader();
                response.zonaGeograficas = new List<ZonaGeograficaN1Entity>();
                while (odr.Read())
                {
                    ZonaGeograficaN1Entity item = new ZonaGeograficaN1Entity();
                    item.zonaGeografica = odr["ZONA_GEOGRAFICA"].ToString();
                    item.numeroPolizas = int.Parse(odr["NUMERO_POLIZAS"].ToString());
                    item.numeroContratantes = int.Parse(odr["NUMERO_CONTRATANTES"].ToString());
                    item.numeroAsegurados = int.Parse(odr["NUMERO_ASEGURADOS"].ToString());
                    item.numeroBeneficiarios = int.Parse(odr["NUMERO_BENEFICIARIOS"].ToString());
                    item.clienteReforzado = int.Parse(odr["CLIENTE_REFORZADO"].ToString());
                    item.montoPrima = double.Parse(odr["MONTO_PRIMA"].ToString());
                    response.zonaGeograficas.Add(item);
                }
                odr.Close();
                OracleDataReader odr2 = ((OracleRefCursor)PRODUCTOS.Value).GetDataReader();
                response.productos = new List<ProductoN1Entity>();
                while (odr2.Read())
                {
                    ProductoN1Entity item = new ProductoN1Entity();
                    item.producto = odr2["PRODUCTO"].ToString();
                    item.numeroPolizas = int.Parse(odr2["NUMERO_POLIZAS"].ToString());
                    item.numeroContratantes = int.Parse(odr2["NUMERO_CONTRATANTES"].ToString());
                    item.numeroAsegurados = int.Parse(odr2["NUMERO_ASEGURADOS"].ToString());
                    item.numeroBeneficiarios = int.Parse(odr2["NUMERO_BENEFICIARIOS"].ToString());
                    item.clienteReforzado= int.Parse(odr2["CLIENTE_REFORZADO"].ToString());
                    item.montoPrima = double.Parse(odr2["MONTO_PRIMA"].ToString());
                    response.productos.Add(item);
                }
                odr2.Close();
                OracleDataReader odr3 = ((OracleRefCursor)CLIENTES_TYPE_REGIMEN.Value).GetDataReader();
                response.clientesType = new List<ClientesTypeRegimenN1Entity>();
                while (odr3.Read())
                {
                    ClientesTypeRegimenN1Entity item = new ClientesTypeRegimenN1Entity();
                    item.tipoRegimen = odr3["TIPO_REGIMEN"].ToString();
                    item.numeroClientes = int.Parse(odr3["NUMERO_CLIENTES"].ToString());
                    response.clientesType.Add(item);
                }
                odr3.Close();
                OracleDataReader odr4 = ((OracleRefCursor)CLIENTES_CHARACTER_CLIENT.Value).GetDataReader();
                response.clientesCharacter = new List<ClientesCharacterClientN1Entity>();
                while (odr4.Read())
                {
                    ClientesCharacterClientN1Entity item = new ClientesCharacterClientN1Entity();
                    item.tipoClientes = odr4["TIPO_CLIENTES"].ToString();
                    item.numeroClientes = int.Parse(odr4["NUMERO_CLIENTES"].ToString());
                    item.numeroClienteReforzado = int.Parse(odr4["NUMERO_CLIENTE_REFORZ"].ToString());
                    item.montoPrima = double.Parse(odr4["MONTO_PRIMA"].ToString());
                    response.clientesCharacter.Add(item);
                }
                odr4.Close();
                this.context.Database.CloseConnection();
                return response;
            }
            catch (Exception ex)
            {
                response.mesagge = ex.Message;
                response.code = 1;
                return response;
            }
        }


    }

}








