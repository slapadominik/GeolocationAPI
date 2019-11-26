using System;
using System.ComponentModel.DataAnnotations;

namespace GeolocationAPI.Persistence.Entities
{
    public class GeolocationData
    {
        public Guid Id { get; set; }
        public string IpAddress { get; set; }
        public string IpAddressType { get; set; }
        [MinLength(2)]
        [MaxLength(2)]
        public string ContinentCode { get; set; }
        public string ContinentName { get; set; }
        [MinLength(2)]
        [MaxLength(2)]
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string City { get; set; }
        [MaxLength(10)]
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string ZipCode { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}