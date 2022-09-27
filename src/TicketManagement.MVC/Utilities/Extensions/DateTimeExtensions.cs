namespace TicketManagement.MVC.Utilities.Extensions
{
    internal static class DateTimeExtensions
    {
        public static DateTime ToLocalByTimeZoneId(this DateTime dateTime, string? timeZoneId)
        {
            var result = dateTime;

            if (timeZoneId == null)
            {
                return result;
            }

            try
            {
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                result = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZoneInfo);
            }
            catch (TimeZoneNotFoundException)
            {
                return result;
            }
            catch (InvalidTimeZoneException)
            {
                return result;
            }

            return result;
        }
    }
}
