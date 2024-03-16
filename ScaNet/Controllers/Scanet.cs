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

        [HttpPost("scanet/sfa/{url}")]
        public async Task<IActionResult> ScanForApi(string url)
        {
            var validUrl = await _helperMethods.GetValidUrl(url);
            var result = await _mainMethods.ScanWebSiteForApis(validUrl);
            return Ok(result);
        }

        [HttpPost("scanet/sfe/{url}")]
        public async Task<IActionResult> ScanForEndpoints(string url)
        {
            var validUrl = await _helperMethods.GetValidUrl(url);
            var result = await _mainMethods.ScanWebSiteForEnpoints(validUrl);
            return Ok(result);
        }
    }
}
