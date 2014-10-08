using System.Drawing;

namespace CoreSitecore.Controls
{
    /// <summary>
    /// A control for rendering an img tag, just like the sc:image control,
    /// but from a Sitecore MediaItem.
    /// </summary>
    public class MediaItemImage : System.Web.UI.WebControls.Image
    {
        /// <summary>
        /// Sitecore MediaItem.
        /// </summary>
        public Sitecore.Data.Items.MediaItem MediaItem { get; set; }

        public int? SCWidth { get; set; }
        public int? SCHeight { get; set; }
        public int? SCMaxWidth { get; set; }
        public int? SCMaxHeight { get; set; }
        public bool? SCAllowStretch { get; set; }
        public System.Drawing.Color SCBackgroundColor { get; set; }
        // TODO: Add more as required

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {            
            if (MediaItem != null)
            {
                // Inspect MediaItem to get details for image
                // TODO: Maybe store this in viewstate?

                // Pass through sc image handler options
                var mediaUrlOptions = new Sitecore.Resources.Media.MediaUrlOptions();
                if (SCWidth.HasValue) { mediaUrlOptions.Width = SCWidth.Value; }
                if (SCHeight.HasValue) { mediaUrlOptions.Height = SCHeight.Value; }
                if (SCMaxWidth.HasValue) { mediaUrlOptions.MaxWidth = SCMaxWidth.Value; }
                if (SCMaxHeight.HasValue) { mediaUrlOptions.MaxHeight = SCMaxHeight.Value; }
                if (SCBackgroundColor != null) { mediaUrlOptions.BackgroundColor = SCBackgroundColor; }
                if (SCAllowStretch.HasValue) { mediaUrlOptions.AllowStretch = SCAllowStretch.Value; }

                // Generate url to image from options
                this.ImageUrl = Sitecore.Resources.Media.MediaManager.GetMediaUrl(MediaItem, mediaUrlOptions);

                // Set alt text
                this.AlternateText = MediaItem.Alt;

                base.Render(writer);
            }
        }
    }
}
