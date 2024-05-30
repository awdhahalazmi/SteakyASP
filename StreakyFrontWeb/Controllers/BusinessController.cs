using Microsoft.AspNetCore.Mvc;
using StreakyAPi.Model;
using StreakyAPi.Model.Reponses;
using StreakyAPi.Model.Request;
using StreakyAPi.Model.Responses;
using StreakyFrontWeb.API;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreakyFrontWeb.Controllers
{
    public class BusinessController : Controller
    {
        private readonly StreakyAPI _streakyAPI;

        public BusinessController(StreakyAPI streakyAPI)
        {
            _streakyAPI = streakyAPI;
        }

        public async Task<IActionResult> BusinessList()
        {
            var businesses = await _streakyAPI.GetAllBusinesses();
            if (businesses == null)
            {
                return View(new List<BusinessResponse>());
            }
            return View(businesses);
        }

        public async Task<IActionResult> AddBusiness()
        {
            ViewBag.Title = "Add Business";
            ViewBag.Action = "AddBusiness";

            var categories = await _streakyAPI.GetCategories();
            var locations = await _streakyAPI.GetLocations();

            ViewBag.Categories = categories ?? new List<Category>();
            ViewBag.Locations = locations ?? new List<LocationResponse>();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBusiness(BusinessRequest businessRequest)
        {
            if (ModelState.IsValid)
            {
                var formContent = new MultipartFormDataContent();

                formContent.Add(new StringContent(businessRequest.Name), nameof(businessRequest.Name));
                formContent.Add(new StringContent(businessRequest.CategoryId.ToString()), nameof(businessRequest.CategoryId));
                formContent.Add(new StreamContent(businessRequest.Image.OpenReadStream()), nameof(businessRequest.Image), businessRequest.Image.FileName);
                formContent.Add(new StringContent(businessRequest.Question), nameof(businessRequest.Question));
                formContent.Add(new StringContent(businessRequest.CorrectAnswer), nameof(businessRequest.CorrectAnswer));
                formContent.Add(new StringContent(businessRequest.WrongAnswer1), nameof(businessRequest.WrongAnswer1));
                formContent.Add(new StringContent(businessRequest.WrongAnswer2), nameof(businessRequest.WrongAnswer2));
                formContent.Add(new StringContent(businessRequest.Question2), nameof(businessRequest.Question2));
                formContent.Add(new StringContent(businessRequest.CorrectAnswerQ2), nameof(businessRequest.CorrectAnswerQ2));
                formContent.Add(new StringContent(businessRequest.WrongAnswerQ2_1), nameof(businessRequest.WrongAnswerQ2_1));
                formContent.Add(new StringContent(businessRequest.WrongAnswerQ2_2), nameof(businessRequest.WrongAnswerQ2_2));

                foreach (var locationId in businessRequest.LocationIds)
                {
                    formContent.Add(new StringContent(locationId.ToString()), $"{nameof(businessRequest.LocationIds)}");
                }

                var response = await _streakyAPI.AddBusiness(formContent);
                if (response)
                {
                    return RedirectToAction(nameof(BusinessList));
                }
                TempData["Error"] = "Failed to add business. Please try again.";
            }

            ViewBag.Categories = await _streakyAPI.GetCategories();
            ViewBag.Locations = await _streakyAPI.GetLocations();
            return View(businessRequest);
        }
        public async Task<IActionResult> EditBusiness(int id)
        {
            ViewBag.Title = "Edit Business";
            ViewBag.Action = "EditBusiness";

            var business = await _streakyAPI.GetBusinessById(id);
            if (business == null)
            {
                return NotFound();
            }

            var categories = await _streakyAPI.GetCategories();
            var locations = await _streakyAPI.GetLocations();

            ViewBag.Categories = categories ?? new List<Category>();
            ViewBag.Locations = locations ?? new List<LocationResponse>();

            var model = new BusinessUpdateRequest
            {
                Id = business.Id,
                Name = business.Name,
                CategoryId = business.CategoryId,
                Question = business.Question,
                CorrectAnswer = business.CorrectAnswer,
                WrongAnswer1 = business.WrongAnswer1,
                WrongAnswer2 = business.WrongAnswer2,
                Question2 = business.Question2,
                CorrectAnswerQ2 = business.CorrectAnswerQ2,
                WrongAnswerQ2_1 = business.WrongAnswerQ2_1,
                WrongAnswerQ2_2 = business.WrongAnswerQ2_2,
                LocationIds = business.Locations.Select(l => l.Id).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditBusiness(BusinessUpdateRequest businessUpdateRequest)
        {
            if (ModelState.IsValid)
            {
                var formContent = new MultipartFormDataContent();

                formContent.Add(new StringContent(businessUpdateRequest.Name), nameof(businessUpdateRequest.Name));
                if (businessUpdateRequest.CategoryId.HasValue)
                {
                    formContent.Add(new StringContent(businessUpdateRequest.CategoryId.Value.ToString()), nameof(businessUpdateRequest.CategoryId));
                }
                if (businessUpdateRequest.Image != null)
                {
                    formContent.Add(new StreamContent(businessUpdateRequest.Image.OpenReadStream()), nameof(businessUpdateRequest.Image), businessUpdateRequest.Image.FileName);
                }
                formContent.Add(new StringContent(businessUpdateRequest.Question), nameof(businessUpdateRequest.Question));
                formContent.Add(new StringContent(businessUpdateRequest.CorrectAnswer), nameof(businessUpdateRequest.CorrectAnswer));
                formContent.Add(new StringContent(businessUpdateRequest.WrongAnswer1), nameof(businessUpdateRequest.WrongAnswer1));
                formContent.Add(new StringContent(businessUpdateRequest.WrongAnswer2), nameof(businessUpdateRequest.WrongAnswer2));
                formContent.Add(new StringContent(businessUpdateRequest.Question2), nameof(businessUpdateRequest.Question2));
                formContent.Add(new StringContent(businessUpdateRequest.CorrectAnswerQ2), nameof(businessUpdateRequest.CorrectAnswerQ2));
                formContent.Add(new StringContent(businessUpdateRequest.WrongAnswerQ2_1), nameof(businessUpdateRequest.WrongAnswerQ2_1));
                formContent.Add(new StringContent(businessUpdateRequest.WrongAnswerQ2_2), nameof(businessUpdateRequest.WrongAnswerQ2_2));

                if (businessUpdateRequest.LocationIds != null)
                {
                    foreach (var locationId in businessUpdateRequest.LocationIds)
                    {
                        formContent.Add(new StringContent(locationId.ToString()), nameof(businessUpdateRequest.LocationIds));
                    }
                }

                var response = await _streakyAPI.EditBusiness(businessUpdateRequest.Id, formContent);
                if (response)
                {
                    return RedirectToAction(nameof(BusinessList));
                }
                TempData["Error"] = "Failed to update business. Please try again.";
            }

            var categories = await _streakyAPI.GetCategories();
            var locations = await _streakyAPI.GetLocations();

            ViewBag.Categories = categories ?? new List<Category>();
            ViewBag.Locations = locations ?? new List<LocationResponse>();

            return View(businessUpdateRequest);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteBusiness(int id)
        {
            var response = await _streakyAPI.DeleteBusiness(id);
            if (response)
            {
                return RedirectToAction(nameof(BusinessList));
            }
            TempData["Error"] = "Failed to delete business. Please try again.";
            return RedirectToAction(nameof(BusinessList));
        }
    }
}
