using Backend.Models.DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.ModelConfiguration.Conventions;
//using System.Data.Entity;


namespace Backend.Models.DbModels
{
    public class DatabaseContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        private readonly DbContextOptions _options;

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            _options = options;
        }

        public DbSet<Country>? Country { get; set; }
        public DbSet<City>? City { get; set; }
        public DbSet<Hotel>? Hotel { get; set; }
        public DbSet<Offer>? Offer { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Offer>().Property(o => o.Price).HasConversion<double>();

            modelBuilder.Entity<Offer>()
           .Property(o => o.DateFrom)
           .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Offer>()
           .Property(o => o.DateTo)
           .HasDefaultValueSql("getdate()");

        }

    }

}

