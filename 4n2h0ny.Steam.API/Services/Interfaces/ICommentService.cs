using _4n2h0ny.Steam.API.Context.Entities;

namespace _4n2h0ny.Steam.API.Services.Interfaces
{
    public interface ICommentService
    {
        public Task<int> CommentOnFriendCommenters(string comment, CancellationToken cancellationToken);
        public Task PreviewComment(string URI, string comment, CancellationToken cancellationToken);
        public bool ValidateComment(string comment);
        public Task<int> CommentOnFriendsWithActiveCommentThread(string comment, CancellationToken cancellationToken);
        public Task AddPredefinedComment(string commentString, CancellationToken cancellationToken);
        public Task<ICollection<PredefinedComment>> ListPredefinedComments(CancellationToken cancellationToken);
        public Task SetPriority(Guid predefinedCommentId, CommentPriority priority, CancellationToken cancellationToken);
        public Task<PredefinedComment> GetFirstPredefinedCommentInQueue(CancellationToken cancellationToken);
        public Task PredefinedCommentPostProcess(PredefinedComment predefinedComment, CancellationToken cancellationToken);
    }
}
