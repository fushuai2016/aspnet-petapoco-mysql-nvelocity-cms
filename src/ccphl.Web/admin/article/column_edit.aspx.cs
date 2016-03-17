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
    public partial class column_edit : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "栏目分类";
        //列表页面名称
        public const string ListName = "column_list.aspx";
        //编辑页面名称
        public const string EditName = "column_edit.aspx";
        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        public int channelid;
        public int siteid;
        private int id = 0;
        protected int columntype;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");
            this.channelid = DTRequest.GetQueryInt("channelid");
            this.siteid = DTRequest.GetQueryInt("siteid");
            this.id = DTRequest.GetQueryInt("id");
            this.columntype = DTRequest.GetQueryInt("columntype");
            if (this.channelid == 0 || this.siteid == 0)
            {
                JscriptMsg("传输参数不正确！", ListName, "Error");
                return;
            }
            this.chid.Value = channelid.ToString();
            this.sid.Value = siteid.ToString();

            /**************************************/
            this.content_cblPageType.Visible = false;
            this.content_txtThumb.Visible = false;
            this.content_indextemplate.Visible = false;
            this.content_listtemplate.Visible = false;
            this.content_showtemplate.Visible = false;
            /**************************************/
            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", ListName, "Error");
                    return;
                }
                if (!db.IsExists<Model.column>(this.id))
                {
                    JscriptMsg("记录不存在或已删除！", ListName, "Error");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminWebLevel(this.siteid, this.channelid, this.columntype, (long)DTEnums.ActionEnum.View); //检查权限
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
                    TreeBind(this.siteid, this.channelid);  
                }
                else
                {
                    TreeBind(this.siteid, this.channelid);
                    txtCallIndex.Focus();
                    txtCallIndex.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=column_validate_code2&siteid="+this.siteid+"&channelid="+this.channelid);
                    
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
        
        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            Model.column model = db.GetModel<Model.column>(_id);

            txtTitle.Text = model.ColumnName;
            txtCallIndex.Text = model.ColumnCode;
            txtCallIndex.Focus(); //设置焦点，防止JS无法提交
            txtCallIndex.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=column_validate_code2&old_column_code=" + Utils.UrlEncode(model.ColumnCode)+"&siteid="+this.siteid+"&channelid="+this.channelid);

            ddlParentId.SelectedValue = model.ParentID.ToString();
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
            txtSortId.Text = model.OrderBy.ToString();

            txtThumb.Text = model.ThumbPath;
            txtLinkUrl.Text = model.LinkOutUrl;
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
            model.SiteID = Utils.StrToInt(this.sid.Value, 0);

            model.ChannelID = Utils.StrToInt(this.chid.Value, 0);
            model.ParentID = Utils.StrToInt(ddlParentId.SelectedValue, 0);
            model.ColumnName = txtTitle.Text.Trim();
            model.ColumnCode = txtCallIndex.Text.Trim();
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

            model.SiteID = Utils.StrToInt(this.sid.Value, 0);
            model.ChannelID = Utils.StrToInt(this.chid.Value, 0);
            model.ParentID = Utils.StrToInt(ddlParentId.SelectedValue, 0);
            model.PageType = (sbyte)Utils.StrToInt(cblPageType.SelectedValue, 0);
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
                return true;
            }


            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改" + Name + model.ColumnName); //记录日志
            return false;
        }
        #endregion

        //保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminWebLevel(this.siteid, this.channelid, this.columntype, (long)DTEnums.ActionEnum.Edit); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误！", "", "Error");
                    return;
                }
                JscriptMsg("修改频道栏目信息成功！", ListName + "?siteid=" + siteid + "&channelid=" + channelid+"&columntype="+this.columntype, "Success", "parent.loadMenuTree");
            }
            else //添加
            {
                ChkAdminWebLevel(this.siteid, this.channelid, this.columntype, (long)DTEnums.ActionEnum.Add); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", "", "Error");
                    return;
                }
                JscriptMsg("添加频道栏目信息成功！", ListName + "?siteid=" + siteid + "&channelid=" + channelid + "&columntype=" + this.columntype, "Success", "parent.loadMenuTree");
            }
        }

    }
}