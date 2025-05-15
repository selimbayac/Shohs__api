using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shosh.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserFollow> UserFollows { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Ban> Bans { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        
        


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "Host=localhost;Port=5432;Database=ShoshDB;Username=postgres;Password=Selim123";
                optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly("Shosh.Data"));
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            // 📌 **User - Entry**
            modelBuilder.Entity<User>()
                .HasMany(u => u.Entries)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 📌 **User - Comment**
            modelBuilder.Entity<User>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 📌 **User - Like**
            modelBuilder.Entity<User>()
                .HasMany(u => u.Likes)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 📌 **Entry - Comment**
            modelBuilder.Entity<Entry>()
                .HasMany(e => e.Comments)
                .WithOne(c => c.Entry)
                .HasForeignKey(c => c.EntryId)
                .OnDelete(DeleteBehavior.Cascade);

            // 📌 **Entry - Like (Entry'ye beğeni eklenebilir)**
            modelBuilder.Entity<Entry>()
                .HasMany(e => e.Likes)
                .WithOne(l => l.Entry)
                .HasForeignKey(l => l.EntryId)
                .OnDelete(DeleteBehavior.Cascade);

            // 📌 **Comment - Like (Yorumlara beğeni eklenebilir)**
            modelBuilder.Entity<Comment>()
                .HasMany(c => c.Likes)
                .WithOne(l => l.Comment)
                .HasForeignKey(l => l.CommentId)
                .OnDelete(DeleteBehavior.Cascade);
            //takip
            modelBuilder.Entity<UserFollow>()
                   .HasOne(uf => uf.Follower)
                   .WithMany(u => u.Following)
                   .HasForeignKey(uf => uf.FollowerId)
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserFollow>()
                .HasOne(uf => uf.Following)
                .WithMany(u => u.Followers)
                .HasForeignKey(uf => uf.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
