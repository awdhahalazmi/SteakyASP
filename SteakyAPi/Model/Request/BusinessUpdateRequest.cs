namespace StreakyAPi.Model.Request
{
    public class BusinessUpdateRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? CategoryId { get; set; }
        public IFormFile? Image { get; set; }
        public string? Question { get; set; }
        public string? CorrectAnswer { get; set; }
        public string? WrongAnswer1 { get; set; }
        public string? WrongAnswer2 { get; set; }
        public string? Question2 { get; set; }

        public string CorrectAnswerQ2 { get; set; }
        public string WrongAnswerQ2_1 { get; set; }
        public string WrongAnswerQ2_2 { get; set; }
        public List<int>? LocationIds { get; set; }
    }
}
