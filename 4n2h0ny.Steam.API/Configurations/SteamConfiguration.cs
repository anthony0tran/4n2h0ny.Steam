namespace _4n2h0ny.Steam.API.Configurations
{
    public record SteamConfiguration
    {
        public required string DefaultProfileUrl { get; init; }
        public required string CommentPageUrl { get; init; }
        public required string FirefoxProfile { get; init; }
    }
}
