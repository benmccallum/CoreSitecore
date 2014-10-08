using System;

namespace CoreSitecore.Controls
{
    /// <summary>
    /// An extension to the default sitecore Link control.
    /// </summary>
    public class LinkExt : Sitecore.Web.UI.WebControls.Link
    {
        /// <summary>
        /// Start tag to render if the field being rendered has a value.
        /// </summary>
        public string StartTag { get; set; }

        /// <summary>
        /// End tag to render if the field being rendered has a value.
        /// </summary>
        public string EndTag { get; set; }

        /// <summary>
        /// Set this to true if you want to render StartTag and EndTag
        /// around the field's "value" even when the field value has no link.
        /// </summary>
        public bool RenderTagsWhenNoLink { get; set; }

        /// <summary>
        /// Set to true to render child controls even
        /// when the field value has no link.
        /// </summary>
        public bool RenderChildrenWhenNoLink { get; set; }

        private string _fieldValue;
        /// <summary>
        /// The Field value.
        /// </summary>
        /// <remarks>
        /// Stored in a backing variable for performance reasons.
        /// </remarks>
        private string fieldValue
        {
            get
            {
                if (_fieldValue == null)
                {
                    _fieldValue = this.GetFieldValue(Field);
                }
                return _fieldValue;
            }
        }

        /// <summary>
        /// Is there a link value set in this Link field?
        /// </summary>
        private bool hasLink
        {
            get
            {
                return !String.IsNullOrWhiteSpace(fieldValue);
            }
        }
        
        /// <summary>
        /// Override standard Text control render method.
        /// </summary>
        /// <param name="output"></param>
        protected override void Render(System.Web.UI.HtmlTextWriter output)
        {
            bool renderChildren = hasLink || RenderChildrenWhenNoLink;
            bool renderTags = hasLink || RenderTagsWhenNoLink;

            if (renderTags)
            {
                output.Write(StartTag);
            }
            if (hasLink)
            {
                base.Render(output);
            }
            else if (renderChildren)
            {
                base.RenderChildren(output);
            }
            if (renderTags)
            {
                output.Write(EndTag);
            }
        }
    }
}