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
        public Task SetCommentedOn(string URI, CancellationToken cancellationToken);
        public Task SetCommentAreaDisabled(string URI, CancellationToken cancellationToken);
        public Task<ICollection<Profile>> GetAllProfilesIgnoreQueryFilters(CancellationToken cancellationToken);
        public Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
