namespace _4n2h0ny.Steam.API.Models
{
    public record ProfileDataParseResult
    {
        public string? PersonaName { get; set; }
        public string? SteamId { get; set; }
        public string? Summary { get; set; }
        public string? Url { get; set; }
    }
}
