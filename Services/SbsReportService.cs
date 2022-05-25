using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using protecta.laft.api.DTO;
using protecta.laft.api.Models;
using protecta.laft.api.Repository;
using System.Data;
using SpreadsheetLight;
using System.IO;
using System.IO.Compression;
using System.Net;
using Newtonsoft.Json;
using HtmlAgilityPack;
//using SautinSoft.Document;
//using objWord = Microsoft.Office.Interop.Word;


namespace protecta.laft.api.Services
{
    public class SbsReportService
    {
        SbsReportRepository sbsReportRepository;

        public SbsReportService()
        {
            this.sbsReportRepository = new SbsReportRepository();
        }

        public List<CommentListResponseDTO> GetCommentList(int alertId, int periodId)
        {
            try
            {
                return this.sbsReportRepository.GetCommentList(alertId, periodId);

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<CommentListResponseDTO>();
            }
        }


        public UpdateCommentListResponseDTO UpdateCommentList(int alertId, int periodId, string comment, int userId)

        {
            try
            {
                return this.sbsReportRepository.UpdateCommentList(alertId, periodId, comment, userId);

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new UpdateCommentListResponseDTO();
            }

        }

        public List<QuestionsByAlertResponseDTO> GetQuestionsByAlert(QuestionsByAlertParametersDTO param)
        {
            try
            {
                return this.sbsReportRepository.GetQuestionsByAlert(param);

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<QuestionsByAlertResponseDTO>();
            }
        }

        public List<AlertListResponseDTO> GetAlertList()
        {
            try
            {
                return this.sbsReportRepository.GetAlertList();

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<AlertListResponseDTO>();
            }
        }

        public UpdateAlertResponseDTO UpdateAlertStatus(int alertId, string alertName, string alertDescription, string alertStatus, int userId, int bussinessDays, string reminderSender, string operType, int idgrupo, string regimenSim, string regimenGen)

        {
            try
            {
                Console.Write("entro en el service");
                return this.sbsReportRepository.UpdateAlertStatus(alertId, alertName, alertDescription, alertStatus, userId, bussinessDays, reminderSender, operType, idgrupo, regimenSim, regimenGen);

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new UpdateAlertResponseDTO();
            }

        }

        public UpdateQuestionResponseDTO UpdateQuestion(int alertId, int questionId, int originId, string questionName, string questionStatus, int userId, string transactionType, int validComment)
        {
            try
            {
                return this.sbsReportRepository.UpdateQuestion(alertId, questionId, originId, questionName, questionStatus, userId, transactionType, validComment);

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new UpdateQuestionResponseDTO();
            }

        }

        public SuspendStatusResponseDTO SuspendFrequencyStatus(int frequencyId, string suspensionId)

        {
            try
            {
                return this.sbsReportRepository.SuspendFrequencyStatus(frequencyId, suspensionId);

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new SuspendStatusResponseDTO();
            }

        }

        //public experianServiceResponseDTO ExperianInvoker (dynamic param) {
        public Dictionary<string, dynamic> ExperianInvoker(dynamic param)
        {
            try
            {
                return this.sbsReportRepository.ExperianInvoker(param);

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new Dictionary<string, dynamic>(); //new experianServiceResponseDTO ();
            }
        }

        public ExchangeRateResponseDTO GetExchangeRate()
        {
            return this.sbsReportRepository.GetExchangeRate();
        }

        public AmountResponseDTO GetAmount()
        {
            return this.sbsReportRepository.GetAmount();
        }

        public SbsReportGenResponseDTO GenerateSbsReport(int operType, decimal exchangeType, int ammount, string startDate, string endDate, string nameReport, string sbsFileType)
        {
            return this.sbsReportRepository.GenerateSbsReport(operType, exchangeType, ammount, startDate, endDate, nameReport, sbsFileType);
        }

        public UserDataResponseDTO GetUser(int userId)
        {
            return this.sbsReportRepository.GetUser(userId);
        }

        public List<Dictionary<string, dynamic>> GetListReports(SbsReportGenListParametersDTO param)
        {
            try
            {
                return this.sbsReportRepository.GetListReports(param);

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<Dictionary<string, dynamic>>();
            }
        }

        public List<AlertMonitoringResponseDTO> GetListAlerts(AlertMonitoringParametersDTO param)
        {
            try
            {
                return this.sbsReportRepository.GetListAlerts(param);

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<AlertMonitoringResponseDTO>();
            }
        }

        public List<SbsReportFileResponseDTO> GetReport(string id, int tipo_archivo)
        {
            return this.sbsReportRepository.GetReport(id, tipo_archivo);
        }

        public List<ReportTypeResponseDTO> GetReportTypes()
        {
            try
            {
                return this.sbsReportRepository.GetReportTypes();

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<ReportTypeResponseDTO>();
            }
        }

        public List<GafiListResponseDTO> GetGafiList()
        {
            try
            {
                return this.sbsReportRepository.GetGafiList();

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<GafiListResponseDTO>();
            }
        }

        //inputParams.gafiId, inputParams.countryGafiName, inputParams.state, inputParams.regUser, inputParams.operType                   
        public UpdateCountryResponseDTO UpdateCountry(int gafiId, string countryGafiName, string state, int regUser, string operType)
        {
            return this.sbsReportRepository.UpdateCountry(gafiId, countryGafiName, state, regUser, operType);
        }

        public List<FrequencyResponseDTO> GetFrequency()
        {
            try
            {
                return this.sbsReportRepository.GetFrequency();

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<FrequencyResponseDTO>();
            }
        }

        public List<FrequencyListResponseDTO> GetFrequencyList()
        {
            try
            {
                return this.sbsReportRepository.GetFrequencyList();

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<FrequencyListResponseDTO>();
            }
        }

        public List<FrequencyActiveResponseDTO> GetFrequencyActive()
        {
            try
            {
                return this.sbsReportRepository.GetFrequencyActive();

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<FrequencyActiveResponseDTO>();
            }
        }

        public UpdateFrequencyResponseDTO UpdateFrequency(int frequencyId, int frequencyType, string startDate, int userId)
        {
            return this.sbsReportRepository.UpdateFrequency(frequencyId, frequencyType, startDate, userId);
        }

        public List<ProfileListResponseDTO> GetProfileList()
        {
            try
            {
                return this.sbsReportRepository.GetProfileList();

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<ProfileListResponseDTO>();
            }
        }

        public List<ProfileListResponseDTO> GetUserByProfileList()
        {
            try
            {
                return this.sbsReportRepository.GetUserByProfileList();

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<ProfileListResponseDTO>();
            }
        }

        public List<RegimeResponseDTO> GetRegimeList(RegimeParametersDTO param)
        {
            try
            {
                return this.sbsReportRepository.GetRegimeList(param);

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<RegimeResponseDTO>();
            }
        }

        public List<GrupoPerfilListResponseDTO> GetGrupoxPerfilList(RegimeParametersDTO param)
        {
            try
            {
                return this.sbsReportRepository.GetGrupoxPerfilList(param);

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<GrupoPerfilListResponseDTO>();
            }
        }

        public List<Dictionary<string, dynamic>> GetAlertByProfileList(dynamic param)
        {
            return this.sbsReportRepository.GetAlertByProfileList(param);
            /*try {
                return this.sbsReportRepository.GetAlertByProfileList (param);

            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return new List<Dictionary<string,dynamic>> ();
            }*/
        }

        public UpdateAlertByProfileResponseDTO UpdateAlertByProfile(int profileId, int regimeId, int alertId, string alertStatus)
        {
            return this.sbsReportRepository.UpdateAlertByProfile(profileId, regimeId, alertId, alertStatus);
        }

        public List<UsersByProfileResponseDTO> GetUsersByProfile(UsersByProfileParametersDTO param)
        {
            try
            {
                return this.sbsReportRepository.GetUsersByProfile(param);

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<UsersByProfileResponseDTO>();
            }
        }

        public Dictionary<string, dynamic> processCargaFile(dynamic param)
        {
            try
            {
                string ruta = @"C:/archivos/comprimido";

                int i = 0;
                foreach (var itemFile in param.arrFiles)
                {
                    Console.WriteLine("el itemFile: " + itemFile);
                    Console.WriteLine("el itemFile.file: " + itemFile.file);
                    Console.WriteLine("el itemFile.name: " + itemFile.name);
                    string fileLocal = itemFile.file;
                    string nameLocal = "/" + itemFile.name;
                    int indice = fileLocal.IndexOf(",") + 1;
                    Console.WriteLine("el indice: " + indice);
                    byte[] data = System.Convert.FromBase64String(fileLocal.Substring(indice));

                    string rutaFinal = Environment.ExpandEnvironmentVariables(ruta + nameLocal);
                    Console.WriteLine("el rutaFinal: " + rutaFinal);
                    File.WriteAllBytes(rutaFinal, data);
                    i++;
                }
                foreach (var itemFile in param.arrFiles)
                {
                    string nameFile = itemFile.name;
                    DesZip(nameFile, @"C:/archivos/comprimido");
                }

                foreach (var itemFile in param.arrFiles)
                {
                    string nameFile = itemFile.name;
                    string nameSinExtension = Path.GetFileNameWithoutExtension(nameFile);
                    string rutaNew = "C:/archivos/descomprimido/" + nameSinExtension + ".txt";
                    rutaNew = Environment.ExpandEnvironmentVariables(rutaNew);
                    string data = File.ReadAllText(rutaNew);
                    Console.WriteLine("el data: " + data);
                    var arrSplite = data.Split("|");
                    Console.WriteLine("el arrSplite: " + arrSplite);
                    int incText = 0;
                    int incValidRow = 0;
                    List<SbsCargaSUNATDTO> arrCargaSUNATSbs = new List<SbsCargaSUNATDTO>();
                    SbsCargaSUNATDTO objCargaSUNATSbs = new SbsCargaSUNATDTO();
                    foreach (string item in arrSplite)
                    {
                        if (incText > 23)
                        {
                            if (incValidRow < 24)
                            {

                                Console.WriteLine("el num: " + incValidRow + " - item :" + item);
                                if (incValidRow == 0) objCargaSUNATSbs.NUM_RUC = item;
                                if (incValidRow == 1) objCargaSUNATSbs.NOM_RAZON = item;
                                if (incValidRow == 2) objCargaSUNATSbs.TIPO_CONTRIBU = item;
                                if (incValidRow == 3) objCargaSUNATSbs.PROFESION_OFI = item;
                                if (incValidRow == 4) objCargaSUNATSbs.NOM_COMERCIAL = item;
                                if (incValidRow == 5) objCargaSUNATSbs.CONDICION_CONTRIBUYENTE = item;
                                if (incValidRow == 6) objCargaSUNATSbs.ESTADO_CONTRIBUYENTE = item;
                                if (incValidRow == 7) objCargaSUNATSbs.FECHA_INSCRIP = item;
                                if (incValidRow == 8) objCargaSUNATSbs.FECHA_INICIO_ACTIVIDAD = item;
                                if (incValidRow == 9) objCargaSUNATSbs.DEPARTAMENTO = item;
                                if (incValidRow == 10) objCargaSUNATSbs.PROVINCIA = item;
                                if (incValidRow == 11) objCargaSUNATSbs.DISTRITO = item;
                                if (incValidRow == 12) objCargaSUNATSbs.DIRECCION = item;
                                if (incValidRow == 13) objCargaSUNATSbs.TELEFONO = item;
                                if (incValidRow == 14) objCargaSUNATSbs.FAX = item;
                                if (incValidRow == 15) objCargaSUNATSbs.ACTIVID_COMERCIO_EXTERIOR = item;
                                if (incValidRow == 16) objCargaSUNATSbs.PRINCIPAL_CIIU = item;
                                if (incValidRow == 17) objCargaSUNATSbs.SECUNDARIO_1_CIIU = item;
                                if (incValidRow == 18) objCargaSUNATSbs.SECUNDARIO_2_CIIU = item;
                                if (incValidRow == 19) objCargaSUNATSbs.AFECTO_NUEVO_RUS = item;
                                if (incValidRow == 20) objCargaSUNATSbs.BUEN_CONTRIBUYENTE = item;
                                if (incValidRow == 21) objCargaSUNATSbs.AGENTE_RETENCION = item;
                                if (incValidRow == 22) objCargaSUNATSbs.AGENTE_PERCEPCION_VTAINT = item;
                                if (incValidRow == 23)
                                {
                                    objCargaSUNATSbs.AGENTE_PERCEPCION_COMLIQ = item;
                                    arrCargaSUNATSbs.Add(objCargaSUNATSbs);
                                }

                                incValidRow++;
                                if (incValidRow == 24)
                                {
                                    objCargaSUNATSbs = new SbsCargaSUNATDTO();
                                    incValidRow = 0;
                                }
                            }
                        }
                        incText++;
                    }

                    Console.WriteLine("el arrCargaSUNATSbs Count: " + arrCargaSUNATSbs.Count);
                    foreach (SbsCargaSUNATDTO obj in arrCargaSUNATSbs)
                    {
                        obj.NID_CODIGO = nameSinExtension;
                        this.sbsReportRepository.InsertDataSUNAT(obj);
                    }
                }








                Dictionary<string, dynamic> objResponse = new Dictionary<string, dynamic>();
                objResponse["code"] = 0;
                objResponse["message"] = "Exito";
                return objResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine("el error : " + ex);
                dynamic objError = new Dictionary<string, dynamic>();
                objError["code"] = 1;
                objError["messageError"] = ex.ToString();
                objError["messageErrorDetalle"] = ex;
                //Utils.ExceptionManager.resolve (ex);
                return objError;
            }
        }

        public Dictionary<string, dynamic> processInsertFile(dynamic param)
        {
            string FECINI = "";
            string FECFIN = "";
            try
            {
            this.getRangeDateForMonth(param.ValueMonth.ToString(), out FECINI, out FECFIN);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            param.FECINI = FECINI;
            param.FECFIN = FECFIN;
            try
            {
                dynamic resp = new Dictionary<string, dynamic>();
                string cadenaTxtUnica = "";
                string cadenaTxtMultiple = "";
                var respUpdateMonitoring = this.sbsReportRepository.UpdateEstadoMonitoreoSbs(param.nameFile.ToString(), param.FECINI.ToString(), param.FECFIN.ToString(), Convert.ToDecimal(param.TC), (param.OPE + "").ToString(), "SINIESTROS - RENTAS - VIDA", "PENDIENTE");
                Console.WriteLine("la respuesta : " + respUpdateMonitoring["code"]);

                DataTable dtData = new DataTable();
                List<SbsReportEnity> arrLista = new List<SbsReportEnity>();
                param["busqueda"] = "VIDA";
                var listaVida =  this.sbsReportRepository.GetCursorSiniestros(param,0, arrLista);//new List<Dictionary<string,dynamic>>();*/
                param["busqueda"] = "SIN";
                var listaSiniestros = this.sbsReportRepository.GetCursorSiniestros(param, 0, listaVida["data"]);//listaVida["dataDT"]
                param["busqueda"] = "RT";
                var listaRentas = this.sbsReportRepository.GetCursorSiniestros(param, 2, listaSiniestros["data"]);




                int incOperacionMULTI = 1;
                int incOperacionUNICA = 1;
                int contadorUnica = 1;
                int contadorMultiple = 1;
                string cadenaInternoDuplidUnica = "";
                string cadenaInternoDuplidMulti = "";
                DataTable arrDTreportUnicas = new DataTable();
                DataTable arrDTreportMulti = new DataTable();
                arrDTreportUnicas = this.sbsReportRepository.SetColumnsDataTable(arrDTreportUnicas);
                arrDTreportMulti = this.sbsReportRepository.SetColumnsDataTable(arrDTreportMulti);
                //DataTable dtRepo = (DataTable)(listaRentas["dataDT"]);//new DataTable();
                List<Dictionary<string, dynamic>> arrObjs = new List<Dictionary<string, dynamic>>();
                List<SbsReportEnity> arrOBjsSbs = new List<SbsReportEnity>();
                foreach (var item in listaRentas["data"])
                {

                    SbsReportEnity objSbsReport = new SbsReportEnity();
                    string modalidad = item.MODALIDAD.Trim();
                    string operacion = item.OPERACION.Trim();
                    string interno = item.INTERNO.Trim();
                    objSbsReport = item;
                    if (modalidad.Equals("U"))
                    {
                        if (!interno.Equals(cadenaInternoDuplidUnica.Trim()) && contadorUnica != 1)
                        {
                            //incOperacionMULTI = 1;
                            incOperacionUNICA++;
                        }
                        objSbsReport.FILA = contadorUnica.ToString("00000000");
                        objSbsReport.OPERACION = incOperacionUNICA.ToString("00000000");
                        arrDTreportUnicas = this.sbsReportRepository.SetRowsDataTable(arrDTreportUnicas, objSbsReport);
                        //incOperacionUNICA++;
                        cadenaInternoDuplidUnica = interno;
                        contadorUnica++;
                    }
                    if (modalidad.Equals("M"))
                    {
                        if (!interno.Equals(cadenaInternoDuplidMulti.Trim()) && contadorMultiple != 1)
                        {
                            //incOperacionMULTI = 1;
                            incOperacionMULTI++;
                        }
                        /*if(interno.Equals(cadenaInternoDuplid.Trim())){
                            //Console.WriteLine("entro a la validacion");
                            incOperacionMULTI++;
                        }*/
                        objSbsReport.FILA = contadorMultiple.ToString("00000000");
                        objSbsReport.OPERACION = incOperacionMULTI.ToString("00000000");
                        arrDTreportMulti = this.sbsReportRepository.SetRowsDataTable(arrDTreportMulti, objSbsReport);


                        cadenaInternoDuplidMulti = interno;
                        contadorMultiple++;
                    }

                    if (((item.ORD_TIPDOC + " ").Trim()).Equals("1"))
                    {
                        Dictionary<string, dynamic> objResponse = new Dictionary<string, dynamic>();
                        objResponse["NUM_DOC"] = item.ORD_NUMDOC;
                        arrObjs.Add(objResponse);
                    }
                    if (((item.BEN_TIP_DOC + " ").Trim()).Equals("1"))
                    {
                        Dictionary<string, dynamic> objResponse = new Dictionary<string, dynamic>();
                        objResponse["NUM_DOC"] = item.BEN_NUM_DOC;
                        arrObjs.Add(objResponse);
                    }
                    arrOBjsSbs.Add(objSbsReport);
                }

                cadenaTxtUnica = cadenaTxtUnica + "0501" + "01" + "REVIS" + DateTime.Now.ToString("yyyyMMdd") + "012" + "               " + "\n";
                cadenaTxtMultiple = cadenaTxtMultiple + "0501" + "01" + "REVIS" + DateTime.Now.ToString("yyyyMMdd") + "012" + "               " + "\n";

                /*Dictionary<string,dynamic> respVIDA = this.processInsertText(listaVida["data"],"","");
                cadenaTxtUnica = cadenaTxtUnica + respVIDA["cadenaTxtUnica"];
                cadenaTxtMultiple = cadenaTxtMultiple + respVIDA["cadenaTxtMultiple"];*/


                //Dictionary<string,dynamic> respSIN = this.processInsertText(arrDTreportUnicas,"","");
                /*Dictionary<string,dynamic> respSIN = this.processInsertText(listaSiniestros["data"],"","");
                cadenaTxtUnica = cadenaTxtUnica + respSIN["cadenaTxtUnica"];
                cadenaTxtMultiple = cadenaTxtMultiple + respSIN["cadenaTxtMultiple"];*/

                Dictionary<string, dynamic> respRT = this.processInsertText(arrOBjsSbs, "", "");//this.processInsertText(listaRentas["data"],"","");
                cadenaTxtUnica = cadenaTxtUnica + respRT["cadenaTxtUnica"];
                cadenaTxtMultiple = cadenaTxtMultiple + respRT["cadenaTxtMultiple"];

                string cadenaTextoFile = "";
                string nombreArchivoDocumentos = "documentosGenerados_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                //Console.WriteLine("entro a la funcion 1");
                //Console.WriteLine("el arrObjs: "+arrObjs.Count);
                List<string> arrRucsExist = new List<string>();

                foreach (var item in arrObjs)
                {
                    string dni = item["NUM_DOC"].Trim();
                    string rucConvert = getConvertDniToRuc(dni);
                    bool existsRuc = false;
                    foreach (var itemValid in arrRucsExist)
                    {
                        if (itemValid == rucConvert)
                        {
                            existsRuc = true;
                        }

                    }
                    if (!existsRuc)
                    {

                        bool validateRuc = getValidateRUC(rucConvert);
                        if (validateRuc)
                        {
                            arrRucsExist.Add(rucConvert);
                        }

                    }
                }
                int incBucleArrObjs = 0;
                foreach (var item in arrRucsExist)
                {
                    try
                    {
                        //Console.WriteLine("entro a la funcion 2");
                        string ruc = item;

                        Console.WriteLine("el ruc : " + ruc);
                        if (incBucleArrObjs == (arrRucsExist.Count) - 1)
                        {
                            //Console.WriteLine("entro al if");
                            cadenaTextoFile = cadenaTextoFile + ruc + "|";
                        }
                        else
                        {
                            //Console.WriteLine("entro al else");
                            cadenaTextoFile = cadenaTextoFile + ruc + "|" + "\r\n";
                        }
                        incBucleArrObjs++;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("el error en el foreach : " + ex);
                    }

                }
                //Console.WriteLine("el incBucleArrObjs : "+incBucleArrObjs);
                //Console.WriteLine("el arrRucsExist.Count : "+arrRucsExist.Count);






                string nombreArchivoXLSUnica = param.nameFile + "_01";
                string nombreArchivoXLSMulti = param.nameFile + "_02";
                string nombreArchivoUnica = param.nameFile + "_01";
                string nombreArchivoMultiple = param.nameFile + "_02";
                System.IO.File.WriteAllText(@"C:\archivos\" + nombreArchivoUnica + ".txt", cadenaTxtUnica);
                System.IO.File.WriteAllText(@"C:\archivos\" + nombreArchivoMultiple + ".txt", cadenaTxtMultiple);

                System.IO.File.WriteAllTextAsync(@"C:\archivos\" + nombreArchivoDocumentos + ".txt", cadenaTextoFile);
                //Console.WriteLine("EL RESULT : "+result)
                this.generateExcelByArray(arrDTreportUnicas, nombreArchivoXLSUnica);
                this.generateExcelByArray(arrDTreportMulti, nombreArchivoXLSMulti);

                CrearZip(nombreArchivoDocumentos + ".txt", @"C:\archivos\");

                string ruta = @"C:\archivos\" + nombreArchivoDocumentos + ".zip";
                string archivoSunat = GetConvertFile(ruta);

                string rutaTxtUNicas = @"C:\archivos\" + nombreArchivoUnica + ".txt";
                string archivoTxtUNicas = GetConvertFile(rutaTxtUNicas);

                string rutaTxtMultiples = @"C:\archivos\" + nombreArchivoMultiple + ".txt";
                string archivoTxtMultiples = GetConvertFile(rutaTxtMultiples);

                string rutaXLSUnicas = @"C:\archivos\" + nombreArchivoXLSUnica + ".xlsx";
                string archivoXLSUnicas = GetConvertFile(rutaXLSUnicas);

                string rutaXLSMultiples = @"C:\archivos\" + nombreArchivoXLSMulti + ".xlsx";
                string archivoXLSMultiples = GetConvertFile(rutaXLSMultiples);



                resp["code"] = 0;
                resp["message"] = "Se genero con exito";
                resp["nomArchivoTxtUnicas"] = nombreArchivoUnica + ".txt";
                resp["nomArchivoTxtMultiples"] = nombreArchivoMultiple + ".txt";
                resp["nomArchivoXLSUNicas"] = nombreArchivoXLSUnica + ".xlsx";
                resp["nomArchivoXLSMultiples"] = nombreArchivoXLSMulti + ".xlsx";
                resp["nomArchivoSunat"] = nombreArchivoDocumentos + ".zip";
                resp["archivoTxtUnicas"] = archivoTxtUNicas;
                resp["archivoTxtMultiples"] = archivoTxtMultiples;
                resp["archivoXLSUNicas"] = archivoXLSUnicas;
                resp["archivoXLSMultiples"] = archivoXLSMultiples;
                resp["archivoSunat"] = archivoSunat;
                this.sbsReportRepository.UpdateEstadoMonitoreoSbs(param.nameFile.ToString(), param.FECINI.ToString(), param.FECFIN.ToString(), Convert.ToDecimal(param.TC), param.OPE.ToString(), "SINIESTROS - RENTAS - VIDA", "PENDIENTE INFORMACIÓN");
                return resp;
                //return this.sbsReportRepository.GetUsersByProfile (param);

            }
            catch (Exception ex)
            {
                Console.WriteLine("el error : " + ex);
                dynamic objError = new Dictionary<string, dynamic>();
                objError["code"] = 1;
                objError["message"] = ex.ToString();
                this.sbsReportRepository.UpdateEstadoMonitoreoSbs(param.nameFile.ToString(), param.FECINI.ToString(), param.FECFIN.ToString(), Convert.ToDecimal(param.TC), param.OPE.ToString(), "SINIESTROS - RENTAS - VIDA", "ERROR");
                //Utils.ExceptionManager.resolve (ex);
                return objError;
            }
        }

        public string getConvertDniToRuc(string dni)
        {
            try
            {
                Console.WriteLine("entro a la funcion");
                //5432765432
                string ruc = "10" + dni;
                var arrNumbersRuc = ruc.Split("");
                //Console.WriteLine("el arrNumbersRuc: "+arrNumbersRuc[0]);
                var arrFactores = ("5432765432").Split("");
                //Console.WriteLine("el arrFactores: "+arrFactores[0]);
                long sumaTotal = 0;
                long residuo = 0;
                long ultimoDigito = 0;

                for (int i = 0; i < arrNumbersRuc.Length; i++)
                {
                    sumaTotal += Convert.ToInt64(arrNumbersRuc[i]) * Convert.ToInt64(arrFactores[i]);
                }
                residuo = sumaTotal % 11;
                ultimoDigito = (residuo == 10) ? 0 : ((residuo == 11) ? 1 : (11 - residuo) % 10);
                return ruc + ultimoDigito;
            }
            catch (Exception ex)
            {
                Console.WriteLine("el error en el convert: " + ex);
                return "";
            }
        }

        public bool getValidateRUC(string ruc)
        {
            try
            {
                string URI = "http://172.23.2.104:90/wsuper/Consulta/Sunat?request=" + ruc;
                //string URI = "http://www.myurl.com/post.php";
                //string myParameters = "request="+ruc;

                using (WebClient wc = new WebClient())
                {
                    string data = "Time = 12:00am temperature = 50";
                    WebClient client = new WebClient();
                    // Optionally specify an encoding for uploading and downloading strings.
                    client.Encoding = System.Text.Encoding.UTF8;
                    // Upload the data.
                    string response = client.UploadString(URI, data);
                    var result = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(response);
                    // Display the server's response.
                    //Console.WriteLine(reply);
                    //Console.WriteLine("el result Exito: "+result["Exito"]);
                    return result["Exito"];
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("el error en el validate: " + ex);
                return false;
            }
        }
        public bool getValidateRUC2(string ruc)
        {
            try
            {
                string URI = "https://api.apis.net.pe/v1/ruc?numero=20471472994" + ruc;
                //string URI = "http://www.myurl.com/post.php";
                //string myParameters = "request="+ruc;

                using (WebClient wc = new WebClient())
                {
                    string data = "Bearer apis-token-1.aTSI1U7KEuT-6bbbCguH-4Y8TI6KS73N";
                    WebClient client = new WebClient();
                    // Optionally specify an encoding for uploading and downloading strings.
                    client.Encoding = System.Text.Encoding.UTF8;
                    // Upload the data.
                    string response = client.UploadString(URI, data);
                    var result = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(response);
                    // Display the server's response.
                    //Console.WriteLine(reply);
                    //Console.WriteLine("el result Exito: "+result["Exito"]);
                    return result["Exito"];
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("el error en el validate: " + ex);
                return false;
            }
        }
        public string GetConvertFile(string ruta)
        {

            ruta = Environment.ExpandEnvironmentVariables(ruta);
            byte[] data = File.ReadAllBytes(ruta);
            string archivoBase64 = Convert.ToBase64String(data, 0, data.Length);
            return archivoBase64;
        }

        public void CrearZip(string fileToAdd, string paths)
        {
            try
            {
                var outFileName = Path.GetFileNameWithoutExtension(fileToAdd) + ".zip";
                var fileNameToAdd = Path.Combine(paths, fileToAdd);
                var zipFileName = Path.Combine(paths, outFileName);

                //Crear el archivo (si quieres puedes editar uno existente cambiando el modo a Update.
                using (ZipArchive archive = ZipFile.Open(zipFileName, ZipArchiveMode.Create))
                {
                    archive.CreateEntryFromFile(fileNameToAdd, Path.GetFileName(fileNameToAdd));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("el ex : " + ex);
                throw ex;
            }
        }

        public void DesZip(string fileToAdd, string paths)
        {
            try
            {
                var outFileName = Path.GetFileNameWithoutExtension(fileToAdd) + ".zip";
                var fileNameToAdd = Path.Combine(paths, fileToAdd);
                var zipFileName = Path.Combine(paths, outFileName);

                using (var archive = ZipFile.Open(zipFileName, ZipArchiveMode.Read))
                {
                    archive.ExtractToDirectory(@"C:\archivos\descomprimido");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("el ex : " + ex);
                throw ex;
            }
        }

        public Dictionary<string, dynamic> processInsertText(List<SbsReportEnity> listaSiniestros, string txtUnica, string txtMultiple)
        {
            try
            {
                //string cadenaTxt = "";
                foreach (var item in listaSiniestros)
                {
                    string modalidad = item.MODALIDAD;
                    if (modalidad == "U")
                    {
                        txtUnica = txtUnica + item.FILA;
                        txtUnica = txtUnica + item.OFICINA;
                        txtUnica = txtUnica + item.OPERACION;
                        txtUnica = txtUnica + item.INTERNO;
                        txtUnica = txtUnica + item.MODALIDAD;
                        txtUnica = txtUnica + item.OPE_UBIGEO;
                        txtUnica = txtUnica + item.OPE_FECHA;
                        txtUnica = txtUnica + item.OPE_HORA;
                        txtUnica = txtUnica + item.EJE_RELACION;
                        txtUnica = txtUnica + item.EJE_CONDICION;
                        txtUnica = txtUnica + item.EJE_TIPPER;
                        txtUnica = txtUnica + item.EJE_TIPDOC;
                        txtUnica = txtUnica + item.EJE_NUMDOC;
                        txtUnica = txtUnica + item.EJE_NUMRUC;
                        txtUnica = txtUnica + item.EJE_APEPAT;
                        txtUnica = txtUnica + item.EJE_APEMAT;
                        txtUnica = txtUnica + item.EJE_NOMBRES;
                        txtUnica = txtUnica + item.EJE_OCUPACION;
                        txtUnica = txtUnica + item.EJE_PAIS;
                        txtUnica = txtUnica + item.EJE_CARGO;
                        txtUnica = txtUnica + item.EJE_PEP;
                        txtUnica = txtUnica + item.EJE_DOMICILIO;
                        txtUnica = txtUnica + item.EJE_DEPART;
                        txtUnica = txtUnica + item.EJE_PROV;
                        txtUnica = txtUnica + item.EJE_DIST;
                        txtUnica = txtUnica + item.EJE_TELEFONO;
                        txtUnica = txtUnica + item.ORD_RELACION;
                        txtUnica = txtUnica + item.ORD_CONDICION;
                        txtUnica = txtUnica + item.ORD_TIPPER;
                        txtUnica = txtUnica + item.ORD_TIPDOC;
                        txtUnica = txtUnica + item.ORD_NUMDOC;
                        txtUnica = txtUnica + item.ORD_NUMRUC;
                        txtUnica = txtUnica + item.ORD_APEPAT;
                        txtUnica = txtUnica + item.ORD_APEMAT;
                        txtUnica = txtUnica + item.ORD_NOMBRES;
                        txtUnica = txtUnica + item.ORD_OCUPACION;
                        txtUnica = txtUnica + item.ORD_PAIS;
                        txtUnica = txtUnica + item.ORD_CARGO;
                        txtUnica = txtUnica + item.ORD_PEP;
                        txtUnica = txtUnica + item.ORD_DOMICILIO;
                        txtUnica = txtUnica + item.ORD_DEPART;
                        txtUnica = txtUnica + item.ORD_PROV;
                        txtUnica = txtUnica + item.ORD_DIST;
                        //txtUnica = txtUnica + item.ORD_UBIGEO;
                        txtUnica = txtUnica + item.ORD_TELEFONO;
                        txtUnica = txtUnica + item.BEN_RELACION;
                        txtUnica = txtUnica + item.BEN_CONDICION;
                        txtUnica = txtUnica + item.BEN_TIP_PER;
                        txtUnica = txtUnica + item.BEN_TIP_DOC;
                        txtUnica = txtUnica + item.BEN_NUM_DOC;
                        txtUnica = txtUnica + item.BEN_NUM_RUC;
                        txtUnica = txtUnica + item.BEN_APEPAT;
                        txtUnica = txtUnica + item.BEN_APEMAT;
                        txtUnica = txtUnica + item.BEN_NOMBRES;
                        txtUnica = txtUnica + item.BEN_OCUPACION;
                        txtUnica = txtUnica + item.BEN_PAIS;
                        txtUnica = txtUnica + item.BEN_CARGO;
                        txtUnica = txtUnica + item.BEN_PEP;
                        txtUnica = txtUnica + item.BEN_DOMICILIO;
                        txtUnica = txtUnica + item.BEN_DEPART;
                        txtUnica = txtUnica + item.BEN_PROV;
                        txtUnica = txtUnica + item.BEN_DIST;
                        //txtUnica = txtUnica + item.BEN_UBIGEO;
                        txtUnica = txtUnica + item.BEN_TELEFONO;
                        txtUnica = txtUnica + item.DAT_TIPFON;
                        txtUnica = txtUnica + item.DAT_TIPOPE;
                        txtUnica = txtUnica + item.DAT_DESOPE;
                        txtUnica = txtUnica + item.DAT_ORIFON;
                        txtUnica = txtUnica + item.DAT_MONOPE;
                        txtUnica = txtUnica + item.DAT_MONOPE_A;
                        txtUnica = txtUnica + item.DAT_MTOOPE;
                        txtUnica = txtUnica + item.DAT_MTOOPEA;
                        txtUnica = txtUnica + item.DAT_COD_ENT_INVO;
                        txtUnica = txtUnica + item.DAT_COD_TIP_CTAO;
                        txtUnica = txtUnica + item.DAT_COD_CTAO;
                        txtUnica = txtUnica + item.DAT_ENT_FNC_EXTO;
                        txtUnica = txtUnica + item.DAT_COD_ENT_INVB;
                        txtUnica = txtUnica + item.DAT_COD_TIP_CTAB;
                        txtUnica = txtUnica + item.DAT_COD_CTAB;
                        txtUnica = txtUnica + item.DAT_ENT_FNC_EXTB;
                        txtUnica = txtUnica + item.DAT_ALCANCE;
                        txtUnica = txtUnica + item.DAT_COD_PAISO;
                        txtUnica = txtUnica + item.DAT_COD_PAISD;
                        txtUnica = txtUnica + item.DAT_INTOPE;
                        txtUnica = txtUnica + item.DAT_FORMA;
                        txtUnica = txtUnica + item.DAT_INFORM;
                        txtUnica = txtUnica + Environment.NewLine;
                    }
                    else
                    {
                        txtMultiple = txtMultiple + item.FILA;
                        txtMultiple = txtMultiple + item.OFICINA;
                        txtMultiple = txtMultiple + item.OPERACION;
                        txtMultiple = txtMultiple + item.INTERNO;
                        txtMultiple = txtMultiple + item.MODALIDAD;
                        txtMultiple = txtMultiple + item.OPE_UBIGEO;
                        txtMultiple = txtMultiple + item.OPE_FECHA;
                        txtMultiple = txtMultiple + item.OPE_HORA;
                        txtMultiple = txtMultiple + item.EJE_RELACION;
                        txtMultiple = txtMultiple + item.EJE_CONDICION;
                        txtMultiple = txtMultiple + item.EJE_TIPPER;
                        txtMultiple = txtMultiple + item.EJE_TIPDOC;
                        txtMultiple = txtMultiple + item.EJE_NUMDOC;
                        txtMultiple = txtMultiple + item.EJE_NUMRUC;
                        txtMultiple = txtMultiple + item.EJE_APEPAT;
                        txtMultiple = txtMultiple + item.EJE_APEMAT;
                        txtMultiple = txtMultiple + item.EJE_NOMBRES;
                        txtMultiple = txtMultiple + item.EJE_OCUPACION;
                        txtMultiple = txtMultiple + item.EJE_PAIS;
                        txtMultiple = txtMultiple + item.EJE_CARGO;
                        txtMultiple = txtMultiple + item.EJE_PEP;
                        txtMultiple = txtMultiple + item.EJE_DOMICILIO;
                        txtMultiple = txtMultiple + item.EJE_DEPART;
                        txtMultiple = txtMultiple + item.EJE_PROV;
                        txtMultiple = txtMultiple + item.EJE_DIST;
                        txtMultiple = txtMultiple + item.EJE_TELEFONO;
                        txtMultiple = txtMultiple + item.ORD_RELACION;
                        txtMultiple = txtMultiple + item.ORD_CONDICION;
                        txtMultiple = txtMultiple + item.ORD_TIPPER;
                        txtMultiple = txtMultiple + item.ORD_TIPDOC;
                        txtMultiple = txtMultiple + item.ORD_NUMDOC;
                        txtMultiple = txtMultiple + item.ORD_NUMRUC;
                        txtMultiple = txtMultiple + item.ORD_APEPAT;
                        txtMultiple = txtMultiple + item.ORD_APEMAT;
                        txtMultiple = txtMultiple + item.ORD_NOMBRES;
                        txtMultiple = txtMultiple + item.ORD_OCUPACION;
                        txtMultiple = txtMultiple + item.ORD_PAIS;
                        txtMultiple = txtMultiple + item.ORD_CARGO;
                        txtMultiple = txtMultiple + item.ORD_PEP;
                        txtMultiple = txtMultiple + item.ORD_DOMICILIO;
                        txtMultiple = txtMultiple + item.ORD_DEPART;
                        txtMultiple = txtMultiple + item.ORD_PROV;
                        txtMultiple = txtMultiple + item.ORD_DIST;
                        //txtMultiple = txtMultiple + item.ORD_UBIGEO;
                        txtMultiple = txtMultiple + item.ORD_TELEFONO;
                        txtMultiple = txtMultiple + item.BEN_RELACION;
                        txtMultiple = txtMultiple + item.BEN_CONDICION;
                        txtMultiple = txtMultiple + item.BEN_TIP_PER;
                        txtMultiple = txtMultiple + item.BEN_TIP_DOC;
                        txtMultiple = txtMultiple + item.BEN_NUM_DOC;
                        txtMultiple = txtMultiple + item.BEN_NUM_RUC;
                        txtMultiple = txtMultiple + item.BEN_APEPAT;
                        txtMultiple = txtMultiple + item.BEN_APEMAT;
                        txtMultiple = txtMultiple + item.BEN_NOMBRES;
                        txtMultiple = txtMultiple + item.BEN_OCUPACION;
                        txtMultiple = txtMultiple + item.BEN_PAIS;
                        txtMultiple = txtMultiple + item.BEN_CARGO;
                        txtMultiple = txtMultiple + item.BEN_PEP;
                        txtMultiple = txtMultiple + item.BEN_DOMICILIO;
                        txtMultiple = txtMultiple + item.BEN_DEPART;
                        txtMultiple = txtMultiple + item.BEN_PROV;
                        txtMultiple = txtMultiple + item.BEN_DIST;
                        //txtMultiple = txtMultiple + item.BEN_UBIGEO;
                        txtMultiple = txtMultiple + item.BEN_TELEFONO;
                        txtMultiple = txtMultiple + item.DAT_TIPFON;
                        txtMultiple = txtMultiple + item.DAT_TIPOPE;
                        txtMultiple = txtMultiple + item.DAT_DESOPE;
                        txtMultiple = txtMultiple + item.DAT_ORIFON;
                        txtMultiple = txtMultiple + item.DAT_MONOPE;
                        txtMultiple = txtMultiple + item.DAT_MONOPE_A;
                        txtMultiple = txtMultiple + item.DAT_MTOOPE;
                        txtMultiple = txtMultiple + item.DAT_MTOOPEA;
                        txtMultiple = txtMultiple + item.DAT_COD_ENT_INVO;
                        txtMultiple = txtMultiple + item.DAT_COD_TIP_CTAO;
                        txtMultiple = txtMultiple + item.DAT_COD_CTAO;
                        txtMultiple = txtMultiple + item.DAT_ENT_FNC_EXTO;
                        txtMultiple = txtMultiple + item.DAT_COD_ENT_INVB;
                        txtMultiple = txtMultiple + item.DAT_COD_TIP_CTAB;
                        txtMultiple = txtMultiple + item.DAT_COD_CTAB;
                        txtMultiple = txtMultiple + item.DAT_ENT_FNC_EXTB;
                        txtMultiple = txtMultiple + item.DAT_ALCANCE;
                        txtMultiple = txtMultiple + item.DAT_COD_PAISO;
                        txtMultiple = txtMultiple + item.DAT_COD_PAISD;
                        txtMultiple = txtMultiple + item.DAT_INTOPE;
                        txtMultiple = txtMultiple + item.DAT_FORMA;
                        txtMultiple = txtMultiple + item.DAT_INFORM;
                        txtMultiple = txtMultiple + Environment.NewLine;
                    }

                }
                Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
                resp["cadenaTxtUnica"] = txtUnica;
                resp["cadenaTxtMultiple"] = txtMultiple;
                return resp;
            }
            catch (Exception ex)
            {
                Console.WriteLine("el ex : " + ex);
                Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
                return objRespuesta;
            }
        }

        public string generateExcelByArray(DataTable arrayData, string nameFile)
        {
            try
            {
                string pathFile = @"C:\archivos\" + nameFile + ".xlsx";
                SLDocument oSLDocument = new SLDocument();

                System.Data.DataTable dt = new System.Data.DataTable();
                dt = arrayData;

                /*dt.Columns.Add("Nombre",typeof(string));
                dt.Columns.Add("Edad",typeof(int));
                dt.Columns.Add("Sexo",typeof(string));

                dt.Rows.Add("Pepe",19,"Hombre");
                dt.Rows.Add("Ana",20,"Mujer");
                dt.Rows.Add("Perla",30,"Mujer");*/


                oSLDocument.ImportDataTable(1, 1, dt, true);
                oSLDocument.SaveAs(pathFile);

                return "Exito";
            }
            catch (Exception ex)
            {
                Console.WriteLine("el error : " + ex);
                throw ex;
            }
        }

        public Dictionary<string, dynamic> processPagosManuales(dynamic param)
        {
            Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
            try
            {
                string nameFile = "NOMBRE_PAGOS_MASIVOS.xlsx";
                string nameSinExtension = Path.GetFileNameWithoutExtension(nameFile);
                string path = @"C:\reportes\Estructura datos reporte RO.xlsx";
                SLDocument sl = new SLDocument(path, "Estructura Memos manuales");
                Console.WriteLine("EL inicio path : " + path);
                Console.WriteLine("EL segundo : " + sl.GetCellValueAsString(1, 1));
                int iRow = 2;
                while (!string.IsNullOrEmpty(sl.GetCellValueAsString(iRow, 1)))
                {
                    string correlativo = sl.GetCellValueAsString(iRow, 1);
                    string nro_memo = sl.GetCellValueAsString(iRow, 2);
                    string motivo = sl.GetCellValueAsString(iRow, 3);
                    string canal_area = sl.GetCellValueAsString(iRow, 4);
                    string asunto = sl.GetCellValueAsString(iRow, 5);
                    string fecha_solicitud = sl.GetCellValueAsString(iRow, 6);
                    string usuario_solicitante = sl.GetCellValueAsString(iRow, 7);
                    string pol_cert_sin = sl.GetCellValueAsString(iRow, 8);
                    string dni_benef_pago = sl.GetCellValueAsString(iRow, 9);
                    string moneda = sl.GetCellValueAsString(iRow, 10);
                    string monto = sl.GetCellValueAsString(iRow, 11);
                    Console.WriteLine("EL correlativo : " + correlativo);
                    Console.WriteLine("EL nro_memo : " + nro_memo);
                    Console.WriteLine("EL motivo : " + motivo);
                    Console.WriteLine("EL canal_area : " + canal_area);
                    Console.WriteLine("EL asunto : " + asunto);
                    Console.WriteLine("EL fecha_solicitud : " + fecha_solicitud);
                    Console.WriteLine("EL usuario_solicitante : " + usuario_solicitante);
                    Console.WriteLine("EL pol_cert_sin : " + pol_cert_sin);
                    Console.WriteLine("EL dni_benef_pago : " + dni_benef_pago);
                    Console.WriteLine("EL moneda : " + moneda);
                    Console.WriteLine("EL monto : " + monto);
                    SbsCargaPagosManualesDTO objPago = new SbsCargaPagosManualesDTO();
                    objPago.NCODIGO_IDEN = pol_cert_sin + "-" + dni_benef_pago;
                    objPago.NCORRELATIVO = correlativo;
                    objPago.NMEMO = nro_memo;
                    objPago.SMOTIVO = motivo;
                    objPago.SCANAL_AREA = canal_area;
                    objPago.SASUNTO = asunto;
                    objPago.SFECHA_SOLICITUD = fecha_solicitud;
                    objPago.SUSUARIO_SOLICITANTE = usuario_solicitante;
                    objPago.POLIZA_CERTIFICADO_SINIESTRO = pol_cert_sin;
                    objPago.DNI_BENEF_PAGO = dni_benef_pago;
                    objPago.SMONEDA = moneda;
                    objPago.NMONTO = Convert.ToDecimal(monto.ToString());

                    var respInsert = this.sbsReportRepository.InsertDataPAGOSMANUALES(objPago);
                    Console.WriteLine("La respuesta respInsert: " + respInsert["code"]);
                    Console.WriteLine("La respuesta respInsert message: " + respInsert["message"]);
                    //int edad = sl.GetCellValueAsInt32(iRow, 3);

                    iRow++;
                }
                objRespuesta["code"] = 0;
                objRespuesta["message"] = "";
                return objRespuesta;
            }
            catch (Exception ex)
            {
                Console.WriteLine("El error en el service de insertar pagos manuales : " + ex);
                objRespuesta["code"] = 2;
                objRespuesta["messageError"] = ex.Message.ToString();
                objRespuesta["messageErrorDetalle"] = ex;
                return objRespuesta;
            }
        }

        //public Dictionary<string,dynamic> copyFilePaste(dynamic param){
        //    Dictionary<string,dynamic> objRespuesta = new Dictionary<string,dynamic>();
        //    try
        //    {
        //        string ruta = @"C:\plantillasLaft\";
        //        object oMissing = System.Reflection.Missing.Value;
        //        objWord.Application objAplication = new objWord.Application();
        //        //objWord.Document objDocumento = objAplication.Documents.Add(oMissing,oMissing,oMissing,oMissing);
        //        objWord.Document objDocumento = objAplication.Documents.Add();
        //        //objAplication.Visible = true;

        //        object start = 0;
        //        object end = 0;
        //        objWord.Range tableLocation = objDocumento.Range(ref start, ref end);

        //        objDocumento.Tables.Add(tableLocation, 2, 3);


        //        objWord.Table table = objDocumento.Tables[1];
        //        //table.Columns[0].Width = objAplication.PixelsToPoints(100f);;
        //        //objWord.Styles styles = table.Range.Document.Styles;
        //        //table.ApplyStyle(BuiltinTableStyle.MediumShading1Accent1);
        //        table.Range.Font.Size = 8;
        //        table.Columns.DistributeWidth();
        //        //object style = "Heading 1";
        //        //table.set_Style("Table Heading White");
        //        objWord.WdBorderType BorderLeft = objWord.WdBorderType.wdBorderLeft;
        //        objWord.WdBorderType BorderRight = objWord.WdBorderType.wdBorderRight;
        //        objWord.WdBorderType BorderTop = objWord.WdBorderType.wdBorderTop;
        //        objWord.WdBorderType BorderBottom = objWord.WdBorderType.wdBorderBottom;

        //        table.Cell(1,1).Range.Text = "CÓDIGO";
        //        table.Cell(1,1).Range.Font.Color = objWord.WdColor.wdColorBlack;
        //        table.Cell(1,1).Range.Font.Bold = 1;
        //        table.Cell(1,1).Range.Borders[BorderLeft].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(1,1).Range.Borders[BorderRight].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(1,1).Range.Borders[BorderTop].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(1,1).Range.Borders[BorderBottom].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(1,2).Range.Text = "SEÑAL DE ALERTA";
        //        table.Cell(1,2).Range.Font.Color = objWord.WdColor.wdColorBlack;
        //        table.Cell(1,2).Range.Font.Bold = 1;
        //        table.Cell(1,2).Range.Borders[BorderLeft].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(1,2).Range.Borders[BorderRight].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(1,2).Range.Borders[BorderTop].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(1,2).Range.Borders[BorderBottom].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(1,3).Range.Text = "PRODUCTO";
        //        table.Cell(1,3).Range.Font.Color = objWord.WdColor.wdColorBlack;
        //        table.Cell(1,3).Range.Font.Bold = 1;
        //        table.Cell(1,3).Range.Borders[BorderLeft].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(1,3).Range.Borders[BorderRight].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(1,3).Range.Borders[BorderTop].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(1,3).Range.Borders[BorderBottom].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(2,1).Range.Text = "C1";
        //        table.Cell(2,1).Range.ParagraphFormat.Alignment = objWord.WdParagraphAlignment.wdAlignParagraphCenter;
        //        table.Cell(2,1).Range.Borders[BorderLeft].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(2,1).Range.Borders[BorderRight].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(2,1).Range.Borders[BorderTop].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(2,1).Range.Borders[BorderBottom].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(2,2).Range.Text = "Con relación a las organizaciones sin fines de lucro, tales como las asociaciones, fundaciones, comités, ONG, entre otras, operaciones no parecen tener un propósito económico lógico o no parece existir un vínculo entre la actividad declarada por la organización y las demás partes que participan en la transacción. ";
        //        table.Cell(2,2).Range.ParagraphFormat.Alignment = objWord.WdParagraphAlignment.wdAlignParagraphJustify;
        //        table.Cell(2,2).Range.Borders[BorderLeft].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(2,2).Range.Borders[BorderRight].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(2,2).Range.Borders[BorderTop].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(2,2).Range.Borders[BorderBottom].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(2,3).Range.Text = "Todos los productos";
        //        table.Cell(2,3).Range.ParagraphFormat.Alignment = objWord.WdParagraphAlignment.wdAlignParagraphCenter;
        //        table.Cell(2,3).Range.Borders[BorderLeft].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(2,3).Range.Borders[BorderRight].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(2,3).Range.Borders[BorderTop].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table.Cell(2,3).Range.Borders[BorderBottom].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;

        //        int sizeFont = 12;

        //        objWord.Paragraph objParrafo1 = objDocumento.Content.Paragraphs.Add(Type.Missing);

        //        objParrafo1.Range.Font.Size = sizeFont;
        //        objParrafo1.Range.Font.Color = objWord.WdColor.wdColorBlack;
        //        objParrafo1.Range.Text = "1.	Metodología aplicada";
        //        objParrafo1.Range.InsertParagraphAfter();

        //        objWord.Paragraph objParrafo2 = objDocumento.Content.Paragraphs.Add(Type.Missing);

        //        objParrafo2.Range.Font.Size = sizeFont;
        //        objParrafo2.Range.Font.Color = objWord.WdColor.wdColorBlack;
        //        objParrafo2.Range.Text = "•	La Oficialía de Cumplimiento envía una primera consulta al área de Comercial Masivos, en el cual cuestiona si como parte de sus funciones identificaron la situación que describe la señal de alerta “C1”.";
        //        objParrafo2.Range.InsertParagraphAfter();

        //        objWord.Paragraph objParrafo3 = objDocumento.Content.Paragraphs.Add(Type.Missing);
        //        objParrafo3.Range.Font.Size = sizeFont;
        //        objParrafo3.Range.Font.Color = objWord.WdColor.wdColorBlack;
        //        objParrafo3.Range.Text = "•	Luego, solicita al área de Operaciones & TI la base de Contratantes con pólizas vigentes al cierre del segundo semestre, de todos los productos de la cartera de la Compañía.";
        //        objParrafo3.Range.InsertParagraphAfter();
        //        objWord.Paragraph objParrafo4 = objDocumento.Content.Paragraphs.Add(Type.Missing);
        //        objParrafo4.Range.Font.Size = sizeFont;
        //        objParrafo4.Range.Font.Color = objWord.WdColor.wdColorBlack;
        //        objParrafo4.Range.Text = "•	La Oficialía de Cumplimiento selecciona una muestra del universo de clientes, que son Cooperativas, Cajas y ONG’s y realiza una segunda consulta al área de Comercial Masivos cuestionando (1) si las operaciones con dichos contratantes guardan relación con la actividad económica del cliente y (2) si los precios con que se comercializa a dichos contratantes, se alinean a las políticas de la Compañía y están dentro de los parámetros de normalidad vigentes en el mercado.";
        //        objParrafo4.Range.InsertParagraphAfter();
        //        objWord.Paragraph objParrafo5 = objDocumento.Content.Paragraphs.Add(Type.Missing);
        //        objParrafo5.Range.Font.Size = sizeFont;
        //        objParrafo5.Range.Font.Color = objWord.WdColor.wdColorBlack;
        //        objParrafo5.Range.Text = "•	En caso se identifique señal de alerta y no sea descartado como falso positivo, se procederá a analizar la inusualidad de la operación según los criterios para la identificación de operaciones inusuales (Capítulo X del Manual SPLAFT).";
        //        objParrafo5.Range.InsertParagraphAfter();
        //        objWord.Paragraph objParrafo6 = objDocumento.Content.Paragraphs.Add(Type.Missing);
        //        objParrafo6.Range.Font.Size = sizeFont;
        //        objParrafo6.Range.Font.Color = objWord.WdColor.wdColorBlack;
        //        objParrafo6.Range.Text = "a.	Resultados y conclusiones";
        //        objParrafo6.Range.InsertParagraphAfter();
        //        objWord.Paragraph objParrafo7 = objDocumento.Content.Paragraphs.Add(Type.Missing);
        //        objParrafo7.Range.Font.Size = sizeFont;
        //        objParrafo7.Range.Font.Color = objWord.WdColor.wdColorBlack;
        //        objParrafo7.Range.Text = "•	El área de Comercial Masivos señaló {{rpta111}} haber identificado ninguna situación atípica respecto a la señal de alerta “C1”. Se tiene el correo como sustento.";
        //        objParrafo7.Range.InsertParagraphAfter();
        //        objWord.Paragraph objParrafo8 = objDocumento.Content.Paragraphs.Add(Type.Missing);
        //        objParrafo8.Range.Font.Size = sizeFont;
        //        objParrafo8.Range.Font.Color = objWord.WdColor.wdColorBlack;
        //        objParrafo8.Range.Text = "Ver módulo xxxx de la Plataforma PLAFT";
        //        objParrafo8.Range.InsertParagraphAfter();
        //        objWord.Paragraph objParrafo9 = objDocumento.Content.Paragraphs.Add(Type.Missing);
        //        objParrafo9.Range.Font.Size = sizeFont;
        //        objParrafo9.Range.Font.Color = objWord.WdColor.wdColorBlack;
        //        objParrafo9.Range.Text = "•	Respecto a la segunda consulta, el área de Comercial Masivos indicó que las operaciones con los contratantes de la muestra {{rpta121}} guaran relación con la actividad económica del cliente y los precios con los cuales se están comercializando a los contratantes se alinean a las políticas de la compañía y están dentro de los parámetros de normalidad vigentes en el mercado. \n Ver módulo xxxx de la Plataforma PLAFT";
        //        objParrafo9.Range.InsertParagraphAfter();
        //        objWord.Paragraph objParrafo10 = objDocumento.Content.Paragraphs.Add(Type.Missing);
        //        objParrafo10.Range.Font.Size = sizeFont;
        //        objParrafo10.Range.Font.Color = objWord.WdColor.wdColorBlack;
        //        objParrafo10.Range.Text = "•	Por lo tanto, se concluye que por la señal de alerta “C1-RS” {{rpta122}} se identificaron casos.";
        //        objParrafo10.Range.InsertParagraphAfter();

        //        objWord.Paragraph objParrafo11 = objDocumento.Content.Paragraphs.Add(Type.Missing);
        //        objParrafo11.Range.Font.Size = sizeFont;
        //        objParrafo11.Range.Font.Color = objWord.WdColor.wdColorBlack;
        //        objParrafo11.Range.Text = "\n \n";
        //        objParrafo11.Range.InsertParagraphAfter();

        //        objWord.Paragraph objParrafo12 = objDocumento.Content.Paragraphs.Add(Type.Missing);
        //        objParrafo12.Range.Font.Size = sizeFont;
        //        objParrafo12.Range.Font.Color = objWord.WdColor.wdColorBlack;
        //        objParrafo12.Range.Text = "hola";
        //        objParrafo12.Range.InsertParagraphAfter();

        //        object start2 = 10;
        //        object end2 = 10;
        //        objWord.Range tableLocation2 = objDocumento.Range(ref start2, ref end2);
        //        objDocumento.Tables.Add(tableLocation2, 2, 3);
        //        objWord.Table table2 = objDocumento.Tables[1];
        //        //table.Columns[0].Width = objAplication.PixelsToPoints(100f);;
        //        //objWord.Styles styles = table.Range.Document.Styles;
        //        //table.ApplyStyle(BuiltinTableStyle.MediumShading1Accent1);
        //        table2.Range.Font.Size = 8;
        //        table2.Columns.DistributeWidth();
        //        //object style = "Heading 1";
        //        //table.set_Style("Table Heading White");
        //        /*objWord.WdBorderType BorderLeft = objWord.WdBorderType.wdBorderLeft;
        //        objWord.WdBorderType BorderRight = objWord.WdBorderType.wdBorderRight;
        //        objWord.WdBorderType BorderTop = objWord.WdBorderType.wdBorderTop;
        //        objWord.WdBorderType BorderBottom = objWord.WdBorderType.wdBorderBottom;*/

        //        table2.Cell(1,1).Range.Text = "CÓDIGO";
        //        table2.Cell(1,1).Range.Font.Color = objWord.WdColor.wdColorBlack;
        //        table2.Cell(1,1).Range.Font.Bold = 1;
        //        table2.Cell(1,1).Range.Borders[BorderLeft].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(1,1).Range.Borders[BorderRight].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(1,1).Range.Borders[BorderTop].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(1,1).Range.Borders[BorderBottom].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(1,2).Range.Text = "SEÑAL DE ALERTA";
        //        table2.Cell(1,2).Range.Font.Color = objWord.WdColor.wdColorBlack;
        //        table2.Cell(1,2).Range.Font.Bold = 1;
        //        table2.Cell(1,2).Range.Borders[BorderLeft].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(1,2).Range.Borders[BorderRight].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(1,2).Range.Borders[BorderTop].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(1,2).Range.Borders[BorderBottom].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(1,3).Range.Text = "PRODUCTO";
        //        table2.Cell(1,3).Range.Font.Color = objWord.WdColor.wdColorBlack;
        //        table2.Cell(1,3).Range.Font.Bold = 1;
        //        table2.Cell(1,3).Range.Borders[BorderLeft].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(1,3).Range.Borders[BorderRight].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(1,3).Range.Borders[BorderTop].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(1,3).Range.Borders[BorderBottom].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(2,1).Range.Text = "C2";
        //        table2.Cell(2,1).Range.ParagraphFormat.Alignment = objWord.WdParagraphAlignment.wdAlignParagraphCenter;
        //        table2.Cell(2,1).Range.Borders[BorderLeft].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(2,1).Range.Borders[BorderRight].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(2,1).Range.Borders[BorderTop].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(2,1).Range.Borders[BorderBottom].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(2,2).Range.Text = "Con relación a las organizaciones sin fines de lucro, tales como las asociaciones, fundaciones, comités, ONG, entre otras, operaciones no parecen tener un propósito económico lógico o no parece existir un vínculo entre la actividad declarada por la organización y las demás partes que participan en la transacción. ";
        //        table2.Cell(2,2).Range.ParagraphFormat.Alignment = objWord.WdParagraphAlignment.wdAlignParagraphJustify;
        //        table2.Cell(2,2).Range.Borders[BorderLeft].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(2,2).Range.Borders[BorderRight].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(2,2).Range.Borders[BorderTop].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(2,2).Range.Borders[BorderBottom].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(2,3).Range.Text = "Todos los productos";
        //        table2.Cell(2,3).Range.ParagraphFormat.Alignment = objWord.WdParagraphAlignment.wdAlignParagraphCenter;
        //        table2.Cell(2,3).Range.Borders[BorderLeft].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(2,3).Range.Borders[BorderRight].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(2,3).Range.Borders[BorderTop].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;
        //        table2.Cell(2,3).Range.Borders[BorderBottom].LineStyle = objWord.WdLineStyle.wdLineStyleSingle;


        //        objDocumento.SaveAs2(ruta+"pruebaWord1");
        //        objDocumento.Close();
        //        objAplication.Quit();

        //        objRespuesta["code"] = 0;
        //        objRespuesta["message"]="Correcto";
        //        return objRespuesta;
        //    }catch(Exception e){
        //        Console.WriteLine("EL error e : "+e);
        //        objRespuesta["code"] = 0;
        //        objRespuesta["message"]="Error";
        //        objRespuesta["messageError"]=e;
        //        return objRespuesta;
        //    }



        //}

        public Dictionary<string, dynamic> convertToTxt(dynamic param)
        {
            Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
            try
            {
                string nameFile = "NOMBRE_PAGOS_MASIVOS.xlsx";
                string nameSinExtension = Path.GetFileNameWithoutExtension(nameFile);
                string path = @"C:\reportes\01211130_3.xlsx";
                SLDocument sl = new SLDocument(path, "Sheet1");
                var worksheetStats = sl.GetWorksheetStatistics();
                Console.WriteLine("EL inicio path : " + path);
                Console.WriteLine("EL segundo : " + sl.GetCellValueAsString(1, 1));
                int iRow = 1;
                string txtUnica = "";
                string txtMultiple = "";
                List<SbsReportEnity> arrObjsSbs = new List<SbsReportEnity>();
                for (int inc = 1; inc <= worksheetStats.EndRowIndex; inc++)
                {
                    SbsReportEnity objReport = new SbsReportEnity();
                    objReport.FILA = sl.GetCellValueAsString(inc, 1);
                    objReport.OFICINA = sl.GetCellValueAsString(inc, 2);
                    objReport.OPERACION = sl.GetCellValueAsString(inc, 3);
                    objReport.INTERNO = sl.GetCellValueAsString(inc, 4);
                    objReport.MODALIDAD = sl.GetCellValueAsString(inc, 5);
                    objReport.OPE_UBIGEO = sl.GetCellValueAsString(inc, 6);
                    objReport.OPE_FECHA = sl.GetCellValueAsString(inc, 7);
                    objReport.OPE_HORA = sl.GetCellValueAsString(inc, 8);
                    objReport.EJE_RELACION = sl.GetCellValueAsString(inc, 9);
                    objReport.EJE_CONDICION = sl.GetCellValueAsString(inc, 10);
                    objReport.EJE_TIPPER = sl.GetCellValueAsString(inc, 11);
                    objReport.EJE_TIPDOC = sl.GetCellValueAsString(inc, 12);
                    objReport.EJE_NUMDOC = sl.GetCellValueAsString(inc, 13);
                    objReport.EJE_NUMRUC = sl.GetCellValueAsString(inc, 14);
                    objReport.EJE_APEPAT = sl.GetCellValueAsString(inc, 15);
                    objReport.EJE_APEMAT = sl.GetCellValueAsString(inc, 16);
                    objReport.EJE_NOMBRES = sl.GetCellValueAsString(inc, 17);
                    objReport.EJE_OCUPACION = sl.GetCellValueAsString(inc, 18);
                    objReport.EJE_PAIS = sl.GetCellValueAsString(inc, 19);
                    objReport.EJE_CARGO = sl.GetCellValueAsString(inc, 20);
                    objReport.EJE_PEP = sl.GetCellValueAsString(inc, 21);
                    objReport.EJE_DOMICILIO = sl.GetCellValueAsString(inc, 22);
                    objReport.EJE_DEPART = sl.GetCellValueAsString(inc, 23);
                    objReport.EJE_PROV = sl.GetCellValueAsString(inc, 24);
                    objReport.EJE_DIST = sl.GetCellValueAsString(inc, 25);
                    objReport.EJE_TELEFONO = sl.GetCellValueAsString(inc, 26);
                    objReport.ORD_RELACION = sl.GetCellValueAsString(inc, 27);
                    objReport.ORD_CONDICION = sl.GetCellValueAsString(inc, 28);
                    objReport.ORD_TIPPER = sl.GetCellValueAsString(inc, 29);
                    objReport.ORD_TIPDOC = sl.GetCellValueAsString(inc, 30);
                    objReport.ORD_NUMDOC = sl.GetCellValueAsString(inc, 31);
                    objReport.ORD_NUMRUC = sl.GetCellValueAsString(inc, 32);
                    objReport.ORD_APEPAT = sl.GetCellValueAsString(inc, 33);
                    objReport.ORD_APEMAT = sl.GetCellValueAsString(inc, 34);
                    objReport.ORD_NOMBRES = sl.GetCellValueAsString(inc, 35);
                    objReport.ORD_OCUPACION = sl.GetCellValueAsString(inc, 36);
                    objReport.ORD_PAIS = sl.GetCellValueAsString(inc, 37);
                    objReport.ORD_CARGO = sl.GetCellValueAsString(inc, 38);
                    objReport.ORD_PEP = sl.GetCellValueAsString(inc, 39);
                    objReport.ORD_DOMICILIO = sl.GetCellValueAsString(inc, 40);
                    objReport.ORD_DEPART = sl.GetCellValueAsString(inc, 41);
                    objReport.ORD_PROV = sl.GetCellValueAsString(inc, 42);
                    objReport.ORD_DIST = sl.GetCellValueAsString(inc, 43);
                    objReport.ORD_TELEFONO = sl.GetCellValueAsString(inc, 44);
                    objReport.BEN_RELACION = sl.GetCellValueAsString(inc, 45);
                    objReport.BEN_CONDICION = sl.GetCellValueAsString(inc, 46);
                    objReport.BEN_TIP_PER = sl.GetCellValueAsString(inc, 47);
                    objReport.BEN_TIP_DOC = sl.GetCellValueAsString(inc, 48);
                    objReport.BEN_NUM_DOC = sl.GetCellValueAsString(inc, 49);
                    objReport.BEN_NUM_RUC = sl.GetCellValueAsString(inc, 50);
                    objReport.BEN_APEPAT = sl.GetCellValueAsString(inc, 51);
                    objReport.BEN_APEMAT = sl.GetCellValueAsString(inc, 52);
                    objReport.BEN_NOMBRES = sl.GetCellValueAsString(inc, 53);
                    objReport.BEN_OCUPACION = sl.GetCellValueAsString(inc, 54);
                    objReport.BEN_PAIS = sl.GetCellValueAsString(inc, 55);
                    objReport.BEN_CARGO = sl.GetCellValueAsString(inc, 56);
                    objReport.BEN_PEP = sl.GetCellValueAsString(inc, 57);
                    objReport.BEN_DOMICILIO = sl.GetCellValueAsString(inc, 58);
                    objReport.BEN_DEPART = sl.GetCellValueAsString(inc, 59);
                    objReport.BEN_PROV = sl.GetCellValueAsString(inc, 60);
                    objReport.BEN_DIST = sl.GetCellValueAsString(inc, 61);
                    objReport.BEN_TELEFONO = sl.GetCellValueAsString(inc, 62);
                    objReport.DAT_TIPFON = sl.GetCellValueAsString(inc, 63);
                    objReport.DAT_TIPOPE = sl.GetCellValueAsString(inc, 64);
                    objReport.DAT_DESOPE = sl.GetCellValueAsString(inc, 65);
                    objReport.DAT_ORIFON = sl.GetCellValueAsString(inc, 66);
                    objReport.DAT_MONOPE = sl.GetCellValueAsString(inc, 67);
                    objReport.DAT_MONOPE_A = sl.GetCellValueAsString(inc, 68);
                    objReport.DAT_MTOOPE = sl.GetCellValueAsString(inc, 69);
                    objReport.DAT_MTOOPEA = sl.GetCellValueAsString(inc, 70);
                    objReport.DAT_COD_ENT_INVO = sl.GetCellValueAsString(inc, 71);
                    objReport.DAT_COD_TIP_CTAO = sl.GetCellValueAsString(inc, 72);
                    objReport.DAT_COD_CTAO = sl.GetCellValueAsString(inc, 73);
                    objReport.DAT_ENT_FNC_EXTO = sl.GetCellValueAsString(inc, 74);
                    objReport.DAT_COD_ENT_INVB = sl.GetCellValueAsString(inc, 75);
                    objReport.DAT_COD_TIP_CTAB = sl.GetCellValueAsString(inc, 76);
                    objReport.DAT_COD_CTAB = sl.GetCellValueAsString(inc, 77);
                    objReport.DAT_ENT_FNC_EXTB = sl.GetCellValueAsString(inc, 78);
                    objReport.DAT_ALCANCE = sl.GetCellValueAsString(inc, 79);
                    objReport.DAT_COD_PAISO = sl.GetCellValueAsString(inc, 80);
                    objReport.DAT_COD_PAISD = sl.GetCellValueAsString(inc, 81);
                    objReport.DAT_INTOPE = sl.GetCellValueAsString(inc, 82);
                    objReport.DAT_FORMA = sl.GetCellValueAsString(inc, 83);
                    objReport.DAT_INFORM = sl.GetCellValueAsString(inc, 84);
                    Console.WriteLine("EL objReport.FILA : " + objReport.FILA);
                    Console.WriteLine("EL objReport.OPERACION : " + objReport.OPERACION);
                    arrObjsSbs.Add(objReport);
                    //int edad = sl.GetCellValueAsInt32(iRow, 3);

                    iRow++;
                }
                Dictionary<string, dynamic> respuesta = this.processInsertText(arrObjsSbs, txtUnica, txtMultiple);
                objRespuesta["code"] = 0;
                objRespuesta["message"] = "";
                if (respuesta["cadenaTxtUnica"].ToString().Length > 0)
                    System.IO.File.WriteAllText(@"C:\reportes\respuestas\pruebaHoyReporte.998", respuesta["cadenaTxtUnica"]);
                else
                    System.IO.File.WriteAllText(@"C:\reportes\respuestas\pruebaHoyReporte.998", respuesta["cadenaTxtMultiple"]);
                return respuesta;//objRespuesta;
            }
            catch (Exception ex)
            {
                Console.WriteLine("El error en el service de insertar pagos manuales : " + ex);
                objRespuesta["code"] = 2;
                objRespuesta["messageError"] = ex.Message.ToString();
                objRespuesta["messageErrorDetalle"] = ex;
                return objRespuesta;
            }
        }

        public Dictionary<string, dynamic> convertToTxtDynamic(dynamic param)
        {
            Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
            try
            {
                string nameFile = "NOMBRE_PAGOS_MASIVOS.xlsx";
                string nameSinExtension = Path.GetFileNameWithoutExtension(nameFile);
                string path = @"C:\reportes\01211130_3.xlsx";
                SLDocument sl = new SLDocument(path, "Sheet1");
                var worksheetStats = sl.GetWorksheetStatistics();
                Console.WriteLine("EL inicio path : " + path);
                Console.WriteLine("EL segundo : " + sl.GetCellValueAsString(1, 1));
                int iRow = 1;
                string txtUnica = "";
                string txtMultiple = "";
                List<SbsReportEnity> arrObjsSbs = new List<SbsReportEnity>();
                for (int inc = 1; inc <= worksheetStats.EndRowIndex; inc++)
                {
                    SbsReportEnity objReport = new SbsReportEnity();
                    objReport.FILA = sl.GetCellValueAsString(inc, 1);
                    objReport.OFICINA = sl.GetCellValueAsString(inc, 2);
                    objReport.OPERACION = sl.GetCellValueAsString(inc, 3);
                    objReport.INTERNO = sl.GetCellValueAsString(inc, 4);
                    objReport.MODALIDAD = sl.GetCellValueAsString(inc, 5);
                    objReport.OPE_UBIGEO = sl.GetCellValueAsString(inc, 6);
                    objReport.OPE_FECHA = sl.GetCellValueAsString(inc, 7);
                    objReport.OPE_HORA = sl.GetCellValueAsString(inc, 8);
                    objReport.EJE_RELACION = sl.GetCellValueAsString(inc, 9);
                    objReport.EJE_CONDICION = sl.GetCellValueAsString(inc, 10);
                    objReport.EJE_TIPPER = sl.GetCellValueAsString(inc, 11);
                    objReport.EJE_TIPDOC = sl.GetCellValueAsString(inc, 12);
                    objReport.EJE_NUMDOC = sl.GetCellValueAsString(inc, 13);
                    objReport.EJE_NUMRUC = sl.GetCellValueAsString(inc, 14);
                    objReport.EJE_APEPAT = sl.GetCellValueAsString(inc, 15);
                    objReport.EJE_APEMAT = sl.GetCellValueAsString(inc, 16);
                    objReport.EJE_NOMBRES = sl.GetCellValueAsString(inc, 17);
                    objReport.EJE_OCUPACION = sl.GetCellValueAsString(inc, 18);
                    objReport.EJE_PAIS = sl.GetCellValueAsString(inc, 19);
                    objReport.EJE_CARGO = sl.GetCellValueAsString(inc, 20);
                    objReport.EJE_PEP = sl.GetCellValueAsString(inc, 21);
                    objReport.EJE_DOMICILIO = sl.GetCellValueAsString(inc, 22);
                    objReport.EJE_DEPART = sl.GetCellValueAsString(inc, 23);
                    objReport.EJE_PROV = sl.GetCellValueAsString(inc, 24);
                    objReport.EJE_DIST = sl.GetCellValueAsString(inc, 25);
                    objReport.EJE_TELEFONO = sl.GetCellValueAsString(inc, 26);
                    objReport.ORD_RELACION = sl.GetCellValueAsString(inc, 27);
                    objReport.ORD_CONDICION = sl.GetCellValueAsString(inc, 28);
                    objReport.ORD_TIPPER = sl.GetCellValueAsString(inc, 29);
                    objReport.ORD_TIPDOC = sl.GetCellValueAsString(inc, 30);
                    objReport.ORD_NUMDOC = sl.GetCellValueAsString(inc, 31);
                    objReport.ORD_NUMRUC = sl.GetCellValueAsString(inc, 32);
                    objReport.ORD_APEPAT = sl.GetCellValueAsString(inc, 33);
                    objReport.ORD_APEMAT = sl.GetCellValueAsString(inc, 34);
                    objReport.ORD_NOMBRES = sl.GetCellValueAsString(inc, 35);
                    objReport.ORD_OCUPACION = sl.GetCellValueAsString(inc, 36);
                    objReport.ORD_PAIS = sl.GetCellValueAsString(inc, 37);
                    objReport.ORD_CARGO = sl.GetCellValueAsString(inc, 38);
                    objReport.ORD_PEP = sl.GetCellValueAsString(inc, 39);
                    objReport.ORD_DOMICILIO = sl.GetCellValueAsString(inc, 40);
                    objReport.ORD_DEPART = sl.GetCellValueAsString(inc, 41);
                    objReport.ORD_PROV = sl.GetCellValueAsString(inc, 42);
                    objReport.ORD_DIST = sl.GetCellValueAsString(inc, 43);
                    objReport.ORD_TELEFONO = sl.GetCellValueAsString(inc, 44);
                    objReport.BEN_RELACION = sl.GetCellValueAsString(inc, 45);
                    objReport.BEN_CONDICION = sl.GetCellValueAsString(inc, 46);
                    objReport.BEN_TIP_PER = sl.GetCellValueAsString(inc, 47);
                    objReport.BEN_TIP_DOC = sl.GetCellValueAsString(inc, 48);
                    objReport.BEN_NUM_DOC = sl.GetCellValueAsString(inc, 49);
                    objReport.BEN_NUM_RUC = sl.GetCellValueAsString(inc, 50);
                    objReport.BEN_APEPAT = sl.GetCellValueAsString(inc, 51);
                    objReport.BEN_APEMAT = sl.GetCellValueAsString(inc, 52);
                    objReport.BEN_NOMBRES = sl.GetCellValueAsString(inc, 53);
                    objReport.BEN_OCUPACION = sl.GetCellValueAsString(inc, 54);
                    objReport.BEN_PAIS = sl.GetCellValueAsString(inc, 55);
                    objReport.BEN_CARGO = sl.GetCellValueAsString(inc, 56);
                    objReport.BEN_PEP = sl.GetCellValueAsString(inc, 57);
                    objReport.BEN_DOMICILIO = sl.GetCellValueAsString(inc, 58);
                    objReport.BEN_DEPART = sl.GetCellValueAsString(inc, 59);
                    objReport.BEN_PROV = sl.GetCellValueAsString(inc, 60);
                    objReport.BEN_DIST = sl.GetCellValueAsString(inc, 61);
                    objReport.BEN_TELEFONO = sl.GetCellValueAsString(inc, 62);
                    objReport.DAT_TIPFON = sl.GetCellValueAsString(inc, 63);
                    objReport.DAT_TIPOPE = sl.GetCellValueAsString(inc, 64);
                    objReport.DAT_DESOPE = sl.GetCellValueAsString(inc, 65);
                    objReport.DAT_ORIFON = sl.GetCellValueAsString(inc, 66);
                    objReport.DAT_MONOPE = sl.GetCellValueAsString(inc, 67);
                    objReport.DAT_MONOPE_A = sl.GetCellValueAsString(inc, 68);
                    objReport.DAT_MTOOPE = sl.GetCellValueAsString(inc, 69);
                    objReport.DAT_MTOOPEA = sl.GetCellValueAsString(inc, 70);
                    objReport.DAT_COD_ENT_INVO = sl.GetCellValueAsString(inc, 71);
                    objReport.DAT_COD_TIP_CTAO = sl.GetCellValueAsString(inc, 72);
                    objReport.DAT_COD_CTAO = sl.GetCellValueAsString(inc, 73);
                    objReport.DAT_ENT_FNC_EXTO = sl.GetCellValueAsString(inc, 74);
                    objReport.DAT_COD_ENT_INVB = sl.GetCellValueAsString(inc, 75);
                    objReport.DAT_COD_TIP_CTAB = sl.GetCellValueAsString(inc, 76);
                    objReport.DAT_COD_CTAB = sl.GetCellValueAsString(inc, 77);
                    objReport.DAT_ENT_FNC_EXTB = sl.GetCellValueAsString(inc, 78);
                    objReport.DAT_ALCANCE = sl.GetCellValueAsString(inc, 79);
                    objReport.DAT_COD_PAISO = sl.GetCellValueAsString(inc, 80);
                    objReport.DAT_COD_PAISD = sl.GetCellValueAsString(inc, 81);
                    objReport.DAT_INTOPE = sl.GetCellValueAsString(inc, 82);
                    objReport.DAT_FORMA = sl.GetCellValueAsString(inc, 83);
                    objReport.DAT_INFORM = sl.GetCellValueAsString(inc, 84);
                    Console.WriteLine("EL objReport.FILA : " + objReport.FILA);
                    Console.WriteLine("EL objReport.OPERACION : " + objReport.OPERACION);
                    arrObjsSbs.Add(objReport);
                    //int edad = sl.GetCellValueAsInt32(iRow, 3);

                    iRow++;
                }
                Dictionary<string, dynamic> respuesta = this.processInsertText(arrObjsSbs, txtUnica, txtMultiple);
                objRespuesta["code"] = 0;
                objRespuesta["message"] = "";
                if (respuesta["cadenaTxtUnica"].ToString().Length > 0)
                    System.IO.File.WriteAllText(@"C:\reportes\respuestas\pruebaHoyReporte.998", respuesta["cadenaTxtUnica"]);
                else
                    System.IO.File.WriteAllText(@"C:\reportes\respuestas\pruebaHoyReporte.998", respuesta["cadenaTxtMultiple"]);
                return respuesta;//objRespuesta;
            }
            catch (Exception ex)
            {
                Console.WriteLine("El error en el service de insertar pagos manuales : " + ex);
                objRespuesta["code"] = 2;
                objRespuesta["messageError"] = ex.Message.ToString();
                objRespuesta["messageErrorDetalle"] = ex;
                return objRespuesta;
            }
        }
        internal Dictionary<string, dynamic> getPromedioTipoCambio(DateTime fechaActual)
        {
            double prom = 0;
            string sfinicio = "";
            string sffin = "";
            this.getRangeDateForMonth(fechaActual, out sfinicio, out sffin);
            Dictionary<string, dynamic> retorno = new Dictionary<string, dynamic>();
            List<double> pVentas = new List<double>();
            var web = new HtmlWeb();
            try
            {
                var doc = web.Load($"https://www.sbs.gob.pe/app/stats/tc-cv-historico.asp?FECHA_CONSULTA_1={sfinicio.Replace("/", "%2F")}&button22=Consultar&FECHA_CONSULTA_2={sffin.Replace("/", "%2F")}&s_moneda=02");
                doc.DocumentNode.Descendants()
                                .Where(n => n.Name == "script")
                                .ToList()
                                .ForEach(n => n.Remove());
                var node = doc.DocumentNode.SelectSingleNode($"//*[@class='APLI_tabla']");
                var nodes = node.SelectNodes("tr");
                for (int i = 0; i < nodes.Count; i++)
                {
                    var tdend = nodes[i].ChildNodes[5];
                    if (i > 1)
                    {
                        string valor = tdend.FirstChild.InnerText.Replace("\r", "").Replace("\n", "").Trim();
                        pVentas.Add(double.Parse(valor));
                    }
                }
                prom = pVentas.Average();
                retorno["promedio"] = prom.ToString(".00");
            }
            catch (Exception ex)
            {
                retorno["mesaje"] = ex.Message;
                return retorno;
            }
            return retorno;
        }
        
        private void getRangeDateForMonth(string date, out string dateIni, out string dateFin)
        {
            DateTime _date;
            DateTime _dateIni;
            DateTime _dateFin;
            bool isDate = DateTime.TryParse(date.ToString(), out _date);
            if (isDate)
            {
                _dateIni = _date.AddDays(-(_date.Day - 1));
                _dateFin = _date.AddMonths(1).AddDays(-_date.Day);
                dateIni = _dateIni.ToString("dd/MM/yyyy");
                dateFin = _dateFin.ToString("dd/MM/yyyy");
            }
            else
            {
                throw new Exception("Ocurrio un error al convertir las fechas, contactar al soporte tecnico.");
            }
        }
        private void getRangeDateForMonth(DateTime date, out string dateIni, out string dateFin)
        {
            DateTime _dateIni;
            DateTime _dateFin;
            _dateIni = date.AddDays(-(date.Day - 1));
            _dateFin = date.AddMonths(1).AddDays(-date.Day);
            dateIni = _dateIni.ToString("dd/MM/yyyy");
            dateFin = _dateFin.ToString("dd/MM/yyyy");
        }
    }
}

