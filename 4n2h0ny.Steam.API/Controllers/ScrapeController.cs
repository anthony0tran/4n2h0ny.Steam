using _4n2h0ny.Steam.API.Helpers;
using _4n2h0ny.Steam.API.Models;
using _4n2h0ny.Steam.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        [HttpPost("friends")]
        [ProducesResponseType(typeof(ScrapedProfilesResult), 200)]
        public async Task<IActionResult> ScrapeFriends(string? profileUrl, CancellationToken cancellationToken, bool syncFriends = true)
        {
            var watch = Stopwatch.StartNew();

            var result = await _profileService.ScrapeFriends(profileUrl, syncFriends, cancellationToken);

            watch.Stop();

            var response = new ScrapedProfilesResult()
            {
                ProfileCount = result.Count,
                ExecutionDuration = watch.Elapsed,
                Profiles = result
            };

            return Ok(response);
        }

        [HttpPost("commenters")]
        [ProducesResponseType(typeof(ScrapedProfilesResult), 200)]
        public async Task<IActionResult> ScrapeCommenters(string? profileUrl, CancellationToken cancellationToken, bool scrapeAll = false)
        {
            var watch = Stopwatch.StartNew();

            var result = await _profileService.ScrapeCommenters(profileUrl, scrapeAll, cancellationToken);

            watch.Stop();

            var response = new ScrapedProfilesResult()
            {
                ProfileCount = result.Count,
                ExecutionDuration = watch.Elapsed,
                Profiles = result
            };

            return Ok(response);
        }

        [HttpPost("data")]
        [ProducesResponseType(typeof(TimeSpan), 200)]
        public async Task<IActionResult> FetchProfileData(CancellationToken cancellationToken)
        {
            var watch = Stopwatch.StartNew();

            await _profileService.FetchProfileData(cancellationToken);

            watch.Stop();

            return Ok(watch.Elapsed);
        }

        [HttpPost("profile/CommentAreaDisabled/data")]
        [ProducesResponseType(typeof(ScrapedProfilesResult), 200)]
        public async Task<IActionResult> FetchCommentAreaDisabledProfileData(CancellationToken cancellationToken)
        {
            var watch = Stopwatch.StartNew();

            var result = await _profileService.FetchCommentAreaDisabledProfileData(cancellationToken);

            watch.Stop();

            var response = new ScrapedProfilesResult()
            {
                ProfileCount = result.Count,
                ExecutionDuration = watch.Elapsed,
                Profiles = result
            };

            return Ok(response);
        }

        [HttpPost("{URI}/data")]
        [ProducesResponseType(typeof(TimeSpan), 200)]
        public async Task<IActionResult> FetchProfileData(string URI, CancellationToken cancellationToken)
        {
            var watch = Stopwatch.StartNew();

            var decodedURI = URIDecoder.DecodePercentageEncodedURI(URI);
            await _profileService.FetchProfileData(decodedURI, cancellationToken);

            watch.Stop();

            return Ok(watch.Elapsed);
        }
    }
}
