using System.Collections.Generic;
using protecta.laft.api.DTO;
namespace protecta.laft.api.Services.Interfaces
{
    public interface IMaestroService
    {
        List<MaestroDTO> GetAll();
        MaestroDTO Get(int id);
    }
}