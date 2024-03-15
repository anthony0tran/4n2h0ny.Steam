namespace _4n2h0ny.Steam.API.Services.Interfaces
{
    public interface ICommentService
    {
        public Task<int> CommentOnFriendCommenters(string comment, CancellationToken cancellationToken);
        public Task PreviewComment(string comment, CancellationToken cancellationToken);
    }
}
