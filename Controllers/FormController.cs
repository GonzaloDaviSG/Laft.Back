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
    public class FormController : ControllerBase {

        private FormsService formsService;

        public FormController () {
            this.formsService = new FormsService ();
        }

        //Servicio para obtener los datos de un usuario
        [HttpGet ("getFormsList")]
        public ActionResult<List<FormsResponseDTO>> GetFormsList () {
            return this.formsService.GetFormsList ();
        }
    }
}