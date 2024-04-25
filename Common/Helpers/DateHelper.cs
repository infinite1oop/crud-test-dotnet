namespace Common.Helpers
{
    public static class DateHelper
    {
        public static bool IsValidIso8601Date(string input)
        {
            return DateTime.TryParseExact(input, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _);
        }
    }
}
