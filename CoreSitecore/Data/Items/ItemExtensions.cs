using CoreSitecore.Caching;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A class for grouping all extensions methods on the <see cref="Sitecore.Data.Items.Item"/> class.
/// </summary>
public static class ItemExtensions
{
    /// <summary>
    /// Takes a sitecore item and formats it's Name property into a 
    /// </summary>
    /// <param name="item">Item to get url for.</param>
    /// <returns>some-item-name-turned-into-a-url</returns>
    public static string GetValidUrlFromItemName(this Item item)
    {
        return item.Name.Replace(" ", "-").ToLower();
    }

    /// <summary>
    /// Associate an image to a certain image field of the item
    /// </summary>
    /// <param name="item"></param>
    /// <param name="imageFieldID"></param>
    /// <param name="imageItemToAssociate"></param>
    /// <param name="endEditingWhenExit">If the calling method is putting the item in editing mode already, set this to false.</param>
    public static void AssociateWithImage(this Item item, ID imageFieldID, Item imageItemToAssociate, bool endEditingWhenExit)
    {
        Assert.ArgumentNotNull(item, "item");

        if (imageItemToAssociate != null)
        {
            ImageField fld = item.Fields[imageFieldID];
            if (fld != null)
            {
                MediaItem mediaItem = (MediaItem)imageItemToAssociate;

                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    if ((fld.MediaItem != null && fld.MediaID != mediaItem.ID) ||
                        (fld.MediaItem == null || fld.MediaID.IsNull))
                    {
                        if (!item.Editing.IsEditing)
                        {
                            item.Editing.BeginEdit();
                        }

                        fld.MediaID = mediaItem.ID;
                        if (!string.IsNullOrWhiteSpace(mediaItem.Alt))
                        {
                            fld.Alt = mediaItem.Alt;
                        }
                        else
                        {
                            fld.Alt = mediaItem.Name;
                        }

                        if (endEditingWhenExit)
                        {
                            item.Editing.EndEdit();
                            item.Editing.AcceptChanges();
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Publish an item to all publishing targets
    /// </summary>
    /// <param name="item"></param>
    /// <param name="mode">The publishing mode</param>
    /// <param name="publishSubItems">Specify whether to publish sub items</param>
    public static void Publish(this Item item, Sitecore.Publishing.PublishMode mode, bool publishSubItems)
    {
        Assert.ArgumentNotNull(item, "item");

        try
        {
            //get all publishing targets
            var publishingTargetsFolder = item.Database.GetItem("/sitecore/system/Publishing targets");

            if (publishingTargetsFolder != null)
            {
                var targetDBs = new List<Database>();
                var languages = item.Languages;
                var targets = publishingTargetsFolder.Children.Where(o => string.Compare(o.TemplateName, "Publishing target", true) == 0);

                //get all defined databases
                foreach (var t in targets)
                {
                    var db = Sitecore.Data.Database.GetDatabase(t["Target database"]);
                    if (db != null)
                    {
                        targetDBs.Add(db);
                    }
                }

                if (targetDBs.Any())
                {
                    foreach (var targetDB in targetDBs)
                    {
                        var publishOptions = new Sitecore.Publishing.PublishOptions(
                                                item.Database,
                                                targetDB,
                                                mode,
                                                item.Language,
                                                DateTime.Now);
                        var publisher = new Sitecore.Publishing.Publisher(publishOptions);


                        publisher.Options.RootItem = item;
                        publisher.Options.Deep = publishSubItems;
                        publisher.Publish();
                    }

                }

            }
        }
        catch (Exception ex)
        {
            Sitecore.Diagnostics.Log.Error("Sitecore - unable to auto-publish item", ex, typeof(ItemExtensions));
        }
    }

    /// <summary>
    /// Get image url form an image field of the item
    /// </summary>
    /// <param name="item"></param>
    /// <param name="imageFieldID"></param>
    /// <returns></returns>
    public static string GetImageUrlFromImageField(this Item item, ID imageFieldID)
    {
        if (item != null)
        {
            ImageField field = item.Fields[imageFieldID];

            if (field != null)
            {
                if (field.MediaItem != null)
                {
                    //Always return absolute path -> CoreSitecore.Hooks.CDN.OriginStorageMediaProvider
                    return Sitecore.Resources.Media.MediaManager.GetMediaUrl(field.MediaItem);
                }
            }
        }
        return string.Empty;
    }

    /// <summary>
    /// Convenience method to access the URL to the Sitecore item
    /// Note: can handle null item (returns null)
    /// </summary>
    /// <param name="item">Item to get the URL for</param>
    /// <param name="absolutePath">returns absolute path if set to True, Default to False</param>
    /// <returns>Item's URL, or null if passed item null</returns>
    public static string GetItemUrl(this Item item, bool absolutePath = false)
    {
        Assert.ArgumentNotNull(item, "item");
        if (absolutePath)
        {
            var options = new UrlOptions { AlwaysIncludeServerUrl = true };
            return LinkManager.GetItemUrl(item, options);
        }
        return LinkManager.GetItemUrl(item);
    }

    /// <summary>
    /// Gets the item url from a general link field on the Item.
    /// </summary>
    /// <param name="item">Item which field is on.</param>
    /// <param name="generalLinkFieldName">Field name of general link field to get url from.</param>
    /// <returns>Url string extracted from field.</returns>
    public static string GetItemUrl(this Item item, string generalLinkFieldName, UrlOptions urlOptions = null)
    {
        Assert.ArgumentNotNull(item, "item");
        Assert.ArgumentNotNullOrEmpty(generalLinkFieldName, "generalLinkFieldName");

        LinkField linkField = item.Fields[generalLinkFieldName];
        if (linkField == null)
        {
            throw new InvalidOperationException("Field " + generalLinkFieldName + " could not be found on the given item of template [name: " + item.TemplateName + ", and id: " + item.TemplateID + "]");
        }
        
        return (linkField.TargetItem != null)
            ? (urlOptions != null)
                ? linkField.TargetItem.GetItemUrl(urlOptions)
                : linkField.TargetItem.GetItemUrl()
            : null;
    }
    /// <summary>
    /// Convenience method to access the optional URL to the Sitecore item
    /// Note: can handle null item (returns null)
    /// </summary>
    /// <param name="item">Item to get the URL for</param>
    /// <param name="urlOptions">Set UrlOptions to get desired URL</param>
    /// <returns>Item's URL, or null if passed item null</returns>
    public static string GetItemUrl(this Item item, UrlOptions urlOptions)
    {
        Assert.ArgumentNotNull(item, "item");
        Assert.ArgumentNotNull(urlOptions, "urlOptions");

        return LinkManager.GetItemUrl(item, urlOptions);
    }
    /// <summary>
    /// Get the URL of the image in the named field.
    /// </summary>
    /// <param name="item">Sitecore item</param>
    /// <param name="imageFieldName">Name of image field</param>
    /// <returns>The URL of the MediaItem for the image in the named field with the required dimension parameters; null if the field not found, no media found, zero-size media, etc</returns>
    public static string GetImageUrl(this Item item, string imageFieldName)
    {
        return GetImageUrl(item, imageFieldName, null, null, null, null);
    }

    /// <summary>
    /// Get the URL of the image in the named field with specified width and height
    /// </summary>
    /// <param name="item">Sitecore item</param>
    /// <param name="imageFieldName">Name of image field</param>
    /// <param name="width">The width of the image that need to be returned; null to not specify</param>
    /// <param name="height">The height of the image that need to be returned; null to not specify</param>
    /// <returns>The URL of the MediaItem for the image in the named field with the required dimension parameters; null if the field not found, no media found, zero-size media, etc</returns>
    public static string GetImageUrl(this Item item, string imageFieldName, int? width, int? height)
    {
        return GetImageUrl(item, imageFieldName, width, height, null, null);
    }

    /// <summary>
    /// Get the URL of the image in the named field with max width and max height
    /// </summary>
    /// <param name="item">Sitecore item</param>
    /// <param name="imageFieldName">Name of image field</param>
    /// <param name="maxWidth">The maxWidth of the image that need to be returned; null to not specify</param>
    /// <param name="maxHeight">The maxHeight of the image that need to be returned; null to not specify</param>
    /// <returns>The URL of the MediaItem for the image in the named field with the required dimension parameters; null if the field not found, no media found, zero-size media, etc</returns>
    public static string GetImageUrlMax(this Item item, string imageFieldName, int? maxWidth, int? maxHeight)
    {
        return GetImageUrl(item, imageFieldName, null, null, maxWidth, maxHeight);
    }

    /// <summary>
    /// Get the URL of the image in the named field with any combination of width, height, max width and/or max height
    /// </summary>
    /// <param name="item">Sitecore item</param>
    /// <param name="imageFieldName">Name of image field</param>
    /// <param name="width">The width of the image that need to be returned; null to not specify</param>
    /// <param name="height">The height of the image that need to be returned; null to not specify</param>
    /// <param name="maxWidth">The maxWidth of the image that need to be returned; null to not specify</param>
    /// <param name="maxHeight">The maxHeight of the image that need to be returned; null to not specify</param>
    /// <returns>The URL of the MediaItem for the image in the named field with the required dimension parameters; null if the field not found, no media found, zero-size media, etc</returns>
    public static string GetImageUrl(this Item item, string imageFieldName, int? width, int? height, int? maxWidth, int? maxHeight)
    {
        Assert.ArgumentNotNull(item, "item");
        Assert.ArgumentNotNullOrEmpty(imageFieldName, "imageFieldName");

        ImageField imageField = item.Fields[imageFieldName];
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

    /// <summary>
    /// Get the referenced target item from the named reference field (Droplink, Droptree, Grouped Droplink). If the field not found or the reference null, it will return null.
    /// </summary>
    /// <returns>The the referenced target item from the named reference field. If the field not found or the reference null, it will return null</returns>
    public static Item GetReferenceTarget(this Item item, string referenceFieldName)
    {
        Assert.ArgumentNotNullOrEmpty(referenceFieldName, "referenceFieldName");

        ReferenceField referenceField = item.Fields[referenceFieldName];
        return (referenceField == null) ? null : referenceField.TargetItem;
    }

    /// <summary>
    /// Returns self or null if predicate is false.
    /// </summary>
    /// <param name="item">Item to test against predicate.</param>
    /// <param name="predicate">Predicate to test item with.</param>
    /// <returns>Item or null.</returns>
    public static Item SelfOrDefault(this Item item, Predicate<Item> predicate)
    {
        return item != null && predicate(item) ? item : null;
    }

    /// <summary>
    /// Gets first self or ancestor matching predicate or null if none found.
    /// </summary>
    public static Item GetSelfOrAncestorFirstOrDefault(this Item item, Func<Item, bool> predicate)
    {
        Item testItem = item;
        while (testItem != null)
        {
            if (predicate(testItem))
            {
                return testItem;
            }
            testItem = testItem.Parent;
        }
        return null;
    }

    /// <summary>
    /// Gets first self or ancestor matching predicate or null if none found.
    /// Implements hierarchical caching - if a parent matches the predicate all nodes from start item to that matching parent are cached.
    /// </summary>
    /// <param name="item">Start item to crawl upwards from.</param>
    /// <param name="cacheKey">Cache key to employ for the supporting dictionary stored in cache.</param>
    /// <param name="predicate">A predicate to look for matches against Items.</param>
    /// <returns>Matching item or null if none exists.</returns>
    public static Item GetSelfOrAncestorFirstOrDefaultHierarchicallyCached(this Item item, string cacheKey, Func<Item, bool> predicate)
    {
        var httpContext = System.Web.HttpContext.Current;


        // Init cache dictionary if required
        var cacheDictionary = httpContext.Cache[cacheKey] as Dictionary<string, Guid>;
        if (cacheDictionary == null)
        {
            httpContext.Cache[cacheKey] = new Dictionary<string, Guid>();
            cacheDictionary = (Dictionary<string, Guid>)httpContext.Cache[cacheKey];
        }

        Item matchedItem = null;        
        if (cacheDictionary.ContainsKey(item.Paths.FullPath))
        {
            // Already cached
            matchedItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(cacheDictionary[item.Paths.FullPath]));
        }       
        else
        {
            // Try for a self or ancestor hit
            matchedItem = item.GetSelfOrAncestorFirstOrDefault(predicate);

            // No match could be found, abort
            if (matchedItem == null)
            {
                return null;
            }

            // Cache for start item, all items up the hierarchical path and the final matched item. 
            var cacheItemPath = item.Paths.FullPath;
            while (cacheItemPath != matchedItem.Paths.FullPath)
            {
                cacheDictionary[cacheItemPath] = matchedItem.ID.Guid;
                cacheItemPath = cacheItemPath.Remove(cacheItemPath.LastIndexOf("/"));
            }
            cacheDictionary[cacheItemPath] = matchedItem.ID.Guid;
        }
        return matchedItem;
    }

    /// <summary>
    /// Gets first ancestor matching predicate or null if none found.
    /// </summary>
    public static Item GetAncestorFirstOrDefault(this Item item, Func<Item, bool> predicate)
    {
        Item testItem = item.Parent;
        while (testItem != null)
        {
            if (predicate(testItem))
            {
                return testItem;
            }
            testItem = testItem.Parent;
        }
        return null;
    }

    /// <summary>
    /// Gets first next sibling matching predicate or null if none found.
    /// </summary>
    public static Item GetNextSiblingFirstOrDefault(this Item item, Func<Item, bool> predicate)
    {
        Item testItem = item.Axes.GetNextSibling();
        while (testItem != null)
        {
            if (predicate(testItem))
            {
                return testItem;
            }
            testItem = testItem.Axes.GetNextSibling();
        }
        return null;
    }

    /// <summary>
    /// Gets first previous sibling matching predicate or null if none found.
    /// </summary>
    public static Item GetPreviousSiblingFirstOrDefault(this Item item, Func<Item, bool> predicate)
    {
        Item testItem = item.Axes.GetPreviousSibling();
        while (testItem != null)
        {
            if (predicate(testItem))
            {
                return testItem;
            }
            testItem = testItem.Axes.GetPreviousSibling();
        }
        return null;
    }

    /// <summary>
    /// Is this item under /sitecore/templates node in the content tree and therefore likely a template?
    /// </summary>
    /// <param name="item">Test item.</param>
    /// <returns></returns>
    public static bool IsTemplateItem(this Item item)
    {
        return item.Paths.IsTemplateItem();
    }

    /// <summary>
    /// Clears the item's cache on it's current database.
    /// </summary>
    /// <param name="item">Item to remove from all the caches.</param>
    public static void ClearCache(this Item item)
    {
        var db = item.Database;

        // Clear item's Item Cache
        db.Caches.ItemCache.RemoveItem(item.ID);

        // Clear item's Data Cache
        db.Caches.DataCache.RemoveItemInformation(item.ID);

        // Clear item's Standard Value Cache
        db.Caches.StandardValuesCache.RemoveKeysContaining(item.ID.ToString());

        // Clear item's Prefetch Cache
        var sqlPrefetchCache = CacheManagerEx.GetSqlPrefetchCache(db.Name);
        if (sqlPrefetchCache != null)
        {
            sqlPrefetchCache.Remove(item);
        }
    }
}