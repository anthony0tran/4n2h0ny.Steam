using _4n2h0ny.Steam.API.Models;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace _4n2h0ny.Steam.API.Repositories.Profiles
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly SteamConfiguration _steamConfiguration;

        public ProfileRepository(IOptions<SteamConfiguration> steamConfigurations)
        {
            _steamConfiguration = steamConfigurations.Value;
        }

        public ICollection<Profile> GetCommenters(string? profileUrl)
        {
            var driver = new FirefoxDriver();

            if (profileUrl == null)
            {
                profileUrl = $"{_steamConfiguration.DefaultProfileUrl}/{_steamConfiguration.CommentPageUrl}";
            }
            else
            {
                profileUrl = $"{profileUrl}/{_steamConfiguration.DefaultProfileUrl}";
            }

            driver.Navigate().GoToUrl(profileUrl);

            var maxCommentPageIndex = GetMaxCommentPageIndex(driver);

            if (maxCommentPageIndex != null)
            {
                GetUserProfilesFromCommentPage(driver);
                // Get users from first page

                // iterate commentPages and get users
                var commentPages = GetCommentPages(maxCommentPageIndex);
            }
            else
            {
                // Do something with single page
                GetUserProfilesFromCommentPage(driver);
            }

            driver.Dispose();
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

        private static string[] GetUserProfilesFromCommentPage(FirefoxDriver driver)
        {
            var profiles = new List<Profile>();
            var commentHeaders = driver.FindElements(By.XPath("//*[@class=\"commentthread_comment_author\"]"))
                .ToArray();

            foreach (var header in commentHeaders)
            {
                var href = header.FindElement(By.ClassName("commentthread_author_link")).GetAttribute("href");
                var commentDate = header.FindElement(By.ClassName("commentthread_comment_timestamp")).GetAttribute("data-timestamp");

                var newProfile = new Profile()
                {
                    ProfileUrl = href,
                    LastDateCommented = DateTime.Now
                };
            }

            return [];
        }

        private record CommentPageIndexString(string IndexString, string PageUrl);
        private record CommentPageIndex(int Index, string PageUrl);
    }
}