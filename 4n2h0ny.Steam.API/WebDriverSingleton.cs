using OpenQA.Selenium.Firefox;

namespace _4n2h0ny.Steam.API
{
    public sealed class WebDriverSingleton
    {
        public FirefoxDriver Driver { get; set; }
        private static readonly FirefoxProfile _profile = new FirefoxProfileManager().GetProfile("4n2h0ny.Steam");
        private static readonly FirefoxOptions _options = new() { Profile = _profile };
        private WebDriverSingleton() 
        {
            Driver = new FirefoxDriver(_options);
        }

        private static readonly Lazy<WebDriverSingleton> initialization = new(() => new WebDriverSingleton());

        public static WebDriverSingleton Instance { get { return GetInitialization(); } }

        private static WebDriverSingleton GetInitialization()
        {
            if (initialization.IsValueCreated && initialization.Value.Driver.SessionId == null)
            {
                initialization.Value.Driver = new FirefoxDriver(_options);
            }

            return initialization.Value;
        }

        public void Quit()
        {
            Instance.Driver.Quit();
        }
    }
}
