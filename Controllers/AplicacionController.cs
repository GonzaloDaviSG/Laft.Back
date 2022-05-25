using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using protecta.laft.api.DTO;
using protecta.laft.api.Services;

namespace protecta.laft.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AplicacionController: ControllerBase
    {
        private AplicacionService service;

        public AplicacionController()
        {
            this.service = new AplicacionService();
        }

        // GET api/aplicacion
        [HttpGet]
        public ActionResult<List<MaestroDTO>> Get()
        {
            return this.service.GetAll();
        }

        // GET api/aplicacion/5
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