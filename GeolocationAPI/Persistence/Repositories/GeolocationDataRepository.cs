using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeolocationAPI.Persistence.Entities;
using GeolocationAPI.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeolocationAPI.Persistence.Repositories
{
    public class GeolocationDataRepository : IGeolocationDataRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GeolocationDataRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public Task AddAsync(GeolocationData geolocationData)
        {
            _applicationDbContext.GeolocationData.AddAsync(geolocationData);
            return _applicationDbContext.SaveChangesAsync();
        }

        public Task DeleteAsync(GeolocationData geolocationData)
        {
            _applicationDbContext.GeolocationData.Remove(geolocationData);
            return _applicationDbContext.SaveChangesAsync();
        }

        public Task<GeolocationData> GetByIpAsync(string ipAddress)
        {
            return _applicationDbContext.GeolocationData.SingleOrDefaultAsync(x => x.IpAddress == ipAddress);
        }

        public Task<List<GeolocationData>> GetAllAsync()
        {
            return _applicationDbContext.GeolocationData.ToListAsync();
        }
    }
}