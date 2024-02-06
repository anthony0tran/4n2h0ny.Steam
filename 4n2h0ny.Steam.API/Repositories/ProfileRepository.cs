using _4n2h0ny.Steam.API.Models;
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
                existingProfile.LatestCommentReceivedOn = profile.LatestCommentReceivedOn;
                existingProfile.FetchedOn = profile.FetchedOn;
            }

            var newProfiles = foundProfiles.Where(np => !existingProfiles.Select(p => p.URI).Contains(np.URI));

            await _profileContext.AddRangeAsync(newProfiles, cancellationToken);
            await _profileContext.SaveChangesAsync(cancellationToken);
            return foundProfiles;
        }

        public async Task<ICollection<Profile>> GetFriendCommenters(CancellationToken cancellationToken) =>
            await _profileContext.Profiles
                .Where(p => p.IsFriend && !p.IsExcluded && !p.CommentAreaDisabled)
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

            profile.CommentAreaDisabled = true;
            await _profileContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<ICollection<Profile>> GetExcludedProfiles(CancellationToken cancellationToken) =>
            await _profileContext.Profiles
                .IgnoreQueryFilters()
                .Where(p => p.IsExcluded)
                .ToListAsync(cancellationToken);
    }
}