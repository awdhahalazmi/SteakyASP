using StreakyAPi.Model;
using StreakyAPi.Model.Auth;
using StreakyAPi.Model.Reponses;
using StreakyAPi.Model.Request;
using StreakyAPi.Model.Responses;
using Microsoft.EntityFrameworkCore;
using StreakyAPi.Model;
using StreakyAPi.Model.Streak;
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

        public async Task<bool> EditBusiness(int id, MultipartFormDataContent content)
        {
            try
            {
                AddAuthTokenHeader();
                var response = await _api.PutAsync($"/business/editBusiness/{id}", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                // Handle exception if necessary
                return false;
            }
        }

        public async Task<BusinessResponse> GetBusinessById(int id)
        {
            AddAuthTokenHeader();
            var response = await _api.GetAsync($"/business/getBusiness/{id}");
            if (response.IsSuccessStatusCode)
            {
                var business = await response.Content.ReadFromJsonAsync<BusinessResponse>();
                return business;
            }
            return null;
        }

        public async Task<bool> DeleteBusiness(int id)
        {
            try
            {
                AddAuthTokenHeader();
                var response = await _api.DeleteAsync($"/business/deleteBusiness/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                // Handle exception if necessary
                return false;
            }
        }

        public async Task<bool> AddStreak(StreakRequest request)
        {
            AddAuthTokenHeader();
            var response = await _api.PostAsJsonAsync("/Streak/addStreak", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<StreakResponse>> GetAllStreaks()
        {
            AddAuthTokenHeader();
            var response = await _api.GetAsync("/Streak/getAllStreaks");
            if (response.IsSuccessStatusCode)
            {
                var streaks = await response.Content.ReadFromJsonAsync<List<StreakResponse>>();
                return streaks;
            }
            return null;
        }

        public async Task<bool> EditStreak(int streakId, StreakRequest request)
        {
            AddAuthTokenHeader();
            var response = await _api.PutAsJsonAsync($"/Streak/editStreak/{streakId}", request);
            return response.IsSuccessStatusCode;
        }





        public async Task<StreakResponse> GetStreakById(int streakId)
        {
            AddAuthTokenHeader();
            var response = await _api.GetAsync($"/Streak/{streakId}");
            if (response.IsSuccessStatusCode)
            {
                var streak = await response.Content.ReadFromJsonAsync<StreakResponse>();
                return streak;
            }
            return null;
        }

        public async Task<bool> DeleteStreak(int streakId)
        {
            AddAuthTokenHeader();
            var response = await _api.DeleteAsync($"/Streak/deleteStreak/{streakId}");
            return response.IsSuccessStatusCode;
        }
        public async Task<List<RewardResponse>> GetAllRewards()
        {
            AddAuthTokenHeader();
            var response = await _api.GetAsync("/rewards/getAllRewards");
            if (response.IsSuccessStatusCode)
            {
                var rewards = await response.Content.ReadFromJsonAsync<List<RewardResponse>>();
                return rewards;
            }
            return null;
        }

        public async Task<bool> DeleteReward(int id)
        {
            AddAuthTokenHeader();
            var response = await _api.DeleteAsync($"/rewards/deleteReward/{id}");
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> AddReward(RewardRequest rewardRequest)
        {
            AddAuthTokenHeader();
            var response = await _api.PostAsJsonAsync("/rewards/addReward", rewardRequest);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditReward(int id, RewardRequest rewardRequest)
        {
            AddAuthTokenHeader();
            var response = await _api.PutAsJsonAsync($"/rewards/editReward/{id}", rewardRequest);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error updating reward: {response.StatusCode} - {error}");
                return false;
            }
        }



        public async Task<RewardResponse> GetRewardById(int id)
        {
            AddAuthTokenHeader();
            var response = await _api.GetAsync($"/rewards/getReward/{id}");
            if (response.IsSuccessStatusCode)
            {
                var reward = await response.Content.ReadFromJsonAsync<RewardResponse>();
                return reward;
            }
            return null;
        }
        public async Task<List<SecretExperienceResponse>> GetAllSecretDeals()
        {
            AddAuthTokenHeader();
            var response = await _api.GetAsync("/SecretExperience");
            if (response.IsSuccessStatusCode)
            {
                var secretDeals = await response.Content.ReadFromJsonAsync<List<SecretExperienceResponse>>();
                return secretDeals;
            }
            return null;
        }
        public async Task<bool> AddSecretExperience(SecretExperienceRequest request)
        {
            AddAuthTokenHeader();
            var response = await _api.PostAsJsonAsync("/SecretExperience", request);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> EditSecretDeal(int id, SecretExperienceEditRequest request)
        {
            AddAuthTokenHeader();
            var response = await _api.PutAsJsonAsync($"/SecretExperience/editSecretExperience/{id}", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<SecretExperienceResponse> GetSecretDealById(int id)
        {
            AddAuthTokenHeader();
            var response = await _api.GetAsync($"/SecretExperience/{id}");
            if (response.IsSuccessStatusCode)
            {
                var secretDeal = await response.Content.ReadFromJsonAsync<SecretExperienceResponse>();
                return secretDeal;
            }
            return null;
        }
        public async Task<bool> DeleteSecretDeal(int id)
        {
            try
            {
                AddAuthTokenHeader();
                var response = await _api.DeleteAsync($"/SecretExperience/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                // Handle exception if necessary
                return false;
            }



        }
    }
}

