using System;

namespace CoreSitecore.Controls
{
    /// <summary>
    /// An extension to the default sitecore Text control.
    /// </summary>
    public class TextExt : Sitecore.Web.UI.WebControls.Text
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
        /// around the field's "value" even when the field value is whitespace.
        /// </summary>
        public bool RenderTagsWhenWhiteSpace { get; set; }

        /// <summary>
        /// Set to true if you want to render output for the control
        /// even when the field value is whitespace.
        /// </summary>
        public bool RenderWhenWhiteSpace { get; set; }

        private string fieldValue;
        /// <summary>
        /// The Field value.
        /// </summary>
        /// <remarks>
        /// Stored in a backing variable for performance reasons.
        /// </remarks>
        public string FieldValue
        {
            get
            {
                if (fieldValue == null)
                {
                    fieldValue = this.GetFieldValue(Field);
                }
                return fieldValue;
            }
        }
        
        /// <summary>
        /// Override standard Text control render method.
        /// </summary>
        /// <param name="output"></param>
        protected override void Render(System.Web.UI.HtmlTextWriter output)
        {
            bool render = RenderWhenWhiteSpace
                ? !String.IsNullOrEmpty(FieldValue)
                : !String.IsNullOrWhiteSpace(FieldValue);

            if (render)
            {
                bool renderTags = RenderTagsWhenWhiteSpace
                        ? !String.IsNullOrEmpty(FieldValue)
                        : !String.IsNullOrWhiteSpace(FieldValue);

                if (renderTags)
                {
                    output.Write(StartTag);
                }
                base.Render(output);
                if (renderTags)
                {
                    output.Write(EndTag);
                }
            }
        }
    }
}