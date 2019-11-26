using System.Collections.Generic;
using System.Threading.Tasks;
using GeolocationAPI.DTO;
using GeolocationAPI.Exceptions;
using GeolocationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace GeolocationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeolocationController : ControllerBase
    {

        private readonly IGeolocationDataService _geolocationDataService;
        private readonly IGeolocationService _geolocationService;
        public GeolocationController(IGeolocationDataService geolocationDataService, IGeolocationService geolocationService)
        {
            _geolocationDataService = geolocationDataService;
            _geolocationService = geolocationService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<ActionResult<string>> AddGeolocationDataByIpAddress([FromBody] IpAddress ipAddress)
        {
            try
            {
                var geolocationData = await _geolocationDataService.GetByIpAddressAsync(ipAddress.Value);
                var id = await _geolocationService.AddGeolocationDataAsync(geolocationData);
                return CreatedAtAction(nameof(Get),new {id}, geolocationData);
            }
            catch (RemoteApiException ex)
            {
                return StatusCode(503);
            }
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}