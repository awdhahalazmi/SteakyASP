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
                // Handle the case when businesses are not retrieved, e.g., show an error message
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
    }
}