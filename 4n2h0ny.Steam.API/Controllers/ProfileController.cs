using _4n2h0ny.Steam.API.Models;
using _4n2h0ny.Steam.API.Repositories.Profiles;
using Microsoft.AspNetCore.Mvc;

namespace _4n2h0ny.Steam.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileController(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        [HttpGet("commenters")]
        public IEnumerable<Profile> GetCommenters(string? profileUrl = null)
        {
            _profileRepository.GetCommenters(profileUrl);
            return new List<Profile>();
        }
    }
}
