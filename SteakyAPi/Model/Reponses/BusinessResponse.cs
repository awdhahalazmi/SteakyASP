using StreakyAPi.Model.Reponses;

namespace StreakyAPi.Model.Responses
{
    public class BusinessResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Image { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public string WrongAnswer1 { get; set; }
        public string WrongAnswer2 { get; set; }
        public string Question2 { get; set; }
        public string CorrectAnswerQ2 { get; set; }
        public string WrongAnswerQ2_1 { get; set; }
        public string WrongAnswerQ2_2 { get; set; }
        public ICollection<LocationResponse> Locations { get; set; } = new List<LocationResponse>();

        public string CategoryName { get; set; }
    }
}