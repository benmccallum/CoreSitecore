using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetChromeData;
using Sitecore.Web.UI.PageModes;
using System;
using System.Text.RegularExpressions;

namespace CoreSitecore.Pipelines
{
    /// <summary>
    /// Gets / reformats the ChromeData shown in the Page Editor when a display name and expanded display name for a dynamic placeholder.
    /// </summary>
    public class GetDynamicKeyPlaceholderChromeData : GetPlaceholderChromeData
    {
        public override void Process(GetChromeDataArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.IsNotNull(args.ChromeData, "Chrome Data");
            if (!"placeholder".Equals(args.ChromeType, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // Get the placeholder key values
            string dynamicKey = args.CustomData["placeHolderKey"] as string;
            Regex regex = new Regex(GetDynamicKeyPlaceholderAllowedRenderings.DYNAMIC_KEY_REGEX);
            Match match = regex.Match(dynamicKey);
            if (!match.Success || match.Groups.Count <= 0)
            {
                return;
            }
            string normalKey = match.Groups[1].Value;

            // Format up some Chrome
            Item item = null;
            if (args.Item != null)
            {
                string layout = ChromeContext.GetLayout(args.Item);
                item = Sitecore.Client.Page.GetPlaceholderItem(normalKey, args.Item.Database, layout);
                if (item != null)
                {
                    args.ChromeData.DisplayName = item.DisplayName;
                    if (!String.IsNullOrEmpty(item.Appearance.ShortDescription))
                    {
                        args.ChromeData.ExpandedDisplayName = item.Appearance.ShortDescription;
                    }
                }                
            }
        }
    }
}