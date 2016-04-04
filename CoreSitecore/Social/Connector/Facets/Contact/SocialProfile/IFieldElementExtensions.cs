using Sitecore.Social.Connector.Facets.Contact.SocialProfile;

public static class IFieldElementExtensions
{
    public static string GetValueOrDefault(this IFieldElement fieldEle)
    {
        return fieldEle == null ? null : fieldEle.Value;
    }
}
