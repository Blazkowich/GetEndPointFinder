using EndPointFinder.Repository.Helpers.HelperMethodsImplementation;
using EndPointFinder.Repository.Interfaces.IApiFinderInterface;
using Microsoft.AspNetCore.Mvc;

namespace ScaNet.Controllers;

[ApiController]
public class ScanApiController : ControllerBase
{
    private readonly IApiFinderGet _apiFinderGet;
    private readonly IApiFinderPost _apiFinderPost;
    private readonly IHelperMethods _helperMethods;

    public ScanApiController(IApiFinderGet apiFinderGet, IApiFinderPost apiFinderPost, IHelperMethods helperMethods)
    {
        _apiFinderGet = apiFinderGet;
        _apiFinderPost = apiFinderPost;
        _helperMethods = helperMethods;
    }

    [HttpPost("scanet/scanApis/{url}")]
    public async Task<IActionResult> ScanForApi(string url, bool imf) // imf - Ignore Media Files
    {
        var validUrl = await _helperMethods.GetValidUrl(url);

        if (validUrl.Url is null)
        {
            return BadRequest(validUrl.Message);

        }

        var result = await _apiFinderPost.ScanAndFind(validUrl.Url, imf);
        return Ok(result);
    }

    [HttpGet("getApis")]
    public async Task<IActionResult> GetAllApis()
    {
        var result = await _apiFinderGet.GetAllApis();
        return Ok(result);
    }

    [HttpGet("getApis/withoutMedia/{id}")]
    public async Task<IActionResult> GetApisWithoutMedia(string id)
    {
        var result = await _apiFinderGet.GetFilteredApisWithoutMedia(id);
        return Ok(result);
    }

    [HttpGet("getApis/{id}")]
    public async Task<IActionResult> GetApisById(string id)
    {
        var result = await _apiFinderGet.GetApiCollectionById(id);
        return Ok(result);
    }
}
