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
        public void ShouldParseProfileDataString()
        {
            string profileDataString = "\r\n\t\tg_rgProfileData = {\"url\":\"https:\\/\\/steamcommunity.com\\/profiles\\/76561198802957358\\/\",\"steamid\":\"76561198802957358\",\"personaname\":\"animutrix\",\"summary\":\"<img src=\\\"https:\\/\\/community.cloudflare.steamstatic.com\\/economy\\/emoticon\\/happyfroggy\\\" alt=\\\":happyfroggy:\\\" class=\\\"emoticon\\\"><br><br> <a class=\\\"bb_link\\\" href=\\\"https:\\/\\/steamcommunity.com\\/id\\/4n2h0ny\\/\\\" target=\\\"_blank\\\" rel=\\\"\\\" > <img src=\\\"https:\\/\\/community.cloudflare.steamstatic.com\\/economy\\/emoticon\\/VLOVEIT\\\" alt=\\\":VLOVEIT:\\\" class=\\\"emoticon\\\"><img src=\\\"https:\\/\\/community.cloudflare.steamstatic.com\\/economy\\/emoticon\\/VLOVEIT\\\" alt=\\\":VLOVEIT:\\\" class=\\\"emoticon\\\"><img src=\\\"https:\\/\\/community.cloudflare.steamstatic.com\\/economy\\/emoticon\\/VLOVEIT\\\" alt=\\\":VLOVEIT:\\\" class=\\\"emoticon\\\"> <\\/a> \"};\r\n\t\tconst g_bViewingOwnProfile = 0;\r\n\t\t$J( function() {\r\n\t\t\twindow.Responsive_ReparentItemsInResponsiveMode && Responsive_ReparentItemsInResponsiveMode( '.responsive_groupfriends_element', $J('#responsive_groupfriends_element_ctn') );\r\n\t\t\t\r\n\t\t\tSetupAnimateOnHoverImages();\r\n\t\t});\r\n\t";

            var result = ProfileDataParser.ParseProfileData(profileDataString);

            Assert.NotNull(result);
            Assert.Equal("animutrix", result.PersonaName);
            Assert.Equal("76561198802957358", result.SteamId);
            Assert.Equal("<img src=\"https://community.cloudflare.steamstatic.com/economy/emoticon/happyfroggy\" alt=\":happyfroggy:\" class=\"emoticon\"><br><br> <a class=\"bb_link\" href=\"https://steamcommunity.com/id/4n2h0ny/\" target=\"_blank\" rel=\"\" > <img src=\"https://community.cloudflare.steamstatic.com/economy/emoticon/VLOVEIT\" alt=\":VLOVEIT:\" class=\"emoticon\"><img src=\"https://community.cloudflare.steamstatic.com/economy/emoticon/VLOVEIT\" alt=\":VLOVEIT:\" class=\"emoticon\"><img src=\"https://community.cloudflare.steamstatic.com/economy/emoticon/VLOVEIT\" alt=\":VLOVEIT:\" class=\"emoticon\"> </a> ", result.Summary);
            Assert.Equal("https://steamcommunity.com/profiles/76561198802957358/", result.Url);
        }

        [Fact]
        public void ShouldParseProfileDataWithDeletedLinkInSummary()
        {
            string profileDataString = "\r\n\t\tg_rgProfileData = {\"url\":\"https:\\/\\/steamcommunity.com\\/profiles\\/76561198802957358\\/\",\"steamid\":\"76561198802957358\",\"personaname\":\"animutrix\",\"summary\":\"<img src=\\\"https:\\/\\/community.cloudflare.steamstatic.com\\/economy\\/emoticon\\/happyfroggy\\\" alt=\\\":happyfroggy:\\\" class=\\\"emoticon\\\"><br><br> <a class=\\\"bb_link\\\" href=\\\"https:\\/\\/steamcommunity.com\\/id\\/4n2h0ny\\/\\\" target=\\\"_blank\\\" rel=\\\"\\\" > <img src=\\\"https:\\/\\/community.cloudflare.steamstatic.com\\/economy\\/emoticon\\/VLOVEIT\\\" alt=\\\":VLOVEIT:\\\" class=\\\"emoticon\\\"><img src=\\\"https:\\/\\/community.cloudflare.steamstatic.com\\/economy\\/emoticon\\/VLOVEIT\\\" alt=\\\":VLOVEIT:\\\" class=\\\"emoticon\\\"><img src=\\\"https:\\/\\/community.cloudflare.steamstatic.com\\/economy\\/emoticon\\/VLOVEIT\\\" alt=\\\":VLOVEIT:\\\" class=\\\"emoticon\\\"> <\\/a>{LINK REMOVED} \"};\r\n\t\tconst g_bViewingOwnProfile = 0;\r\n\t\t$J( function() {\r\n\t\t\twindow.Responsive_ReparentItemsInResponsiveMode && Responsive_ReparentItemsInResponsiveMode( '.responsive_groupfriends_element', $J('#responsive_groupfriends_element_ctn') );\r\n\t\t\t\r\n\t\t\tSetupAnimateOnHoverImages();\r\n\t\t});\r\n\t";

            var result = ProfileDataParser.ParseProfileData(profileDataString);

            Assert.NotNull(result);
            Assert.Equal("animutrix", result.PersonaName);
            Assert.Equal("76561198802957358", result.SteamId);
            Assert.Equal("<img src=\"https://community.cloudflare.steamstatic.com/economy/emoticon/happyfroggy\" alt=\":happyfroggy:\" class=\"emoticon\"><br><br> <a class=\"bb_link\" href=\"https://steamcommunity.com/id/4n2h0ny/\" target=\"_blank\" rel=\"\" > <img src=\"https://community.cloudflare.steamstatic.com/economy/emoticon/VLOVEIT\" alt=\":VLOVEIT:\" class=\"emoticon\"><img src=\"https://community.cloudflare.steamstatic.com/economy/emoticon/VLOVEIT\" alt=\":VLOVEIT:\" class=\"emoticon\"><img src=\"https://community.cloudflare.steamstatic.com/economy/emoticon/VLOVEIT\" alt=\":VLOVEIT:\" class=\"emoticon\"> </a> ", result.Summary);
            Assert.Equal("https://steamcommunity.com/profiles/76561198802957358/", result.Url);
        }

        [Fact]
        public void ShouldExtractCountryFromHeaderString()
        {
            string headerString = "\r\n\t\t\t\t\t\t<bdi>Nic</bdi>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t&nbsp;\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<img class=\"profile_flag\" src=\"https://community.cloudflare.steamstatic.com/public/images/countryflags/za.gif\">\r\n\t\t\t\t\t\t\t\t\t\t\t\tSouth Africa\t\t\t\t\t";
            var result = ProfileDataParser.ExtractCountry(headerString);

            Assert.NotNull(result);
            Assert.Equal("South Africa", result);
        }

        [Fact]
        public void ShouldReturnFalseIfMessageDoesNotContainTag()
        {
            var message = "This a text without a tag";
            var result = CommentHelper.CommentContainsValidTags(message);

            Assert.False(result);
        }

        [Theory]
        [InlineData("This a text with brackets in the wrong order ]text[. Like this")]
        [InlineData("]text[")]
        [InlineData("]te]xt[")]
        public void ShouldReturnFalseWhenBracketsAreInvalid(string message)
        {
            var result = CommentHelper.CommentContainsValidTags(message);
            Assert.False(result);
        }

        [Theory]
        [InlineData("This a text with a [text] tag")]
        [InlineData("This text [first] has multiple [tags]")]
        [InlineData("[The whole message is between brackets]")]
        [InlineData("[]")]
        [InlineData("Hello [name]!")]
        public void ShouldReturnTrueWhenBracketsAreValid(string message)
        {
            var result = CommentHelper.CommentContainsValidTags(message);
            Assert.True(result);
        }
    }
}
