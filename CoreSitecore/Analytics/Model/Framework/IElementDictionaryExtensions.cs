using Sitecore.Analytics.Model.Framework;

public static class IElementDictionaryExtensions
{
    public static TElement GetSafely<TElement>(this IElementDictionary<TElement> dict, string key)
        where TElement : class, Sitecore.Analytics.Model.Framework.IElement
    {
        return dict.Contains(key) ? dict[key] : null;
    }

}
