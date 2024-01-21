namespace _4n2h0ny.Steam.API.Models
{
    public record Profile
    {
        public required string ProfileUrl { get; init; }
        public DateTime LastDateCommented { get; set; }
        public bool IsFriend { get; set; }
    }
}
