using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;

namespace ccphl.Web.admin.article
{
    public partial class column_list : Web.UI.ManagePage
    {
        protected int channelid;
        protected int siteid;
        protected int columntype;
        //页面名称
        public const string Name = "栏目分类";
        //列表页面名称
        public const string ListName = "column_list.aspx";
        //编辑页面名称
        public const string EditName = "column_edit.aspx";
        protected void Page_Load(object sender, EventArgs e)
        {
            this.channelid = DTRequest.GetQueryInt("channelid");
            this.siteid = DTRequest.GetQueryInt("siteid");
            this.columntype = DTRequest.GetQueryInt("columntype");
            if (!Page.IsPostBack)
            {
               
                if (this.siteid == 0 || channelid==0)
                {
                    JscriptMsg("传输参数不正确！", "", "Error");
                    return;
                }
                ChkAdminWebLevel(this.siteid, this.channelid, this.columntype, (long)DTEnums.ActionEnum.View); //检查权限
                this.chid.Value = channelid.ToString();
                this.sid.Value = siteid.ToString();
                RptBind(siteid, channelid);
            }
        }
        
        //数据绑定
        private void RptBind(int siteid,int channelid)
        {
            List<column> list = GetColumnTreeList(siteid, channelid);
            this.rptList.DataSource = list;
            this.rptList.DataBind();
        }

        //美化列表
        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Literal LitFirst = (Literal)e.Item.FindControl("LitFirst");
                HiddenField hidLayer = (HiddenField)e.Item.FindControl("hidLayer");
                string LitStyle = "<span style=\"display:inline-block;width:{0}px;\"></span>{1}{2}";
                string LitImg1 = "<span class=\"folder-open\"></span>";
                string LitImg2 = "<span class=\"folder-line\"></span>";

                int classLayer = Convert.ToInt32(hidLayer.Value);
                if (classLayer == 1)
                {
                    LitFirst.Text = LitImg1;
                }
                else
                {
                    LitFirst.Text = string.Format(LitStyle, (classLayer - 2) * 24, LitImg2, LitImg1);
                }
            }
        }
       
        //保存排序
        protected void btnSave_Click(object sender, EventArgs e)
        {
            ChkAdminWebLevel(this.siteid, this.channelid, this.columntype, (long)DTEnums.ActionEnum.Edit); //检查权限
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                int sortId;
                if (!int.TryParse(((TextBox)rptList.Items[i].FindControl("txtSortId")).Text.Trim(), out sortId))
                {
                    sortId = 99;
                }
                db.UpdateField<Model.column>(id, "OrderBy", sortId);
            }
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "保存" + Name + "排序"); //记录日志
            JscriptMsg("保存排序成功！", Utils.CombUrlTxt(ListName, "mid={0}&siteid={1}&channelid={2}&columntype={3}", ModuleID.ToString(), sid.Value, chid.Value,this.columntype.ToString()), "Success", "parent.loadMenuTree");
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
                    var model = db.GetModel<Model.column>(id);
                    if (model.ColumnID == 0)
                    {
                        JscriptMsg("栏目不存在，请刷新后再试！", "", "Error");
                        return;
                    }
                    if (db.IsExists<Model.column>(" ParentID=" + id) || model.ColumnID==0)
                    {
                        JscriptMsg("栏目“" + model .ColumnName+ "”存在子栏目不允许删除！", "", "Error");
                        return;
                    }
                    if (db.IsExists<Model.article>(" ArticleID IN(SELECT ArticleID FROM ccphl_article WHERE ColumnID IN(SELECT ColumnID FROM ccphl_column WHERE ColumnNode LIKE '%," + id + ",%')) AND IsDelete=0 "))
                    {
                        JscriptMsg("该栏目或子栏目存在新闻，不允许删除！", "", "Error");
                        return;
                    }
                    if (db.Delete<Model.column>(id) > 0)
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
            JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！",
                 Utils.CombUrlTxt(ListName, "mid={0}&siteid={1}&channelid={2}&columntype={3}", ModuleID.ToString(), sid.Value, chid.Value, this.columntype.ToString()), "Success", "parent.loadMenuTree");
        }
       
    }
}