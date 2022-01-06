using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4n2h0ny.Steam
{
    public static class Globals
    {
        public static readonly string ChromeDriverPath = Path.Combine(Directory.GetCurrentDirectory(), "Drivers");

        public static readonly int MaxPageIndex = 50;

        public static readonly string CommentTemplate = "It’s woohoo Wednesday. Today is going to be a good day. :heart:";

        public static readonly string DefaultCommentString = "It’s woohoo Wednesday. Today is going to be a good day. :heart:";

        public static readonly string DebuggingAddress = "localhost:0420";
    }
}
