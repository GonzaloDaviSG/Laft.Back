using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using protecta.laft.api.DTO;
using protecta.laft.api.Services;
namespace protecta.laft.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController: ControllerBase
    {
        private PersonaService service;

        public PersonaController()
        {
            this.service = new PersonaService();
        }

        // GET api/persona
        [HttpGet]
        public ActionResult<List<MaestroDTO>> Get()
        {
            return this.service.GetAll();
        }

        // GET api/persona/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var dto = this.service.Get(id);
            if(dto == null || dto.id == 0){
                return NotFound();
            }
            return Ok(dto);
        }
    }
}