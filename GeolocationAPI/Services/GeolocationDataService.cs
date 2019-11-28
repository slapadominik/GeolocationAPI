using System.Threading.Tasks;
using GeolocationAPI.Controllers;
using GeolocationAPI.DTO;
using GeolocationAPI.Exceptions;
using GeolocationAPI.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace GeolocationAPI.Services
{
    public class GeolocationDataService : IGeolocationDataService
    {
        private readonly IRestClient _restClient;
        private readonly IConfiguration _configuration;

        public GeolocationDataService(IConfiguration configuration)
        {
            _configuration = configuration;
            _restClient = new RestClient("http://api.ipstack.com/");
        }

        public async Task<RemoteGeolocationData> GetByIpAddressAsync(string ipAddress)
        {
            var request = new RestRequest("{ipAddress}", Method.GET);
            request.AddUrlSegment("ipAddress", ipAddress);
            request.AddParameter("access_key", _configuration.GetSection("ApiKeys")["IpStack"]);
            var response = await _restClient.ExecuteGetTaskAsync<RemoteGeolocationData>(request);
            if (!response.IsSuccessful)
            {
                throw new RemoteApiException(response.ErrorMessage, response.ErrorException);
            }
            if (response.Data.City == null 
                || response.Data.CountryCode == null 
                || response.Data.ZipCode == null)
            {
                return null;
            }
            return response.Data;
        }
    }
}