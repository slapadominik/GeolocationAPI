using System.Threading.Tasks;
using GeolocationAPI.Persistence.Entities;
using GeolocationAPI.Persistence.Repositories.Interfaces;

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
    }
}