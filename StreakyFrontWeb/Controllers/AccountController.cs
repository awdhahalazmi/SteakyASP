using Microsoft.AspNetCore.Mvc;
using StreakyAPi.Model.Reponses;

namespace StreakyFrontWeb.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Profile()
        {
          
          
            return View();
        }
    }
}
