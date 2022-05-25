using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using protecta.laft.api.DTO;
using protecta.laft.api.Services;

namespace protecta.laft.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroController: ControllerBase
    {
        private RegistroService registroService;

        public RegistroController()
        {
            this.registroService = new RegistroService();
        }

        // GET api/registro
        [HttpGet]
        public ActionResult<List<RegistroDTO>> Get()
        {
            return this.registroService.GetAll();
        }

        // GET api/registro/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var dto = this.registroService.Get(id);
            if(dto == null || dto.id == 0){
                return NotFound();
            }
            return Ok(dto);
        }

        // GET api/registro/5/aplicaciones
        /*
        [HttpGet("{id}/aplicaciones")]
        public IActionResult GetAplicaciones(int id)
        {
            var dto = this.registroService.getAplicaciones(id);
            if(dto == null){
                return NotFound();
            }
            return Ok(dto);
        }
        

        // GET api/registro/5/aplicaciones/2/productos
        [HttpGet("{id}/aplicaciones/{idRegistroApp}/productos")]
        public IActionResult GetAplicacionesProductos(int id,int idRegistroApp)
        {
            var dto = this.registroService.getProductosAplicacion(idRegistroApp);
            if(dto == null){
                return NotFound();
            }
            return Ok(dto);
        }

*/


        // POST api/registro
        [HttpPost]
        public IActionResult Post([FromBody] RegistroDTO dto)
        {
            if(ModelState.IsValid){
                dto = this.registroService.Add(dto);
                return Ok(dto);
            }

            return BadRequest(ModelState);
        }

        // PUT api/registro
        [HttpPut]
        public IActionResult Put([FromBody] RegistroDTO dto)
        {
            if(ModelState.IsValid){
                dto = this.registroService.Update(dto);
                return Ok(dto);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("getAll")]
        public IActionResult getAll([FromBody] BusquedaDTO dto)
        {
            if(ModelState.IsValid){
               var model = this.registroService.getAll(dto);
                return Ok(model);
            }
            return BadRequest(ModelState);
        }


       /* [HttpPost("report")]
        public IActionResult getReportClient(string TipoDoc ,string documento, string nombres)
        {
            var dto = this.registroService.GetRegistrosByActiva(documento,nombres);
            if(dto == null){
                return NotFound();
            }
            return Ok(dto);
        }*/


    }
}