using System;
using System.Web;
using System.Web.Mvc;

namespace CoreSitecore.Sys.Web.Mvc
{
    /// <summary>
    /// An AuthorizeByDomain attribute implementation that checks the current Sitecore.Context.User is not null, authenticated and belongs to extranet domain.
    /// </summary>
    public class AuthorizeByDomainAttribute : AuthorizeAttribute
    {
        private readonly string _domainName;

        /// <summary>
        /// If specified, on an unauthorized request, user will be redirected to this url.
        /// </summary>
        public string RedirectToUrl { get; set; }

        public AuthorizeByDomainAttribute(string domainName)
        {
            _domainName = domainName;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = Sitecore.Context.User;
            return user != null && user.IsAuthenticated && user.GetDomainName().Equals(_domainName, StringComparison.OrdinalIgnoreCase);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!string.IsNullOrWhiteSpace(RedirectToUrl))
            {
                filterContext.Result = new RedirectResult(RedirectToUrl);
            }

            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}
