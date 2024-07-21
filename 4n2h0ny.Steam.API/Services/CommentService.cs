using _4n2h0ny.Steam.API.Configurations;
using _4n2h0ny.Steam.API.Context.Entities;
using _4n2h0ny.Steam.API.Helpers;
using _4n2h0ny.Steam.API.Models;
using _4n2h0ny.Steam.API.Repositories.Interfaces;
using _4n2h0ny.Steam.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace _4n2h0ny.Steam.API.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IProfileService _profileService;
        private readonly ISteamService _steamService;
        private readonly FirefoxDriver _driver;
        private readonly CommentConfiguration _commentConfiguration;
        private readonly SteamConfiguration _steamConfiguration;

        public CommentService(
            IOptions<CommentConfiguration> options,
            IOptions<SteamConfiguration> steamConfiguration,
            ISteamService steamService,
            IProfileService profileService,
            ICommentRepository commentRepository)
        {
            _profileService = profileService;
            _driver = WebDriverSingleton.Instance.Driver;
            _commentConfiguration = options.Value;
            _steamService = steamService;
            _steamConfiguration = steamConfiguration.Value;
            _commentRepository = commentRepository;
        }

        public async Task<int> CommentOnFriendCommenters(string comment, CancellationToken cancellationToken)
        {
            var commentProcessStartedOn = DateTime.UtcNow;
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
            || p.CommentedOn <= DateTime.UtcNow.AddHours(-2)).ToArray();

            var profilesToCommentOnCount = profilesToCommentOn.Length;

            foreach (var profile in profilesToCommentOn)
            {
                await CommentOnProfile(profile.URI, comment, cancellationToken);
            }

            await _commentRepository.AddComment(comment, commentProcessStartedOn, profilesToCommentOn, cancellationToken);

            return profilesToCommentOnCount;
        }

        public async Task<int> CommentOnFriendsWithActiveCommentThread(string comment, CancellationToken cancellationToken)
        {
            var commentProcessStartedOn = DateTime.UtcNow;
            var profiles = await _profileService.GetFriendsWithActiveCommentThread(cancellationToken);

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
            || p.CommentedOn <= DateTime.UtcNow.AddHours(-2)).ToArray();

            var profilesToCommentOnCount = profilesToCommentOn.Length;

            foreach (var profile in profilesToCommentOn)
            {
                await CommentOnProfile(profile.URI, comment, cancellationToken);
            }

            await _commentRepository.AddComment(comment, commentProcessStartedOn, profilesToCommentOn, cancellationToken);

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
            var hasValidTag = CommentHelper.CommentContainsValidTags(comment).Any()
                && comment.Contains(Tags.NameTag);

            if (!hasValidTag)
            {
                return comment;
            }

            var profile = await _profileService.GetProfile(URI, cancellationToken);

            if (profile == null)
            {
                return _commentConfiguration.DefaultComment;
            }

            var name = DetermineName(profile.ProfileData);

            if (name == null)
            {
                return _commentConfiguration.DefaultComment;
            }

            return comment.Replace(Tags.NameTag, name);
        }

        public string? DetermineName(ProfileData profileData)
        {
            if (!string.IsNullOrWhiteSpace(profileData.PersonaName))
            {
                return profileData.PersonaName;
            }

            if (!string.IsNullOrWhiteSpace(profileData.RealName))
            {
                return profileData.RealName;
            }

            return null;
        }

        public bool ValidateComment(string comment)
        {
            if (CommentHelper.CommentContainsNoTags(comment))
            {
                return true;
            }

            var tags = CommentHelper.CommentContainsValidTags(comment).ToArray();

            if (tags.Length == 0)
            {
                return false;
            }

            var commentContainKnownTags = tags.Contains(Tags.NameTag);

            if (!commentContainKnownTags)
            {
                return false;
            }

            return true;
        }

        public async Task AddPredefinedComment(string commentString, CancellationToken cancellationToken)
        {
            await _commentRepository.AddPredefinedComment(commentString, cancellationToken);
        }

        public async Task<ICollection<PredefinedComment>> ListPredefinedComments(CancellationToken cancellationToken) =>
            await _commentRepository.ListPredefinedComments(cancellationToken);
    }
}
