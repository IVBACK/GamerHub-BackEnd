using GamerHub.CORE.Models;
using Microsoft.EntityFrameworkCore;

namespace GamerHub.DATA.DBContext
{
    public class GamerHubDBContext : DbContext
    {
        public GamerHubDBContext(DbContextOptions<GamerHubDBContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        public DbSet<Post> Post { get; set; }

        public DbSet<Admin> Admin { get; set; }

        public DbSet<Game> Game { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Friendship>()
                .HasKey(f => new { f.RequestedById, f.RequestedToId });

            modelBuilder.Entity<Friendship>()
                .HasOne(fs => fs.RequestedBy)
                .WithMany(u => u.SentFriendships)
                .HasForeignKey(fs => fs.RequestedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>()
                .HasOne(fs => fs.RequestedTo)
                .WithMany(u => u.ReceivedFriendships)
                .HasForeignKey(fs => fs.RequestedToId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
