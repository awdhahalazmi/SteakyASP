using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreakyAPi.Model;
using StreakyAPi.Model.Request;
using StreakyAPi.Model.Streak;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Linq;
using StreakyAPi.Model.Reponses;
using StreakyAPi.Model.Responses;

namespace StreakyAPi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class SecretExperienceController : ControllerBase
    {
        private readonly StreakyContext _context;

        public SecretExperienceController(StreakyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSecretExperiences()
        {
            var secretExperiences = await _context.SecretExperiences
                .Include(se => se.Business)
                .Select(se => new SecretExperienceResponse
                {
                    Id = se.Id,
                    StartDate = se.StartDate,
                    EndDate = se.EndDate,
                    Title = se.Title,
                    Description = se.Description,
                    StreakClaimed = se.StreakClaimed,
                    BusinessName = se.Business.Name,
                    BusinessImage = se.Business.Image

                })
                .ToListAsync();

            return Ok(secretExperiences);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSecretExperienceById(int id)
        {
            var secretExperience = await _context.SecretExperiences
                .Include(se => se.Business)
                .Select(se => new SecretExperienceResponse
                {
                    Id = se.Id,
                    StartDate = se.StartDate,
                    EndDate = se.EndDate,
                    Title = se.Title,
                    Description = se.Description,
                    StreakClaimed = se.StreakClaimed,
                    BusinessName = se.Business.Name,
                    BusinessImage = se.Business.Image

                })
                .FirstOrDefaultAsync(se => se.Id == id);

            if (secretExperience == null)
            {
                return NotFound();
            }

            return Ok(secretExperience);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSecretExperience([FromBody] SecretExperienceRequest request)
        {
            var business = await _context.Businesses.FindAsync(request.BusinessId);
            if (business == null)
            {
                return BadRequest("Invalid BusinessId");
            }

            var secretExperience = new SecretExperience
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Title = request.Title,
                Description = request.Description,
                StreakClaimed = request.StreakClaimed,
                BusinessId = request.BusinessId,
                Business = business
            };

            _context.SecretExperiences.Add(secretExperience);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Secret experience has been added" });
        }
        [HttpPut("editSecretExperience/{id}")]
        public async Task<IActionResult> UpdateSecretExperience(int id, [FromBody] SecretExperienceEditRequest updatedSecretExperience)
        {
            var existingSecretExperience = await _context.SecretExperiences
                .Include(se => se.Business)
                .FirstOrDefaultAsync(se => se.Id == id);

            if (existingSecretExperience == null)
            {
                return NotFound("Secret experience not found");
            }

            if (updatedSecretExperience.StartDate.HasValue)
            {
                existingSecretExperience.StartDate = updatedSecretExperience.StartDate.Value;
            }
            if (updatedSecretExperience.EndDate.HasValue)
            {
                existingSecretExperience.EndDate = updatedSecretExperience.EndDate.Value;
            }
            if (!string.IsNullOrEmpty(updatedSecretExperience.Title))
            {
                existingSecretExperience.Title = updatedSecretExperience.Title;
            }
            if (!string.IsNullOrEmpty(updatedSecretExperience.Description))
            {
                existingSecretExperience.Description = updatedSecretExperience.Description;
            }
            if (updatedSecretExperience.StreakClaimed.HasValue)
            {
                existingSecretExperience.StreakClaimed = updatedSecretExperience.StreakClaimed.Value;
            }
            if (updatedSecretExperience.BusinessId.HasValue)
            {
                var business = await _context.Businesses.FindAsync(updatedSecretExperience.BusinessId.Value);
                if (business == null)
                {
                    return BadRequest("Invalid BusinessId");
                }
                existingSecretExperience.BusinessId = updatedSecretExperience.BusinessId.Value;
                existingSecretExperience.Business = business;
            }

            _context.SecretExperiences.Update(existingSecretExperience);
            await _context.SaveChangesAsync();

            var response = new SecretExperienceEditResponse
            {
                Id = existingSecretExperience.Id,
                StartDate = existingSecretExperience.StartDate,
                EndDate = existingSecretExperience.EndDate,
                Title = existingSecretExperience.Title,
                Description = existingSecretExperience.Description,
                StreakClaimed = existingSecretExperience.StreakClaimed,
                BusinessName = existingSecretExperience.Business.Name,
                BusinessImage = existingSecretExperience.Business.Image
            };

            return Ok(new { Message = "Secret experience updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSecretExperience(int id)
        {
            var secretExperience = await _context.SecretExperiences.FindAsync(id);

            if (secretExperience == null)
            {
                return NotFound("Secret experience not found");
            }

            _context.SecretExperiences.Remove(secretExperience);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Secret experience deleted successfully" });
        }
    }
}
