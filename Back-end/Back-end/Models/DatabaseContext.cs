using Backend.Models.DbModels;
using System.Data.Entity;

namespace BookApp.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(): base() { }

        public DbSet<Country> Country { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Hotel> Hotel { get; set; }
        public DbSet<Offer> Offer { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Admin> Admin { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlite($"Filename=vacationey.db");

    }

}
