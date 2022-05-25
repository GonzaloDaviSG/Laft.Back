using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using protecta.laft.api.DTO;
using protecta.laft.api.Models;
using protecta.laft.api.Services;
using protecta.laft.api.Services.Interfaces;
using protecta.laft.api.Utils;

namespace protecta.laft.api.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class MenuConfigController : ControllerBase {

        private MenuConfigService menuConfigService;

        public MenuConfigController () {

            this.menuConfigService = new MenuConfigService ();

        }

        //Servicio para obtener los datos de un usuario 
        [HttpPost ("getOptionList")]
        public ActionResult GetOptionList (MenuListParametersDTO param) {
            return Ok (this.menuConfigService.GetOptionList (param));
        }

        [HttpPost ("getSubOptionList")]
        public ActionResult GetSubOptionList (SubmenuListParametersDTO param) {
            return Ok (this.menuConfigService.GetSubOptionList (param));
        }
    }
}