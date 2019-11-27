using System;
using System.Linq;
using GeolocationAPI.Validators.Interfaces;

namespace GeolocationAPI.Validators
{
    public class IpAddressValidator : IIpAddressValidator
    {
        public bool IsValid(string ipAddress)
        {
            var parts = ipAddress.Split('.');

            return parts.Length == 4
                           && !parts.Any(
                               x =>
                               {
                                   if (!Int32.TryParse(x, out var y))
                                   {
                                       return true;
                                   }
                                   return y > 255 || y < 0;
                               });
        }
    }
}