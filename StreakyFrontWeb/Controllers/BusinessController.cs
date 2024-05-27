using Microsoft.AspNetCore.Mvc;
using StreakyFrontWeb.Models;
using System.Collections.Generic;

namespace StreakyFrontWeb.Controllers
{
    public class BusinessController : Controller
    {
        private static List<Business> businesses = new List<Business>
        {
            new Business { id = 1, Name = "Cinescape", Categoryid = "1", Description = "Entertainment", LogoUrl = "~/images/c.jpg", ImageUrl = "https://uniguest.com/wp-content/uploads/2022/08/Cinescape.png", Question = "Sample Question", CorrectAnswer = "Correct Answer", WrongAnswer1 = "Wrong Answer 1", WrongAnswer2 = "Wrong Answer 2", Location1 = "Location 1", Location2 = "Location 2" }
        };

        public IActionResult BusinessList()
        {
            return View(businesses);
        }

        [HttpGet]
        public IActionResult AddBusiness()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBusiness(Business business)
        {
            if (ModelState.IsValid)
            {
                business.id = businesses.Count + 1;
                businesses.Add(business);
                return RedirectToAction("BusinessList");
            }
            return View(business);
        }

        [HttpGet]
        public IActionResult EditBusiness(int id)
        {
            var business = businesses.Find(b => b.id == id);
            if (business == null)
            {
                return NotFound();
            }
            return View(business);
        }

        [HttpPost]
        public IActionResult EditBusiness(Business business)
        {
            if (ModelState.IsValid)
            {
                var existingBusiness = businesses.Find(b => b.id == business.id);
                if (existingBusiness == null)
                {
                    return NotFound();
                }

                existingBusiness.Name = business.Name;
                existingBusiness.Categoryid = business.Categoryid;
                existingBusiness.Description = business.Description;
                existingBusiness.LogoUrl = business.LogoUrl;
                existingBusiness.ImageUrl = business.ImageUrl;
                existingBusiness.Question = business.Question;
                existingBusiness.CorrectAnswer = business.CorrectAnswer;
                existingBusiness.WrongAnswer1 = business.WrongAnswer1;
                existingBusiness.WrongAnswer2 = business.WrongAnswer2;
                existingBusiness.Location1 = business.Location1;
                existingBusiness.Location2 = business.Location2;

                return RedirectToAction("BusinessList");
            }
            return View(business);
        }
    }
}

