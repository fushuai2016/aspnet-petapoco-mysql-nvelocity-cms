using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using Model;
using ccphl.DBUtility;
using ccphl.Common;
using ccphl.NVelocity;

namespace ccphl.Web.UI
{
    /// <summary>
    ///DefaultHandler 的摘要说明
    /// </summary>
    public class DefaultHandler :ManagePage, System.Web.SessionState.IRequiresSessionState
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessRequest(this.Context);
        }
        public override void ProcessRequest(System.Web.HttpContext context)
        {
            //if (!IsPostBack)
            {
                system_site wb = siteConfig;
                if (wb == null)
                {
                    context.Response.Write("站点不存在！");
                    return;
                }
                if (string.IsNullOrEmpty(wb.TemplatePath))
                {
                    context.Response.Write("请选择站点模板目录！");
                    return;
                }
                if (!Directory.Exists(Utils.GetMapPath("~" + wb.TemplatePath.Trim())))
                {
                    context.Response.Write("模板目录不存在！");
                    return;
                }
                if (string.IsNullOrEmpty(wb.IndexTemplate.Trim()))
                {
                    context.Response.Write("请选择首页模板！");
                    return;
                }
                if (!File.Exists(Utils.GetMapPath("~" + wb.TemplatePath + "/" + wb.IndexTemplate + "")))
                {
                    context.Response.Write("首页模板不存在！");
                    return;
                }
                VelocityHelper vh = new VelocityHelper();
                vh.Init("~"+wb.TemplatePath);
                vh.Put("site", wb);
                vh.Display(wb.IndexTemplate);
            }

        }
    }
    
}