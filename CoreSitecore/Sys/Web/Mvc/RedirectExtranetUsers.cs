using CoreSitecore.Helpers;
using System.Web.Mvc;

namespace CoreSitecore.Sys.Web.Mvc
{
    /// <summary>
    /// An action filter attribute that checks if a user is authenticated and authenticated under the extranet domain.
    /// If so, it will enforce a redirection to a configured url/item.
    /// </summary>
    public class RedirectExtranetUsers : ActionFilterAttribute
    {
        private const string DefaultRedirectUrlSettingName = "CoreSitecore.ActionFilters.DefaultRedirectExtranetUrl";

        /// <summary>
        /// Extranet users (i.e. IsAuthenticated and under extranet domain) will be redirected to a url found by inspecting this setting.
        ///
        /// If this setting is an Sitecore.Data.ID, the url to that item will be used.
        /// Else, it will be assumed to be a URL that can be redirected to as is.
        ///
        /// Default: "CoreSitecore.ActionFilters.DefaultRedirectExtranetUrl"
        /// </summary>
        public string RedirectUrlSettingName { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Sitecore.Context.User.IsAuthenticated && Sitecore.Context.User.GetDomainName() == "extranet")
            {
                var redirectionUrl = RedirectHelper.GetRedirectUrlFromSetting(RedirectUrlSettingName, DefaultRedirectUrlSettingName);

                if (!string.IsNullOrWhiteSpace(redirectionUrl))
                {
                    filterContext.Result = new RedirectResult(redirectionUrl);
                    return;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
