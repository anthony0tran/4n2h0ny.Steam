using _4n2h0ny.Steam.API.Configurations;
using _4n2h0ny.Steam.API.Helpers;
using _4n2h0ny.Steam.API.Models;
using _4n2h0ny.Steam.API.Repositories.Profiles;
using _4n2h0ny.Steam.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace _4n2h0ny.Steam.API.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly SteamConfiguration _steamConfiguration;
        private readonly ISteamService _steamService;
        private readonly FirefoxDriver _driver;

        public ProfileService(IOptions<SteamConfiguration> steamConfigurations, IProfileRepository profileRepository, ISteamService steamService)
        {
            _profileRepository = profileRepository;
            _steamService = steamService;
            _steamConfiguration = steamConfigurations.Value;
            _driver = WebDriverSingleton.Instance.Driver;
        }

        public async Task<ICollection<Profile>> GetCommenters(string? profileUrl, CancellationToken cancellationToken)
        {
            var isLoggedIn = _steamService.CheckLogin(profileUrl);

            if (!isLoggedIn)
            {
                throw new InvalidOperationException("User is not logged in...");
            }

            var profiles = GetCommenters(profileUrl);
            return await _profileRepository.AddOrUpdateProfile(profiles, cancellationToken);
        }

        private ICollection<Profile> GetCommenters(string? profileUrl)
        {
            var profiles = new HashSet<Profile>();
            if (profileUrl == null)
            {
                profileUrl = $"{_steamConfiguration.DefaultProfileUrl}/{_steamConfiguration.CommentPageUrl}";
            }
            else
            {
                profileUrl = $"{profileUrl}/{_steamConfiguration.CommentPageUrl}";
            }

            _driver.Navigate().GoToUrl(profileUrl);

            var maxCommentPageIndex = GetMaxCommentPageIndex(_driver);

            if (maxCommentPageIndex != null)
            {
                var initialFoundProfiles = GetUserProfilesFromCommentPage(_driver);

                foreach (var foundProfile in initialFoundProfiles)
                {
                    profiles.Add(foundProfile);
                }

                var commentPages = GetCommentPages(maxCommentPageIndex);

                if (commentPages == null)
                {
                    return profiles;
                }

                foreach (var commentPage in commentPages)
                {
                    _driver.Navigate().GoToUrl(commentPage);
                    var foundProfilesNextPages = GetUserProfilesFromCommentPage(_driver);
                    foreach (var foundProfile in foundProfilesNextPages)
                    {
                        profiles.Add(foundProfile);
                    }
                }
            }
            else
            {
                var foundProfiles = GetUserProfilesFromCommentPage(_driver);
                foreach (var foundProfile in foundProfiles)
                {
                    profiles.Add(foundProfile);
                }
            }

            return profiles;
        }

        private static string[]? GetCommentPages(CommentPageIndex? maxCommentPageIndex)
        {
            if (maxCommentPageIndex == null)
            {
                return null;
            }

            var pageCountIndex = maxCommentPageIndex.PageUrl.IndexOf("ctp=");
            var baseCommentPageUrl = maxCommentPageIndex.PageUrl[..pageCountIndex];

            var resultList = new string[maxCommentPageIndex.Index];

            return Enumerable.Range(2, maxCommentPageIndex.Index)
                .Select(i => $"{baseCommentPageUrl}ctp={i}")
                .ToArray();
        }

        private static CommentPageIndex? GetMaxCommentPageIndex(FirefoxDriver driver)
        {
            var commentPageIndexList = driver.FindElements(By.XPath("//*[@class=\"commentthread_pagelinks\"]/a"))
                            .Select(e => new CommentPageIndexString(e.GetAttribute("innerHTML"), e.GetAttribute("href")))
                            .Distinct()
                            .ToArray();

            if (commentPageIndexList.Length == 0)
            {
                return null;
            }

            if (int.TryParse(commentPageIndexList.Last().IndexString, out var highestIndex))
            {
                return new(highestIndex, commentPageIndexList.Last().PageUrl);
            }

            return null;
        }

        private static List<Profile> GetUserProfilesFromCommentPage(FirefoxDriver driver)
        {
            var profiles = new List<Profile>();
            var comments = driver.FindElements(By.ClassName("commentthread_comment"))
                .ToArray();

            foreach (var comment in comments)
            {
                var friend = comment.FindElements(By.ClassName("commentthread_comment_friendindicator")).ToArray();
                var href = comment.FindElement(By.ClassName("commentthread_author_link")).GetAttribute("href");

                if (profiles.Any(p => p.URI == href))
                {
                    continue;
                }

                var unixTimeStamp = comment.FindElement(By.ClassName("commentthread_comment_timestamp")).GetAttribute("data-timestamp");

                var newProfile = new Profile()
                {
                    URI = href,
                    LastDateCommented = DateParser.ParseUnixTimeStampToDateTime(unixTimeStamp),
                    IsFriend = friend.Length > 0
                };

                profiles.Add(newProfile);
            }

            return profiles;
        }

        public async Task<ICollection<Profile>> GetFriendCommenters(CancellationToken cancellationToken) =>
            await _profileRepository.GetFriendCommenters(cancellationToken);

        private record CommentPageIndexString(string IndexString, string PageUrl);
        private record CommentPageIndex(int Index, string PageUrl);
    }
}
