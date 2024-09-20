using _4n2h0ny.Steam.API.Context.Entities;

namespace _4n2h0ny.Steam.API.Repositories.Profiles
{
    public interface IProfileRepository
    {
        public Task<ICollection<Profile>> AddOrUpdateProfile(ICollection<Profile> profiles, CancellationToken cancellationToken);
        public Task<ICollection<Profile>> ListFriendCommenters(CancellationToken cancellationToken);
        public Task<DateTime?> GetDateLatestComment(CancellationToken cancellationToken);
        public Task<Profile?> SetIsExcluded(string URI, bool isExcluded, CancellationToken cancellationToken);
        public Task<ICollection<Profile>> ListExcludedProfiles(CancellationToken cancellationToken);
        public Task SetCommentedOn(string URI, CancellationToken cancellationToken);
        public Task SetCommentAreaDisabled(string URI, CancellationToken cancellationToken);
        public Task<ICollection<Profile>> GetCommentAreaDisabledProfiles(CancellationToken cancellationToken);
        public Task SetProfileNotFound(string URI, bool profileNotFound, CancellationToken cancellationToken);
        public Task SetProfileIsPrivate(string URI, bool isPrivate, CancellationToken cancellationToken);
        public Task<ICollection<Profile>> GetAllProfilesIgnoreQueryFilters(CancellationToken cancellationToken);
        public Task SaveChangesAsync(CancellationToken cancellationToken);
        public Task<Profile?> GetProfileByURI(string URI, CancellationToken cancellationToken);
        public Task ResetIsFriends(CancellationToken cancellationToken);
        public Task<ICollection<Profile>> GetFriendsWithActiveCommentThread(CancellationToken cancellationToken);
        public Task<ReceivedComment?> GetLatestReceivedComment(CancellationToken cancellationToken);
        public Task<ICollection<ReceivedComment>> AddReceivedComments(ICollection<ReceivedComment> foundReceivedComments, CancellationToken cancellationToken);
        public Task<Profile?> GetProfileByURIIgnoreFilters(string URI, CancellationToken cancellationToken);
        public Task SyncProfileLatestCommentReceivedOn(Profile profile, DateTime latestCommentReceivedOn, CancellationToken cancellationToken);
    }
}
