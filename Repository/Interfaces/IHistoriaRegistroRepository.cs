using System.Collections.Generic;
using protecta.laft.api.Models;

namespace protecta.laft.api.Repository.Interfaces
{
    public interface IHistoriaRegistroRepository
    {
         List<HistoriaRegistro> GetByCarga(int idCarga);
        List<HistoriaRegistro> GetByRegistro(int idRegistro);
    }
}