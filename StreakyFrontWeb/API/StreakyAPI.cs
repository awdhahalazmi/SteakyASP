using StreakyAPi.Model.Auth;
using StreakyAPi.Model.Reponses;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;


namespace StreakyFrontWeb.API
{
    public class StreakyAPI
    {
        private readonly HttpClient _api;

        public StreakyAPI(HttpClient httpClient)
        {
            _api = httpClient;
        }

        public async Task<String> Login(string email, string password)
        {
            var response = await _api.PostAsJsonAsync("/api/auth/login",
                new LoginRequest { Email = email, Password = password });

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<UserLoginResponse>(); 
                var token = tokenResponse.Token;
                return token;
            }
            return "";
        }
    }
}
