namespace protecta.laft.api.DTO {
    public class UpdateQuestionParametersDTO {

        public int alertId { get; set; }
        
        public int questionId { get; set; }

        public int originId { get; set; }

        public string questionName { get; set; }

        public string questionStatus { get; set; }

        public int userId { get; set; }
        public string transactionType { get; set; }
        public int validComment { get; set; }

    }
}
