public static class IDExtensions
{
    /// <summary>
    /// Returns the id sanitized in the format "uid-{ShortID}" for use in html as a uid.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string Sanitize(this Sitecore.Data.ID id)
    {
        return "uid-" + id.ToShortID();
    }
}