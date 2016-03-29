using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;
using Sitecore.Web;
using System;

namespace CoreSitecore.Resources.Media
{
    /// <summary>
    /// Set's Jpeg compression in the media handler pipeline if the correct querystring is supplied.
    /// </summary>
    /// <remarks>
    /// Original source: https://laubplusco.net/make-sitecore-deliver-images-fits-screen/
    /// </remarks>
    public class SetJpegCompressionProcessor
    {
        public static string JpegCompressionLevelQueryKey
        {
            get { return Settings.GetSetting("CoreSitecore.Media.JpegCompressionLevelQueryKey", "jq"); }
        }

        public void Process(GetMediaStreamPipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            if (args.Options.Thumbnail || !IsJpegImageRequest(args.MediaData.MimeType))
            {
                return;
            }

            if (args.OutputStream == null || !args.OutputStream.AllowMemoryLoading)
            {
                return;
            }

            var jpegQualityQuery = WebUtil.GetQueryString(JpegCompressionLevelQueryKey);
            if (string.IsNullOrEmpty(jpegQualityQuery))
            {
                return;
            }

            int jpegQuality;
            if (!int.TryParse(jpegQualityQuery, out jpegQuality) || jpegQuality <= 0 || jpegQuality > 100)
            {
                return;
            }

            var compressedStream = ChangeJpegCompressionLevelService.Change(args.OutputStream, jpegQuality);
            args.OutputStream = new MediaStream(compressedStream, args.MediaData.Extension, args.OutputStream.MediaItem);
        }

        protected bool IsJpegImageRequest(string mimeType)
        {
            return mimeType.Equals("image/jpeg", StringComparison.InvariantCultureIgnoreCase)
                || mimeType.Equals("image/pjpeg", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
