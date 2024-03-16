using _4n2h0ny.Steam.API.Entities;

namespace _4n2h0ny.Steam.API.Services.Interfaces
{
    public interface IProfileService
    {
        public Task<ICollection<Profile>> GetCommenters(string? profileUrl, bool scrapeAll, CancellationToken cancellationToken);
        public Task<ICollection<Profile>> ListFriendCommenters(CancellationToken cancellationToken);
        public Task<Profile?> SetIsExcluded(IsExcludedRequest request);
        public Task<ICollection<Profile>> ListExcludedProfiles(CancellationToken cancellationToken);
        public Task SetCommentedOn(string URI, CancellationToken cancellationToken);
        public Task SetCommentAreaDisabled(string URI, CancellationToken cancellationToken);
        public Task FetchProfileData(CancellationToken cancellationToken);
        public Task FetchProfileData(string URI, CancellationToken cancellationToken);
    }

    public record IsExcludedRequest(string URI, bool IsExcluded, CancellationToken CancellationToken);
}
