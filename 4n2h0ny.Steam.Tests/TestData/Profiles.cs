using _4n2h0ny.Steam.API.Models;

namespace _4n2h0ny.Steam.Tests.TestData
{
    public class Profiles
    {
        public static Profile Default => new()
        {
            ProfileUrl = "https://steamcommunity.com/id/4n2h0ny",
            LastDateCommented = DateTime.Now,
            IsFriend = true
        };
    }
}
