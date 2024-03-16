using _4n2h0ny.Steam.API.Entities;

namespace _4n2h0ny.Steam.API.Models
{
    public record ScrapedProfilesResult
    {
        public int ProfileCount { get; set; }
        public ICollection<Profile> Profiles { get; set; } = [];
    }
}
