using EndPointFinder.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ScaNet.Controllers
{
    [ApiController]
    public class Scanet : ControllerBase
    {
        private readonly IMainMethods _mainMethods;
        private readonly IHelperMethods _helperMethods;

        public Scanet(IMainMethods mainMethods, IHelperMethods helperMethods)
        {
            _mainMethods = mainMethods;
            _helperMethods = helperMethods;
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

        [HttpPost("scanet/scanEndpoints/{url}")]
        public async Task<IActionResult> ScanForEndpoints(string url)
        {
            var validUrl = await _helperMethods.GetValidUrl(url);
            
            if (validUrl.Url is null)
            {
                return BadRequest(validUrl.Message);
            }

            var result = await _mainMethods.ScanWebSiteForEnpoints(validUrl.Url);
            return Ok(result);
        }
    }
}
