using System;
using System.Linq;
using System.Collections.Generic;
using protecta.laft.api.Models;

namespace protecta.laft.api.Repository
{
    public class HistoriaRegistrosRepository: Interfaces.IHistoriaRegistroRepository
    {
        private DB.ApplicationDbContext context;

        public HistoriaRegistrosRepository()
        {
            this.context = new DB.ApplicationDbContext(DB.ApplicationDB.UsarPrincipal());
        }

        public List<HistoriaRegistro> GetByCarga(int idCarga){
            try{
                var historias = from r in this.context.HistoriaRegistros
                                where r.nIdCarga == idCarga
                                select r;
                
                return historias.ToList();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new List<HistoriaRegistro>();
            }
        }

        public List<HistoriaRegistro> GetByRegistro(int idRegistro){
            try{
                var historias = from r in this.context.HistoriaRegistros
                                where r.nId == idRegistro
                                select r;
                
                return historias.ToList();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new List<HistoriaRegistro>();
            }
        }

    }
}