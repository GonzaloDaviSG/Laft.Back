namespace protecta.laft.api.DTO
{
    public class ConfigProductoDTO
    {
        public int id{get;set;}
        public string fechaRegistro { get; set; }
        public string usuario { get; set; }
        public bool activo {get;set;}
        public MaestroDTO producto{get;set;}
    }
}