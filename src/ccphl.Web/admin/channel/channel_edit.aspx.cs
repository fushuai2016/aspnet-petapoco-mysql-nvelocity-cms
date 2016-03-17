using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;
using System.IO;

namespace ccphl.Web.admin.channel
{
    public partial class channel_edit : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "频道";
        //列表页面名称
        public const string ListName = "channel_list.aspx";
        //编辑页面名称
        public const string EditName = "channel_edit.aspx";

        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;
        public int siteid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");
            this.id = DTRequest.GetQueryInt("id");
            this.siteid = DTRequest.GetQueryInt("siteid");
            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
               
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", "", "Error");
                    return;
                }
                if (!db.IsExists<Model.channel>(this.id))
                {
                    JscriptMsg("记录不存在或已删除！", "", "Error");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.View); //检查权限
                TreeBind(); //绑定类别
                cblPageType.Items.Add(new ListItem("请选择页面类型", "0"));
                cblPageType.Items.Add(new ListItem("专题页面", "1"));
                cblPageType.Items.Add(new ListItem("列表页面", "2"));
                cblPageType.Items.Add(new ListItem("详细页面", "3"));
                indextemplate.Items.Add(new ListItem("请选择主页模板", ""));
                listtemplate.Items.Add(new ListItem("请选择列表模板", ""));
                showtemplate.Items.Add(new ListItem("请选择详细模板", ""));
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
                else
                {
                    //txtCode.Attributes.Add("onBlur", "change2cn(this.value, this.form.txtName)");
                    if (siteid > 0)
                        txtCode.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=channel_validate_code&siteid=" + siteid);
                    else
                        txtCode.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=channel_validate_code&siteid=" + siteConfig.SiteID);
                    txtName.Attributes.Add("onBlur", "change2py(this.value, this.form.txtCode)");
                    txtCode.Focus();
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
        private void TreeBind()
        {
            system_user user = this.GetAdminInfo();
            this.ddlCategoryId.Items.Clear();
            if (user.UserType == 3)
            {
                this.cbIsSystem.Visible = true;
                this.ddlCategoryId.Items.Add(new ListItem("请选择站点...", ""));
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
                this.cbIsSystem.Visible = false;
                system_site site = siteConfig;
                this.ddlCategoryId.Items.Add(new ListItem(site.SiteName, site.SiteID.ToString()));
            }
            if (siteid > 0)
            {
                this.ddlCategoryId.SelectedValue = siteid.ToString();
            }
        }
        #endregion

        //切换站点
        protected void ddlCategoryId_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt(EditName, "action={0}&mid={1}&id={2}&siteid={3}", action,ModuleID.ToString(), id.ToString(), ddlCategoryId.SelectedValue));
        }
       

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {

            Model.channel model = db.GetModel<Model.channel>(_id);

            txtCode.Text = model.ChannelCode;
            txtName.Text = model.ChannelName;
            txtName.Attributes.Add("onBlur", "change2py(this.value, this.form.txtCode)");
            txtCode.Focus(); //设置焦点，防止JS无法提交
            if(siteid>0)
                txtCode.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=channel_validate_code&old_channel_code=" + Utils.UrlEncode(model.ChannelCode) + "&siteid=" + siteid);
            else
                txtCode.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=channel_validate_code&old_channel_code=" + Utils.UrlEncode(model.ChannelCode) + "&siteid=" + siteConfig.SiteID);
            if(siteid>0)
                ddlCategoryId.SelectedValue = siteid.ToString();
            else
                ddlCategoryId.SelectedValue = model.SiteID.ToString();
            cblPageType.SelectedValue = model.PageType.ToString();
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
            if (model.IsSystem == 1)
            {
                cbIsSystem.Checked = true;
            }
            txtPageSize.Text = model.PageSize.ToString();
            txtSortId.Text = model.OrderBy.ToString();

            txtThumb.Text = model.ThumbPath;
            txtLinkUrl.Text = model.LinkOutUrl;
            system_site newsite = new system_site();
            if(siteid>0)
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
            Model.channel model = new Model.channel();

            model.GUID = Guid.NewGuid().ToString();
            model.SiteID = Utils.StrToInt(ddlCategoryId.SelectedValue, 0);
            model.ChannelCode = txtCode.Text.Trim();
            model.ChannelName =txtName.Text.Trim();
            model.ThumbPath = txtThumb.Text.Trim();

            model.PageType = (sbyte)Utils.StrToInt(cblPageType.SelectedValue, 0);
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
            model.PageSize = Utils.StrToInt(txtPageSize.Text.Trim(), 10);
            model.OrderBy = Utils.StrToInt(txtSortId.Text.Trim(), 99);

            model.AddTime = DateTime.Now;
            model.LinkOutUrl = txtLinkUrl.Text.Trim();
            model.IndexTemplate = indextemplate.Text;
            model.ListTemplate = listtemplate.Text;
            model.DetailTemplate = showtemplate.Text;
            system_user user = this.GetAdminInfo();
            model.IsSystem = 0;
            if (user.UserType == 3)
            {
                if (cbIsSystem.Checked)
                {
                    model.IsSystem = 1;
                }
            }

            model.Keywords = txtkeywords.Text.Trim();
            model.Description=txtdescription.Text.Trim();
            model.SEO_Title=txtSeoTitle.Text.Trim();
            model.SEO_Keywords=txtSeoKeywords.Text.Trim();
            model.SEO_Description=txtSeoDescription.Text.Trim();
            if (db.Insert<Model.channel>(model) < 1)
            {
                return false;
            }

            AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加"+Name + model.ChannelName); //记录日志
            return true;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            Model.channel model = db.GetModel<Model.channel>(_id);

            string old_name = model.ChannelCode;
            model.SiteID = Utils.StrToInt(ddlCategoryId.SelectedValue, 0);
            model.ChannelCode = txtCode.Text.Trim();
            model.ChannelName = txtName.Text.Trim();
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
            model.PageType = (sbyte)Utils.StrToInt(cblPageType.SelectedValue, 0);
            model.PageSize = Utils.StrToInt(txtPageSize.Text.Trim(), 10);
            model.OrderBy = Utils.StrToInt(txtSortId.Text.Trim(), 99);
            model.LinkOutUrl = txtLinkUrl.Text.Trim();
            model.IsSystem = 0;
            system_user user = this.GetAdminInfo();
            if (user.UserType == 3)
            {
                if (cbIsSystem.Checked)
                {
                    model.IsSystem = 1;
                }
            }
            model.IndexTemplate = indextemplate.Text;
            model.ListTemplate = listtemplate.Text;
            model.DetailTemplate = showtemplate.Text;

            model.Keywords = txtkeywords.Text.Trim();
            model.Description = txtdescription.Text.Trim();
            model.SEO_Title = txtSeoTitle.Text.Trim();
            model.SEO_Keywords = txtSeoKeywords.Text.Trim();
            model.SEO_Description = txtSeoDescription.Text.Trim();
            if (db.Update<Model.channel>(model)<1)
            {
                return false;
            }


            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改"+ Name+ model.ChannelName); //记录日志
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
                JscriptMsg("修改频道信息成功！",ListName, "Success", "parent.loadMenuTree");
            }
            else //添加
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.Add); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", "", "Error");
                    return;
                }
                JscriptMsg("添加频道信息成功！", ListName, "Success", "parent.loadMenuTree");
            }
        }
    }
}