using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;

namespace ccphl.Web.admin.dialog
{
    public partial class dialog_attach : Web.UI.ManagePage
    {
        public int channelid;
        public int siteid;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.channelid = DTRequest.GetQueryInt("channelid");
            this.siteid = DTRequest.GetQueryInt("siteid");
        }
    }
}