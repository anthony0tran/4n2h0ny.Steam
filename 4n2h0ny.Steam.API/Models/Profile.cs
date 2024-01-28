namespace _4n2h0ny.Steam.API.Models
{
    public sealed record Profile : IEquatable<Profile>
    {
        public Guid Id { get; set; }
        public required string URI { get; init; }
        public DateTime LastDateCommented { get; set; }
        public bool IsFriend { get; set; }
        public bool IsExcluded { get; set; }

        public override int GetHashCode()
        {
            return URI.GetHashCode();
        }

        public bool Equals(Profile? other)
            => other != null && URI == other.URI;
    }
}
