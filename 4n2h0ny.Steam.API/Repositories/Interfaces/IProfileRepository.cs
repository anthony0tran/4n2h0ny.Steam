using _4n2h0ny.Steam.API.Models;
namespace _4n2h0ny.Steam.API.Repositories.Profiles
{
    public interface IProfileRepository
    {
        public Task<ICollection<Profile>> AddOrUpdateProfile(ICollection<Profile> profiles, CancellationToken cancellationToken);
        public Task<ICollection<Profile>> GetFriendCommenters(CancellationToken cancellationToken);
        public Task<DateTime?> GetDateLatestComment(CancellationToken cancellationToken);
        public Task<Profile?> SetIsExcluded(string URI, bool isExcluded, CancellationToken cancellationToken);
        public Task<ICollection<Profile>> GetExcludedProfiles(CancellationToken cancellationToken);
    }
}
