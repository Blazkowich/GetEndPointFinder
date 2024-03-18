using EndPointFinder.Repository.Helpers.HelperMethodsImplementation;
using EndPointFinder.Repository.Interfaces.IEndpointFinderInterface;
using Microsoft.AspNetCore.Mvc;

namespace ScaNet.Controllers
{
    [ApiController]
    public class ScanEndpointController : ControllerBase
    {
        private readonly IEndpointFinderGet _endpointFinderGet;
        private readonly IEndpointFinderPost _endpointFinderPost;
        private readonly IHelperMethods _helperMethods;

        public ScanEndpointController(IHelperMethods helperMethods, IEndpointFinderGet endpointFinderGet, IEndpointFinderPost endpointFinderPost)
        {
            _helperMethods = helperMethods;
            _endpointFinderGet = endpointFinderGet;
            _endpointFinderPost = endpointFinderPost;
        }

        [HttpGet("getEndpoints")]
        public async Task<IActionResult> GetAllEndpoints()
        {
            var result = await _endpointFinderGet.GetAllEndpoints();
            return Ok(result);
        }

        [HttpPost("scanet/scanEndpointsAll/{url}")]
        public async Task<IActionResult> ScanAllTypeEndpoints(string url)
        {
            var validUrl = await _helperMethods.GetValidUrl(url);
            
            if (validUrl.Url is null)
            {
                return BadRequest(validUrl.Message);
            }

            var result = await _endpointFinderPost.MergedEndpointScanner(validUrl.Url);
            return Ok(result);
        }

        [HttpPost("scanet/scanEndpointsClean/{url}")]
        public async Task<IActionResult> ScanOnlyCleanEndpoints(string url)
        {
            var validUrl = await _helperMethods.GetValidUrl(url);

            if (validUrl.Url is null)
            {
                return BadRequest(validUrl.Message);
            }

            var result = await _endpointFinderPost.ScanEndpointsWithoutApi(validUrl.Url);
            return Ok(result);
        }

        [HttpPost("scanet/scanEndpointsOnlyApi/{url}")]
        public async Task<IActionResult> ScanOnlyApiEndpoints(string url)
        {
            var validUrl = await _helperMethods.GetValidUrl(url);

            if (validUrl.Url is null)
            {
                return BadRequest(validUrl.Message);
            }

            var result = await _endpointFinderPost.ScanEndpointsWithApi(validUrl.Url);
            return Ok(result);
        }

        [HttpPost("scanet/scanEndpointsOnlyS/{url}")]
        public async Task<IActionResult> ScanOnlySEndpoints(string url)
        {
            var validUrl = await _helperMethods.GetValidUrl(url);

            if (validUrl.Url is null)
            {
                return BadRequest(validUrl.Message);
            }

            var result = await _endpointFinderPost.ScanEndpointsWithS(validUrl.Url);
            return Ok(result);
        }

        [HttpPost("scanet/scanEndpointsOnlyApiAndS/{url}")]
        public async Task<IActionResult> ScanOnlyApiAndSEndpoints(string url)
        {
            var validUrl = await _helperMethods.GetValidUrl(url);

            if (validUrl.Url is null)
            {
                return BadRequest(validUrl.Message);
            }

            var result = await _endpointFinderPost.ScanEndpointsWithApiAndS(validUrl.Url);
            return Ok(result);
        }
    }
}
