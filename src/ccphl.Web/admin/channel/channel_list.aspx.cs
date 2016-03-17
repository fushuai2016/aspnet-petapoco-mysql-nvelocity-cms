using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;

namespace ccphl.Web.admin.channel
{
    public partial class channel_list : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "频道";
        //列表页面名称
        public const string ListName = "channel_list.aspx";
        //编辑页面名称
        public const string EditName = "channel_edit.aspx";

        protected long totalCount;
        protected int page;
        protected int pageSize;

        protected string keywords = string.Empty;
        protected string channelid = string.Empty;
        protected int siteid =0;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = DTRequest.GetQueryString("keywords");
            this.channelid = DTRequest.GetQueryString("channelid");
            int sid=DTRequest.GetQueryInt("siteid",0);
            if (sid != 0)
            {
                siteid = sid;
            }
            
            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.View); //检查权限
                TreeBind(); //绑定类别
                RptBind( CombSqlTxt(channelid, keywords), "ORDER BY OrderBy ASC,ChannelID Desc");
            }
        }

        #region 绑定站点=================================
        private void TreeBind()
        {
            system_user user = this.GetAdminInfo();
            this.ddlCategoryId.Items.Clear();
            if (user.UserType == 3)
            {
                this.ddlCategoryId.Items.Add(new ListItem("全部站点", "0"));
                var site = GetSiteTreeList(0);
                foreach (var s in site)
                {
                    string Id = s.SiteID.ToString();
                    int ClassLayer = s.SiteLayer ?? 1;
                    string Title = s.SiteName;

                    if (ClassLayer == 1)
                    {
                        this.ddlCategoryId.Items.Add(new ListItem(Title, Id));
                    }
                    else
                    {
                        Title = "├ " + Title;
                        Title = Utils.StringOfChar(ClassLayer - 1, "　") + Title;
                        this.ddlCategoryId.Items.Add(new ListItem(Title, Id));
                    }
                }
                
            }
            else
            {
                system_site site = siteConfig;
                this.ddlCategoryId.Items.Add(new ListItem(site.SiteName, site.SiteID.ToString()));
            }
        }
        #endregion

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {
            this.page = DTRequest.GetQueryInt("page", 1);
            ddlCategoryId.SelectedValue = siteid.ToString();
            txtKeywords.Text = this.keywords;
            if (string.IsNullOrEmpty(_strWhere.Trim()))
            {
                if (siteid == 0)
                {
                    this.rptList.DataSource = db.GetPageList<Model.channel>(this.page, this.pageSize, " (SiteID>" + siteid + " OR IsSystem=1) ", _orderby, out this.totalCount);
                }
                else
                {
                    this.rptList.DataSource = db.GetPageList<Model.channel>(this.page, this.pageSize, " (SiteID=" + siteid + " OR IsSystem=1) ", _orderby, out this.totalCount);
                }
                this.rptList.DataBind();
            }
            else
            {
                if (siteid == 0)
                {
                    this.rptList.DataSource = db.GetPageList<Model.channel>(this.page, this.pageSize, _strWhere + " AND (SiteID>" + siteid + " OR IsSystem=1) ", _orderby, out this.totalCount);
                }
                else
                {
                    this.rptList.DataSource = db.GetPageList<Model.channel>(this.page, this.pageSize, _strWhere + " AND (SiteID=" + siteid + " OR IsSystem=1) ", _orderby, out this.totalCount);
                }
                
                this.rptList.DataBind();
            }
            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt(ListName, "mid={0}&keywords={1}&page={2}&siteid={3}", ModuleID.ToString(), this.keywords, "__id__",siteid.ToString());
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string channelid, string _keywords)
        {
            StringBuilder strTemp = new StringBuilder();
            _keywords = _keywords.Replace("'", "");
            if (!string.IsNullOrEmpty(_keywords))
            {
                strTemp.Append("  ChannelName LIKE  '%" + _keywords + "%'");
            }
            return strTemp.ToString();
        }
        #endregion

        #region 返回每页数量=============================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("channel_page_size"), out _pagesize))
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
            Response.Redirect(Utils.CombUrlTxt(ListName, "mid={0}&keywords={1}&siteid={2}", ModuleID.ToString(), txtKeywords.Text,ddlCategoryId.SelectedValue));
        }

        //筛选类型
        protected void ddlCategoryId_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt(ListName, "mid={0}&siteid={1}", ModuleID.ToString(),ddlCategoryId.SelectedValue));
        }

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("channel_page_size", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt(ListName, "mid={0}&keywords={1}&siteid={2}", ModuleID.ToString(), this.keywords,ddlCategoryId.SelectedValue));
        }

        //保存排序
        protected void btnSave_Click(object sender, EventArgs e)
        {
            ChkAdminLevel((long)DTEnums.ActionEnum.Edit); //检查权限
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                int sortId;
                if (!int.TryParse(((TextBox)rptList.Items[i].FindControl("txtSortId")).Text.Trim(), out sortId))
                {
                    sortId = 99;
                }
                db.UpdateField<Model.channel>(id, "OrderBy", sortId);
            }
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "保存"+Name+"排序"); //记录日志
            JscriptMsg("保存排序成功！", Utils.CombUrlTxt(ListName, "mid={0}&keywords={1}&siteid={2}", ModuleID.ToString(), this.keywords,ddlCategoryId.SelectedValue), "Success", "parent.loadMenuTree");
        }

        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChkAdminLevel((long)DTEnums.ActionEnum.Delete); //检查权限
            int sucCount = 0;
            int errorCount = 0;
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                   
                    if (db.Delete<Model.channel>(id)>0)
                    {
                        sucCount += 1;
                    }
                    else
                    {
                        errorCount += 1;
                    }
                }
            }
            AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除"+Name+"成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
            JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！",
                Utils.CombUrlTxt(ListName, "mid={0}&keywords={1}&siteid={2}", ModuleID.ToString(), this.keywords,ddlCategoryId.SelectedValue), "Success", "parent.loadMenuTree");
        }
        public string GetSiteName(int siteid)
        {
            string sitename = db.GetModel<system_site>(siteid).SiteName;
            return sitename;
        }
    }
}