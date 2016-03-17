using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;
using ccphl.Entity;

namespace ccphl.Web.admin.settings
{
    public partial class img_edit : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "图片";
        //列表页面名称
        public const string ListName = "img_list.aspx";
        //编辑页面名称
        public const string EditName = "img_edit.aspx";
        XmlConfig<ImageConfig> xc = new XmlConfig<ImageConfig>();
        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private string id = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");

            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                this.id = DTRequest.GetQueryString("id");
                if (this.id == "")
                {
                    JscriptMsg("传输参数不正确！", "", "Error");
                    return;
                }
                List<ImageConfig> list = xc.loadImgListConfig();
                if (list.Count(p => p.imgcode == this.id)==0)
                {
                    JscriptMsg("记录不存在或已删除！", "", "Error");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.View); //检查权限
              
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
                else
                {
                    txtCode.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=img_validate_code");
                }
            }
        }

        #region 赋值操作=================================
        private void ShowInfo(string _id)
        {
            List<ImageConfig> list = xc.loadImgListConfig();

            ImageConfig model = list.Where(p => p.imgcode == _id).SingleOrDefault();

            txtCode.Text = model.imgcode;
            txtName.Text = model.imgname;
            txtCode.Focus(); //设置焦点，防止JS无法提交
            txtCode.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=img_validate_code&old_img_code=" + Utils.UrlEncode(model.imgcode));
            if (model.imgstatus == 1)
            {
                cbIsLock.Checked = true;
            }
            txtwidth.Text = model.imgwidthshare.ToString();
            txtheight.Text = model.imgheightshare.ToString();
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            ImageConfig model = new ImageConfig();
            List<ImageConfig> list = xc.loadImgListConfig();
            model.imgcode=txtCode.Text;
            model.imgname=txtName.Text;
            model.imgwidthshare=int.Parse(txtwidth.Text);
             model.imgheightshare=int.Parse(txtheight.Text);
             if (cbIsLock.Checked == true)
             {
                 model.imgstatus = 1;
             }
             else
             {
                 model.imgstatus = 0;
             }
            list.Add(model);
            List<ImageConfig> lis = xc.saveImgListConifg(list);

            AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加"+Name + model.imgname); //记录日志
            return true;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(string _id)
        {
            List<ImageConfig> list = xc.loadImgListConfig();
            ImageConfig model = list.Where(p => p.imgcode == _id).SingleOrDefault();
            list = list.Where(p => p.imgcode != _id).ToList();
            model.imgcode = txtCode.Text;
            model.imgname = txtName.Text;
            model.imgwidthshare = int.Parse(txtwidth.Text);
            model.imgheightshare = int.Parse(txtheight.Text);
            if (cbIsLock.Checked == true)
            {
                model.imgstatus = 1;
            }
            else
            {
                model.imgstatus = 0;
            }
            list.Add(model);
            List<ImageConfig> lis = xc.saveImgListConifg(list);
           

            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改"+ Name+ model.imgname); //记录日志
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