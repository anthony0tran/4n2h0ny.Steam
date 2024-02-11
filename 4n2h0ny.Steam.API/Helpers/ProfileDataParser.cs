using _4n2h0ny.Steam.API.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace _4n2h0ny.Steam.API.Helpers
{
    public class ProfileDataParser
    {
        public static ProfileDataParseResult? ParseProfileData(string profileDataString)
        {
            string pattern = @"\{(.*?)\}";

            Match match = Regex.Match(profileDataString, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<ProfileDataParseResult>(match.Value, options);
            }

            return null;
        }

        public static string? ExtractCountry(string innerHTMLString)
        {
            var index = innerHTMLString.LastIndexOf('>');
            var result = innerHTMLString[(index+1)..];
            result = result.Replace("\r", string.Empty)
                .Replace("\n", string.Empty)
                .Replace("\t", string.Empty);

            return result;
        }

        public static string? StripEmptyCharacters(string innerHTMLString) => 
            innerHTMLString
                .Replace("\r", string.Empty)
                .Replace("\n", string.Empty)
                .Replace("\t", string.Empty);
    }
}
