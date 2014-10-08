using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetPlaceholderRenderings;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CoreSitecore.Pipelines
{
    /// <summary>
    /// Gets the allowed renderings for a <see cref="CoreSitecore.Controls.DynamicKeyPlaceholder"/>.
    /// </summary>
    /// <remarks>
    /// Dynamic Key is formed by Key + UniqueID hence this pipeline removes the ID and finds placeholder settings for the original Key.
    /// </remarks>
    public class GetDynamicKeyPlaceholderAllowedRenderings : GetAllowedRenderings
    {
        /// <summary>
        /// Regex pattern to match the unique id {guid} appended to the standard placeholder key.
        /// </summary>
        public const string DYNAMIC_KEY_REGEX = @"(.+){[\d\w]{8}\-([\d\w]{4}\-){3}[\d\w]{12}}";

        public new void Process(GetPlaceholderRenderingsArgs args)
        {
            Assert.IsNotNull(args, "args");

            string placeholderKey = args.PlaceholderKey;
            Regex regex = new Regex(DYNAMIC_KEY_REGEX);
            Match match = regex.Match(placeholderKey);
            if (match.Success && match.Groups.Count > 0)
            {
                // Set with normalPlaceholderKey void of index on end
                placeholderKey = match.Groups[1].Value;
            }
            else
            {
                return;
            }

            // Same code as Sitecore.Pipelines.GetPlaceholderRenderings.GetAllowedRenderings but with fake placeholderKey
            Item placeholderItem = null;
            if (ID.IsNullOrEmpty(args.DeviceId))
            {
                placeholderItem = Client.Page.GetPlaceholderItem(placeholderKey, args.ContentDatabase, args.LayoutDefinition);
            }
            else
            {
                using (new DeviceSwitcher(args.DeviceId, args.ContentDatabase))
                {
                    placeholderItem = Client.Page.GetPlaceholderItem(placeholderKey, args.ContentDatabase, args.LayoutDefinition);
                }
            }

            List<Item> renderings = null;
            if (placeholderItem != null)
            {
                bool allowedControlsSpecified;
                args.HasPlaceholderSettings = true;
                renderings = this.GetRenderings(placeholderItem, out allowedControlsSpecified);
                if (allowedControlsSpecified)
                {
                    args.CustomData["allowedControlsSpecified"] = true;
                    args.Options.ShowTree = false;
                }
            }
            if (renderings != null)
            {
                if (args.PlaceholderRenderings == null)
                {
                    args.PlaceholderRenderings = new List<Item>();
                }
                args.PlaceholderRenderings.AddRange(renderings);
            }
        }
    }
}