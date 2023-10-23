using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tabemory.Models;

namespace Tabemory.Data
{
    public class TabemoryDbContext : IdentityDbContext<ApplicationUser>
    {
        public TabemoryDbContext(DbContextOptions<TabemoryDbContext> options)
            : base(options)
        {

        }

        public DbSet<Tabemory.Models.Record> Record { get; set; } = default!;

        public DbSet<Tabemory.Models.Review>? Review { get; set; }

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