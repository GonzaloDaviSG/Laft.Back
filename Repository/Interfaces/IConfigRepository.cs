using protecta.laft.api.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace protecta.laft.api.Repository.Interfaces
{
    public interface IConfigRepository
    {
        List<ResourceProfileDTO> ListResourceProfile(ResourceProfileRequestDTO dto);
        List<ResourceProfileHistoryDTO> ListResourceProfileHistory(ResourceProfileRequestDTO dto);
    }
}
