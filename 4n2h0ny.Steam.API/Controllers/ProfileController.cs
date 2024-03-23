using _4n2h0ny.Steam.API.Context.Entities;
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

        [HttpGet("friends")]
        public async Task<ICollection<Profile>> ListFriendCommenters(CancellationToken cancellationToken)
        {
            var result = await _profileService.ListFriendCommenters(cancellationToken);
            return result.ToArray();
        }

        [HttpGet("excluded")]
        public async Task<ICollection<Profile>> ListExcludedProfiles(CancellationToken cancellationToken) => 
            await _profileService.ListExcludedProfiles(cancellationToken);

        [HttpPut("[action]")]
        public async Task<Profile?> SetIsExcluded([FromQuery] IsExcludedRequest request) =>
            await _profileService.SetIsExcluded(request);
    }
}
