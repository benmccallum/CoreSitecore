using Sitecore.Configuration;
using Sitecore.Data;

namespace CoreSitecore.Helpers
{
    public static class RedirectHelper
    {
        /// <summary>
        /// Gets a redirect url from a Sitecore setting,
        /// whereby the setting value can be the ID of a Sitecore item to redirect to or a url to redirect to.
        /// </summary>
        /// <returns></returns>
        public static string GetRedirectUrlFromSetting(string redirectUrlSettingName, string defaultRedirectUrlSettingName)
        {
            var redirectionUrl = string.Empty;

            var settingName = string.IsNullOrWhiteSpace(redirectUrlSettingName) ? defaultRedirectUrlSettingName : redirectUrlSettingName;
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

            return redirectionUrl;
        }
    }
}
