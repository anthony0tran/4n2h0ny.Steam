namespace _4n2h0ny.Steam.API.Context.Entities
{
    public sealed record Comment
    {
        public Guid Id { get; init; }
        public required string CommentString { get; init; }
        public DateTime CommentProcessStartedOn { get; init; }
        public ICollection<Profile> Profiles { get; set; } = [];
    }
}
