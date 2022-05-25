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
    public class HistoriaController: ControllerBase
    {
        private HistoriaService historiaService;

        public HistoriaController()
        {
            this.historiaService = new HistoriaService();
        }

        // GET api/historia
        [HttpGet("carga/{id}")]
        public ActionResult<List<HistoriaRegistroDTO>> GetByCarga(int id)
        {
            return this.historiaService.GetByCarga(id);
        }

        [HttpGet("registro/{id}")]
        public ActionResult<List<HistoriaRegistroDTO>> GetByRegistro(int id)
        {
            return this.historiaService.GetByRegistro(id);
        }
       
    }
}