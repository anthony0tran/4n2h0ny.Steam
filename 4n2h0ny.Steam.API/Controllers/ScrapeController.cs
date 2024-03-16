using _4n2h0ny.Steam.API.Helpers;
using _4n2h0ny.Steam.API.Models;
using _4n2h0ny.Steam.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.DevTools.V119.DOM;

namespace _4n2h0ny.Steam.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScrapeController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ScrapeController(IProfileService profileRepository)
        {
            _profileService = profileRepository;
        }

        [HttpGet("friends")]
        public async Task<ScrapedProfilesResult> ScrapeFriends(string? profileUrl, CancellationToken cancellationToken)
        {

        }

        [HttpGet("commenters")]
        public async Task<ScrapedProfilesResult> ScrapeCommenters(string? profileUrl, CancellationToken cancellationToken, bool scrapeAll = false)
        {
            var result = await _profileService.ScrapeCommenters(profileUrl, scrapeAll, cancellationToken);
            return new()
            {
                ProfileCount = result.Count,
                Profiles = result
            };
        }

        [HttpPost("data")]
        public async Task FetchProfileData(CancellationToken cancellationToken) =>
            await _profileService.FetchProfileData(cancellationToken);

        [HttpPost("{URI}/data")]
        public async Task FetchProfileData(string URI, CancellationToken cancellationToken)
        {
            var decodedURI = URIDecoder.DecodePercentageEncodedURI(URI);
            await _profileService.FetchProfileData(decodedURI, cancellationToken);
        }
    }
}
