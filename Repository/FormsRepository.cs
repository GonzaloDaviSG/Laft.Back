using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using protecta.laft.api.DTO;
using protecta.laft.api.Models;

namespace protecta.laft.api.Repository {
    public class FormsRepository : Interfaces.IFormsRepository {
        private DB.ApplicationDbContext context;

        public FormsRepository () {
            this.context = new DB.ApplicationDbContext (DB.ApplicationDB.UsarOracle ());
        }

        public List<Forms> GetFormsList () {
            try {

                  return this.context.Forms.OrderBy (x => x.sSkey).ToList ();
                 
            } catch (Exception ex) {
                Utils.ExceptionManager.resolve (ex);
                return null;
            }
        }
    }
}