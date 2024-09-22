namespace _4n2h0ny.Steam.API.Models
{
    public record ReceivedCommentsResult
    {
        public int CommentCount { get; set; }
        public TimeSpan ExecutionDuration { get; set; }
    }
}
