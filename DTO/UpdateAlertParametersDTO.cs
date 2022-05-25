namespace protecta.laft.api.DTO {
    public class UpdateAlertParametersDTO {

        public int alertId { get; set; }
        public string alertName { get; set; }
        public string alertDescription { get; set; }
        public string alertStatus { get; set; }
        public int userId { get; set; }
        public int bussinessDays { get; set; }
        public string reminderSender { get; set; }
        public string operType { get; set; }

        public int idgrupo  { get; set; }

        public string regimenSim  { get; set; }

        public string regimenGen  { get; set; }
    }
}