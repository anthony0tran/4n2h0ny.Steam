using _4n2h0ny.Steam.API.Models;
using _4n2h0ny.Steam.API.Repositories.Profiles;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.Firefox;

namespace _4n2h0ny.Steam.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileRepository _profileRepository;
        private readonly FirefoxDriver _driver;

        public ProfileController(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
            _driver = WebDriverSingleton.Instance.Driver;
        }

        [HttpGet("commenters")]
        public IEnumerable<Profile> GetCommenters(string? profileUrl = null)
        {
            _profileRepository.GetCommenters(profileUrl);

            _driver.Dispose();

            return new List<Profile>();
        }
    }
}
