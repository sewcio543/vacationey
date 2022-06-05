using Backend.Models.DbModels;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;


namespace Backend.Models.DbModels
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) { }

        public DbSet<Country>? Country { get; set; }
        //public DbSet<Region>? Region { get; set; }
        public DbSet<City>? City { get; set; }
        public DbSet<Hotel>? Hotel { get; set; }
        public DbSet<Offer>? Offer { get; set; }
        public DbSet<User>? User { get; set; }
        public DbSet<Admin>? Admin { get; set; }

  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

    }

}
