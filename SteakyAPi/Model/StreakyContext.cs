using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using StreakyAPi.Model.Request;
using StreakyAPi.Model.Streak;
using System.Diagnostics.Metrics;
using System.Net.NetworkInformation;

namespace StreakyAPi.Model
{
    public class StreakyContext : DbContext
    {
        public StreakyContext(DbContextOptions<StreakyContext> options) : base(options)
        {
        }

        public DbSet<UserAccount> Users { get; set; }
        //public DbSet<Post> Posts { get; set; }
        //public DbSet<Pin> Pins { get; set; }
        //public DbSet<Rating> Ratings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<UserStreak> UserStreaks { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Streaks> Streaks { get; set; }
        public DbSet<Reward> Rewards { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAccount>()
           .HasOne(u => u.Gender)
           .WithMany()
           .HasForeignKey(u => u.GenderId)
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserAccount>()
                .HasMany(u => u.Categories)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "UserCategory",
                    j => j.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
                    j => j.HasOne<UserAccount>().WithMany().HasForeignKey("UserId"))
                .ToTable("UserCategories");

            modelBuilder.Entity<UserAccount>()
             .HasMany(u => u.Friends)
             .WithMany()
             .UsingEntity<Dictionary<string, object>>(
                 "UserFriend",
                 j => j.HasOne<UserAccount>().WithMany().HasForeignKey("FriendId").OnDelete(DeleteBehavior.Restrict),
                 j => j.HasOne<UserAccount>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Restrict));

            modelBuilder.Entity<FriendRequest>()
                .HasOne(fr => fr.Requester)
                .WithMany(u => u.SentFriendRequests)
                .HasForeignKey(fr => fr.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendRequest>()
                .HasOne(fr => fr.Receiver)
                .WithMany(u => u.ReceivedFriendRequests)
                .HasForeignKey(fr => fr.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserStreak>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserStreaks)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserStreak>()
                .HasOne(us => us.Streak)
                .WithMany(s => s.UserStreaks)
                .HasForeignKey(us => us.StreakId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Business>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Businesses)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Streaks>()
                .HasMany(s => s.Businsses)
                .WithMany(b => b.Streaks)
                .UsingEntity<Dictionary<string, object>>(
                    "StreakBusiness",
                    j => j.HasOne<Business>().WithMany().HasForeignKey("BusinessId"),
                    j => j.HasOne<Streaks>().WithMany().HasForeignKey("StreakId"));

            modelBuilder.Entity<Reward>()
                .HasOne(r => r.Streak)
                .WithMany(s => s.Rewards)
                .HasForeignKey(r => r.StreakId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reward>()
                .HasOne(r => r.Business)
                .WithMany(b => b.Rewards)
                .HasForeignKey(r => r.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}