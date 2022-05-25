using System.Collections.Generic;
using protecta.laft.api.DTO;

namespace protecta.laft.api.Services.Interfaces
{
    public interface IRegistroService
    {
        List<RegistroDTO> GetAll();
        RegistroDTO Get(int id);
        RegistroDTO Add(RegistroDTO dto);
        RegistroDTO Update(RegistroDTO dto);
    }
}