using protecta.laft.api.Models;
using protecta.laft.api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace protecta.laft.api.Services
{
    public class Colaborador : ExcelService
    {
        public void prepareData(ExcelEntity item)
        {
            columns = new List<string>() { "NTIPO_DOCUMENTO", "SNUM_DOCUMENTO", "SNOM_COMPLETO", "DFECHA_NACIMIENTO", "NACIONALIDAD" };
            Dictionary<string, dynamic> response = new Dictionary<string, dynamic>();
            
            response = ValidateHeadBoard(item);
            if (response["CODIGO"] == 1) {
                response = ValidateBody(item);
            }
        }
    }
}

