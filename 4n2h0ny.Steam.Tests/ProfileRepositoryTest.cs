using _4n2h0ny.Steam.API.Helpers;
using _4n2h0ny.Steam.API.Models;
using _4n2h0ny.Steam.Tests.TestData;

namespace _4n2h0ny.Steam.Tests
{
    public class HelpersTest
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
        public void ProfileHashSetShouldNotAddDuplicates()
        {
            var hashset = new HashSet<Profile>();

            var profile = Profiles.Default;
            var profileIdentical = Profiles.Default;
            var profileDifferentDate = Profiles.Default with { LastDateCommented = DateTime.Now.AddDays(1) };
            var profileDifferentIsFriend = Profiles.Default with { IsFriend = false };

            hashset.Add(profile);
            hashset.Add(profileIdentical);
            hashset.Add(profileDifferentDate);
            hashset.Add(profileDifferentIsFriend);

            Assert.Single(hashset);
        }
    }
}