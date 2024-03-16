using _4n2h0ny.Steam.API.Configurations;
using _4n2h0ny.Steam.API.Helpers;
using _4n2h0ny.Steam.API.Entities;
using _4n2h0ny.Steam.API.Repositories.Profiles;
using _4n2h0ny.Steam.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Collections.ObjectModel;
using _4n2h0ny.Steam.API.Models;

namespace _4n2h0ny.Steam.API.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly ISteamService _steamService;
        private readonly SteamConfiguration _steamConfiguration;
        private readonly FirefoxDriver _driver;
        private readonly ILogger<ProfileService> _logger;

        public ProfileService(
            IOptions<SteamConfiguration> steamConfigurations,
            IProfileRepository profileRepository,
            ISteamService steamService,
            ILogger<ProfileService> logger)
        {
            _profileRepository = profileRepository;
            _steamService = steamService;
            _steamConfiguration = steamConfigurations.Value;
            _driver = WebDriverSingleton.Instance.Driver;
            _logger = logger;
        }

        public async Task<ICollection<Profile>> GetCommenters(string? profileUrl, bool scrapeAll, CancellationToken cancellationToken)
        {
            var isLoggedIn = _steamService.CheckLogin(profileUrl);

            if (!isLoggedIn)
            {
                throw new InvalidOperationException("User is not logged in...");
            }

            var lastFoundCommentDate = !scrapeAll
                ? await _profileRepository.GetDateLatestComment(cancellationToken)
                : null;

            var profiles = GetCommenters(profileUrl, lastFoundCommentDate);
            return await _profileRepository.AddOrUpdateProfile(profiles, cancellationToken);
        }

        private HashSet<Profile> GetCommenters(string? profileUrl, DateTime? lastFoundCommentDate)
        {
            var profiles = new HashSet<Profile>();
            var reachedPreviousFoundComment = false;
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
                string[]? commentPages = GetProfilesOnIntitialPage(lastFoundCommentDate, profiles, maxCommentPageIndex, ref reachedPreviousFoundComment);

                if (commentPages == null || reachedPreviousFoundComment)
                {
                    return profiles;
                }

                foreach (var commentPage in commentPages)
                {
                    if (reachedPreviousFoundComment)
                    {
                        return profiles;
                    };

                    _driver.Navigate().GoToUrl(commentPage);
                    GetProfilesOnCurrentPage(lastFoundCommentDate, profiles, ref reachedPreviousFoundComment);
                }
            }
            else
            {
                GetProfilesOnCurrentPage(lastFoundCommentDate, profiles, ref reachedPreviousFoundComment);
            }

            return profiles;
        }

        private void GetProfilesOnCurrentPage(DateTime? lastFoundCommentDate, HashSet<Profile> profiles, ref bool reachedPreviousFoundComment)
        {
            var foundProfiles = GetUserProfilesFromCommentPage(_driver, lastFoundCommentDate, ref reachedPreviousFoundComment);
            foreach (var foundProfile in foundProfiles)
            {
                profiles.Add(foundProfile);
            }
        }

        private string[]? GetProfilesOnIntitialPage(DateTime? lastFoundCommentDate, HashSet<Profile> profiles, CommentPageIndex? maxCommentPageIndex, ref bool reachedPreviousFoundComment)
        {
            var initialFoundProfiles = GetUserProfilesFromCommentPage(_driver, lastFoundCommentDate, ref reachedPreviousFoundComment);

            foreach (var foundProfile in initialFoundProfiles)
            {
                profiles.Add(foundProfile);
            }

            var commentPages = GetCommentPages(maxCommentPageIndex);
            return commentPages;
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

            return Enumerable.Range(2, maxCommentPageIndex.Index - 1)
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

        private static List<Profile> GetUserProfilesFromCommentPage(FirefoxDriver driver, DateTime? lastFoundCommentDate, ref bool reachedPreviousFoundComment)
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

                var unixTimeStamp = comment
                    .FindElement(By.ClassName("commentthread_comment_timestamp"))
                    .GetAttribute("data-timestamp");

                var commentDate = DateParser.ParseUnixTimeStampToDateTime(unixTimeStamp);

                if (lastFoundCommentDate != null && commentDate <= lastFoundCommentDate)
                {
                    reachedPreviousFoundComment = true;
                    return profiles;
                }

                var newProfile = new Profile()
                {
                    URI = href,
                    LatestCommentReceivedOn = commentDate,
                    IsFriend = friend.Length > 0,
                    FetchedOn = DateTime.UtcNow
                };

                profiles.Add(newProfile);
            }

            return profiles;
        }

        public async Task FetchProfileData(CancellationToken cancellationToken)
        {
            var profiles = await _profileRepository.GetAllProfilesIgnoreQueryFilters(cancellationToken);

            foreach (var profile in profiles)
            {
                var data = ScrapeData(profile, cancellationToken);
                profile.ProfileData = data;
            }

            await _profileRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task FetchProfileData(string URI, CancellationToken cancellationToken)
        {
            var profile = await _profileRepository.GetProfileByURI(URI, cancellationToken);

            if (profile == null)
            {
                return;
            }

            var data = ScrapeData(profile, cancellationToken);
            profile.ProfileData = data;

            await _profileRepository.SaveChangesAsync(cancellationToken);
        }

        private ProfileData ScrapeData(Profile profile, CancellationToken cancellationToken)
        {
            if (profile.ProfileDataId == Guid.Empty)
            {
                profile.ProfileData = new();
            }

            if (profile.ProfileData == null)
            {
                throw new InvalidOperationException($"Data not loaded for profile. Id: {profile.Id}");
            }

            _driver.Navigate().GoToUrl(profile.URI);

            if (!CheckProfileCanBeFound())
            {
                _profileRepository.SetProfileNotFound(profile.URI, true, cancellationToken);
                return profile.ProfileData;
            }

            if (CheckProfileIsPrivate())
            {
                _profileRepository.SetProfileIsPrivate(profile.URI, true, cancellationToken);
                return profile.ProfileData;
            }

            ScrapeSteamIdAndPersonaName(profile);
            ScrapeLevel(profile);
            ScrapeRealNameAndCountry(profile);
            ScrapeFriendCount(profile);
            ScrapeAwardBadgeAndGameCount(profile);
            ScrapeTotalCommendCount(profile);

            profile.ProfileData.LastFetchedOn = DateTime.UtcNow;

            return profile.ProfileData;
        }

        private bool CheckProfileIsPrivate()
        {
            var profilePrivateInfoElement = _driver.FindElements(By.CssSelector("div[class='profile_private_info']"));
            if (profilePrivateInfoElement.Count == 0)
            {
                return false;
            }

            var profilePrivateInfo = profilePrivateInfoElement.First().GetAttribute("innerHTML");

            if (profilePrivateInfo.Trim() == "This profile is private.")
            {
                return true;
            }

            return false;
        }

        private bool CheckProfileCanBeFound()
        {
            var messageElement = _driver.FindElements(By.CssSelector("div[id='message']"));
            if (messageElement.Count == 0)
            {
                return true;
            }

            var h3Element = messageElement.First().FindElements(By.CssSelector("h3"));

            if (h3Element.Count == 0)
            {
                throw new InvalidOperationException("No H3 element found in the message element");
            }

            var message = h3Element.First().GetAttribute("innerHTML");

            if (message != "The specified profile could not be found.")
            {
                return true;
            }

            return false;
        }

        private void ScrapeTotalCommendCount(Profile profile)
        {
            var allCommentsContainer = _driver.FindElements(By.ClassName("commentthread_allcommentslink"));

            if (allCommentsContainer.Count == 0)
            {
                return;
            }

            var allCommentsCountElements = allCommentsContainer.First().FindElements(By.CssSelector("span"));
            if (allCommentsCountElements.Count == 0)
            {
                return;
            }

            var allCommentsCountString = allCommentsCountElements.First().GetAttribute("innerHTML");
            allCommentsCountString = ProfileDataParser.StripCommaFromNumber(allCommentsCountString);
            if (int.TryParse(allCommentsCountString, out var allCommentsCount))
            {
                profile.ProfileData.TotalCommendsCount = allCommentsCount;
            }
        }

        private void ScrapeAwardBadgeAndGameCount(Profile profile)
        {
            var linkContainer = _driver.FindElements(By.ClassName("responsive_count_link_area"));
            if (linkContainer.Count == 0)
            {
                _logger.LogWarning("Could not find linkContainer for profile with Id: {profileId}", profile.Id);
                return;
            }

            var countContainers = _driver.FindElements(By.CssSelector("div[class='profile_count_link ellipsis']"));
            ScrapeAwardCount(profile, countContainers);
            ScrapeBadgeCount(profile, countContainers);
            ScrapeGameCount(profile, countContainers);
        }

        private void ScrapeGameCount(Profile profile, ReadOnlyCollection<IWebElement> countContainers)
        {
            var gameCountContainer = countContainers.Where(IsGameElement);

            if (gameCountContainer == null)
            {
                return;
            }

            SetValue(profile, gameCountContainer, CountType.Games);
        }

        private void ScrapeBadgeCount(Profile profile, ReadOnlyCollection<IWebElement> countContainers)
        {
            var badgeCountContainer = countContainers.Where(IsBadgeElement);

            if (badgeCountContainer == null)
            {
                return;
            }

            SetValue(profile, badgeCountContainer, CountType.Badge);
        }

        private bool IsGameElement(IWebElement element)
        {
            var linkLabel = element.FindElements(By.ClassName("count_link_label")).FirstOrDefault();

            if (linkLabel == null)
            {
                return false;
            };

            return linkLabel.GetAttribute("innerHTML") == "Games";
        }

        private bool IsBadgeElement(IWebElement element)
        {
            var linkLabel = element.FindElements(By.ClassName("count_link_label")).FirstOrDefault();

            if (linkLabel == null)
            {
                return false;
            };

            return linkLabel.GetAttribute("innerHTML") == "Badges";
        }

        private void ScrapeAwardCount(Profile profile, ReadOnlyCollection<IWebElement> countContainers)
        {
            var awardContainer = countContainers.Where(IsAwardElement);

            if (awardContainer == null)
            {
                return;
            }

            SetValue(profile, awardContainer, CountType.Award);
        }

        private static void SetValue(Profile profile, IEnumerable<IWebElement> container, CountType type)
        {
            if (!container.Any())
            {
                return;
            }

            var countElement = container.First().FindElements(By.ClassName("profile_count_link_total"));

            if (countElement.Count == 0)
            {
                return;
            }

            var countString = countElement.First().GetAttribute("innerHTML");
            countString = ProfileDataParser.StripEmptyCharacters(countString);
            if (int.TryParse(countString, out var count))
            {
                _ = type switch
                {
                    CountType.Award => profile.ProfileData.AwardCount = count,
                    CountType.Badge => profile.ProfileData.BadgeCount = count,
                    CountType.Games => profile.ProfileData.GameCount = count,
                    _ => null
                };
            }
        }

        private bool IsAwardElement(IWebElement element)
        {
            var linkLabel = element.FindElements(By.ClassName("count_link_label")).FirstOrDefault();

            if (linkLabel == null)
            {
                return false;
            };

            return linkLabel.GetAttribute("innerHTML") == "Profile Awards";
        }

        private void ScrapeFriendCount(Profile profile)
        {
            var FriendElementContainer = _driver.FindElements(By.CssSelector("div[class='profile_friend_links profile_count_link_preview_ctn responsive_groupfriends_element']"));

            if (FriendElementContainer.Count == 0)
            {
                _logger.LogWarning("Could not find FriendElementContainer for profile with Id: {profileId}", profile.Id);
                return;
            }

            var friendCountElement = FriendElementContainer.First().FindElements(By.ClassName("profile_count_link_total"));
            if (friendCountElement.Count != 0)
            {
                var friendCountString = friendCountElement.First().GetAttribute("innerHTML");

                if (int.TryParse(friendCountString, out var friendCount))
                {
                    profile.ProfileData.FriendCount = friendCount;
                }
            }

            var commonFriendCountElement = FriendElementContainer.First().FindElements(By.CssSelector("div[class='profile_in_common responsive_hidden']"));
            if (commonFriendCountElement.Count == 0)
            {
                _logger.LogWarning("Could not find commonFriendCountElement for profile with Id: {profileId}", profile.Id);
                return;
            }

            var whiteLinkContainer = commonFriendCountElement.First().FindElements(By.ClassName("whiteLink"));
            if (whiteLinkContainer.Count == 0)
            {
                _logger.LogWarning("Could not find whiteLinkContainer for profile with Id: {profileId}", profile.Id);
                return;
            }

            var commonFriendCountString = whiteLinkContainer.First().GetAttribute("innerHTML");

            commonFriendCountString = ProfileDataParser.StripFriendFromString(commonFriendCountString);

            if (int.TryParse(commonFriendCountString, out var commonFriendCount))
            {
                profile.ProfileData.CommonFriendCount = commonFriendCount;
            }
        }

        private void ScrapeRealNameAndCountry(Profile profile)
        {
            var realNameHeader = _driver.FindElements(By.CssSelector("div[class='header_real_name ellipsis']"));
            if (realNameHeader.Count == 0)
            {
                _logger.LogWarning("Could not find realNameHeader for profile with Id: {profileId}", profile.Id);
                return;
            }

            var realNameElement = realNameHeader.First().FindElements(By.CssSelector("bdi"));

            if (realNameElement.Count != 0)
            {
                var realName = realNameElement.First().GetAttribute("innerHTML");
                profile.ProfileData.RealName = realName;
            }

            var countryInnerHtml = realNameHeader.First().GetAttribute("innerHTML");

            var country = ProfileDataParser.ExtractCountry(countryInnerHtml);

            if (!string.IsNullOrEmpty(country))
            {
                profile.ProfileData.Country = country;
            }
        }

        private void ScrapeLevel(Profile profile)
        {
            var personaElementContainer = _driver.FindElements(By.CssSelector("div[class='persona_name persona_level']"));
            if (personaElementContainer.Count == 0)
            {
                _logger.LogWarning("Could not find personaNameContainer for profile with Id: {profileId}", profile.Id);
                return;
            }

            var playerLevelElement = personaElementContainer.First().FindElements(By.ClassName("friendPlayerLevelNum"));

            if (playerLevelElement.Count == 0)
            {
                return;
            }

            var levelString = playerLevelElement.First().GetAttribute("innerHTML");

            if (int.TryParse(levelString, out var level))
            {
                profile.ProfileData.Level = level;
            }
        }

        private void ScrapeSteamIdAndPersonaName(Profile profile)
        {
            var contentComponent = _driver.FindElements(By.Id("responsive_page_template_content"));

            if (contentComponent.Count == 0)
            {
                _logger.LogWarning("Could not find responsive_page_template_content for profile with Id: {profileId}", profile.Id);
                return;
            }

            var scriptElement = contentComponent
                .First()
                .FindElements(By.XPath(".//script"))
                .FirstOrDefault();

            if (scriptElement == null)
            {
                _logger.LogWarning("Could not find g_rgProfileData for profile with Id: {profileId}", profile.Id);
                return;
            }

            var data = scriptElement.GetAttribute("innerHTML");

            // get json from string and parse
            var parseResult = ProfileDataParser.ParseProfileData(data);

            if (parseResult == null)
            {
                _logger.LogWarning("Could parse profileData for profile with Id: {profileId}", profile.Id);
                return;
            }

            if (long.TryParse(parseResult.SteamId, out var steamId))
            {
                profile.ProfileData.SteamId = steamId;
            }

            profile.ProfileData.PersonaName = parseResult.PersonaName;
        }

        public async Task<ICollection<Profile>> ListFriendCommenters(CancellationToken cancellationToken) =>
            await _profileRepository.ListFriendCommenters(cancellationToken);

        public async Task<Profile?> SetIsExcluded(IsExcludedRequest request) =>
            await _profileRepository.SetIsExcluded(request.URI, request.IsExcluded, request.CancellationToken);

        public async Task<ICollection<Profile>> ListExcludedProfiles(CancellationToken cancellationToken) =>
            await _profileRepository.ListExcludedProfiles(cancellationToken);

        public async Task SetCommentedOn(string URI, CancellationToken cancellationToken) =>
            await _profileRepository.SetCommentedOn(URI, cancellationToken);

        public async Task SetCommentAreaDisabled(string URI, CancellationToken cancellationToken) =>
            await _profileRepository.SetCommentAreaDisabled(URI, cancellationToken);

        private record CommentPageIndexString(string IndexString, string PageUrl);
        private record CommentPageIndex(int Index, string PageUrl);
    }
}
