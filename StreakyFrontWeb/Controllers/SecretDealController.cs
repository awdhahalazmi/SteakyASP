using Microsoft.AspNetCore.Mvc;
using StreakyFrontWeb.API;
using StreakyAPi.Model.Request;
using StreakyAPi.Model.Responses;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StreakyFrontWeb.Controllers
{
    public class SecretDealController : Controller
    {
        private readonly StreakyAPI _streakyAPI;

        public SecretDealController(StreakyAPI streakyAPI)
        {
            _streakyAPI = streakyAPI;
        }

        public async Task<IActionResult> List()
        {
            var secretDeals = await _streakyAPI.GetAllSecretDeals();
            return View(secretDeals);
        }

        [HttpGet]
        public async Task<IActionResult> AddDeal()
        {
            var streaks = await _streakyAPI.GetAllStreaks();
            var businesses = await _streakyAPI.GetAllBusinesses();

            ViewBag.Streaks = streaks.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Title
            }).ToList();

            ViewBag.Businesses = businesses.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDeal(SecretExperienceRequest request)
        {
            if (!ModelState.IsValid)
            {
                var streaks = await _streakyAPI.GetAllStreaks();
                var businesses = await _streakyAPI.GetAllBusinesses();

                ViewBag.Streaks = streaks.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Title
                }).ToList();

                ViewBag.Businesses = businesses.Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Name
                }).ToList();

                return View(request);
            }

            var success = await _streakyAPI.AddSecretExperience(request);

            if (success)
            {
                return RedirectToAction("List");
            }

            ModelState.AddModelError("", "An error occurred while adding the secret deal.");
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> EditDeal(int id)
        {
            var secretExperience = await _streakyAPI.GetSecretDealById(id);

            if (secretExperience == null)
            {
                return NotFound();
            }

            var streaks = await _streakyAPI.GetAllStreaks();
            var businesses = await _streakyAPI.GetAllBusinesses();

            ViewBag.Streaks = streaks.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Title
            }).ToList();

            ViewBag.Businesses = businesses.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();

            var editModel = new SecretExperienceEditRequest
            {
                Id = secretExperience.Id,
                StartDate = secretExperience.StartDate,
                EndDate = secretExperience.EndDate,
                Title = secretExperience.Title,
                Description = secretExperience.Description,
                StreakClaimed = secretExperience.StreakClaimed,
            };

            return View(editModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDeal(SecretExperienceEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                var streaks = await _streakyAPI.GetAllStreaks();
                var businesses = await _streakyAPI.GetAllBusinesses();

                ViewBag.Streaks = streaks.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Title
                }).ToList();

                ViewBag.Businesses = businesses.Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Name
                }).ToList();

                return View(request);
            }

            var success = await _streakyAPI.EditSecretDeal(request.Id, request);

            if (success)
            {
                return RedirectToAction("List");
            }

            ModelState.AddModelError("", "An error occurred while editing the secret deal.");
            return View(request);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _streakyAPI.DeleteSecretDeal(id);

            if (success)
            {
                return RedirectToAction("List");
            }

            ModelState.AddModelError("", "An error occurred while deleting the secret deal.");
            return RedirectToAction("EditDeal", new { id });
        }
    }
}