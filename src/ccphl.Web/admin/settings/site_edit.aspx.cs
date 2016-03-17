using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;
using System.IO;

namespace ccphl.Web.admin.settings
{
    public partial class site_edit : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "站点";
        //列表页面名称
        public const string ListName = "site_list.aspx";
        //编辑页面名称
        public const string EditName = "site_edit.aspx";

        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");
            this.id = DTRequest.GetQueryInt("id");

            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", "", "Error");
                    return;
                }
                if (!db.IsExists<system_site>(this.id))
                {
                    JscriptMsg("站点不存在或已被删除！", "", "Error");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.View); //检查权限
                TreeBind(DTEnums.NavigationEnum.System.ToString()); //绑定站点
                TemplateBind();
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
                else
                {
                    if (this.id > 0)
                    {
                        this.ddlParentId.SelectedValue = this.id.ToString();
                    }
                    webname.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=site_validate_name");
                    webname.Attributes.Add("onBlur", "change2py(this.value, this.form.webcode)");
                    webcode.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=site_validate_code");
                    weburl.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=site_validate_url");
                }
            }
        }

        #region 绑定=============================
        private void TreeBind(string nav_type)
        {

            List<system_site> list = GetSiteTreeList(0);

            this.ddlParentId.Items.Clear();
            this.ddlParentId.Items.Add(new ListItem("无父级站点", "0"));
            foreach(var m in list)
            {
                string Id = m.SiteID.ToString();
                int ClassLayer = m.SiteLayer??1;
                string Title = m.SiteName;

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


        #region 绑定模板=============================
        private void TemplateBind()
        {
            template.Items.Add(new ListItem("请选择模板文件目录", ""));
            string[] dirList1 = Directory.GetDirectories(HttpContext.Current.Server.MapPath("~/template"));
            for (int i = 0; i < dirList1.Length; i++)
            {
                DirectoryInfo d = new DirectoryInfo(dirList1[i]);
                if (d.Name.IndexOf("images") == -1 || d.Name.IndexOf("style") == -1 || d.Name.IndexOf("js") == -1)
                {
                    template.Items.Add(new ListItem("/template/" + d.Name, "/template/" + d.Name));
                }
            }
            template.Attributes.Add("onchange", "selectChange(this,'" + this.template.ClientID + "')");
            indextemplate.Items.Add(new ListItem("请选择主页模板", ""));
            listtemplate.Items.Add(new ListItem("请选择列表模板", ""));
            showtemplate.Items.Add(new ListItem("请选择详细模板", ""));
        }
        #endregion

        #region 绑定频道=========================
        private void ChannelBind()
        {
            var list = db.GetModelList<Model.channel>(" (SiteID=" + siteConfig.SiteID + " OR IsSystem=1) AND Status=1 ORDER BY ChannelID ASC");
            cblChannel.Items.Clear();
            foreach (var m in list)
            {
                cblChannel.Items.Add(new ListItem(m.ChannelName,m.ChannelID.ToString()));
            }
            //赋值频道
            for (int i = 0; i < cblChannel.Items.Count; i++)
            {
                cblChannel.Items[i].Selected = true;
            }
        }
        #endregion

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            system_site model = db.GetModel<system_site>(_id);
            /*网站基本信息*/
            ddlParentId.SelectedValue = model.ParentID.ToString();
            txtSortId.Text = model.OrderBy.ToString();
            if (model.Status == 0)
            {
                cbIsLock.Checked = true;
            }
            webcode.Text = model.SiteCode;
            webname.Text = model.SiteName;
            weburl.Text = model.DomainName;
            webname.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=site_validate_name&old_site_name=" + Utils.UrlEncode(model.SiteName));
            webcode.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=site_validate_code&old_site_code=" + Utils.UrlEncode(model.SiteCode));
            webname.Focus(); //设置焦点，防止JS无法提交
            weburl.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=site_validate_url&old_site_url=" + Utils.UrlEncode(model.DomainName));
            weblogo.Text = model.ThumbPath;
            webicp.Text = model.ICP;
            webfax.Text = model.Fax;
            webcopyright.Text = model.CopyRight;
            webcompany.Text = model.Company;
            webaddress.Text = model.Address;
            webmail.Text = model.Email;
            webtitle.Text = model.SiteTitle;
            webkeyword.Text = model.SiteKeywords;
            webdescription.Text = model.SiteDiscription;
            //关联表关联去除的频道
            var sitechannel = db.GetModelList<site_channel>(" SiteID="+siteConfig.SiteID);
            foreach (var s in sitechannel)
            {
                for (int i = 0; i < cblChannel.Items.Count; i++)
                {
                    int itemvalue = int.Parse(cblChannel.Items[i].Value);
                    if (s.ChannelID == itemvalue)
                    {
                        cblChannel.Items[i].Selected = false;
                        break;
                    }
                }
            }
            webclosereason.Text = model.CloseReason;
            webcountcode.Text = model.CountCode;
            /*网站基本信息*/

            /*模板设置*/
           
            template.SelectedIndex = template.Items.IndexOf(template.Items.FindByText(model.TemplatePath));
            if (!string.IsNullOrEmpty(model.TemplatePath))
            {
                DirectoryInfo fileDirectory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~" + model.TemplatePath.Trim()));
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
            indextemplate.SelectedValue = model.IndexTemplate;
            listtemplate.SelectedValue = model.ListTemplate;
            showtemplate.SelectedValue = model.DetailTemplate;
            /*模板设置*/

            /*水印设置*/
            watermarktype.SelectedIndex = watermarktype.Items.IndexOf(watermarktype.Items.FindByValue((model.WaterType??0).ToString()));
            watermarkposition.SelectedIndex = watermarkposition.Items.IndexOf(watermarkposition.Items.FindByValue((model.WaterPos ?? 0).ToString()));
            watermarkpic.Text = model.WaterImgUrl;
            watermarktransparency.Text = (model.WaterTransparency??0).ToString();
            watermarktext.Text = model.WaterText;
            watermarkfont.SelectedIndex = watermarkfont.Items.IndexOf(watermarkfont.Items.FindByText(model.WaterFont));
            watermarkfontsize.Text = (model.WaterFontSize ?? 0).ToString();
            /*水印设置*/
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            try
            {
                system_site model = new system_site();
                /*网站基本信息*/
                model.ParentID = int.Parse(ddlParentId.SelectedValue);
                model.SiteCode = webcode.Text;
                model.SiteName=webname.Text ;
                model.DomainName=weburl.Text;
                model.OrderBy = int.Parse(txtSortId.Text.Trim());
                model.Status = 1;
                if (cbIsLock.Checked == true)
                {
                    model.Status = 0;
                }
                model.AddTime = DateTime.Now;
                model.GUID = Guid.NewGuid().ToString();
                model.ThumbPath= weblogo.Text;
                model.ICP=webicp.Text ;
                model.Fax= webfax.Text;
                model.CopyRight=webcopyright.Text ;
                model.Company=webcompany.Text ;
                model.Address=webaddress.Text ;
                model.Email=webmail.Text;
                model.SiteTitle=webtitle.Text;
                model.SiteKeywords=webkeyword.Text;
                model.SiteDiscription=webdescription.Text;
                model.CloseReason= webclosereason.Text;
                model.CountCode = webcountcode.Text;
                /*网站基本信息*/

                /*模板设置*/
                model.TemplatePath = template.SelectedValue;
                model.IndexTemplate = indextemplate.SelectedValue;
                model.ListTemplate = listtemplate.SelectedValue;
                model.DetailTemplate = showtemplate.SelectedValue;
                /*模板设置*/

                /*水印设置*/
                model.WaterType =short.Parse(watermarktype.SelectedValue);
                model.WaterPos = short.Parse(watermarkposition.SelectedValue);
                model.WaterImgUrl=watermarkpic.Text;
                model.WaterTransparency=int.Parse(watermarktransparency.Text);
                model.WaterText=watermarktext.Text;
                model.WaterFont = watermarkfont.SelectedValue;
                model.WaterFontSize =int.Parse(watermarkfontsize.Text);
                /*水印设置*/
                long _id = db.InsertTree<system_site>(model, "SiteNode", "SiteLayer", "ParentID");
                if (_id > 0)
                {
                    //保存取消的频道
                    for (int i = 0; i < cblChannel.Items.Count; i++)
                    {
                        if (cblChannel.Items[i].Selected==false)
                        {
                            int itemvalue = int.Parse(cblChannel.Items[i].Value);
                            site_channel sc = new site_channel();
                            sc.ChannelID = itemvalue;
                            sc.SiteID = (int)_id;
                            sc.Insert();
                        }
                    }
                    AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加"+Name+"信息:" + model.SiteName); //记录日志
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            try
            {
                system_site model = db.GetModel<system_site>(_id);

                /*网站基本信息*/
                model.ParentID = int.Parse(ddlParentId.SelectedValue);
                model.SiteCode = webcode.Text;
                model.SiteName = webname.Text;
                model.DomainName = weburl.Text;
                model.OrderBy = int.Parse(txtSortId.Text.Trim());
                model.Status = 1;
                if (cbIsLock.Checked == true)
                {
                    model.Status = 0;
                }
                model.ThumbPath = weblogo.Text;
                model.ICP = webicp.Text;
                model.Fax = webfax.Text;
                model.CopyRight = webcopyright.Text;
                model.Company = webcompany.Text;
                model.Address = webaddress.Text;
                model.Email = webmail.Text;
                model.SiteTitle = webtitle.Text;
                model.SiteKeywords = webkeyword.Text;
                model.SiteDiscription = webdescription.Text;
                model.CloseReason = webclosereason.Text;
                model.CountCode = webcountcode.Text;
                /*网站基本信息*/

                /*模板设置*/
                model.TemplatePath = template.SelectedValue;
                model.IndexTemplate = indextemplate.SelectedValue;
                model.ListTemplate = listtemplate.SelectedValue;
                model.DetailTemplate = showtemplate.SelectedValue;
                /*模板设置*/

                /*水印设置*/
                model.WaterType = short.Parse(watermarktype.SelectedValue);
                model.WaterPos = short.Parse(watermarkposition.SelectedValue);
                model.WaterImgUrl = watermarkpic.Text;
                model.WaterTransparency = int.Parse(watermarktransparency.Text);
                model.WaterText = watermarktext.Text;
                model.WaterFont = watermarkfont.SelectedValue;
                model.WaterFontSize = int.Parse(watermarkfontsize.Text);
                /*水印设置*/

                if (db.UpdateTree<system_site>(model, "SiteNode", "SiteLayer", "ParentID") > 0)
                {
                    db.Delete<site_channel>(" SiteID="+_id);
                    //保存取消的频道
                    for (int i = 0; i < cblChannel.Items.Count; i++)
                    {
                        if (cblChannel.Items[i].Selected == false)
                        {
                            int itemvalue = int.Parse(cblChannel.Items[i].Value);
                            site_channel sc = new site_channel();
                            sc.ChannelID = itemvalue;
                            sc.SiteID = (int)_id;
                            sc.Insert();
                        }
                    }
                    AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改"+Name+"信息:" + model.SiteName); //记录日志
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
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
                JscriptMsg("修改站点成功！", ListName, "Success", "parent.loadMenuTree");
            }
            else //添加
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.Add); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", "", "Error");
                    return;
                }
                JscriptMsg("添加站点成功！", ListName, "Success", "parent.loadMenuTree");
            }
        }
        /// <summary>
        /// 递归树表，按层级关系，输出列表
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        private List<system_module> GetTreeList(int siteid, int parentid = 0)
        {
            List<system_module> list = new List<system_module>();
            string sql = "";
            if (parentid == 0)
            {
                sql = "  ParentID=" + parentid + " AND (IsSystem=1 OR SiteID=" + siteid + ") ORDER BY OrderBy ASC ";
            }
            else
            {
                sql = "  ParentID=" + parentid + " AND SiteID=" + siteid + " ORDER BY OrderBy ASC ";
            }
            var mlist = db.GetModelList<system_module>(sql);
            foreach (var m in mlist)
            {
                if (db.GetCount<system_module>(" ParentID=" + m.ModuleID + " AND SiteID=" + siteid + "") > 0)
                {
                    list.Add(m);
                    var childlist = GetTreeList(siteid, m.ModuleID);
                    foreach (var c in childlist)
                    {
                        list.Add(c);
                    }
                }
                else
                {
                    list.Add(m);
                }
            }
            return list;

        }

    }
}