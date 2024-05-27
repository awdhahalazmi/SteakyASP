using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StreakyAPi.Model;
using StreakyAPi.Model.Request;
using StreakyAPi.Model.Responses;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using StreakyAPi.Model.Reponses;
using StreakyAPi.Model.Token;

namespace StreakyAPi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly StreakyContext _context;

        public BusinessController(StreakyContext context)
        {
            _context = context;
        }

        [HttpPost("addLocation")]
        public async Task<IActionResult> AddLocation([FromBody] LocationRequest request)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                return BadRequest("Location name is required");
            }

            var newLocation = new Location
            {
                Name = request.Name,
                URL = request.URL,
                Radius = request.Radius,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };

            _context.Locations.Add(newLocation);
            await _context.SaveChangesAsync();

            var response = new LocationResponse
            {
                Id = newLocation.Id,
                Name = newLocation.Name,
                URL = newLocation.URL,
                Radius = newLocation.Radius,
                Latitude = newLocation.Latitude,
                Longitude = newLocation.Longitude
            };

            return CreatedAtAction(nameof(GetLocationById), new { id = newLocation.Id }, response);
        }

        [HttpGet("getLocation/{id}")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            var response = new LocationResponse
            {
                Id = location.Id,
                Name = location.Name,
                URL = location.URL,
                Radius = location.Radius,
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };

            return Ok(response);
        }

        [HttpGet("getAllLocations")]
        public async Task<IActionResult> GetAllLocations()
        {
            var locations = await _context.Locations.ToListAsync();
            var response = locations.Select(location => new LocationResponse
            {
                Id = location.Id,
                Name = location.Name,
                URL = location.URL,
                Radius = location.Radius,
                Latitude = location.Latitude,
                Longitude = location.Longitude
            }).ToList();

            return Ok(response);
        }

        [HttpPost("business")]
        public async Task<IActionResult> CreatePost([FromForm] BusinessRequest request)
        {
            var email = User.FindFirst(TokenClaimsConstant.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null)
            {
                return BadRequest("Invalid CategoryId");
            }

            var locations = _context.Locations.Where(l => request.LocationIds.Contains(l.Id)).ToList();
            if (locations.Count != request.LocationIds.Count)
            {
                return BadRequest("One or more LocationIds are invalid");
            }

            var business = new Business
            {
                Name = request.Name,
                Question = request.Question,
                CorrectAnswer = request.CorrectAnswer,
                WrongAnswer1 = request.WrongAnswer1,
                WrongAnswer2 = request.WrongAnswer2,
                CategoryId = request.CategoryId,
                Locations = locations
            };

            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "businesses");
            Directory.CreateDirectory(directoryPath);
            var fileName = $"{Guid.NewGuid()}_{request.Image.FileName}";
            var filePath = Path.Combine(directoryPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.Image.CopyToAsync(stream);
            }

            business.Image = $"uploads/businesses/{fileName}";

            _context.Businesses.Add(business);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Business created" });
        }

        [HttpGet("getBusinesses")]
        public async Task<IActionResult> GetBusinesses()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";

            var businesses = await _context.Businesses
                .Include(b => b.Category)
                .Include(b => b.Locations)
                .ToListAsync();

            var response = businesses.Select(b => new BusinessResponse
            {
                Id = b.Id,
                Name = b.Name,
                CategoryId = b.CategoryId,
                Image = $"{baseUrl}/{b.Image}",
                Question = b.Question,
                CorrectAnswer = b.CorrectAnswer,
                WrongAnswer1 = b.WrongAnswer1,
                WrongAnswer2 = b.WrongAnswer2,
                Locations = b.Locations.Select(l => new LocationResponse
                {
                    Id = l.Id,
                    Name = l.Name,
                    URL = l.URL,
                    Radius = l.Radius,
                    Latitude = l.Latitude,
                    Longitude = l.Longitude
                }).ToList(),
                CategoryName = b.Category.Name
            }).ToList();

            return Ok(response);
        }

        [HttpPut("editBusiness/{id}")]
        public async Task<IActionResult> EditBusiness(int id, [FromForm] BusinessUpdateRequest request)
        {
            var business = await _context.Businesses.FindAsync(id);
            if (business == null)
            {
                return NotFound("Business not found");
            }

            // Update fields only if they are provided in the request
            if (!string.IsNullOrEmpty(request.Name))
            {
                business.Name = request.Name;
            }

            if (request.CategoryId.HasValue)
            {
                var category = await _context.Categories.FindAsync(request.CategoryId.Value);
                if (category == null)
                {
                    return BadRequest("Invalid CategoryId");
                }
                business.CategoryId = request.CategoryId.Value;
            }

            if (!string.IsNullOrEmpty(request.Question))
            {
                business.Question = request.Question;
            }

            if (!string.IsNullOrEmpty(request.CorrectAnswer))
            {
                business.CorrectAnswer = request.CorrectAnswer;
            }

            if (!string.IsNullOrEmpty(request.WrongAnswer1))
            {
                business.WrongAnswer1 = request.WrongAnswer1;
            }

            if (!string.IsNullOrEmpty(request.WrongAnswer2))
            {
                business.WrongAnswer2 = request.WrongAnswer2;
            }

            if (request.LocationIds != null && request.LocationIds.Any())
            {
                var locations = _context.Locations.Where(l => request.LocationIds.Contains(l.Id)).ToList();
                if (locations.Count != request.LocationIds.Count)
                {
                    return BadRequest("One or more LocationIds are invalid");
                }
                business.Locations = locations;
            }

            if (request.Image != null)
            {
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "businesses");
                Directory.CreateDirectory(directoryPath);
                var fileName = $"{Guid.NewGuid()}_{request.Image.FileName}";
                var filePath = Path.Combine(directoryPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.Image.CopyToAsync(stream);
                }

                business.Image = $"uploads/businesses/{fileName}";
            }

            _context.Businesses.Update(business);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Business updated" });
        }

    }
}
