using System.Text.RegularExpressions;

namespace _4n2h0ny.Steam.API.Helpers
{
    public static class CommentHelper
    {
        public static bool CommentContainsValidTags(string comment)
        {
            if (CommentContainsNoTags(comment))
            {
                return false;
            }

            string pattern = @"\[(?:[^\[\]]*(?:\[(?:[^\[\]]*)\])?[^\[\]]*)*\]";

            Match match = Regex.Match(comment, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return true;
            }

            return false;
        }

        public static bool CommentContainsNoTags(string comment) =>
            !comment.Contains('[') && !comment.Contains(']');
    }
}
