using System;
using System.Linq;
using System.Collections.Generic;
using protecta.laft.api.Models;

namespace protecta.laft.api.Repository
{
    public class RegistroAplicacionRepository
    {
        private DB.ApplicationDbContext context;
        public RegistroAplicacionRepository()
        {
            this.context = new DB.ApplicationDbContext(DB.ApplicationDB.UsarPrincipal());
            this.context.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public List<RegistroAplicacion> GetAll()
        {
            try{
                return this.context.RegistroAplicaciones.ToList();

            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new List<RegistroAplicacion>();
            }
        }
        public RegistroAplicacion Get(int Id)
        {
            try{
                return this.context.RegistroAplicaciones.Find(Id);
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new RegistroAplicacion();
            }
        }

        public void Add(RegistroAplicacion model){
            try{
                this.context.RegistroAplicaciones.AddRange(model);
                this.context.SaveChanges();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
            }
        }
        public void Update(RegistroAplicacion model)
        {
            try{
                this.context.RegistroAplicaciones.Update(model);
                this.context.SaveChanges();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
            }
        }
    }
}