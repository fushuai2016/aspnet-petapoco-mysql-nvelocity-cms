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
    public partial class browser_log : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "为民服务站用户浏览器信息";
        //列表页面名称
        public const string ListName = "browser_log.aspx";

        protected long totalCount;
        protected int pageIndex;
        protected int pageSize;

        protected string keywords = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            string viewtype=Utils.GetCookie("browser_log_view");
            if (viewtype == "Img")
            {
                Response.Redirect(Utils.CombUrlTxt("browser_log_img.aspx", "mid={0}", ModuleID.ToString()));
            }
            this.keywords = DTRequest.GetQueryString("keywords");
            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.View); //检查权限

                RptBind(CombSqlTxt(keywords), " ORDER BY AddTime DESC ");
            }
        }

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string _keywords)
        {
            StringBuilder strTemp = new StringBuilder();
            _keywords = _keywords.Replace("'", "");
            if (!string.IsNullOrEmpty(_keywords))
            {
                strTemp.Append(" (UserCode LIKE  '%" + _keywords + "%' OR RegionCode like '%" + _keywords + "%' OR PlatForms like '%" + _keywords + "%' OR Browsers like '%" + _keywords + "%' OR Ver like '%" + _keywords + "%' OR Engines like '%" + _keywords + "%') ");
            }

            return strTemp.ToString();
        }
        #endregion

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {
            this.pageIndex = DTRequest.GetQueryInt("page", 1);
            txtKeywords.Text = this.keywords;

            this.rptList.DataSource = db.GetPageList<t_browser>(this.pageIndex, this.pageSize, _strWhere, _orderby, out totalCount);
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
            if (int.TryParse(Utils.GetCookie("browser_page_size"), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    return _pagesize;
                }
            }
            return _default_size;
        }
        #endregion

        //设置图文列表显示
        protected void lbtnViewImg_Click(object sender, EventArgs e)
        {
            Utils.WriteCookie("browser_log_view", "Img", 14400);
            Response.Redirect(Utils.CombUrlTxt("browser_log_img.aspx", "mid={0}",
                ModuleID.ToString()));
        }
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
                    Utils.WriteCookie("browser_page_size", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt(ListName, "mid={0}&keywords={1}", ModuleID.ToString(), this.keywords));
        }
    }
}