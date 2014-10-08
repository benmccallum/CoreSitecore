using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

public static class ImageFieldExtensions
{
    /// <summary>
    /// Get the URL of the image in the named field with any combination of max width and/or max height
    /// </summary>
    public static string GetImageUrlMax(this ImageField imageField, int? maxWidth, int? maxHeight)
    {
        return GetImageUrl(imageField, null, null, maxWidth, maxHeight);
    }

    /// <summary>
    /// Get the URL of the image in the named field with any combination of width, height, max width and/or max height
    /// </summary>
    public static string GetImageUrl(this ImageField imageField, int? width, int? height, int? maxWidth, int? maxHeight)
    {
        if (imageField != null && imageField.MediaItem != null)
        {
            string mediaUrl = ((MediaItem)imageField.MediaItem).GetMediaUrl(width, height, maxWidth, maxHeight);
            if (mediaUrl != null)
            {
                return mediaUrl.Replace(" ", "%20");
            }
        }
        return null;
    }
}