using _4n2h0ny.Steam.API.Helpers;
using _4n2h0ny.Steam.API.Models;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace _4n2h0ny.Steam.API.Repositories.Profiles
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly SteamConfiguration _steamConfiguration;
        private readonly FirefoxDriver _driver;

        public ProfileRepository(IOptions<SteamConfiguration> steamConfigurations)
        {
            _steamConfiguration = steamConfigurations.Value;
            _driver = WebDriverSingleton.Instance.Driver;
        }

        public ICollection<Profile> GetCommenters(string? profileUrl)
        {
            if (profileUrl == null)
            {
                profileUrl = $"{_steamConfiguration.DefaultProfileUrl}/{_steamConfiguration.CommentPageUrl}";
            }
            else
            {
                profileUrl = $"{profileUrl}/{_steamConfiguration.DefaultProfileUrl}";
            }

            _driver.Navigate().GoToUrl(profileUrl);

            var maxCommentPageIndex = GetMaxCommentPageIndex(_driver);

            if (maxCommentPageIndex != null)
            {
                var profiles = GetUserProfilesFromCommentPage(_driver);
                // Get users from first page

                // iterate commentPages and get users
                var commentPages = GetCommentPages(maxCommentPageIndex);
            }
            else
            {
                // Do something with single page
                GetUserProfilesFromCommentPage(_driver);
            }

            return new List<Profile>();
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

            if (int.TryParse(commentPageIndexList.Last().IndexString, out var highestIndex))
            {
                return new(highestIndex, commentPageIndexList.Last().PageUrl);
            }

            return null;
        }

        private static List<Profile> GetUserProfilesFromCommentPage(FirefoxDriver driver)
        {
            var profiles = new List<Profile>();
            var commentHeaders = driver.FindElements(By.XPath("//*[@class=\"commentthread_comment_author\"]"))
                .ToArray();

            foreach (var header in commentHeaders)
            {
                var href = header.FindElement(By.ClassName("commentthread_author_link")).GetAttribute("href");

                if (profiles.Any(p => p.ProfileUrl == href))
                {
                    continue;
                }

                var unixTimeStamp = header.FindElement(By.ClassName("commentthread_comment_timestamp")).GetAttribute("data-timestamp");

                var newProfile = new Profile()
                {
                    ProfileUrl = href,
                    LastDateCommented = DateParser.ParseUnixTimeStampToDateTime(unixTimeStamp),
                };

                profiles.Add(newProfile);
            }

            return profiles;
        }

        private record CommentPageIndexString(string IndexString, string PageUrl);
        private record CommentPageIndex(int Index, string PageUrl);
    }
}