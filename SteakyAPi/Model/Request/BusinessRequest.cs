namespace StreakyAPi.Model.Request
{
    public class BusinessRequest
    {
      
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public IFormFile Image { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public string WrongAnswer1 { get; set; }
        public string WrongAnswer2 { get; set; }
        public List<int> LocationIds { get; set; } = new List<int>(); 

    }
}
