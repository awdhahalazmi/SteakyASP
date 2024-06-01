namespace StreakyAPi.Model.Request
{
    public class EditProfileRequest
    {
        public string Name { get; set; }
        public int? GenderId { get; set; } 

        public IFormFile? Image { get; set; } 
    }
}
