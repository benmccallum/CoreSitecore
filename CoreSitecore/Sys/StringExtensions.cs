using System.Text.RegularExpressions;

public static class StringExtensions
{
    /// <summary>
    /// Convert the string to a valid sitecore item name
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToSitecoreValidItemName(this string value)
    {
        var invalidChars = "[^a-zA-Z0-9\\s]";
        if (Regex.IsMatch(value, invalidChars))
        {
            var newName = Regex.Replace(value, invalidChars, " ");
            return newName.Replace("  ", " ").Trim();
        }
        return value.Trim();
    }

    /// <summary>
    /// Convert any string to a SEO friendly URL
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToSitecoreSEOFriendlyURL(this string value)
    {
        var validName = value.ToSitecoreValidItemName();
        return validName.ToLower().Replace(" ", "-");
    }
}