using _4n2h0ny.Steam.API.Context;
using _4n2h0ny.Steam.API.Context.Entities;
using _4n2h0ny.Steam.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task AddPredefinedComment(string commentString, CancellationToken cancellationToken)
        {
            var predefinedComment = new PredefinedComment()
            {
                CommentString = commentString
            };

            await _profileContext.AddAsync(predefinedComment);
            await _profileContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<PredefinedComment> GetFirstPredefinedCommentInQueue(CancellationToken cancellationToken) =>
            await _profileContext.PredefinedComments
                .OrderByDescending(pc => pc.Priority)
                .ThenBy(pc => pc.LatestCommentedOn)
                .FirstOrDefaultAsync(cancellationToken) 
                ?? throw new InvalidOperationException("No predefinedComment found");

        public async Task PredefinedCommentPostProcess(PredefinedComment predefinedComment, CancellationToken cancellationToken)
        {
            predefinedComment.Priority = CommentPriority.Default;
            predefinedComment.CommentedCount++;
            predefinedComment.LatestCommentedOn = DateTime.UtcNow;
            await _profileContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<ICollection<PredefinedComment>> ListPredefinedComments(CancellationToken cancellationToken) => 
            await _profileContext.PredefinedComments.ToListAsync(cancellationToken);

        public async Task SetPriority(Guid predefinedCommentId, CommentPriority priority, CancellationToken cancellationToken)
        {
            var predefinedComment = await _profileContext.PredefinedComments.FirstOrDefaultAsync(pc => pc.Id == predefinedCommentId, cancellationToken) 
                ?? throw new InvalidOperationException($"No predefinedComment found with Id: {predefinedCommentId}");

            predefinedComment.Priority = priority;

            await _profileContext.SaveChangesAsync(cancellationToken);
        }
    }
}
