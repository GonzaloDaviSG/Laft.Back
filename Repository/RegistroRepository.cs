using System;
using System.Linq;
using System.Collections.Generic;
using protecta.laft.api.Models;

namespace protecta.laft.api.Repository
{
    public class RegistroRepository : Interfaces.IRegistroRepository
    {
        private DB.ApplicationDbContext context;

        public RegistroRepository()
        {
            this.context = new DB.ApplicationDbContext(DB.ApplicationDB.UsarPrincipal());
            this.context.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public List<Registro> GetAll()
        {
            try
            {
                
                return this.context.Registros.ToList();
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<Registro>();
            }
        }
        public Registro Get(int Id)
        {
            try
            {
                return this.context.Registros.Find(Id);
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new Registro();
            }
        }

        public List<RegistroAplicacion> getAplicaciones(int idRegistro)
        {
            try
            {
                var aplicaciones = from r in this.context.RegistroAplicaciones
                                   where r.nIdRegistro == idRegistro
                                   select r;

                return aplicaciones.ToList();
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<RegistroAplicacion>();
            }
        }

        public List<RegistroAplicacionProducto> getProductosAplicacion(int idRegistroApp)
        {
            try
            {
                var aplicaciones = from r in this.context.RegistroAplicacionProductos
                                   where r.nIdRegistroApp == idRegistroApp
                                   select r;

                return aplicaciones.ToList();

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<RegistroAplicacionProducto>();
            }
        }

        public void Add(Registro registro)
        {
            try
            {
                this.context.Registros.AddRange(registro);
                this.context.SaveChanges();
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
            }
        }
        public void Update(Registro registro)
        {
            try
            {
                this.context.Registros.Update(registro);
                this.context.SaveChanges();
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
            }
        }

        public List<Registro> getAll(DateTime fechaIni, DateTime fechaFin, string opc, string tipoDoc, string numDoc,
        string firstname, string lastname, string lastname2)
        {
            try
            {
                if (fechaIni != null && fechaFin != null)
                {
                    if (opc == "0")
                    {
                        return this.context.Registros.Where(x => x.dRegistro >= fechaIni && x.dRegistro <= fechaFin).OrderByDescending(x=>x.dfechaCarga).ToList();
                    }
                    else if (opc == "1")
                    {
                        return this.context.Registros.Where(x => x.dRegistro >= fechaIni && x.dRegistro <= fechaFin
                        && x.nIdDocumento.ToString() == tipoDoc && x.sNumDoc == numDoc).OrderByDescending(x=>x.dfechaCarga).ToList();
                    }
                    else if (opc == "2")
                    {
                        return this.context.Registros.Where(x => x.dRegistro >= fechaIni && x.dRegistro <= fechaFin
                        && (x.sApePat + x.sApeMat  + x.sNombre).Trim().ToUpper() == (lastname+lastname2+firstname).Trim().ToUpper()).OrderByDescending(x=>x.dfechaCarga).ToList();
                    }else{
                        return this.context.Registros.OrderByDescending(x=>x.dfechaCarga).ToList();
                    }

                }
                else
                {
                    return this.context.Registros.OrderByDescending(x=>x.dfechaCarga).ToList();
                }


            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<Registro>();
            }
        }


    }
}