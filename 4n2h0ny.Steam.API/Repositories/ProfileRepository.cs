using _4n2h0ny.Steam.API.Models;
using _4n2h0ny.Steam.API.Repositories.Profiles;
using Microsoft.EntityFrameworkCore;

namespace _4n2h0ny.Steam.API.Repositories
{
    public class ProfileRepository : IProfileRepository
    {            
        private readonly ProfileContext _profileContext;

        public ProfileRepository(ProfileContext profileContext)
        {       
            _profileContext = profileContext;
        }

        public async Task<ICollection<Profile>> AddOrUpdateProfile(ICollection<Profile> foundProfiles, CancellationToken cancellationToken)
        {
            var existingProfiles = await _profileContext.Profiles
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
                existingProfile.LastDateCommented = profile.LastDateCommented;
            }

            var newProfiles = foundProfiles.Where(np => !existingProfiles.Select(p => p.URI).Contains(np.URI));

            await _profileContext.AddRangeAsync(newProfiles, cancellationToken);
            await _profileContext.SaveChangesAsync(cancellationToken);
            return foundProfiles;
        }

        public async Task<ICollection<Profile>> GetFriendCommenters(CancellationToken cancellationToken) =>
            await _profileContext.Profiles
                .Where(p => p.IsFriend)
                .ToArrayAsync(cancellationToken);

        public async Task<DateTime?> GetDateLatestComment(CancellationToken cancellationToken) =>
            await _profileContext.Profiles
            .OrderByDescending(p => p.LastDateCommented)
            .Take(1)
            .Select(p => p.LastDateCommented)
            .SingleOrDefaultAsync(cancellationToken);
    }
}