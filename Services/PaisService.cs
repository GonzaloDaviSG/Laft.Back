using System;
using System.Collections.Generic;
using protecta.laft.api.DTO;
using protecta.laft.api.Repository;
namespace protecta.laft.api.Services
{
    public class PaisService: Interfaces.IMaestroService
    {
        PaisRepository repository;

        public PaisService()
        {
            this.repository = new PaisRepository();
        }

        public List<MaestroDTO> GetAll()
        {
            try{
                return Utils.Parse.dtos(this.repository.GetAll());

            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new List<MaestroDTO>();
            }
        }
        public MaestroDTO Get(int id)
        {
            try{
                return Utils.Parse.dto(this.repository.Get(id));
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new MaestroDTO();
            }
        }
    }
}