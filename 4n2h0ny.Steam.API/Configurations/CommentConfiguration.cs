namespace _4n2h0ny.Steam.API.Configurations
{
    public record CommentConfiguration
    {
        public required bool EnableCommenting { get; init; }
        public required string DefaultComment { get; init; }
    }
}
