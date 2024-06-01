using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using StreakyAPi.Model.Auth;
using StreakyFrontWeb.API;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StreakyAPi.Model.Request;

namespace StreakyFrontWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly StreakyAPI _streakyAPI;
        private readonly ILogger<AccountController> _logger;

        public AccountController(StreakyAPI streakyAPI, ILogger<AccountController> logger)
        {
            _streakyAPI = streakyAPI;
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                _logger.LogInformation("Attempting to log in user with email: {Email}", request.Email);
                var jwtToken = await _streakyAPI.Login(request.Email, request.Password);
                if (!string.IsNullOrEmpty(jwtToken))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtSecurityToken = handler.ReadJwtToken(jwtToken);

                    var claimsIdentity = new ClaimsIdentity(jwtSecurityToken.Claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = false,
                        AllowRefresh = true
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    HttpContext.Session.SetString("AuthToken", jwtToken);
                    HttpContext.Response.Cookies.Append("AuthToken", jwtToken);

                    _logger.LogInformation("User {Email} logged in successfully", request.Email);
                    return RedirectToAction("BusinessList", "Business");
                }

                _logger.LogWarning("Invalid login attempt for user {Email}", request.Email);
                ModelState.AddModelError("Username", "Invalid Username and/or Password");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in user {Email}", request.Email);
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            }

            return View(request);
        }
        public async Task<IActionResult> Profile()
        {
            var profile = await _streakyAPI.GetProfile();
            if (profile == null)
            {
                _logger.LogWarning("Profile not found.");
                return RedirectToAction("Login");
            }
            return View(profile);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(EditProfileRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _streakyAPI.EditProfile(request);
                if (result)
                {
                    return RedirectToAction("Profile");
                }
                ModelState.AddModelError(string.Empty, "Failed to update profile.");
            }
            var profile = await _streakyAPI.GetProfile();
            return View("Profile", profile);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }


    }
}
