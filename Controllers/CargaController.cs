using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using protecta.laft.api.DTO;
using protecta.laft.api.Services;

namespace protecta.laft.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargaController: ControllerBase
    {
        private CargaService cargaService;
        private RegistroService registroService;

        public CargaController()
        {
            this.cargaService = new CargaService();
            this.registroService = new RegistroService();
        }

        // GET api/carga
        [HttpGet]
        public IActionResult Get()
        {
            var dto = this.cargaService.GetAll();
            if(dto == null){
                return NotFound();
            }
            
            return Ok(dto);
        }

        // GET api/carga/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var dto = this.cargaService.Get(id);
            if(dto == null || dto.id == 0){
                return NotFound();
            }
            return Ok(dto);
        }


        // GET api/carga/5/registros?documento=4208&&nombres=ivan calapuja
        [HttpGet("{id}/registros")]
        public IActionResult GetRegistros(int id,string documento, string nombres)
        {
            var dto = cargaService.GetRegistros(id,documento,nombres);
            if(dto == null){
                return NotFound();
            }
            return Ok(dto);
        }

        // GET api/carga/activa
        [HttpGet("activa")]
        public IActionResult GetActivo(int id)
        {
            var dto = this.cargaService.GetActivo();
            if(dto == null){
                return NotFound();
            }
            return Ok(dto);
        }

        // GET api/carga/activa/registros
        [HttpGet("activa/registros")]
        public IActionResult GetRegistrosByActiva(string documento, string nombres)
        {
            var dto = cargaService.GetRegistrosByActiva(documento,nombres);
            if(dto == null){
                return NotFound();
            }
            return Ok(dto);
        }
        

        // POST api/carga
        [HttpPost]
        public IActionResult Post([FromBody] CargaDTO dto)
        {
            if(ModelState.IsValid){
                var clientId = dto.registros.Find(x=> x.id != 0);
                dto = this.cargaService.Add(dto);
                return Ok(dto);
            }

            return BadRequest(ModelState);
        }
    }
}