using _4n2h0ny.Steam.API.Context.Entities;

namespace _4n2h0ny.Steam.API.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        public Task AddComment(string comment, DateTime commentProcessStartedOn, ICollection<Profile> commentedProfiles, CancellationToken cancellationToken);
        public Task AddPredefinedComment(string commentString, CancellationToken cancellationToken);
        public Task<ICollection<PredefinedComment>> ListPredefinedComments(CancellationToken cancellationToken);
        public Task SetPriority(Guid predefinedCommentId, CommentPriority priority, CancellationToken cancellationToken);
    }
}
