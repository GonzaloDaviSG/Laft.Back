using System;
using System.Collections.Generic;
using System.Linq;
using protecta.laft.api.DTO;
using protecta.laft.api.Repository;
using protecta.laft.api.Models;
using Newtonsoft.Json;

namespace protecta.laft.api.Services
{
    public class ConfigService
    {
        SenialAplicacionRepository senialAplicacionRepository;
        SenialAplicacionProductoRepository senialAplicacionProductoRepository;
        RegistroAplicacionRepository registroAplicacionRepository;
        RegistroAplicacionProductoRepository registroAplicacionProductoRepository;
        SenialRepository senialRepository;
        AplicacionRepository aplicacionRepository;
        ProductoRepository productoRepository;
        ConfigRepository configRepository;

        public ConfigService()
        {
            this.senialAplicacionRepository = new SenialAplicacionRepository();
            this.senialAplicacionProductoRepository = new SenialAplicacionProductoRepository();
            this.registroAplicacionRepository = new RegistroAplicacionRepository();
            this.registroAplicacionProductoRepository = new RegistroAplicacionProductoRepository();
            this.senialRepository = new SenialRepository();
            this.aplicacionRepository = new AplicacionRepository();
            this.productoRepository = new ProductoRepository();
            this.configRepository = new ConfigRepository();
        }

        public List<ConfigSenialDTO> GetConfigSenial()
        {
            try
            {
                List<ConfigSenialDTO> dto = new List<ConfigSenialDTO>();

                foreach (var senial in this.senialRepository.GetAll())
                {
                    dto.Add(this.GetConfigSenial(senial.nId));
                }

                return dto;
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<ConfigSenialDTO>();
            }
        }
        
        internal List<ResourceProfileHistoryDTO> ListResourceProfileHistory(ResourceProfileRequestDTO dto)
        {
            List<ResourceProfileHistoryDTO> items = null;
            try
            {
                items = this.configRepository.ListResourceProfileHistory(dto);
            }
            catch (Exception)
            {
                throw;
            }
            return items;
        }
        internal List<ResourceProfileHistoryDTO> UpdateResourceProfile(ResourceProfileParametersDTO dto)
        {
            List<ResourceProfileHistoryDTO> items = null;
            try
            {
                dto.items2 = new List<Data2>();
                dto.items2 = JsonConvert.DeserializeObject<List<Data2>>(dto.items);
                items = this.configRepository.UpdateResourceProfile(dto);
            }
            catch (Exception)
            {
                throw;
            }
            return items;
        }
        public List<ResourceProfileDTO> ListResourceProfile(ResourceProfileRequestDTO dto)
        {
            List<ResourceProfileDTO> items = null;
            try
            {
                items = this.configRepository.ListResourceProfile(dto);
            }
            catch (Exception)
            {
                throw;
            }
            return items;
        }

        public ConfigSenialDTO GetConfigSenial(int idSenial)
        {
            try
            {
                ConfigSenialDTO dto = new ConfigSenialDTO();
                var aplicaciones = this.aplicacionRepository.GetAll();
                var productos = this.productoRepository.GetAll();
                var senialesAplicacionProducto = this.senialAplicacionProductoRepository.GetAll();
                dto.senial = Utils.Parse.dto(this.senialRepository.Get(idSenial));

                dto.aplicaciones = Utils.Parse.dtos(this.senialAplicacionRepository.GetAll().Where(t => t.nIdSenial == idSenial).ToList());

                foreach (var appDTO in dto.aplicaciones)
                {
                    appDTO.aplicacion = Utils.Parse.dto(aplicaciones.Where(t => t.nId == appDTO.aplicacion.id).FirstOrDefault());
                    appDTO.productos = Utils.Parse.dtos(senialesAplicacionProducto.Where(t => t.nIdSenialApp == appDTO.id).ToList());

                    foreach (var prod in appDTO.productos)
                    {
                        prod.producto = Utils.Parse.dto(productos.Where(t => t.nId == prod.producto.id).FirstOrDefault());
                    }
                }

                return dto;
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new ConfigSenialDTO();
            }

        }

        public ConfigRegistroDTO GetConfigRegistro(int idRegistro)
        {
            try
            {
                ConfigRegistroDTO dto = new ConfigRegistroDTO();
                var aplicaciones = this.aplicacionRepository.GetAll();
                var productos = this.productoRepository.GetAll();
                var registrosAplicacionProducto = this.registroAplicacionProductoRepository.GetAll();

                dto.idRegistro = idRegistro;
                dto.aplicaciones = Utils.Parse.dtos(this.registroAplicacionRepository.GetAll().Where(t => t.nIdRegistro == idRegistro).ToList());

                foreach (var appDTO in dto.aplicaciones)
                {
                    appDTO.aplicacion = Utils.Parse.dto(aplicaciones.Where(t => t.nId == appDTO.aplicacion.id).FirstOrDefault());
                    appDTO.productos = Utils.Parse.dtos(registrosAplicacionProducto.Where(t => t.nIdRegistroApp == appDTO.id).ToList());

                    foreach (var prod in appDTO.productos)
                    {
                        prod.producto = Utils.Parse.dto(productos.Where(t => t.nId == prod.producto.id).FirstOrDefault());
                    }
                }

                return dto;

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new ConfigRegistroDTO();
            }
        }


        public List<ConfigSenialDTO> SaveConfigSenial(List<ConfigSenialDTO> dtos)
        {
            try
            {

                List<ConfigSenialDTO> config = new List<ConfigSenialDTO>();

                foreach (var dto in dtos)
                {
                    List<SenialAplicacion> senialesApp = Utils.Parse.models(dto);

                    foreach (var model in senialesApp)
                    {
                        model.dRegistro = DateTime.Now;
                        if (model.nId > 0)
                        {
                            this.senialAplicacionRepository.Update(model);
                        }
                        else
                        {
                            this.senialAplicacionRepository.Add(model);
                        }
                    }

                    foreach (var app in dto.aplicaciones)
                    {
                        List<SenialAplicacionProducto> aplicacionProductos = Utils.Parse.models(app);
                        foreach (var model in aplicacionProductos)
                        {
                            model.dRegistro = DateTime.Now; 
                            if (model.nId > 0)
                            {
                                this.senialAplicacionProductoRepository.Update(model);
                            }
                            else
                            {
                                this.senialAplicacionProductoRepository.Add(model);
                            }
                        }
                    }
                }

                return dtos;
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<ConfigSenialDTO>();
            }

        }


        public ConfigRegistroDTO SaveConfigRegistro(ConfigRegistroDTO dto)
        {
            try
            {
                ConfigRegistroDTO config = new ConfigRegistroDTO();

                foreach (var appDTO in dto.aplicaciones)
                {
                    RegistroAplicacion model = Utils.Parse.model(appDTO);
                    model.nIdRegistro = dto.idRegistro;

                    if (model.nId > 0)
                    {
                        this.registroAplicacionRepository.Update(model);
                    }
                    else
                    {
                        this.registroAplicacionRepository.Add(model);
                    }



                    foreach (var prodDTO in appDTO.productos)
                    {
                        RegistroAplicacionProducto modelProducto = Utils.Parse.model(prodDTO);
                        modelProducto.nIdRegistroApp = model.nId;

                        if (modelProducto.nId > 0)
                        {
                            this.registroAplicacionProductoRepository.Update(modelProducto);
                        }
                        else
                        {
                            this.registroAplicacionProductoRepository.Add(modelProducto);
                        }

                    }
                }


                return config;
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new ConfigRegistroDTO();
            }


        }

    }
}