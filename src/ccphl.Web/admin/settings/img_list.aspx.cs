using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;
using ccphl.Entity;

namespace ccphl.Web.admin.settings
{
    public partial class img_list : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "图片";
        //列表页面名称
        public const string ListName = "img_list.aspx";
        //编辑页面名称
        public const string EditName = "img_edit.aspx";
        XmlConfig<ImageConfig> xc = new XmlConfig<ImageConfig>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.View); //检查权限
                RptBind();
            }
        }

        #region 数据绑定=================================
        private void RptBind()
        {
            this.rptList.DataSource = xc.loadImgListConfig();

            this.rptList.DataBind();
        }
        #endregion

        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChkAdminLevel((long)DTEnums.ActionEnum.Delete); //检查权限
            int sucCount = 0;
            int errorCount = 0;
            List<ImageConfig>  list=xc.loadImgListConfig();
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                string id = ((HiddenField)rptList.Items[i].FindControl("hidId")).Value;
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    list=list.Where(p => p.imgcode != id).ToList();
                    sucCount += 1;
                }
            }
            xc.saveImgListConifg(list);
            AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除"+Name+"成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
            JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！",
                Utils.CombUrlTxt(ListName, "mid={0}", ModuleID.ToString()), "Success", "parent.loadMenuTree");
        }
    }
}