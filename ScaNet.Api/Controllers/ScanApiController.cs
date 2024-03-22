using AutoMapper;
using EndPointFinder.Models.ApiScanerModels;
using EndPointFinder.Repository.Helpers.HelperMethodsImplementation;
using EndPointFinder.Repository.Interfaces.IApiFinderInterface;
using Microsoft.AspNetCore.Mvc;
using ScaNet.Helpers;

namespace ScaNet.Controllers;

[ApiController]
public class ScanApiController : ControllerBase
{
    private readonly IApiFinderGet _apiFinderGet;
    private readonly IApiFinderPost _apiFinderPost;
    private readonly IHelperMethods _helperMethods;
    private readonly IMapper _mapper;

    public ScanApiController(IApiFinderGet apiFinderGet, IApiFinderPost apiFinderPost, IHelperMethods helperMethods, IMapper mapper)
    {
        _apiFinderGet = apiFinderGet;
        _apiFinderPost = apiFinderPost;
        _helperMethods = helperMethods;
        _mapper = mapper;
    }

    [HttpPost("scanet/scanApis/{url}")]
    public async Task<IActionResult> ScanForApi(string url, bool imf) // imf - Ignore Media Files
    {
        var validUrl = _helperMethods.GetValidUrl(url);

        if (validUrl.Url is null)
        {
            return BadRequest(validUrl.Message);

        }

        var result = await _apiFinderPost.ScanAndFind(validUrl.Url, imf);
        return result.ToActionResult<ApiScanerRootModels, ApiScanerModels>(_mapper);
    }

    [HttpGet("getApis")]
    public async Task<IActionResult> GetAllApis()
    {
        var result = await _apiFinderGet.GetAllApis();
        return result.ToActionResult<IEnumerable<ApiScanerRootModels>, IEnumerable<ApiScanerModels>>(_mapper);
    }

    [HttpGet("getApis/{id}")]
    public async Task<IActionResult> GetApisById(string id, bool imf) // imf - If True - Ignore Media Files
    {
        var result = await _apiFinderGet.GetApiCollectionById(id, imf);
        return result.ToActionResult<IEnumerable<ApiModels>, IEnumerable<ApiScanerModels.ApiResponseModels>>(_mapper);
    }

    [HttpGet("getKeys/{id}")]
    public async Task<IActionResult> GetKeysById(string id)
    {
        var result = await _apiFinderGet.GetKeyCollectionById(id);
        return result.ToActionResult<IEnumerable<KeyModels>, IEnumerable<ApiScanerModels.KeyResponseModels>>(_mapper);
    }
}
