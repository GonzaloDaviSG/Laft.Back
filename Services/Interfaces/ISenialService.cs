using System.Collections.Generic;
using protecta.laft.api.DTO;
namespace protecta.laft.api.Services.Interfaces
{
    public interface ISenialService
    {
         List<SenialDTO> GetAll();
        SenialDTO Get(int id);
    }
}