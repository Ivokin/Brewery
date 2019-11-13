using Brewery.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Brewery.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                   : base(options)
        { }

        public virtual DbSet<Brew> Brews { get; set; }

        public virtual DbSet<Produced> Produced { get; set; }

        public virtual DbSet<Recipe> Recipes { get; set; }

        public virtual DbSet<Resource> Resources { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Recipe>()
                .HasKey(x => new { x.ResourceId, x.BrewId });
        }
    }
}
