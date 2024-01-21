using System;

namespace _4n2h0ny.Steam.API.Helpers
{
    public static class DateParser
    {
        public static DateTime ParseUnixTimeStampToDateTime(string dateString)
        {
            if (!double.TryParse(dateString, out var secondsAfterEpoch))
            {
                return DateTime.MinValue;
            }

            var epochTimeStamp = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return epochTimeStamp.AddSeconds(secondsAfterEpoch);
        }
    }
}
