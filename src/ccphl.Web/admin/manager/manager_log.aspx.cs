using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;

namespace ccphl.Web.admin.manager
{
    public partial class manager_log : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "管理日志";
        //列表页面名称
        public const string ListName = "manager_log.aspx";

        protected long totalCount;
        protected int pageIndex;
        protected int pageSize;

        protected string keywords = string.Empty;
        system_user model = new system_user();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = DTRequest.GetQueryString("keywords");
            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.View); //检查权限
                model = GetAdminInfo(); //取得当前管理员信息
                RptBind(CombSqlTxt(keywords), " ORDER BY OperatedTime DESC ");
            }
        }

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string _keywords)
        {
            StringBuilder strTemp = new StringBuilder();
            _keywords = _keywords.Replace("'", "");
            if (!string.IsNullOrEmpty(_keywords))
            {
                strTemp.Append(" (UserName LIKE  '%" + _keywords + "%' OR LogCotent like '%" + _keywords + "%' OR OperateType like '%" + _keywords + "%') AND SiteID=" + siteConfig.SiteID);
            }
            else
            {
                strTemp.Append(" SiteID=" + siteConfig.SiteID);
            }

            return strTemp.ToString();
        }
        #endregion

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {
            this.pageIndex = DTRequest.GetQueryInt("page", 1);
            txtKeywords.Text = this.keywords;

            this.rptList.DataSource = db.GetPageList<system_log>(this.pageIndex, this.pageSize, _strWhere, _orderby, out totalCount);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt(ListName, "mid={0}&keywords={1}&page={2}",ModuleID.ToString(), this.keywords, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.pageIndex, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 返回每页数量=============================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("manager_page_size"), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    return _pagesize;
                }
            }
            return _default_size;
        }
        #endregion

        //关健字查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt(ListName, "mid={0}&keywords={1}", ModuleID.ToString(), txtKeywords.Text));
        }
              
        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("manager_page_size", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt(ListName, "mid={0}&keywords={1}", ModuleID.ToString(), this.keywords));
        }

        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChkAdminLevel((long)DTEnums.ActionEnum.Delete); //检查权限
            string datetime = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") ;
            long sucCount = db.Delete<system_log>(" OperatedTime<" + datetime);
            AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除"+Name + sucCount + "条"); //记录日志
            JscriptMsg("删除日志" + sucCount + "条", Utils.CombUrlTxt(ListName, "keywords={0}", this.keywords), "Success");
        }
    }
}