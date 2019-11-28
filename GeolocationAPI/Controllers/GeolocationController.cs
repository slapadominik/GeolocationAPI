using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GeolocationAPI.Converters.Interfaces;
using GeolocationAPI.DTO;
using GeolocationAPI.Exceptions;
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
        private readonly IGeolocationDataConverter _geolocationDataConverter;
        private readonly IIpAddressValidator _ipAddressValidator;

        public GeolocationController(
            IGeolocationDataService geolocationDataService,
            IGeolocationService geolocationService, 
            IIpAddressValidator ipAddressValidator, 
            IGeolocationDataConverter geolocationDataConverter)
        {
            _geolocationDataService = geolocationDataService;
            _geolocationService = geolocationService;
            _ipAddressValidator = ipAddressValidator;
            _geolocationDataConverter = geolocationDataConverter;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeolocationDataResource>>> GetAll()
        {
            return Ok((await _geolocationService.GetAllAsync()).Select(x => _geolocationDataConverter.Convert(x)));
        }

        [HttpGet("{ipAddress}")]
        public async Task<ActionResult<GeolocationDataResource>> Get(string ipAddress)
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
            return Ok(_geolocationDataConverter.Convert(geolocationData));
        }

        [HttpPost]
        public async Task<ActionResult<GeolocationDataResource>> Add([FromBody] IpAddress ipAddress)
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
                return CreatedAtAction(nameof(Get), new {localGeolocationData.IpAddress}, _geolocationDataConverter.Convert(localGeolocationData));
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