using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using protecta.laft.api.DTO;
using protecta.laft.api.Models;
using protecta.laft.api.Services;
using protecta.laft.api.Services.Interfaces;
using protecta.laft.api.Utils;

namespace protecta.laft.api.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class SbsReportController : ControllerBase {
        private SbsReportService sbsReportService;
        private EmailService emailService;

        public SbsReportController () {

            this.sbsReportService = new SbsReportService ();
            this.emailService = new EmailService ();

        }

        [HttpPost ("getCommentList")]
        public ActionResult<CommentListResponseDTO> GetCommentList (CommentListParamDTO param) {
            return Ok (this.sbsReportService.GetCommentList (param.alertId, param.periodId));
        }

        
        [HttpPost ("updateCommentList")]
        public ActionResult<UpdateCommentListResponseDTO> UpdateCommentList (UpdateCommentListParamDTO param) {
            return Ok (this.sbsReportService.UpdateCommentList (param.alertId, param.periodId, param.comment, param.userId));
        }

        [HttpPost ("getQuestionsByAlert")]
        public ActionResult<QuestionsByAlertResponseDTO> GetQuestionsByAlert (QuestionsByAlertParametersDTO inputParams) {
            return Ok (this.sbsReportService.GetQuestionsByAlert (inputParams));
        }

        [HttpGet ("getAlertList")]
        public ActionResult GetAlertList () {
            return Ok (this.sbsReportService.GetAlertList ());
        }

        [HttpPost ("updateAlert")]
        public ActionResult<UpdateAlertResponseDTO> UpdateAlertStatus (UpdateAlertParametersDTO inputParams) {
             Console.Write("entro en el controle");
            return Ok (this.sbsReportService.UpdateAlertStatus (inputParams.alertId, inputParams.alertName, inputParams.alertDescription, inputParams.alertStatus, inputParams.userId, inputParams.bussinessDays, inputParams.reminderSender, inputParams.operType,inputParams.idgrupo, inputParams.regimenSim,inputParams.regimenGen));
        }

        [HttpPost ("updateQuestion")]
        public ActionResult<UpdateQuestionResponseDTO> UpdateQuestion (UpdateQuestionParametersDTO inputParams) {
            return Ok (this.sbsReportService.UpdateQuestion (inputParams.alertId, inputParams.questionId, inputParams.originId, inputParams.questionName, inputParams.questionStatus, inputParams.userId, inputParams.transactionType, inputParams.validComment));
        }

        [HttpPost ("suspendFrequencyStatus")]
        public ActionResult<SuspendStatusResponseDTO> SuspendFrequencyStatus (SuspendStatusParametersDTO inputParams) {
            return Ok (this.sbsReportService.SuspendFrequencyStatus (inputParams.frequencyId, inputParams.suspensionId));
        }

        [HttpPost ("experianInvoker")]
        public IActionResult ExperianInvoker (dynamic inputParams) {
            Console.WriteLine ("HOLAS : " + inputParams);
            return Ok (this.sbsReportService.ExperianInvoker (inputParams));
        }
        //Servicio para obtener el tipo de cambio
        [HttpGet ("getExchangeRate")]
        public ActionResult<ExchangeRateResponseDTO> GetExchangeRate () {
            var exchangeRate = sbsReportService.GetExchangeRate ();
            return exchangeRate;
        }

        [HttpGet ("getAmount")]
        public ActionResult<AmountResponseDTO> GetAmount () {
            var amount = sbsReportService.GetAmount ();
            return amount;
        }
        //Servicio para generar el reporte sbs y enviar correo
        [HttpPost ("generateReport")]
        public ActionResult<SbsReportGenResponseDTO> GenerateSbsReport (SbsReportGenParametersDTO report) {
            if (ModelState.IsValid) {

                //Generar report
                var sbsReport = this.sbsReportService.GenerateSbsReport (report.operType, report.exchangeType, report.ammount, report.startDate, report.endDate, report.nameReport, report.sbsFileType);

                //Obtener usuario en sesión
                var user = this.sbsReportService.GetUser (report.userId);

                //Enviar correo
                var email = this.emailService.SenderEmailReportGen (user.name, user.email, sbsReport.route, sbsReport.reportSbsId, sbsReport.message, report.startDate, report.endDate, report.desReport, report.desOperType, report.sbsFileType);

                return sbsReport;
            }
            return BadRequest (ModelState);
        }
        //Servicio para listar los reportes sbs que se generaron
        [HttpPost ("sbsReportList")]
        public ActionResult<SbsReportGenListResponseDTO> GetListReports (SbsReportGenListParametersDTO inputParams) {
            return Ok (this.sbsReportService.GetListReports (inputParams));
        }

        [HttpPost ("alertList")]
        public ActionResult<AlertMonitoringResponseDTO> GetListAlerts (AlertMonitoringParametersDTO inputParams) {
            return Ok (this.sbsReportService.GetListAlerts (inputParams));
        }

        //Servicio para btener archivo del reporte
        [HttpPost ("getSbsReportFile")]
        public ActionResult<List<SbsReportFileResponseDTO>> GetReport (SbsReportFileParametersDTO file) {
            if (ModelState.IsValid) {
                Console.Write ("id  id  id  id " + file);
                var route = this.sbsReportService.GetReport (file.id, file.tipo_archivo);
                return route;
                //return this.sbsReportService.GetReport (file);
            }
            return BadRequest (ModelState);
        }

        [HttpGet ("getReportTypes")]
        public ActionResult GetReportTypes () {
            return Ok (this.sbsReportService.GetReportTypes ());
        }

        [HttpGet ("getGafiList")]
        public ActionResult GetGafiList () {
            return Ok (this.sbsReportService.GetGafiList ());
        }

        [HttpPost ("updateCountry")]
        public ActionResult<UpdateCountryResponseDTO> UpdateCountry (UpdateCountryParametersDTO inputParams) {
            if (ModelState.IsValid) {

                var countryUpdate = this.sbsReportService.UpdateCountry (inputParams.gafiId, inputParams.countryGafiName, inputParams.status, inputParams.regUser, inputParams.operType);
                return countryUpdate;
            }
            return BadRequest (ModelState);
        }

        [HttpGet ("getFrequency")]
        public ActionResult GetFrequency () {
            return Ok (this.sbsReportService.GetFrequency ());
        }

        [HttpGet ("getFrequencyList")]
        public ActionResult GetFrequencyList () {
            return Ok (this.sbsReportService.GetFrequencyList ());
        }

        [HttpGet ("getFrequencyActive")]
        public ActionResult GetFrequencyActive () {
            return Ok (this.sbsReportService.GetFrequencyActive ());
        }

        [HttpPost ("updateFrequency")]
        public ActionResult<UpdateFrequencyResponseDTO> UpdateFrequency (UpdateFrequencyParametersDTO inputParams) {
            if (ModelState.IsValid) {

                var frecuencyUpdate = this.sbsReportService.UpdateFrequency (inputParams.frequencyId, inputParams.frequencyType, inputParams.startDate, inputParams.userId);
                return frecuencyUpdate;
            }
            return BadRequest (ModelState);
        }

        [HttpGet ("getProfileList")]
        public ActionResult GetProfileList () {
            return Ok (this.sbsReportService.GetProfileList ());
        }

        [HttpGet ("getUserByProfileList")]
        public ActionResult GetUserByProfileList () {
            return Ok (this.sbsReportService.GetUserByProfileList ());
        }

        [HttpPost ("getRegimeList")]
        public ActionResult<RegimeResponseDTO> GetRegimeList (RegimeParametersDTO inputParams) {
            return Ok (this.sbsReportService.GetRegimeList (inputParams));
        }

        [HttpPost ("GetGrupoxPerfilList")]
        public ActionResult<GrupoPerfilListResponseDTO> GetGrupoxPerfilList (RegimeParametersDTO inputParams) {
            return Ok (this.sbsReportService.GetGrupoxPerfilList (inputParams));
        }

        [HttpPost ("getAlertByProfileList")]
        public ActionResult<AlertByProfileResponseDTO> GetAlertByProfileList (dynamic inputParams) {
            return Ok (this.sbsReportService.GetAlertByProfileList (inputParams));
        }

        [HttpPost ("updateAlertByProfile")]
        public ActionResult<UpdateAlertByProfileResponseDTO> UpdateAlertByProfile (UpdateAlertByProfileParametersDTO inputParams) {
            if (ModelState.IsValid) {

                var alertByProfileUpdated = this.sbsReportService.UpdateAlertByProfile (inputParams.profileId, inputParams.regimeId, inputParams.alertId, inputParams.alertStatus);
                return alertByProfileUpdated;
            }
            return BadRequest (ModelState);
        }

        [HttpPost ("getUsersByProfile")]
        public ActionResult<UsersByProfileResponseDTO> GetUsersByProfile (UsersByProfileParametersDTO inputParams) {
            return Ok (this.sbsReportService.GetUsersByProfile (inputParams));
        }

        [HttpPost ("processInsertFile")]
        public IActionResult processInsertFile (dynamic inputParams) {
            return Ok (this.sbsReportService.processInsertFile (inputParams));
        }

        [HttpPost ("processCargaFile")]
        public IActionResult processCargaFile (dynamic inputParams) {
            return Ok (this.sbsReportService.processCargaFile (inputParams));
        }

        [HttpPost ("processPagosManuales")]
        public IActionResult processPagosManuales (dynamic inputParams) {
            return Ok (this.sbsReportService.processPagosManuales (inputParams));
        }

        [HttpPost ("copyFilePaste")]
        //public IActionResult copyFilePaste (dynamic inputParams) {
        //    return Ok (this.sbsReportService.copyFilePaste (inputParams));
        //}

        [HttpPost ("convertToTxt")]
        public IActionResult convertToTxt (dynamic inputParams) {
            return Ok (this.sbsReportService.convertToTxt (inputParams));
        }


        [Route("getPromedioTipoCambio")]
        [HttpPost]
        public IActionResult getPromedioTipoCambio(dynamic param)
        {
            DateTime _date;
            bool istrue = DateTime.TryParse(param.date.ToString(), out _date);

            return Ok(this.sbsReportService.getPromedioTipoCambio(_date.AddMonths(-1)));

        }

        [Route("getValidateRUC2")]
        [HttpPost]
        public IActionResult getValidateRUC2(dynamic param)
        {

            return Ok(this.sbsReportService.getValidateRUC2(param.dni));

        }
    }
}
