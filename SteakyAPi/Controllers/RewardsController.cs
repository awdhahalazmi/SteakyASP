using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreakyAPi.Model;
using StreakyAPi.Model.Request;
using StreakyAPi.Model.Responses;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using StreakyAPi.Model.Token;


namespace StreakyAPi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class RewardsController : ControllerBase
    {
        private readonly StreakyContext _context;
        private readonly IConfiguration _configuration;



        public RewardsController(StreakyContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        [HttpPost("addReward")]
        public async Task<IActionResult> AddReward([FromBody] RewardRequest request)
        {
            var email = User.FindFirst(TokenClaimsConstant.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound("User not found");
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
                BusinessImage = business.Image  
            };

            return Ok(new { Message = "Reward added successfully"});
        }

        [HttpPut("editReward/{rewardId}")]
        public async Task<IActionResult> EditReward(int rewardId, [FromBody] RewardRequest request)
        {
            var email = User.FindFirst(TokenClaimsConstant.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound("User not found");
            }
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
            var email = User.FindFirst(TokenClaimsConstant.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound("User not found");
            }
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
            var email = User.FindFirst(TokenClaimsConstant.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";

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
                    BusinessImage = $"{baseUrl}/{r.Business.Image}"  
                })
                .ToListAsync();

            return Ok(rewards);
        }
        [HttpGet("getReward/{rewardId}")]
        public async Task<IActionResult> GetRewardById(int rewardId)
        {
            var email = User.FindFirst(TokenClaimsConstant.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var reward = await _context.Rewards
                .Include(r => r.Streak)
                .Include(r => r.Business)
                .FirstOrDefaultAsync(r => r.Id == rewardId);

            if (reward == null)
            {
                return NotFound("Reward not found");
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";
            var response = new RewardResponse
            {
                Id = reward.Id,
                StreakId = reward.StreakId,
                StreakTitle = reward.Streak.Title,
                Description = reward.Description,
                PointsClaimed = reward.PointsClaimed,
                BusinessId = reward.BusinessId,
                BusinessName = reward.Business.Name,
                BusinessImage = $"{baseUrl}/{reward.Business.Image}"
            };

            return Ok(response);
        }

    }
}
