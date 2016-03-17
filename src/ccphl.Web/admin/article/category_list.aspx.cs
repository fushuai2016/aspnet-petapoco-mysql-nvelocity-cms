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
    public partial class category_list : Web.UI.ManagePage
    {
        protected int channelid;
        protected int siteid;
        //页面名称
        public const string Name = "频道栏目";
        //列表页面名称
        public const string ListName = "category_list.aspx";
        //编辑页面名称
        public const string EditName = "category_edit.aspx";
        protected void Page_Load(object sender, EventArgs e)
        {
            this.channelid = DTRequest.GetQueryInt("channelid");
            this.siteid = DTRequest.GetQueryInt("siteid");
           
            if (!Page.IsPostBack)
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.View); //检查权限
                //if (siteid == 0)
                //    siteid = siteConfig.SiteID;
                SiteTreeBind();
                ChannelTreeBind(siteid);
                if (siteid> 0)
                {
                    this.ddlCategoryId.SelectedValue = siteid.ToString();
                }
                if (channelid > 0)
                {
                    this.ddlChannelId.SelectedValue = channelid.ToString();
                }
                RptBind(siteid, channelid);
            }
        }
        //站点树绑定
        private void SiteTreeBind()
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
        //频道绑定
        private void ChannelTreeBind(int siteid)
        {
            this.ddlChannelId.Items.Clear();
            this.ddlChannelId.Items.Add(new ListItem("全部频道", "0"));
            
            var chs = db.GetModelList<Model.channel>(" SiteID=" + siteid);
            foreach (var s in chs)
            {
                this.ddlChannelId.Items.Add(new ListItem(s.ChannelName, s.ChannelID.ToString()));
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
        //筛选类型
        protected void ddlCategoryId_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt(ListName, "mid={0}&siteid={1}&channelid={2}", ModuleID.ToString(), ddlCategoryId.SelectedValue, ddlChannelId.SelectedValue));
        }
        //筛选频道
        protected void ddlChannelId_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt(ListName, "mid={0}&siteid={1}&channelid={2}", ModuleID.ToString(), ddlCategoryId.SelectedValue, ddlChannelId.SelectedValue));
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
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "保存" + Name + "排序"); //记录日志
            JscriptMsg("保存排序成功！", Utils.CombUrlTxt(ListName, "mid={0}&siteid={1}&channelid={2}", ModuleID.ToString(), ddlCategoryId.SelectedValue, ddlChannelId.SelectedValue), "Success", "parent.loadMenuTree");
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

                    if (db.Delete<Model.channel>(id) > 0)
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
                 Utils.CombUrlTxt(ListName, "mid={0}&siteid={1}&channelid={2}", ModuleID.ToString(), ddlCategoryId.SelectedValue, ddlChannelId.SelectedValue), "Success", "parent.loadMenuTree");
        }
        public string GetChannelName(int channel_id)
        {
            string sitename = db.GetModel<Model.channel>(channel_id).ChannelName;
            return sitename;
        }
    }
}