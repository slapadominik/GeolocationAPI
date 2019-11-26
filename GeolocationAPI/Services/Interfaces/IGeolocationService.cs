using System;
using System.Threading.Tasks;
using GeolocationAPI.Controllers;
using GeolocationAPI.DTO;
using GeolocationAPI.DTO.Remote;

namespace GeolocationAPI.Services.Interfaces
{
    public interface IGeolocationService
    {
        Task<Guid> AddGeolocationDataAsync(RemoteGeolocationData remoteGeolocationData);
    }
}