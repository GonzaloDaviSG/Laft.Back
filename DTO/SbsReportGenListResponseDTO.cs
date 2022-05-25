namespace protecta.laft.api.DTO {
    public class SbsReportGenListResponseDTO

    {
        public string estadoReporte { get; set; }
        public string id { get; set; }
        public string fechaInicioReporte { get; set; }
        public string fechaFinReporte { get; set; }
        public decimal tipoCambio { get; set; }
        public string tipoOperacion { get; set; }
        public string origen { get; set; }
        public string fechaProcesoEjecucion { get; set; }
        public string tipoDeArchivo { get; set; }       

    }
}