using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using protecta.laft.api.Models;
namespace protecta.laft.api.Repository
{
    public class DocumentoRepository: Interfaces.IMaestroRepository<Documento>
    {
        private DB.ApplicationDbContext context;
        public DocumentoRepository()
        {
            this.context = new DB.ApplicationDbContext(DB.ApplicationDB.UsarPrincipal());
        }

        public List<Documento> GetAll()
        {
            try{
                return this.context.Documentos.ToList();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new List<Documento>();
            }
        }
        public Documento Get(int Id)
        {
            try{
                return this.context.Documentos.Find(Id);

            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new Documento();
            }
        }

         public void Add(Documento documento)
        {
            try{
               this.context.Documentos.AddRange(documento);
               this.context.SaveChanges();
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
            }
        }

    }
}