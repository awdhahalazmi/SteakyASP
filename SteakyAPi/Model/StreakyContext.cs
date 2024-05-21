using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using StreakyAPi.Model.Request;
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
        }
    }
}