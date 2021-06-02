using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class PropertyContext : DbContext
    {
        public PropertyContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ConstructionYear> ConstructionYears { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<RealEstate> RealEstates { get; set; }
        public DbSet<RealEstateType> RealEstateTypes { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Rating>()
                .HasOne(x => x.ByUser)
                .WithMany(x => x.RatingsByMe)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Rating>()
                .HasOne(x => x.AboutUser)
                .WithMany(x => x.Ratings)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
