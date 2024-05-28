using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using StreakyAPi.Model.Auth;
using StreakyFrontWeb.API;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StreakyFrontWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly StreakyAPI _api;

        public AccountController(StreakyAPI api)
        {
            _api = api;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var jwtToken = await _api.Login(request.Email, request.Password);
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

                return RedirectToAction("BusinessList", "Business");
            }

            ModelState.AddModelError("Email", "Invalid Username and/or Password");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult Denied()
        {
            return View();
        }
    }
}
