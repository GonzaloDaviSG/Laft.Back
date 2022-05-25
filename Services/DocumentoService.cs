using System;
using System.Collections.Generic;
using System.Linq;
using protecta.laft.api.DTO;
using protecta.laft.api.Repository;
namespace protecta.laft.api.Services
{
    public class DocumentoService : Interfaces.IMaestroService
    {
        DocumentoRepository repository;

        public DocumentoService()
        {
            this.repository = new DocumentoRepository();
        }

        public List<MaestroDTO> GetAll()
        {
            try
            {
                return Utils.Parse.dtos(this.repository.GetAll());

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new List<MaestroDTO>();
            }
        }
        public MaestroDTO Get(int id)
        {
            try
            {
                return Utils.Parse.dto(this.repository.Get(id));

            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new MaestroDTO();
            }
        }

        public MaestroDTO Add(MaestroDTO dto)
        {
            try
            {
                dto.activo = true;
                dto.usuario = "LAFT";
                dto.fechaRegistro = DateTime.Now.ToString("dd/MM/yyyy");
                dto.descripcion = dto.descripcion.ToUpper();
                List<MaestroDTO> Lista = this.GetAll();
                var Document =Lista.Where(x => x.descripcion.ToLower() == dto.descripcion.ToLower()).ToList();
                if(Document.Count > 0){
                    dto.id = Document[0].id;
                }else{
                    var NumMax = Lista.Max(x => x.id) + 1;
                    dto.id = NumMax;
                    var model = Utils.Parse.models(dto);
                    this.repository.Add(model);
                }
                return dto;
            }
            catch (Exception ex)
            {
                Utils.ExceptionManager.resolve(ex);
                return new MaestroDTO();
            }
        }




    }
}