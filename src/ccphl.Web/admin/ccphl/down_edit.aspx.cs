using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;

namespace ccphl.Web.admin.ccphl
{
    public partial class down_edit : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "为民服务站下载";
        //列表页面名称
        public const string ListName = "down_list.aspx";
        //编辑页面名称
        public const string EditName = "down_edit.aspx";

        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");
            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                if (!int.TryParse(Request.QueryString["id"] as string, out this.id))
                {
                    JscriptMsg("传输参数不正确！", "", "Error");
                    return;
                }
                if (!db.IsExists<download>(this.id))
                {
                    JscriptMsg("记录不存在或已被删除！", "", "Error");
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
            }
        }

       

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {

            download model = db.GetModel<download>(_id);
            txtpath.Text = model.FilePath;
            thumb.Text = model.Thumb;
            txtfile.Text = model.FileName;
            filename.Value = model.FileName;
            filesize.Value = model.FileSize.ToString();
            fileext.Value = model.FileExt;
            txtRemark.Text = model.Description;
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            download model = new download(); 
          
            model.GUID = Guid.NewGuid().ToString();


            model.Thumb = thumb.Text;
            model.FilePath = txtpath.Text;
            if (txtfile.Text.Trim() != "")
            {
                model.FileName = txtfile.Text;
            }
            else
            {
                model.FileName = filename.Value;
            }
            model.FileSize=Utils.StrToInt( filesize.Value,0);
            model.FileExt= fileext.Value ;
            model.AddTime = DateTime.Now;
            model.Description = txtRemark.Text;
            long id = db.Insert<download>(model);
            if (id > 0)
            {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加"+Name+":" + model.FileName); //记录日志
                return true;
            }
            return false;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            bool result = false;
            download model = db.GetModel<download>(_id);
            if (model == null)
            {
                return false;
            }
            model.Thumb = thumb.Text;
            model.FilePath = txtpath.Text;
            if (txtfile.Text.Trim() != "")
            {
                model.FileName = txtfile.Text;
            }
            else
            {
                model.FileName = filename.Value;
            }
            model.FileSize = Utils.StrToInt(filesize.Value, 0);
            model.FileExt = fileext.Value;
            model.AddTime = DateTime.Now;
            model.Description = txtRemark.Text;

            if (db.Update<download>(model) > 0)
            {
               
                AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改" + Name + ":" + model.FileName); //记录日志
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