using _4n2h0ny.Steam.API.Models;
using _4n2h0ny.Steam.API.Repositories.Profiles;
using _4n2h0ny.Steam.API.Services.Interfaces;

namespace _4n2h0ny.Steam.API.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<ICollection<Profile>> GetCommenters(string? profileUrl, CancellationToken cancellationToken)
        {
            var profiles = _profileRepository.GetCommenters(profileUrl);
            return await _profileRepository.AddOrUpdateProfile(profiles, cancellationToken);
        }
    }
}
