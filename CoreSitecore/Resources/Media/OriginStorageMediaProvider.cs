using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Events.Hooks;
using Sitecore.IO;
using Sitecore.Resources;
using Sitecore.Resources.Media;
using Sitecore.Web;
using System;

namespace CoreSitecore.Resources.Media
{
    public class OriginStorageMediaProvider : MediaProvider, IHook 
    {
        public string Database { get; set; }   
        public string OriginPrefix { get; set; }
        public string SecureOriginPrefix { get; set; }

        public void Initialize() 
        { 
            MediaManager.Provider = this; 
        }     

        public override string MediaLinkPrefix 
        { 
            get 
            { 
                if (OriginPrefix != null && Context.Database != null && Context.Database.Name == Database) 
                {
                    if (System.Web.HttpContext.Current.Request.Url.Scheme == "https") return OriginPrefix.Replace("http://", "https://");
                    return OriginPrefix; 
                } 
                
                return Config.MediaLinkPrefix; 
            } 
        }

        public override string GetMediaUrl(MediaItem item, MediaUrlOptions options)
        {
            bool flag = options.Thumbnail || this.HasMediaContent(item);
            if (!flag && item.InnerItem["path"].Length > 0)
            {
                return item.InnerItem["path"];
            }
            if (options.UseDefaultIcon && !flag)
            {
                return Themes.MapTheme(Settings.DefaultIcon);
            }

            string text = this.MediaLinkPrefix;

            if (options.AbsolutePath)
            {
                text = options.VirtualFolder + text;
            }
            else
            {
                if (text.StartsWith("/", StringComparison.InvariantCulture))
                {
                    text = StringUtil.Mid(text, 1);
                }
            }

            //Validate OriginPrefix and the Database
            if (OriginPrefix != null && Context.Database != null && Context.Database.Name == Database)
            {
                //Validate the URL
                if (text.StartsWith("/http")) text = text.Substring(1);
            }

            if (options.AlwaysIncludeServerUrl)
            {
                text = FileUtil.MakePath(WebUtil.GetServerUrl(), text, '/');
            }
            string text2 = StringUtil.GetString(new string[]
			{
				options.RequestExtension,
				item.Extension,
				"ashx"
			});
            text2 = StringUtil.EnsurePrefix('.', text2);
            string text3 = options.ToString();
            if (text3.Length > 0)
            {
                text2 = text2 + "?" + text3;
            }
            string text4 = "/sitecore/media library/";
            string path = item.InnerItem.Paths.Path;
            string str;
            if (options.UseItemPath && path.StartsWith(text4, StringComparison.OrdinalIgnoreCase))
            {
                str = StringUtil.Mid(path, text4.Length);
            }
            else
            {
                str = item.ID.ToShortID().ToString();
            }
            return text + str + (options.IncludeExtension ? text2 : string.Empty);
        }
    }
}