using System;
using System.Linq;
using System.Collections.Generic;
using protecta.laft.api.Models;

namespace protecta.laft.api.Repository
{
    public class SenialAplicacionProductoRepository
    {
        private DB.ApplicationDbContext context;
        public SenialAplicacionProductoRepository()
        {
            this.context = new DB.ApplicationDbContext(DB.ApplicationDB.UsarPrincipal());
        }

        public List<SenialAplicacionProducto> GetAll()
        {
            try{
                return this.context.SenialAplicacionProductos.ToList();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new List<SenialAplicacionProducto>();
            }
        }
        public SenialAplicacionProducto Get(int Id)
        {
            try{
                return this.context.SenialAplicacionProductos.Find(Id);
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new SenialAplicacionProducto();
            }
        }

        public void Add(SenialAplicacionProducto model){
            try{
                this.context.SenialAplicacionProductos.Add(model);
                this.context.SaveChanges();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
            }
        }
        public void Update(SenialAplicacionProducto model)
        {
            try{
                this.context.SenialAplicacionProductos.Update(model);
            this.context.SaveChanges();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
            }
        }
    }
}