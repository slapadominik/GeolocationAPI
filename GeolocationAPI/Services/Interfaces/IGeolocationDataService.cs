﻿using System.Threading.Tasks;
using GeolocationAPI.Controllers;
using GeolocationAPI.DTO;

namespace GeolocationAPI.Services.Interfaces
{
    public interface IGeolocationDataService
    {
        Task<RemoteGeolocationData> GetByIpAddressAsync(string ipAddress);
    }
}