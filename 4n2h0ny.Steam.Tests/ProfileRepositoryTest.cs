using _4n2h0ny.Steam.API;
using _4n2h0ny.Steam.API.Configurations;
using _4n2h0ny.Steam.API.Helpers;
using _4n2h0ny.Steam.API.Models;
using _4n2h0ny.Steam.API.Repositories;
using _4n2h0ny.Steam.API.Repositories.Profiles;
using _4n2h0ny.Steam.Tests.TestData;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace _4n2h0ny.Steam.Tests
{
    public class HelpersTest
    {
        private readonly IProfileRepository _profileRepository;
        private readonly ProfileContext _profileContext;
        private readonly WebDriverSingleton _webDriverSingleton;

        public HelpersTest()
        {
            _webDriverSingleton = WebDriverSingleton.Instance;
            _profileContext = CreateContext();
            _profileRepository = new ProfileRepository(Substitute.For<IOptions<SteamConfiguration>>(), _profileContext);
        }

        [Fact]
        public void DateStringShouldParseToDateTime()
        {
            var dateString = "1705856586";
            var result = DateParser.ParseUnixTimeStampToDateTime(dateString);

            Assert.True(result == new DateTime(2024, 1, 21, 17, 3, 6));
        }

        [Fact]
        public void InvalidStringShouldThrowException()
        {
            var dateString = "invalidDateString";
            var result = DateParser.ParseUnixTimeStampToDateTime(dateString);

            Assert.True(result == DateTime.MinValue);
        }

        [Fact]
        public void ProfileHashSetShouldNotAddDuplicates()
        {
            var hashset = new HashSet<Profile>();

            var profile = Profiles.Default;
            var profileIdentical = Profiles.Default;
            var profileDifferentDate = Profiles.Default with { LastDateCommented = DateTime.Now.AddDays(1) };
            var profileDifferentIsFriend = Profiles.Default with { IsFriend = false };

            hashset.Add(profile);
            hashset.Add(profileIdentical);
            hashset.Add(profileDifferentDate);
            hashset.Add(profileDifferentIsFriend);

            Assert.Single(hashset);
        }

        [Fact]
        public async Task ShouldAddProfilesIfNotExists()
        {
            // Arrange
            _profileContext.Profiles.Add(Profiles.Default);
            _profileContext.SaveChanges();

            var profiles = new List<Profile>()
            {
                Profiles.Default with 
                { 
                    IsFriend = false 
                },
                Profiles.Default with
                {
                    URI = "https://steamcommunity.com/id/friend1"
                }
            };

            // Act
            var result = await _profileRepository.AddOrUpdateProfile(profiles, Arg.Any<CancellationToken>());

            // Assert
            var dbResults = _profileContext.Profiles.ToList();
            Assert.Equal(2, dbResults.Count);
            Assert.False(dbResults.Single(p => p.URI == Profiles.Default.URI).IsFriend);
            _webDriverSingleton.Quit();
        }

        private static ProfileContext CreateContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ProfileContext>()
                .UseSqlite(connection)
                .Options;

            var context = new ProfileContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}