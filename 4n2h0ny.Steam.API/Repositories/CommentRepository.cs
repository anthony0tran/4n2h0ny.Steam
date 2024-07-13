using _4n2h0ny.Steam.API.Context;
using _4n2h0ny.Steam.API.Context.Entities;
using _4n2h0ny.Steam.API.Repositories.Interfaces;

namespace _4n2h0ny.Steam.API.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ProfileContext _profileContext;
        private readonly ILogger<CommentRepository> _logger;

        public CommentRepository(ProfileContext profileContext, ILogger<CommentRepository> logger)
        {
            _profileContext = profileContext;
            _logger = logger;
        }

        public async Task AddComment(string commentString, DateTime commentProcessStartedOn, ICollection<Profile> commentedProfiles, CancellationToken cancellationToken)
        {
            var comment = new Comment()
            {
                CommentString = commentString,
                CommentProcessStartedOn = commentProcessStartedOn,
                Profiles = commentedProfiles
            };

            await _profileContext.AddAsync(comment, cancellationToken);
            await _profileContext.SaveChangesAsync(cancellationToken);
        }
    }
}
