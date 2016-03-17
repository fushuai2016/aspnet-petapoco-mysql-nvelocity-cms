using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;

namespace ccphl.Web.admin.manager
{
    public partial class role_list : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "管理角色";
        //列表页面名称
        public const string ListName = "role_list.aspx";
        //编辑页面名称
        public const string EditName = "role_edit.aspx";

        protected string keywords = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = DTRequest.GetQueryString("keywords");
            if (!Page.IsPostBack)
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.View); //检查权限
                system_user model = GetAdminInfo(); //取得当前管理员信息
                RptBind(CombSqlTxt(this.keywords));
            }
        }

        #region 数据绑定=================================
        private void RptBind(string _strWhere)
        {
            this.txtKeywords.Text = this.keywords;
            if (string.IsNullOrEmpty(_strWhere))
                this.rptList.DataSource = db.GetModelList<system_role>(" SiteID="+siteConfig.SiteID);
            else
                this.rptList.DataSource = db.GetModelList<system_role>(_strWhere+" AND SiteID=" + siteConfig.SiteID);
            this.rptList.DataBind();
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string _keywords)
        {
            StringBuilder strTemp = new StringBuilder();
            _keywords = _keywords.Replace("'", "");
            if (!string.IsNullOrEmpty(_keywords))
            {
                strTemp.Append(" RoleName like '%" + _keywords + "%'");
            }

            return strTemp.ToString();
        }
        #endregion

        //查询操作
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt(ListName, "mid={0}&keywords={1}",ModuleID.ToString(), txtKeywords.Text.Trim()));
        }

        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChkAdminLevel((long)DTEnums.ActionEnum.Delete); //检查权限
            int sucCount = 0; //成功数量
            int errorCount = 0; //失败数量
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    if (db.Delete<system_role>(id)>0)
                    {
                        db.Delete<system_role_column_button>(" RoleID="+id);//删除角色栏目按钮关联表
                        db.Delete<system_role_column_function>(" RoleID=" + id);//删除角色栏目功能关联表
                        db.Delete<system_role_module_button>(" RoleID=" + id);//删除角色菜单按钮关联表
                        db.Delete<system_role_module_function>(" RoleID=" + id);//删除角色菜单功能关联表
                        db.Delete<system_role_user>(" RoleID=" + id);//删除角色用户关联表
                        sucCount++;
                    }
                    else
                    {
                        errorCount++;
                    }
                }
            }
            AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除"+Name + sucCount + "条，失败" + errorCount + "条"); //记录日志
            JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt(ListName, "keywords={0}", this.keywords), "Success");
        }
    }
}