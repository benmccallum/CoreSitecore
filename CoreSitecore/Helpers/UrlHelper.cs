using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreSitecore.Helpers
{
    public class UrlHelper
    {
        /// <summary>
        /// Gets default url options object with AlwaysIncludeServerUrl set to true - meaning absolute paths!
        /// </summary>
        /// <returns></returns>
        public static UrlOptions GetAbsoluteUrlOptions()
        {
            var urlOptions = LinkManager.GetDefaultUrlOptions();
            urlOptions.AlwaysIncludeServerUrl = true;
            return urlOptions;
        }

        /// <summary>
        /// Get SEO Friendly name
        /// </summary>
        /// <param name="title"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string ToSeoFriendly(string title, int maxLength)
        {
            var match = Regex.Match(title.ToLower(), "[\\w]+");
            StringBuilder result = new StringBuilder("");
            bool maxLengthHit = false;
            while (match.Success && !maxLengthHit)
            {
                if (result.Length + match.Value.Length <= maxLength)
                {
                    result.Append(match.Value + "-");
                }
                else
                {
                    maxLengthHit = true;
                    // Handle a situation where there is only one word and it is greater than the max length.
                    if (result.Length == 0) result.Append(match.Value.Substring(0, maxLength));
                }
                match = match.NextMatch();
            }
            // Remove trailing '-'
            if (result[result.Length - 1] == '-') result.Remove(result.Length - 1, 1);
            return result.ToString();
        }
    }
}
