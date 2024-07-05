using _4n2h0ny.Steam.API.Configurations;
using _4n2h0ny.Steam.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace _4n2h0ny.Steam.API.Services
{
    public class SteamService : ISteamService
    {
        private readonly SteamConfiguration _steamConfiguration;
        private readonly FirefoxDriver _driver;
        private readonly ILogger<SteamService> _logger;
        public SteamService(IOptions<SteamConfiguration> steamConfigurations, ILogger<SteamService> logger)
        {
            _steamConfiguration = steamConfigurations.Value;
            _driver = WebDriverSingleton.Instance.Driver;
            _logger = logger;
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

        public bool Login(string? profileUrl)
        {
            try
            {
                profileUrl ??= _steamConfiguration.LoginProfileUrl;
                _driver.Navigate().GoToUrl(profileUrl);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not login. Error: {errorMessage}", ex.Message);
                return false;
            }
        }
    }
}
