using _4n2h0ny.Steam.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace _4n2h0ny.Steam.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SteamController : ControllerBase
    {
        private readonly ISteamService _steamService;

        public SteamController(ISteamService steamService)
        {
            _steamService = steamService;
        }

        [HttpGet("[action]")]
        public bool CheckLogin(string? profileUrl) =>
            _steamService.CheckLogin(profileUrl);
    }
}
