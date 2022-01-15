using OpenQA.Selenium.Chrome;
using System;
using System.Windows;

namespace _4n2h0ny.Steam.GUI
{
    public sealed class WebDriverSingleton
    {
        public ChromeDriver? Driver { get; set; }

        public WebDriverSingleton()
        {
            try
            {
                var chromeDriverService = ChromeDriverService.CreateDefaultService(Globals.ChromeDriverPath);
                chromeDriverService.HideCommandPromptWindow = true;

                ChromeOptions options = new()
                {
                    DebuggerAddress = Globals.DebuggingAddress,
                    PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.Eager
                };

                Driver = new(chromeDriverService, options);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void DisposeDriver(OutputDialog outputDialog)
        {
            if(Driver != null)
            {
                Driver.Dispose();
                outputDialog.AppendLogTxtBox("Chrome driver disposed");
            }
        }
    }
}
