using Microsoft.AspNetCore.Mvc;
using StreakyFrontWeb.Models;
using System.Collections.Generic;
using System.Linq;

namespace StreakyFrontWeb.Controllers
{
    public class StreakListController : Controller
    {
        private static List<Streaks> streaks = new List<Streaks>
        {
            new Streaks { Id = 1, Title = "First Streak", Description = "Description for first streak", BusinessId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(30) },
            new Streaks { Id = 2, Title = "Second Streak", Description = "Description for second streak", BusinessId = 2, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(60) }
        };

        //private static List<Business> businesses = new List<Business>
        //{
        //    new Business { Id = 1, Name = "Business 1" },
        //    new Business { Id = 2, Name = "Business 2" }
        //};

        public IActionResult StreakList()
        {
            return View(streaks);
        }

        [HttpGet]
        public IActionResult AddStreak()
        {
           // ViewBag.Businesses = businesses;
            return View();
        }

        [HttpPost]
        public IActionResult AddStreak(Streaks streak)
        {
            if (ModelState.IsValid)
            {
                streak.Id = streaks.Max(s => s.Id) + 1;
                streaks.Add(streak);
                return RedirectToAction("StreakList");
            }
           // ViewBag.Businesses = businesses;
            return View(streak);
        }

        [HttpGet]
        public IActionResult EditStreak(int id)
        {
            var streak = streaks.FirstOrDefault(s => s.Id == id);
            if (streak == null)
            {
                return NotFound();
            }
          //  ViewBag.Businesses = businesses;
            return View(streak);
        }

        [HttpPost]
        public IActionResult EditStreak(Streaks streak)
        {
            if (ModelState.IsValid)
            {
                var existingStreak = streaks.FirstOrDefault(s => s.Id == streak.Id);
                if (existingStreak == null)
                {
                    return NotFound();
                }
                existingStreak.Title = streak.Title;
                existingStreak.Description = streak.Description;
                existingStreak.BusinessId = streak.BusinessId;
                existingStreak.StartDate = streak.StartDate;
                existingStreak.EndDate = streak.EndDate;

                return RedirectToAction("StreakList");
            }
          //  ViewBag.Businesses = businesses;
            return View(streak);
        }
    }
}




//using Microsoft.AspNetCore.Mvc;

//using StreakyFrontWeb.Models;


//namespace StreakyFrontWeb.Controllers
//{


//        public class StreakListController : Controller
//        {
//            private static List<Streak> _streaks = new List<Streak>
//        {
//            new Streak { Id = 1, Name = "Pick Yo", Description = "This Week's Streak", StreakDate = DateTime.Now },
//            new Streak { Id = 2, Name = "Ananas", Description = "This Week's Streak", StreakDate = DateTime.Now },
//            new Streak { Id = 3, Name = "Pick Yo", Description = "This Week's Streak", StreakDate = DateTime.Now }
//        };

//            public IActionResult Index()
//            {
//                return View(_streaks);
//            }

//            public IActionResult Edit(int id)
//            {
//                var streak = _streaks.FirstOrDefault(s => s.Id == id);
//                if (streak == null)
//                {
//                    return NotFound();
//                }
//                return View(streak);
//            }

//            [HttpPost]
//            public IActionResult Edit(Streak streak)
//            {
//                var existingStreak = _streaks.FirstOrDefault(s => s.Id == streak.Id);
//                if (existingStreak == null)
//                {
//                    return NotFound();
//                }

//                existingStreak.Name = streak.Name;
//                existingStreak.Description = streak.Description;
//                existingStreak.StreakDate = streak.StreakDate;

//                return RedirectToAction(nameof(Index));
//            }

//            public IActionResult Add()
//            {
//                return View();
//            }

//            [HttpPost]
//            public IActionResult Add(Streak streak)
//            {
//                if (ModelState.IsValid)
//                {
//                    streak.Id = _streaks.Max(s => s.Id) + 1;
//                    _streaks.Add(streak);
//                    return RedirectToAction(nameof(Add));
//                }
//                return View(streak);
//            }

//            [HttpPost]
//            public IActionResult Delete(int id)
//            {
//                var streak = _streaks.FirstOrDefault(s => s.Id == id);
//                if (streak != null)
//                {
//                    _streaks.Remove(streak);
//                }
//                return RedirectToAction(nameof(Index));
//            }
//        }
//    }



