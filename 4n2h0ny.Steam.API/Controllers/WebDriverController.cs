using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.Firefox;

namespace _4n2h0ny.Steam.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebDriverController : ControllerBase
    {
        private readonly FirefoxDriver _driver;

        public WebDriverController()
        {
            _driver = WebDriverSingleton.Instance.Driver;
        }

        [HttpPost("[action]")]
        public void Quit() =>
            _driver.Quit();

        [HttpPost("[action]")]
        public bool Instantiate(string? URI = null) 
        {
            var instance = WebDriverSingleton.Instance;

            if (!string.IsNullOrEmpty(URI)) 
            {
                instance.Driver.Navigate().GoToUrl(URI);
            }

            if (instance.Driver != null)
            {
                return true;
            }

            return false;
        }
            
    }
}
