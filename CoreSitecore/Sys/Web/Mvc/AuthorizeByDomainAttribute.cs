using Sitecore.Configuration;
using Sitecore.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoreSitecore.Sys.Web.Mvc
{
    /// <summary>
    /// An AuthorizeByDomain attribute implementation that checks the current Sitecore.Context.User is not null, authenticated and belongs to extranet domain.
    /// </summary>
    public class AuthorizeByDomainAttribute : AuthorizeAttribute
    {
        private const string DefaultRedirectUrlSettingName = "CoreSitecore.Authorization.DefaultUnauthorizedRedirectUrl";

        private readonly string[] _domainNames;

        /// <summary>
        /// If specified, on an unauthorized request, user will be redirected to a url found by inspecting this setting.
        ///
        /// If this setting is an Sitecore.Data.ID, the url to that item will be used.
        /// Else, it will be assumed to be a URL that can be redirected to as is.
        ///
        /// Default: CoreSitecore.AuthorizeByDomainAttribute.DefaultRedirectUrl
        /// </summary>
        public string RedirectUrlSettingName { get; set; }

        /// <summary>
        /// If true, will append returnUrl querystring to url of redirected to page to support kickback later.
        /// Default: true
        /// </summary>
        public bool AppendReturnUrlQueryString { get; set; }

        /// <summary>
        /// The querystring key/name to use if appending a return url.
        /// Default: returnUrl
        /// </summary>
        public string ReturnUrlQueryStringKey { get; set; }

        public AuthorizeByDomainAttribute(string[] domainNames)
        {
            _domainNames = domainNames;

            // Defaults
            AppendReturnUrlQueryString = true;
            ReturnUrlQueryStringKey = "returnUrl";
        }

        /// <summary>
        /// Overrides the default Authorize logic, checking that a user
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = Sitecore.Context.User;
            return user != null && user.IsAuthenticated && _domainNames.Contains(user.GetDomainName());
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var redirectionUrl = string.Empty;

            var settingName = string.IsNullOrWhiteSpace(RedirectUrlSettingName) ? DefaultRedirectUrlSettingName : RedirectUrlSettingName;
            var settingValue = Settings.GetSetting(settingName);

            if (!string.IsNullOrWhiteSpace(settingValue))
            {
                // If ID, lookup item by ID and figure out it's URL
                if (ID.IsID(settingValue))
                {
                    var itemToRedirectTo = Sitecore.Context.Database.GetItem(new ID(settingValue));
                    if (itemToRedirectTo != null)
                    {
                        redirectionUrl = itemToRedirectTo.GetItemUrl();
                    }
                }
                else
                {
                    // Assume is url and just use that
                    redirectionUrl = settingValue;
                }
            }

            if (!string.IsNullOrWhiteSpace(redirectionUrl))
            {
                if (AppendReturnUrlQueryString)
                {
                    var currentUrl = filterContext.RequestContext.HttpContext.Request.Url.PathAndQuery;
                    var currentUrlEncoded = HttpUtility.UrlEncode(currentUrl);
                    redirectionUrl = CoreSystem.Helpers.UrlHelper.AppendQueryString(redirectionUrl, ReturnUrlQueryStringKey, currentUrlEncoded);
                }

                filterContext.Result = new RedirectResult(redirectionUrl);
            }
            else
            {
                // Note: This seems to just stop the view from rendering, not actually do a redirect to loginUrl in Sitecore's implementation.
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}
