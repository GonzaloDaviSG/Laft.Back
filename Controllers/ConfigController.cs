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
    public class ConfigController : ControllerBase
    {
        private ConfigService service;

        public ConfigController()
        {
            this.service = new ConfigService();
        }

        [HttpGet("senial")]
        public ActionResult<List<ConfigSenialDTO>> GetConfigSenial()
        {
            var config = service.GetConfigSenial();
            return config;
        }

        [HttpGet("senial/{id}")]
        public ActionResult<ConfigSenialDTO> GetConfigSenial(int id)
        {
            var config = service.GetConfigSenial(id);
            return config;
        }

        [HttpGet("registro/{id}")]
        public ActionResult<ConfigRegistroDTO> GetConfigRegistro(int id)
        {
            var config = service.GetConfigRegistro(id);
            return config;
        }


        [HttpPost("senial")]
        public IActionResult Post([FromBody] List<ConfigSenialDTO> dto)
        {
            if (ModelState.IsValid)
            {
                dto = this.service.SaveConfigSenial(dto);
                return Ok(dto);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("listresourceprofile")]
        public IActionResult ListResourceProfile(ResourceProfileRequestDTO dto)
        {
            var response = this.service.ListResourceProfile(dto);
            return Ok(response);
        }

        [HttpPost("listresourceprofilehistory")]
        public IActionResult ListResourceProfileHistory(ResourceProfileRequestDTO dto)
        {
            var response = this.service.ListResourceProfileHistory(dto);
            return Ok(response);
        }
        [HttpPost("updateresourceprofile")]
        public IActionResult UpdateResourceProfile(ResourceProfileParametersDTO dto)
        { 
            var response = this.service.UpdateResourceProfile(dto);
            return Ok(response);
        }
    }
}