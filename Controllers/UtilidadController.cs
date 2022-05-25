using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using protecta.laft.api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace protecta.laft.api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UtilidadController : Controller
    {
        UtilidadService utilidadService;
        public UtilidadController()
        {
            utilidadService = new UtilidadService();
        }
        [HttpGet("generatePdfDemanda")]
        public IActionResult generatePdfDemanda()
        {
            try
            {
                utilidadService.getsDocuments();
                return Ok("");
            }
            catch
            {
                return View();
            }
        }
        [Route("gettestque")] /**/
        [HttpGet]
        public IActionResult gettestque()
        {
            return Ok(this.utilidadService.gettestque());
        }
    }
}

