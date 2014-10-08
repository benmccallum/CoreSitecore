namespace CoreSitecore.Controls
{
    /// <summary>
    /// Overrides the default sitecore placeholder by rendering a wrapping div around its contents in Page Editor mode.
    /// </summary>
    /// <remarks>
    /// The PageEditor doesn't seem to like placeholders that don't have a parent div element.
    /// Havoc ensues and you cannot delete renderings/sublayouts from placeholders in edit mode.
    /// </remarks>
    public class WrappedPlaceholder : Sitecore.Web.UI.WebControls.Placeholder
    {
        protected override void Render(System.Web.UI.HtmlTextWriter output)
        {
            if (Sitecore.Context.PageMode.IsPageEditor)
            {
                output.Write("<div class=\"sc-placeholder\">");
            }
            base.Render(output);
            if (Sitecore.Context.PageMode.IsPageEditor)
            {
                output.Write("</div>");
            }            
        }
    }
}