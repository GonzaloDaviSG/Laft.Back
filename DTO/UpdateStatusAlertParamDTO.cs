namespace protecta.laft.api.DTO {
    public class UpdateStatusAlertParamDTO {
        public int alertId { get; set; }
        public int periodId { get; set; }
        public string status { get; set; }
        public int regimeId{ get; set; }

    }
}