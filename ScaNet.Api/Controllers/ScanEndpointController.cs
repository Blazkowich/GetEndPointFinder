using AutoMapper;
using EndPointFinder.Models.EndpointScanerModels;
using EndPointFinder.Repository.Helpers.HelperMethodsImplementation;
using EndPointFinder.Repository.Interfaces.IEndpointFinderInterface;
using Microsoft.AspNetCore.Mvc;
using ScaNet.Helpers;
using System.Collections.Generic;

namespace ScaNet.Controllers
{
    [ApiController]
    public class ScanEndpointController : ControllerBase
    {
        private readonly IEndpointFinderGet _endpointFinderGet;
        private readonly IEndpointFinderPost _endpointFinderPost;
        private readonly IHelperMethods _helperMethods;
        private readonly IMapper _mapper;

        public ScanEndpointController(
            IHelperMethods helperMethods, 
            IEndpointFinderGet endpointFinderGet, 
            IEndpointFinderPost endpointFinderPost,
            IMapper mapper)
        {
            _helperMethods = helperMethods;
            _endpointFinderGet = endpointFinderGet;
            _endpointFinderPost = endpointFinderPost;
            _mapper = mapper;
        }

        [HttpGet("getEndpoints")]
        public async Task<IActionResult> GetAllEndpoints()
        {
            var result = await _endpointFinderGet.GetAllEndpoints();
            return result.ToActionResult<IEnumerable<EndpointScanerRootModels> , IEnumerable<EndpointScanerModels>>(_mapper);
        }

        [HttpPost("scanapi/scanEndpointsAll/{url}")]
        public async Task<IActionResult> ScanAllTypeEndpoints(string url)
        {
            var validUrl = _helperMethods.GetValidUrl(url);
            
            if (validUrl.Url is null)
            {
                return BadRequest(validUrl.Message);
            }

            var result = await _endpointFinderPost.MergedEndpointScanner(validUrl.Url);
            return result.ToActionResult<EndpointScanerRootModels, EndpointScanerModels>(_mapper);
        }

        [HttpPost("scanapi/scanEndpointsClean/{url}")]
        public async Task<IActionResult> ScanOnlyCleanEndpoints(string url)
        {
            var validUrl = _helperMethods.GetValidUrl(url);

            if (validUrl.Url is null)
            {
                return BadRequest(validUrl.Message);
            }

            var result = await _endpointFinderPost.ScanEndpointsWithoutApi(validUrl.Url);

            return result.ToActionResult<EndpointScanerRootModels, EndpointScanerModels>(_mapper);
        }

        [HttpPost("scanapi/scanEndpointsOnlyApi/{url}")]
        public async Task<IActionResult> ScanOnlyApiEndpoints(string url)
        {
            var validUrl = _helperMethods.GetValidUrl(url);

            if (validUrl.Url is null)
            {
                return BadRequest(validUrl.Message);
            }

            var result = await _endpointFinderPost.ScanEndpointsWithApi(validUrl.Url);
            return result.ToActionResult<EndpointScanerRootModels, EndpointScanerModels>(_mapper);
        }

        [HttpPost("scanapi/scanEndpointsOnlyS/{url}")]
        public async Task<IActionResult> ScanOnlySEndpoints(string url)
        {
            var validUrl = _helperMethods.GetValidUrl(url);

            if (validUrl.Url is null)
            {
                return BadRequest(validUrl.Message);
            }

            var result = await _endpointFinderPost.ScanEndpointsWithS(validUrl.Url);
            return result.ToActionResult<EndpointScanerRootModels, EndpointScanerModels>(_mapper);
        }

        [HttpPost("scanapi/scanEndpointsOnlyApiAndS/{url}")]
        public async Task<IActionResult> ScanOnlyApiAndSEndpoints(string url)
        {
            var validUrl = _helperMethods.GetValidUrl(url);

            if (validUrl.Url is null)
            {
                return BadRequest(validUrl.Message);
            }

            var result = await _endpointFinderPost.ScanEndpointsWithApiAndS(validUrl.Url);
            return result.ToActionResult<EndpointScanerRootModels, EndpointScanerModels>(_mapper);
        }
    }
}
