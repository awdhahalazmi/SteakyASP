using Microsoft.AspNetCore.Mvc;

using StreakyFrontWeb.Models;


namespace StreakyFrontWeb.Controllers
{


        public class StreakListController : Controller
        {
            private static List<Streak> _streaks = new List<Streak>
        {
            new Streak { Id = 1, Name = "Pick Yo", Description = "This Week's Streak", StreakDate = DateTime.Now },
            new Streak { Id = 2, Name = "Ananas", Description = "This Week's Streak", StreakDate = DateTime.Now },
            new Streak { Id = 3, Name = "Pick Yo", Description = "This Week's Streak", StreakDate = DateTime.Now }
        };

            public IActionResult Index()
            {
                return View(_streaks);
            }

            public IActionResult Edit(int id)
            {
                var streak = _streaks.FirstOrDefault(s => s.Id == id);
                if (streak == null)
                {
                    return NotFound();
                }
                return View(streak);
            }

            [HttpPost]
            public IActionResult Edit(Streak streak)
            {
                var existingStreak = _streaks.FirstOrDefault(s => s.Id == streak.Id);
                if (existingStreak == null)
                {
                    return NotFound();
                }

                existingStreak.Name = streak.Name;
                existingStreak.Description = streak.Description;
                existingStreak.StreakDate = streak.StreakDate;

                return RedirectToAction(nameof(Index));
            }

            public IActionResult Add()
            {
                return View();
            }

            [HttpPost]
            public IActionResult Add(Streak streak)
            {
                if (ModelState.IsValid)
                {
                    streak.Id = _streaks.Max(s => s.Id) + 1;
                    _streaks.Add(streak);
                    return RedirectToAction(nameof(Add));
                }
                return View(streak);
            }

            [HttpPost]
            public IActionResult Delete(int id)
            {
                var streak = _streaks.FirstOrDefault(s => s.Id == id);
                if (streak != null)
                {
                    _streaks.Remove(streak);
                }
                return RedirectToAction(nameof(Index));
            }
        }
    }




    //    public class StreakListController : Controller
    //    {
    //        private static List<Streak> _streaks = new List<Streak>
    //    {
    //        new Streak { Id = 1, Name = "Pick Yo", Description = "This Week's Streak", StreakDate = DateTime.Now },
    //        new Streak { Id = 2, Name = "Ananas", Description = "This Week's Streak", StreakDate = DateTime.Now },
    //        new Streak { Id = 3, Name = "Pick Yo", Description = "This Week's Streak", StreakDate = DateTime.Now }
    //    };

//        public IActionResult Index()
//        {
//            return View(_streaks);
//        }

//        public IActionResult EditS(int id)
//        {
//            var streak = _streaks.FirstOrDefault(s => s.Id == id);
//            if (streak == null)
//            {
//                return NotFound();
//            }
//            return View(streak);
//        }

//        [HttpPost]
//        public IActionResult Edit(Streak streak)
//        {
//            var existingStreak = _streaks.FirstOrDefault(s => s.Id == streak.Id);
//            if (existingStreak == null)
//            {
//                return NotFound();
//            }

//            existingStreak.Name = streak.Name;
//            existingStreak.Description = streak.Description;
//            existingStreak.StreakDate = streak.StreakDate;

//            return RedirectToAction(nameof(Index));
//        }

//        [HttpPost]
//        public IActionResult Delete(int id)
//        {
//            var streak = _streaks.FirstOrDefault(s => s.Id == id);
//            if (streak != null)
//            {
//                _streaks.Remove(streak);
//            }
//            return RedirectToAction(nameof(Index));
//        }
//    }
//}
