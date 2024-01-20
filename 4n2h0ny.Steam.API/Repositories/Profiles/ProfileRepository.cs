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

            if (maxCommentPageIndex.HasValue)
            {
                IterateCommentPages(driver, maxCommentPageIndex);
            }
            else
            {
                // Do something with single page
            }

            driver.Dispose();
            return new List<Profile>();
        }

        private static void IterateCommentPages(FirefoxDriver driver, (int index, string pageUrl)? maxCommentPageIndex)
        {
            var pageCountIndex = maxCommentPageIndex.Value.pageUrl.IndexOf("ctp=");
            var baseCommentPageUrl = maxCommentPageIndex.Value.pageUrl[..pageCountIndex];

            foreach (var i in Enumerable.Range(1, maxCommentPageIndex.Value.index))
            {
                // Go to page
                var commentPageUrl = $"{baseCommentPageUrl}ctp={i}";
                Thread.Sleep(2000);
                driver.Navigate().GoToUrl(commentPageUrl);
            };
        }

        private static (int index, string pageUrl)? GetMaxCommentPageIndex(FirefoxDriver driver)
        {
            var commentPageIndexList = driver.FindElements(By.XPath("//*[@class=\"commentthread_pagelinks\"]/a"))
                            .Select(e => new CommentPageIndex(e.GetAttribute("innerHTML"), e.GetAttribute("href")))
                            .Distinct()
                            .ToArray();

            if (int.TryParse(commentPageIndexList.Last().IndexString, out var highestIndex))
            {
                return (highestIndex, commentPageIndexList.Last().PageUrl);
            }

            return null;
        }

        private record CommentPageIndex(string IndexString, string PageUrl);
    }
}