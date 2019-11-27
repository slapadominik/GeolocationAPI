using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeolocationAPI.Persistence.Entities;

namespace GeolocationAPI.Persistence.Repositories.Interfaces
{
    public interface IGeolocationDataRepository
    {
        Task AddAsync(GeolocationData geolocationData);
        Task DeleteAsync(GeolocationData geolocationData);
        Task<GeolocationData> GetByIpAsync(string ipAddress);
        Task<List<GeolocationData>> GetAllAsync();
    }
}