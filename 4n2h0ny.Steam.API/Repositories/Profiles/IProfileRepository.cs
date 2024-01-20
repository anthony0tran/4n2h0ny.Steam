using _4n2h0ny.Steam.API.Models;
namespace _4n2h0ny.Steam.API.Repositories.Profiles
{
    public interface IProfileRepository
    {
        public ICollection<Profile> GetCommenters(string? profileUrl);
    }
}
