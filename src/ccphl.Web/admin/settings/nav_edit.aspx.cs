using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;

namespace ccphl.Web.admin.settings
{
    public partial class nav_edit : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "导航";
        //列表页面名称
        public const string ListName = "nav_list.aspx";
        //编辑页面名称
        public const string EditName = "nav_edit.aspx";

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
                if (!db.IsExists<system_module>(this.id))
                {
                    JscriptMsg("导航不存在或已被删除！", "", "Error");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.View); //检查权限
                TreeBind(DTEnums.NavigationEnum.System.ToString()); //绑定导航菜单
                ActionTypeBind(); //绑定操作权限类型
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
                    txtName.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=navigation_validate");
                }
            }
        }

        #region 绑定导航菜单=============================
        private void TreeBind(string nav_type)
        {
            List<system_module> list = GetTreeList(siteConfig.SiteID,0);

            this.ddlParentId.Items.Clear();
            this.ddlParentId.Items.Add(new ListItem("无父级导航", "0"));
            foreach(var m in list)
            {
                string Id = m.ModuleID.ToString();
                int ClassLayer = m.ModuleLayer??1;
                string Title = m.ModuleName;

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

        #region 绑定操作权限类型=========================
        private void ActionTypeBind()
        {
            cblActionType.Items.Clear();
            foreach (KeyValuePair<string, int> kvp in Utils.ActionType())
            {
                cblActionType.Items.Add(new ListItem(kvp.Key + "(" + kvp.Value + ")", kvp.Value.ToString()));
            }
        }
        #endregion

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            system_module model = db.GetModel<system_module>(_id);

            ddlParentId.SelectedValue = model.ParentID.ToString();
            txtSortId.Text = model.OrderBy.ToString();
            if (model.Status == 0)
            {
                cbIsLock.Checked = true;
            }
            txtName.Text = model.ModuleCode;
            txtName.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=navigation_validate&old_name=" + Utils.UrlEncode(model.ModuleCode));
            txtName.Focus(); //设置焦点，防止JS无法提交
            if (model.IsSystem == 1)
            {
                ddlParentId.Enabled = false;
                txtName.ReadOnly = true;
            }
            txtTitle.Text = model.ModuleName;
            txtLinkUrl.Text = model.ModuleUrl;
            txtIcon.Text = model.Icon;
            txtRemark.Text = model.Remark;
            //赋值操作权限类型
            long actionValue = model.ButtonAuthority??0;
            for (int i = 0; i < cblActionType.Items.Count; i++)
            {
                long itemvalue = long.Parse(cblActionType.Items[i].Value);
                if ((actionValue & itemvalue) == itemvalue)
                {
                    cblActionType.Items[i].Selected = true;
                }
            }
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            try
            {
                system_module model = new system_module();
                model.ModuleCode = txtName.Text.Trim();
                model.ModuleName = txtTitle.Text.Trim();
                model.ModuleUrl = txtLinkUrl.Text.Trim();
                model.OrderBy = int.Parse(txtSortId.Text.Trim());
                model.Icon = txtIcon.Text.Trim();
                model.Status = 1;
                if (cbIsLock.Checked == true)
                {
                    model.Status = 0;
                }
                model.Remark = txtRemark.Text.Trim();
                model.ParentID = int.Parse(ddlParentId.SelectedValue);

                model.IsSystem = 0;
                model.AddTime = DateTime.Now;
                model.GUID = Guid.NewGuid().ToString();
                model.SiteID = siteConfig.SiteID;
                model.SiteNode = siteConfig.SiteNode;

                //添加操作权限类型
                long action_type =0;
                for (int i = 0; i < cblActionType.Items.Count; i++)
                {
                    if (cblActionType.Items[i].Selected)
                    {
                        long itemvalue = long.Parse(cblActionType.Items[i].Value);
                        action_type += itemvalue;
                    }
                }
                model.ButtonAuthority = action_type;

                if (db.InsertTree<system_module>(model,"ModuleNode","ModuleLayer","ParentID") > 0)
                {
                    AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加"+Name+"信息:" + model.ModuleName); //记录日志
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
                system_module model = db.GetModel<system_module>(_id);

                model.ModuleCode = txtName.Text.Trim();
                model.ModuleName = txtTitle.Text.Trim();
                model.ModuleUrl = txtLinkUrl.Text.Trim();
                model.OrderBy = int.Parse(txtSortId.Text.Trim());
                model.Icon = txtIcon.Text.Trim();
                model.Status = 1;
                if (cbIsLock.Checked == true)
                {
                    model.Status = 0;
                }
                model.Remark = txtRemark.Text.Trim();
                model.ParentID = int.Parse(ddlParentId.SelectedValue);
                if (model.IsSystem == 0)
                {
                    int parentId = int.Parse(ddlParentId.SelectedValue);
                    //如果选择的父ID不是自己,则更改
                    if (parentId != model.ModuleID)
                    {
                        model.ParentID = parentId;
                    }
                }

                //添加操作权限类型
                long action_type = 0;
                for (int i = 0; i < cblActionType.Items.Count; i++)
                {
                    if (cblActionType.Items[i].Selected)
                    {
                        long itemvalue = long.Parse(cblActionType.Items[i].Value);
                        action_type += itemvalue;
                    }
                }
                model.ButtonAuthority = action_type;

                if (db.UpdateTree<system_module>(model,"ModuleNode","ModuleLayer","ParentID")>0)
                {
                    AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改导航信息:" + model.ModuleName); //记录日志
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
                JscriptMsg("修改导航菜单成功！", ListName, "Success", "parent.loadMenuTree");
            }
            else //添加
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.Add); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", "", "Error");
                    return;
                }
                JscriptMsg("添加导航菜单成功！", ListName, "Success", "parent.loadMenuTree");
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