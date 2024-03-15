using _4n2h0ny.Steam.API.Entities;
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
        public async Task<ICollection<Profile>> GetCommenters(string? profileUrl, CancellationToken cancellationToken, bool scrapeAll = false)
        {
            var result = await _profileService.GetCommenters(profileUrl, scrapeAll, cancellationToken);
            return result.ToList();
        }

        [HttpPost("profile/data")]
        public async Task FetchProfileData(CancellationToken cancellationToken) =>
            await _profileService.FetchProfileData(cancellationToken);
    }
}
