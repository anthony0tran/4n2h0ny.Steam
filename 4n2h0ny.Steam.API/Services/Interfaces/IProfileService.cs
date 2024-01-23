using _4n2h0ny.Steam.API.Models;

namespace _4n2h0ny.Steam.API.Services.Interfaces
{
    public interface IProfileService
    {
        public Task<ICollection<Profile>> GetCommenters(string? profileUrl, CancellationToken cancellationToken);
    }
}
