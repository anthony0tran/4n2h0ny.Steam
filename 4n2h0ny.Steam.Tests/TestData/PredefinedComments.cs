using _4n2h0ny.Steam.API.Context.Entities;

namespace _4n2h0ny.Steam.Tests.TestData
{
    public static class PredefinedComments
    {
        private static readonly DateTime _latestCommentedOn = DateTime.UtcNow;
        public static ICollection<PredefinedComment> testPredefinedComments = [First, Second, FirstWithHighPriority, FirstWithLowPriority, SecondWithHighPriority, SecondWithLowPriority];

        public static PredefinedComment First => new()
        {
            CommentString = "First",
            LatestCommentedOn = _latestCommentedOn.AddDays(-1),
            Priority = CommentPriority.Default
        };

        public static PredefinedComment Second => new()
        {
            CommentString = "Second",
            LatestCommentedOn = _latestCommentedOn,
            Priority = CommentPriority.Default
        };

        public static PredefinedComment FirstWithHighPriority => new()
        {
            CommentString = "First with high priority",
            LatestCommentedOn = _latestCommentedOn.AddDays(-1),
            Priority = CommentPriority.High
        };

        public static PredefinedComment SecondWithHighPriority => new()
        {
            CommentString = "Second with high priority",
            LatestCommentedOn = _latestCommentedOn,
            Priority = CommentPriority.High
        };

        public static PredefinedComment FirstWithLowPriority => new()
        {
            CommentString = "First with low priority",
            LatestCommentedOn = _latestCommentedOn.AddDays(-1),
            Priority = CommentPriority.Low
        };

        public static PredefinedComment SecondWithLowPriority => new()
        {
            CommentString = "Second with low priority",
            LatestCommentedOn = _latestCommentedOn,
            Priority = CommentPriority.Low
        };
    }
}
