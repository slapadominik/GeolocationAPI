using System;

namespace GeolocationAPI.DTO
{
    public class GeolocationDataResource
    {
        public string IpAddress { get; set; }
        public string IpAddressType { get; set; }
        public string ContinentCode { get; set; }
        public string ContinentName { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string City { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string ZipCode { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}