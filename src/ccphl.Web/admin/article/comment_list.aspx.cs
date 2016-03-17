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
    public partial class comment_list : Web.UI.ManagePage
    {
        protected int channelid;
        protected int siteid;
        protected int columntype;
        protected long totalCount;
        protected int page;
        protected int pageSize;
        //页面名称
        public const string Name = "评论";
        //列表页面名称
        public const string ListName = "comment_list.aspx";
        //编辑页面名称
        public const string EditName = "comment_edit.aspx";

        protected string property = string.Empty;
        protected string keywords = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.channelid = DTRequest.GetQueryInt("channelid");
            this.siteid = DTRequest.GetQueryInt("siteid");

            this.property = DTRequest.GetQueryString("property");
            this.keywords = DTRequest.GetQueryString("keywords");
            this.columntype = DTRequest.GetQueryInt("columntype");

            if (channelid == 0||siteid==0)
            {
                JscriptMsg("参数不正确！", "", "Error");
                return;
            }
            this.chid.Value = this.channelid.ToString();
            this.sid.Value = this.siteid.ToString();

            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminWebLevel(this.siteid, this.channelid, this.columntype, (long)DTEnums.ActionEnum.View); //检查权限
                RptBind(CombSqlTxt(this.keywords, this.property));
            }
        }

        #region 数据绑定=================================
        private void RptBind(string _strWhere)
        {
            this.page = DTRequest.GetQueryInt("page", 1);
            this.ddlProperty.SelectedValue = this.property;
            this.txtKeywords.Text = this.keywords;
            this.rptList.DataSource = db.GetPageList<comment>(this.page, this.pageSize, _strWhere, " ORDER BY AddTime DESC ", out this.totalCount);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt(ListName, "mid={0}&siteid={1}&channelid={2}&keywords={3}&property={4}&columntype={5}&page={6}", ModuleID.ToString(), this.siteid.ToString(), this.channelid.ToString(), this.keywords, this.property,this.columntype.ToString(), "__id__");
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
                strTemp.Append(" SiteID=" + this.siteid + " AND ChannelID=" + this.channelid + " AND (ArticleTitle LIKE '%" + _keywords + "%' OR Text LIKE '%" + _keywords + "%' OR ReplyText LIKE '%" + _keywords + "%')");
            }
            if (!string.IsNullOrEmpty(_property))
            {
                switch (_property)
                {
                    case "isLock":
                        strTemp.Append(" AND Status=0");
                        break;
                    case "unLock":
                        strTemp.Append(" AND Status=1");
                        break;
                }
            }
            return strTemp.ToString();
        }
        #endregion

        #region 返回评论每页数量=========================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("article_comment_page_size"), out _pagesize))
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
            Response.Redirect(Utils.CombUrlTxt(ListName, "mid={0}&siteid={1}&channelid={2}&keywords={3}&property={4}&columntype={5}", ModuleID.ToString(), this.siteid.ToString(), this.channelid.ToString(), this.keywords, ddlProperty.SelectedValue,this.columntype.ToString()));
        }

        //筛选属性
        protected void ddlProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt(ListName, "mid={0}&siteid={1}&channelid={2}&keywords={3}&property={4}&columntype={5}", ModuleID.ToString(), this.siteid.ToString(), this.channelid.ToString(), this.keywords, ddlProperty.SelectedValue, this.columntype.ToString()));
        }

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("article_comment_page_size", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt(ListName, "mid={0}&siteid={1}&channelid={2}&keywords={3}&property={4}&columntype={5}", ModuleID.ToString(), this.siteid.ToString(), this.channelid.ToString(), this.keywords, this.property, this.columntype.ToString()));
        }

        //审核
        protected void btnAudit_Click(object sender, EventArgs e)
        {
            ChkAdminWebLevel(this.siteid, this.channelid, this.columntype, (long)DTEnums.ActionEnum.Audit); //检查权限
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                int sortId;
                if (!int.TryParse(((TextBox)rptList.Items[i].FindControl("txtSortId")).Text.Trim(), out sortId))
                {
                    sortId = 99;
                }
                db.UpdateField<Model.comment>(id, "OrderBy", sortId);
            }
            AddAdminLog(DTEnums.ActionEnum.Audit.ToString(), "审核" +Name+ "评论信息"); //记录日志
            JscriptMsg("审核通过成功！", Utils.CombUrlTxt(ListName, "mid={0}&siteid={1}&channelid={2}&keywords={3}&property={4}&columntype={5}", ModuleID.ToString(), this.siteid.ToString(), this.channelid.ToString(), this.keywords, this.property, this.columntype.ToString()), "Success");
        }

        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChkAdminWebLevel(this.siteid, this.channelid, this.columntype, (long)DTEnums.ActionEnum.Delete); //检查权限
            int sucCount = 0;
            int errorCount = 0;
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    if (db.Delete<comment>(id)>0)
                    {
                        sucCount += 1;
                    }
                    else
                    {
                        errorCount += 1;
                    }
                }
            }
            AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除" + Name + "成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
            JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt(ListName, "mid={0}&siteid={1}&channelid={2}&keywords={3}&property={4}&columntype={5}", ModuleID.ToString(), this.siteid.ToString(), this.channelid.ToString(), this.keywords, this.property, this.columntype.ToString()), "Success");
        }
    }
}