using Microsoft.AspNetCore.Mvc;
using StreakyAPi.Model;
using System.Collections.Generic;
using System.Linq;

namespace StreakyFrontWeb.Controllers
{
    public class BusinessController : Controller
    {
        private static List<Business> _businesses = new List<Business>
        {
            new Business { Id = 1, Name = "Coffee Shop", CategoryId = 1, Image = "https://via.placeholder.com/50", Question = "What is the best coffee?", CorrectAnswer = "Espresso", WrongAnswer1 = "Latte", WrongAnswer2 = "Cappuccino" },
            new Business { Id = 2, Name = "Retail Shop", CategoryId = 2, Image = "https://via.placeholder.com/50", Question = "What is the best product?", CorrectAnswer = "Shoes", WrongAnswer1 = "Bags", WrongAnswer2 = "Clothes" }
        };

        public IActionResult BusinessList()
        {
            return View(_businesses);
        }

        public IActionResult AddBusiness()
        {
            ViewBag.Title = "Add Business";
            ViewBag.Action = "AddBusiness";
            ViewBag.Categories = new List<Category>
                {
                    new Category { Id = 1, Name = "Coffee Shops" },
                    new Category { Id = 2, Name = "Retail Shops" },
                    new Category { Id = 3, Name = "Leisure Activity" }
                };
            return View("AddBusiness");
        }

        [HttpPost]
        public IActionResult AddBusiness(Business business)
        {
            if (ModelState.IsValid)
            {
                business.Id = _businesses.Max(b => b.Id) + 1;
                _businesses.Add(business);
                return RedirectToAction(nameof(BusinessList));
            }
            return View("AddEditBusiness", business);
        }

        public IActionResult EditBusiness(int id)
        {
            var business = _businesses.FirstOrDefault(b => b.Id == id);
            if (business == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit Business";
            ViewBag.Action = "EditBusiness";
            ViewBag.Categories = new List<Category>
                {
                    new Category { Id = 1, Name = "Coffee Shops" },
                    new Category { Id = 2, Name = "Retail Shops" },
                    new Category { Id = 3, Name = "Leisure Activity" }
                };
            return View("EditBusiness", business);
        }

        [HttpPost]
        public IActionResult EditBusiness(Business business)
        {
            if (ModelState.IsValid)
            {
                var existingBusiness = _businesses.FirstOrDefault(b => b.Id == business.Id);
                if (existingBusiness == null)
                {
                    return NotFound();
                }
                existingBusiness.Name = business.Name;
                existingBusiness.CategoryId = business.CategoryId;
                existingBusiness.Image = business.Image;
                existingBusiness.Question = business.Question;
                existingBusiness.CorrectAnswer = business.CorrectAnswer;
                existingBusiness.WrongAnswer1 = business.WrongAnswer1;
                existingBusiness.WrongAnswer2 = business.WrongAnswer2;
                return RedirectToAction(nameof(BusinessList));
            }
            return View("EditBusiness", business);
        }
    }
}

