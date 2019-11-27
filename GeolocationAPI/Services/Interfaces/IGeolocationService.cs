using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeolocationAPI.DTO.Remote;
using GeolocationAPI.Persistence.Entities;

namespace GeolocationAPI.Services.Interfaces
{
    public interface IGeolocationService
    {
        Task<GeolocationData> AddAsync(RemoteGeolocationData remoteGeolocationData);
        Task<List<GeolocationData>> GetAllAsync();
        Task<GeolocationData> GetAsync(string ipAddress);
        Task DeleteAsync(string ipAddress);
    }
}