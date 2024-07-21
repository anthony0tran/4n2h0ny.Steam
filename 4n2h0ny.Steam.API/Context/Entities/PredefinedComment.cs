namespace _4n2h0ny.Steam.API.Context.Entities
{
    public sealed record PredefinedComment
    {
        public Guid Id { get; init; }
        public required string CommentString { get; init; }
        public DateTime? LatestCommentedOn { get; init; }
        public DateTime Created { get; init; }
        public int CommentedCount { get; set; }
    }
}
