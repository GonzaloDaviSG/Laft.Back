using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
//using System.Web.DynamicData;
using System.Web.Http;
//using System.Web.Http.Cors;
//using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using protecta.laft.api.DTO;
using protecta.laft.api.Services;
using System.Net;
using protecta.laft.api.Models;

namespace protecta.laft.api.Controllers
{
    [Route("api/monitoreoSenales")]
    [ApiController]
    public class MonitoreoSenalesController : ControllerBase
    {
        private SenialService service;
        private EmailService emailService;

        public MonitoreoSenalesController()
        {
            this.service = new SenialService();
            this.emailService = new EmailService();
        }

        [Route("GetMovementHistory")]
        [HttpPost]
        public IActionResult GetMovementHistory(dynamic param)
        {
            return Ok(this.service.GetMovementHistory(param));
        }

        [Route("GetProfileList")]
        [HttpGet]
        public IActionResult GetProfileList()
        {
            return Ok(this.service.GetProfileList());
        }

        [Route("GetListAction")]
        [HttpGet]
        public IActionResult GetListAction()
        {
            return Ok(this.service.GetListAction());
        }

        [Route("GetListPerfiles")]
        [HttpGet]
        public IActionResult GetListPerfiles()
        {
            return Ok(this.service.GetListPerfiles());
        }

        [Route("GetListCorreo")]
        [HttpGet]
        public IActionResult GetListCorreo()
        {
            return Ok(this.service.GetListCorreo());
        }

        [Route("GetListaPerfiles")]
        [HttpGet]
        public IActionResult GetListaPerfiles()
        {
            return Ok(this.service.GetListaPerfiles());
        }


        [Route("GetGrupoSenal")]
        [HttpGet]
        public IActionResult GetGrupoSenal()
        {
            return Ok(this.service.GetGrupoSenal());
        }

        [Route("GetPolicyList")]
        [HttpPost]
        public IActionResult GetPolicyList(dynamic param)
        {
            return Ok(this.service.GetPolicyList(param));
        }

        [Route("GetQuestionDetail")]
        [HttpPost]
        public IActionResult GetQuestionDetail(MonitoreoSenalesParamsDTO param)
        {
            return Ok(this.service.GetQuestionDetail(param));
        }

        [Route("GetListConifgCorreoDefault")]
        [HttpGet]
        public IActionResult GetListConifgCorreoDefault()
        {
            return Ok(this.service.GetListConifgCorreoDefault());
        }

        [Route("GetQuestionHeader")]
        [HttpPost]
        public IActionResult GetQuestionHeader(MonitoreoSenalesParamsDTO param)
        {
            return Ok(this.service.GetQuestionHeader(param));
        }
        [Route("GetSearchClientsPep")]
        [HttpPost]
        public IActionResult GetSearchClientsPep(dynamic param)
        {
            return Ok(this.service.GetSearchClientsPep(param));
        }
        [Route("GetSearchClientsPepSeacsa")]
        [HttpPost]
        public IActionResult GetSearchClientsPepSeacsa(dynamic param)
        {
            return Ok(this.service.GetSearchClientsPepSeacsa(param));
        }
        [Route("InsertQuestionDetail")]
        [HttpPost]
        public IActionResult InsertQuestionDetail(QuestionDetailDTO param)
        {
            return Ok(this.service.InsertQuestionDetail(param));
        }

        [Route("InsertQuestionHeader")]
        [HttpPost]
        public IActionResult InsertQuestionHeader(MonitoreoSenalesParamsDTO param)
        {
            return Ok(this.service.InsertQuestionHeader(param));
        }

        [Route("GetAlertFormList")]
        [HttpPost]
        public IActionResult GetAlertFormList(AlertFormParamDTO param)
        {
            return Ok(this.service.GetAlertFormList(param));
        }

        [Route("GetOfficialAlertFormList")]
        [HttpPost]
        public IActionResult GetOfficialAlertFormList(OfficialAlertFormParamDTO param)
        {
            return Ok(this.service.GetOfficialAlertFormList(param));
        }

        [Route("GetListNcCompanies")]
        [HttpPost]
        public IActionResult GetListNcCompanies(NcCompaniesParamDTO param)
        {
            return Ok(this.service.GetListNcCompanies(param));
        }

        [Route("UpdateListNcCompanies")]
        [HttpPost]
        public IActionResult UpdateListNcCompanies(UpdateNcCompaniesParamDTO param)
        {
            return Ok(this.service.UpdateListNcCompanies(param));
        }

        [Route("UpdateRevisedState")]
        [HttpPost]
        public IActionResult UpdateRevisedState(RevisedParamDTO param)
        {
            return Ok(this.service.UpdateRevisedState(param));
        }

        [Route("GetCommentsHeader")]
        [HttpPost]
        public IActionResult GetCommentsHeader(CommentsHeaderParamDTO param)
        {
            return Ok(this.service.GetCommentsHeader(param));
        }

        [Route("InsertCommentHeader")]
        [HttpPost]
        public IActionResult InsertCommentHeader(CommentsHeaderParamDTO param)
        {
            return Ok(this.service.InsertCommentHeader(param));
        }

        [Route("UploadFiles")]
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public IActionResult UploadFiles(dynamic param)
        {
            this.service.UploadFiles(param);
            return Ok();
        }


        [Route("GetAttachedFiles")]
        [HttpPost]
        public IActionResult GetAttachedFiles(dynamic param)
        {
            return Ok(this.service.GetAttachedFiles(param));
        }

        [Route("DownloadFile")]
        [HttpPost]
        public IActionResult DownloadFile(dynamic param)
        {
            return Ok(this.service.DownloadFile(param));
        }

        [Route("InsertAttachedFiles")]
        [HttpPost]
        public IActionResult InsertAttachedFiles(dynamic param)
        {
            return Ok(this.service.InsertAttachedFiles(param));
        }


        [Route("GetWorkModuleList")]
        [HttpPost]
        public IActionResult GetWorkModuleList(dynamic param)
        {
            return Ok(this.service.GetWorkModuleList(param));
        }

        [Route("GetWorkModuleDetail")]
        [HttpPost]
        public IActionResult GetWorkModuleDetail(dynamic param)
        {
            return Ok(this.service.GetWorkModuleDetail(param));
        }

        [Route("InsertCompanyDetailUser")]
        [HttpPost]
        public IActionResult InsertCompanyDetailUser(CompanyDetailUserDTO param)
        {
            return Ok(this.service.InsertCompanyDetailUser(param));
        }

        [Route("GetProductsCompany")]
        [HttpPost]
        public IActionResult GetProductsCompany()
        {
            return Ok(this.service.GetProductsCompany());
        }

        [Route("SendComplimentary")]
        [HttpPost]
        public IActionResult SendComplimentary(/*ComplimentraryDTO*/dynamic param)
        {

            var complimentary = this.service.SendComplimentary(param);
            //Servicio para enviar el correo
            var emailSended = this.emailService.SenderEmailCompRequest(param);

            return Ok(complimentary);
        }

        [Route("GetOCEmailList")]
        [HttpPost]
        public IActionResult GetOCEmail()
        {
            return Ok(this.service.GetOCEmail());
        }

        [Route("GetListaCargo")]
        [HttpPost]
        public IActionResult GetListaCargo()
        {
            return Ok(this.service.GetListaCargo());
        }

        [Route("GetListaAlertaComplemento")]
        [HttpPost]
        public IActionResult GetListaAlertaComplemento()
        {
            return Ok(this.service.GetListaAlertaComplemento());
        }

        [Route("ListaUsariosComp")]
        [HttpPost]
        public IActionResult ListaUsariosComp()
        {
            return Ok(this.service.ListaUsariosComp());
        }


        [Route("GetListaResultado")]
        [HttpPost]
        public IActionResult GetListaResultado(dynamic param)
        {
            return Ok(this.service.GetListaResultado(param));
        }


        [Route("EmailSender")]
        [HttpPost]
        public IActionResult EmailSender(EmailSenderDTO param)
        {

            var emailSended = this.emailService.EmailSender(param.fullName, param.manager, param.rol, param.email);

            return Ok();
        }

        [HttpPost("updateStatusAlert")]
        public IActionResult UpdateStatusAlert(dynamic param)
        {
            return Ok(this.service.UpdateStatusAlert(param));
        }
        [HttpPost("GetAnulacionAlerta")]
        public IActionResult GetAnulacionAlerta(dynamic param)
        {
            return Ok(this.service.GetAnulacionAlerta(param));
        }


        [HttpPost("DeleteAdjuntosInformAlerta")]
        public IActionResult DeleteAdjuntosInformAlerta(dynamic param)
        {
            return Ok(this.service.DeleteAdjuntosInformAlerta(param));
        }

        [HttpPost("getDeleteAdjuntos")]
        public IActionResult getDeleteAdjuntos(dynamic param)
        {
            return Ok(this.service.getDeleteAdjuntos(param));
        }



        [HttpPost("GetRegistrarDatosExcelGC")]
        public IActionResult GetRegistrarDatosExcelGC(dynamic param)
        {
            return Ok(this.service.GetRegistrarDatosExcelGC(param));
        }

        [HttpPost("GetRegistrarDatosExcelRegistronegativo")]
        public IActionResult GetRegistrarDatosExcelRegistronegativo(dynamic param)
        {
            return Ok(this.service.GetRegistrarDatosExcelRegistronegativo(param));
        }

        [HttpPost("GetRegistrarDatosExcelEs10")]
        public IActionResult GetRegistrarDatosExcelEs10(dynamic param)
        {
            return Ok(this.service.GetRegistrarDatosExcelEs10(param));
        }

      

        

        [HttpPost("GetInsertaHistorialUsuario")]
        public IActionResult GetInsertaHistorialUsuario(dynamic param)
        {
            return Ok(this.service.GetInsertaHistorialUsuario(param));
        }

        [HttpPost("UpdInformes")]
        public IActionResult UpdInformes(dynamic param)
        {
            return Ok(this.service.UpdInformes(param));
        }

        [HttpPost("UpdActualizarCorreoOC")]
        public IActionResult UpdActualizarCorreoOC(dynamic param)
        {
            return Ok(this.service.UpdActualizarCorreoOC(param));
        }


        [HttpGet("getCorreo_OC")]
        public IActionResult getCorreo_OC()
        {
            return Ok(this.service.getCorreo_OC());
        }

        [HttpGet("getObtenerContrasennaCorreo")]
        public IActionResult getObtenerContrasennaCorreo()
        {
            return Ok(this.service.getObtenerContrasennaCorreo());
        }

        [HttpGet("ObtenerCorreoyContrasenaOC")]
        public IActionResult ObtenerCorreoyContrasenaOC()
        {
            return Ok(this.service.ObtenerCorreoyContrasenaOC());
        }

        


        [HttpPost("delEliminarDemanda")]
        public IActionResult DelEliminarDemanda(dynamic param)
        {
            return Ok(this.service.DelEliminarDemanda(param));
        }

        [HttpPost("UpdRutaComplementos")]
        public IActionResult UpdRutaComplementos(dynamic param)
        {
            return Ok(this.service.UpdRutaComplementos(param));
        }

        [HttpPost("getCuerpoCorreo")]
        public IActionResult getCuerpoCorreo(dynamic param)
        {
            return Ok(this.service.getCuerpoCorreo(param));
        }

        [HttpPost("DesenciptarPassUsuario")]
        public IActionResult DesenciptarPassUsuario(dynamic param)
        {
            return Ok(this.service.DesenciptarPassUsuario(param));
        }

        [HttpPost("getDeleteRegistrosNegativos")]
        public IActionResult getDeleteRegistrosNegativos()
        {
            return Ok(this.service.getDeleteRegistrosNegativos());
        }

        


        [Route("GetGafiList")]
        [HttpPost]
        public IActionResult GetGafiList()
        {
            return Ok(this.service.GetGafiList());
        }

        [Route("GetListaInformes")]
        [HttpPost]
        public IActionResult GetListaInformes(dynamic param)
        {
            return Ok(this.service.GetListaInformes(param));
        }


        [Route("GetSignalList")]
        [HttpPost]
        public IActionResult GetSignalList(dynamic param)
        {
            return Ok(this.service.GetSignalList(param));
        }
        [Route("GetListaResultadoProveedorContraparte")]
        [HttpPost]
        public IActionResult GetListaResultadoProveedorContraparte(dynamic param)
        {
            return Ok(this.service.GetListaResultadoProveedorContraparte(param));
        }

        [Route("GetListaResultadoEstadosCorreos")]
        [HttpPost]
        public IActionResult GetListaResultadoEstadosCorreos(dynamic param)
        {
            return Ok(this.service.GetListaResultadoEstadosCorreos(param));
        }

        [Route("UpdateStatusToReviewed")]
        [HttpPost]
        public IActionResult UpdateStatusToReviewed(dynamic param)
        {
            return Ok(this.service.UpdateStatusToReviewed(param));
        }

        [Route("GetRegimeList")]
        [HttpPost]
        public IActionResult GetRegimeList()
        {
            return Ok(this.service.GetRegimeList());
        }

        [Route("GetCurrentPeriod")]
        [HttpPost]
        public IActionResult GetCurrentPeriod()
        {
            return Ok(this.service.GetCurrentPeriod());
        }

        [Route("GetAlertReportList")]
        [HttpPost]
        public IActionResult GetAlertReportList(dynamic param)
        {
            return Ok(this.service.GetAlertReportList(param));
        }

        [Route("GetInternationalLists")]
        [HttpPost]
        public IActionResult GetInternationalLists(dynamic param)
        {
            return Ok(this.service.GetInternationalLists(param));
        }

        [Route("GetPepList")]
        [HttpPost]
        public IActionResult GetPepList(dynamic param)
        {
            return Ok(this.service.GetPepList(param));
        }

        [Route("GetFamiliesPepList")]
        [HttpPost]
        public IActionResult GetFamiliesPepList(dynamic param)
        {
            return Ok(this.service.GetFamiliesPepList(param));
        }

        [Route("GetSacList")]
        [HttpPost]
        public IActionResult GetSacList(dynamic param)
        {
            return Ok(this.service.GetSacList(param));
        }

        [Route("GetListEspecial")]
        [HttpPost]
        public IActionResult GetListEspecial(dynamic param)
        {
            return Ok(this.service.GetListEspecial(param));
        }

        [Route("UpdateUnchecked")]
        [HttpPost]
        public async Task<IActionResult> UpdateUnchecked(dynamic param)
        {
            return await Task.Run(() =>
                  {
                      return Ok(this.service.UpdateUnchecked(param));
                  });
        }

        [Route("GetAddressList")]
        [HttpPost]
        public IActionResult GetAddressList(dynamic param)
        {
            return Ok(this.service.GetAddressList(param));
        }

        [Route("FillReport")]
        [HttpPost]
        //public IActionResult FillReport (dynamic param) {
        //    return Ok (this.service.FillReport (param));
        //}

        [Route("EnvioCorreoConfirmacion")]
        [HttpPost]
        public IActionResult EnvioCorreoConfirmacion(dynamic param)
        {
            return Ok(this.service.EnvioCorreoConfirmacion(param));
        }

        [Route("EnvioCorreoComplementoSinSennal")]
        [HttpPost]
        public IActionResult EnvioCorreoComplementoSinSennal(dynamic param)
        {
            return Ok(this.service.EnvioCorreoComplementoSinSennal(param));
        }

        [Route("getCorreoCustomAction")]
        [HttpPost]
        public IActionResult getCorreoCustomAction(dynamic param)
        {
            return Ok(this.service.getCorreoCustomAction(param));
        }
        


        [Route("EnvioCorreoActualizacionPass")]
        [HttpPost]
        public IActionResult EnvioCorreoActualizacionPass(dynamic param)
        {
            return Ok(this.service.EnvioCorreoActualizacionPass(param));
        }

        [Route("Consulta360")]
        [HttpPost]
        public IActionResult Consulta360(dynamic param)
        {
            return Ok(this.service.Consulta360(param));
        }

        [Route("Consulta360Previous")]
        [HttpPost]
        public IActionResult Consulta360Previous(dynamic param)
        {
            return Ok(this.service.Consulta360Previous(param));
        }

        //[Route("ConsultaWC")]
        //[HttpPost]
        //public IActionResult ConsultaWC(dynamic param)
        //{
        //    return Ok(this.service.ConsultaWC(param));
        //}

        // [Route ("DownloadPDF")]
        // [HttpPost]
        // public IActionResult DownloadPDF (dynamic param) {
        //     return Ok (this.service.DownloadPDF (param));            
        // }        

        [Route("getListasInternacionalesByType")]
        [HttpPost]
        public IActionResult getListasInternacionalesByType(dynamic param)
        {
            return Ok(this.service.getListasInternacionalesByType(param));
        }

        [Route("getListaInternacional")]
        [HttpPost]
        public IActionResult getListaInternacional(dynamic param)
        {
            return Ok(this.service.getListaInternacional(param));
        }

        [Route("getListaResultadosCoincid")]
        [HttpPost]
        public IActionResult getListaResultadosCoincid(dynamic param)
        {
            return Ok(this.service.getListaResultadosCoincid(param));
        }



        [Route("getResultadosCoincidencias")]
        [HttpPost]
        public IActionResult getResultadosCoincidencias(dynamic param)
        {
            return Ok(this.service.getResultadosCoincidencias(param));
        }

        [Route("InsertAttachedFilesByAlert")]
        [HttpPost]
        public IActionResult InsertAttachedFilesByAlert(dynamic param)
        {
            return Ok(this.service.InsertAttachedFilesByAlert(param));
        }


        [Route("InsertAttachedFilesInformByAlert")]
        [HttpPost]
        public IActionResult InsertAttachedFilesInformByAlert(dynamic param)
        {
            return Ok(this.service.InsertAttachedFilesInformByAlert(param));
        }

        [Route("GetAttachedFilesByAlert")]
        [HttpPost]
        public IActionResult GetAttachedFilesByAlert(dynamic param)
        {
            return Ok(this.service.GetAttachedFilesByAlert(param));
        }

        [Route("GetAttachedFilesInformByAlert")]
        [HttpPost]
        public IActionResult GetAttachedFilesInformByAlert(dynamic param)
        {
            return Ok(this.service.GetAttachedFilesInformByAlert(param));
        }

        [Route("GetAttachedFilesInformByCabecera")]
        [HttpPost]
        public IActionResult GetAttachedFilesInformByCabecera(dynamic param)
        {
            return Ok(this.service.GetAttachedFilesInformByCabecera(param));
        }

        [Route("DownloadFileByAlert")]
        [HttpPost]
        public IActionResult DownloadFileByAlert(dynamic param)
        {
            return Ok(this.service.DownloadFileByAlert(param));
        }

        [Route("DownloadUniversalFileByAlert")]
        [HttpPost]
        public IActionResult DownloadUniversalFileByAlert(dynamic param)
        {
            return Ok(this.service.DownloadUniversalFileByAlert(param));
        }
        [Route("DownloadTemplate")]
        [HttpPost]
        public IActionResult DownloadTemplate(dynamic param)
        {
            return Ok(this.service.DownloadTemplate(param));
        }


        [Route("UploadFilesByAlert")]
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public IActionResult UploadFilesByAlert(dynamic param)
        {
            this.service.UploadFilesByAlert(param);
            return Ok();
        }

        [Route("UploadFilesUniversalByRuta")]
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public IActionResult UploadFilesUniversalByRuta(dynamic param)
        {

            return Ok(this.service.UploadFilesUniversalByRuta(param));
        }

        [Route("UploadFilesInformByAlert")]
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public IActionResult UploadFilesInformByAlert(dynamic param)
        {
            this.service.UploadFilesInformByAlert(param);
            return Ok();
        }

        [Route("GetPerfilXGrupo")]
        [HttpPost]
        public IActionResult GetPerfilXGrupo(dynamic param)
        {
            return Ok(this.service.GetPerfilXGrupo(param));
        }

        [Route("GetListaComplementos")]
        [HttpPost]
        public IActionResult GetListaComplementos(dynamic param)
        {
            return Ok(this.service.GetListaComplementos(param));
        }

        [Route("GetListaCompUsu")]
        [HttpPost]
        public IActionResult GetListaCompUsu(dynamic param)
        {
            return Ok(this.service.GetListaCompUsu(param));
        }

        [Route("GetListaComplementoUsuario")]
        [HttpPost]
        public IActionResult GetListaComplementoUsuario(dynamic param)
        {
            return Ok(this.service.GetListaComplementoUsuario(param));
        }


        [Route("GetListaPolizas")]
        [HttpPost]
        public IActionResult GetListaPolizas(dynamic param)
        {
            Console.WriteLine("el rsultado: " + param);
            return Ok(this.service.GetListaPolizas(param));
        }

        [Route("GetGrupoXPerfil")]
        [HttpPost]
        public IActionResult GetGrupoXPerfil(dynamic param)
        {
            return Ok(this.service.GetGrupoXPerfil(param));
        }
        [Route("GetSubGrupoSenal")]
        [HttpPost]
        public IActionResult GetSubGrupoSenal(dynamic param)
        {
            return Ok(this.service.GetSubGrupoSenal(param));
        }

        [Route("GetGrupoXSenal")]
        [HttpPost]
        public IActionResult GetGrupoXSenal(dynamic param)
        {
            return Ok(this.service.GetGrupoXSenal(param));
        }
        [Route("GetRegistrarDatosZonaGeografica")]
        [HttpPost]
        public IActionResult GetRegistrarDatosZonaGeografica(dynamic param)
        {
            return Ok(this.service.GetRegistrarDatosZonaGeografica(param));
        }


        [Route("GetResultadoTratamiento")]
        [HttpPost]
        public IActionResult GetResultadoTratamiento(dynamic param)
        {
            return Ok(this.service.GetResultadoTratamiento(param));
        }

        [Route("GetResultsList")]
        [HttpPost]
        public IActionResult GetResultsList(dynamic param)
        {
            return Ok(this.service.GetResultsList(param));
        }
        [Route("GetListaResultadoGC")]
        [HttpPost]
        public IActionResult GetListaResultadoGC(dynamic param)
        {
            return Ok(this.service.GetListaResultadoGC(param));
        }


        [Route("UpdateListClienteRefor")]
        [HttpPost]
        public IActionResult UpdateListClienteRefor(dynamic param)
        {
            return Ok(this.service.UpdateListClienteRefor(param));
        }

        [Route("AnularResultadosCliente")]
        [HttpPost]
        public IActionResult AnularResultadosCliente(dynamic param)
        {
            return Ok(this.service.AnularResultadosCliente(param));
        }

        [Route("GetUpdateCorreos")]
        [HttpPost]
        public IActionResult GetUpdateCorreos(dynamic param)
        {
            Console.WriteLine("el sersultado: " + param);
            return Ok(this.service.GetUpdateCorreos(param));
        }

        [Route("BusquedaADemandal")]
        [HttpPost]
        public IActionResult BusquedaADemandal(DemandaRequestDTO param)
        {
            return Ok(this.service.BusquedaADemanda(param));
        }

        [Route("BusquedaConcidenciaXDocXName")]
        [HttpPost]
        public IActionResult BusquedaConcidenciaXDocXName(dynamic param)
        {
            return Ok(this.service.BusquedaConcidenciaXDocXName(param));
        }
        [Route("BusquedaManual")]
        [HttpPost]
        public IActionResult BusquedaManual(dynamic param)
        {
            return Ok(this.service.BusquedaManual(param));
        }

        [Route("UpdateTratamientoCliente")]
        [HttpPost]
        public IActionResult UpdateTratamientoCliente(dynamic param)
        {
            return Ok(this.service.UpdateTratamientoCliente(param));
        }

        [Route("GetResultadoTratamientoHistorico")]
        [HttpPost]
        public IActionResult GetResultadoTratamientoHistorico(dynamic param)
        {
            return Ok(this.service.GetResultadoTratamientoHistorico(param));
        }

        [Route("GetCantidadResultadoTratamientoHistorico")]
        [HttpPost]
        public IActionResult GetCantidadResultadoTratamientoHistorico(dynamic param)
        {
            return Ok(this.service.GetCantidadResultadoTratamientoHistorico(param));
        }

        [Route("UpdateStateSenialCabUsuario")]
        [HttpPost]
        public IActionResult UpdateStateSenialCabUsuario(dynamic param)
        {
            return Ok(this.service.UpdateStateSenialCabUsuario(param));
        }

        [HttpPost("UpdateStateSenialCabUsuarioRealByForm")]
        public IActionResult UpdateStateSenialCabUsuarioReal(dynamic param)
        {
            return Ok(this.service.UpdateStateSenialCabUsuarioReal(param));
        }

        [HttpPost("BusquedaConcidenciaXNombre")]
        public IActionResult BusquedaConcidenciaXNombre(dynamic param)
        {
            return Ok(this.service.BusquedaConcidenciaXNombre(param));
        }

        //[HttpPost("BusquedaConcidenciaXNombreDemanda")]
        //public IActionResult BusquedaConcidenciaXNombreDemanda(dynamic param)
        //{
        //    return Ok(this.service.BusquedaConcidenciaXNombreDemanda(param));
        //}

        //[HttpPost("BusquedaConcidenciaXNumeroDocDemanda")]
        //public IActionResult BusquedaConcidenciaXNumeroDocDemanda(dynamic param)
        //{
        //    return Ok(this.service.BusquedaConcidenciaXNombreDemanda(param));
        //}
        public string GenerarCodigo()
        {
            int longitud = 8;
            const string alfabeto = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder token = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < longitud; i++)
            {
                int indice = rnd.Next(alfabeto.Length);
                token.Append(alfabeto[indice]);
            }
            return token.ToString();
        }

        [HttpPost("BusquedaConcidenciaXDoc")]
        public IActionResult BusquedaConcidenciaXDoc(dynamic param)
        {
            return Ok(this.service.BusquedaConcidenciaXDoc(param));
        }

        [HttpPost("GetResultadoCoincidenciasPen")]
        public IActionResult GetResultadoCoincidenciasPen(dynamic param)
        {
            return Ok(this.service.GetResultadoCoincidenciasPen(param));
        }

        [HttpPost("GetHistorialEstadoCli")]
        public IActionResult GetHistorialEstadoCli(dynamic param)
        {
            return Ok(this.service.GetHistorialEstadoCli(param));
        }

        [HttpPost("InsertUpdateProfile")]
        public IActionResult InsertUpdateProfile(dynamic param)
        {
            return Ok(this.service.InsertUpdateProfile(param));
        }

        [HttpPost("InsertUpdateProfileGrupos")]
        public IActionResult InsertUpdateProfileGrupos(dynamic param)
        {
            return Ok(this.service.InsertUpdateProfileGrupos(param));
        }

        [HttpPost("InsertUpdateComplemento")]
        public IActionResult InsertUpdateComplemento(dynamic param)
        {
            return Ok(this.service.InsertUpdateComplemento(param));
        }

        [HttpPost("ValidarPolizaVigente")]
        public IActionResult ValidarPolizaVigente(dynamic param)
        {
            return Ok(this.service.ValidarPolizaVigente(param));
        }

        [HttpPost("GetUpdComplementoCab")]
        public IActionResult GetUpdComplementoCab(dynamic param)
        {
            return Ok(this.service.GetUpdComplementoCab(param));
        }

        [HttpPost("GetInsCormularioComplUsu")]
        public IActionResult GetInsCormularioComplUsu(dynamic param)
        {
            if(param.VALIDADOR_CORREO == "COMPLETADO")
            {
                var emailSended = this.emailService.SenderEmailComplemento(param);

            }

           return Ok(this.service.GetInsCormularioComplUsu(param));
        }

        [HttpPost("GetUpdPssUsuario")]
        public IActionResult GetUpdPssUsuario(dynamic param)
        {
            var emailSended = this.emailService.SenderEmailCompRequest2(param);

            return Ok(this.service.GetUpdPssUsuario(param));
        }

        [HttpPost("GetUpdUsuarioEncriptado")]
        public IActionResult GetUpdUsuarioEncriptado(dynamic param)
        {


            return Ok(this.service.GetUpdUsuarioEncriptado(param));
        }

        [HttpPost("GetActualizarFechaEnvio")]
        public IActionResult GetActualizarFechaEnvio(dynamic param)
        {


            return Ok(this.service.GetActualizarFechaEnvio(param));
        }

        [HttpPost("GetActualizarContadorCorreo")]
        public IActionResult GetActualizarContadorCorreo(dynamic param)
        {


            return Ok(this.service.GetActualizarContadorCorreo(param));
        }




        [HttpPost("GetActPassUsuario")]
        public IActionResult GetActPassUsuario(dynamic param)
        {

            return Ok(this.service.GetActPassUsuario(param));
        }

        [Route("getListaUsuarioCorreos")]
        [HttpPost]
        public IActionResult getListaUsuarioCorreos(dynamic param)
        {
            return Ok(this.service.getListaUsuarioCorreos(param));
        }

        [Route("getListaAdjuntos")]
        [HttpPost]
        public IActionResult getListaAdjuntos(dynamic param)
        {
            return Ok(this.service.getListaAdjuntos(param));
        }

        [Route("GetValidarExisteCorreo")]
        [HttpPost]
        public IActionResult GetValidarExisteCorreo(dynamic param)
        {
            return Ok(this.service.GetValidarExisteCorreo(param));
        }

        [Route("GetValidarHash")]
        [HttpPost]
        public IActionResult GetValidarHash(dynamic param)
        {
            return Ok(this.service.GetValidarHash(param));
        }

        [Route("GetFechaInicioPeriodo")]
        [HttpPost]
        public IActionResult GetFechaInicioPeriodo()
        {
            return Ok(this.service.GetFechaInicioPeriodo());
        }

        [Route("GetFechaFeriado")]
        [HttpPost]
        public IActionResult GetFechaFeriado(dynamic param)
        {
            return Ok(this.service.GetFechaFeriado(param));
        }


        [Route("GetListaOtrosClientes")]
        [HttpPost]
        public IActionResult GetListaOtrosClientes(dynamic param)
        {
            return Ok(this.service.GetListaOtrosClientes(param));
        }

        [Route("GetListaRegistroNegativo")]
        [HttpPost]
        public IActionResult GetListaRegistroNegativo(dynamic param)
        {
            return Ok(this.service.GetListaRegistroNegativo(param));
        }

        



        [Route("GetListaEmpresas")]
        [HttpPost]
        public IActionResult GetListaEmpresas(dynamic param)
        {
            return Ok(this.service.GetListaEmpresas(param));
        }


        [HttpPost("InsCorreoUsuario")]
        public IActionResult InsCorreoUsuario(dynamic param)
        {

            return Ok(this.service.InsCorreoUsuario(param));
        }



        [Route("GetAlertaResupuesta")]
        [HttpPost]
        public IActionResult GetAlertaResupuesta(dynamic param)
        {

            return Ok(this.service.GetAlertaResupuesta(param));
        }


        [HttpPost("GetValFormularioCompl")]
        public IActionResult GetValFormularioCompl(dynamic param)
        {
            return Ok(this.service.GetValFormularioCompl(param));
        }

        [Route("GetListaTipo")]
        [HttpGet]
        public IActionResult GetListaTipo()
        {
            return Ok(this.service.GetListaTipo());
        }

        [Route("LeerDataExcel")]
        [HttpPost]
        public IActionResult LeerDataExcel(dynamic param)
        {
            return Ok(this.service.LeerDataExcel(param));
        }
        [Route("setDataExcelDemanda")]
        [HttpPost]
        public IActionResult setDataExcelDemanda(dynamic param)
        {
            return Ok(this.service.setDataExcelDemanda(param));
        }
        [Route("ObtenerPlantillaCotizacion")]
        [HttpPost]
        public IActionResult ObtenerPlantillaCotizacion(dynamic param)
        {
            return Ok(this.service.ObtenerPlantillaCotizacion(param));
        }
        [Route("ObtenerPlantillaEs10")]
        [HttpGet]
        public IActionResult ObtenerPlantillaEs10()
        {
            return Ok(this.service.ObtenerPlantillaEs10());
        }

        [Route("GetSetearDataExcel")]
        [HttpPost]
        public IActionResult GetSetearDataExcel(informeN1 param)
        {
            return Ok(this.service.GetSetearDataExcel(param));
        }

        [Route("GetEliminarArchivo")]
        [HttpPost]
        public IActionResult GetEliminarArchivo(dynamic param)
        {
            return Ok(this.service.GetEliminarArchivo(param));
        }


        [Route("ValidarFechaHash")]
        [HttpPost]
        public IActionResult ValidarFechaHash(dynamic param)
        {
            return Ok(this.service.ValidarFechaHash(param));
        }

        [Route("RecordarotorioCorreos")]
        [HttpGet]
        public IActionResult RecordarotorioCorreos()
        {
            return Ok(this.service.RecordarotorioCorreos());
        }

        [Route("EncriptarTexto")]
        [HttpGet]
        public IActionResult EncriptarTexto(dynamic param)
        {
            return Ok(this.service.EncriptarTexto(param));
        }


        [Route("DesencriptarTexto")]
        [HttpGet]
        public IActionResult DesencriptarTexto(dynamic param)
        {
            return Ok(this.service.DesencriptarTexto(param));
        }

        [Route("ActulizarContrasenaEncriptada")]
        [HttpPost]
        public IActionResult ActulizarContrasenaEncriptada(dynamic param)
        {
            return Ok(this.service.ActulizarContrasenaEncriptada(param));


        }

        [Route("getClientsforRegimen")]
        [HttpPost]
        public IActionResult getClientsforRegimen(dynamic param)
        {

            var resp = this.service.getClientsforRegimen(param);
            return Ok(resp);
        }

        [Route("getrucsunat")]
        [HttpPost]
        public IActionResult getrucsunat(dynamic param)
        {

            return Ok(this.service.getrucsunat(param));

        }

        [Route("getListWebLinksCliente")]
        [HttpPost]
        public IActionResult getListWebLinksCliente(dynamic param)
        {
                return Ok(this.service.getListWebLinksCliente(param));
        }
        [Route("getListWebLinksClienteAll")]
        [HttpPost]
        public IActionResult getListWebLinksClienteAll(dynamic param)
        {
                return Ok(this.service.getListWebLinksClienteAll(param));
        }
        [Route("getDeleteWebLinksCoincidence")]
        [HttpPost]
        public IActionResult getDeleteWebLinksCoincidence(dynamic param)
        {

            return Ok(this.service.getDeleteWebLinksCoincidence(param));

        }
        [Route("addWebLinkscliente")]
        [HttpPost]
        public IActionResult addWebLinkscliente(dynamic param)
        {
                return Ok(this.service.addWebLinkscliente(param));
        }
        [Route("getClientWcEstado")]
        [HttpPost]
        public IActionResult getClientWcEstado(dynamic param)
        {
            return Ok(this.service.getClientWcEstado(param));
        }
        [Route("getListProveedor")]
        [HttpGet]
        public IActionResult getListProveedor()
        {
            return Ok(this.service.getListProveedor());
        }
        [Route("GetListaEs10")]
        [HttpPost]
        public IActionResult GetListaEs10(dynamic param)
        {
            return Ok(this.service.GetListaEs10(param));
        }
        [Route("GetListaActividadEconomica")]
        [HttpPost]
        public IActionResult GetListaActividadEconomica(dynamic param)
        {
            return Ok(this.service.GetListaActividadEconomica(param));
        }
        [Route("GetKriListContratantes")]
        [HttpGet]
        public IActionResult GetKriListContratantes()
        {
            return Ok(this.service.GetKriListContratantes());
        }

        [Route("GetRegistrarDatosActividadEconomica")]
        [HttpPost]
        public IActionResult GetRegistrarDatosActividadEconomica(dynamic param) {
            return Ok(this.service.GetRegistrarDatosActividadEconomica(param));
        }
        [Route("GetPeriodoSemestral")]
        [HttpGet]
        public IActionResult GetPeriodoSemestral( )
        {
            return Ok(this.service.GetPeriodoSemestral());
        }
        [Route("GetKriListZonasGeograficas")]
        [HttpGet]
        public IActionResult GetKriListZonasGeograficas()
        {
            return Ok(this.service.GetKriListZonasGeograficas());
        }
        [Route("GetKriSearchZonaGeografica")]
        [HttpPost]
        public IActionResult GetKriSearchZonaGeografica(dynamic param)
        {
            return Ok(this.service.GetKriSearchZonaGeografica(param));
        }
        [Route("updZonasGeograficas")]
        [HttpPost]
        public IActionResult updZonasGeograficas(dynamic param)
        {
            return Ok(this.service.updZonasGeograficas(param));
        }
        [Route("getInformeKri")]
        [HttpPost]
        public IActionResult getInformeKri(dynamic param)
        {
            return Ok(this.service.getInformeKri(param));
        }
        [Route("getInformeN1")] /**/
        [HttpPost]
        public IActionResult getInformeN1(dynamic param)
        {
            return Ok(this.service.getInformeN1(param));
        }
    }
}
