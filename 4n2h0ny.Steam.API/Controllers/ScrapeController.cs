using _4n2h0ny.Steam.API.Helpers;
using _4n2h0ny.Steam.API.Models;
using _4n2h0ny.Steam.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("commenters")]
        public async Task<ScrapeCommentersResult> GetCommenters(string? profileUrl, CancellationToken cancellationToken, bool scrapeAll = false)
        {
            var result = await _profileService.GetCommenters(profileUrl, scrapeAll, cancellationToken);
            return new()
            {
                CommentersCount = result.Count,
                Commenters = result
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
