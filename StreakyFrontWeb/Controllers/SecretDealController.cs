using Microsoft.AspNetCore.Mvc;
using StreakyFrontWeb.Models;
using System.Collections.Generic;

namespace StreakyFrontWeb.Controllers
{
    public class SecretDealController : Controller
    {
        private static List<SecretDeal> secretDeals = new List<SecretDeal>
        {
            new SecretDeal { Id = 1, DealDate = "2024-05-01", Deal = "Special Offer", Streaks = "5", Business = "Pick" },
            new SecretDeal { Id = 2, DealDate = "2024-05-02", Deal = "New Branch Opening", Streaks = "10", Business = "Ananas" }
        };

        public IActionResult List()
        {
            return View(secretDeals);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(SecretDeal secretDeal)
        {
            if (ModelState.IsValid)
            {
                secretDeal.Id = secretDeals.Count + 1;
                secretDeals.Add(secretDeal);
                return RedirectToAction("List");
            }
            return View(secretDeal);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var secretDeal = secretDeals.Find(sd => sd.Id == id);
            if (secretDeal == null)
            {
                return NotFound();
            }
            return View(secretDeal);
        }

        [HttpPost]
        public IActionResult Edit(SecretDeal secretDeal)
        {
            if (ModelState.IsValid)
            {
                var existingDeal = secretDeals.Find(sd => sd.Id == secretDeal.Id);
                if (existingDeal == null)
                {
                    return NotFound();
                }

                existingDeal.DealDate = secretDeal.DealDate;
                existingDeal.Deal = secretDeal.Deal;
                existingDeal.Streaks = secretDeal.Streaks;
                existingDeal.Business = secretDeal.Business;

                return RedirectToAction("List");
            }
            return View(secretDeal);
        }
    }
}
