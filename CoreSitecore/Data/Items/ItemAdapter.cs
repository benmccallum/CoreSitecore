using Sitecore.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreSitecore.Data.Items
{
    /// <summary>
    /// An adapter for the standard Sitecore item that exposes the same
    /// members using the virtual keyword, so we can moq them.
    /// </summary>
    public class ItemAdapter
    {
        /// <summary>
        /// Store a reference to the original Item internally.
        /// </summary>
        private Sitecore.Data.Items.Item item;

        /// <summary>
        /// Empty constructor.
        /// DO NOT USE! ONLY HERE FOR MOQ.
        /// </summary>
        public ItemAdapter() { }

        /// <summary>
        /// Initializes a new instance of the ItemAdapter class with the given Sitecore Item.
        /// </summary>
        /// <param name="item"></param>
        public ItemAdapter(Sitecore.Data.Items.Item item)
        {
            this.item = item;
        }

        public virtual Sitecore.Data.Items.Item InternalItem
        {
            get { return item; }
        }

        public virtual Sitecore.Data.ID ID
        {
            get { return item.ID;  }
        }

        public virtual string Name
        {
            get { return item.Name; }
        }

        public virtual string this[int index]
        {
            get { return item[index]; }
        }

        public virtual string this[string fieldName]
        {
            get { return item[fieldName]; }
        }

        public virtual ChildList Children
        {
            get { return item.Children; }
        }

        public virtual Sitecore.Collections.FieldCollection Fields
        {
            get { return item.Fields; }
        }

        public virtual Sitecore.Data.Items.Item Parent
        {
            get { return item.Parent; }
        }

        public virtual Sitecore.Data.ID TemplateID
        {
            get { return item.TemplateID; }
        }
    }
}