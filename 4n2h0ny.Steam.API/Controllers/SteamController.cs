using _4n2h0ny.Steam.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.Firefox;

namespace _4n2h0ny.Steam.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SteamController : ControllerBase
    {
        private readonly ISteamService _steamService;
        private readonly FirefoxDriver _driver;

        public SteamController(ISteamService steamService)
        {
            _steamService = steamService;
            _driver = WebDriverSingleton.Instance.Driver;
        }

        [HttpGet("[action]")]
        public bool CheckLogin(string? profileUrl)
        {
            var result = _steamService.CheckLogin(profileUrl);

            _driver.Dispose();

            return result;
        }
    }
}
