using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;
using System.Collections.Specialized;

namespace SitecoreCore.Web.UI.WebControls
{
    /// <summary>
    /// Base class to be inherited from by all Sitecore sublayout ascx user controls.
    /// </summary>
    public abstract class SublayoutBase : System.Web.UI.UserControl
    {
        /// <summary>
        /// Gets a reference to the System.Web.UI.Page that controls the server control as a LayoutBase type.
        /// </summary>
        /// <remarks>
        /// Hide the default behaviour which returns a System.Web.UI.Page object.
        /// Instead, return the Page as our LayoutBase type.
        /// </remarks>
        public new LayoutBase Page
        {
            get { return base.Page as LayoutBase; }
            set { base.Page = value; }
        }

        /// <summary>
        /// Gets a reference to the Sitecore Sublayout.
        /// </summary>
        protected Sublayout Sublayout
        {
            get
            {
                return Parent as Sublayout;
            }
        }

        private Item dataSource;
        /// <summary>
        /// Gets the datasource for the current Sitecore Item.
        /// </summary>
        protected virtual Item DataSource
        {
            get
            {
                if (dataSource == null && Sublayout != null)
                {
                    dataSource = Sitecore.Context.Database.GetItem(Sublayout.DataSource);
                }
                return dataSource;
            }
        }

        private NameValueCollection parameters;
        /// <summary>
        /// Gets the Sitecore Sublayout Parameters entered into the CMS for this sublayout item as a nice collection.
        /// </summary>
        protected NameValueCollection Parameters
        {
            get
            {
                if (parameters == null)
                {
                    parameters = Sitecore.Web.WebUtil.ParseUrlParameters(Sublayout.Parameters);
                }
                return parameters;
            }
        }
    }
}