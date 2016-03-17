using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;

namespace ccphl.Web.admin.ccphl
{
    public partial class browser_log_img : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "为民服务站用户浏览器信息";
        //列表页面名称
        public const string ListName = "browser_log_img.aspx";


        protected void Page_Load(object sender, EventArgs e)
        {
            string viewtype = Utils.GetCookie("browser_log_view");
            if (viewtype == "Txt")
            {
                Response.Redirect(Utils.CombUrlTxt("browser_log.aspx", "mid={0}", ModuleID.ToString()));
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.View); //检查权限
            }
        }

        //设置文字列表显示
        protected void lbtnViewTxt_Click(object sender, EventArgs e)
        {
            Utils.WriteCookie("browser_log_view", "Txt", 14400);
            Response.Redirect(Utils.CombUrlTxt("browser_log.aspx", "mid={0}&page=1",
                 ModuleID.ToString()));
        }
    }
}