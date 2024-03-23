using System.Text.RegularExpressions;

namespace _4n2h0ny.Steam.API.Helpers
{
    public static class CommentHelper
    {
        public static IEnumerable<string> CommentContainsValidTags(string comment)
        {
            if (CommentContainsNoTags(comment))
            {
                yield break;
            }

            string pattern = @"\[(?:[^\[\]]*(?:\[(?:[^\[\]]*)\])?[^\[\]]*)*\]";

            MatchCollection matches = Regex.Matches(comment, pattern, RegexOptions.IgnoreCase);

            foreach (Match match in matches.Cast<Match>())
            {
                yield return match.Value;
            }
        }

        public static bool CommentContainsNoTags(string comment) =>
            !comment.Contains('[') && !comment.Contains(']');
    }
}
