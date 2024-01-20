using _4n2h0ny.Steam.API.Models;
using Microsoft.Extensions.Options;
using OpenQA.Selenium.Firefox;

namespace _4n2h0ny.Steam.API.Repositories.Profiles
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly SteamConfiguration _steamConfiguration;

        public ProfileRepository(IOptions<SteamConfiguration> steamConfigurations)
        {
            _steamConfiguration = steamConfigurations.Value;
        }

        public ICollection<Profile> GetCommenters(string? profileUrl)
        {
            var driver = new FirefoxDriver();
            profileUrl ??= _steamConfiguration.DefaultProfileUrl;
            driver.Navigate().GoToUrl(profileUrl);

            driver.Dispose();
            return new List<Profile>();
        }
    }
}