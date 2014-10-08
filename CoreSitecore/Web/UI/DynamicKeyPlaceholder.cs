using Sitecore.Common;
using Sitecore.Layouts;
using Sitecore.Web.UI;
using Sitecore.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace CoreSitecore.Web.UI
{
    public class DynamicKeyPlaceholder : Sitecore.Web.UI.WebControl, IExpandable
    {
        protected string key = Placeholder.DefaultPlaceholderKey;
        protected string dynamicKey = null;
        protected Placeholder placeholder;

        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value.ToLower();
            }
        }

        /// <summary>
        /// A dynamic placeholder key if multiple placeholders with the same Key
        /// are in the same container.
        /// </summary>
        protected string DynamicKey
        {
            get
            {
                if (dynamicKey == null)
                {
                    dynamicKey = key;

                    // Find the last placeholder processed, will help us find our parent
                    Stack<Placeholder> stack = Switcher<Placeholder, PlaceholderSwitcher>.GetStack(false);
                    if (stack == null || !stack.Any())
                    {
                        return dynamicKey;
                    }
                    
                    Placeholder current = stack.Peek();

                    // Find itself and other siblings in the same placeholder/container
                    var currentContextKey = StandardiseKey(current.ContextKey);
                    var currentKey = StandardiseKey(current.Key);
                    var thisRenderingAndSiblingRenderings =
                        from rendering in Sitecore.Context.Page.Renderings
                        let standardisedKey = StandardiseKey(rendering.Placeholder)
                        where (standardisedKey == currentContextKey || standardisedKey == currentKey)
                            && rendering.AddedToPage
                        select rendering;

                    if (thisRenderingAndSiblingRenderings.Any())
                    {
                        // This rendering is the last one to be added as it was just processed (hence we are here)
                        var thisRendering = thisRenderingAndSiblingRenderings.Last();
                        dynamicKey = key + thisRendering.UniqueId;
                    }
                }
                return dynamicKey;
            }
        }

        protected override void CreateChildControls()
        {
            placeholder = new Placeholder() { Key = this.DynamicKey };
            this.Controls.Add(placeholder);
            placeholder.Expand();
        }

        protected override void DoRender(HtmlTextWriter output)
        {
            base.RenderChildren(output);
        }

        /// <summary>
        /// Normalises key values so that page editor added controls
        /// and manually added controls conform to the same case and format.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string StandardiseKey(string key)
        {
            return key.TrimStart('/').ToLower();
        }

        #region IExpandable Members

        public void Expand()
        {
            this.EnsureChildControls();
        }

        #endregion
    }
}