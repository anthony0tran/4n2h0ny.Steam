using _4n2h0ny.Steam.API.Context.Entities;

namespace _4n2h0ny.Steam.API.Models
{
    public record ScrapedProfilesResult
    {
        public int ProfileCount { get; set; }
        public TimeSpan ExecutionDuration { get; set; }
        public ICollection<string> ProfileURIs { get; set; } = [];
    }
}
