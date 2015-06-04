using Sitecore.Links;

public static class UrlOptionsExtensions
{
    public static UrlOptions ButAbsolute(this UrlOptions urlOptions)
    {
        urlOptions.AlwaysIncludeServerUrl = true;
        return urlOptions;
    }
}
