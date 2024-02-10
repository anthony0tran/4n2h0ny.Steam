using _4n2h0ny.Steam.API.Helpers;

namespace _4n2h0ny.Steam.Tests
{
    public class HelpersTests
    {
        [Fact]
        public void DateStringShouldParseToDateTime()
        {
            var dateString = "1705856586";
            var result = DateParser.ParseUnixTimeStampToDateTime(dateString);

            Assert.True(result == new DateTime(2024, 1, 21, 17, 3, 6));
        }

        [Fact]
        public void InvalidStringShouldThrowException()
        {
            var dateString = "invalidDateString";
            var result = DateParser.ParseUnixTimeStampToDateTime(dateString);

            Assert.True(result == DateTime.MinValue);
        }

        [Fact]
        public void X()
        {
            string profileDataString = "\r\n\t\tg_rgProfileData = {\"url\":\"https:\\/\\/steamcommunity.com\\/profiles\\/76561198802957358\\/\",\"steamid\":\"76561198802957358\",\"personaname\":\"animutrix\",\"summary\":\"<img src=\\\"https:\\/\\/community.cloudflare.steamstatic.com\\/economy\\/emoticon\\/happyfroggy\\\" alt=\\\":happyfroggy:\\\" class=\\\"emoticon\\\"><br><br> <a class=\\\"bb_link\\\" href=\\\"https:\\/\\/steamcommunity.com\\/id\\/4n2h0ny\\/\\\" target=\\\"_blank\\\" rel=\\\"\\\" > <img src=\\\"https:\\/\\/community.cloudflare.steamstatic.com\\/economy\\/emoticon\\/VLOVEIT\\\" alt=\\\":VLOVEIT:\\\" class=\\\"emoticon\\\"><img src=\\\"https:\\/\\/community.cloudflare.steamstatic.com\\/economy\\/emoticon\\/VLOVEIT\\\" alt=\\\":VLOVEIT:\\\" class=\\\"emoticon\\\"><img src=\\\"https:\\/\\/community.cloudflare.steamstatic.com\\/economy\\/emoticon\\/VLOVEIT\\\" alt=\\\":VLOVEIT:\\\" class=\\\"emoticon\\\"> <\\/a> \"};\r\n\t\tconst g_bViewingOwnProfile = 0;\r\n\t\t$J( function() {\r\n\t\t\twindow.Responsive_ReparentItemsInResponsiveMode && Responsive_ReparentItemsInResponsiveMode( '.responsive_groupfriends_element', $J('#responsive_groupfriends_element_ctn') );\r\n\t\t\t\r\n\t\t\tSetupAnimateOnHoverImages();\r\n\t\t});\r\n\t";
        
            ProfileDataParser.ParseProfileData(profileDataString);
        }
    }
}
