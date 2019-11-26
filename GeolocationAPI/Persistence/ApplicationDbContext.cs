using GeolocationAPI.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeolocationAPI.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<GeolocationData> GeolocationData { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}