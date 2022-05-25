using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using protecta.laft.api.DTO;
using protecta.laft.api.Services;

namespace protecta.laft.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SenialController: ControllerBase
    {
        private SenialService service;

        public SenialController()
        {
            this.service = new SenialService();
        }

        // GET api/senial
        [HttpGet]
        public ActionResult<List<SenialDTO>> Get()
        {
            return this.service.GetAll();
        }

        // GET api/senial/5
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
        public IActionResult Put([FromBody] List<SenialDTO> dto)
        {
            if(ModelState.IsValid){
                dto = this.service.SaveSenial(dto);
                return Ok(dto);
            }
            return BadRequest(ModelState);
        }

        [Route("getGafiByParams")]
        [HttpPost]
        public ActionResult getGafiByParams(AlertGafiDTO dto){
            
            var resp  = this.service.getGafiByParams(dto);
            return Ok(resp);
        }

        [Route("getNCByParams")]
        [HttpPost]
        public ActionResult getNCByParams(AlertNCDTORequest dto){
            
            var resp  = this.service.getNCByParams(dto);
            return Ok(resp);
        }

        [Route("getClientsS2ByParams")]
        [HttpPost]
        public ActionResult getClientsS2ByParams(ClientAlertS2ReqDTO dto){
            
            var resp  = this.service.getClientsS2ByParams(dto);
            return Ok(resp);
        }
        
        [Route("getClientsRG4ByParams")]
        [HttpPost]
        public ActionResult getClientsRG4ByParams(ClientAlertRG4ReqDTO dto){
            
            var resp  = this.service.getClientsRG4ByParams(dto);
            return Ok(resp);
        }
    }
}
