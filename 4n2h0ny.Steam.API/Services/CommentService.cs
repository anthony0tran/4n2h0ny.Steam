using _4n2h0ny.Steam.API.Configurations;
using _4n2h0ny.Steam.API.Helpers;
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
        private readonly CommentConfiguration _commentConfiguration;
        private readonly SteamConfiguration _steamConfiguration;

        public CommentService(
            IOptions<CommentConfiguration> options,
            IOptions<SteamConfiguration> steamConfiguration,
            ISteamService steamService,
            IProfileService profileService)
        {
            _profileService = profileService;
            _driver = WebDriverSingleton.Instance.Driver;
            _commentConfiguration = options.Value;
            _steamService = steamService;
            _steamConfiguration = steamConfiguration.Value;
        }

        public async Task<int> CommentOnFriendCommenters(string comment, CancellationToken cancellationToken)
        {
            var profiles = await _profileService.ListFriendCommenters(cancellationToken);

            if (profiles.Count == 0)
            {
                throw new InvalidOperationException("No profiles found to comment on.");
            }

            var isLoggedIn = _steamService.CheckLogin(profiles.First().URI);

            if (!isLoggedIn)
            {
                throw new InvalidOperationException("Unable to comment. Not logged into steam");
            }

            var profilesToCommentOn = profiles.Where(p => p.CommentedOn == null
            || p.CommentedOn <= DateTime.UtcNow.AddHours(-2));

            var profilesToCommentOnCount = profilesToCommentOn.Count();

            foreach (var profile in profilesToCommentOn)
            {
                await CommentOnProfile(profile.URI, comment, cancellationToken);
            }

            return profilesToCommentOnCount;
        }

        public async Task PreviewComment(string URI, string comment, CancellationToken cancellationToken)
        {
            var isLoggedIn = _steamService.CheckLogin(_steamConfiguration.DefaultProfileUrl);

            if (!isLoggedIn)
            {
                throw new InvalidOperationException("Unable to comment. Not logged into steam");
            }

            var profileToCommentOnURI = string.IsNullOrEmpty(URI) 
                ? _steamConfiguration.DefaultProfileUrl
                : URI;

            await CommentOnProfile(profileToCommentOnURI, comment, cancellationToken, true);
        }

        private async Task CommentOnProfile(string URI, string comment, CancellationToken cancellationToken, bool isPreview = false)
        {
            _driver.Navigate().GoToUrl(URI);

            var commentThreadEntry = _driver.FindElements(By.ClassName("commentthread_entry"));

            if (commentThreadEntry.Count == 0)
            {
                await _profileService.SetCommentAreaDisabled(URI, cancellationToken);
                return;
            }

            var textArea = commentThreadEntry.First().FindElement(By.ClassName("commentthread_textarea"));
            var postButton = commentThreadEntry.First().FindElement(By.ClassName("btn_green_white_innerfade"));

            var processedComment = await ProcessComment(URI, comment, cancellationToken);

            textArea.Click();

            textArea.SendKeys(processedComment);

            if (_commentConfiguration.EnableCommenting && !isPreview)
            {
                postButton.Click();
                await _profileService.SetCommentedOn(URI, cancellationToken);
            }
        }

        private async Task<string> ProcessComment(string URI, string comment, CancellationToken cancellationToken)
        {
            var hasValidTag = CommentHelper.CommentContainsValidTags(comment) 
                && comment.Contains("[name]");

            if (!hasValidTag)
            {
                return comment;
            }

            // get realName and personaName from URI
            var profile = await _profileService.GetProfile(URI, cancellationToken);

            // replace [name]

            return string.Empty;
        }

        public bool ValidateComment(string comment)
        {
            if (CommentHelper.CommentContainsNoTags(comment))
            {
                return true;
            }

            var tagsAreValid = CommentHelper.CommentContainsValidTags(comment);

            if (!tagsAreValid)
            {
                return false;
            }

            var commentContainKnownTags = comment.Contains("[name]");

            if (!commentContainKnownTags)
            {
                return false;
            }

            return true;
        }
    }
}
