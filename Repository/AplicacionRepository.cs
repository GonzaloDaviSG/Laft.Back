using System;
using System.Linq;
using System.Collections.Generic;
using protecta.laft.api.Models;

namespace protecta.laft.api.Repository
{
    public class AplicacionRepository: Interfaces.IMaestroRepository<Aplicacion>
    {
        private DB.ApplicationDbContext context;
        public AplicacionRepository()
        {
            this.context = new DB.ApplicationDbContext(DB.ApplicationDB.UsarPrincipal());
        }

        public List<Aplicacion> GetAll()
        {
            try{
                return this.context.Aplicaciones.ToList();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new List<Aplicacion>();
            }
        }
        public Aplicacion Get(int Id)
        {
            try{
                return this.context.Aplicaciones.Find(Id);
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new Aplicacion();
            }
        }
    }
}