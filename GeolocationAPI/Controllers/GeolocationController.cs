using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GeolocationAPI.DTO;
using GeolocationAPI.Exceptions;
using GeolocationAPI.Persistence.Entities;
using GeolocationAPI.Services.Interfaces;
using GeolocationAPI.Validators.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace GeolocationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeolocationController : ControllerBase
    {

        private readonly IGeolocationDataService _geolocationDataService;
        private readonly IGeolocationService _geolocationService;
        private readonly IIpAddressValidator _ipAddressValidator;

        public GeolocationController(IGeolocationDataService geolocationDataService, IGeolocationService geolocationService, IIpAddressValidator ipAddressValidator)
        {
            _geolocationDataService = geolocationDataService;
            _geolocationService = geolocationService;
            _ipAddressValidator = ipAddressValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeolocationData>>> GetAll()
        {
            return Ok(await _geolocationService.GetAllAsync());
        }

        [HttpGet("{ipAddress}")]
        public async Task<ActionResult<GeolocationData>> Get(string ipAddress)
        {
            if (!_ipAddressValidator.IsValid(ipAddress))
            {
                return BadRequest($"Invalid IpAddress format: {ipAddress}.");
            }
            var geolocationData = await _geolocationService.GetAsync(ipAddress);
            if (geolocationData == null)
            {
                return NotFound($"Not found geolocation data for IpAddress {ipAddress}");
            }
            return Ok(geolocationData);
        }

        [HttpPost]
        public async Task<ActionResult<GeolocationData>> Add([FromBody] IpAddress ipAddress)
        {
            try
            {
                if (!_ipAddressValidator.IsValid(ipAddress.Value))
                {
                    return BadRequest($"Invalid IpAddress format: {ipAddress.Value}.");
                }
                var remoteGeolocationData = await _geolocationDataService.GetByIpAddressAsync(ipAddress.Value);
                if (remoteGeolocationData == null)
                {
                    return BadRequest($"Not found geolocation data for IpAddress {ipAddress.Value}");
                }
                var localGeolocationData = await _geolocationService.AddAsync(remoteGeolocationData);
                return CreatedAtAction(nameof(Get), new {localGeolocationData.IpAddress}, localGeolocationData);
            }
            catch (RemoteApiException ex)
            {
                return StatusCode((int) HttpStatusCode.ServiceUnavailable, ex.Message);
            }
            catch (EntityDuplicateException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{ipAddress}")]
        public async Task<ActionResult> Delete(string ipAddress)
        {
            if (!_ipAddressValidator.IsValid(ipAddress))
            {
                return BadRequest($"Invalid IpAddress format: {ipAddress}.");
            }
            await _geolocationService.DeleteAsync(ipAddress);
            return Ok();
        }
    }
}