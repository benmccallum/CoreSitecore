

namespace CoreSitecore.Caching
{
    /// <summary>
    /// Helper methods around the Sitecore CacheManager.
    /// </summary>
    public static class CacheManagerEx
    {
        public static Sitecore.Caching.Cache GetSqlPrefetchCache(string databaseName)
        {
            return Sitecore.Caching.CacheManager.FindCacheByName("SqlDataProvider - Prefetch data(" + databaseName + ")");
        }
    }
}
