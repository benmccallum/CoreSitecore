using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;

namespace CoreSitecore.Helpers
{
    public class MultiListFieldHelper
    {
        /// <summary>
        /// Get Sitecore Items from the pipe-separated list of GUID IDs in the fieldValue.
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        public static IEnumerable<Item> GetItems(string fieldValue)
        {
            Assert.ArgumentNotNull(fieldValue, "fieldValue");

            foreach (var guid in fieldValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var item = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(guid));
                if (item != null)
                {
                    yield return item;
                }
            }
        }
    }
}
