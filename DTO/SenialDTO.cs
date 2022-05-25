namespace protecta.laft.api.DTO
{
    public class SenialDTO
    {
        public int id { get; set; }
        public string descripcion{get;set;}
        public string color{get; set;}
        public string fechaRegistro{get;set;}
        public string usuario{get; set;}
        public bool activo{get;set;}
        public bool indAlert {get;set;}
        public bool indError {get;set;}
    }
}