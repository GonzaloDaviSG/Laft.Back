using System.Collections.Generic;
using protecta.laft.api.DTO;
namespace protecta.laft.api.Services.Interfaces
{
    public interface IlLoginService
    {
         bool ValExistUser(string username, string password);
    
    }
}