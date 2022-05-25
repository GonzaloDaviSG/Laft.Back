namespace protecta.laft.api.DTO {
    public class AlertNotaCreditoResponseDTO {
        public int alerta {get; set;}
        public int periodo {get; set;}
        public string fecha {get; set;}
        public string producto {get; set;}
        public string motivo {get; set;}
        public string codigoCliente {get; set;}
        public string nombreCliente {get; set;}
        public string tipoDocumento {get; set;}
        public string numeroDocumento {get; set;}
        public string tipoComprobante {get; set;}
        public string numeroComprobante {get; set;}
        public decimal amount {get; set;}
        public decimal iva {get; set;}
        public decimal rightTissue {get; set;}
        public int poliza {get; set;}
        public string recibo {get; set;}

    }
}
