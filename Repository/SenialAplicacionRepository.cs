using System;
using System.Linq;
using System.Collections.Generic;
using protecta.laft.api.Models;

namespace protecta.laft.api.Repository
{
    public class SenialAplicacionRepository
    {
        private DB.ApplicationDbContext context;
        public SenialAplicacionRepository()
        {
            this.context = new DB.ApplicationDbContext(DB.ApplicationDB.UsarPrincipal());
        }

        public List<SenialAplicacion> GetAll()
        {
            try{
                return this.context.SenialAplicaciones.ToList();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new List<SenialAplicacion>();
            }
        }
        public SenialAplicacion Get(int Id)
        {
            try{
                return this.context.SenialAplicaciones.Find(Id);
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new SenialAplicacion();
            }
        }

        public void Add(SenialAplicacion model){
            try{
                this.context.SenialAplicaciones.Add(model);
                this.context.SaveChanges();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
            }
        }
        public void Update(SenialAplicacion model)
        {
            try{
                this.context.SenialAplicaciones.Update(model);
                this.context.SaveChanges();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
            }
        }

    }
}