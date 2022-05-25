namespace protecta.laft.api.DTO
{
    public class HistoriaRegistroDTO
    {
        public int id { get; set; }
        public int secuencia { get; set; }
        public string numero { get; set; }
        public int idCarga{get; set;}
        public int idPersona{get;set;}
        public string persona{get;set;}
        public int idPais{get;set;}
        public string pais{get;set;}
        public int idSenial{get;set;}
        public string senial{get;set;}
        public int idDocumento{get;set;}
        public string documento{get;set;}
        public string numeroDocumento{get;set;}
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string nombre { get; set; }
        public string observacion { get; set; }
        public string fechaRegistro { get; set; }
        public string usuario { get; set; }
        public bool activo {get;set;}
        public string usuarioDB { get; set; }
        public string usuarioPC { get; set; }
        public string ip { get; set; }
        public string host { get; set; }
        public string fechaRegistroHistoria { get; set; }
        public string tipoHisoria { get; set; }

    }
}