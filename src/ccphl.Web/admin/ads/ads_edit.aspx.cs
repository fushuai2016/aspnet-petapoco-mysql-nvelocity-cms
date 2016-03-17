using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;

namespace ccphl.Web.admin.ads
{
    public partial class ads_edit : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "广告";
        //列表页面名称
        public const string ListName = "ads_list.aspx";
        //编辑页面名称
        public const string EditName = "ads_edit.aspx";

        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;
        public int siteid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");
            int siteid = DTRequest.GetQueryInt("siteid");
            if (siteid==0)
            {
                JscriptMsg("传输参数不正确！", "", "Error");
                return;
            }
            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                if (!int.TryParse(Request.QueryString["id"] as string, out this.id))
                {
                    JscriptMsg("传输参数不正确！", "", "Error");
                    return;
                }
                if (!db.IsExists<ad>(this.id))
                {
                    JscriptMsg("记录不存在或已被删除！", "", "Error");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.View); //检查权限
                TreeBind();
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
            }
        }
        #region 绑定位置=================================
        private void TreeBind()
        {
            this.ddlParentId.Items.Clear();
            this.ddlParentId.Items.Add(new ListItem("请选择位置", ""));
            var site = GetAdsPosTreeList(siteConfig.SiteID);
            foreach (var s in site)
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

            ad model = db.GetModel<ad>(_id);
            this.ddlParentId.SelectedValue = model.AdsPositionID.ToString();
            if (model.Status == 0)
            {
                cbStatus.Checked = true;
            }
            txtTitle.Text = model.AdsName;
            txtLinkUrl.Text = model.LinkUrl;
            thumb.Text = model.ImagePath;
            txtSortId.Text = model.OrderBy.ToString();
           
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            ad model = new ad(); 
          
            model.GUID = Guid.NewGuid().ToString();
            model.AdsPositionID = Convert.ToInt32(this.ddlParentId.SelectedValue);
            model.SiteID = this.siteid;
            model.ImagePath = thumb.Text.Trim();
            model.AdsName = txtTitle.Text.Trim();
            model.LinkUrl = txtLinkUrl.Text.Trim();
            model.Status = 1;

            if (cbStatus.Checked == true)
            {
                model.Status = 0;
            }
            model.OrderBy = Convert.ToInt32(this.txtSortId.Text);
            model.AddTime = DateTime.Now;
            model.Click = 0;
            long id = db.Insert<ad>(model);
            if (id > 0)
            {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加"+Name+":" + model.AdsName); //记录日志
                return true;
            }
            return false;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            bool result = false;
            ad model = db.GetModel<ad>(_id);
            model.AdsPositionID = Convert.ToInt32(this.ddlParentId.SelectedValue);
            model.SiteID = this.siteid;
            model.ImagePath = thumb.Text.Trim();
            model.AdsName = txtTitle.Text.Trim();
            model.LinkUrl = txtLinkUrl.Text.Trim();
            model.Status = 1;

            if (cbStatus.Checked == true)
            {
                model.Status = 0;
            }
            model.OrderBy = Convert.ToInt32(this.txtSortId.Text);

            if (db.Update<ad>(model) > 0)
            {

                AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改" + Name + ":" + model.AdsName); //记录日志
                result = true;
            }

            return result;
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
                JscriptMsg("修改"+Name+"信息成功！",ListName, "Success");
            }
            else //添加
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.Add); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", "", "Error");
                    return;
                }
                JscriptMsg("添加"+Name+"信息成功！",ListName, "Success");
            }
        }
    }
}