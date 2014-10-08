using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;
using System;

public static class FieldExtensions
{
    /// <summary>
    /// Returns a boolean value indicating whether or not the field has a value 
    /// and is non null and non-whitespace/blank.
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public static bool HasNonBlankValue(this Sitecore.Data.Fields.Field field)
    {
        Assert.ArgumentNotNull(field, "field");

        return field.HasValue && !String.IsNullOrWhiteSpace(field.Value);
    }


    /// <summary>
    /// Returns the URL from a Link Field
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public static string GetLinkFieldUrl(this Sitecore.Data.Fields.LinkField field)
    {
        switch (field.LinkType)
        {
            case "internal":
                // Use LinkMananger for internal links, if link is not empty
                return field.TargetItem != null ? Sitecore.Links.LinkManager.GetItemUrl(field.TargetItem) : string.Empty;
            case "media":
                // Use MediaManager for media links, if link is not empty
                return field.TargetItem != null ? Sitecore.StringUtil.EnsurePrefix('/', Sitecore.Resources.Media.MediaManager.GetMediaUrl(field.TargetItem)) : string.Empty;
            case "external":
                // Just return external links
                return field.Url;
            case "anchor":
                // Prefix anchor link with # if link if not empty
                return !string.IsNullOrEmpty(field.Anchor) ? "#" + field.Anchor : string.Empty;
            case "mailto":
                // Just return mailto link
                return field.Url;
            case "javascript":
                // Just return javascript
                return field.Url;
            default:
                // Just please the compiler, this
                // condition will never be met
                return field.Url;
        }
    }

    /// <summary>
    /// Format the LinkField
    /// </summary>
    /// <param name="field"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string FormatLinkField(this Sitecore.Data.Fields.LinkField field, string content, string cssClass) 
    {
        string target = String.Empty;
        string altText = String.Empty;
        string url = GetLinkFieldUrl(field);

        if (!String.IsNullOrEmpty(field.Target)) {
            target = " target=\"" + field.Target + "\"";
        }

        if (!String.IsNullOrEmpty(field.Text)) {
            altText = " alt=\"" + field.Text + "\"";
        }
        
        return String.Format("<a class=\"{4}\" href=\"{0}\"{1}{2}>{3}</a>", url, target, altText, content, cssClass);
    }

    /// <summary>
    /// Check if field is valid
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public static bool IsValid(this Sitecore.Data.Fields.Field field)
    {
        return (field != null && !string.IsNullOrWhiteSpace(field.Name));
    }
}