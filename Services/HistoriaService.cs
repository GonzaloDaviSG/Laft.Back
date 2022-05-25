using System;
using System.Collections.Generic;
using System.Linq;
using protecta.laft.api.Models;
using protecta.laft.api.DTO;
using protecta.laft.api.Repository;

namespace protecta.laft.api.Services
{
    public class HistoriaService
    {
        HistoriaRegistrosRepository repository;

        public HistoriaService()
        {
            this.repository = new HistoriaRegistrosRepository();
        }

        public List<HistoriaRegistroDTO> GetByCarga(int idCarga)
        {
            try
            {
                return Utils.Parse.dtos(this.repository.GetByCarga(idCarga));
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<HistoriaRegistroDTO>();
            }
        }

        public List<HistoriaRegistroDTO> GetByRegistro(int idRegistro)
        {
            try
            {
                return Utils.Parse.dtos(this.repository.GetByRegistro(idRegistro));
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<HistoriaRegistroDTO>();
            }
        }

    }
}