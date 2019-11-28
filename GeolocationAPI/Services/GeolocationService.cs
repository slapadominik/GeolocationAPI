using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeolocationAPI.Converters.Interfaces;
using GeolocationAPI.DTO;
using GeolocationAPI.Exceptions;
using GeolocationAPI.Persistence.Entities;
using GeolocationAPI.Persistence.Repositories.Interfaces;
using GeolocationAPI.Services.Interfaces;

namespace GeolocationAPI.Services
{
    public class GeolocationService : IGeolocationService
    {
        private readonly IGeolocationDataRepository _geolocationDataRepository;
        private readonly IGeolocationDataConverter _geolocationDataConverter;

        public GeolocationService(IGeolocationDataRepository geolocationDataRepository, IGeolocationDataConverter geolocationDataConverter)
        {
            _geolocationDataRepository = geolocationDataRepository;
            _geolocationDataConverter = geolocationDataConverter;
        }

        public async Task<GeolocationData> AddAsync(RemoteGeolocationData remoteGeolocationData)
        {
            if (await _geolocationDataRepository.GetByIpAsync(remoteGeolocationData.IpAddress) != null)
            {
                throw new EntityDuplicateException($"Geolocation data for IP {remoteGeolocationData.IpAddress} already exists.");
            }

            var geolocationData = _geolocationDataConverter.Convert(remoteGeolocationData);
            await _geolocationDataRepository.AddAsync(geolocationData);
            return geolocationData;
        }

        public Task<List<GeolocationData>> GetAllAsync()
        {
            return _geolocationDataRepository.GetAllAsync();
        }

        public Task<GeolocationData> GetAsync(string ipAddress)
        {
            return _geolocationDataRepository.GetByIpAsync(ipAddress);
        }

        public async Task DeleteAsync(string ipAddress)
        {
            var geolocationData = await _geolocationDataRepository.GetByIpAsync(ipAddress);
            if (geolocationData != null)
            {
                await _geolocationDataRepository.DeleteAsync(geolocationData);
            }
        }
    }
}