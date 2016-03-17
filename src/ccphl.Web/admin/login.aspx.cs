using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using System.Security.Cryptography;
using System.Text;
using Model;
using ccphl.DBUtility;

namespace ccphl.Web.admin
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtUserName.Text = Utils.GetCookie("CCPHLRememberName");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text.Trim();
            string userPwd = txtPassword.Text.Trim();

            if (userName.Equals("") || userPwd.Equals(""))
            {
                msgtip.InnerHtml = "<b style=\"color:black;\">请输入用户名或密码</b>";
                return;
            }
            if (Session["AdminLoginSun"] == null)
            {
                Session["AdminLoginSun"] = 1;
            }
            else
            {
                Session["AdminLoginSun"] = Convert.ToInt32(Session["AdminLoginSun"]) + 1;
            }
            //判断登录错误次数
            if (Session["AdminLoginSun"] != null && Convert.ToInt32(Session["AdminLoginSun"]) > 5)
            {
                msgtip.InnerHtml = "<b style=\"color:red;\">错误超过5次，关闭浏览器重新登录！</b>";
                return;
            }
            MySqlUtility db = new MySqlUtility();
            system_user model = new system_user();
            model = db.GetModel<system_user>(" UserName='" + userName + "'");
            if (model.UserID==0)
            {
                msgtip.InnerHtml = "<b style=\"color:red;\">用户名或密码有误，请重试！</b>";
                return;
            }
            if (model.Status == 0)
            {
                msgtip.InnerHtml = "<b style=\"color:red;\">该用户已被禁用，请联系管理员！</b>";
                return;
            }
            if (DESEncrypt.Encrypt(txtPassword.Text.Trim(), model.Salt)!= model.Password)
            {
                msgtip.InnerHtml = "<b style=\"color:red;\">用户名或密码有误，请重试！</b>";
                return;
            }
            Session[DTKeys.SESSION_ADMIN_INFO] = model;
            Session.Timeout = 45;
            //写入登录日志
            system_log log = new system_log();
            log.ClientHost = DTRequest.GetIP();
            log.GUID = Guid.NewGuid().ToString();
            log.SiteID = model.SiteID;
            log.SiteNode = model.SiteNode;
            log.UserID = model.UserID;
            log.UserName = model.UserName;
            log.OperatedTime = DateTime.Now;
            log.OperateType = DTEnums.ActionEnum.Login.ToString();
            log.LogCotent = "用户登录";
            log.Insert();
            //写入Cookies
            Utils.WriteCookie("CCPHLRememberName", model.UserName, 14400);
            Utils.WriteCookie("AdminName", "ccphcms", model.UserName);
            Utils.WriteCookie("AdminPwd", "ccphcms", model.Password);
            Response.Redirect("index.aspx");
            return;
        }

    }
}