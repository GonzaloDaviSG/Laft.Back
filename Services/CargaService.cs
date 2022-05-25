using System;
using System.Collections.Generic;
using System.Linq;
using protecta.laft.api.Models;
using protecta.laft.api.DTO;
using protecta.laft.api.Repository;

namespace protecta.laft.api.Services
{
    public class CargaService : Interfaces.ICargaService
    {
        private CargaRepository repository;
        private RegistroService registroService;

        public CargaService()
        {
            this.repository = new CargaRepository();
            this.registroService = new RegistroService();
        }

        public List<CargaDTO> GetAll()
        {
            try
            {
                return Utils.Parse.dtos(this.repository.GetAll());
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<CargaDTO>();
            }
        }
        public CargaDTO Get(int id)
        {
            try
            {
                var dto = Utils.Parse.dto(this.repository.Get(id));
                if (dto.id > 0)
                {
                    dto.registros = this.registroService.GetByCarga(id);
                }
                return dto;
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new CargaDTO();
            }

        }

        public CargaDTO GetActivo()
        {
            try
            {
                var activo = this.GetAll().Where(t => t.activo == true).FirstOrDefault();
                if(activo != null){
                    if (activo.id > 0)
                    {
                        activo = this.Get(activo.id);
                    }
                }

                return activo;

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new CargaDTO();
            }
        }

        public List<RegistroDTO> GetRegistros(int id, string documento, string nombres)
        {
            try
            {

                List<RegistroDTO> registros = registroService.GetByCarga(id);

                if (!String.IsNullOrWhiteSpace(documento))
                {
                    registros = registros.Where(t => !string.IsNullOrEmpty(t.numeroDocumento) && t.numeroDocumento.Contains(documento.Trim())).ToList();
                }

                if (!String.IsNullOrWhiteSpace(nombres))
                {
                    registros = registros.Where(t => (t.nombre + " " + t.apellidoPaterno + " " + t.apellidoMaterno).ToUpper().Contains(nombres.Trim().ToUpper())).ToList();
                }

                return registros;
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<RegistroDTO>();
            }

        }

        public List<RegistroDTO> GetRegistrosByActiva(string documento, string nombres)
        {
            try{
                var dto = this.GetActivo();
                return this.GetRegistros(dto.id, documento, nombres);
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new List<RegistroDTO>();
            }
        }


        public CargaDTO Add(CargaDTO dto)
        {
            try{
                dto.fechaRegistro = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                dto.activo = true;
                var model = Utils.Parse.model(dto);

                this.InactivarCargas(dto.usuario);
                this.repository.Add(model);

                if(model.nId > 0){
                    if (dto.registros != null)
                    {
                        foreach (var registroDTO in dto.registros)
                        {
                            registroDTO.idCarga = model.nId;
                            registroDTO.id = 0;
                            this.registroService.Add(registroDTO);
                        }
                    }
                }
                return Utils.Parse.dto(model);
            }
            catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new CargaDTO();
            }
        }
        public CargaDTO Update(CargaDTO dto)
        {
            try{
                var model = Utils.Parse.model(dto);
                this.repository.Update(model);
                return Utils.Parse.dto(model);
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
                return new CargaDTO();
            }
        }

        private void InactivarCargas(string usuario)
        {
            try{
                foreach (var model in this.repository.GetAll().Where(t => t.nEstado == Estado.Activo).ToList())
                {
                    model.nEstado = Estado.Inactivo;
                    model.sUsuario = usuario;
                    this.repository.Update(model);
                }
            }catch(Exception ex){
                Utils.ExceptionManager.resolve(ex);
            }
        }
    }
}