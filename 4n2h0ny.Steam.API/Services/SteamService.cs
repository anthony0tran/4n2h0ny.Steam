using _4n2h0ny.Steam.API.Repositories.Profiles;
using _4n2h0ny.Steam.API.Services.Interfaces;

namespace _4n2h0ny.Steam.API.Services
{
    public class SteamService : ISteamService
    {
        private readonly ISteamRepository _steamRepository;
        public SteamService(ISteamRepository steamRepository)
        {
            _steamRepository = steamRepository;
        }

        public bool CheckLogin(string? profileUrl)
        {
            return _steamRepository.CheckLogin(profileUrl);
        }
    }
}
