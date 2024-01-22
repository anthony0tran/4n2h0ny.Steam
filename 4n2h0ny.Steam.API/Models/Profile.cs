namespace _4n2h0ny.Steam.API.Models
{
    public sealed record Profile : IEquatable<Profile>
    {
        public required string ProfileUrl { get; init; }
        public DateTime LastDateCommented { get; set; }
        public bool IsFriend { get; set; }

        public override int GetHashCode()
        {
            return ProfileUrl.GetHashCode();
        }

        public bool Equals(Profile? other)
            => other != null && ProfileUrl == other.ProfileUrl;
    }
}
