using StreakyAPi.Model;
using StreakyAPi.Model.Auth;
using StreakyAPi.Model.Reponses;
using StreakyAPi.Model.Responses;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace StreakyFrontWeb.API
{
    public class StreakyAPI
    {
        private readonly HttpClient _api;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StreakyAPI(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _api = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddAuthTokenHeader()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("AuthToken");
            if (!string.IsNullOrEmpty(token))
            {
                _api.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<string> Login(string email, string password)
        {
            var response = await _api.PostAsJsonAsync("/auth/login", new LoginRequest { Email = email, Password = password });

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<UserLoginResponse>();
                var token = tokenResponse?.Token;

                // Store token in session
                _httpContextAccessor.HttpContext.Session.SetString("AuthToken", token);

                return token;
            }
            return string.Empty;
        }

        public async Task<List<BusinessResponse>> GetAllBusinesses()
        {
            AddAuthTokenHeader();
            var response = await _api.GetAsync("/business/getBusinesses");
            if (response.IsSuccessStatusCode)
            {
                var businesses = await response.Content.ReadFromJsonAsync<List<BusinessResponse>>();
                return businesses;
            }
            return null;
        }

        public async Task<List<Category>> GetCategories()
        {
            AddAuthTokenHeader();
            var response = await _api.GetAsync("/auth/categories");
            if (response.IsSuccessStatusCode)
            {
                var categories = await response.Content.ReadFromJsonAsync<List<Category>>();
                return categories;
            }
            return null;
        }

        public async Task<List<LocationResponse>> GetLocations()
        {
            AddAuthTokenHeader();
            var response = await _api.GetAsync("/business/getAllLocations");
            if (response.IsSuccessStatusCode)
            {
                var locations = await response.Content.ReadFromJsonAsync<List<LocationResponse>>();
                return locations;
            }
            return null;
        }

        public async Task<bool> AddBusiness(MultipartFormDataContent content)
        {
            try
            {
                AddAuthTokenHeader();
                var response = await _api.PostAsync("/business/business", content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch
            {
                // Handle exception if necessary
            }
            return false;
        }
    }
}
