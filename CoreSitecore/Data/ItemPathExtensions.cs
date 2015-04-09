using Sitecore.Data;
using System;

/// <summary>
/// Extension methods on the <seealso cref="Sitecore.Data.ItemPath"/> type.
/// </summary>
public static class ItemPathExtensions
{
    /// <summary>
    /// Is this path indicating a Template (i.e. under /sitecore/templates)?
    /// </summary>
    /// <param name="itemPath">ItemPath</param>
    /// <returns>True/False.</returns>
    public static bool IsTemplateItem(this ItemPath itemPath)
    {
        return itemPath.Path.StartsWith("/sitecore/templates/", StringComparison.OrdinalIgnoreCase);
    }
}
