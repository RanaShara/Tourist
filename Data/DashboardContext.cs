using Microsoft.EntityFrameworkCore;
using TouristP.Models; 
namespace TouristP.Data
{
    public class DashboardContext : DbContext
    {
        public DashboardContext(DbContextOptions<DashboardContext> options)
            : base(options)
        {
        }

        public DbSet<Package> Package { get; set; }
        public DbSet<City> City { get; set; }
    }
}
