using System;
using System.Linq;
using System.Collections.Generic;
using protecta.laft.api.Models;

namespace protecta.laft.api.Repository
{
    public class CargaRepository: Interfaces.ICargaRepository
    {
        private DB.ApplicationDbContext context;
        public CargaRepository()
        {
            this.context = new DB.ApplicationDbContext(DB.ApplicationDB.UsarPrincipal());
        }

        public List<Carga> GetAll()
        {
            try{
                return this.context.Cargas.ToList();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new List<Carga>();
            }
        }

        public Carga Get(int id)
        {
            try{
                return this.context.Cargas.Find(id);
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new Carga();
            }
        }

        public void Add(Carga model){
            try{
                this.context.Cargas.Add(model);
                this.context.SaveChanges();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
            }
        }

        public void Update(Carga model){
            try{
                this.context.Cargas.Update(model);
                this.context.SaveChanges();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
            }
        }
        
    }
}