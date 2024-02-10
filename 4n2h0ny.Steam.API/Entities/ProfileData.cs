namespace _4n2h0ny.Steam.API.Entities
{
    public sealed record ProfileData
    {
        public Guid Id { get; init; }
        public long? SteamId { get; set; }
        public string? PersonaName { get; set; }
        public string? RealName { get; set; }
        public string? Country { get; set; }
        public int? Level {  get; set; }
        public int? AwardCount { get; set; }
        public int? BadgeCount { get; set; }
        public int? FriendCount { get; set; }
        public int? CommonFriendCount { get; set; }
        public int? TotalCommendsCount { get; set; }
        public int? GameCount { get; set; }
        public bool CommentAreaDisabled { get; set; }
        public DateTime? LastFetchedOn { get; set; }
    }
}
