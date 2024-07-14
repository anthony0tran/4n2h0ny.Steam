using _4n2h0ny.Steam.API.Context;
using _4n2h0ny.Steam.API.Context.Entities;
using _4n2h0ny.Steam.API.Repositories;
using _4n2h0ny.Steam.API.Repositories.Interfaces;
using _4n2h0ny.Steam.Tests.TestData;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace _4n2h0ny.Steam.Tests
{
    public class CommentRepositoryTests
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ProfileContext _profileContext;
        private readonly ILogger<CommentRepository> _logger = Substitute.For<ILogger<CommentRepository>>();
        public CommentRepositoryTests()
        {
            _profileContext = CreateContext();
            _commentRepository = new CommentRepository(_profileContext, _logger);

        }

        [Fact]
        public async Task ShouldAddComments()
        {
            // Arrange
            var firstComment = "Hello World!";
            var secondComment = "Enjoy your day!";

            var profiles = new List<Profile>()
            {
                Profiles.Default,
                Profiles.Default with {URI = "http://second"},
                Profiles.Default with {URI = "http://third"},
            };

            _profileContext.AddRange(profiles);
            _profileContext.SaveChanges();

            // Act
            await _commentRepository.AddComment(firstComment, DateTime.UtcNow, profiles, CancellationToken.None);
            await _commentRepository.AddComment(secondComment, DateTime.UtcNow, profiles, CancellationToken.None);

            // Assert
            var dbResults = _profileContext.Profiles.Include(p => p.Comments).ToArray();

            Assert.Equal(profiles.Count, dbResults.Length);
            Assert.All(dbResults, p => Assert.Contains(firstComment, p.Comments.Select(c => c.CommentString)));
            Assert.All(dbResults, p => Assert.Contains(secondComment, p.Comments.Select(c => c.CommentString)));
            Assert.All(dbResults, p => Assert.Equal(2, p.Comments.Count));
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
