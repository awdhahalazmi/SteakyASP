using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreakyAPi.Model;
using StreakyAPi.Model.Request;
using StreakyAPi.Model.Responses;
using System.Linq;
using System.Threading.Tasks;

namespace StreakyAPi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class RewardsController : ControllerBase
    {
        private readonly StreakyContext _context;

        public RewardsController(StreakyContext context)
        {
            _context = context;
        }

        [HttpPost("addReward")]
        public async Task<IActionResult> AddReward([FromBody] RewardRequest request)
        {
            var streak = await _context.Streaks.FindAsync(request.StreakId);
            if (streak == null)
            {
                return BadRequest("Invalid StreakId");
            }

            var business = await _context.Businesses.FindAsync(request.BusinessId);
            if (business == null)
            {
                return BadRequest("Invalid BusinessId");
            }

            var reward = new Reward
            {
                StreakId = request.StreakId,
                Description = request.Description,
                PointsClaimed = request.PointsClaimed,
                BusinessId = request.BusinessId
            };

            _context.Rewards.Add(reward);
            await _context.SaveChangesAsync();

            var response = new RewardResponse
            {
                Id = reward.Id,
                StreakId = reward.StreakId,
                StreakTitle = streak.Title,
                Description = reward.Description,
                PointsClaimed = reward.PointsClaimed,
                BusinessId = reward.BusinessId,
                BusinessName = business.Name,
                BusinessImage = business.Image  // Include business image in the response
            };

            return Ok(new { Message = "Reward added successfully", Reward = response });
        }

        [HttpPut("editReward/{rewardId}")]
        public async Task<IActionResult> EditReward(int rewardId, [FromBody] RewardRequest request)
        {
            var reward = await _context.Rewards.FindAsync(rewardId);
            if (reward == null)
            {
                return NotFound("Reward not found");
            }

            var streak = await _context.Streaks.FindAsync(request.StreakId);
            if (streak == null)
            {
                return BadRequest("Invalid StreakId");
            }

            var business = await _context.Businesses.FindAsync(request.BusinessId);
            if (business == null)
            {
                return BadRequest("Invalid BusinessId");
            }

            reward.StreakId = request.StreakId;
            reward.Description = request.Description;
            reward.PointsClaimed = request.PointsClaimed;
            reward.BusinessId = request.BusinessId;

            _context.Rewards.Update(reward);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Reward updated" });
        }

        [HttpDelete("deleteReward/{rewardId}")]
        public async Task<IActionResult> DeleteReward(int rewardId)
        {
            var reward = await _context.Rewards.FindAsync(rewardId);
            if (reward == null)
            {
                return NotFound("Reward not found");
            }

            _context.Rewards.Remove(reward);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Reward deleted" });
        }

        [HttpGet("getAllRewards")]
        public async Task<IActionResult> GetAllRewards()
        {
            var rewards = await _context.Rewards
                .Select(r => new RewardResponse
                {
                    Id = r.Id,
                    StreakId = r.StreakId,
                    StreakTitle = r.Streak.Title,
                    Description = r.Description,
                    PointsClaimed = r.PointsClaimed,
                    BusinessId = r.BusinessId,
                    BusinessName = r.Business.Name,
                    BusinessImage = r.Business.Image  
                })
                .ToListAsync();

            return Ok(rewards);
        }
    }
}
