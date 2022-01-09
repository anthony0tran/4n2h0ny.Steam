using System.IO;

namespace _4n2h0ny.Steam.GUI
{
    public static class Globals
    {
        public static readonly double version = 1.4;

        public static readonly string ChromeDriverPath = Path.Combine(Directory.GetCurrentDirectory(), "Drivers");

        public static readonly int MaxPageIndex = 250;

        public static readonly string CommentTemplate = "Have a great day {0}! :heart:";

        public static readonly string DefaultCommentString = "Have a great day! :heart:";

        public static readonly string DebuggingAddress = "localhost:0420";        
    }
}
