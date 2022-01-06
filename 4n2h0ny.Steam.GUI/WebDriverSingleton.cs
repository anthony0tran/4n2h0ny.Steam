using OpenQA.Selenium.Chrome;

namespace _4n2h0ny.Steam.GUI
{
    public sealed class WebDriverSingleton
    {
        public ChromeDriver Driver { get; set; }
        
        public WebDriverSingleton()
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService(Globals.ChromeDriverPath);
            chromeDriverService.HideCommandPromptWindow = true;

            ChromeOptions options = new()
            {
                DebuggerAddress = Globals.DebuggingAddress
            };

            Driver = new(chromeDriverService, options);
        }

        public void DisposeDriver()
        {
            Driver.Dispose();
        }
    }
}
