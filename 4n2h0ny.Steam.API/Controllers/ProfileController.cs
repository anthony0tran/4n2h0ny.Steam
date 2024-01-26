using _4n2h0ny.Steam.API.Models;
using _4n2h0ny.Steam.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.Firefox;

namespace _4n2h0ny.Steam.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly FirefoxDriver _driver;

        public ProfileController(IProfileService profileRepository)
        {
            _profileService = profileRepository;
            _driver = WebDriverSingleton.Instance.Driver;
        }

        [HttpGet("commenters")]
        public ICollection<Profile> GetCommenters(string? profileUrl, CancellationToken cancellationToken)
        {
            _profileService.GetCommenters(profileUrl, cancellationToken);

            _driver.Dispose();

            return new List<Profile>();
        }

        [HttpGet("friends")]
        public async Task<ICollection<Profile>> GetFriendCommenters(CancellationToken cancellationToken)
        {
            _driver.Dispose();
            var result = await _profileService.GetFriendCommenters(cancellationToken);
            return result.ToArray();
        }
    }
}
