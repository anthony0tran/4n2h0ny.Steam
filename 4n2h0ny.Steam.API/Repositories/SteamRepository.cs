using _4n2h0ny.Steam.API.Configurations;
using _4n2h0ny.Steam.API.Repositories.Profiles;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace _4n2h0ny.Steam.API.Repositories
{
    public class SteamRepository : ISteamRepository
    {
        private readonly SteamConfiguration _steamConfiguration;
        private readonly FirefoxDriver _driver;

        public SteamRepository(IOptions<SteamConfiguration> steamConfigurations)
        {
            _steamConfiguration = steamConfigurations.Value;
            _driver = WebDriverSingleton.Instance.Driver;
        }

        public bool CheckLogin(string? profileUrl)
        {
            profileUrl ??= _steamConfiguration.DefaultProfileUrl;
            _driver.Navigate().GoToUrl(profileUrl);

            var avatarElement = _driver.FindElements(By.ClassName("user_avatar"));
            if (avatarElement.Count == 0)
            {
                return false;
            }

            return true;
        }
    }
}