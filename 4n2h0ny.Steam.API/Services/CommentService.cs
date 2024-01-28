using _4n2h0ny.Steam.API.Configurations;
using _4n2h0ny.Steam.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace _4n2h0ny.Steam.API.Services
{
    public class CommentService : ICommentService
    {
        private readonly IProfileService _profileService;
        private readonly FirefoxDriver _driver;
        private readonly CommentConfiguration _configuration;

        public CommentService(IProfileService profileService, IOptions<CommentConfiguration> options)
        {
            _profileService = profileService;
            _driver = WebDriverSingleton.Instance.Driver;
            _configuration = options.Value;
        }

        public async Task CommentOnFriendCommenters(string comment, CancellationToken cancellationToken)
        {
            var profiles = await _profileService.GetFriendCommenters(cancellationToken);

            foreach (var profile in profiles)
            {
                CommentOnProfile(profile.URI, comment);
            }
        }

        private void CommentOnProfile(string URI, string comment)
        {
            // go to profile
            _driver.Navigate().GoToUrl(URI);

            // find commentbox
            var commentThreadEntry = _driver.FindElement(By.ClassName("commentthread_entry"))
                ?? throw new InvalidOperationException("Can't find commentThread entry");

            var textArea = commentThreadEntry.FindElement(By.ClassName("commentthread_textarea"));
            var postButton = commentThreadEntry.FindElement(By.ClassName("btn_green_white_innerfade"));

            // click commentbox
            textArea.Click();

            // comment
            textArea.SendKeys(comment);

            // click post button
            if (_configuration.EnableCommenting)
            {
                postButton.Click();
            }
        }
    }
}
