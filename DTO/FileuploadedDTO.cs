using System;
using System.Collections.Generic;

namespace protecta.laft.api.DTO {
    public class FileuploadedDTO {

        public string dateUpload { get; set; }
        public List<FilesDTO> files { get; set; }
        public int idUser { get; set; }
       
    }
}