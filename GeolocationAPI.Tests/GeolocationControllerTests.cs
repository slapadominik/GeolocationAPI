using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using GeolocationAPI.Persistence.Entities;
using NUnit.Framework;

namespace GeolocationAPI.Tests
{
    [TestFixture]
    public class GeolocationControllerTests
    {
        private APIWebApplicationFactory _factory;
        private HttpClient _httpClient;

        [OneTimeSetUp]
        public void GivenARequestToTheController()
        {
            _factory = new APIWebApplicationFactory();
            _httpClient = _factory.CreateClient();
        }

        [Test]
        public async Task GetAll_ShouldReturnAllGeolocationDataFromDatabase()
        {
            //Act
            var result = await _httpClient.GetAsync("/api/geolocation");

            //Assert
            var geolocations = await result.Content.ReadAsAsync<IEnumerable<GeolocationData>>();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Content.Headers.ContentType.MediaType.Should().Be("application/json");
            geolocations.Should().NotBeNull();
            geolocations.Count().Should().Be(2);
        }

        [TestCase("10.10.10.256")]
        [TestCase("10.10.256.0")]
        [TestCase("10.256.0.0")]
        [TestCase("256.0.0.0")]
        [TestCase("80.80")]
        [TestCase("80.80.80")]
        [TestCase("80.80.80.a")]
        [TestCase("a.a.a.a")]
        public async Task Get_GivenInvalidIpAddress_ShouldReturnBadRequest(string ipAddress)
        {
            //Act
            var result = await _httpClient.GetAsync($"/api/geolocation/{ipAddress}");

            //Assert
            var errorMessgae = await result.Content.ReadAsStringAsync();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Content.Headers.ContentType.MediaType.Should().Be("text/plain");
            errorMessgae.Should().Contain("Invalid IpAddress format");
        }

        [Test]
        public async Task Get_GivenValidIpAddress_ShouldReturnOKAndGeolocationData()
        {
            //Arrange
            string ipAddress = "80.80.80.80";
            var expectedResult = new GeolocationData
            {
                IpAddress = ipAddress,
                City = "Warszawa",
                ContinentCode = "EU",
                CountryCode = "PL",
                Latitude = 52.234,
                Longitude = 21.12,
                CountryName = "Poland",
                IpAddressType = "ipv4"
            };

            //Act
            var result = await _httpClient.GetAsync($"/api/geolocation/{ipAddress}");

            //Assert
            var geolocationData = await result.Content.ReadAsAsync<GeolocationData>();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Content.Headers.ContentType.MediaType.Should().Be("application/json");
            geolocationData.Should().NotBeNull();
            geolocationData.Should().BeEquivalentTo(expectedResult);
        }
    }
}