using System;
using System.Collections.Generic;
using System.Linq;

namespace protecta.laft.api.DTO
{
    public class userResponseDTO
    {
        public int idUsuario {get;set;}
        public string username {get;set;}
        public string fullName {get;set;}
        public int idPerfil {get;set;}
        public bool ingreso {get;set;}
        public string message {get;set;}
        public string tipoUsuario {get;set;}
    }
}
