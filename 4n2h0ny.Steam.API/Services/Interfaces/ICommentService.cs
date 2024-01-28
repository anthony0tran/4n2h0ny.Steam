namespace _4n2h0ny.Steam.API.Services.Interfaces
{
    public interface ICommentService
    {
        public Task CommentOnFriendCommenters(string comment, CancellationToken cancellationToken);
    }
}
