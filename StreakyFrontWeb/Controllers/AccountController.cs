using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using StreakyAPi.Model.Auth;
using StreakyFrontWeb.API;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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

                    HttpContext.Session.SetString("Token", jwtToken);
                    HttpContext.Response.Cookies.Append("Token", jwtToken);

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
    }
}
