using System;
using GeolocationAPI.DTO;
using GeolocationAPI.Persistence.Entities;

namespace GeolocationAPI.Converters.Interfaces
{
    public interface IGeolocationDataConverter
    {
        GeolocationData Convert(RemoteGeolocationData remoteGeolocationData);
        GeolocationDataResource Convert(GeolocationData localGeolocationData);
    }
}