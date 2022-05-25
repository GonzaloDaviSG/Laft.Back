using System;
using System.Collections.Generic;
using System.Linq;
using protecta.laft.api.Models;
using protecta.laft.api.DTO;


namespace protecta.laft.api.Utils
{
    public class ExceptionManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ExceptionManager));
        public static void resolve(Exception ex){
            log.Error("---------------------------------------------------------------------------");
            log.Error("Mensaje:: " + ex.Message);
            log.Error("Origen:: " + ex.Source);
            log.Error("Detalle::",ex);
        }

    }
}