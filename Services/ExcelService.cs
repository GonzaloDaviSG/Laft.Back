using protecta.laft.api.Models;
using protecta.laft.api.Services.Interfaces;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace protecta.laft.api.Services
{
    public abstract class ExcelService //: IExcelService
    {
        public List<string> columns { get; set; }
        string Ruta = "C:/archivos";
        int valueHead = 1;
        public  Dictionary<string, dynamic> ValidateHeadBoard(ExcelEntity item)
        {
            Dictionary<string, dynamic> response = new Dictionary<string, dynamic>();
            string filePath = this.Ruta + "/" + item.rutaExcel;
            SLDocument sl = new SLDocument(filePath);
            //validate Excel Header
            try
            {
                for (int i = 1; i <= this.columns.Count; i++)
                {
                    if (sl.GetCellValueAsString(this.valueHead, i).ToUpper() == columns[i - 1])
                    {
                        response["CODIGO"] = 2;
                        response["FILA"] = this.valueHead;
                        response["MENSAJE"] = "No tiene la cabecera " + this.columns[i] + " en la fila " + this.valueHead;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                sl.Dispose();
                response["CODIGO"] = 2;
                response["FILA"] = this.valueHead;
                response["MENSAJE"] = "Ocurrio un error en la validacion de la cabecera al leer el archivo.";
                return response;
            }
            finally
            {
                sl.Dispose();
            }
            return response;
        }
        public Dictionary<string, dynamic> ValidateBody(ExcelEntity item)
        {
            Dictionary<string, dynamic> response = new Dictionary<string, dynamic>();
            string filePath = this.Ruta + "/" + item.rutaExcel;
            SLDocument sl = new SLDocument(filePath);
            //validate Excel body
            try
            {
                for (int i = 0; i < this.columns.Count; i++)
                {
                    while (!string.IsNullOrEmpty(sl.GetCellValueAsString(valor, 1)))
                    {
                        Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();

                        item["NTIPO_DOCUMENTO"] = sl.GetCellValueAsString(valor, 1).ToUpper().Trim();
                        item["SNUM_DOCUMENTO"] = sl.GetCellValueAsString(valor, 2).ToUpper().Trim();
                        item["SNOM_COMPLETO"] = sl.GetCellValueAsString(valor, 3).ToUpper().Trim();
                        item["DFECHA_NACIMIENTO"] = sl.GetCellValueAsDateTime(valor, 4).ToString("dd/MM/yyyy");
                        item["NACIONALIDAD"] = sl.GetCellValueAsString(valor, 5).ToUpper().Trim();

                        lista.Add(item);
                        valor++;
                    }
                }
            }
            catch (Exception)
            {
                sl.Dispose();
                response["CODIGO"] = 2;
                response["FILA"] = this.valueHead;
                response["MENSAJE"] = "Ocurrio un error al obtener los datos del archivo.";
                return response;
            }
            finally {
                sl.Dispose();
            }

            return response;
        }
    }
}
