using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using protecta.laft.api.Models;
namespace protecta.laft.api.Repository
{
    public class PaisRepository: Interfaces.IMaestroRepository<Pais>
    {
        private DB.ApplicationDbContext context;
        public PaisRepository()
        {
            this.context = new DB.ApplicationDbContext(DB.ApplicationDB.UsarPrincipal());
        }

        public List<Pais> GetAll()
        {
            try{
                return this.context.Paises.ToList();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new List<Pais>();
            }
        }
        public Pais Get(int Id)
        {
            try{
                return this.context.Paises.Find(Id);
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new Pais();
            }
        }
    }
}