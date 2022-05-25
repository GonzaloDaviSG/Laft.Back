using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using protecta.laft.api.Models;
namespace protecta.laft.api.Repository
{
    public class PersonaRepository: Interfaces.IMaestroRepository<Persona>
    {
        private DB.ApplicationDbContext context;
        public PersonaRepository()
        {
            this.context = new DB.ApplicationDbContext(DB.ApplicationDB.UsarPrincipal());
        }

        public List<Persona> GetAll()
        {
            try{
                return this.context.Personas.ToList();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new List<Persona>();
            }
        }
        public Persona Get(int Id)
        {
            try{
                return this.context.Personas.Find(Id);
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new Persona();
            }
        }
    }
}