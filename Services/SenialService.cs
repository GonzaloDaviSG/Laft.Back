using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;
// using Microsoft.Office.Interop.Word;
//using Mustache;
using protecta.laft.api.DTO;
using protecta.laft.api.Models;
using protecta.laft.api.Repository;
// using WordToPDF;
using Microsoft.CodeAnalysis;
using System.Net;
using System.Collections.Specialized;

using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Data;
using ExcelDataReader;
using SpreadsheetLight;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;
using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace protecta.laft.api.Services
{
    public class SenialService : Interfaces.ISenialService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(SenialService));
        SenialRepository repository;
        private EmailService emailService;
        string Ruta = "C:/archivos";
        private readonly Utils.apiWC _configuration;
        public List<int> ProveedorIds = null;
        public SenialService()
        {
            this.repository = new SenialRepository();
            this.emailService = new EmailService();
            ProveedorIds = new List<int> { 1, 3 };
        }
        private SenialService(Utils.apiWC config)
        {
            _configuration = config;
        }
        public List<SenialDTO> GetAll()
        {
            try
            {
                return Utils.Parse.dtos(this.repository.GetAll());
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<SenialDTO>();
            }
        }
        public SenialDTO Get(int id)
        {
            try
            {
                return Utils.Parse.dto(this.repository.Get(id));
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new SenialDTO();
            }
        }

        public List<SenialDTO> SaveSenial(List<SenialDTO> dtos)
        {
            try
            {
                List<SenialDTO> config = new List<SenialDTO>();

                foreach (var dto in dtos)
                {
                    Models.Senial Senial = Utils.Parse.models(dto);
                    this.repository.Update(Senial);
                }
                return config;
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<SenialDTO>();
            }

        }

        internal object getClientsforRegimen(dynamic param)
        {
            return this.repository.getClientsforRegimen(param);
        }

        public List<Dictionary<string, dynamic>> GetOCEmail()
        {
            return this.repository.GetOCEmail();
        }

        public List<Dictionary<string, dynamic>> GetListaCargo()
        {
            return this.repository.GetListaCargo();
        }

        public List<Dictionary<string, dynamic>> GetListaAlertaComplemento()
        {
            return this.repository.GetListaAlertaComplemento();
        }

        public List<Dictionary<string, dynamic>> ListaUsariosComp()
        {
            return this.repository.ListaUsariosComp();
        }

        public List<Dictionary<string, dynamic>> GetListaResultado(dynamic param)
        {
            return this.repository.GetListaResultado(param);
        }


        public List<Dictionary<string, dynamic>> GetQuestionDetail(MonitoreoSenalesParamsDTO param)
        {
            List<Dictionary<string, dynamic>> lista = this.repository.GetDetailQuestions(param);
            /*Dictionary<string, dynamic> preguntas = new Dictionary<string, dynamic> ();
            lista.ForEach (it => {
                if (!preguntas.ContainsKey (it["SPREGUNTA"])) {
                    preguntas[it["SPREGUNTA"]] = null;
                }
                var sub = preguntas[it["SPREGUNTA"]];
                if (sub == null) {
                    sub = new List<Dictionary<string, dynamic>> ();
                    preguntas[it["SPREGUNTA"]] = sub;
                }
                sub.Add (it);

            });
            QuestionDetailDTO item = new QuestionDetailDTO ();
            item.preguntas = preguntas;*/
            return lista;//item;
        }

        internal object GetSearchClientsPepSeacsa(dynamic param)
        {
            return this.repository.GetSearchClientsPepSeacsa(param);
        }

        internal object GetSearchClientsPep(dynamic param)
        {
            return this.repository.GetSearchClientsPep(param);
        }

        public List<Dictionary<string, dynamic>> GetMovementHistory(dynamic param)
        {
            return this.repository.GetMovementHistory(param);
        }

        public List<Dictionary<string, dynamic>> GetListConifgCorreoDefault()
        {
            return this.repository.GetListConifgCorreoDefault();
        }

        public List<Dictionary<string, dynamic>> GetProfileList()
        {
            return this.repository.GetProfileList();
        }

        public List<Dictionary<string, dynamic>> GetListAction()
        {
            return this.repository.GetListAction();
        }
        public List<Dictionary<string, dynamic>> GetListPerfiles()
        {
            return this.repository.GetListPerfiles();
        }


        public List<Dictionary<string, dynamic>> GetGrupoSenal()
        {
            return this.repository.GetGrupoSenal();
        }

        public List<Dictionary<string, dynamic>> GetListCorreo()
        {
            return this.repository.GetListCorreo();
        }

        public List<Dictionary<string, dynamic>> GetListaPerfiles()
        {
            return this.repository.GetListaPerfiles();
        }

        public List<Dictionary<string, dynamic>> GetPolicyList(dynamic param)
        {
            return this.repository.GetPolicyList(param);
        }

        public List<Dictionary<string, dynamic>> GetQuestionHeader(MonitoreoSenalesParamsDTO param)
        {
            return this.repository.GetHeaderQuestions(param);
        }

        public Dictionary<string, dynamic> InsertQuestionDetail(QuestionDetailDTO param)
        {
            return this.repository.InsertQuestionDetail(param);
        }


        public Dictionary<string, dynamic> InsertQuestionHeader(MonitoreoSenalesParamsDTO param)
        {
            return this.repository.InsertQuestionHeader(param);
        }

        public Dictionary<string, dynamic> GetAlertFormList(AlertFormParamDTO param)
        {
            List<Dictionary<string, dynamic>> lista = this.repository.GetAlertFormList(param);
            Dictionary<string, dynamic> grupoAlertas = new Dictionary<string, dynamic>();
            lista.ForEach(it =>
            {
                if (!grupoAlertas.ContainsKey(it["NIDAGRUPA"] + "|" + it["NIDREGIMEN"]))
                {
                    grupoAlertas[it["NIDAGRUPA"] + "|" + it["NIDREGIMEN"]] = null;
                }
                var sub = grupoAlertas[it["NIDAGRUPA"] + "|" + it["NIDREGIMEN"]];
                if (sub == null)
                {
                    sub = new List<Dictionary<string, dynamic>>();
                    grupoAlertas[it["NIDAGRUPA"] + "|" + it["NIDREGIMEN"]] = sub;
                }
                sub.Add(it);
            });
            return grupoAlertas;
        }

        public List<Dictionary<string, dynamic>> GetOfficialAlertFormList(OfficialAlertFormParamDTO param)
        {
            return this.repository.GetOfficialAlertFormList(param);
        }

        public List<Dictionary<string, dynamic>> GetListNcCompanies(NcCompaniesParamDTO param)
        {
            return this.repository.GetListNcCompanies(param);
        }

        public Dictionary<string, dynamic> UpdateListNcCompanies(UpdateNcCompaniesParamDTO param)
        {
            return this.repository.UpdateListNcCompanies(param);
        }

        public Dictionary<string, dynamic> UpdateStatusAlert(dynamic param)
        {
            return this.repository.UpdateStatusAlert(param);

        }
        public Dictionary<string, dynamic> GetAnulacionAlerta(dynamic param)
        {
            return this.repository.GetAnulacionAlerta(param);

        }


        public Dictionary<string, dynamic> DeleteAdjuntosInformAlerta(dynamic param)
        {
            return this.repository.DeleteAdjuntosInformAlerta(param);

        }

        public Dictionary<string, dynamic> getDeleteAdjuntos(dynamic param)
        {
            return this.repository.getDeleteAdjuntos(param);

        }

        public Dictionary<dynamic, dynamic> GetRegistrarDatosExcelGC(dynamic param)
        {
            return this.repository.GetRegistrarDatosExcelGC(param);

        }
        public RegistroNegativoModel preCargaRegistrosNegativos(dynamic param)
        {
            string filePath = this.Ruta + "/" + param.RutaExcel;
            SLDocument sl = new SLDocument(filePath);
            var worksheetStats = sl.GetWorksheetStatistics();
            int valor = 2;
            int validaraCabecera = 1;
            int fila = 1;
            int cantidad = 0;
            RegistroNegativoModel item = new RegistroNegativoModel();
            try
            {
                if (sl.GetCellValueAsString(validaraCabecera, 1).ToUpper() != "N")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera N en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 2).ToUpper() != "TIPO_DE_PERSONA")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera TIPO_DE_PERSONA en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 3).ToUpper() != "PAIS_NACIONALIDAD")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera PAIS_NACIONALIDAD en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 4).ToUpper() != "TIPO_DOCUMENTO")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera TIPO_DOCUMENTO en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 5).ToUpper() != "N_ID")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera N_ID en la fila " + fila;
                    return item;
                }

                if (sl.GetCellValueAsString(validaraCabecera, 6).ToUpper() != "APELLIDO_PATERNO")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera APELLIDO_PATERNO en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 7).ToUpper() != "APELLIDO_MATERNO")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera APELLIDO_MATERNO en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 8).ToUpper() != "NOMBRES_RAZON_SOCIAL")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera NOMBRES_RAZON_SOCIAL en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 9).ToUpper() != "SEÑAL_LAFT")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera SEÑAL_LAFT en la fila " + fila;
                    return item;

                }
                if (sl.GetCellValueAsString(validaraCabecera, 10).ToUpper() != "FILTRO")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera FILTRO en la fila " + fila;
                    return item;
                }

                if (sl.GetCellValueAsString(validaraCabecera, 11).ToUpper() != "FECHA_DESCUBRIMIENTO")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera FECHA_DESCUBRIMIENTO en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 12).ToUpper() != "DOCUMENTO_REFERENCIA")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera DOCUMENTO_REFERENCIA en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 13).ToUpper() != "TIPO_LISTA")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera TIPO_LISTA en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 14).ToUpper() != "NRO_DOC")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera NRO_DOC en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 15).ToUpper() != "NOMBRE_COMPLETO")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera NOMBRE_COMPLETO en la fila " + fila;
                    return item;
                }
                if (item.codigo != 2)
                {
                    if (string.IsNullOrEmpty(sl.GetCellValueAsString(2, 1)))
                    {
                        item.codigo = 2;
                        item.fila = fila;
                        item.mensaje = "Hay errores en la fila 2";
                        item.cantidad = 0;
                        return item;
                    }

                    this.repository.getDeleteRegistrosNegativos();
                    int indexs = worksheetStats.EndRowIndex;
                    ItemRegistroNegativo _item = new ItemRegistroNegativo();
                    _item.numero = new string[indexs];
                    _item.tipoPersona = new string[indexs];
                    _item.pais = new string[indexs];
                    _item.tipoDocumento = new string[indexs];
                    _item.numeroDocumento = new string[indexs];
                    _item.apellidoParteno = new string[indexs];
                    _item.apellidoMaterno = new string[indexs];
                    _item.nombre = new string[indexs];
                    _item.senalLaft = new string[indexs];
                    _item.filtro = new string[indexs];
                    _item.fechaNacimiento = new string[indexs];
                    _item.documentoReferencia = new string[indexs];
                    _item.tipoLista = new string[indexs];
                    _item.numeroDocumento2 = new string[indexs];
                    _item.nombreCompleto = new string[indexs];
                    int arraposition = 0;
                    while (!string.IsNullOrEmpty(sl.GetCellValueAsString(valor, 1)))
                    {
                        item.items = new ItemRegistroNegativo();
                        _item.numero[arraposition] = sl.GetCellValueAsString(valor, 1).Trim();
                        _item.tipoPersona[arraposition] = sl.GetCellValueAsString(valor, 2).Trim();
                        _item.pais[arraposition] = sl.GetCellValueAsString(valor, 3).Trim();
                        _item.tipoDocumento[arraposition] = sl.GetCellValueAsString(valor, 4).Trim();
                        _item.numeroDocumento[arraposition] = sl.GetCellValueAsString(valor, 5).ToUpper().Trim();
                        _item.apellidoParteno[arraposition] = sl.GetCellValueAsString(valor, 6).Trim();
                        _item.apellidoMaterno[arraposition] = sl.GetCellValueAsString(valor, 7).Trim();
                        _item.nombre[arraposition] = sl.GetCellValueAsString(valor, 8).Trim();
                        _item.senalLaft[arraposition] = sl.GetCellValueAsString(valor, 9).Trim();
                        _item.filtro[arraposition] = sl.GetCellValueAsString(valor, 10).Trim();
                        _item.fechaNacimiento[arraposition] = sl.GetCellValueAsString(valor, 11).Trim();
                        _item.documentoReferencia[arraposition] = sl.GetCellValueAsString(valor, 12).Trim();
                        _item.tipoLista[arraposition] = sl.GetCellValueAsString(valor, 13).Trim();
                        _item.numeroDocumento2[arraposition] = sl.GetCellValueAsString(valor, 14).Trim();
                        _item.nombreCompleto[arraposition] = sl.GetCellValueAsString(valor, 15).Trim();
                        arraposition++;
                        valor++;
                        //cantidad = valor;
                    }

                    item.items = _item;
                    item.cantidad = valor;
                    item = this.repository.GetRegistrarDatosExcelRegistronegativo(item);
                }
            }
            catch (Exception ex)
            {
                item.mensaje = ex.Message.ToString();
                item.codigo = 2;
            }
            finally
            {
                //item.items = null;
                //sl = null;
            }
            return item;
        }
        public RegistroNegativoModel GetRegistrarDatosExcelRegistronegativo(dynamic param)
        {
            return this.preCargaRegistrosNegativos(param);

        }


        public Dictionary<dynamic, dynamic> GetInsertaHistorialUsuario(dynamic param)
        {
            return this.repository.GetInsertaHistorialUsuario(param);

        }

        public Dictionary<dynamic, dynamic> UpdInformes(dynamic param)
        {
            return this.repository.UpdInformes(param);

        }

        public Dictionary<dynamic, dynamic> UpdActualizarCorreoOC(dynamic param)
        {
            return this.repository.UpdActualizarCorreoOC(param);

        }



        public Dictionary<dynamic, dynamic> getCorreo_OC()
        {
            return this.repository.getCorreo_OC();

        }

        public Dictionary<dynamic, dynamic> getObtenerContrasennaCorreo()
        {
            return this.repository.getObtenerContrasennaCorreo();

        }




        public Dictionary<dynamic, dynamic> DelEliminarDemanda(dynamic param)
        {
            return this.repository.DelEliminarDemanda(param);

        }

        public Dictionary<dynamic, dynamic> UpdRutaComplementos(dynamic param)
        {
            return this.repository.UpdRutaComplementos(param);

        }

        public Dictionary<dynamic, dynamic> getCuerpoCorreo(dynamic param)
        {
            return this.repository.getCuerpoCorreo(param);

        }

        public Dictionary<dynamic, dynamic> DesenciptarPassUsuario(dynamic param)
        {
            return this.repository.DesenciptarPassUsuario(param);

        }

        public Dictionary<string, dynamic> getDeleteRegistrosNegativos()
        {
            return this.repository.getDeleteRegistrosNegativos();

        }






        public Dictionary<string, dynamic> UpdateRevisedState(RevisedParamDTO param)
        {
            return this.repository.UpdateRevisedState(param);
        }

        public List<Dictionary<string, dynamic>> GetCommentsHeader(CommentsHeaderParamDTO param)
        {
            return this.repository.GetCommentsHeader(param);
        }

        public Dictionary<string, dynamic> InsertCommentHeader(CommentsHeaderParamDTO param)
        {
            return this.repository.InsertCommentHeader(param);
        }

        public void UploadFiles(dynamic param)
        {
            int i = 0;
            string ruta = this.Ruta + "/" + param.nIdCabUsuario + "/";
            ruta = Environment.ExpandEnvironmentVariables(ruta);
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }
            foreach (string it in param.listFiles)
            {
                byte[] data = System.Convert.FromBase64String(it.Substring(it.IndexOf(",") + 1));
                File.WriteAllBytes(ruta + param.listFileName[i], data);
                i++;
            }
        }

        public Dictionary<string, string> DownloadFile(dynamic param)
        {
            string ruta = this.Ruta + "/" + param.nIdCabUsuario + "/" + param.file;
            ruta = Environment.ExpandEnvironmentVariables(ruta);
            byte[] data = File.ReadAllBytes(ruta);
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["base64"] = Convert.ToBase64String(data, 0, data.Length);
            return output;
        }

        public Dictionary<string, dynamic> InsertAttachedFiles(dynamic param)
        {
            return this.repository.InsertAttachedFiles(param);
        }

        public List<Dictionary<string, dynamic>> GetAttachedFiles(dynamic param)
        {
            return this.repository.GetAttachedFiles(param);
        }

        public List<Dictionary<string, dynamic>> GetWorkModuleList(dynamic param)
        {
            return this.repository.GetWorkModuleList(param);
        }

        public List<Dictionary<string, dynamic>> GetWorkModuleDetail(dynamic param)
        {
            return this.repository.GetWorkModuleDetail(param);
        }

        public Dictionary<string, dynamic> InsertCompanyDetailUser(CompanyDetailUserDTO param)
        {
            return this.repository.InsertCompanyDetailUser(param);
        }

        public List<Dictionary<string, dynamic>> GetProductsCompany()
        {
            return this.repository.GetProductsCompany();
        }

        public Dictionary<string, dynamic> SendComplimentary(dynamic param)
        {
            return this.repository.SendComplimentary(param);
        }

        public List<Dictionary<string, dynamic>> GetGafiList()
        {
            return this.repository.GetGafiList();
        }

        public List<Dictionary<string, dynamic>> GetListaInformes(dynamic param)
        {
            return this.repository.GetListaInformes(param);
        }

        public List<Dictionary<string, dynamic>> GetSignalList(dynamic param)
        {
            return this.repository.GetSignalList(param);
        }
        public List<Dictionary<string, dynamic>> GetListaResultadoProveedorContraparte(dynamic param)
        {
            return this.repository.GetListaResultadoProveedorContraparte(param);
        }

        public List<Dictionary<string, dynamic>> GetListaResultadoEstadosCorreos(dynamic param)
        {
            return this.repository.GetListaResultadoEstadosCorreos(param);
        }

        public Dictionary<string, dynamic> UpdateStatusToReviewed(dynamic param)
        {
            return this.repository.UpdateStatusToReviewed(param);
        }


        public Dictionary<string, dynamic> UpdateUnchecked(dynamic param)
        {
            return this.repository.UpdateUnchecked(param);
        }

        public List<Dictionary<string, dynamic>> GetAddressList(dynamic param)
        {
            return this.repository.GetAddressList(param);
        }

        public List<Dictionary<string, dynamic>> GetRegimeList()
        {
            return this.repository.GetRegimeList();
        }
        public Dictionary<string, dynamic> GetCurrentPeriod()
        {
            return this.repository.GetCurrentPeriod();
        }

        public List<Dictionary<string, dynamic>> GetAlertReportList(dynamic param)
        {
            return this.repository.GetAlertReportList(param);
        }

        public List<AlertGafiResponseDTO> getGafiByParams(AlertGafiDTO dtos)
        {
            return this.repository.getGafiByParams(dtos);
        }
        public List<AlertNotaCreditoResponseDTO> getNCByParams(AlertNCDTORequest dtos)
        {
            return this.repository.getNCByParams(dtos);
        }

        public Object Consulta360(dynamic data)
        {
            var output = new Dictionary<string, string>();
            string url = "http://10.10.1.58/apipoliza/api/poliza/Search/GetPolicyDetails";

            var request = (HttpWebRequest)WebRequest.Create(url);

            //string json = $"{{\"Ramo\":\"{data.Ramo}\",\"Producto\":\"{data.Producto}\",\"Poliza\":\"{data.Poliza}\",\"Certificado\":\"{data.Certificado}\",\"FechaConsulta\":\"{data.FechaConsulta}\",\"Endoso\":\"{data.Endoso}\",}}";

            var obj = new
            {
                Ramo = data.Ramo,
                Producto = data.Producto,
                Poliza = data.Poliza,
                Certificado = data.Certificado,
                FechaConsulta = data.FechaConsulta,
                Endoso = data.Endoso
            };

            var jsonObj = JsonConvert.SerializeObject(obj);

            Console.WriteLine(jsonObj);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(jsonObj);
                streamWriter.Flush();
                streamWriter.Close();
            }
            try
            {

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null)
                        {
                            //var output = new Dictionary<string, string>();
                            //  return output;
                        }
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            var Resultado = JsonConvert.DeserializeObject(responseBody);
                            //output = Resultado
                            // Do something with responseBody
                            Console.WriteLine(Resultado);
                            output["Resultado"] = responseBody;
                            return Resultado;

                        }
                    }
                }
            }
            catch (WebException ex)
            {

                // Handle error
            }

            //var output = new Dictionary<string, string>();
            output["base64"] = "error";
            return output;
        }

        public Object Consulta360Previous(dynamic data)
        {
            var output = new Dictionary<string, string>();
            string url = "http://10.10.1.56/apipoliza/api/poliza/Client/GetCertificates";

            var request = (HttpWebRequest)WebRequest.Create(url);

            //string json = $"{{\"Ramo\":\"{data.Ramo}\",\"Producto\":\"{data.Producto}\",\"Poliza\":\"{data.Poliza}\",\"Certificado\":\"{data.Certificado}\",\"FechaConsulta\":\"{data.FechaConsulta}\",\"Endoso\":\"{data.Endoso}\",}}";

            var obj = new
            {
                //TipoDocumento = "2",
                //NumeroDocumento= "26697856",
                //Nombres = null,
                //Poliza= null,
                //CodAplicacion= "360",
                //Producto= null,
                //FechaSolicitud=null,
                //Rol=null,
                //Tipo=null,
                //estado= null,
                //Ramo = null,
                //pagina =1,
                //NumeroResgistros ="10000000",
                //Endoso = null,
                //Usuario = "1"

                TipoDocumento = data.TipoDocumento,
                NumeroDocumento = data.NumeroDocumento,
                //Poliza = data.Poliza,
                CodAplicacion = data.CodAplicacion,
                //Producto = data.Producto,
                //FechaSolicitud = data.FechaSolicitud,
                //Rol = data.Rol,
                //Tipo = data.Tipo,
                //estado = data.estado,
                //Ramo = data.Ramo,
                pagina = data.pagina,
                NumeroResgistros = data.NumeroResgistros,
                //Endoso = data.Endoso,
                Usuario = data.Usuario
            };

            var jsonObj = JsonConvert.SerializeObject(obj);

            Console.WriteLine(jsonObj);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(jsonObj);
                streamWriter.Flush();
                streamWriter.Close();
            }
            try
            {

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null)
                        {
                            //var output = new Dictionary<string, string>();
                            //  return output;
                        }
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            var ResultadoPrevious = JsonConvert.DeserializeObject(responseBody);
                            //output = Resultado
                            // Do something with responseBody
                            Console.WriteLine(ResultadoPrevious);
                            output["ResultadoPrevious"] = responseBody;
                            return ResultadoPrevious;

                        }
                    }
                }
            }
            catch (WebException ex)
            {

                // Handle error
            }

            //var output = new Dictionary<string, string>();
            output["base64"] = "error";
            return output;
        }

        public Dictionary<string, string> EnvioCorreoConfirmacion(dynamic param)
        {
            var objRespuesta = new Dictionary<string, string>();
            try
            {
                this.emailService.ConfirmacionBandeja(param);
                objRespuesta["code"] = "1";
                objRespuesta["mensaje"] = "Se envio correctamente";
            }
            catch (Exception ex)
            {

                objRespuesta["code"] = "2";
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
            }



            return objRespuesta;
        }

        public Dictionary<string, string> EnvioCorreoActualizacionPass(dynamic param)
        {
            var objRespuesta = new Dictionary<string, string>();
            try
            {
                this.emailService.RecuperarContrasennaUsuario(param);
                objRespuesta["code"] = "1";
                objRespuesta["mensaje"] = "Se envio correctamente";
            }
            catch (Exception ex)
            {

                objRespuesta["code"] = "2";
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
            }



            return objRespuesta;
        }

        internal List<Dictionary<string, dynamic>> getListProveedor()
        {
            return repository.getListProveedor();
        }



        //public Dictionary<string, string> FillReport(dynamic param)
        //{
        //    Console.WriteLine("NIDALERTA: {0}, NPERIODO_PROCESO: {1}, NIDUSUARIO_ASIGNADO: {2}", param.NIDALERTA, param.NPERIODO_PROCESO, param.NIDUSUARIO_ASIGNADO);
        //    var id = Guid.NewGuid();
        //    string ruta = string.Format("C:/plantillasLaft/{0}/{1}/", param.NIDALERTA, id);
        //    Environment.ExpandEnvironmentVariables(ruta);
        //    if (!Directory.Exists(ruta))
        //    {
        //        Directory.CreateDirectory(ruta);
        //    }
        //    var rutaTemplate = "";
        //    var rutaUsuario = "";
        //    int nidalerta = Convert.ToInt32(param.NIDALERTA);
        //    if ((nidalerta > 21 && nidalerta < 30) || nidalerta == 35)
        //    {
        //        rutaUsuario = string.Format("{0}/{1}.docx", ruta, "Trabajadores");
        //        rutaTemplate = string.Format("{0}/{1}/{2}.docx", "C:/plantillasLaft", param.NIDALERTA, "Trabajadores");
        //    }
        //    else if (nidalerta > 29 && nidalerta < 35)
        //    {
        //        rutaUsuario = string.Format("{0}/{1}.docx", ruta, "Proveedores");
        //        rutaTemplate = string.Format("{0}/{1}/{2}.docx", "C:/plantillasLaft", param.NIDALERTA, "Proveedores");
        //    }
        //    else
        //    {
        //        rutaUsuario = string.Format("{0}/{1}.docx", ruta, param.SNOMBRE_ALERTA);
        //        rutaTemplate = string.Format("{0}/{1}/{2}.docx", "C:/plantillasLaft", param.NIDALERTA, param.SNOMBRE_ALERTA);
        //    }

        //    var rutaExtract = string.Format("{0}/{1}", ruta, "ex");
        //    File.Copy(rutaTemplate, rutaUsuario);
        //    Directory.CreateDirectory(rutaExtract);
        //    ZipFile.ExtractToDirectory(rutaUsuario, rutaExtract);
        //    var docExtract = string.Format("{0}/{1}", rutaExtract, "word/document.xml");
        //    var texto = File.ReadAllText(docExtract);

        //    Console.WriteLine("param.NIDALERTA: " + param.NIDALERTA);
        //    Console.WriteLine("param.SNOMBRE_ALERTA: " + param.SNOMBRE_ALERTA);
        //    Console.WriteLine("param.NIDUSUARIO_ASIGNADO: " + param.NIDUSUARIO_ASIGNADO);
        //    Console.WriteLine("param.NPERIODO_PROCESO: " + param.NPERIODO_PROCESO);
        //    Console.WriteLine("param.NIDREGIMEN: " + param.NIDREGIMEN);


        //    List<Dictionary<string, dynamic>> lista = this.repository.GetAlertReportList(param);
        //    Console.WriteLine("lista: {0}", lista.Count);
        //    lista.ForEach(it => Console.WriteLine("it: {0} param: {1} et: {2} rpt: {3}", it["NIDUSUARIO_ASIGNADO"], param.NIDUSUARIO_ASIGNADO, it["ETIQUETA"], it["SRESPUESTA"]));
        //    //List<Dictionary<string, dynamic>> listaPorUsuario = lista.Where (it => param.NIDUSUARIO_ASIGNADO.ToString ().Equals (it["NIDUSUARIO_ASIGNADO"].ToString ()) || it["SOBLIGA_USUARIO"].ToString().Equals("1")).ToList ();
        //    List<Dictionary<string, dynamic>> listaPorUsuario = lista.ToList();
        //    Console.WriteLine("listaPorUsuario : " + listaPorUsuario);
        //    Console.WriteLine("listaPorUsuario length: " + listaPorUsuario.Count);
        //    var compiler = new FormatCompiler();
        //    var generator = compiler.Compile(texto);

        //    var resultado = this.generarTemplate(listaPorUsuario, generator);

        //    File.Delete(docExtract);
        //    File.WriteAllText(docExtract, resultado);
        //    File.Delete(rutaUsuario);
        //    ZipFile.CreateFromDirectory(rutaExtract, string.Format("{0}/{1}-nuevo.docx", ruta, param.SNOMBRE_ALERTA));
        //    byte[] base64 = File.ReadAllBytes(string.Format("{0}/{1}-nuevo.docx", ruta, param.SNOMBRE_ALERTA));
        //    string base64String = Convert.ToBase64String(base64);
        //    var output = new Dictionary<string, string>();
        //    output["base64"] = base64String;
        //    return output;
        //}
        internal ResponseCoincidenciaDemanda BusquedaADemanda(DemandaRequestDTO param)
        {
            ResponseCoincidenciaDemanda respuesta = new ResponseCoincidenciaDemanda();
            ResponseCoincidenciaDemanda respuestaIdecon = new ResponseCoincidenciaDemanda();
            ResponseCoincidenciaDemanda respuestaIdeconDoc = new ResponseCoincidenciaDemanda();
            ResponseCoincidenciaDemanda respuestaWc = new ResponseCoincidenciaDemanda();
            try
            {

                var thIdecon = new Thread(() =>
                {
                    List<Proveedor> pro = param.LFUENTES.FindAll(t => t.ISCHECK && ProveedorIds.Contains(t.NIDPROVEEDOR));

                    if (pro.Count > 0)
                    {
                        int valor = 0;
                        //Si es Idecon y Registro negativos  el valor es 0
                        if (pro.Count == ProveedorIds.Count)
                            valor = 0;
                        else
                            //si no , tomar el que esta marcado 
                            valor = pro.Find(t => t.ISCHECK).NIDPROVEEDOR;

                        if ((param.P_SNUM_DOCUMENTO == null || param.P_SNUM_DOCUMENTO.Trim() == "") && !(param.P_SNOMCOMPLETO == null || param.P_SNOMCOMPLETO.Trim() == "") && param.P_TIPOBUSQUEDA == 1)
                        {
                            respuestaIdecon = this.repository.BusquedaConcidenciaXNombreDemanda(param, valor);
                        }
                        else if (!(param.P_SNUM_DOCUMENTO == null || param.P_SNUM_DOCUMENTO.Trim() == "") && (param.P_SNOMCOMPLETO == null || param.P_SNOMCOMPLETO.Trim() == "") && param.P_TIPOBUSQUEDA == 1)
                        {
                            respuestaIdeconDoc = this.repository.BusquedaConcidenciaXNumeroDocDemanda(param, valor);
                        }
                        else if ((param.P_SNUM_DOCUMENTO == null || param.P_SNUM_DOCUMENTO.Trim() == "") && (param.P_SNOMCOMPLETO == null || param.P_SNOMCOMPLETO.Trim() == "") && param.P_TIPOBUSQUEDA == 2)
                        {
                            respuestaIdecon = this.repository.BusquedaConcidenciaXNombreDemanda(param, valor);
                            respuestaIdeconDoc = this.repository.BusquedaConcidenciaXNumeroDocDemanda(param, valor);
                        }
                        else
                        {
                            respuestaIdecon = this.repository.BusquedaConcidenciaXNombreDemanda(param, valor);
                            respuestaIdeconDoc = this.repository.BusquedaConcidenciaXNumeroDocDemanda(param, valor);
                        }
                    }
                });
                //var thIdeconDoc = new Thread(() =>
                //{

                //});
                var thWc = new Thread(() =>
                {
                    if (param.LFUENTES.Exists(t => t.ISCHECK && t.NIDPROVEEDOR == 4))
                        if (!(param.P_SNOMCOMPLETO == null || param.P_SNOMCOMPLETO.ToString().Trim() == "") || param.P_TIPOBUSQUEDA.ToString() == "2")
                        {
                            respuestaWc = this.ConsultaADemandaWC(param);
                        }
                        else
                        {
                            return;
                        }
                });
                thIdecon.Start();
                thWc.Start();
                thIdecon.Join();
                thWc.Join();

                if (respuestaIdecon.code == 0 && respuestaWc.code == 0 && respuestaIdeconDoc.code == 0)/*si la busqueda fue por documento y nombre y satisfactorio 0*/
                {
                    respuesta.code = 0;
                    respuesta.mensaje = respuestaIdecon.mensaje;
                    respuesta.Items = new List<CoincidenciaDemanda>();
                    if (respuestaIdecon.Items != null)
                        respuesta.Items.AddRange(respuestaIdecon.Items);
                    if (respuestaIdeconDoc.Items != null)
                        respuesta.Items.AddRange(respuestaIdeconDoc.Items);
                    if (respuestaWc.Items != null)
                        respuesta.Items.AddRange(respuestaWc.Items);
                }
                else if (respuestaIdecon.code == 0 && respuestaIdeconDoc.code == 0)/*si la busqueda fue por nombre y la respuesta de busqueda por documento es nulo y salio satisfactorio 0*/
                {
                    respuesta.code = 0;
                    respuesta.mensaje = respuestaIdecon.mensaje;
                    respuesta.Items = new List<CoincidenciaDemanda>();
                    if (respuestaIdecon.Items != null)
                        respuesta.Items.AddRange(respuestaIdecon.Items);
                    if (respuestaIdeconDoc.Items != null)
                        respuesta.Items.AddRange(respuestaIdeconDoc.Items);
                }
                else if (respuestaWc.code == 0)/*si la busqueda fue solo por documento y satisfactorio 0*/
                {
                    respuesta.code = 0;
                    respuesta.mensaje = respuestaWc.mensaje;
                    respuesta.Items = new List<CoincidenciaDemanda>();
                    if (respuestaWc.Items != null)
                        respuesta.Items.AddRange(respuestaWc.Items);
                }
                else /*si la busqueda no fue satisfactoria en alguno de los casos 1*/
                {
                    if (respuestaIdecon.code == 1)
                    {
                        respuesta = respuestaIdecon;
                    }
                    else if (respuestaWc.code == 1)
                    {
                        respuesta = respuestaWc;
                    }
                    else if (respuestaIdeconDoc.code == 1)
                    {
                        respuesta = respuestaIdeconDoc;
                    }
                }
                //respuesta.Items = this.setListDemanda(respuesta.Items);

                string fecha = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToString("T", CultureInfo.GetCultureInfo("en-US"));
                respuesta.Items.ForEach(t =>
                {
                    t.DFECHA_BUSQUEDA = fecha;
                    t.SUSUARIO_BUSQUEDA = param.P_SNOMBREUSUARIO;
                });
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.code = 1;
                respuesta.mensaje = ex.Message;
                return respuesta;
            }
        }

        internal InformeKri getInformeKri(dynamic param)
        {
            InformeKri response = new InformeKri();
            InformeKri es10 = new InformeKri();
            InformeKri actividadEconomica = new InformeKri();
            InformeKri zonaGeografica = new InformeKri();
            es10 = repository.getInformacionEs10(param);
            if (es10.code == 1)
                return es10;
            else
            {
                response = es10;
            }
            actividadEconomica = repository.getInformacionActividadEconomica(param);
            if (actividadEconomica.code == 1)
                return actividadEconomica;
            else
            {
                response.ActividadEconomicaCuadro = new List<Dictionary<string, dynamic>>();
                response.ActividadEconomicaCuadro = actividadEconomica.ActividadEconomicaCuadro;
            }
            zonaGeografica = repository.getInformacionZonaGeografica(param);
            if (zonaGeografica.code == 1)
                return zonaGeografica;
            else
            {
                response.ZonasGeograficas = new List<Dictionary<string, dynamic>>();
                response.ZonasGeograficas = zonaGeografica.ZonasGeograficas;
                response.ZonaGeograficaCuadro = new List<Dictionary<string, dynamic>>();
                response.ZonaGeograficaCuadro = zonaGeografica.ZonaGeograficaCuadro;
            }
            return response;
        }

        public informeN1 getInformeN1(dynamic param)
        {
            return this.repository.getInformeN1(param);

        }

        //public List<CoincidenciaDemanda> setListDemanda(List<CoincidenciaDemanda> _items) {

        //    List<Dictionary<string, dynamic>> listas = new List<Dictionary<string, dynamic>>();
        //    List<Dictionary<string, string>> tipoLista = new List<Dictionary<string, string>>();
        //    listas = repository.getListasPorProveedor();
        //    List<string> nameBusq = new List<string>();
        //    List<string> numBusq = new List<string>();
        //    List<CoincidenciaDemanda> items = null;
        //    CoincidenciaDemanda item = null;
        //    List<CoincidenciaDemanda> itemsReturn = null;
        //    nameBusq = _items.Select(t=> t.SNOMBRE_BUSQUEDA).Distinct().ToList();
        //    numBusq = _items.Select(t => t.SNUMDOC_BUSQUEDA).Distinct().ToList();

        //    for (int _n = 0; _n < nameBusq.Count; _n++)
        //    {
        //        items = new List<CoincidenciaDemanda>();
        //        items = _items.FindAll(t=> t.SNOMBRE_BUSQUEDA == nameBusq[_n]);
        //        tipoLista = listas.FindAll(t => t["NIDPROVEEDOR"] == items[0].NIDPROVEEDOR);
        //        for (int i = 0; i < tipoLista.Count; i++)
        //        {
        //            item = items.Find(t => t.SLISTA == tipoLista[i]["NIDTIPOLISTA"]);
        //            if (item != null) {

        //            }

        //                itemsReturn
        //        }
        //    }


        //    return items;
        //}
        public ResponseCoincidenciaDemanda ConsultaADemandaWC(DemandaRequestDTO param)
        {
            ResponseCoincidenciaDemanda _response = new ResponseCoincidenciaDemanda();
            Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
            string apiWcUrl = Utils.Config.AppSetting["apiWC:url"];
            string url = apiWcUrl + "/api/WC1/GetDemandaSearch";
            var request = (HttpWebRequest)WebRequest.Create(url);

            var obj = new
            {
                name = param.P_SNOMCOMPLETO,
                typeDocument = param.P_NOMBRE_RAZON,
                codBusqueda = param.P_SCODBUSQUEDA,
                tipoBusqueda = param.P_TIPOBUSQUEDA,
                usuario = param.P_SNOMBREUSUARIO
            };

            var jsonObj = JsonConvert.SerializeObject(obj);
            Console.WriteLine(jsonObj);
            request.Method = "POST";
            request.Timeout = 7200000;
            request.ContentType = "application/json";
            request.Accept = "application/json";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(jsonObj);
                streamWriter.Flush();
                streamWriter.Close();
            }
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            dynamic Resultado = JsonConvert.DeserializeObject(responseBody);
                            _response = JsonConvert.DeserializeObject<ResponseCoincidenciaDemanda>(Resultado.ToString());
                            //objRespuesta["code"] = Convert.ToInt32(Resultado.nCode);
                            //objRespuesta["mensaje"] = Resultado.sMessage;
                            //objRespuesta["items"] = Resultado.items;

                            return _response;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                _response.code = 1;
                _response.mensaje = ex.Message;
                return _response;
            }
        }

        internal Dictionary<String, dynamic> BusquedaManual(dynamic param)
        {
            Dictionary<String, dynamic> respuesta = new Dictionary<string, dynamic>();
            Dictionary<String, dynamic> respuestaIdecon = new Dictionary<string, dynamic>();
            Dictionary<String, dynamic> respuestaWc = new Dictionary<string, dynamic>();
            try
            {
                var thIdecon = new Thread(() =>
                {
                    respuestaIdecon = this.repository.BusquedaConcidenciaXDocXName(param);
                });
                var thWc = new Thread(() =>
                {
                    respuestaWc = this.ConsultaWC(param);
                });
                thIdecon.Start();
                thWc.Start();
                thIdecon.Join();
                thWc.Join();


                if (respuestaIdecon["code"] == 0 || respuestaWc["code"] == 0)
                {
                    if (respuestaIdecon["code"] == 0)
                    {
                        respuesta = respuestaIdecon;
                    }
                    if (respuestaWc["code"] == 0)
                    {
                        if (respuesta.Count == 0)
                        {
                            respuesta = respuestaWc;
                        }
                        else
                        {
                            if (respuesta["mensaje"].ToString().Trim() != "Si encontro coincidencia")
                                respuesta = respuestaWc;
                        }
                    }
                    if (respuesta["mensaje"].ToString().Trim() == "Si encontro coincidencia")
                    {
                        actualizarTratamiento(param);
                    }
                }
                else
                {
                    if (respuestaIdecon["code"] == 1)
                    {
                        respuesta = respuestaIdecon;
                    }
                    else if (respuestaWc["code"] == 1)
                    {
                        respuesta = respuestaWc;
                    }
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta["code"] = 1;
                respuesta["mensaje"] = ex.Message;
                return respuesta;
            }

        }

        internal Dictionary<string, dynamic> getrucsunat(dynamic param)
        {
            double prom = 0;
            Dictionary<string, dynamic> retorno = new Dictionary<string, dynamic>();
            List<double> pVentas = new List<double>();
            var web = new HtmlWeb();
            try
            {
                var doc = web.Load($"https://e-consultaruc.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias?" + $"accion=consPorTipdoc&razSoc=&nroRuc=&nrodoc=47147299&token={param.token.ToString()}&contexto=ti-it&modo=1&search1=&rbtnTipo=2&tipdoc=1&search2=47147299&search3=&codigo=");
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
            }
            return retorno;
        }

        internal object getClientWcEstado(dynamic param)
        {
            return this.repository.getClientWcEstado(param);
        }

        internal object addWebLinkscliente(dynamic param)
        {
            return this.repository.addWebLinkscliente(param);
        }

        internal object getDeleteWebLinksCoincidence(dynamic param)
        {
            return this.repository.getDeleteWebLinksCoincidence(param);
        }

        internal List<Dictionary<string, dynamic>> getListWebLinksCliente(dynamic param)
        {
            var responseStr = "";

            List<Dictionary<string, dynamic>> items = this.repository.getListWebLinksCliente(param);
            if (param["SPROCESO"].ToString() == "1")
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i]["SSTATE"].ToString() == "")
                    {
                        this.proccessLinks(items[i]);
                    }
                }
            }
            return items;
        }
        internal List<Dictionary<string, dynamic>> getListWebLinksClienteAll(dynamic param)
        {
            var responseStr = "";

            List<Dictionary<string, dynamic>> items = this.repository.getListWebLinksCliente(param);
            if (param["SPROCESO"].ToString() == "1")
            {
                for (int i = 0; i < items.Count; i++)
                {
                    this.proccessLinks(items[i]);
                }
            }
            return items;
        }
        private void proccessLinks(Dictionary<string, dynamic> item)
        {
            var status = "";
            try
            {
                Task tarea = Task.Run(async () =>
                {
                    HttpClient httpClient = new HttpClient();
                    httpClient.Timeout = new TimeSpan(0, 0, 10);
                    string url = item["SURI"].ToString();
                    url = url.StartsWith("https") ? url.Replace("https", "http") : url;
                    log.Info("url " + url);
                    var resp = await httpClient.GetAsync(url);
                    status = resp.StatusCode.ToString();
                });
                tarea.Wait();
                if (status == "OK")
                    item["SSTATE"] = "HABILITADO";
                else
                    item["SSTATE"] = "NO HABILITADO";
                this.repository.updateWebLink(item);
            }
            catch (Exception ex)
            {
                log.Info("Message " + ex.Message);
                item["SSTATE"] = "NO HABILITADO";
                if (ex.Message.Contains("SSL"))
                    item["SSTATE"] = "FUENTE NO VERIFICADA POR RESTRICCIÓN DEL SERVIDOR";
                this.repository.updateWebLink(item);
            }
        }


        public void actualizarTratamiento(dynamic param)
        {
            List<Dictionary<string, dynamic>> proveedores = repository.GetProveedorCoincidencia(param);
            for (int i = 0; i < proveedores.Count; i++)
            {
                param.NIDPROVEEDOR = proveedores[i]["NIDPROVEEDOR"];
                repository.actualizarTratamiento(param);
            }
        }
        private string generarTemplate(List<Dictionary<string, dynamic>> listaPorUsuario, dynamic generator)
        {
            try
            {
                Console.WriteLine("generar : " + listaPorUsuario);
                dynamic valores = new ExpandoObject();
                //string perfil = null;
                foreach (var item in listaPorUsuario)
                {
                    Console.WriteLine("generar item : " + item);
                    try
                    {
                        //Console.WriteLine("el item : "+item);
                        //Console.WriteLine("el item[ETIQUETA] : "+item["ETIQUETA"]);
                        //Console.WriteLine("el item[ETIQUETA] : "+item["SRESPUESTA"]);
                        //Console.WriteLine("el item[SPERFIL] : "+item["SPERFIL"]);

                        if (item["ETIQUETA"].StartsWith("tbl"))
                        {
                            Console.WriteLine("el item[NCANTIDAD] : " + item["NCANTIDAD"]);
                            List<Tabla> listaTabla = this.fillTable(item);
                            ((IDictionary<string, object>)valores).Add(item["ETIQUETA"], listaTabla);
                        }
                        else
                        {
                            Console.WriteLine(item["ETIQUETA"]);
                            Console.WriteLine("El !item[ETIQUETA].Equals(periodoAnio) : " + !(item["ETIQUETA"].Equals("periodoAnio") || item["ETIQUETA"].Equals("periodoMes")));
                            if (!(item["ETIQUETA"].Equals("periodoAnio") || item["ETIQUETA"].Equals("periodoMes")))
                            {
                                //((IDictionary<string, object>) valores).Add ("texto", item["SCOMENTARIO"]);
                                ((IDictionary<string, object>)valores).Add("texto", "comentario");
                                ((IDictionary<string, object>)valores).Add("SPERFIL_NAME_USUARIO", item["SPERFIL"]);
                                ((IDictionary<string, object>)valores).Add("USU_NOMBRE_COMPLETO", item["SNOM_USUARIO_ASIG"]);
                                ((IDictionary<string, object>)valores).Add("rpta121", item["SRESPUESTA"]);
                                ((IDictionary<string, object>)valores).Add("rpta122", item["SRESPUESTA"]);

                            }


                          ((IDictionary<string, object>)valores).Add(item["ETIQUETA"], item["SRESPUESTA"]);
                            //perfil = item["SPERFIL"];
                        }
                    }
                    catch (Exception ex) { throw; }
                }
                Console.WriteLine("el valores : " + valores);
                string resultado = generator.Render(valores);
                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine("el error : " + ex);
                throw;
            }

        }

        internal List<Dictionary<string, dynamic>> GetListaTipo()
        {
            try
            {
                return this.repository.GetListaTipo();
            }
            catch (Exception ex)
            {
                Console.WriteLine("el error : " + ex);
                throw;
            }
        }

        private List<Tabla> fillTable(Dictionary<string, dynamic> item)
        {
            try
            {
                List<Tabla> listaTabla = new List<Tabla>();
                if (item["NCANTIDAD"] == 0)
                {
                    return listaTabla;
                }
                else
                {
                    var valores = item["SRESPUESTA"].Split("|");
                    var listaValores = new List<string>(valores);
                    while (listaValores.Any())
                    {
                        var tabla = new Tabla();
                        var tablaType = tabla.GetType();
                        for (int i = 0; i < 4; i++)
                        {
                            var primerElemento = listaValores[0];
                            var k = i + 1;
                            tablaType.GetProperty("valor" + k).SetValue(tabla, primerElemento, null);
                            listaValores.RemoveAt(0);

                        }
                        Console.WriteLine("el tale : " + tabla);
                        Console.WriteLine(tabla);
                        listaTabla.Add(tabla);
                    }

                    Console.WriteLine(listaTabla);
                    return listaTabla;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("el error en el fillTable : " + ex);
                throw;
            }
        }


        public class Tabla
        {
            public string valor1 { get; set; }
            public string valor2 { get; set; }
            public string valor3 { get; set; }
            public string valor4 { get; set; }
            public string valor5 { get; set; }
            public string valor6 { get; set; }
        }

        // public Dictionary<string, string> DownloadPDF (dynamic param) {
        //     Object oMissing = System.Reflection.Missing.Value;    
        //       Console.WriteLine ("NIDALERTA: {0}, NPERIODO_PROCESO: {1}, NIDUSUARIO_ASIGNADO: {2}", param.NIDALERTA, param.NPERIODO_PROCESO, param.NIDUSUARIO_ASIGNADO);
        //     var id = Guid.NewGuid ();
        //     string ruta = string.Format ("C:/plantillasLaft/{0}/{1}/", param.NIDALERTA, id);
        //     string pdfRoute = string.Format ("C:/plantillasLaft/pdf/{0}/{1}/", param.NIDALERTA, id);
        //     Environment.ExpandEnvironmentVariables (ruta);
        //     if (!Directory.Exists (ruta)) {
        //         Directory.CreateDirectory (ruta);
        //     }
        //     var rutaTemplate = string.Format ("{0}/{1}/{2}.docx", "C:/plantillasLaft", param.NIDALERTA, param.SNOMBRE_ALERTA);
        //     var rutaUsuario = string.Format ("{0}/{1}.docx", ruta, param.SNOMBRE_ALERTA);
        //     var rutaExtract = string.Format ("{0}/{1}", ruta, "ex");
        //     File.Copy (rutaTemplate, rutaUsuario);
        //     Directory.CreateDirectory (rutaExtract);
        //     ZipFile.ExtractToDirectory (rutaUsuario, rutaExtract);
        //     var docExtract = string.Format ("{0}/{1}", rutaExtract, "word/document.xml");
        //     var texto = File.ReadAllText (docExtract);
        //     List<Dictionary<string, dynamic>> lista = this.repository.GetAlertReportList (param);
        //     Console.WriteLine ("lista: {0}", lista.Count);
        //     lista.ForEach (it => Console.WriteLine ("it: {0} param: {1} et: {2} rpt: {3}", it["NIDUSUARIO_ASIGNADO"], param.NIDUSUARIO_ASIGNADO, it["ETIQUETA"], it["SRESPUESTA"]));
        //     List<Dictionary<string, dynamic>> listaPorUsuario = lista.Where (it => param.NIDUSUARIO_ASIGNADO.ToString ().Equals (it["NIDUSUARIO_ASIGNADO"].ToString ())).ToList ();
        //     var compiler = new FormatCompiler ();
        //     var generator = compiler.Compile (texto);
        //     dynamic valores = new ExpandoObject ();
        //     foreach (var item in listaPorUsuario) {
        //         try {
        //             Console.WriteLine ("{0} => {1}", item["ETIQUETA"], item["SRESPUESTA"]);
        //             ((IDictionary<string, object>) valores).Add (item["ETIQUETA"], item["SRESPUESTA"]);
        //         } catch (ArgumentException ex) { }
        //     }
        //     string resultado = generator.Render (valores);
        //     File.Delete (docExtract);
        //     File.WriteAllText (docExtract, resultado);
        //     File.Delete (rutaUsuario);
        //     ZipFile.CreateFromDirectory (rutaExtract, string.Format ("{0}/{1}-nuevo.docx", ruta, param.SNOMBRE_ALERTA));
        //     var newRoute = (string.Format ("{0}/{1}-nuevo.pdf", pdfRoute , param.SNOMBRE_ALERTA));

        //     // string FileExtension = Path.GetExtension (rutaTemplate);
        //     // string ChangeExtension = rutaTemplate.Replace (FileExtension, ".pdf");
        //     Application app = new Application();
        //     Document document = new Document();
        //     document = app.Documents.Open(rutaTemplate,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing,ref oMissing);      

        //     document.Activate();
        //     document.ExportAsFixedFormat(newRoute.ToString(), WdExportFormat.wdExportFormatPDF,false,WdExportOptimizeFor.wdExportOptimizeForOnScreen,
        //             WdExportRange.wdExportAllDocument,1,1,WdExportItem.wdExportDocumentContent,true,true,
        //             WdExportCreateBookmarks.wdExportCreateHeadingBookmarks,true,true,false,ref oMissing);
        //     document.Close();    

        //     byte[] base64 = File.ReadAllBytes (string.Format ("{0}/{1}-nuevo.docx", ruta, param.SNOMBRE_ALERTA));
        //     string base64String = Convert.ToBase64String (base64);
        //     var output = new Dictionary<string, string> ();
        //     output["base64"] = base64String;
        //     return output;
        // }


        public List<Dictionary<string, dynamic>> GetInternationalLists(dynamic param)
        {
            return this.repository.GetInternationalLists(param);
        }

        public List<Dictionary<string, dynamic>> GetPepList(dynamic param)
        {
            return this.repository.GetPepList(param);
        }

        public List<Dictionary<string, dynamic>> GetFamiliesPepList(dynamic param)
        {
            return this.repository.GetFamiliesPepList(param);
        }

        public List<Dictionary<string, dynamic>> GetSacList(dynamic param)
        {
            return this.repository.GetSacList(param);
        }

        public List<Dictionary<string, dynamic>> GetListEspecial(dynamic param)
        {
            return this.repository.GetListEspecial(param);
        }

        public List<ClientAlertS2ResDTO> getClientsS2ByParams(ClientAlertS2ReqDTO dtos)
        {
            return this.repository.getClientsS2ByParams(dtos);
        }

        public List<ClientAlertRG4ResDTO> getClientsRG4ByParams(ClientAlertRG4ReqDTO dtos)
        {
            return this.repository.getClientsRG4ByParams(dtos);
        }
        public Dictionary<string, dynamic> getListasInternacionalesByType(dynamic param)
        {
            return this.repository.getListasInternacionalesByType(param);
        }
        public List<Dictionary<string, dynamic>> getListaInternacional(dynamic param)
        {
            return this.repository.getListaInternacional(param);
        }
        public List<Dictionary<string, dynamic>> getListaResultadosCoincid(dynamic param)
        {
            return this.repository.getListaResultadosCoincid(param);
        }



        public List<Dictionary<string, dynamic>> getResultadosCoincidencias(dynamic param)
        {
            return this.repository.getResultadosCoincidencias(param);
        }

        public Dictionary<string, dynamic> InsertAttachedFilesByAlert(dynamic param)
        {
            return this.repository.InsertAttachedFilesByAlert(param);
        }

        public Dictionary<string, dynamic> InsertAttachedFilesInformByAlert(dynamic param)
        {
            return this.repository.InsertAttachedFilesInformByAlert(param);
        }

        public List<Dictionary<string, dynamic>> GetAttachedFilesByAlert(dynamic param)
        {
            return this.repository.GetAttachedFilesByAlert(param);
        }
        public List<Dictionary<string, dynamic>> GetAttachedFilesInformByAlert(dynamic param)
        {
            return this.repository.GetAttachedFilesInformByAlert(param);
        }

        public List<Dictionary<string, dynamic>> GetAttachedFilesInformByCabecera(dynamic param)
        {
            return this.repository.GetAttachedFilesInformByCabecera(param);
        }

        public Dictionary<string, string> DownloadFileByAlert(dynamic param)
        {
            string ruta = this.Ruta + "/" + param.NIDALERTA + "/" + param.file;
            ruta = Environment.ExpandEnvironmentVariables(ruta);
            byte[] data = File.ReadAllBytes(ruta);
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["base64"] = Convert.ToBase64String(data, 0, data.Length);
            return output;
        }

        public Dictionary<string, string> DownloadUniversalFileByAlert(dynamic param)
        {
            string ruta = this.Ruta + "/" + param.ruta;
            ruta = Environment.ExpandEnvironmentVariables(ruta);
            byte[] data = File.ReadAllBytes(ruta);
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["base64"] = Convert.ToBase64String(data, 0, data.Length);
            return output;
        }

        public Dictionary<string, string> DownloadTemplate(dynamic param)
        {
            string rutaNew = "C://PlantillaEndosos/PlantillaEndosos.xlsx";
            //    C:\PlantillaEndosos
            rutaNew = Environment.ExpandEnvironmentVariables(rutaNew);
            byte[] data = File.ReadAllBytes(rutaNew);
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["base64"] = Convert.ToBase64String(data, 0, data.Length);
            return output;
        }

        public void UploadFilesByAlert(dynamic param)
        {
            int i = 0;
            string ruta = this.Ruta + "/" + param.NIDALERTA + "/";
            ruta = Environment.ExpandEnvironmentVariables(ruta);
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }
            foreach (string it in param.listFiles)
            {
                byte[] data = System.Convert.FromBase64String(it.Substring(it.IndexOf(",") + 1));
                File.WriteAllBytes(ruta + param.listFileName[i], data);
                i++;
            }
        }

        public void UploadFilesInformByAlert(dynamic param)
        {
            int i = 0;
            string ruta = this.Ruta + "/" + param.STIPO_CARGA + "/" + param.NIDALERTA + "/" + param.NPERIODO_PROCESO + "/" + param.NREGIMEN + "/";
            ruta = Environment.ExpandEnvironmentVariables(ruta);
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }
            foreach (string it in param.listFiles)
            {
                byte[] data = System.Convert.FromBase64String(it.Substring(it.IndexOf(",") + 1));
                File.WriteAllBytes(ruta + param.listFileName[i], data);
                i++;
            }
        }

        public Dictionary<string, dynamic> UploadFilesUniversalByRuta(dynamic param)
        {
            Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
            try
            {
                int i = 0;
                string ruta = this.Ruta + "/" + param.SRUTA + "/";
                ruta = Environment.ExpandEnvironmentVariables(ruta);
                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }
                foreach (string it in param.listFiles)
                {
                    byte[] data = System.Convert.FromBase64String(it.Substring(it.IndexOf(",") + 1));
                    File.WriteAllBytes(ruta + param.listFileName[i], data);
                    i++;
                }
                objRespuesta["code"] = 0;
                objRespuesta["message"] = "Exito";
            }
            catch (Exception ex)
            {
                objRespuesta["code"] = 2;
                objRespuesta["messageError"] = ex.Message.ToString();
                objRespuesta["messageErrorDetalle"] = ex;
            }
            return objRespuesta;
        }

        public List<Dictionary<string, dynamic>> GetResultadoTratamiento(dynamic param)
        {
            return this.repository.GetResultadoTratamiento(param);
        }

        public List<Dictionary<string, dynamic>> GetPerfilXGrupo(dynamic param)
        {
            return this.repository.GetPerfilXGrupo(param);
        }

        public List<Dictionary<string, dynamic>> GetListaComplementos(dynamic param)
        {
            return this.repository.GetListaComplementos(param);
        }

        public List<Dictionary<string, dynamic>> GetListaCompUsu(dynamic param)
        {
            return this.repository.GetListaCompUsu(param);
        }

        public List<Dictionary<string, dynamic>> GetListaComplementoUsuario(dynamic param)
        {
            return this.repository.GetListaComplementoUsuario(param);
        }

        public List<Dictionary<string, dynamic>> GetListaPolizas(dynamic param)
        { return this.repository.GetListaPolizas(param); }
        public List<Dictionary<string, dynamic>> GetGrupoXPerfil(dynamic param)
        {
            return this.repository.GetGrupoXPerfil(param);
        }

        public List<Dictionary<string, dynamic>> GetGrupoXSenal(dynamic param)
        {
            return this.repository.GetGrupoXSenal(param);
        }

        public List<Dictionary<string, dynamic>> GetSubGrupoSenal(dynamic param)
        {
            return this.repository.GetSubGrupoSenal(param);
        }

        public List<Dictionary<string, dynamic>> GetResultsList(dynamic param)
        {
            return this.repository.GetResultsList(param);
        }

        public List<Dictionary<string, dynamic>> GetListaResultadoGC(dynamic param)
        {
            return this.repository.GetListaResultadoGC(param);
        }



        public Dictionary<dynamic, dynamic> UpdateListClienteRefor(dynamic param)
        {
            return this.repository.UpdateListClienteRefor(param);
        }

        public Dictionary<dynamic, dynamic> AnularResultadosCliente(dynamic param)
        {
            return this.repository.AnularResultadosCliente(param);
        }

        public Dictionary<dynamic, dynamic> GetUpdateCorreos(dynamic param)
        {
            Console.WriteLine("el sersultado2: " + param);
            return this.repository.GetUpdateCorreos(param);
        }

        public Dictionary<dynamic, dynamic> BusquedaConcidenciaXDocXName(dynamic param)
        {
            return this.repository.BusquedaConcidenciaXDocXName(param);
        }

        public List<Dictionary<dynamic, dynamic>> GetResultadoTratamientoHistorico(dynamic param)
        {
            try
            {
                return this.repository.GetResultadoTratamientoHistorico(param);
            }
            catch (Exception ex)
            {
                Console.WriteLine("el ex : " + ex);

                throw ex;
            }
        }

        public List<Dictionary<dynamic, dynamic>> GetCantidadResultadoTratamientoHistorico(dynamic param)
        {
            try
            {
                return this.repository.GetCantidadResultadoTratamientoHistorico(param);
            }
            catch (Exception ex)
            {
                Console.WriteLine("el ex : " + ex);

                throw ex;
            }
        }
        public Dictionary<string, dynamic> UpdateTratamientoCliente(dynamic param)
        {
            return this.repository.UpdateTratamientoCliente(param);
        }

        public Dictionary<dynamic, dynamic> UpdateStateSenialCabUsuario(dynamic param)
        {
            try
            {
                return this.repository.GetResultadoTratamientoHistorico(param);
            }
            catch (Exception ex)
            {
                Console.WriteLine("el ex : " + ex);

                throw ex;
            }
        }

        public Dictionary<dynamic, dynamic> UpdateStateSenialCabUsuarioReal(dynamic param)
        {
            return this.repository.UpdateStateSenialCabUsuario(param);

        }
        public Dictionary<dynamic, dynamic> BusquedaConcidenciaXNombre(dynamic param)
        {
            return this.repository.BusquedaConcidenciaXNombre(param);

        }

        //public Dictionary<string, dynamic> BusquedaConcidenciaXNombreDemanda(dynamic param)
        //{
        //    return this.repository.BusquedaConcidenciaXNombreDemanda(param);

        //}

        //public Dictionary<string, dynamic> BusquedaConcidenciaXNumeroDocDemanda(dynamic param)
        //{
        //    return this.repository.BusquedaConcidenciaXNumeroDocDemanda(param);

        //}

        public Dictionary<dynamic, dynamic> BusquedaConcidenciaXDoc(dynamic param)
        {
            return this.repository.BusquedaConcidenciaXDoc(param);

        }

        public Dictionary<dynamic, dynamic> GetResultadoCoincidenciasPen(dynamic param)
        {
            return this.repository.GetResultadoCoincidenciasPen(param);

        }

        public Dictionary<dynamic, dynamic> GetHistorialEstadoCli(dynamic param)
        {
            return this.repository.GetHistorialEstadoCli(param);

        }

        public Dictionary<dynamic, dynamic> InsertUpdateProfile(dynamic param)
        {
            return this.repository.InsertUpdateProfile(param);

        }

        public Dictionary<dynamic, dynamic> InsertUpdateProfileGrupos(dynamic param)
        {
            return this.repository.InsertUpdateProfileGrupos(param);

        }

        public Dictionary<dynamic, dynamic> InsertUpdateComplemento(dynamic param)
        {
            return this.repository.InsertUpdateComplemento(param);

        }

        public Dictionary<dynamic, dynamic> ValidarPolizaVigente(dynamic param)
        {
            return this.repository.ValidarPolizaVigente(param);

        }

        public Dictionary<dynamic, dynamic> GetUpdComplementoCab(dynamic param)
        {
            return this.repository.GetUpdComplementoCab(param);

        }

        public Dictionary<dynamic, dynamic> GetInsCormularioComplUsu(dynamic param)
        {
            return this.repository.GetInsCormularioComplUsu(param);

        }

        public Dictionary<dynamic, dynamic> GetActPassUsuario(dynamic param)
        {
            return this.repository.GetActPassUsuario(param);

        }

        public List<Dictionary<string, dynamic>> getListaUsuarioCorreos(dynamic param)
        {
            return this.repository.getListaUsuarioCorreos(param);

        }

        public List<Dictionary<string, dynamic>> getListaAdjuntos(dynamic param)
        {
            return this.repository.getListaAdjuntos(param);

        }

        public Dictionary<dynamic, dynamic> GetValidarExisteCorreo(dynamic param)
        {
            return this.repository.GetValidarExisteCorreo(param);

        }
        public List<Dictionary<string, dynamic>> GetValidarHash(dynamic param)
        {
            return this.repository.GetValidarHash(param);

        }
        public List<Dictionary<string, dynamic>> GetFechaFeriado(dynamic param)
        {
            return this.repository.GetFechaFeriado(param);

        }

        public List<Dictionary<string, dynamic>> GetListaOtrosClientes(dynamic param)
        {
            return this.repository.GetListaOtrosClientes(param);

        }

        public List<Dictionary<string, dynamic>> GetListaRegistroNegativo(dynamic param)
        {
            return this.repository.GetListaRegistroNegativo(param);

        }

        public Dictionary<dynamic, dynamic> GetFechaInicioPeriodo()
        {
            return this.repository.GetFechaInicioPeriodo();

        }

        public List<Dictionary<string, dynamic>> GetListaEmpresas(dynamic param)
        {
            return this.repository.GetListaEmpresas(param);

        }



        public Dictionary<dynamic, dynamic> InsCorreoUsuario(dynamic param)
        {
            return this.repository.InsCorreoUsuario(param);

        }
        public List<Dictionary<string, dynamic>> GetAlertaResupuesta(dynamic param)
        {
            return this.repository.GetAlertaResupuesta(param);

        }

        public Dictionary<dynamic, dynamic> GetUpdPssUsuario(dynamic param)
        {
            return this.repository.GetUpdPssUsuario(param);

        }

        public Dictionary<dynamic, dynamic> GetUpdUsuarioEncriptado(dynamic param)
        {
            return this.repository.GetUpdUsuarioEncriptado(param);

        }

        public Dictionary<dynamic, dynamic> GetActualizarFechaEnvio(dynamic param)
        {
            return this.repository.GetActualizarFechaEnvio(param);

        }

        public Dictionary<dynamic, dynamic> GetActualizarContadorCorreo(dynamic param)
        {
            return this.repository.GetActualizarContadorCorreo(param);

        }



        public Dictionary<dynamic, dynamic> GetValFormularioCompl(dynamic param)
        {
            return this.repository.GetValFormularioCompl(param);

        }

        public Dictionary<dynamic, dynamic> getCorreoCustomAction(dynamic param)
        {
            return this.repository.getCorreoCustomAction(param);

        }



        public Dictionary<string, dynamic> ConsultaWC(dynamic param)
        {
            Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
            string apiWcUrl = Utils.Config.AppSetting["apiWC:url"];
            string url = apiWcUrl + "/api/WC1/GetClients";
            var request = (HttpWebRequest)WebRequest.Create(url);
            var obj = new
            {
                name = param.SNOMCOMPLETO,
                //alertId = param.NIDTIPOLISTA,
                periodId = param.NPERIODO_PROCESO,
                tipoCargaId = param.NTIPOCARGA,
                sClient = param.SCLIENT,
                nIdUsuario = param.NIDUSUARIO_MODIFICA,
                grupoSenalId = param.NIDGRUPOSENAL,
                grupoSubSenalId = param.NIDSUBGRUPOSENAL
            };
            var jsonObj = JsonConvert.SerializeObject(obj);
            Console.WriteLine(jsonObj);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(jsonObj);
                streamWriter.Flush();
                streamWriter.Close();
            }
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            dynamic Resultado = JsonConvert.DeserializeObject(responseBody);
                            objRespuesta["code"] = Convert.ToInt32(Resultado.nCode);
                            objRespuesta["mensaje"] = Resultado.sMessage;
                            return objRespuesta;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                objRespuesta["code"] = 1;
                objRespuesta["mensaje"] = ex.Message;
                return objRespuesta;
            }
        }

        public Dictionary<string, string> setDataExcelDemanda(dynamic param)
        {
            Dictionary<string, string> response = new Dictionary<string, string>();
            string filePath = this.Ruta + "/" + param.RutaExcel;
            SLDocument sl = new SLDocument(filePath);
            var data = sl.GetWorksheetStatistics();
            BusquedaDemanda item = new BusquedaDemanda();
            int rows = data.EndRowIndex - 1;
            try
            {
                item.nombreUsuario = new List<string>();
                item.codBusqueda = new List<string>();
                item.nombreCliente = new List<string>();
                item.tipoDocumento = new List<string>();
                item.numeroRuc = new List<string>();
                if (data.EndRowIndex > 0)
                {
                    for (int i = 0; i < rows; i++)
                    {
                        if (sl.GetCellValueAsString("A" + (i + 2)).Trim() == ""
                            && sl.GetCellValueAsString("B" + (i + 2)).Trim() == ""
                            && sl.GetCellValueAsString("C" + (i + 2)).Trim() == "")
                        {
                            continue;
                        }
                        item.nombreUsuario.Add(param.SNOMBREUSUARIO.ToString());
                        item.codBusqueda.Add(param.SCODBUSQUEDA.ToString());
                        item.nombreCliente.Add(sl.GetCellValueAsString("A" + (i + 2)));
                        item.tipoDocumento.Add(sl.GetCellValueAsString("B" + (i + 2)));
                        item.numeroRuc.Add(sl.GetCellValueAsString("C" + (i + 2)));
                    }
                }
                response = repository.insertMasiveDataDemanda(item);
            }
            catch (Exception ex)
            {

                throw;
            }

            return response;
        }
        public List<Dictionary<string, dynamic>> LeerDataExcel(dynamic param)
        {
            Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            List<Dictionary<string, dynamic>> listaValidacion = new List<Dictionary<string, dynamic>>();

            try
            {
                string filePath = this.Ruta + "/" + param.RutaExcel;
                SLDocument sl = new SLDocument(filePath);
                int valor = 2;
                int validaraCabecera = 1;
                int fila = 1;

                if (param.VALIDADOR == "GESTOR-CLIENTE-COLABORADOR")
                {

                    if (sl.GetCellValueAsString(validaraCabecera, 1).ToUpper() != "NTIPO_DOCUMENTO")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera NTIPO_DOCUMENTO en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 2).ToUpper() != "SNUM_DOCUMENTO")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera SNUM_DOCUMENTO en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 3).ToUpper() != "SNOM_COMPLETO")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera SNOM_COMPLETO en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 4).ToUpper() != "DFECHA_NACIMIENTO")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera DFECHA_NACIMIENTO en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 5).ToUpper() != "NACIONALIDAD")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera NACIONALIDAD en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }



                    if (listaValidacion.Count == 0)
                    {
                        while (!string.IsNullOrEmpty(sl.GetCellValueAsString(valor, 1)))
                        {
                            Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();

                            item["NTIPO_DOCUMENTO"] = sl.GetCellValueAsString(valor, 1).ToUpper().Trim();
                            item["SNUM_DOCUMENTO"] = sl.GetCellValueAsString(valor, 2).ToUpper().Trim();
                            item["SNOM_COMPLETO"] = sl.GetCellValueAsString(valor, 3).ToUpper().Trim();
                            item["DFECHA_NACIMIENTO"] = sl.GetCellValueAsDateTime(valor, 4).ToString("dd/MM/yyyy");
                            item["NACIONALIDAD"] = sl.GetCellValueAsString(valor, 5).ToUpper().Trim();

                            lista.Add(item);
                            valor++;
                        }

                        if (string.IsNullOrEmpty(sl.GetCellValueAsString(2, 1)))
                        {
                            lista.Clear();
                            objRespuesta["CODIGO"] = 2;
                            objRespuesta["FILA"] = fila;
                            objRespuesta["MENSAJE"] = "Hay errores en la fila 2";
                            lista.Add(objRespuesta);
                            return lista;
                        }



                        if (lista.Count != 0)
                        {
                            lista.ForEach(it =>
                            {
                                fila++;

                                if (it["NTIPO_DOCUMENTO"] == "")
                                {
                                    lista.Clear();
                                    objRespuesta["CODIGO"] = 2;
                                    objRespuesta["FILA"] = fila;
                                    objRespuesta["MENSAJE"] = "No tiene el campo NTIPO_DOCUMENTO en la fila " + fila;
                                    lista.Add(objRespuesta);
                                    return;

                                }
                                if (it["NTIPO_DOCUMENTO"] == "1")
                                {
                                    if (it["SNUM_DOCUMENTO"].Length != 11)
                                    {
                                        lista.Clear();
                                        objRespuesta["CODIGO"] = 2;
                                        objRespuesta["FILA"] = fila;
                                        objRespuesta["MENSAJE"] = "No tiene el campo SNUM_DOCUMENTO adecuado en la fila " + fila;
                                        lista.Add(objRespuesta);
                                        return;
                                    }


                                }

                                if (it["NTIPO_DOCUMENTO"] == "2")
                                {
                                    if (it["SNUM_DOCUMENTO"].Length != 8)
                                    {
                                        lista.Clear();
                                        objRespuesta["CODIGO"] = 2;
                                        objRespuesta["FILA"] = fila;
                                        objRespuesta["MENSAJE"] = "No tiene el campo SNUM_DOCUMENTO adecuado en la fila " + fila;
                                        lista.Add(objRespuesta);
                                        return;
                                    }


                                }
                                //if (it["NTIPO_DOCUMENTO"] == "4")
                                //{
                                //    if (it["SNUM_DOCUMENTO"].Length != 12)
                                //    {
                                //        lista.Clear();
                                //        objRespuesta["CODIGO"] = 2;
                                //        objRespuesta["FILA"] = fila;
                                //        objRespuesta["MENSAJE"] = "No tiene el campo SNUM_DOCUMENTO adecuado en la fila " + fila;
                                //        lista.Add(objRespuesta);
                                //        return;
                                //    }


                                //}
                                if (it["NTIPO_DOCUMENTO"] == "6")
                                {
                                    if (it["SNUM_DOCUMENTO"].Length != 12)
                                    {
                                        lista.Clear();
                                        objRespuesta["CODIGO"] = 2;
                                        objRespuesta["FILA"] = fila;
                                        objRespuesta["MENSAJE"] = "No tiene el campo SNUM_DOCUMENTO adecuado en la fila " + fila;
                                        lista.Add(objRespuesta);
                                        return;
                                    }


                                }
                                if (it["SNUM_DOCUMENTO"] == "")
                                {
                                    lista.Clear();
                                    objRespuesta["CODIGO"] = 2;
                                    objRespuesta["FILA"] = fila;
                                    objRespuesta["MENSAJE"] = "No tiene el campo SNUM_DOCUMENTO en la fila " + fila;
                                    lista.Add(objRespuesta);
                                    return;

                                }
                                if (it["SNOM_COMPLETO"] == "")
                                {
                                    lista.Clear();
                                    objRespuesta["CODIGO"] = 2;
                                    objRespuesta["FILA"] = fila;
                                    objRespuesta["MENSAJE"] = "No tiene el campo SNOM_COMPLETO en la fila " + fila;
                                    lista.Add(objRespuesta);
                                    return;

                                }

                                if (it["DFECHA_NACIMIENTO"] == "")
                                {
                                    lista.Clear();
                                    objRespuesta["CODIGO"] = 2;
                                    objRespuesta["FILA"] = fila;
                                    objRespuesta["MENSAJE"] = "No tiene el campo DFECHA_NACIMIENTO en la fila " + fila;
                                    lista.Add(objRespuesta);
                                    return;

                                }

                                if (it["NACIONALIDAD"] != "" && it["NACIONALIDAD"] != "1" && it["NACIONALIDAD"] != "2")
                                {
                                    lista.Clear();
                                    objRespuesta["CODIGO"] = 2;
                                    objRespuesta["FILA"] = fila;
                                    objRespuesta["MENSAJE"] = "El campo NACIONALIDAD tiene un formato incorrecto en la fila " + fila;
                                    lista.Add(objRespuesta);
                                    return;

                                }



                            });


                        }

                    }

                    //09678040


                }

                else if (param.VALIDADOR == "GESTOR-CLIENTE-PROVEEDOR-CONTRAPARTE")//GESTOR-CLIENTE-PROVEEDOR-PROVEEDOR
                {
                    //ACA SE VALIDA LAS CABECERAS
                    if (sl.GetCellValueAsString(validaraCabecera, 1).ToUpper() != "NTIPO_DOCUMENTO")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera NTIPO_DOCUMENTO en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 2).ToUpper() != "SNUM_DOCUMENTO")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera SNUM_DOCUMENTO en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 3).ToUpper() != "SNOM_COMPLETO")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera SNOM_COMPLETO en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 4).ToUpper() != "SNUM_DOCUMENTO_EMPRESA")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera SNUM_DOCUMENTO_EMPRESA en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 5).ToUpper() != "SNOM_COMPLETO_EMPRESA")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera SNOM_COMPLETO_EMPRESA en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 6).ToUpper() != "NACIONALIDAD")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera NACIONALIDAD en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 7).ToUpper() != "CARGO")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera CARGO en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }




                    if (listaValidacion.Count == 0)
                    {
                        while (!string.IsNullOrEmpty(sl.GetCellValueAsString(valor, 1)))
                        //while (true)
                        {
                            Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();

                            item["NTIPO_DOCUMENTO"] = sl.GetCellValueAsString(valor, 1).ToUpper().Trim();
                            item["SNUM_DOCUMENTO"] = sl.GetCellValueAsString(valor, 2).ToUpper().Trim();
                            item["SNOM_COMPLETO"] = sl.GetCellValueAsString(valor, 3).ToUpper().Trim();
                            item["SNUM_DOCUMENTO_EMPRESA"] = sl.GetCellValueAsString(valor, 4).ToUpper().Trim();
                            item["SNOM_COMPLETO_EMPRESA"] = sl.GetCellValueAsString(valor, 5).ToUpper().Trim();
                            item["NACIONALIDAD"] = sl.GetCellValueAsString(valor, 6).ToUpper().Trim();
                            item["CARGO"] = sl.GetCellValueAsString(valor, 7).ToUpper().Trim();

                            lista.Add(item);
                            valor++;
                            if (!string.IsNullOrEmpty(sl.GetCellValueAsString(valor, 1)))
                            {
                                //var setResult =  sl.GetCellValueAsString(valor, 1).Trim() ;
                                //var newResult =  setResult == null ? " " : setResult;
                                //DBNull.Value ? string.Empty;
                                //sl.GetCellValueAsString(valor, 1).Trim() = newResult.ToString();
                                //string.IsNullOrEmpty(sl.GetCellValueAsString(valor, 1)) = true;
                            }
                        }

                        if (string.IsNullOrEmpty(sl.GetCellValueAsString(2, 1)))
                        {
                            lista.Clear();
                            objRespuesta["CODIGO"] = 2;
                            objRespuesta["FILA"] = fila;
                            objRespuesta["MENSAJE"] = "Hay errores en la fila 2";
                            lista.Add(objRespuesta);
                            return lista;
                        }




                        //if ((lista.Count != 0 && param.idGrupo == 3 && param.idSubGrupo == 2) || (lista.Count != 0 && param.idGrupo == 4 && (param.idSubGrupo == 5 || param.idSubGrupo == 3)))
                        //{
                        //    lista.ForEach(it =>
                        //    {
                        //        fila++;
                        //        if (it["CARGO"] == "")
                        //        {
                        //            lista.Clear();
                        //            objRespuesta["CODIGO"] = 2;
                        //            objRespuesta["FILA"] = fila;
                        //            objRespuesta["MENSAJE"] = "No tiene el campo CARGO en la fila " + fila;
                        //            lista.Add(objRespuesta);
                        //            return;

                        //        }
                        //    });
                        //}

                        if (lista.Count != 0)
                        {
                            lista.ForEach(it =>
                            {
                                fila++;
                                if (it["NACIONALIDAD"] != "" && it["NACIONALIDAD"] != "1" && it["NACIONALIDAD"] != "2")
                                {
                                    lista.Clear();
                                    objRespuesta["CODIGO"] = 2;
                                    objRespuesta["FILA"] = fila;
                                    objRespuesta["MENSAJE"] = "El campo NACIONALIDAD tiene un formato incorrecto en la fila " + fila;
                                    lista.Add(objRespuesta);
                                    return;

                                }
                            });
                        }
                        //if (lista.Count != 0)
                        //{
                        //  lista.ForEach(it => {
                        //fila++;
                        //if (it["NTIPO_DOCUMENTO"] == "")
                        //{
                        //    lista.Clear();
                        //    objRespuesta["CODIGO"] = 2;
                        //    objRespuesta["FILA"] = fila;
                        //    objRespuesta["MENSAJE"] = "No tiene el campo NTIPO_DOCUMENTO en la fila " + fila;
                        //    lista.Add(objRespuesta);
                        //    return;

                        //}
                        //if (it["SNUM_DOCUMENTO"] == "")
                        //{
                        //    lista.Clear();
                        //    objRespuesta["CODIGO"] = 2;
                        //    objRespuesta["FILA"] = fila;
                        //    objRespuesta["MENSAJE"] = "No tiene el campo SNUM_DOCUMENTO en la fila " + fila;
                        //    lista.Add(objRespuesta);
                        //    return;

                        //}
                        //if (it["SNOM_COMPLETO"] == "")
                        //{
                        //    lista.Clear();
                        //    objRespuesta["CODIGO"] = 2;
                        //    objRespuesta["FILA"] = fila;
                        //    objRespuesta["MENSAJE"] = "No tiene el campo SNOM_COMPLETO en la fila " + fila;
                        //    lista.Add(objRespuesta);
                        //    return;

                        //}

                        //if (it["SNUM_DOCUMENTO_EMPRESA"] == "")
                        //{
                        //    lista.Clear();
                        //    objRespuesta["CODIGO"] = 2;
                        //    objRespuesta["FILA"] = fila;
                        //    objRespuesta["MENSAJE"] = "No tiene el campo SNUM_DOCUMENTO_EMPRESA en la fila " + fila;
                        //    lista.Add(objRespuesta);
                        //    return;

                        //}
                        //if (it["SNOM_COMPLETO_EMPRESA"] == "")
                        //{
                        //    lista.Clear();
                        //    objRespuesta["CODIGO"] = 2;
                        //    objRespuesta["FILA"] = fila;
                        //    objRespuesta["MENSAJE"] = "No tiene el campo SNOM_COMPLETO_EMPRESA en la fila " + fila;
                        //    lista.Add(objRespuesta);
                        //    return;

                        //}



                        //});


                        //}

                    }


                }

                else if (param.VALIDADOR == "REGISTO-NEGATIVO")
                {

                    if (sl.GetCellValueAsString(validaraCabecera, 1).ToUpper() != "N")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera N en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 2).ToUpper() != "TIPO_DE_PERSONA")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera TIPO_DE_PERSONA en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 3).ToUpper() != "PAIS_NACIONALIDAD")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera PAIS_NACIONALIDAD en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 4).ToUpper() != "TIPO_DOCUMENTO")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera TIPO_DOCUMENTO en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 5).ToUpper() != "N_ID")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera N_ID en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }

                    if (sl.GetCellValueAsString(validaraCabecera, 6).ToUpper() != "APELLIDO_PATERNO")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera APELLIDO_PATERNO en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 7).ToUpper() != "APELLIDO_MATERNO")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera APELLIDO_MATERNO en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 8).ToUpper() != "NOMBRES_RAZON_SOCIAL")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera NOMBRES_RAZON_SOCIAL en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 9).ToUpper() != "SEÑAL_LAFT")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera SEÑAL_LAFT en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;

                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 10).ToUpper() != "FILTRO")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera FILTRO en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }

                    if (sl.GetCellValueAsString(validaraCabecera, 11).ToUpper() != "FECHA_DESCUBRIMIENTO")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera FECHA_DESCUBRIMIENTO en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 12).ToUpper() != "DOCUMENTO_REFERENCIA")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera DOCUMENTO_REFERENCIA en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 13).ToUpper() != "TIPO_LISTA")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera TIPO_LISTA en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 14).ToUpper() != "NRO_DOC")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera NRO_DOC en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }
                    if (sl.GetCellValueAsString(validaraCabecera, 15).ToUpper() != "NOMBRE_COMPLETO")
                    {
                        listaValidacion.Clear();
                        objRespuesta["CODIGO"] = 2;
                        objRespuesta["FILA"] = fila;
                        objRespuesta["MENSAJE"] = "No tiene la cabecera NOMBRE_COMPLETO en la fila " + fila;
                        listaValidacion.Add(objRespuesta);
                        lista = listaValidacion;
                        return lista;
                    }






                    if (listaValidacion.Count == 0)
                    {
                        while (!string.IsNullOrEmpty(sl.GetCellValueAsString(valor, 1)))
                        {
                            Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();

                            item["N"] = sl.GetCellValueAsString(valor, 1).Trim();
                            item["TIPO_DE_PERSONA"] = sl.GetCellValueAsString(valor, 2).Trim();
                            item["PAIS_NACIONALIDAD"] = sl.GetCellValueAsString(valor, 3).Trim();
                            item["TIPO_DOCUMENTO"] = sl.GetCellValueAsString(valor, 4).Trim();
                            item["N_ID"] = sl.GetCellValueAsString(valor, 5).ToUpper().Trim();
                            item["APELLIDO_PATERNO"] = sl.GetCellValueAsString(valor, 6).Trim();
                            item["APELLIDO_MATERNO"] = sl.GetCellValueAsString(valor, 7).Trim();
                            item["NOMBRES_RAZON_SOCIAL"] = sl.GetCellValueAsString(valor, 8).Trim();
                            item["SEÑAL_LAFT"] = sl.GetCellValueAsString(valor, 9).Trim();
                            item["FILTRO"] = sl.GetCellValueAsString(valor, 10).Trim();
                            item["FECHA_DESCUBRIMIENTO"] = sl.GetCellValueAsString(valor, 11).Trim();
                            item["DOCUMENTO_REFERENCIA"] = sl.GetCellValueAsString(valor, 12).Trim();
                            item["TIPO_LISTA"] = sl.GetCellValueAsString(valor, 13).Trim();
                            item["NRO_DOC"] = sl.GetCellValueAsString(valor, 14).Trim();
                            item["NOMBRE_COMPLETO"] = sl.GetCellValueAsString(valor, 15).Trim();

                            lista.Add(item);
                            valor++;
                        }

                        if (string.IsNullOrEmpty(sl.GetCellValueAsString(2, 1)))
                        {
                            lista.Clear();
                            objRespuesta["CODIGO"] = 2;
                            objRespuesta["FILA"] = fila;
                            objRespuesta["MENSAJE"] = "Hay errores en la fila 2";
                            lista.Add(objRespuesta);
                            return lista;
                        }




                    }




                }


                else if (param.VALIDADOR == "DEMANDA")
                {
                    while (!string.IsNullOrEmpty(sl.GetCellValueAsString(valor, 1)))
                    {
                        Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();

                        item["SNOMBRE_COMPLETO"] = sl.GetCellValueAsString(valor, 1);
                        item["STIPO_DOCUMENTO"] = sl.GetCellValueAsString(valor, 2);
                        item["SNUM_DOCUMENTO"] = sl.GetCellValueAsString(valor, 3);


                        lista.Add(item);
                        valor++;
                    }
                }
                else
                {

                    lista.Clear();
                    objRespuesta["CODIGO"] = 2;
                    objRespuesta["FILA"] = 0;
                    objRespuesta["MENSAJE"] = "No se encontro el formato de envío";
                    lista.Add(objRespuesta);
                    return lista;

                }




            }
            catch (Exception ex)
            {
                //objRespuesta["CODIGO"] = 2;
                //objRespuesta["MENSAJE"] = ex;

                //Console.WriteLine(ex);
            }



            return lista;
        }



        public List<Dictionary<string, dynamic>> ValidarFechaHash(dynamic param)
        {

            Dictionary<string, dynamic> objRespuesta = new Dictionary<string, dynamic>();
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();
            Dictionary<dynamic, dynamic> respuestaFecha = new Dictionary<dynamic, dynamic>();

            try
            {

                respuestaFecha = this.repository.GetValidarExisteCorreo(param);
                //IDENTIFICO LA FECHA DE LA CREACION DEL HASH
                DateTime FechaHash = respuestaFecha["ID"][0].FECHA;
                objRespuesta["FechaHash"] = FechaHash.ToString("dd/MM/yyyy HH:mm:ss");
                //AUMENTO 30 MIN A LA FECHA REGISTRADA
                FechaHash = FechaHash.AddMinutes(30);
                objRespuesta["FechaHashActualizada"] = FechaHash.ToString("dd/MM/yyyy HH:mm:ss");
                //CONVIERTO EN STRING LA FECHA ACTUALIZADA DEL HASH
                string NewFechaHash = FechaHash.ToString("dd/MM/yyyy HH:mm:ss");
                //OBTENGO LA FECHA ANTUAL
                string FechaActual = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                objRespuesta["FechaActual"] = FechaActual.ToString();

                lista.Add(objRespuesta);
                return lista;

            }
            catch (Exception ex)
            {
                objRespuesta["CODIGO"] = 2;
                objRespuesta["MENSAJE"] = ex;
                lista.Add(objRespuesta);

                return lista;
                //Console.WriteLine(ex);
            }



            return lista;
        }

        public Dictionary<dynamic, dynamic> RecordarotorioCorreos()
        {
            Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
            Dictionary<dynamic, dynamic> objRespuestaPeriodo = new Dictionary<dynamic, dynamic>();
            Dictionary<dynamic, dynamic> objRespuestaCorreoCantidad = new Dictionary<dynamic, dynamic>();
            Dictionary<dynamic, dynamic> objRespuestaAlertaCantidadEnviado = new Dictionary<dynamic, dynamic>();
            Dictionary<dynamic, dynamic> objRespuestaActualizacionFecha = new Dictionary<dynamic, dynamic>();
            List<Dictionary<string, dynamic>> objRespuestaFeriado = new List<Dictionary<string, dynamic>>();
            Dictionary<string, dynamic> objParamsCorreo = new Dictionary<string, dynamic>();
            List<Dictionary<string, dynamic>> lista = new List<Dictionary<string, dynamic>>();

            try
            {


                //obtengo el dato del periodo
                objRespuestaPeriodo = this.repository.GetFechaInicioPeriodo();
                var obj = new { NPERIODO_PROCESO = objRespuestaPeriodo["Periodo"] };

                //Obtenemos la cantidad de veces que se tiene que enviar el correo
                objRespuestaCorreoCantidad = this.repository.GetObtenerCantidadEnvio();

                //obtenemos las veces que ya se envi� los correos
                objRespuestaAlertaCantidadEnviado = this.repository.GetObtenerCantidadEnviados(obj);

                //Validamos para que ya no envie el correo

                if (objRespuestaCorreoCantidad["Cantidad"] <= objRespuestaAlertaCantidadEnviado["CantidadEnviado"])
                {
                    objRespuesta["code"] = 2;
                    objRespuesta["mensaje"] = "Ya se envió el correo la cantidad de veces configuradas";
                    return objRespuesta;
                }


                //obtengo el asunto y cuerpo del correo
                objParamsCorreo["NIDACCION"] = 4;
                var objRespCorreo = this.repository.getCorreoCustomAction(objParamsCorreo);


                //Obtengo la lista de los clientes que falta responder


                lista = this.repository.GetListaResultadoEstadosCorreos(obj);
                if (lista.Count() == 0)
                {
                    objRespuesta["code"] = 2;
                    objRespuesta["mensaje"] = "No hay resultado de la lista de correos";
                    return objRespuesta;
                }

                var FechaValidadorEnvio = lista.ToList()[0]["DFECHA_REENVIO"].ToString("dd/MM/yyyy");

                //obtengo la fecha actual del servidor
                var FechaActual = DateTime.Now.ToString("dd/MM/yyyy");

                //Obtengo si el dia actual se envia los correos
                if (FechaActual == FechaValidadorEnvio)
                {
                    //Con esto se si la fecha es feriado o no 
                    var FechaValidador = new
                    {
                        //FECHA = "25/12/2021"
                        FECHA = lista.ToList()[0]["DFECHA_REENVIO"].ToString("dd/MM/yyyy")
                    };
                    objRespuestaFeriado = this.repository.GetFechaFeriado(FechaValidador);




                    //Valido si es feriado o no
                    if (objRespuestaFeriado.Count() > 0)
                    {
                        //Le sumo un dia a la fecha para que pueda validar el siguiente dia
                        var objParamsActualizacionFecha = new { NPERIODO_PROCESO = objRespuestaPeriodo["Periodo"] };

                        objRespuestaActualizacionFecha = this.repository.GetActualizarFechaEnvio(objParamsActualizacionFecha);
                        objRespuesta["code"] = 2;
                        objRespuesta["mensaje"] = "Es Feriado";
                        return objRespuesta;
                    }
                    else
                    {

                        //Con esto envio los parametros para el envio del correo
                        var objCorreo = new
                        {
                            SASUNTO_CORREO = objRespCorreo["SASUNTO_CORREO"],
                            SCUERPO_CORREO = objRespCorreo["SCUERPO_CORREO"],
                            PERIDOACTUAL = objRespuestaPeriodo["Periodo"],
                            FechaInicioPeriodo = objRespuestaPeriodo["FechaInicioPeriodo"],
                        };

                        foreach (var item in lista)
                        {
                            //Envio los correos
                            this.emailService.EnviarCorreoRecordatorio(item, objCorreo);

                            //Actualizo el contador
                            var objParamsActualizacionContador = new
                            {
                                NPERIODO_PROCESO = objRespuestaPeriodo["Periodo"],
                                NIDUSUARIO_ASIGNADO = item["ID_USUARIO"],
                                NIDGRUPOSENAL = item["NIDGRUPOSENAL"]
                            };

                            this.repository.GetActualizarContadorCorreo(objParamsActualizacionContador);

                        }
                    }
                }
                else
                {
                    objRespuesta["code"] = 2;
                    objRespuesta["mensaje"] = "Hoy no se envia correos";
                    return objRespuesta;
                }






                return objRespuesta;
            }
            catch (WebException ex)
            {
                objRespuesta["code"] = 1;
                objRespuesta["mensaje"] = ex.Message;
                return objRespuesta;
            }
        }

        public Dictionary<dynamic, dynamic> EncriptarTexto(dynamic param)
        {
            Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();

            try
            {
                string newTexto = param.texto;
                string result = string.Empty;
                byte[] encryted = System.Text.Encoding.Unicode.GetBytes(newTexto);
                result = Convert.ToBase64String(encryted);
                //return result;

                objRespuesta["code"] = 1;
                objRespuesta["mensaje"] = "Se encripto correctamente";
                objRespuesta["result"] = result;
                return objRespuesta;
            }
            catch (Exception ex)
            {

                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message;
                return objRespuesta;
            }
        }

        public Dictionary<dynamic, dynamic> DesencriptarTexto(dynamic param)
        {
            Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();

            try
            {
                string newTexto = param.texto;
                string result = string.Empty;
                byte[] decryted = Convert.FromBase64String(newTexto);
                //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
                result = System.Text.Encoding.Unicode.GetString(decryted);
                //return result;

                objRespuesta["code"] = 1;
                objRespuesta["mensaje"] = "Se desencripto correctamente";
                objRespuesta["result"] = result;
                return objRespuesta;
            }
            catch (Exception ex)
            {

                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message;
                return objRespuesta;
            }
        }

        public Dictionary<dynamic, dynamic> ActulizarContrasenaEncriptada(dynamic param)
        {
            Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
            Dictionary<dynamic, dynamic> objRespuestaEncriptado = new Dictionary<dynamic, dynamic>();

            try
            {
                param.texto = param.PASSWORD;
                objRespuestaEncriptado = EncriptarTexto(param);
                if (objRespuestaEncriptado["code"] == 2)
                {
                    objRespuesta["code"] = 2;
                    objRespuesta["mensaje"] = "Hubo problemas al encriptar el texto";
                    return objRespuesta;

                }
                var objParams = new
                {
                    CORREO = param.CORREO,
                    PASSWORD = objRespuestaEncriptado["result"]

                };
                this.repository.UpdActualizarCorreoOC(objParams);

                objRespuesta["code"] = 1;
                objRespuesta["mensaje"] = "Se actulizo Correctamente";
                return objRespuesta;
            }
            catch (Exception ex)
            {

                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message;
                return objRespuesta;
            }
        }

        public Dictionary<dynamic, dynamic> ObtenerCorreoyContrasenaOC()
        {
            Dictionary<dynamic, dynamic> objRespuesta = new Dictionary<dynamic, dynamic>();
            Dictionary<dynamic, dynamic> objRespuestaCorreo = new Dictionary<dynamic, dynamic>();
            Dictionary<dynamic, dynamic> objRespuestaDesencriptado = new Dictionary<dynamic, dynamic>();


            try
            {

                objRespuestaCorreo = this.repository.getObtenerContrasennaCorreo();

                var objParams = new
                {
                    texto = objRespuestaCorreo["PASSWORD"]
                };

                objRespuestaDesencriptado = DesencriptarTexto(objParams);

                var objParamsResultado = new
                {
                    texto = objRespuestaCorreo["PASSWORD"]
                };
                objRespuesta["correo"] = objRespuestaCorreo["CORREO"];
                objRespuesta["password"] = objRespuestaDesencriptado["result"];

                return objRespuesta;
            }
            catch (Exception ex)
            {

                objRespuesta["code"] = 2;
                objRespuesta["mensaje"] = ex.Message;
                return objRespuesta;
            }
        }


        public Dictionary<string, string> EnvioCorreoComplementoSinSennal(dynamic param)
        {
            var objRespuesta = new Dictionary<string, string>();
            try
            {
                this.emailService.SenderEmailComplementoSinSennal(param);
                objRespuesta["code"] = "1";
                objRespuesta["mensaje"] = "Se envio correctamente";
            }
            catch (Exception ex)
            {

                objRespuesta["code"] = "2";
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
            }



            return objRespuesta;
        }
        public string ObtenerPlantillaCotizacion(dynamic data)
        {
            string templatePath = "C:/archivos/PLANTILLAS/REGISTRO-NEGATIVO/MiPlantillaCotizacion.xlsx";
            string plantilla = "";

            try
            {
                MemoryStream ms = new MemoryStream();
                //ms.Flush();
                //ms.Position = 0; // or ms.Seek(0, SeekOrigin.Begin);
                using (FileStream fs = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
                {
                    fs.CopyTo(ms);
                }
                using (SLDocument sl = new SLDocument(ms))
                {
                    //int i = 7;
                    //int letra = 65;

                    sl.SetCellValue("B1", "Prueba1");
                    sl.SetCellValue("B2", "Prueba2");
                    sl.SetCellValue("B3", "Prueba3");

                    //foreach (QuotationReportVM item in lista)
                    //{
                    //int c = 0;

                    //  sl.SetCellValue(Char.ToString((char)(letra + 0)) + 7,"richi");

                    //i++;
                    //}
                    //using (MemoryStream ms2 = new MemoryStream()) 
                    // {
                    //MemoryStream ms2 = new MemoryStream();
                    //  sl.SaveAs(ms2);
                    //plantilla = Convert.ToBase64String(ms2.ToArray(), 0, ms2.ToArray().Length);
                    //}

                    using (var ms2 = new MemoryStream())
                    {
                        sl.SaveAs(ms2);
                        plantilla = Convert.ToBase64String(ms2.ToArray(), 0, ms2.ToArray().Length);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return plantilla;
        }


        public Dictionary<string, string> ObtenerPlantillaEs10()
        {
            string templatePath = "C:/archivos/PLANTILLAS/ES10/plantillaES10.xlsx";
            templatePath = Environment.ExpandEnvironmentVariables(templatePath);
            byte[] data = File.ReadAllBytes(templatePath);
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["base64"] = Convert.ToBase64String(data, 0, data.Length);
            return output;
        }
        public Es10 GetRegistrarDatosExcelEs10(dynamic param)
        {
            return this.preCargaEs10(param);

        }
        public ZonaGeografica GetRegistrarDatosZonaGeografica(dynamic param)
        {
            return this.preCargaZonaGeografica(param);

        }
        public ZonaGeografica preCargaZonaGeografica(dynamic param)
        {
            string filePath = this.Ruta + "/" + param.RutaExcel;
            SLDocument sl = new SLDocument(filePath);
            var worksheetStats = sl.GetWorksheetStatistics();
            int valor = 2;
            int validaraCabecera = 1;
            int fila = 1;
            int cantidad = 0;
            ZonaGeografica item = new ZonaGeografica();
            try
            {
                if (sl.GetCellValueAsString(validaraCabecera, 1).ToUpper().Trim() != "PERIODO")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera PERIODO en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 2).ToUpper().Trim() != "PRODUCTO")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera PRODUCTO en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 3).ToUpper().Trim() != "TIPO DOCUMENTO")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera TIPO DOCUMENTO en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 4).ToUpper().Trim() != "NÚMERO DE DOCUMENTO")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera NÚMERO DE DOCUMENTO en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 5).ToUpper().Trim() != "PRIMER NOMBRE")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera PRIMER NOMBRE en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 6).ToUpper().Trim() != "SEGUNDO NOMBRE")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera SEGUNDO NOMBRE en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 7).ToUpper().Trim() != "APELLIDO PATERNO")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera APELLIDO PATERNO en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 8).ToUpper().Trim() != "APELLIDO MATERNO")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera APELLIDO MATERNO en la fila " + fila;
                    return item;
                }

                if (sl.GetCellValueAsString(validaraCabecera, 9).ToUpper().Trim() != "DEPARTAMENTO")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera DEPARTAMENTO en la fila " + fila;
                    return item;
                }

                if (item.codigo != 2)
                {
                    if (string.IsNullOrEmpty(sl.GetCellValueAsString(2, 1)))
                    {
                        item.codigo = 2;
                        item.fila = fila;
                        item.mensaje = "Hay errores en la fila 2";
                        item.cantidad = 0;
                        return item;
                    }
                    param.NPERIODO_PROCESO = sl.GetCellValueAsInt32(valor, 1);
                    this.repository.delZonaGeografica(param);
                    int indexs = worksheetStats.EndRowIndex - 1;
                    ItemZonaGeografica _item = new ItemZonaGeografica();
                    _item.periodoProceso = new List<int>();
                    _item.producto = new List<string>();
                    _item.tipDoc = new List<string>();
                    _item.numDoc = new List<string>();
                    _item.primerNombre = new List<string>();
                    _item.segundoNombre = new List<string>();
                    _item.apellidoParterno = new List<string>();
                    _item.apellidoMaterno = new List<string>();
                    _item.region = new List<string>();
                    int arraposition = 0;
                    while (!string.IsNullOrEmpty(sl.GetCellValueAsString(valor, 1)))
                    {
                        if (!(sl.GetCellValueAsInt32(valor, 1) == 0 &&
                            sl.GetCellValueAsString(valor, 2) == "" &&
                            sl.GetCellValueAsString(valor, 3).Trim() == "" &&
                            sl.GetCellValueAsString(valor, 4).Trim() == "" &&
                            sl.GetCellValueAsString(valor, 5).Trim() == "" &&
                            sl.GetCellValueAsString(valor, 6).Trim() == "" &&
                            sl.GetCellValueAsString(valor, 7).ToUpper().Trim() == "" &&
                            sl.GetCellValueAsString(valor, 8).Trim() == "" &&
                            sl.GetCellValueAsString(valor, 9) == ""))
                        {
                            _item.periodoProceso.Add(sl.GetCellValueAsInt32(valor, 1));
                            _item.producto.Add(sl.GetCellValueAsString(valor, 2));
                            _item.tipDoc.Add(sl.GetCellValueAsString(valor, 3).Trim());
                            _item.numDoc.Add(sl.GetCellValueAsString(valor, 4).Trim());
                            _item.primerNombre.Add(sl.GetCellValueAsString(valor, 5).Trim());
                            _item.segundoNombre.Add(sl.GetCellValueAsString(valor, 6).Trim());
                            _item.apellidoParterno.Add(sl.GetCellValueAsString(valor, 7).ToUpper().Trim());
                            _item.apellidoMaterno.Add(sl.GetCellValueAsString(valor, 8).Trim());
                            _item.region.Add(sl.GetCellValueAsString(valor, 9));
                            arraposition++;
                            valor++;
                        }
                        //item.items = new ItemZonaGeografica();

                    }

                    item.items = _item;
                    item.cantidad = valor;
                    item = this.repository.GetRegistrarDatosExcelZonaGeografica(item);
                }
            }
            catch (Exception ex)
            {
                item.mensaje = ex.Message.ToString();
                item.codigo = 2;
            }
            finally
            {
                //item.items = null;
                //sl = null;
            }
            return item;
        }

        public Dictionary<string, string> GetSetearDataExcel(informeN1 param)
        {


            var objRespuesta = new Dictionary<string, string>();
            string fileName = "Formato-N1-Plantilla.xlsx";
            string sourcePath = @"C:\archivos\PLANTILLAS\N1";
            string targetPath = @"C:\archivos\PLANTILLAS\N1\Generado";
            informeN1 dataInforme = param;
            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            string destFile = System.IO.Path.Combine(targetPath, fileName);

            System.IO.Directory.CreateDirectory(targetPath);

            System.IO.File.Copy(sourceFile, destFile, true);

            try
            {
                string docName = "C:/archivos/PLANTILLAS/N1/Generado/Formato-N1-Plantilla.xlsx";

                string Hoja1 = "Indicaciones";
                string Hoja2 = "Definiciones";
                string Hoja3 = "a) Zonas Geográficas";
                string Hoja4 = "b) Productos";
                string Hoja5 = "c) Clientes";
                string Hoja6 = "Validación";

                string[] hojas = new string[] { Hoja3, Hoja4, Hoja5 };
                List<Celdas> celdas = new List<Celdas>();
                uint iniciadorZonaGeograficaNacional = 4;
                uint iniciadorZonaGeograficaExtranjero = 29;

                //Esto es para la zona geografica Nacional

                List<ZonaGeograficaN1Entity> zonaNacional = new List<ZonaGeograficaN1Entity>();
                ZonaGeograficaN1Entity zonasExtranjero = new ZonaGeograficaN1Entity();
                ProductoN1Entity productoAccPersonales = new ProductoN1Entity();
                ProductoN1Entity productoDesgravamen = new ProductoN1Entity();
                ProductoN1Entity productoSCTR = new ProductoN1Entity();
                ProductoN1Entity productoSepelio = new ProductoN1Entity();
                ProductoN1Entity productoSOAT = new ProductoN1Entity();
                ProductoN1Entity productoVidaParticular = new ProductoN1Entity();
                ProductoN1Entity productoIndividualCorto = new ProductoN1Entity();
                ProductoN1Entity productoVidaLeyExTrabajadores = new ProductoN1Entity();
                ProductoN1Entity productoVidaLeyTrabajadores = new ProductoN1Entity();

                ClientesTypeRegimenN1Entity tipoClienteGeneral = new ClientesTypeRegimenN1Entity();
                ClientesTypeRegimenN1Entity tipoClienteReforzado = new ClientesTypeRegimenN1Entity();
                ClientesTypeRegimenN1Entity tipoClienteSimplificado = new ClientesTypeRegimenN1Entity();

                ClientesCharacterClientN1Entity clientesExtranjero = new ClientesCharacterClientN1Entity();
                ClientesCharacterClientN1Entity clientesNacional = new ClientesCharacterClientN1Entity();
                ClientesCharacterClientN1Entity clientesJuridico = new ClientesCharacterClientN1Entity();
                ClientesCharacterClientN1Entity clientesNatural = new ClientesCharacterClientN1Entity();
                ClientesCharacterClientN1Entity clientesPep = new ClientesCharacterClientN1Entity();

                zonaNacional = dataInforme.zonaGeograficas.FindAll(t => t.zonaGeografica != "EXTRANJERO").ToList();
                zonasExtranjero = dataInforme.zonaGeograficas.Find(t => t.zonaGeografica == "EXTRANJERO");

                productoAccPersonales = dataInforme.productos.Find(t => t.producto == "ACCIDENTES PERSONALES");
                productoDesgravamen = dataInforme.productos.Find(t => t.producto == "DESGRAVAMEN");
                productoSCTR = dataInforme.productos.Find(t => t.producto == "SCTR");
                productoSepelio = dataInforme.productos.Find(t => t.producto == "SEPELIO DE CORTO PLAZO");
                productoSOAT = dataInforme.productos.Find(t => t.producto == "SOAT");
                productoVidaParticular = dataInforme.productos.Find(t => t.producto == "VIDA GRUPO PARTICULAR");
                productoIndividualCorto = dataInforme.productos.Find(t => t.producto == "VIDA INDIVIDUAL DE CORTO PLAZO");
                productoVidaLeyExTrabajadores = dataInforme.productos.Find(t => t.producto == "VIDA LEY EX-TRABAJADORES");
                productoVidaLeyTrabajadores = dataInforme.productos.Find(t => t.producto == "VIDA LEY TRABAJADORES");

                tipoClienteGeneral = dataInforme.clientesType.Find(t => t.tipoRegimen == "RÉGIMEN GENERAL");
                tipoClienteReforzado = dataInforme.clientesType.Find(t => t.tipoRegimen == "RÉGIMEN REFORZADO");
                tipoClienteSimplificado = dataInforme.clientesType.Find(t => t.tipoRegimen == "RÉGIMEN SIMPLIFICADO");

                clientesExtranjero = dataInforme.clientesCharacter.Find(t => t.tipoClientes == "Clientes Extranjeros");
                clientesNacional = dataInforme.clientesCharacter.Find(t => t.tipoClientes == "Clientes Nacionales");
                clientesJuridico = dataInforme.clientesCharacter.Find(t => t.tipoClientes == "EMPRESA (PERSONA JURÍDICA)");
                clientesNatural = dataInforme.clientesCharacter.Find(t => t.tipoClientes == "PERSONA NATURAL");
                clientesPep = dataInforme.clientesCharacter.Find(t => t.tipoClientes == "PERSONAS EXPUESTAS POLITICAMENTE (PEP)");


                for (int i = 0; i < zonaNacional.Count; i++)
                {
                    celdas.Add(new Celdas { sheet = Hoja3, number = iniciadorZonaGeograficaNacional, letter = "D", value = zonaNacional[i].numeroPolizas.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja3, number = iniciadorZonaGeograficaNacional, letter = "E", value = zonaNacional[i].numeroContratantes.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja3, number = iniciadorZonaGeograficaNacional, letter = "F", value = zonaNacional[i].numeroAsegurados.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja3, number = iniciadorZonaGeograficaNacional, letter = "G", value = zonaNacional[i].numeroBeneficiarios.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja3, number = iniciadorZonaGeograficaNacional, letter = "H", value = zonaNacional[i].clienteReforzado.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja3, number = iniciadorZonaGeograficaNacional, letter = "I", value = zonaNacional[i].montoPrima.ToString() });
                    iniciadorZonaGeograficaNacional++;
                }
                //Esto es para la zona geografica extrnajera
                celdas.Add(new Celdas { sheet = Hoja3, number = iniciadorZonaGeograficaExtranjero, letter = "D", value = zonasExtranjero.numeroPolizas.ToString() });
                celdas.Add(new Celdas { sheet = Hoja3, number = iniciadorZonaGeograficaExtranjero, letter = "E", value = zonasExtranjero.numeroContratantes.ToString() });
                celdas.Add(new Celdas { sheet = Hoja3, number = iniciadorZonaGeograficaExtranjero, letter = "F", value = zonasExtranjero.numeroAsegurados.ToString() });
                celdas.Add(new Celdas { sheet = Hoja3, number = iniciadorZonaGeograficaExtranjero, letter = "G", value = zonasExtranjero.numeroBeneficiarios.ToString() });
                celdas.Add(new Celdas { sheet = Hoja3, number = iniciadorZonaGeograficaExtranjero, letter = "H", value = zonasExtranjero.clienteReforzado.ToString() });
                celdas.Add(new Celdas { sheet = Hoja3, number = iniciadorZonaGeograficaExtranjero, letter = "I", value = zonasExtranjero.montoPrima.ToString() });

                //Esto es para tipo de regimen
                celdas.Add(new Celdas { sheet = Hoja5, number = 4, letter = "D", value = tipoClienteSimplificado.numeroClientes.ToString() });
                celdas.Add(new Celdas { sheet = Hoja5, number = 5, letter = "D", value = tipoClienteGeneral.numeroClientes.ToString() });
                celdas.Add(new Celdas { sheet = Hoja5, number = 6, letter = "D", value = tipoClienteReforzado.numeroClientes.ToString() });

                //Esto es para caracterisiticas de clientes
                celdas.Add(new Celdas { sheet = Hoja5, number = 11, letter = "E", value = clientesPep.numeroClientes.ToString() });
                celdas.Add(new Celdas { sheet = Hoja5, number = 11, letter = "F", value = clientesPep.numeroClienteReforzado.ToString() });
                celdas.Add(new Celdas { sheet = Hoja5, number = 11, letter = "G", value = clientesPep.montoPrima.ToString() });

                celdas.Add(new Celdas { sheet = Hoja5, number = 14, letter = "E", value = clientesNacional.numeroClientes.ToString() });
                celdas.Add(new Celdas { sheet = Hoja5, number = 14, letter = "F", value = clientesNacional.numeroClienteReforzado.ToString() });
                celdas.Add(new Celdas { sheet = Hoja5, number = 14, letter = "G", value = clientesNacional.montoPrima.ToString() });

                celdas.Add(new Celdas { sheet = Hoja5, number = 15, letter = "E", value = clientesExtranjero.numeroClientes.ToString() });
                celdas.Add(new Celdas { sheet = Hoja5, number = 15, letter = "F", value = clientesExtranjero.numeroClienteReforzado.ToString() });
                celdas.Add(new Celdas { sheet = Hoja5, number = 15, letter = "G", value = clientesExtranjero.montoPrima.ToString() });

                celdas.Add(new Celdas { sheet = Hoja5, number = 16, letter = "E", value = clientesNatural.numeroClientes.ToString() });
                celdas.Add(new Celdas { sheet = Hoja5, number = 16, letter = "F", value = clientesNatural.numeroClienteReforzado.ToString() });
                celdas.Add(new Celdas { sheet = Hoja5, number = 16, letter = "G", value = clientesNatural.montoPrima.ToString() });

                celdas.Add(new Celdas { sheet = Hoja5, number = 17, letter = "E", value = clientesJuridico.numeroClientes.ToString() });
                celdas.Add(new Celdas { sheet = Hoja5, number = 17, letter = "F", value = clientesJuridico.numeroClienteReforzado.ToString() });
                celdas.Add(new Celdas { sheet = Hoja5, number = 17, letter = "G", value = clientesJuridico.montoPrima.ToString() });

                //Productos
                if (false)
                {
                    celdas.Add(new Celdas { sheet = Hoja4, number = 36, letter = "E", value = productoAccPersonales.numeroPolizas.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 36, letter = "F", value = productoAccPersonales.numeroContratantes.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 36, letter = "G", value = productoAccPersonales.numeroAsegurados.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 36, letter = "H", value = productoAccPersonales.numeroBeneficiarios.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 36, letter = "I", value = productoAccPersonales.clienteReforzado.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 36, letter = "J", value = productoAccPersonales.montoPrima.ToString() });

                    celdas.Add(new Celdas { sheet = Hoja4, number = 52, letter = "E", value = productoDesgravamen.numeroPolizas.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 52, letter = "F", value = productoDesgravamen.numeroContratantes.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 52, letter = "G", value = productoDesgravamen.numeroAsegurados.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 52, letter = "H", value = productoDesgravamen.numeroBeneficiarios.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 52, letter = "I", value = productoDesgravamen.clienteReforzado.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 52, letter = "J", value = productoDesgravamen.montoPrima.ToString() });

                    celdas.Add(new Celdas { sheet = Hoja4, number = 67, letter = "E", value = productoSCTR.numeroPolizas.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 67, letter = "F", value = productoSCTR.numeroContratantes.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 67, letter = "G", value = productoSCTR.numeroAsegurados.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 67, letter = "H", value = productoSCTR.numeroBeneficiarios.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 67, letter = "I", value = productoSCTR.clienteReforzado.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 67, letter = "J", value = productoSCTR.montoPrima.ToString() });

                    celdas.Add(new Celdas { sheet = Hoja4, number = 54, letter = "E", value = productoSepelio.numeroPolizas.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 54, letter = "F", value = productoSepelio.numeroContratantes.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 54, letter = "G", value = productoSepelio.numeroAsegurados.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 54, letter = "H", value = productoSepelio.numeroBeneficiarios.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 54, letter = "I", value = productoSepelio.clienteReforzado.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 54, letter = "J", value = productoSepelio.montoPrima.ToString() });

                    celdas.Add(new Celdas { sheet = Hoja4, number = 34, letter = "E", value = productoSOAT.numeroPolizas.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 34, letter = "F", value = productoSOAT.numeroContratantes.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 34, letter = "G", value = productoSOAT.numeroAsegurados.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 34, letter = "H", value = productoSOAT.numeroBeneficiarios.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 34, letter = "I", value = productoSOAT.clienteReforzado.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 34, letter = "J", value = productoSOAT.montoPrima.ToString() });

                    celdas.Add(new Celdas { sheet = Hoja4, number = 50, letter = "E", value = productoVidaParticular.numeroPolizas.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 50, letter = "F", value = productoVidaParticular.numeroContratantes.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 50, letter = "G", value = productoVidaParticular.numeroAsegurados.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 50, letter = "H", value = productoVidaParticular.numeroBeneficiarios.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 50, letter = "I", value = productoVidaParticular.clienteReforzado.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 50, letter = "J", value = productoVidaParticular.montoPrima.ToString() });

                    celdas.Add(new Celdas { sheet = Hoja4, number = 53, letter = "E", value = productoIndividualCorto.numeroPolizas.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 53, letter = "F", value = productoIndividualCorto.numeroContratantes.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 53, letter = "G", value = productoIndividualCorto.numeroAsegurados.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 53, letter = "H", value = productoIndividualCorto.numeroBeneficiarios.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 53, letter = "I", value = productoIndividualCorto.clienteReforzado.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 53, letter = "J", value = productoIndividualCorto.montoPrima.ToString() });

                    //celdas.Add(new Celdas { sheet = Hoja4, number = 69, letter = "E", value =  data.productoSobrevivencia.NUMERO_POLIZAS.ToString() });
                    //celdas.Add(new Celdas { sheet = Hoja4, number = 69, letter = "F", value =  data.productoSobrevivencia.NUMERO_CONTRATANTES.ToString() });
                    //celdas.Add(new Celdas { sheet = Hoja4, number = 69, letter = "G", value =  data.productoSobrevivencia.NUMERO_ASEGURADOS.ToString() });
                    //celdas.Add(new Celdas { sheet = Hoja4, number = 69, letter = "H", value =  data.productoSobrevivencia.NUMERO_BENEFICIARIOS.ToString() });
                    //celdas.Add(new Celdas { sheet = Hoja4, number = 69, letter = "I", value =  data.productoSobrevivencia.CLIENTE_REFORZADO.ToString() });
                    //celdas.Add(new Celdas { sheet = Hoja4, number = 69, letter = "J", value =  data.productoSobrevivencia.MONTO_PRIMA.ToString() });

                    celdas.Add(new Celdas { sheet = Hoja4, number = 58, letter = "E", value = productoVidaLeyExTrabajadores.numeroPolizas.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 58, letter = "F", value = productoVidaLeyExTrabajadores.numeroContratantes.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 58, letter = "G", value = productoVidaLeyExTrabajadores.numeroAsegurados.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 58, letter = "H", value = productoVidaLeyExTrabajadores.numeroBeneficiarios.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 58, letter = "I", value = productoVidaLeyExTrabajadores.clienteReforzado.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 58, letter = "J", value = productoVidaLeyExTrabajadores.montoPrima.ToString() });

                    celdas.Add(new Celdas { sheet = Hoja4, number = 51, letter = "E", value = productoVidaLeyTrabajadores.numeroPolizas.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 51, letter = "F", value = productoVidaLeyTrabajadores.numeroContratantes.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 51, letter = "G", value = productoVidaLeyTrabajadores.numeroAsegurados.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 51, letter = "H", value = productoVidaLeyTrabajadores.numeroBeneficiarios.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 51, letter = "I", value = productoVidaLeyTrabajadores.clienteReforzado.ToString() });
                    celdas.Add(new Celdas { sheet = Hoja4, number = 51, letter = "J", value = productoVidaLeyTrabajadores.montoPrima.ToString() });

                }
                //celdas.Add(new Celdas { sheet = Hoja3, number = 5, letter = "C", value = " '1" });
                //celdas.Add(new Celdas() { sheet = Hoja3, number = 6, letter = "C", value = " '2" });
                //celdas.Add(new Celdas() { sheet = Hoja3, number = 7, letter = "C", value = " '3" });
                //celdas.Add(new Celdas() { sheet = Hoja3, number = 8, letter = "C", value = " '4" });
                //celdas.Add(new Celdas() { sheet = Hoja3, number = 8, letter = "D", value = " '4" });
                //celdas.Add(new Celdas() { sheet = Hoja4, number = 6, letter = "D", value = "PERU" });
                //celdas.Add(new Celdas() { sheet = Hoja4, number = 7, letter = "D", value = "FRANCIA" });
                //celdas.Add(new Celdas() { sheet = Hoja4, number = 8, letter = "D", value = "COREA" });
                //celdas.Add(new Celdas() { sheet = Hoja4, number = 9, letter = "D", value = "COLOMBIA" });
                //celdas.Add(new Celdas() { sheet = Hoja4, number = 10, letter = "D", value = "5" });
                //celdas.Add(new Celdas() { sheet = Hoja5, number = 5, letter = "D", value = "CHILE" });
                //celdas.Add(new Celdas() { sheet = Hoja5, number = 6, letter = "D", value = "ARGENTINA" });
                //celdas.Add(new Celdas() { sheet = Hoja5, number = 7, letter = "D", value = "INDIA" });

                using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(docName, true))
                {
                    SharedStringTablePart shareStringPart;
                    if (spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
                    {
                        shareStringPart = spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                    }
                    else
                    {
                        shareStringPart = spreadSheet.WorkbookPart.AddNewPart<SharedStringTablePart>();
                    }
                    for (int i = 0; i < hojas.Length; i++)
                    {
                        WorksheetPart worksheetPart = GetWorksheetPart(spreadSheet.WorkbookPart, hojas[i]);
                        List<Celdas> celdas2 = celdas.FindAll(t => t.sheet == hojas[i]);
                        for (int j = 0; j < celdas2.Count; j++)
                        {
                           
                            Cell cell = InsertCellInWorksheet(celdas2[j].letter, celdas2[j].number, worksheetPart);
                            if (celdas2[j].value != "0")
                            {
                                cell.CellValue = new CellValue(celdas2[j].value);
                                cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                                worksheetPart.Worksheet.Save();
                            }
                            
                           
                        }

                    }
                    spreadSheet.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = true;
                    spreadSheet.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;

                }

            }
            catch (Exception ex)
            {

                objRespuesta["code"] = "2";
                objRespuesta["mensaje"] = ex.Message.ToString();
                objRespuesta["mensajeError"] = ex.ToString();
            }


            return objRespuesta;
        }

        public class Celdas
        {
            public string sheet { get; set; }
            public uint number { get; set; }
            public string letter { get; set; }
            public string value { get; set; }
            public CellValues tipo { get; set; }
        }

        public static WorksheetPart GetWorksheetPart(WorkbookPart workbookPart, string sheetName)
        {
            string relId = workbookPart.Workbook.Descendants<Sheet>()
                                 .Where(s => sheetName.Equals(s.Name))
                                 .First()
                                 .Id;
            return (WorksheetPart)workbookPart.GetPartById(relId);
        }

        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;
            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };

                sheetData.Append(row);
            }
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (cell.CellReference.Value.Length == cellReference.Length)
                    {
                        if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                        {
                            refCell = cell;
                            break;
                        }
                    }
                }
                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);
                worksheet.Save();
                return newCell;
            }
        }


        public Es10 preCargaEs10(dynamic param)
        {
            string filePath = this.Ruta + "/" + param.RutaExcel;
            SLDocument sl = new SLDocument(filePath);
            var worksheetStats = sl.GetWorksheetStatistics();
            int valor = 2;
            int validaraCabecera = 1;
            int fila = 1;
            int i = 1;
            int cantidad = 0;
            Es10 item = new Es10();
            try
            {
                if (sl.GetCellValueAsString(validaraCabecera, i).ToUpper().Trim() != "RAMO")
                {
                    i++;
                }
                if (sl.GetCellValueAsString(validaraCabecera, i).ToUpper().Trim() != "RAMO")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera RAMO en la fila " + fila;
                    return item;
                }
                i++;
                if (sl.GetCellValueAsString(validaraCabecera, i).ToUpper().Trim() != "RIESGO")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera RIESGO en la fila " + fila;
                    return item;
                }
                i++;
                if (sl.GetCellValueAsString(validaraCabecera, i).ToUpper().Trim() != "CODIGO DE RIESGO")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera CODIGO DE RIESGO en la fila " + fila;
                    return item;
                }
                i++;
                if (sl.GetCellValueAsString(validaraCabecera, i).ToUpper().Trim() != "CODIGO DE REGISTRO")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera CODIGO DE REGISTRO en la fila " + fila;
                    return item;
                }
                i++;
                if (sl.GetCellValueAsString(validaraCabecera, i).ToUpper().Trim() != "NOMBRE COMERCIAL DEL PRODUCTO O POLIZA DE SEGURO")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera NOMBRE COMERCIAL DEL PRODUCTO O PÓLIZA DE SEGURO en la fila " + fila;
                    return item;
                }
                i++;
                if (sl.GetCellValueAsString(validaraCabecera, i).ToUpper().Trim() != "MONEDA")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera MONEDA en la fila " + fila;
                    return item;
                }
                i++;
                if (sl.GetCellValueAsString(validaraCabecera, i).ToUpper().Trim() != "FECHA DE INICIO DE COMERCIALIZACION")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera FECHA DE INICIO DE COMERCIALIZACION en la fila " + fila;
                    return item;
                }
                i++;
                if (sl.GetCellValueAsString(validaraCabecera, i).ToUpper().Trim() != "N DE ASEGURADOS")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera N DE ASEGURADOS en la fila " + fila;
                    return item;
                }

                if (item.codigo != 2)
                {
                    if (string.IsNullOrEmpty(sl.GetCellValueAsString(2, 1)))
                    {
                        item.codigo = 2;
                        item.fila = fila;
                        item.mensaje = "Hay errores en la fila 2";
                        item.cantidad = 0;
                        return item;
                    }
                    int periodoProceso = 0;
                    if (i == 8)
                    {
                        periodoProceso = this.repository.getPeriodoSemestralMax();
                    }
                    else
                    {
                        periodoProceso = sl.GetCellValueAsInt32(2, 1);
                    }

                    param.NPERIODO_PROCESO = periodoProceso;
                    this.repository.getDeleteEs10(param);
                    int indexs = worksheetStats.EndRowIndex - 1;
                    ItemEs10 _item = new ItemEs10();
                    _item.periodoProceso = new List<int>();
                    _item.ramo = new List<string>();
                    _item.riesgo = new List<string>();
                    _item.codRiesgo = new List<int>();
                    _item.codRegistro = new List<string>();
                    _item.nomComercial = new List<string>();
                    _item.moneda = new List<string>();
                    _item.fecini = new List<DateTime?>();
                    _item.numAsegurados = new List<int>();
                    _item.sRegimen = new List<string>();
                    int arraposition = 0;
                    bool isData = false;
                    int column = i == 9 ? 2 : 1;
                    while (!string.IsNullOrEmpty(sl.GetCellValueAsString(valor, column)))
                    {
                        if (!(sl.GetCellValueAsString(valor, column).Trim() == "" &&
                            sl.GetCellValueAsString(valor, column + 1).Trim() == "" &&
                            sl.GetCellValueAsInt32(valor, column + 2) == 0 &&
                            sl.GetCellValueAsString(valor, column + 3).Trim() == "" &&
                            sl.GetCellValueAsString(valor, column + 4).ToUpper().Trim() == "" &&
                            sl.GetCellValueAsString(valor, column + 5).Trim() == "" &&
                            sl.GetCellValueAsDateTime(valor, column + 6) == null &&
                            sl.GetCellValueAsInt32(valor, column + 7) == 0 &&
                            sl.GetCellValueAsString(valor, column + 8) == ""))
                        {
                            isData = true;
                            _item.periodoProceso.Add(periodoProceso);
                            _item.ramo.Add(sl.GetCellValueAsString(valor, column).Trim());
                            _item.riesgo.Add(sl.GetCellValueAsString(valor, column + 1).Trim());
                            _item.codRiesgo.Add(sl.GetCellValueAsInt32(valor, column + 2));
                            _item.codRegistro.Add(sl.GetCellValueAsString(valor, column + 3).Trim());
                            _item.nomComercial.Add(sl.GetCellValueAsString(valor, column + 4).ToUpper().Trim());
                            _item.moneda.Add(sl.GetCellValueAsString(valor, column + 5).Trim());
                            _item.fecini.Add(sl.GetCellValueAsDateTime(valor, column + 6));
                            _item.numAsegurados.Add(sl.GetCellValueAsInt32(valor, column + 7));
                            _item.sRegimen.Add(sl.GetCellValueAsString(valor, column + 8));
                            arraposition++;
                            valor++;
                        }
                    }
                    item.items = new ItemEs10();
                    item.items = _item;
                    item.cantidad = valor;
                    if (isData)
                        item = this.repository.GetRegistrarDatosExcelEs10(item);
                }
            }
            catch (Exception ex)
            {
                item.mensaje = ex.Message.ToString();
                item.codigo = 2;
            }
            finally
            {
                //item.items = null;
                //sl = null;
            }
            return item;
        }
        public Es10Response GetListaEs10(dynamic param)
        {
            return this.repository.GetListaEs10(param);

        }
        public List<Dictionary<string, dynamic>> GetKriListContratantes()
        {
            return this.repository.GetKriListContratantes();

        }
        public List<Dictionary<string, dynamic>> GetKriListZonasGeograficas()
        {
            return this.repository.GetKriListZonasGeograficas();
        }
        public ZonaGeograficaResponse GetKriSearchZonaGeografica(dynamic param)
        {
            return this.repository.GetKriSearchZonaGeografica(param);
        }
        public Dictionary<string, dynamic> updZonasGeograficas(dynamic param)
        {
            return this.repository.updateZonaGeografica(param);
        }
        public ActividadEconomica GetRegistrarDatosActividadEconomica(dynamic param)
        {
            return this.preCargaActividadEconomica(param);

        }
        public List<FrequencyListResponseDTO> GetPeriodoSemestral()
        {
            return this.repository.GetPeriodoSemestral();

        }
        public List<Dictionary<string, dynamic>> GetListaActividadEconomica(dynamic param)
        {
            return this.repository.GetListaActividadEconomica(param);
        }
        public ActividadEconomica preCargaActividadEconomica(dynamic param)
        {
            string filePath = this.Ruta + "/" + param.RutaExcel;
            SLDocument sl = new SLDocument(filePath);
            var worksheetStats = sl.GetWorksheetStatistics();
            int valor = 2;
            int validaraCabecera = 1;
            int fila = 1;
            int cantidad = 0;
            ActividadEconomica item = new ActividadEconomica();
            try
            {
                if (sl.GetCellValueAsString(validaraCabecera, 1).ToUpper().Trim() != "PERIODO")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera PERIODO en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 2).ToUpper().Trim() != "RAZÓN SOCIAL")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera RAZÓN SOCIAL en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 3).ToUpper().Trim() != "NÚMERO DE RUC")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera NÚMERO DE RUC en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 4).ToUpper().Trim() != "TIPO CONTRIBUYENTE")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera TIPO CONTRIBUYENTE en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 5).ToUpper().Trim() != "ACTIVIDAD ECONÓMICA")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera ACTIVIDAD ECONÓMICA en la fila " + fila;
                    return item;
                }
                if (sl.GetCellValueAsString(validaraCabecera, 6).ToUpper().Trim() != "SECTOR")
                {
                    item.codigo = 2;
                    item.fila = fila;
                    item.mensaje = "No tiene la cabecera SECTOR en la fila " + fila;
                    return item;
                }

                if (item.codigo != 2)
                {
                    if (string.IsNullOrEmpty(sl.GetCellValueAsString(2, 1)))
                    {
                        item.codigo = 2;
                        item.fila = fila;
                        item.mensaje = "Hay errores en la fila 2";
                        item.cantidad = 0;
                        return item;
                    }
                    param.NPERIODO_PROCESO = sl.GetCellValueAsInt32(valor, 1);
                    this.repository.getDeleteActivity(param);
                    int indexs = worksheetStats.EndRowIndex - 1;
                    ItemsActividadEconomica _item = new ItemsActividadEconomica();
                    _item.nPeriodoProceso = new List<int>();
                    _item.sDescription = new List<string>();
                    _item.sNumRuc = new List<string>();
                    _item.sTipoContribuyente = new List<string>();
                    _item.sActivityEconomy = new List<string>();
                    _item.sSector = new List<string>();
                    int arraposition = 0;
                    bool isData = false;
                    while (!string.IsNullOrEmpty(sl.GetCellValueAsString(valor, 1)))
                    {
                        if (!(sl.GetCellValueAsInt32(valor, 1) == 0 &&
                           sl.GetCellValueAsString(valor, 2).Trim() == "" &&
                           sl.GetCellValueAsString(valor, 3).Trim() == "" &&
                           sl.GetCellValueAsString(valor, 4) == "" &&
                           sl.GetCellValueAsString(valor, 5).Trim() == "" &&
                           sl.GetCellValueAsString(valor, 6).Trim() == ""))
                        {
                            isData = true;
                            item.items = new ItemsActividadEconomica();
                            _item.nPeriodoProceso.Add(sl.GetCellValueAsInt32(valor, 1));
                            _item.sDescription.Add(sl.GetCellValueAsString(valor, 2).Trim());
                            _item.sNumRuc.Add(sl.GetCellValueAsString(valor, 3).Trim());
                            _item.sTipoContribuyente.Add(sl.GetCellValueAsString(valor, 4));
                            _item.sActivityEconomy.Add(sl.GetCellValueAsString(valor, 5).Trim());
                            _item.sSector.Add(sl.GetCellValueAsString(valor, 6).Trim());
                        }
                        arraposition++;
                        valor++;
                    }

                    item.items = _item;
                    item.cantidad = valor;

                    if (isData)
                        item = this.repository.GetRegistrarDatosExcelActividadEconomico(item);
                }
            }
            catch (Exception ex)
            {
                item.mensaje = ex.Message.ToString();
                item.codigo = 2;
            }
            finally
            {
                //item.items = null;
                //sl = null;
            }
            return item;
        }
        //public void consultaSunat() {
        //    string urlBase = "http://ww1.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias?accion=consPorRuc&nroRuc=";
        //    string urlCaptcha = "http://ww1.sunat.gob.pe/cl-ti-itmrconsruc/captcha?accion=image";

        //    UrlDownloadToFile urlDownloadToFile = null;
        //    string path = TempFileManager.Instance.CreateTempFile("urldownload");
        //    urlDownloadToFile.FilePath = path;
        //    urlDownloadToFile.Download(SilentProgressHost.Instance);
        //    return new FileStream(path, FileMode.Open, FileAccess.Read);
        //}


        public Dictionary<string, string> GetEliminarArchivo(dynamic data)
        {

            string ruta = this.Ruta + "/" + data.ruta;
            var objRespuesta = new Dictionary<string, string>();


            if (System.IO.Directory.Exists(ruta))
            {
                try
                {
                    System.IO.Directory.Delete(ruta, true);

                    objRespuesta["code"] = "1";
                    objRespuesta["mensaje"] = "Se elimino correctamente";
                    objRespuesta["mensajeError"] = "Se elimino correctamente";

                }

                catch (System.IO.IOException e)
                {
                    objRespuesta["code"] = "2";
                    objRespuesta["mensaje"] = e.Message.ToString();
                    objRespuesta["mensajeError"] = e.ToString();

                }
            }
            else
            {
                objRespuesta["code"] = "2";
                objRespuesta["mensaje"] = "No existe el archivo";
                objRespuesta["mensajeError"] = "No existe el archivo";
            };


            return objRespuesta;
        }


    }
}
