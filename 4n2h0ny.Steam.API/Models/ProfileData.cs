namespace _4n2h0ny.Steam.API.Models
{
    public sealed record ProfileData
    {
        public Guid Id { get; init; }
        public long? SteamId { get; set; }
        public string? PersonaName { get; set; }
        public string? RealName { get; set; }
        public bool CommentAreaDisabled { get; set; }
    }
}
