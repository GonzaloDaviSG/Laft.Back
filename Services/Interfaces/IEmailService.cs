using System.Threading.Tasks;
using protecta.laft.api.DTO;

namespace protecta.laft.api.Services.Interfaces {
    public interface IEmailService {
        string SenderEmail (string user, string email, string route, string reportId, string message, string startDate, string enDate, string desReport, string desOperType, string sbsFileType);
    }
}