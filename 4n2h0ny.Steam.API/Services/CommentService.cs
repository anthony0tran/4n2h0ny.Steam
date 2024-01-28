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
        private readonly ISteamService _steamService;
        private readonly FirefoxDriver _driver;
        private readonly CommentConfiguration _configuration;

        public CommentService(IProfileService profileService, IOptions<CommentConfiguration> options, ISteamService steamService)
        {
            _profileService = profileService;
            _driver = WebDriverSingleton.Instance.Driver;
            _configuration = options.Value;
            _steamService = steamService;
        }

        public async Task CommentOnFriendCommenters(string comment, CancellationToken cancellationToken)
        {
            var profiles = await _profileService.GetFriendCommenters(cancellationToken);

            if (profiles.Count == 0)
            {
                throw new InvalidOperationException("No profiles found to comment on.");
            }

            var isLoggedIn = _steamService.CheckLogin(profiles.First().URI);

            if (!isLoggedIn)
            {
                throw new InvalidOperationException("Unable to comment. Not logged into steam");
            }

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
            var commentThreadEntry = _driver.FindElements(By.ClassName("commentthread_entry"));

            if (commentThreadEntry.Count == 0)
            {
                return;
            }

            var textArea = commentThreadEntry.First().FindElement(By.ClassName("commentthread_textarea"));
            var postButton = commentThreadEntry.First().FindElement(By.ClassName("btn_green_white_innerfade"));

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
