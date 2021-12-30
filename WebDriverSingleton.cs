using OpenQA.Selenium.Chrome;

namespace _4n2h0ny.Steam
{
    public sealed class WebDriverSingleton
    {
        public ChromeDriver Driver { get; set; }

        public WebDriverSingleton()
        {
            ChromeOptions options = new()
            {
                DebuggerAddress = Globals.DebuggingAddress
            };

            Driver = new(Globals.ChromeDriverPath, options);
        }

        public void DisposeDriver()
        {
            Driver.Dispose();
        }
    }
}
