namespace StreakyAPi.Model.Auth
{
    public class SignUpRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public int GenderId { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>();

        public bool IsAdmin { get; set; } = false;

        

    

    }

}
