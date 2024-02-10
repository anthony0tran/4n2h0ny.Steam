namespace _4n2h0ny.Steam.API.Models
{
    public sealed record ProfileData
    {
        public Guid Id { get; init; }
        public long? SteamId { get; set; }
        public string? PersonaName { get; set; }
        public string? RealName { get; set; }
        public int? Level {  get; set; }
        public int? AwardCount { get; set; }
        public int? BadgeCount { get; set; }
        public int? FriendCount { get; set; }
        public int? CommonFriendCount { get; set; }
        public bool CommentAreaDisabled { get; set; }
    }
}
