using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GeolocationAPI.DTO;
using GeolocationAPI.DTO.Remote;
using GeolocationAPI.Persistence.Entities;
using GeolocationAPI.Services.Interfaces;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GeolocationAPI.Tests
{
    [TestFixture]
    public class GeolocationControllerTests
    {
        private APIWebApplicationFactory _factory;
        private HttpClient _httpClient;
        private Mock<IGeolocationDataService> _geolocationDataServiceMock;

        [SetUp]
        public void SetUp()
        {
            _geolocationDataServiceMock = new Mock<IGeolocationDataService>(MockBehavior.Strict);
        }

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


        [Test]
        public async Task Post_GivenValidAndNonDuplicatedIpAddress_ShouldReturnCreatedAndGeolocationData()
        {
            //Arrange
            string ipAddress = "80.80.80.81";
            var remoteGeolocationData = new RemoteGeolocationData
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
            _geolocationDataServiceMock.Setup(x => x.GetByIpAddressAsync(It.IsAny<string>())).ReturnsAsync(remoteGeolocationData);
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton(_geolocationDataServiceMock.Object);
                });
            }).CreateClient();

            //Act
            var stringContent = new StringContent(JsonConvert.SerializeObject(new IpAddress{Value = ipAddress}), Encoding.UTF8, "application/json");
            var result = await client.PostAsync($"/api/geolocation", stringContent);

            //Assert
            var geolocationData = await result.Content.ReadAsAsync<GeolocationData>();
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            result.Content.Headers.ContentType.MediaType.Should().Be("application/json");
            geolocationData.Should().NotBeNull();
            geolocationData.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task Post_GivenValidAndDuplicatedIpAddress_ShouldReturnConflict()
        {
            //Arrange
            string ipAddress = "80.80.80.80";
            var remoteGeolocationData = new RemoteGeolocationData
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
            _geolocationDataServiceMock.Setup(x => x.GetByIpAddressAsync(It.IsAny<string>())).ReturnsAsync(remoteGeolocationData);
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton(_geolocationDataServiceMock.Object);
                });
            }).CreateClient();

            //Act
            var stringContent = new StringContent(JsonConvert.SerializeObject(new IpAddress { Value = ipAddress }), Encoding.UTF8, "application/json");
            var result = await client.PostAsync($"/api/geolocation", stringContent);

            //Assert
            await result.Content.ReadAsStringAsync();
            result.StatusCode.Should().Be(HttpStatusCode.Conflict);
            result.Content.Headers.ContentType.MediaType.Should().Be("text/plain");
        }

        [Test]
        public async Task Delete_GivenValidIpAddress_ShouldReturnOk()
        {
            //Arrange
            string ipAddress = "80.80.80.80";

            //Act
            var result = await _httpClient.DeleteAsync($"/api/geolocation/{ipAddress}");

            //Assert
            await result.Content.ReadAsStringAsync();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task Delete_GivenInvalidIpAddress_ShouldReturnBadRequest()
        {
            //Arrange
            string ipAddress = "256.0.0.0";

            //Act
            var result = await _httpClient.DeleteAsync($"/api/geolocation/{ipAddress}");

            //Assert
            await result.Content.ReadAsStringAsync();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}