namespace _4n2h0ny.Steam.API.Helpers
{
    public static class URIDecoder
    {
        public static string DecodePercentageEncodedURI(string URI) =>
            URI.Replace("%2F", "/");
    }
}
