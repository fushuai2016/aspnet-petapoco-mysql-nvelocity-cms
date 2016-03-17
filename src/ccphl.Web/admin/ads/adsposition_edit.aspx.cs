using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;
using System.IO;

namespace ccphl.Web.admin.ads
{
    public partial class adsposition_edit : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "广告位置";
        //列表页面名称
        public const string ListName = "adsposition_list.aspx";
        //编辑页面名称
        public const string EditName = "adsposition_edit.aspx";
        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型

        public int siteid;
        private int id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");

            this.siteid = DTRequest.GetQueryInt("siteid");
            this.id = DTRequest.GetQueryInt("id");
            if (this.siteid == 0)
            {
                JscriptMsg("传输参数不正确！", ListName, "Error");
                return;
            }

            this.sid.Value = siteid.ToString();
            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", ListName, "Error");
                    return;
                }
                if (!db.IsExists<Model.adsposition>(this.id))
                {
                    JscriptMsg("记录不存在或已删除！", ListName, "Error");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.View); //检查权限
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                    TreeBind(this.siteid);  
                }
                else
                {
                    TreeBind(this.siteid);
                    txtCallIndex.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=adsposition_validate_code&siteid=" + this.siteid);
                    
                    if (this.id > 0)
                    {
                        this.ddlParentId.SelectedValue = this.id.ToString();
                    }
                }
            }
        }

        #region 绑定类别=================================
        
        private void TreeBind(int site_id)
        {

            this.ddlParentId.Items.Clear();
            this.ddlParentId.Items.Add(new ListItem("无父级位置", "0"));
            var cols = GetAdsPosTreeList(site_id);
            foreach (var s in cols)
            {
                string Id = s.AdsPositionID.ToString();
                int ClassLayer = s.PositionLayer ?? 1;
                string Title = s.PositionName;

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
            Model.adsposition model = db.GetModel<Model.adsposition>(_id);

            txtTitle.Text = model.PositionName;
            txtCallIndex.Text = model.PositionCode;
            txtTitle.Focus(); //设置焦点，防止JS无法提交
            txtCallIndex.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=adsposition_validate_code&old_position_code=" + Utils.UrlEncode(model.PositionCode) + "&siteid=" + this.siteid);

            ddlParentId.SelectedValue = model.ParentID.ToString();
            if (model.Status == 0)
            {
                cbStatus.Checked = true;
            }
            
            txtSortId.Text = model.OrderBy.ToString();
            
            txtdescription.Text = model.Description;
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            Model.adsposition model = new Model.adsposition();

            model.GUID = Guid.NewGuid().ToString();
            model.SiteID = Utils.StrToInt(this.sid.Value, 0);

            model.ParentID = Utils.StrToInt(ddlParentId.SelectedValue, 0);
            model.PositionName = txtTitle.Text.Trim();
            model.PositionCode = txtCallIndex.Text.Trim();

            model.Status = 1;

            if (cbStatus.Checked == true)
            {
                model.Status = 0;
            }
            model.OrderBy = Utils.StrToInt(txtSortId.Text.Trim(), 99);

            model.AddTime = DateTime.Now;
           
            model.Description = txtdescription.Text.Trim();

            long _id = db.InsertTree<adsposition>(model, "PositionNode", "PositionLayer", "ParentID");
             if (_id <= 0)
            {
                return false;
            }

            AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加" + Name + model.PositionName); //记录日志
            return true;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            Model.adsposition model = db.GetModel<Model.adsposition>(_id);

            model.SiteID = Utils.StrToInt(this.sid.Value, 0);
            model.ParentID = Utils.StrToInt(ddlParentId.SelectedValue, 0);
            model.PositionName = txtTitle.Text.Trim();
            model.PositionCode = txtCallIndex.Text.Trim();
            model.Status = 1;
            if (cbStatus.Checked == true)
            {
                model.Status = 0;
            }
            model.OrderBy = Utils.StrToInt(txtSortId.Text.Trim(), 99);
            model.Description = txtdescription.Text.Trim();
            if (db.UpdateTree<adsposition>(model, "PositionNode", "PositionLayer", "ParentID") > 0)
            {
                return true;
            }


            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改" + Name + model.PositionName); //记录日志
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
                JscriptMsg("修改广告位置信息成功！", ListName, "Success", "parent.loadMenuTree");
            }
            else //添加
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.Add); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", "", "Error");
                    return;
                }
                JscriptMsg("添加广告位置信息成功！", ListName, "Success", "parent.loadMenuTree");
            }
        }

    }
}