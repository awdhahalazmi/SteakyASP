using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreakyAPi.Model;
using StreakyAPi.Model.Reponses;
using StreakyAPi.Model.Request;
using StreakyAPi.Model.Responses;
using StreakyAPi.Model.Streak;
using StreakyAPi.Model.Token;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StreakyAPi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class StreakController : ControllerBase
    {
        private readonly StreakyContext _context;

        public StreakController(StreakyContext context)
        {
            _context = context;
        }

        [HttpPost("addStreak")]
        public async Task<IActionResult> AddStreak([FromBody] StreakRequest request)
        {
            var businesses = await _context.Businesses
                .Where(b => request.BusinessIds.Contains(b.Id))
                .ToListAsync();

            if (businesses.Count != request.BusinessIds.Count)
            {
                return BadRequest("One or more BusinessIds are invalid");
            }

            var streak = new Streaks
            {
                Title = request.Title,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Businsses = businesses
            };

            _context.Streaks.Add(streak);
            await _context.SaveChangesAsync();

            var response = new StreakResponse
            {
                Id = streak.Id,
                Title = streak.Title,
                Description = streak.Description,
                StartDate = streak.StartDate,
                EndDate = streak.EndDate,
                Businesses = businesses.Select(b => new BusinessResponse
                {
                    Id = b.Id,
                    Name = b.Name,
                    CategoryId = b.CategoryId,
                    Image = b.Image,
                    Question = b.Question,
                    CorrectAnswer = b.CorrectAnswer,
                    WrongAnswer1 = b.WrongAnswer1,
                    WrongAnswer2 = b.WrongAnswer2,
                    Locations = b.Locations?.Select(l => new LocationResponse
                    {
                        Id = l.Id,
                        Name = l.Name,
                        URL = l.URL,
                        Radius = l.Radius,
                        Latitude = l.Latitude,
                        Longitude = l.Longitude
                    }).ToList() ?? new List<LocationResponse>(),
                    CategoryName = b.Category?.Name
                }).ToList()
            };

            return Ok(new { Message = "Streak created", Streak = response });
        }

        //[HttpPost("startStreak/{streakId}")]
        //public async Task<IActionResult> StartStreak(int streakId)
        //{
        //    var email = User.FindFirst(TokenClaimsConstant.Email).Value;
        //    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        //    if (user == null)
        //    {
        //        return NotFound("User not found");
        //    }

        //    var streak = await _context.Streaks
        //        .Include(s => s.Businsses)
        //        .ThenInclude(b => b.Locations)
        //        .FirstOrDefaultAsync(s => s.Id == streakId);
        //    if (streak == null)
        //    {
        //        return NotFound("Streak not found");
        //    }

        //    var userStreak = new UserStreak
        //    {
        //        UserId = user.Id,
        //        StreakId = streak.Id,
        //        StartDate = DateTime.Now,
        //        EndDate = streak.EndDate
        //    };

        //    _context.UserStreaks.Add(userStreak);
        //    await _context.SaveChangesAsync();

        //    var response = new
        //    {
        //        StreakId = streak.Id,
        //        StreakTitle = streak.Title,
        //        Businesses = streak.Businsses.Select(b => new
        //        {
        //            b.Name,
        //            Locations = b.Locations.Select(l => new LocationResponse
        //            {
        //                Id = l.Id,
        //                Name = l.Name,
        //                URL = l.URL,
        //                Radius = l.Radius,
        //                Latitude = l.Latitude,
        //                Longitude = l.Longitude
        //            }).ToList()
        //        }).ToList()
        //    };

        //    return Ok(response);
        //}

        [HttpGet("getAllStreaks")]
        public async Task<IActionResult> GetAllStreaks()
        {
            var streaks = await _context.Streaks
                .Include(s => s.Businsses)
                    .ThenInclude(b => b.Locations)
                .Include(s => s.Businsses)
                    .ThenInclude(b => b.Category)
                .ToListAsync();

            var response = streaks.Select(streak => new StreakResponse
            {
                Id = streak.Id,
                Title = streak.Title,
                Description = streak.Description,
                StartDate = streak.StartDate,
                EndDate = streak.EndDate,
                Businesses = streak.Businsses.Select(b => new BusinessResponse
                {
                    Id = b.Id,
                    Name = b.Name,
                    CategoryId = b.CategoryId,
                    Image = b.Image,
                    Question = b.Question,
                    CorrectAnswer = b.CorrectAnswer,
                    WrongAnswer1 = b.WrongAnswer1,
                    WrongAnswer2 = b.WrongAnswer2,
                    Locations = b.Locations?.Select(l => new LocationResponse
                    {
                        Id = l.Id,
                        Name = l.Name,
                        URL = l.URL,
                        Radius = l.Radius,
                        Latitude = l.Latitude,
                        Longitude = l.Longitude
                    }).ToList() ?? new List<LocationResponse>(),  // Handle null Locations
                    CategoryName = b.Category?.Name  // Handle null Category
                }).ToList()
            }).ToList();

            return Ok(response);
        }
        [HttpPut("editStreak/{streakId}")]
        public async Task<IActionResult> EditStreak(int streakId, [FromBody] StreakRequest request)
        {
            var streak = await _context.Streaks
                .Include(s => s.Businsses)
                .FirstOrDefaultAsync(s => s.Id == streakId);

            if (streak == null)
            {
                return NotFound("Streak not found");
            }

            var businesses = await _context.Businesses
                .Where(b => request.BusinessIds.Contains(b.Id))
                .ToListAsync();

            if (businesses.Count != request.BusinessIds.Count)
            {
                return BadRequest("One or more BusinessIds are invalid");
            }

            streak.Title = request.Title;
            streak.Description = request.Description;
            streak.StartDate = request.StartDate;
            streak.EndDate = request.EndDate;
            streak.Businsses = businesses;

            _context.Streaks.Update(streak);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Streak updated" });
        }

        [HttpDelete("deleteStreak/{streakId}")]
        public async Task<IActionResult> DeleteStreak(int streakId)
        {
            var streak = await _context.Streaks.FindAsync(streakId);

            if (streak == null)
            {
                return NotFound("Streak not found");
            }

            _context.Streaks.Remove(streak);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Streak deleted" });
        }
        [HttpGet("getUserStreaks")]
        public async Task<IActionResult> GetUserStreaks()
        {
            var email = User.FindFirst(TokenClaimsConstant.Email).Value;
            var user = await _context.Users
                .Include(u => u.UserStreaks)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return NotFound("User not found");
            }

            user.UserStreaksCount = user.UserStreaks.Count; // Update the UserStreaksCount

            return Ok(new { TotalStreaks = user.UserStreaksCount });
        }

        [HttpGet("getUserStreaksWithinDays/{days}")]
        public async Task<IActionResult> GetUserStreaksWithinDays(int days)
        {
            var email = User.FindFirst(TokenClaimsConstant.Email).Value;
            var user = await _context.Users
                .Include(u => u.UserStreaks)
                .ThenInclude(us => us.Streak)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var currentDate = DateTime.Now;
            var streaksWithinDays = user.UserStreaks
                .Where(us => (currentDate - us.StartDate).TotalDays <= days)
                .ToList();

            user.UserStreaksCount = user.UserStreaks.Count; // Update the UserStreaksCount

            var response = streaksWithinDays.Select(us => new
            {
                StreakId = us.StreakId,
                StreakTitle = us.Streak.Title,
                StartDate = us.StartDate,
                EndDate = us.EndDate
            }).ToList();

            return Ok(new { TotalStreaks = user.UserStreaksCount, StreaksWithinDays = response });
        }

        [HttpPost("startStreak/{streakId}")]
        public async Task<IActionResult> StartStreak(int streakId)
        {
            var email = User.FindFirst(TokenClaimsConstant.Email)?.Value;
            if (email == null)
            {
                return BadRequest("User email is missing");
            }

            var user = await _context.Users
                .Include(u => u.UserStreaks)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var streak = await _context.Streaks.FindAsync(streakId);
            if (streak == null)
            {
                return NotFound("Streak not found");
            }

            var currentDate = DateTime.Now;
            var existingStreak = user.UserStreaks
                .FirstOrDefault(us => us.StreakId == streakId && (currentDate - us.StartDate).TotalDays <= (streak.EndDate - streak.StartDate).TotalDays);

            if (existingStreak != null)
            {
                return BadRequest("You have already gained this streak within the given days");
            }

            var userStreak = new UserStreak
            {
                UserId = user.Id,
                StreakId = streak.Id,
                StartDate = currentDate,
                EndDate = streak.EndDate
            };

            user.UserStreaks.Add(userStreak);
            user.UserStreaksCount = user.UserStreaks.Count; // Update the UserStreaksCount

            _context.UserStreaks.Add(userStreak);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Streak started" });
        }
    }
}