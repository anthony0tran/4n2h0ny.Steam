using _4n2h0ny.Steam.API.Context;
using _4n2h0ny.Steam.API.Context.Entities;
using _4n2h0ny.Steam.API.Repositories.Profiles;
using Microsoft.EntityFrameworkCore;

namespace _4n2h0ny.Steam.API.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ProfileContext _profileContext;
        private readonly ILogger<ProfileRepository> _logger;

        public ProfileRepository(ProfileContext profileContext, ILogger<ProfileRepository> logger)
        {
            _profileContext = profileContext;
            _logger = logger;
        }

        public async Task<ICollection<Profile>> AddOrUpdateProfile(ICollection<Profile> foundProfiles, CancellationToken cancellationToken)
        {
            var existingProfiles = await _profileContext.Profiles
                .IgnoreQueryFilters()
                .Where(p => foundProfiles.Select(fp => fp.URI).Contains(p.URI))
                .ToListAsync(cancellationToken);

            foreach (var profile in foundProfiles)
            {
                var existingProfile = existingProfiles.SingleOrDefault(p => p.URI == profile.URI);
                if (existingProfile == null)
                {
                    continue;
                }
                existingProfile.IsFriend = profile.IsFriend;
                existingProfile.FetchedOn = profile.FetchedOn;
                existingProfile.FetchedIsFriendOn = profile.FetchedIsFriendOn;
            }

            var newProfiles = foundProfiles.Where(np => !existingProfiles.Select(p => p.URI).Contains(np.URI));

            await _profileContext.AddRangeAsync(newProfiles, cancellationToken);
            await _profileContext.SaveChangesAsync(cancellationToken);
            return foundProfiles;
        }

        public async Task<ICollection<Profile>> ListFriendCommenters(CancellationToken cancellationToken) =>
            await _profileContext.Profiles
                .Where(p => p.IsFriend && !p.IsExcluded && !p.ProfileData.CommentAreaDisabled)
                .Where(p => p.LatestCommentReceivedOn.Date >= DateTime.Today.AddMonths(-3))
                .Where(p => p.CommentedOn == null
                || p.CommentedOn <= DateTime.UtcNow.AddHours(-2))
                .ToArrayAsync(cancellationToken);

        public async Task<DateTime?> GetDateLatestComment(CancellationToken cancellationToken) =>
            await _profileContext.Profiles
            .OrderByDescending(p => p.LatestCommentReceivedOn)
            .Take(1)
            .Select(p => p.LatestCommentReceivedOn)
            .SingleOrDefaultAsync(cancellationToken);

        public async Task<Profile?> SetIsExcluded(string URI, bool isExcluded, CancellationToken cancellationToken)
        {
            var profile = await _profileContext.Profiles
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(p => p.URI == URI, cancellationToken);

            if (profile == null)
            {
                _logger.LogWarning("Profile with URI: {URI}, not found.", URI);
                return null;
            }

            profile.IsExcluded = isExcluded;

            await _profileContext.SaveChangesAsync(cancellationToken);
            return profile;
        }

        public async Task SetCommentedOn(string URI, CancellationToken cancellationToken)
        {
            var profile = await _profileContext.Profiles.SingleOrDefaultAsync(p => p.URI == URI, cancellationToken);

            if (profile == null)
            {
                _logger.LogWarning("Could not set commentedOn. Profile with URI: {URI} not found", URI);
                return;
            }

            profile.CommentedOn = DateTime.UtcNow;
            await _profileContext.SaveChangesAsync(cancellationToken);
        }

        public async Task SetCommentAreaDisabled(string URI, CancellationToken cancellationToken)
        {
            var profile = await _profileContext.Profiles.SingleOrDefaultAsync(p => p.URI == URI, cancellationToken);

            if (profile == null)
            {
                _logger.LogWarning("Could not set commentAreaDisabled. Profile with URI: {URI} not found", URI);
                return;
            }

            profile.ProfileData.CommentAreaDisabled = true;
            await _profileContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<ICollection<Profile>> GetAllProfilesIgnoreQueryFilters(CancellationToken cancellationToken) =>
            await _profileContext.Profiles
                .IgnoreQueryFilters()
                .ToListAsync(cancellationToken);

        public async Task<ICollection<Profile>> GetCommentAreaDisabledProfiles(CancellationToken cancellationToken) =>
            await _profileContext.Profiles
                .IgnoreQueryFilters()
                .Where(p => p.ProfileData.CommentAreaDisabled == true)
                .ToListAsync(cancellationToken);

        public async Task<ICollection<Profile>> ListExcludedProfiles(CancellationToken cancellationToken) =>
            await _profileContext.Profiles
                .IgnoreQueryFilters()
                .Where(p => p.IsExcluded)
                .ToListAsync(cancellationToken);

        public async Task SaveChangesAsync(CancellationToken cancellationToken) =>
            await _profileContext.SaveChangesAsync(cancellationToken);

        public async Task<Profile?> GetProfileByURI(string URI, CancellationToken cancellationToken) =>
            await _profileContext.Profiles.SingleOrDefaultAsync(p => p.URI == URI, cancellationToken);

        public async Task SetProfileNotFound(string URI, bool profileNotFound, CancellationToken cancellationToken)
        {
            var profile = await _profileContext.Profiles.SingleOrDefaultAsync(p => p.URI == URI, cancellationToken);

            if (profile == null)
            {
                _logger.LogWarning("Could not set ProfileNotFound. Profile with URI: {URI} not found", URI);
                return;
            }

            profile.NotFound = profileNotFound;
            await _profileContext.SaveChangesAsync(cancellationToken);
        }

        public async Task SetProfileIsPrivate(string URI, bool isPrivate, CancellationToken cancellationToken)
        {
            var profile = await _profileContext.Profiles.SingleOrDefaultAsync(p => p.URI == URI, cancellationToken);

            if (profile == null)
            {
                _logger.LogWarning("Could not set IsPrivate. Profile with URI: {URI} not found", URI);
                return;
            }

            profile.IsPrivate = isPrivate;
            await _profileContext.SaveChangesAsync(cancellationToken);
        }

        public async Task ResetIsFriends(CancellationToken cancellationToken)
        {
            var friendsQueryable = _profileContext.Profiles
                .IgnoreQueryFilters()
                .Where(p => p.IsFriend);

            foreach (var friend in friendsQueryable)
            {
                friend.IsFriend = false;
            }

            await _profileContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<ICollection<Profile>> GetFriendsWithActiveCommentThread(CancellationToken cancellationToken) =>
            await _profileContext.Profiles
            .Where(p => p.IsFriend && !p.IsExcluded && !p.ProfileData.CommentAreaDisabled)
            .Where(p => p.ProfileData.LatestDateCommentOnFetch > DateTime.UtcNow.AddDays(-31))
            .Where(p => p.ProfileData.CommentDelta > 10)
            .Where(p => p.CommentedOn == null
            || p.CommentedOn <= DateTime.UtcNow.AddHours(-2))
            .ToArrayAsync(cancellationToken);
    }
}