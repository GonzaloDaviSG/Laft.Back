using System;
using System.Collections.Generic;
using System.Linq;
using protecta.laft.api.Models;
using protecta.laft.api.DTO;
using protecta.laft.api.Repository;

namespace protecta.laft.api.Services
{
    public class RegistroService : Interfaces.IRegistroService
    {
        private CargaRepository Cargarepository;
        private RegistroRepository repository;
        private PaisRepository paisRepository;
        private SenialRepository senialRepository;
        private DocumentoRepository documentoRepository;
        private PersonaRepository personaRepository;
        private ConfigService configService;

        private List<Pais> paises;
        private List<Documento> documentos;
        private List<Persona> personas;
        private List<Senial> seniales;

        private CargaDTO CargaActiva;


        public RegistroService()
        {
            this.repository = new RegistroRepository();
            this.paisRepository = new PaisRepository();
            this.senialRepository = new SenialRepository();
            this.documentoRepository = new DocumentoRepository();
            this.personaRepository = new PersonaRepository();
            this.paises = this.paisRepository.GetAll();
            this.documentos = this.documentoRepository.GetAll();
            this.personas = this.personaRepository.GetAll();
            this.seniales = this.senialRepository.GetAll();
            this.configService = new ConfigService();
            this.Cargarepository = new CargaRepository();
        }

        public List<CargaDTO> GetAllCarga()
        {
            try
            {
                return Utils.Parse.dtos(this.Cargarepository.GetAll());
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<CargaDTO>();
            }
        }
        public CargaDTO GetActivo()
        {
            try
            {
                var activo = this.GetAllCarga().Where(t => t.activo == true).FirstOrDefault();
                return activo;

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new CargaDTO();
            }
        }


        public List<RegistroDTO> GetAll()
        {
            try
            {
                return this.loadsMaestros(this.repository.GetAll());
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<RegistroDTO>();
            }
        }

        public RegistroDTO Get(int id)
        {
            try
            {
                return this.loadConfiguracion(this.loadMaestros(this.repository.Get(id)));
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new RegistroDTO();
            }
        }

        public List<RegistroDTO> GetByCarga(int id)
        {
            try
            {
                return this.loadsMaestros(this.repository.GetAll().Where(t => t.nIdCarga == id).ToList());
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<RegistroDTO>();
            }
        }

        private List<RegistroDTO> loadsMaestros(List<Registro> from)
        {
            try
            {
                List<RegistroDTO> to = new List<RegistroDTO>();

                if (from != null)
                {
                    foreach (var model in from)
                    {
                        to.Add(this.loadMaestros(model));
                    }
                }

                return to;
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<RegistroDTO>();
            }
        }

        private RegistroDTO loadMaestros(Registro from)
        {
            try
            {
                RegistroDTO to = new RegistroDTO();
                to = Utils.Parse.dto(from);

                var pais = this.paises.Where(r => r.nId == from.nIdPais).FirstOrDefault();
                var documento = this.documentos.Where(r => r.nId == from.nIdDocumento).FirstOrDefault();
                var persona = this.personas.Where(r => r.nId == from.nIdPersona).FirstOrDefault();
                var senial = this.seniales.Where(r => r.nId == from.nIdSenial).FirstOrDefault();

                if (pais != null && pais.nId > 0)
                {
                    to.pais = Utils.Parse.dto(pais);
                }

                if (documento != null && documento.nId > 0)
                {
                    to.documento = Utils.Parse.dto(documento);
                }

                if (persona != null && persona.nId > 0)
                {
                    to.persona = Utils.Parse.dto(persona);
                }

                if (senial != null && senial.nId > 0)
                {
                    to.senial = Utils.Parse.dto(senial);
                }

                to.configRegistro = new ConfigRegistroDTO();
                to.configRegistro.aplicaciones = new List<ConfigAplicacionDTO>();
                to.aplicaciones = new List<ConfigAplicacionDTO>();

                return to;

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new RegistroDTO();
            }
        }

        private RegistroDTO loadConfiguracion(RegistroDTO from)
        {
            try
            {
                var configSenial = this.configService.GetConfigSenial(from.senial.id);
                var configRegistro = this.configService.GetConfigRegistro(from.id);

                from.configRegistro = configRegistro;

                List<ConfigAplicacionDTO> aplicaciones = new List<ConfigAplicacionDTO>();
                aplicaciones.AddRange(configSenial.aplicaciones.Where(t => t.activo).ToList());
                aplicaciones.AddRange(configRegistro.aplicaciones.Where(t => t.activo).ToList());
                from.aplicaciones = aplicaciones;

                return from;
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new RegistroDTO();
            }
        }
        public RegistroDTO Add(RegistroDTO dto)
        {
            CargaDTO CargaActiva = new CargaDTO();
            // var CargaActiva = this.cargaService.GetActivo();
            if (dto.idCarga == 0)
            {
                CargaActiva = this.GetActivo();
            }

            try
            {
                if (dto.idCarga == 0)
                {
                    dto.idCarga = CargaActiva.id;
                }
                dto.activo = true;
                dto.categoriaNombre = setUpper(dto.categoriaNombre);
                dto.nombre = setUpper(dto.nombre);
                dto.apellidoMaterno = setUpper(dto.apellidoMaterno);
                dto.apellidoPaterno = setUpper(dto.apellidoPaterno);
                dto.fechaCarga = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                // dto.fechaRegistro = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

                Registro model = Utils.Parse.model(dto);
                this.repository.Add(model);

                if (dto.configRegistro != null)
                {
                    dto.configRegistro.idRegistro = model.nId;
                    this.configService.SaveConfigRegistro(dto.configRegistro);
                }

                return Utils.Parse.dto(model);
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new RegistroDTO();
            }
        }

        public string setUpper(string campo)
        {
            return campo = (campo != null) ? campo.ToUpper() : "";
        }

        public RegistroDTO Update(RegistroDTO dto)
        {
            try
            {
                dto.editado = true;
                dto.nombre = setUpper(dto.nombre);
                dto.apellidoMaterno = setUpper(dto.apellidoMaterno);
                dto.apellidoPaterno = setUpper(dto.apellidoPaterno);
                if (dto.fechaCarga == null)
                {
                    dto.fechaCarga = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                }
                Registro model = Utils.Parse.model(dto);
                this.repository.Update(model);
                this.configService.SaveConfigRegistro(dto.configRegistro);
                return Utils.Parse.dto(model);
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new RegistroDTO();
            }
        }

        public List<RegistroDTO> getAll(BusquedaDTO dto)
        {
            try
            {
                var model = this.repository.getAll(Convert.ToDateTime(dto.fechaInicio), Convert.ToDateTime(dto.fechaFin),dto.opc,
                dto.tipoDoc,dto.numDoc,dto.firstname,dto.lastname,dto.lastname2);
                return this.loadsMaestros(model);
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<RegistroDTO>();
            }
        }

      
    }

}