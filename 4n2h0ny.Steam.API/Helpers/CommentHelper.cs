using System.Text.RegularExpressions;

namespace _4n2h0ny.Steam.API.Helpers
{
    public static class CommentHelper
    {
        public static bool ContainsTag(string comment)
        {
            string pattern = @"\[[^\[\]]*?\](?=\s|$)";

            Match match = Regex.Match(comment, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return true;
            }

            return false;
        }
    }
}
