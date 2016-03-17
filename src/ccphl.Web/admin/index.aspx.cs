﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;

namespace ccphl.Web.admin
{
    public partial class index : Web.UI.ManagePage
    {
        protected system_user admin_info;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.sid.Value = siteConfig.SiteID.ToString();
            if (!Page.IsPostBack)
            {
                admin_info = GetAdminInfo();
            }
        }

        //安全退出
        protected void lbtnExit_Click(object sender, EventArgs e)
        {
            Session[DTKeys.SESSION_ADMIN_INFO] = null;
            Utils.WriteCookie("AdminName", "ccphlcms", -14400);
            Utils.WriteCookie("AdminPwd", "ccphlcms", -14400);
            Response.Redirect("login.aspx");
        }

    }
}