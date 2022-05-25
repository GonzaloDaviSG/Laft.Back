using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using protecta.laft.api.DTO;
using protecta.laft.api.Services;
using System.Reflection;
using System.Threading.Tasks;
using log4net;

namespace protecta.laft.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaisController: ControllerBase
    {
        private PaisService service;
        public PaisController()
        {
            this.service = new PaisService();
        }

        // GET api/pais
        [HttpGet]
        public ActionResult<List<MaestroDTO>> Get()
        {
            return this.service.GetAll();
        }

        // GET api/pais/5
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