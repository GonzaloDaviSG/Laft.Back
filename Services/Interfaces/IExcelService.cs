using protecta.laft.api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace protecta.laft.api.Services.Interfaces
{
    public interface IExcelService
    {
        List<string> columns { get; set }
        Dictionary<string, dynamic> ValidateHeadBoard(ExcelEntity item);
        Dictionary<string, dynamic> PrepareData(ExcelEntity item);
    }
}
