using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using ccphl.DBUtility;
using Model;

namespace ccphl.Web.admin.article
{
    public partial class article_list : Web.UI.ManagePage
    {
        protected int siteid;
        protected int channelid;
        protected long totalCount;
        protected int page;
        protected int pageSize;

        protected int columnid;
        protected int columntype;

        protected string property = string.Empty;
        protected string keywords = string.Empty;
        protected string prolistview = string.Empty;
        //页面名称
        public const string Name = "文章";
        //列表页面名称
        public const string ListName = "article_list.aspx";
        //编辑页面名称
        public const string EditName = "article_edit.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.siteid = DTRequest.GetQueryInt("siteid");
            this.channelid = DTRequest.GetQueryInt("channelid");
            this.columnid = DTRequest.GetQueryInt("columnid");
            this.keywords = DTRequest.GetQueryString("keywords");
            this.property = DTRequest.GetQueryString("property");
            this.columntype = DTRequest.GetQueryInt("columntype");

            if (siteid == 0 || channelid==0)
            {
                JscriptMsg("频道参数不正确！", "", "Error");
                return;
            }
  
            this.pageSize = GetPageSize(10); //每页数量
            this.prolistview = Utils.GetCookie("article_list_view"); //显示方式
            if (!Page.IsPostBack)
            {
                ChkAdminWebLevel(this.siteid, this.channelid, this.columntype, (long)DTEnums.ActionEnum.View); //检查权限
                TreeBind(this.siteid, this.channelid); //绑定类别
                RptBind(this.siteid,this.channelid, this.columnid, "ArticleID>0" + CombSqlTxt(this.keywords, this.property));
            }
        }

        #region 绑定类别=================================
        private void TreeBind(int site_id,int channel_id)
        {
            this.ddlCategoryId.Items.Clear();
            this.ddlCategoryId.Items.Add(new ListItem("所有栏目", ""));
            var cols = GetColumnTreeList2(site_id, channel_id);
            foreach (var s in cols)
            {
                string Id = s.ColumnID.ToString();
                int ClassLayer = s.ColumnLayer ?? 1;
                string Title = s.ColumnName;

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
        #endregion

        #region 数据绑定=================================
        private void RptBind(int site_id,int channel_id, int column_id,string _strWhere)
        {
            this.page = DTRequest.GetQueryInt("page", 1);
            if (this.columnid > 0)
            {
                this.ddlCategoryId.SelectedValue = column_id.ToString();
            }
            this.ddlProperty.SelectedValue = this.property;
            this.txtKeywords.Text = this.keywords;
            //图表或列表显示
            switch (this.prolistview)
            {
                case "Txt":
                    this.rptList2.Visible = false;
                    this.rptList1.DataSource = db.GetPageList<Model.article>(this.page, this.pageSize, _strWhere, " ORDER BY OrderBy ASC,AddTime DESC ", out this.totalCount);
                    this.rptList1.DataBind();
                    break;
                default:
                    this.rptList1.Visible = false;
                    this.rptList2.DataSource = db.GetPageList<Model.article>(this.page, this.pageSize, _strWhere, " ORDER BY OrderBy ASC,AddTime DESC ", out this.totalCount);
                    this.rptList2.DataBind();
                    break;
            }
            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt(ListName, "siteid={0}&channelid={1}&columnid={2}&keywords={3}&property={4}&columntype={5}&page={6}",
                site_id.ToString(), channel_id.ToString(), column_id.ToString(), this.keywords, this.property,this.columntype.ToString(), "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string _keywords, string _property)
        {
            StringBuilder strTemp = new StringBuilder();
            _keywords = _keywords.Replace("'", "");
            if (!string.IsNullOrEmpty(_keywords))
            {
                strTemp.Append(" AND ArticleName Like '%" + _keywords + "%' ");
            }
            if (this.columnid > 0)
            {
                strTemp.Append(" AND ColumnID=" + this.columnid);
            }
            if (!string.IsNullOrEmpty(_property))
            {
                switch (_property)
                {
                    case "isLock":
                        strTemp.Append(" AND Status=1");
                        break;
                    case "unIsLock":
                        strTemp.Append(" AND Status=0");
                        break;
                    case "isCom":
                        strTemp.Append(" AND IsCom=1");
                        break;
                    case "isTop":
                        strTemp.Append(" AND IsTop=1");
                        break;
                    case "isRed":
                        strTemp.Append(" AND IsRed=1");
                        break;
                    case "isHot":
                        strTemp.Append(" AND IsHot=1");
                        break;
                    case "isPost":
                        strTemp.Append(" AND IsPost=1");
                        break;
                }
            }
            strTemp.Append(" AND IsDelete=0  ");
            return strTemp.ToString();
        }
        #endregion

        #region 返回图文每页数量=========================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("article_page_size"), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    return _pagesize;
                }
            }
            return _default_size;
        }
        #endregion

        //设置操作
        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ChkAdminWebLevel(this.siteid, this.channelid, this.columntype, (long)DTEnums.ActionEnum.Edit); //检查权限
            int id = Convert.ToInt32(((HiddenField)e.Item.FindControl("hidId")).Value);
            Model.article model = db.GetModel<Model.article>(id);
            switch (e.CommandName)
            {
                case "lbtnIsMsg":
                    if (model.IsCom == 1)
                        db.UpdateField<Model.article>(id,"IsCom",0);
                    else
                        db.UpdateField<Model.article>(id, "IsCom", 1);
                    break;
                case "lbtnIsTop":
                    if (model.IsTop == 1)
                        db.UpdateField<Model.article>(id, "IsTop", 0);
                    else
                        db.UpdateField<Model.article>(id, "IsTop", 1);
                    break;
                case "lbtnIsRed":
                    if (model.IsRed == 1)
                        db.UpdateField<Model.article>(id, "IsRed", 0);
                    else
                        db.UpdateField<Model.article>(id, "IsRed", 1);
                    break;
                case "lbtnIsHot":
                    if (model.IsHot == 1)
                        db.UpdateField<Model.article>(id, "IsHot", 0);
                    else
                        db.UpdateField<Model.article>(id, "IsHot", 1);
                    break;
                case "lbtnIsSlide":
                    if (model.IsPost == 1)
                        db.UpdateField<Model.article>(id, "IsPost", 0);
                    else
                        db.UpdateField<Model.article>(id, "IsPost", 1);
                    break;
            }
            RptBind(this.siteid, this.channelid, this.columnid, "ArticleID>0" + CombSqlTxt(this.keywords, this.property));
        }

        //关健字查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt(ListName, "siteid={0}&channelid={1}&columnid={2}&keywords={3}&property={4}&columntype={5}",
                this.siteid.ToString(), this.channelid.ToString(), this.columnid.ToString(), txtKeywords.Text, this.property, this.columntype.ToString()));
        }

        //筛选类别
        protected void ddlCategoryId_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt(ListName, "siteid={0}&channelid={1}&columnid={2}&keywords={3}&property={4}&columntype={5}",
                this.siteid.ToString(), this.channelid.ToString(), ddlCategoryId.SelectedValue, this.keywords, this.property, this.columntype.ToString()));
        }

        //筛选属性
        protected void ddlProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt(ListName, "siteid={0}&channelid={1}&columnid={2}&keywords={3}&property={4}&columntype={5}",
                this.siteid.ToString(), this.channelid.ToString(), this.columnid.ToString(), this.keywords, ddlProperty.SelectedValue, this.columntype.ToString()));
        }

        //设置文字列表显示
        protected void lbtnViewTxt_Click(object sender, EventArgs e)
        {
            Utils.WriteCookie("article_list_view", "Txt", 14400);
            Response.Redirect(Utils.CombUrlTxt(ListName, "siteid={0}&channelid={1}&columnid={2}&keywords={3}&property={4}&page={5}&columntype={5}",
                this.siteid.ToString(), this.channelid.ToString(), this.columnid.ToString(), this.keywords, this.property, this.page.ToString(), this.columntype.ToString()));
        }

        //设置图文列表显示
        protected void lbtnViewImg_Click(object sender, EventArgs e)
        {
            Utils.WriteCookie("article_list_view", "Img", 14400);
            Response.Redirect(Utils.CombUrlTxt(ListName, "siteid={0}&channelid={1}&columnid={2}&keywords={3}&property={4}&page={5}&columntype={5}",
                 this.siteid.ToString(), this.channelid.ToString(), this.columnid.ToString(), this.keywords, this.property, this.page.ToString(), this.columntype.ToString()));
        }

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("article_page_size", _pagesize.ToString(), 43200);
                }
            }
            Response.Redirect(Utils.CombUrlTxt(ListName, "siteid={0}&channelid={1}&columnid={2}&keywords={3}&property={4}&columntype={5}",
                this.siteid.ToString(), this.channelid.ToString(), this.columnid.ToString(), this.keywords, this.property, this.columntype.ToString()));
        }

        //保存排序
        protected void btnSave_Click(object sender, EventArgs e)
        {
            ChkAdminWebLevel(this.siteid, this.channelid, this.columntype, (long)DTEnums.ActionEnum.Edit); //检查权限

            Repeater rptList = new Repeater();
            switch (this.prolistview)
            {
                case "Txt":
                    rptList = this.rptList1;
                    break;
                default:
                    rptList = this.rptList2;
                    break;
            }
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                int sortId;
                if (!int.TryParse(((TextBox)rptList.Items[i].FindControl("txtSortId")).Text.Trim(), out sortId))
                {
                    sortId = 99;
                }
                db.UpdateField<Model.article>(id, "OrderBy", sortId);
            }
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "保存频道内容排序"); //记录日志
            JscriptMsg("保存排序成功啦！", Utils.CombUrlTxt(ListName, "siteid={0}&channelid={1}&columnid={2}&keywords={3}&property={4}&columntype={5}",
                this.siteid.ToString(), this.channelid.ToString(), this.columnid.ToString(), this.keywords, this.property, this.columntype.ToString()), "Success");
        }

        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChkAdminWebLevel(this.siteid, this.channelid, this.columntype, (long)DTEnums.ActionEnum.Delete); //检查权限
            int sucCount = 0; //成功数量
            int errorCount = 0; //失败数量
            Repeater rptList = new Repeater();
            switch (this.prolistview)
            {
                case "Txt":
                    rptList = this.rptList1;
                    break;
                default:
                    rptList = this.rptList2;
                    break;
            }
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    if (db.UpdateField<Model.article>(id, "IsDelete", 1)>0)
                    {
                        sucCount++;
                    }
                    else
                    {
                        errorCount++;
                    }
                }
            }
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "删除频道内容成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
            JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt(ListName, "siteid={0}&channelid={1}&columnid={2}&keywords={3}&property={4}&columntype={5}",
                this.siteid.ToString(), this.channelid.ToString(), this.columnid.ToString(), this.keywords, this.property, this.columntype.ToString()), "Success");
        }
        public string GetColumnName(int column_id)
        {
            string sitename = db.GetModel<Model.column>(column_id).ColumnName;
            return sitename;
        }
    }
}