using System;
using System.Threading.Tasks;
using GeolocationAPI.Controllers;
using GeolocationAPI.Converters.Interfaces;
using GeolocationAPI.DTO;
using GeolocationAPI.DTO.Remote;
using GeolocationAPI.Persistence.Repositories;
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

        public async Task<Guid> AddGeolocationDataAsync(RemoteGeolocationData remoteGeolocationData)
        {
            var geolocationId = Guid.NewGuid();
            await _geolocationDataRepository.AddAsync(_geolocationDataConverter.Convert(geolocationId, remoteGeolocationData));
            return geolocationId;
        }
    }
}