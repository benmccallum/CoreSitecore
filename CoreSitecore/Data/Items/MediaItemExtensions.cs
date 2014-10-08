using Sitecore.Data.Items;
using Sitecore.Resources.Media;

/// <summary>
/// Class for grouping all extension methods on the <see cref="Sitecore.Data.Items.MediaItem"/> class.
/// </summary>
public static class MediaItemExtensions
{
    /// <summary>
    /// Get the url of the MediaItem
    /// </summary>
    /// <returns>The url of the MediaItem with the default dimensions; null if no media found</returns>
    public static string GetMediaUrl(this MediaItem item)
    {
        return GetMediaUrl(item, null, null, null, null);
    }

    /// <summary>
    /// Get the url of the MediaItem with specified width and height
    /// </summary>
    /// <param name="width">The width of the image that need to be returned; null to not specify</param>
    /// <param name="height">The height of the image that need to be returned; null to not specify</param>
    /// <returns>The url of the MediaItem with the required dimension parameters; null if no media found</returns>
    public static string GetMediaUrl(this MediaItem item, int? width, int? height)
    {
        return GetMediaUrl(item, width, height, null, null);
    }

    /// <summary>
    /// Get the url of the MediaItem with max width and max height
    /// </summary>
    /// <param name="maxWidth">The maxWidth of the image that need to be returned; null to not specify</param>
    /// <param name="maxHeight">The maxHeight of the image that need to be returned; null to not specify</param>
    /// <returns>The url of the MediaItem with the required dimension parameters; null if no media found</returns>
    public static string GetMediaUrlMax(this MediaItem item, int? maxWidth, int? maxHeight)
    {
        return GetMediaUrl(item, null, null, maxWidth, maxHeight);
    }

    /// <summary>
    /// Get the url of the MediaItem with any combination of width, height, max width and/or max height
    /// </summary>
    /// <param name="width">The width of the image that need to be returned; null to not specify</param>
    /// <param name="height">The height of the image that need to be returned; null to not specify</param>
    /// <param name="maxWidth">The maxWidth of the image that need to be returned; null to not specify</param>
    /// <param name="maxHeight">The maxHeight of the image that need to be returned; null to not specify</param>
    /// <returns>The url of the MediaItem with the required dimension parameters; null if no media found</returns>
    public static string GetMediaUrl(this MediaItem item, int? width, int? height, int? maxWidth, int? maxHeight)
    {
        if (item == null || item.Size == 0)
        {
            return null;
        }

        MediaUrlOptions opt = new MediaUrlOptions();
        opt.UseItemPath = true;

        if (width.HasValue && width.Value > 0)
        {
            opt.Width = width.Value;
        }

        if (height.HasValue && height.Value > 0)
        {
            opt.Height = height.Value;
        }

        if (maxWidth.HasValue && maxWidth.Value > 0)
        {
            opt.MaxWidth = maxWidth.Value;
        }

        if (maxHeight.HasValue && maxHeight.Value > 0)
        {
            opt.MaxHeight = maxHeight.Value;
        }

        if (Sitecore.Configuration.Settings.Media.AlwaysIncludeServerUrl == false) 
        {
            var returnURL = Sitecore.Resources.Media.MediaManager.GetMediaUrl(item, opt);
            return returnURL.StartsWith("/") ? returnURL.Substring(1) : returnURL;
        }

        return Sitecore.StringUtil.EnsurePrefix('/', Sitecore.Resources.Media.MediaManager.GetMediaUrl(item, opt));
    }
}