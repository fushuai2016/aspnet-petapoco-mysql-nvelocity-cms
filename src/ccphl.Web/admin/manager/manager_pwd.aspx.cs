using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;

namespace ccphl.Web.admin.manager
{
    public partial class manager_pwd : Web.UI.ManagePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                system_user model = GetAdminInfo();
                ShowInfo(model);
            }
        }

        #region 赋值操作=================================
        private void ShowInfo(system_user model)
        {
            txtUserName.Text = model.UserName;
            txtRealName.Text = model.RealName;
            txtTelephone.Text = model.Mobile;
            txtEmail.Text = model.Email;
        }
        #endregion

        //保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            system_user m= GetAdminInfo();
            system_user model = db.GetModel<system_user>(m.UserID);
            if (DESEncrypt.Encrypt(txtOldPassword.Text.Trim(), model.Salt) != model.Password)
            {
                JscriptMsg("旧密码不正确！", "", "Warning");
                return;
            }
            if (txtPassword.Text.Trim() != txtPassword1.Text.Trim())
            {
                JscriptMsg("两次密码不一致！", "", "Warning");
                return;
            }
            model.Password = DESEncrypt.Encrypt(txtPassword.Text.Trim(), model.Salt);
            if (txtRealName.Text.Trim() != "")
                model.RealName = txtRealName.Text.Trim();
            if (txtTelephone.Text.Trim() != "")
                model.Mobile = txtTelephone.Text.Trim();
            if (txtEmail.Text.Trim() != "")
                model.Email = txtEmail.Text.Trim();

            if (model.Update()<=0)
            {
                JscriptMsg("保存过程中发生错误！", "", "Error");
                return;
            }
            Session[DTKeys.SESSION_ADMIN_INFO] = null;
            JscriptMsg("密码修改成功！", "manager_pwd.aspx", "Success");
        }
    }
}