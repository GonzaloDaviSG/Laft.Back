using System.Collections.Generic;
using protecta.laft.api.DTO;
using protecta.laft.api.Models;

namespace protecta.laft.api.Repository.Interfaces
{
    public interface ILoginRepository
    {
         userResponseDTO ValExistUser(string username,string password);
    }
}