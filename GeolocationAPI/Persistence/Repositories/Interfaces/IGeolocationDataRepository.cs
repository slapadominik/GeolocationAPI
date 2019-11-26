using System.Threading.Tasks;
using GeolocationAPI.Persistence.Entities;

namespace GeolocationAPI.Persistence.Repositories.Interfaces
{
    public interface IGeolocationDataRepository
    {
        Task AddAsync(GeolocationData geolocationData);
    }
}