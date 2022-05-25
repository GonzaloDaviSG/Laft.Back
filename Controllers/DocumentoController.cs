using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using protecta.laft.api.DTO;
using protecta.laft.api.Services;

namespace protecta.laft.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentoController: ControllerBase
    {
        private DocumentoService service;

        public DocumentoController()
        {
            this.service = new DocumentoService();
        }

        // GET api/documento
        [HttpGet]
        public ActionResult<List<MaestroDTO>> Get()
        {
            return this.service.GetAll();
        }

        // GET api/documento/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var dto = this.service.Get(id);
            if(dto == null || dto.id == 0){
                return NotFound();
            }
            return Ok(dto);
        }

        [HttpPut]
        public IActionResult Put([FromBody] MaestroDTO dto)
        {
            if(ModelState.IsValid){
                dto = this.service.Add(dto);
                return Ok(dto);
            }

            return BadRequest(ModelState);
        }

    }
}