namespace _4n2h0ny.Steam.API.Context.Entities
{
    public sealed record PredefinedComment
    {
        public Guid Id { get; init; }
        public required string CommentString { get; init; }
        public DateTime? LatestCommentedOn { get; init; }
        public DateTime Created { get; private init; }
        public int CommentedCount { get; set; }
        public CommentPriority Priority { get; set; } = CommentPriority.Default;
    }

    public enum CommentPriority
    {
        Default = 0,
        Low = 1,
        High =2
    }
}
