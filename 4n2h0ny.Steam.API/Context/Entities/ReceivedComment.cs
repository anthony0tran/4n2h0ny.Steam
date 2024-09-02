namespace _4n2h0ny.Steam.API.Context.Entities
{
    public sealed record ReceivedComment
    {
        public Guid Id { get; init; }
        public required string CommentString { get; init; }
        public required Profile Profile { get; init; }
        public DateTime ReceivedOn { get; init; }
    }
}
