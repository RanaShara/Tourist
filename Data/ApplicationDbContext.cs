using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TouristP.Models;

namespace TouristP.Data
{
    
    
        public class ApplicationDbContext : DbContext
        {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Package> Package {  get; set; }
        public DbSet<City> City { get; set; }
    }
}
