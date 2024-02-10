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
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<ProfileDataParseResult>(match.Value, options);
            }

            return null;
        }
    }
}
