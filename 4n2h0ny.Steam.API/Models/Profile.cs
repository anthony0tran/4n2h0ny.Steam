namespace _4n2h0ny.Steam.API.Models
{
    public sealed record Profile : IEquatable<Profile>
    {
        public Guid Id { get; init; }
        public required string URI { get; init; }
        public DateTime LatestCommentReceivedOn { get; set; }
        public bool IsFriend { get; set; }
        public bool IsExcluded { get; set; }
        public DateTime? CommentedOn { get; set; }
        public DateTime? FetchedOn { get; set; }
        public Guid ProfileDataId { get; set; }
        public ProfileData ProfileData { get; set; } = new();

        public override int GetHashCode()
            => URI.GetHashCode();

        public bool Equals(Profile? other)
            => other != null && URI == other.URI;
    }
}
