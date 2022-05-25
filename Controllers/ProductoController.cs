using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using protecta.laft.api.DTO;
using protecta.laft.api.Services;

namespace protecta.laft.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController: ControllerBase
    {
        private ProductoService service;

        public ProductoController()
        {
            this.service = new ProductoService();
        }

        // GET api/producto
        [HttpGet]
        public ActionResult<List<MaestroDTO>> Get()
        {
            return this.service.GetAll();
        }

        // GET api/producto/5
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