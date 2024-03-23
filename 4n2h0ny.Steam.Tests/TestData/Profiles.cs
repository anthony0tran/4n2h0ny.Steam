using _4n2h0ny.Steam.API.Context.Entities;

namespace _4n2h0ny.Steam.Tests.TestData
{
    public class Profiles
    {
        public static Profile Default => new()
        {
            URI = "https://steamcommunity.com/id/4n2h0ny",
            LatestCommentReceivedOn = DateTime.Now,
            IsFriend = true
        };
    }
}
