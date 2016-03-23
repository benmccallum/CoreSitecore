namespace CoreSitecore.Sys.Web.Mvc
{
    /// <summary>
    /// An Authorize attribute implementation that checks the current Sitecore.Context.User is not null, authenticated and belongs to extranet domain.
    /// </summary>
    public class AuthorizeExtranetAttribute : AuthorizeByDomainAttribute
    {
        public AuthorizeExtranetAttribute()
            : base(new [] { "extranet" })
        {

        }
    }
}
