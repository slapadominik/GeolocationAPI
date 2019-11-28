using RestSharp.Deserializers;

namespace GeolocationAPI.DTO
{
    public class RemoteGeolocationData 
    {
        [DeserializeAs(Name = "ip")]
        public string IpAddress { get; set; }
        [DeserializeAs(Name = "type")]
        public string IpAddressType { get; set; }
        [DeserializeAs(Name = "continent_code")]
        public string ContinentCode { get; set; }
        [DeserializeAs(Name = "continent_name")]
        public string ContinentName { get; set; }
        [DeserializeAs(Name = "country_code")]
        public string CountryCode { get; set; }
        [DeserializeAs(Name = "country_name")]
        public string CountryName { get; set; }
        [DeserializeAs(Name = "city")]
        public string City { get; set; }
        [DeserializeAs(Name = "zip")]
        public string ZipCode { get; set; }
        [DeserializeAs(Name = "region_code")]
        public string RegionCode { get; set; }
        [DeserializeAs(Name = "region_name")]
        public string RegionName { get; set; }
        [DeserializeAs(Name = "longitude")]
        public double Longitude { get; set; }
        [DeserializeAs(Name = "latitude")]
        public double Latitude { get; set; }
    }
}