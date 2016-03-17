using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;
using System.IO;

namespace ccphl.Web.admin.article
{
    public partial class category_edit : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "频道栏目";
        //列表页面名称
        public const string ListName = "category_list.aspx";
        //编辑页面名称
        public const string EditName = "category_edit.aspx";
        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        public int channelid;
        public int siteid;
        private int id = 0;
        private int ischange= 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");
            this.channelid = DTRequest.GetQueryInt("channelid");
            this.siteid = DTRequest.GetQueryInt("siteid");
            this.id = DTRequest.GetQueryInt("id");
            this.ischange = DTRequest.GetQueryInt("ischange");
            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", ListName, "Error");
                    return;
                }
                if (!db.IsExists<Model.channel>(this.id))
                {
                    JscriptMsg("记录不存在或已删除！", ListName, "Error");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.View); //检查权限
             
                
                indextemplate.Items.Add(new ListItem("请选择主页模板", ""));
                listtemplate.Items.Add(new ListItem("请选择列表模板", ""));
                showtemplate.Items.Add(new ListItem("请选择详细模板", ""));
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                    SiteTreeBind(this.siteid); //绑定类别
                    TreeBind(this.siteid, this.channelid);
                    if (this.siteid > 0)
                    {
                        this.ddlSiteId.SelectedValue = this.siteid.ToString();
                    }
                    if (this.channelid > 0)
                    {
                        this.ddlChannelId.SelectedValue = this.channelid.ToString();
                    }
                    
                }
                else
                {

                    SiteTreeBind(this.siteid); //绑定类别
                    TreeBind(this.siteid, this.channelid);
                    txtCallIndex.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=column_validate_code");
                    if (this.siteid > 0)
                    {
                        this.ddlSiteId.SelectedValue = this.siteid.ToString();
                    }
                    if (this.channelid > 0)
                    {
                        this.ddlChannelId.SelectedValue = this.channelid.ToString();
                    }
                    if (this.id > 0)
                    {
                        this.ddlParentId.SelectedValue = this.id.ToString();
                    }
                    if (siteid > 0)
                    {
                        system_site newsite = new system_site();
                        newsite = db.GetModel<system_site>(siteid);
                        if (!string.IsNullOrEmpty(newsite.TemplatePath))
                        {
                            DirectoryInfo fileDirectory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~" + newsite.TemplatePath.Trim()));
                            FileInfo[] fileinfo = fileDirectory.GetFiles();
                            for (int i = fileinfo.Length - 1; i >= 0; i--)
                            {
                                if (fileinfo[i].Extension == ".html")
                                {
                                    if (fileinfo[i].Name.Replace(".html", "").EndsWith("index"))
                                    {
                                        indextemplate.Items.Add(new ListItem(fileinfo[i].Name, fileinfo[i].Name));
                                    }
                                    else if (fileinfo[i].Name.Replace(".html", "").EndsWith("list"))
                                    {
                                        listtemplate.Items.Add(new ListItem(fileinfo[i].Name, fileinfo[i].Name));
                                    }
                                    else if (fileinfo[i].Name.Replace(".html", "").EndsWith("show"))
                                    {
                                        showtemplate.Items.Add(new ListItem(fileinfo[i].Name, fileinfo[i].Name));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #region 绑定类别=================================
        private void SiteTreeBind(int site_id)
        {
             system_user user = this.GetAdminInfo();
             this.ddlSiteId.Items.Clear();
             if (user.UserType == 3)
             {
                 this.ddlSiteId.Visible = true;
                 this.ddlSiteId.Items.Add(new ListItem("请选择站点...", ""));
                 var site = GetSiteTreeList(0);
                 foreach (var s in site)
                 {
                     string Id = s.SiteID.ToString();
                     int ClassLayer = s.SiteLayer ?? 1;
                     string Title = s.SiteName;

                     if (ClassLayer == 1)
                     {
                         this.ddlSiteId.Items.Add(new ListItem(Title, Id));
                     }
                     else
                     {
                         Title = "├ " + Title;
                         Title = Utils.StringOfChar(ClassLayer - 1, "　") + Title;
                         this.ddlSiteId.Items.Add(new ListItem(Title, Id));
                     }
                 }
             }
             else
             {
                 system_site site = siteConfig;
                 this.ddlSiteId.Items.Add(new ListItem(site.SiteName, site.SiteID.ToString()));
                 this.ddlSiteId.Visible = false;
             }
             this.ddlChannelId.Items.Clear();
             this.ddlChannelId.Items.Add(new ListItem("请选择频道...", ""));
             var chs = db.GetModelList<Model.channel>(" SiteID=" + site_id + " AND Status=1 Order By OrderBy ASC ");
             foreach (var s in chs)
             {
                 this.ddlChannelId.Items.Add(new ListItem(s.ChannelName, s.ChannelID.ToString()));
             }
        }
        private void TreeBind(int site_id,int channel_id)
        {

            this.ddlParentId.Items.Clear();
            this.ddlParentId.Items.Add(new ListItem("无父级栏目", "0"));
            var cols = GetColumnTreeList2(site_id, channel_id);
            foreach (var s in cols)
            {
                string Id = s.ColumnID.ToString();
                int ClassLayer = s.ColumnLayer ?? 1;
                string Title = s.ColumnName;

                if (ClassLayer == 1)
                {
                    this.ddlParentId.Items.Add(new ListItem(Title, Id));
                }
                else
                {
                    Title = "├ " + Title;
                    Title = Utils.StringOfChar(ClassLayer - 1, "　") + Title;
                    this.ddlParentId.Items.Add(new ListItem(Title, Id));
                }
            }
           
        }
        #endregion
        //切换站点
        protected void ddlSiteId_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt(EditName, "action={0}&mid={1}&id={2}&siteid={3}&channelid={4}&ischange=1", action, ModuleID.ToString(), id.ToString(), ddlSiteId.SelectedValue,ddlChannelId.SelectedValue));
        }
        //切换频道
        protected void ddlChannelId_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt(EditName, "action={0}&mid={1}&id={2}&siteid={3}&channelid={4}&ischange=1", action, ModuleID.ToString(), id.ToString(), ddlSiteId.SelectedValue, ddlChannelId.SelectedValue));
        }
        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            Model.column model = db.GetModel<Model.column>(_id);

            txtTitle.Text = model.ColumnName;
            txtCallIndex.Text = model.ColumnCode;
            txtTitle.Focus(); //设置焦点，防止JS无法提交
            txtCallIndex.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=column_validate_code&old_column_code=" + Utils.UrlEncode(model.ColumnCode));
            if (siteid > 0 && this.ischange == 1)
            {
                ddlSiteId.SelectedValue = siteid.ToString();
            }
            else
            {
                siteid = model.SiteID??0;
                ddlSiteId.SelectedValue = model.SiteID.ToString();
            }
            if (channelid > 0 && this.ischange == 1)
                ddlChannelId.SelectedValue = channelid.ToString();
            else
            {
                channelid = model.ChannelID ?? 0;
                ddlChannelId.SelectedValue = model.ChannelID.ToString();
            }
            
            ddlParentId.SelectedValue = model.ParentID.ToString();
            if (model.Status == 0)
            {
                cbStatus.Checked = true;
            }
            if (model.IsAlbums == 1)
            {
                cbIsAlbums.Checked = true;
            }
            if (model.IsAttach == 1)
            {
                cbIsAttach.Checked = true;
            }
            if (model.IsEdit == 1)
            {
                cbIsEdit.Checked = true;
            }
            txtSortId.Text = model.OrderBy.ToString();

            txtThumb.Text = model.ThumbPath;
            txtLinkUrl.Text = model.LinkOutUrl;
            system_site newsite = new system_site();
            if (siteid > 0)
                newsite = db.GetModel<system_site>(siteid);
            else
                newsite = db.GetModel<system_site>(model.SiteID);
            if (!string.IsNullOrEmpty(newsite.TemplatePath))
            {
                DirectoryInfo fileDirectory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~" + newsite.TemplatePath.Trim()));
                FileInfo[] fileinfo = fileDirectory.GetFiles();
                for (int i = fileinfo.Length - 1; i >= 0; i--)
                {
                    if (fileinfo[i].Extension == ".html")
                    {
                        if (fileinfo[i].Name.Replace(".html", "").EndsWith("index"))
                        {
                            indextemplate.Items.Add(new ListItem(fileinfo[i].Name, fileinfo[i].Name));
                        }
                        else if (fileinfo[i].Name.Replace(".html", "").EndsWith("list"))
                        {
                            listtemplate.Items.Add(new ListItem(fileinfo[i].Name, fileinfo[i].Name));
                        }
                        else if (fileinfo[i].Name.Replace(".html", "").EndsWith("show"))
                        {
                            showtemplate.Items.Add(new ListItem(fileinfo[i].Name, fileinfo[i].Name));
                        }
                    }
                }
            }
            indextemplate.SelectedIndex = indextemplate.Items.IndexOf(indextemplate.Items.FindByText(model.IndexTemplate));
            listtemplate.SelectedIndex = listtemplate.Items.IndexOf(listtemplate.Items.FindByText(model.ListTemplate));
            showtemplate.SelectedIndex = showtemplate.Items.IndexOf(showtemplate.Items.FindByText(model.DetailTemplate));
            txtkeywords.Text = model.Keywords;
            txtdescription.Text = model.Description;
            txtSeoTitle.Text = model.SEO_Title;
            txtSeoKeywords.Text = model.SEO_Keywords;
            txtSeoDescription.Text = model.SEO_Description;
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            Model.column model = new Model.column();

            model.GUID = Guid.NewGuid().ToString();
            model.SiteID = Utils.StrToInt(ddlSiteId.SelectedValue, 0);
            if (model.SiteID == 0)
                model.SiteID = siteConfig.SiteID;
            model.ChannelID = Utils.StrToInt(ddlChannelId.SelectedValue, 0);
            model.ParentID = Utils.StrToInt(ddlParentId.SelectedValue, 0);
            model.ColumnName = txtTitle.Text.Trim();
            model.ColumnCode = txtCallIndex.Text.Trim();
            model.ThumbPath = txtThumb.Text.Trim();

            model.Status = 1;
            model.IsAlbums = 0;
            model.IsAttach = 0;
            model.IsEdit = 0;

            if (cbIsAlbums.Checked == true)
            {
                model.IsAlbums = 1;
            }
            if (cbIsAttach.Checked == true)
            {
                model.IsAttach = 1;
            }
            if (cbIsEdit.Checked == true)
            {
                model.IsEdit = 1;
            }
            if (cbStatus.Checked == true)
            {
                model.Status = 0;
            }
            model.OrderBy = Utils.StrToInt(txtSortId.Text.Trim(), 99);

            model.AddTime = DateTime.Now;
            model.LinkOutUrl = txtLinkUrl.Text.Trim();
            model.IndexTemplate = indextemplate.Text;
            model.ListTemplate = listtemplate.Text;
            model.DetailTemplate = showtemplate.Text;

            model.Keywords = txtkeywords.Text.Trim();
            model.Description = txtdescription.Text.Trim();
            model.SEO_Title = txtSeoTitle.Text.Trim();
            model.SEO_Keywords = txtSeoKeywords.Text.Trim();
            model.SEO_Description = txtSeoDescription.Text.Trim();
            long _id = db.InsertTree<column>(model, "ColumnNode", "ColumnLayer", "ParentID");
             if (_id <= 0)
            {
                return false;
            }

            AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加" + Name + model.ColumnName); //记录日志
            return true;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            Model.column model = db.GetModel<Model.column>(_id);

            model.SiteID = Utils.StrToInt(ddlSiteId.SelectedValue, 0);
            model.ChannelID = Utils.StrToInt(ddlChannelId.SelectedValue, 0);
            model.ParentID = Utils.StrToInt(ddlParentId.SelectedValue, 0);
            model.ColumnName = txtTitle.Text.Trim();
            model.ColumnCode = txtCallIndex.Text.Trim();
            model.IsAlbums = 0;
            model.IsAttach = 0;
            model.IsEdit = 0;
            model.Status = 1;
            if (cbIsAlbums.Checked == true)
            {
                model.IsAlbums = 1;
            }
            if (cbIsAttach.Checked == true)
            {
                model.IsAttach = 1;
            }
            if (cbIsEdit.Checked == true)
            {
                model.IsEdit = 1;
            }
            if (cbStatus.Checked == true)
            {
                model.Status = 0;
            }
            model.OrderBy = Utils.StrToInt(txtSortId.Text.Trim(), 99);
            model.LinkOutUrl = txtLinkUrl.Text.Trim();
            
            model.IndexTemplate = indextemplate.Text;
            model.ListTemplate = listtemplate.Text;
            model.DetailTemplate = showtemplate.Text;

            model.Keywords = txtkeywords.Text.Trim();
            model.Description = txtdescription.Text.Trim();
            model.SEO_Title = txtSeoTitle.Text.Trim();
            model.SEO_Keywords = txtSeoKeywords.Text.Trim();
            model.SEO_Description = txtSeoDescription.Text.Trim();
            if (db.UpdateTree<column>(model, "ColumnNode", "ColumnLayer", "ParentID") > 0)
            {
                return false;
            }


            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改" + Name + model.ColumnName); //记录日志
            return true;
        }
        #endregion

        //保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.Edit); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误！", "", "Error");
                    return;
                }
                JscriptMsg("修改频道栏目信息成功！", ListName + "?siteid=" + siteid + "&channelid=" + channelid, "Success", "parent.loadMenuTree");
            }
            else //添加
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.Add); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", "", "Error");
                    return;
                }
                JscriptMsg("添加频道栏目信息成功！", ListName + "?siteid=" + siteid + "&channelid=" + channelid, "Success", "parent.loadMenuTree");
            }
        }

    }
}