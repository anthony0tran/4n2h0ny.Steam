﻿using _4n2h0ny.Steam.API.Helpers;

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
    }
}