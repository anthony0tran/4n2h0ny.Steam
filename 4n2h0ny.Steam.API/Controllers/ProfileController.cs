using _4n2h0ny.Steam.API.Models;
using _4n2h0ny.Steam.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace _4n2h0ny.Steam.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileRepository)
        {
            _profileService = profileRepository;
        }

        [HttpGet("commenters")]
        public async Task<ICollection<Profile>> GetCommenters(string? profileUrl, CancellationToken cancellationToken, bool scrapeAll = false)
        {
            var result = await _profileService.GetCommenters(profileUrl, scrapeAll, cancellationToken);
            return result.ToList();
        }

        [HttpGet("friends")]
        public async Task<ICollection<Profile>> GetFriendCommenters(CancellationToken cancellationToken)
        {
            var result = await _profileService.GetFriendCommenters(cancellationToken);
            return result.ToArray();
        }

        [HttpGet("excluded")]
        public async Task<ICollection<Profile>> GetExcludedProfiles(CancellationToken cancellationToken) => 
            await _profileService.GetExcludedProfiles(cancellationToken);

        [HttpPut("[action]")]
        public async Task<Profile?> SetIsExcluded([FromQuery] IsExcludedRequest request) =>
            await _profileService.SetIsExcluded(request);
    }
}
