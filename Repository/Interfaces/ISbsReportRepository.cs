using System.Collections.Generic;
using System;
using protecta.laft.api.DTO;
using protecta.laft.api.Models;

namespace protecta.laft.api.Repository.Interfaces {
    public interface ISbsReportRepository {
        ExchangeRateResponseDTO GetExchangeRate ();
        SbsReportGenResponseDTO GenerateSbsReport (int operType, decimal exchangeType, int ammount, string startDate, string endDate, string nameReport, string sbsFileType);
        UserDataResponseDTO GetUser (int userId);
        //List<SbsReport> GetListReports (DateTime startD, DateTime endD, string report, string search);
        List<SbsReportFileResponseDTO> GetReport (string id,int tipo_archivo);

    }
}