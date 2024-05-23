using Microsoft.AspNetCore.Mvc;
using StreakyFrontWeb.Models;

namespace StreakyFrontWeb.Controllers
{
  
        public class RewardsController : Controller
        {
            private static List<Offer> _offers = new List<Offer>
        {
            new Offer { Id = 1, OfferDate = DateTime.Now, Offers = "10% discount", Points = 10, Business = "Cinescape" },
            new Offer { Id = 2, OfferDate = DateTime.Now, Offers = "3 KD Coupon", Points = 5, Business = "Pick" }
        };

            public IActionResult Index()
            {
                return View(_offers);
            }

            public IActionResult AddOffer()
            {
                return View();
            }

            [HttpPost]
            public IActionResult AddOffer(Offer offer)
            {
                if (ModelState.IsValid)
                {
                    offer.Id = _offers.Max(r => r.Id) + 1;
                    _offers.Add(offer);
                    return RedirectToAction(nameof(Index));
                }
                return View(offer);
            }

            [HttpPost]
            public IActionResult Delete(int id)
            {
                var offer = _offers.FirstOrDefault(r => r.Id == id);
                if (offer != null)
                {
                    _offers.Remove(offer);
                }
                return RedirectToAction(nameof(Index));
            }
        }
    }

