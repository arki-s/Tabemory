using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tabemory.Models;

namespace Tabemory.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Record>()
                .HasOne(e => e.User)
                .WithMany(e => e.Records)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<Review>()
                .HasOne(e => e.Record)
                .WithMany(e => e.Reviews)
                .OnDelete(DeleteBehavior.ClientCascade);

        }


    }
}