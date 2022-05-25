using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using protecta.laft.api.Models;
namespace protecta.laft.api.Repository
{
    public class ProductoRepository: Interfaces.IMaestroRepository<Producto>
    {
        private DB.ApplicationDbContext context;
        public ProductoRepository()
        {
            this.context = new DB.ApplicationDbContext(DB.ApplicationDB.UsarPrincipal());
        }

        public List<Producto> GetAll()
        {
            try{
                return this.context.Productos.ToList();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new List<Producto>();
            }
        }
        public Producto Get(int Id)
        {
            try{
                return this.context.Productos.Find(Id);

            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new Producto();
            }
        }
    }
}