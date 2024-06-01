using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using StreakyAPi.Model.Request;
using StreakyFrontWeb.API;
using StreakyAPi.Model;
using StreakyAPi.Model.Reponses;
using StreakyAPi.Model.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

public class MyRewardController : Controller
{
    private readonly StreakyAPI _streakyAPI;

    public MyRewardController(StreakyAPI streakyAPI)
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
            var response = await _streakyAPI.EditReward(rewardUpdateRequest.Id, rewardUpdateRequest);
            if (response)
            {
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Failed to update reward. Please try again.";
        }
        else
        {
            TempData["Error"] = "Invalid data. Please check the input and try again.";
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