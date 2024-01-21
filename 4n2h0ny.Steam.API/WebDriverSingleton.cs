using OpenQA.Selenium.Firefox;

namespace _4n2h0ny.Steam.API
{
    public sealed class WebDriverSingleton
    {
        public FirefoxDriver Driver { get; set; }
        private WebDriverSingleton() 
        {
            Driver = new FirefoxDriver();
        }

        private static readonly Lazy<WebDriverSingleton> initialization = new(() => new WebDriverSingleton());

        public static WebDriverSingleton Instance { get {  return initialization.Value; } }
    }
}
