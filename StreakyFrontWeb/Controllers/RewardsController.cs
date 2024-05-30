using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StreakyAPi.Model.Request;
using StreakyFrontWeb.API;
using System.Linq;
using System.Threading.Tasks;

public class RewardsController : Controller
{
    private readonly StreakyAPI _streakyAPI;

    public RewardsController(StreakyAPI streakyAPI)
    {
        _streakyAPI = streakyAPI;
    }

    public async Task<IActionResult> Index()
    {
        var rewards = await _streakyAPI.GetAllRewards();
        return View(rewards);
    }

    [HttpGet]
    public async Task<IActionResult> AddReward()
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
    public async Task<IActionResult> AddReward(RewardRequest rewardRequest)
    {
        if (ModelState.IsValid)
        {
            bool response = await _streakyAPI.AddReward(rewardRequest);
            if (response)
            {
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Failed to add reward. Please try again.";
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

        return View(rewardRequest);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        bool response = await _streakyAPI.DeleteReward(id);
        if (response)
        {
            return RedirectToAction(nameof(Index));
        }
        TempData["Error"] = "Failed to delete reward. Please try again.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> EditReward(int id)
    {
        ViewBag.Title = "Edit Reward";
        ViewBag.Action = "EditReward";

        var reward = await _streakyAPI.GetRewardById(id);
        if (reward == null)
        {
            return NotFound();
        }

        var streaks = await _streakyAPI.GetAllStreaks();
        var businesses = await _streakyAPI.GetAllBusinesses();

        if (streaks == null)
        {
            return StatusCode(500, "Error retrieving streaks");
        }

        if (businesses == null)
        {
            return StatusCode(500, "Error retrieving businesses");
        }

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

        var model = new RewardRequest
        {
            Id = reward.Id,
            StreakId = reward.StreakId,
            Description = reward.Description,
            PointsClaimed = reward.PointsClaimed,
            BusinessId = reward.BusinessId
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditReward(RewardRequest rewardUpdateRequest)
    {
        if (ModelState.IsValid)
        {
            var formContent = new MultipartFormDataContent
            {
                { new StringContent(rewardUpdateRequest.StreakId.ToString()), nameof(rewardUpdateRequest.StreakId) },
                { new StringContent(rewardUpdateRequest.Description), nameof(rewardUpdateRequest.Description) },
                { new StringContent(rewardUpdateRequest.PointsClaimed.ToString()), nameof(rewardUpdateRequest.PointsClaimed) },
                { new StringContent(rewardUpdateRequest.BusinessId.ToString()), nameof(rewardUpdateRequest.BusinessId) }
            };

            var response = await _streakyAPI.EditReward(rewardUpdateRequest.Id, formContent);
            if (response)
            {
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Failed to update reward. Please try again.";
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

        return View(rewardUpdateRequest);
    }
}
