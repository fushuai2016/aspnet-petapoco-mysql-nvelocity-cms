using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;

namespace ccphl.Web.admin
{
    public partial class center : Web.UI.ManagePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                system_user admin_info = GetAdminInfo(); //管理员信息
                //登录信息
                if (admin_info != null)
                {
                    system_log model1 = db.GetModel<system_log>(" UserID=" + admin_info.UserID + " AND OperateType='" + DTEnums.ActionEnum.Login.ToString() + "' ORDER BY LogID DESC");
                    if (model1 != null)
                    {
                        //本次登录
                        litIP.Text = model1.ClientHost;
                    }
                    system_log model2 = db.GetModel<system_log>(" UserID=" + admin_info.UserID + " AND OperateType='" + DTEnums.ActionEnum.Login.ToString() + "' AND LogID!=" + model1.LogID + " ORDER BY LogID DESC  ");
                    if (model2 != null)
                    {
                        //上一次登录
                        litBackIP.Text = model2.ClientHost;
                        litBackTime.Text = model2.OperatedTime.ToString();
                    }
                }
                //LitUpgrade.Text = Utils.GetDomainStr(DTKeys.CACHE_OFFICIAL_UPGRADE, DESEncrypt.Decrypt(DTKeys.FILE_URL_UPGRADE_CODE, "hl"));
                // LitNotice.Text = Utils.GetDomainStr(DTKeys.CACHE_OFFICIAL_NOTICE, DESEncrypt.Decrypt(DTKeys.FILE_URL_NOTICE_CODE, "hl"));
                //Utils.GetDomainStr("dt_cache_domain_info", "http://www.dtcms.net/upgrade.ashx?u=" + Request.Url.DnsSafeHost + "&i=" + Request.ServerVariables["LOCAL_ADDR"]);
            }
        }
    }
}