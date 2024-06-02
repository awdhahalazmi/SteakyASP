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

        public async Task<string> Login(string email, string password)
        {
            var response = await _api.PostAsJsonAsync("/auth/login", new LoginRequest { Email = email, Password = password });

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<UserLoginResponse>();
                return tokenResponse?.Token;
            }
            return string.Empty;
        }
    }
}
