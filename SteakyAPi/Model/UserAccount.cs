using StreakyAPi.Model.Request;
using System.Collections.Generic;

namespace StreakyAPi.Model
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; } = "";
        public int GenderId { get; set; }
        public string ImagePath { get; set; } = "";
        public double Points { get; set; } = 0;
        //public int CategoryId { get; set; }
        public Gender Gender { get; set; }
       // public Category Category { get; set; }
        public bool IsAdmin { get; set; }

        public ICollection<Category> Categories { get; set; } = new List<Category>();

        public ICollection<FriendRequest> SentFriendRequests { get; set; } = new List<FriendRequest>();
        public ICollection<FriendRequest> ReceivedFriendRequests { get; set; } = new List<FriendRequest>();
        public ICollection<UserAccount> Friends { get; set; } = new List<UserAccount>();

        private UserAccount() { }

        public static UserAccount Create(string email, string password, int genderId, List<int> categoryIds, StreakyContext context, bool isAdmin = false)
        {
            var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password, BCrypt.Net.HashType.SHA384);
            var categories = context.Categories.Where(c => categoryIds.Contains(c.Id)).ToList();
            return new UserAccount
            {
                Email = email,
                Password = hashedPassword,
                GenderId = genderId,
                Categories = categories,
                IsAdmin = isAdmin
            };
        }

        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, this.Password);
        }
    }
}