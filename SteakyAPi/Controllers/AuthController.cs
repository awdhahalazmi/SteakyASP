using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreakyAPi.Model;
using StreakyAPi.Model.Auth;
using StreakyAPi.Model.Reponses;
using StreakyAPi.Model.Request;
using StreakyAPi.Model.Token;
using static System.Net.Mime.MediaTypeNames;

namespace StreakyAPi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly TokenService _service;
        private readonly StreakyContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(TokenService service, StreakyContext context, IConfiguration configuration)
        {
            _service = service;
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("signup")]
        public IActionResult Signup(SignUpRequest request)
        {
            if (_context.Users.Any(u => u.Email == request.Email))
            {
                return Conflict("Username already exists");
            }

            var gender = _context.Genders.FirstOrDefault(g => g.GenderName == request.GenderName);
            if (gender == null)
            {
                return BadRequest("Invalid GenderName");
            }

            var categories = _context.Categories.Where(c => request.CategoryIds.Contains(c.Id)).ToList();
            if (categories.Count != request.CategoryIds.Count)
            {
                return BadRequest("One or more CategoryIds are invalid");
            }

            var newUser = UserAccount.Create(
                request.Email,
                request.Password,
                gender.Id,
                request.CategoryIds,
                _context,
                request.IsAdmin
            );

            _context.Users.Add(newUser);
            _context.SaveChanges();

            var (isValid, token) = _service.GenerateToken(request.Email, request.Password);

            if (!isValid)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error generating token");
            }

            return Ok(new { Token = token });
        }


        [HttpPost("login")]
        public IActionResult Login(LoginRequest loginDetails)
        {
            var response = _service.GenerateToken(loginDetails.Email, loginDetails.Password);
            if (response.IsValid)
            {
                return Ok(new UserLoginResponse { Token = response.Token });
            }
            return BadRequest("Username and/or Password is wrong");
        }
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            var email = User.FindFirst(TokenClaimsConstant.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";
            var userProfile = new ProfileResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                GenderId = user.GenderId,
                ImagePath = $"{baseUrl}/{user.ImagePath}",
                Points = user.Points


            };

            return Ok(userProfile);
        }


        [HttpPost("profile")]
        public async Task<IActionResult> EditProfile(EditProfileRequest request)
        {

            var email = User.FindFirst(TokenClaimsConstant.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound("User not found");
            }


            user.Name = request.Name;
            if (request.GenderId.HasValue)
            {
                user.GenderId = request.GenderId.Value;
            }
            if (request.Image != null)
            {


                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads", user.Id.ToString());
                Directory.CreateDirectory(uploadsDir);

                var filePath = Path.Combine(uploadsDir, request.Image.FileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await request.Image.CopyToAsync(stream);
                }

                user.ImagePath = $"uploads/{user.Id}/{request.Image.FileName}";
            }
            _context.SaveChanges();

            return Ok(new { Message = "Profile updated successfully" });
        }





        [HttpGet("categories")]
        public IActionResult GetCategoryIds()
        {
            var categoryIds = _context.Categories.ToList();
            return Ok(categoryIds);
        }

        [HttpPost("categories")]
        public IActionResult CreateCategory([FromBody] CategoryRequest request)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                return BadRequest("Category name is required");
            }

            var newCategory = new Category
            {
                Name = request.Name
            };

            _context.Categories.Add(newCategory);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCategoryById), new { id = newCategory.Id }, newCategory);
        }

        [HttpGet("categories/{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }
        [HttpPost("genders")]
        public IActionResult CreateGender([FromBody] GenderRequest request)
        {
            if (string.IsNullOrEmpty(request.GenderName))
            {
                return BadRequest("Gender name is required");
            }

            var newGender = new Gender
            {
                GenderName = request.GenderName
            };

            _context.Genders.Add(newGender);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetGenderById), new { id = newGender.Id }, newGender);
        }

        [HttpGet("genders/{id}")]
        public IActionResult GetGenderById(int id)
        {
            var gender = _context.Genders.Find(id);
            if (gender == null)
            {
                return NotFound();
            }

            return Ok(gender);
        }

        [HttpGet("genders")]
        public IActionResult GetGenders()
        {
            var genders = _context.Genders.ToList();
            return Ok(genders);
        }


        [HttpPost("addFriend/{userId}")]
        public IActionResult SendFriendRequest(int userId)
        {
            var email = User.FindFirst(TokenClaimsConstant.Email).Value;
            var currentUser = _context.Users.FirstOrDefault(u => u.Email == email);
            if (currentUser == null)
            {
                return NotFound("User not found");
            }

            var userToFollow = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (userToFollow == null)
            {
                return NotFound("User to follow not found");
            }

            var existingRequest = _context.FriendRequests
                .FirstOrDefault(fr => fr.RequesterId == currentUser.Id && fr.ReceiverId == userToFollow.Id);

            if (existingRequest != null)
            {
                return BadRequest("Friend request already sent");
            }

            var friendRequest = new FriendRequest
            {
                RequesterId = currentUser.Id,
                ReceiverId = userToFollow.Id,
                RequestDate = DateTime.Now,
                IsAccepted = false
            };

            _context.FriendRequests.Add(friendRequest);
            _context.SaveChanges();

            return Ok(new { Message = "Friend request sent successfully" });
        }



        [HttpPost("acceptFriendRequest/{requestId}")]
        public IActionResult AcceptFriendRequest(int requestId)
        {
            var email = User.FindFirst(TokenClaimsConstant.Email).Value;
            var currentUser = _context.Users.Include(r => r.Friends).FirstOrDefault(u => u.Email == email);
            if (currentUser == null)
            {
                return NotFound("User not found");
            }

            var friendRequest = _context.FriendRequests
                .Include(fr => fr.Requester)
                .FirstOrDefault(fr => fr.Id == requestId && fr.ReceiverId == currentUser.Id);

            if (friendRequest == null)
            {
                return NotFound("Friend request not found");
            }

            if (friendRequest.IsAccepted)
            {
                return BadRequest("Friend request already accepted");
            }

            friendRequest.IsAccepted = true;
            currentUser.Friends.Add(friendRequest.Requester);
            friendRequest.Requester.Friends.Add(currentUser);

            _context.SaveChanges();

            return Ok(new { Message = "Friend request accepted successfully" });
        }

        [HttpGet("friendRequests")]
        public IActionResult GetFriendRequests()
        {
            var email = User.FindFirst(TokenClaimsConstant.Email).Value;
            var currentUser = _context.Users.FirstOrDefault(u => u.Email == email);
            if (currentUser == null)
            {
                return NotFound("User not found");
            }

            var friendRequests = _context.FriendRequests
                .Where(fr => fr.ReceiverId == currentUser.Id && !fr.IsAccepted)
                .Include(fr => fr.Requester)
                .ToList();

            var friendRequestResponses = friendRequests.Select(fr => new
            {
                fr.Id,
                RequesterId = fr.Requester.Id,
                RequesterName = fr.Requester.Name,
                RequesterEmail = fr.Requester.Email,
                fr.RequestDate
            }).ToList();

            return Ok(friendRequestResponses);
        }

        [HttpGet("friends")]
        public IActionResult GetFriends()
        {
            var email = User.FindFirst(TokenClaimsConstant.Email).Value;
            var currentUser = _context.Users
                .Include(u => u.Friends)
                .FirstOrDefault(u => u.Email == email);

            if (currentUser == null)
            {
                return NotFound("User not found");
            }

            var friends = currentUser.Friends.Select(f => new
            {
                f.Id,
                f.Name,
                f.Email,
                f.UserStreaks
            }).ToList();

            return Ok(friends);


        }
        [HttpPost("updatePoints")]
        public IActionResult UpdatePoints(UpdatePointsRequest request)
        {
            var email = User.FindFirst(TokenClaimsConstant.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.Points += request.Points;
            _context.SaveChanges();

            return Ok(new { Message = "Points updated successfully" });
        }
    }
}