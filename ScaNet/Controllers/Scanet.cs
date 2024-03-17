using EndPointFinder.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ScaNet.Controllers
{
    [ApiController]
    public class Scanet : ControllerBase
    {
        private readonly IMainMethods _mainMethods;
        private readonly IEndpointFinder _endpointFinder;
        private readonly IHelperMethods _helperMethods;

        public Scanet(IMainMethods mainMethods, IHelperMethods helperMethods, IEndpointFinder endpointFinder)
        {
            _mainMethods = mainMethods;
            _helperMethods = helperMethods;
            _endpointFinder = endpointFinder;
        }

        [HttpGet("getApis")]
        public async Task<IActionResult> GetAllApis()
        {
            var result = await _mainMethods.GetAllApis();
            return Ok(result);
        }

        [HttpGet("getEndpoints")]
        public async Task<IActionResult> GetAllEndpoints()
        {
            var result = await _mainMethods.GetAllEndpoints();
            return Ok(result);
        }

        [HttpPost("scanet/scanApis/{url}")]
        public async Task<IActionResult> ScanForApi(string url)
        {
            var validUrl = await _helperMethods.GetValidUrl(url);
            
            if (validUrl.Url is null)
            {
                return BadRequest(validUrl.Message);
            
            }

            var result = await _mainMethods.ScanWebSiteForApis(validUrl.Url);
            return Ok(result);
        }

        [HttpPost("scanet/scanAllEndpoints/{url}")]
        public async Task<IActionResult> ScanAllTypeEndpoints(string url)
        {
            var validUrl = await _helperMethods.GetValidUrl(url);
            
            if (validUrl.Url is null)
            {
                return BadRequest(validUrl.Message);
            }

            var result = await _mainMethods.ScanWebSiteForEnpoints(validUrl.Url);
            return Ok(result);
        }

        [HttpPost("scanet/scanCleanEndpoints/{url}")]
        public async Task<IActionResult> ScanOnlyCleanEndpoints(string url)
        {
            var validUrl = await _helperMethods.GetValidUrl(url);

            if (validUrl.Url is null)
            {
                return BadRequest(validUrl.Message);
            }

            var result = await _endpointFinder.GetEndpointsWithoutApi(validUrl.Url);
            return Ok(result);
        }

        [HttpPost("scanet/scanApiEndpoints/{url}")]
        public async Task<IActionResult> ScanOnlyApiEndpoints(string url)
        {
            var validUrl = await _helperMethods.GetValidUrl(url);

            if (validUrl.Url is null)
            {
                return BadRequest(validUrl.Message);
            }

            var result = await _endpointFinder.GetEndpointsWithApi(validUrl.Url);
            return Ok(result);
        }

        [HttpPost("scanet/scanSEndpoints/{url}")]
        public async Task<IActionResult> ScanOnlySEndpoints(string url)
        {
            var validUrl = await _helperMethods.GetValidUrl(url);

            if (validUrl.Url is null)
            {
                return BadRequest(validUrl.Message);
            }

            var result = await _endpointFinder.GetEndpointsWithS(validUrl.Url);
            return Ok(result);
        }

        [HttpPost("scanet/scanApiAndSEndpoints/{url}")]
        public async Task<IActionResult> ScanOnlyApiAndSEndpoints(string url)
        {
            var validUrl = await _helperMethods.GetValidUrl(url);

            if (validUrl.Url is null)
            {
                return BadRequest(validUrl.Message);
            }

            var result = await _endpointFinder.GetEndpointsWithApiAndS(validUrl.Url);
            return Ok(result);
        }
    }
}
