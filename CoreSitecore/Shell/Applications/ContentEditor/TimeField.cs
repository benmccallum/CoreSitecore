using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using System;

namespace CoreSitecore.Shell.Applications.ContentEditor
{
    public class TimeField : Sitecore.Web.UI.HtmlControls.Input, Sitecore.Shell.Applications.ContentEditor.IContentField
    {
        private TimePicker _picker;

        public string ItemID
        {
            get { return base.GetViewStateString("ItemID"); }
            set
            {
                Assert.ArgumentNotNull(value, "value");
                base.SetViewStateString("ItemID", value);
            }
        }

        public string RealValue
        {
            get { return base.GetViewStateString("RealValue"); }
            set
            {
                Assert.ArgumentNotNull(value, "value");
                base.SetViewStateString("RealValue", value);
            }
        }

        public TimeField()
        {
            this.Class = "scContentControl";
            base.Change = "#";
            base.Activation = true;
        }

        public override void HandleMessage(Message message)
        {
            Assert.ArgumentNotNull(message, "message");
            base.HandleMessage(message);
            if (message["id"] != this.ID)
            {
                return;
            }
            string name;
            if ((name = message.Name) != null)
            {
                if (name == "contentdate:today")
                {
                    this.Now();
                    return;
                }
                if (name != "contentdate:clear")
                {
                    return;
                }
                this.ClearField();
            }
        }

        public string GetValue()
        {
            if (this._picker == null)
            {
                return this.RealValue;
            }
            return this._picker.Value;
        }

        public void SetValue(string value)
        {
            Assert.ArgumentNotNull(value, "value");
            this.RealValue = value;
            if (this._picker != null)
            {
                this._picker.Value = value;
            }
        }

        protected override Item GetItem()
        {
            return Client.ContentDatabase.GetItem(this.ItemID);
        }

        protected override bool LoadPostData(string value)
        {
            if (base.LoadPostData(value))
            {
                this._picker.Value = (value ?? string.Empty);
                return true;
            }
            return false;
        }

        protected override void OnInit(EventArgs e)
        {
            base.SetViewStateBool("Showtime", true);
            this._picker = new TimePicker();
            this._picker.ID = this.ID + "_picker";
            this.Controls.Add(this._picker);
            if (!string.IsNullOrEmpty(this.RealValue))
            {
                this._picker.Value = this.RealValue;
            }
            this._picker.OnChanged += delegate
            {
                this.SetModified();
            };
            this._picker.Disabled = this.Disabled;
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            base.ServerProperties["Value"] = base.ServerProperties["Value"];
            base.ServerProperties["RealValue"] = base.ServerProperties["RealValue"];
        }

        protected override void SetModified()
        {
            base.SetModified();
            if (base.TrackModified)
            {
                Sitecore.Context.ClientPage.Modified = true;
            }
        }

        private void ClearField()
        {
            this.SetRealValue(string.Empty);
        }

        private void SetRealValue(string realvalue)
        {
            if (realvalue != this.RealValue)
            {
                this.SetModified();
            }
            this.RealValue = realvalue;
            this._picker.Value = realvalue;
        }

        private void Now()
        {
            this.SetRealValue(DateUtil.IsoNowTime);
        }
    }
}