using System;
using System.Linq;
using System.Collections.Generic;
using protecta.laft.api.Models;

namespace protecta.laft.api.Repository
{
    public class RegistroAplicacionProductoRepository
    {
        private DB.ApplicationDbContext context;
        public RegistroAplicacionProductoRepository()
        {
            this.context = new DB.ApplicationDbContext(DB.ApplicationDB.UsarPrincipal());
            this.context.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public List<RegistroAplicacionProducto> GetAll()
        {
            try{
                return this.context.RegistroAplicacionProductos.ToList();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new List<RegistroAplicacionProducto>();
            }
        }
        public RegistroAplicacionProducto Get(int Id)
        {
            try{
                return this.context.RegistroAplicacionProductos.Find(Id);
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new RegistroAplicacionProducto();
            }
        }

        public void Add(RegistroAplicacionProducto model){
            try{
                this.context.RegistroAplicacionProductos.AddRange(model);
                this.context.SaveChanges();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
            }
        }
        public void Update(RegistroAplicacionProducto model)
        {
            try{
                this.context.RegistroAplicacionProductos.Update(model);
                this.context.SaveChanges();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
            }
        }
    }
}