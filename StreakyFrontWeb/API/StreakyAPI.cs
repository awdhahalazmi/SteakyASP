using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StreakyAPi.Model.Auth;
using StreakyAPi.Model.Reponses;

namespace StreakyFrontWeb.API
{
    public class StreakyAPI
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public StreakyAPI(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["StreakyAPI:BaseUrl"];
            _httpClient.BaseAddress = new Uri(_baseUrl);

        
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", GetTokenFromContext());
        }

        private string GetTokenFromContext()
        {
         
            return string.Empty;
        }

        public async Task<string> Login(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login",
                new LoginRequest { Email = email, Password = password });

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<UserLoginResponse>();
                var token = tokenResponse.Token;
                return token;
            }
            return string.Empty;
        }
    }
}
