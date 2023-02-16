namespace MF.Common.Extensions;

public static class StringExtensions
{
    public static string Truncate(this string str, int length)
    {
        if(string.IsNullOrEmpty(str)) return "";

        if(str.Length <= length) return str;

        return $"{str.Substring(0, length)}...";
    }

    public static DateTime ConverToDateTime(this string str)
    {
        try
        {
            var parsedDate = DateTime.Parse(str);
            return parsedDate;
        }
        catch
        {
            return DateTime.Now;
        }

    }
}
