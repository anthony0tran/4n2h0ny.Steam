using _4n2h0ny.Steam.API.Models;
using _4n2h0ny.Steam.API.Repositories.Profiles;
using _4n2h0ny.Steam.API.Services.Interfaces;

namespace _4n2h0ny.Steam.API.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly ISteamService _steamService;

        public ProfileService(IProfileRepository profileRepository, ISteamService steamService)
        {
            _profileRepository = profileRepository;
            _steamService = steamService;
        }

        public async Task<ICollection<Profile>> GetCommenters(string? profileUrl, CancellationToken cancellationToken)
        {
            var isLoggedIn = _steamService.CheckLogin(profileUrl);

            if (!isLoggedIn)
            {
                throw new InvalidOperationException("User is not logged in...");
            }

            var profiles = _profileRepository.GetCommenters(profileUrl);
            return await _profileRepository.AddOrUpdateProfile(profiles, cancellationToken);
        }

        public async Task<ICollection<Profile>> GetFriendCommenters(CancellationToken cancellationToken) =>
            await _profileRepository.GetFriendCommenters(cancellationToken);
    }
}
