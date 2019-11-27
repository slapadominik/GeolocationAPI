using System;
using GeolocationAPI.DTO.Remote;
using GeolocationAPI.Persistence.Entities;

namespace GeolocationAPI.Converters.Interfaces
{
    public interface IGeolocationDataConverter
    {
        GeolocationData Convert(RemoteGeolocationData remoteGeolocationData);
    }
}