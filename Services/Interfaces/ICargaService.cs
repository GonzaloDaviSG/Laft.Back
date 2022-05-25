using System.Collections.Generic;
using protecta.laft.api.DTO;
namespace protecta.laft.api.Services.Interfaces
{
    public interface ICargaService
    {
        List<CargaDTO> GetAll();
        CargaDTO Get(int id);
        CargaDTO Add(CargaDTO dto);
        CargaDTO Update(CargaDTO dto);
    }
}