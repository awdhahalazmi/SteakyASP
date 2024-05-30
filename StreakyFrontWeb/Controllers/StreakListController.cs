using Microsoft.AspNetCore.Mvc;
using StreakyFrontWeb.API;
using StreakyAPi.Model.Request;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;
using StreakyAPi.Model.Responses;

namespace StreakyFrontWeb.Controllers
{
    public class StreakListController : Controller
    {
        private readonly StreakyAPI _streakyAPI;

        public StreakListController(StreakyAPI streakyAPI)
        {
            _streakyAPI = streakyAPI;
        }

        public async Task<IActionResult> StreakList()
        {
            var streaks = await _streakyAPI.GetAllStreaks();
            return View(streaks);
        }

        [HttpGet]
        public async Task<IActionResult> AddStreak()
        {
            ViewBag.Businesses = await _streakyAPI.GetAllBusinesses();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStreak(StreakRequest streakRequest)
        {
            if (ModelState.IsValid)
            {
                bool response = await _streakyAPI.AddStreak(streakRequest);
                if (response)
                {
                    return RedirectToAction(nameof(StreakList));
                }
                TempData["Error"] = "Failed to add streak. Please try again.";
            }

            ViewBag.Businesses = await _streakyAPI.GetAllBusinesses(); // Reload businesses on failure
            return View(streakRequest);
        }

        [HttpGet]
        public async Task<IActionResult> EditStreak(int id)
        {
            ViewBag.Title = "Edit Streak";
            ViewBag.Action = "EditStreak";

            var streak = await _streakyAPI.GetStreakById(id);
            if (streak == null)
            {
                return NotFound();
            }

            var businesses = await _streakyAPI.GetAllBusinesses(); // Assuming streaks are related to businesses
            ViewBag.Businesses = businesses ?? new List<BusinessResponse>();

            var model = new StreakRequest
            {
                Id = streak.Id,
                Title = streak.Title,
                Description = streak.Description,
                StartDate = streak.StartDate,
                EndDate = streak.EndDate,
                BusinessIds = streak.Businesses.Select(b => b.Id).ToList()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStreak(int id, StreakRequest streakRequest)
        {
            if (ModelState.IsValid)
            {
                bool response = await _streakyAPI.EditStreak(id, streakRequest);
                if (response)
                {
                    return RedirectToAction(nameof(StreakList));
                }
                TempData["Error"] = "Failed to edit streak. Please try again.";
            }
            else
            {
                // Log model state errors
                foreach (var state in ModelState)
                {
                    var key = state.Key;
                    var errors = state.Value.Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"ModelState Error - Key: {key}, Error: {error.ErrorMessage}");
                    }
                }
            }

            // In case of an error, reload necessary data for the view
            var businesses = await _streakyAPI.GetAllBusinesses();
            ViewBag.Businesses = businesses ?? new List<BusinessResponse>();
            return View(streakRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStreak(int id)
        {
            bool response = await _streakyAPI.DeleteStreak(id);
            if (response)
            {
                return RedirectToAction(nameof(StreakList));
            }
            TempData["Error"] = "Failed to delete streak. Please try again.";
            return RedirectToAction(nameof(EditStreak), new { id });
        }
    }
}
