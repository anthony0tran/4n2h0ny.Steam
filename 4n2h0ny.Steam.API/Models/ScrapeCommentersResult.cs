using _4n2h0ny.Steam.API.Entities;

namespace _4n2h0ny.Steam.API.Models
{
    public record ScrapeCommentersResult
    {
        public int CommentersCount { get; set; }
        public ICollection<Profile> Commenters { get; set; } = [];
    }
}
