using _4n2h0ny.Steam.API.Entities;
using _4n2h0ny.Steam.API.Repositories;
using _4n2h0ny.Steam.API.Repositories.Profiles;
using _4n2h0ny.Steam.Tests.TestData;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace _4n2h0ny.Steam.Tests
{
    public class ProfileRepositoryTests
    {
        private readonly IProfileRepository _profileRepository;
        private readonly ProfileContext _profileContext;
        private readonly ILogger<ProfileRepository> _logger = Substitute.For<ILogger<ProfileRepository>>();

        public ProfileRepositoryTests()
        {
            _profileContext = CreateContext();
            _profileRepository = new ProfileRepository(_profileContext, _logger);
        }

        [Fact]
        public void ProfileHashSetShouldNotAddDuplicates()
        {
            var hashset = new HashSet<Profile>();

            var profile = Profiles.Default;
            var profileIdentical = Profiles.Default;
            var profileDifferentDate = Profiles.Default with { LatestCommentReceivedOn = DateTime.Now.AddDays(1) };
            var profileDifferentIsFriend = Profiles.Default with { IsFriend = false };

            hashset.Add(profile);
            hashset.Add(profileIdentical);
            hashset.Add(profileDifferentDate);
            hashset.Add(profileDifferentIsFriend);

            Assert.Single(hashset);
        }

        [Fact]
        public async Task ShouldAddProfilesIfNotExistsAndUpdateExisting()
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
            Assert.Equal(2, result.Count);
            Assert.Equal(2, dbResults.Count);
            Assert.False(dbResults.Single(p => p.URI == Profiles.Default.URI).IsFriend);
        }

        [Fact]
        public async Task ShouldNotAddExisting()
        {
            // Arrange
            _profileContext.Profiles.Add(Profiles.Default);
            _profileContext.SaveChanges();

            var profiles = new List<Profile>()
            {
                Profiles.Default
            };

            // Act
            var result = await _profileRepository.AddOrUpdateProfile(profiles, Arg.Any<CancellationToken>());

            // Assert
            var dbResults = _profileContext.Profiles.ToList();
            Assert.Single(result);
            Assert.Single(dbResults);
        }

        [Fact]
        public async Task ShouldEditExisting()
        {
            // Arrange
            _profileContext.Profiles.Add(Profiles.Default);
            _profileContext.SaveChanges();

            var profiles = new List<Profile>()
            {
                Profiles.Default with
                {
                    LatestCommentReceivedOn = DateTime.UtcNow,
                    IsFriend = false
                }
            };

            // Act
            var result = await _profileRepository.AddOrUpdateProfile(profiles, Arg.Any<CancellationToken>());

            // Assert
            var dbResults = _profileContext.Profiles.ToList();
            Assert.Single(result);
            Assert.Single(dbResults);
            Assert.True(dbResults.Single().LatestCommentReceivedOn == profiles.Single().LatestCommentReceivedOn);
            Assert.True(dbResults.Single().IsFriend == profiles.Single().IsFriend);
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