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
    public partial class manager_edit : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "管理员";
        //列表页面名称
        public const string ListName = "manager_list.aspx";
        //编辑页面名称
        public const string EditName = "manager_edit.aspx";

        string defaultpassword = "0|0|0|0"; //默认显示密码
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
                if (!db.IsExists<system_user>(this.id))
                {
                    JscriptMsg("记录不存在或已被删除！", "", "Error");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.View); //检查权限
                ddlUserType.Items.Add(new ListItem("普通用户", "0"));
                ddlUserType.Items.Add(new ListItem("高级用户", "1"));
                ddlUserType.Items.Add(new ListItem("站点管理员", "2"));
                ddlUserType.Items.Add(new ListItem("超级管理员", "3"));
                system_user model = GetAdminInfo(); //取得管理员信息
                RoleBind();
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
            }
        }

        #region 角色类型=================================
        private void RoleBind()
        {
            ddlRoleId.Items.Clear();
            var list = db.GetModelList<system_role>(" Status=1 AND SiteID="+siteConfig.SiteID );
            foreach (var r in list)
            {
                ddlRoleId.Items.Add(new ListItem(r.RoleName, r.RoleID.ToString()));
            }
          
        }
        #endregion

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {

            system_user model = db.GetModel<system_user>(_id);
            var rolelist = db.GetModelList<system_role_user>(" UserID=" + _id);
            for (int i = 0; i < ddlRoleId.Items.Count; i++)
            { 
                long itemvalue = long.Parse(ddlRoleId.Items[i].Value);
                foreach (var r in rolelist)
                {
                    if (r.RoleID == itemvalue)
                    {
                        ddlRoleId.Items[i].Selected = true;
                        break;
                    }
                }
            }
            ddlUserType.SelectedValue = model.UserType.ToString();
            if (model.Status == 0)
            {
                cbIsLock.Checked = false;
            }
            else
            {
                cbIsLock.Checked = true;
            }
            txtUserName.Text = model.UserName;
            txtUserName.ReadOnly = true;
            txtUserName.Attributes.Remove("ajaxurl");
            if (!string.IsNullOrEmpty(model.Password))
            {
                txtPassword.Attributes["value"] = txtPassword1.Attributes["value"] = defaultpassword;
            }
            txtRealName.Text = model.RealName;
            txtTelephone.Text = model.Mobile;
            txtEmail.Text = model.Email;
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            system_user model = new system_user() ; 
            //检测用户名是否重复
            if (db.IsExists<system_user>(" UserName='" + txtUserName.Text.Trim()+"'"))
            {
                return false;
            }
            model.GUID = Guid.NewGuid().ToString();
            model.SiteID = siteConfig.SiteID;
            model.SiteNode = siteConfig.SiteNode;
            model.UserType = int.Parse(ddlUserType.SelectedValue);
            if (cbIsLock.Checked == true)
            {
                model.Status =1;
            }
            else
            {
                model.Status = 0;
            }
           
            model.UserName = txtUserName.Text.Trim();
            //获得6位的salt加密字符串
            model.Salt = Utils.GetCheckCode(6);
            //以随机生成的6位字符串做为密钥加密
            model.Password = DESEncrypt.Encrypt(txtPassword.Text.Trim(), model.Salt);
           
            model.RealName = txtRealName.Text.Trim();
            model.Mobile = txtTelephone.Text.Trim();
            model.Email = txtEmail.Text.Trim();
            model.AddTime = DateTime.Now;
            long id = db.Insert<system_user>(model);
            if (id > 0)
            {
                for (int i = 0; i < ddlRoleId.Items.Count; i++)
                {
                    if (ddlRoleId.Items[i].Selected)
                    {
                        int itemvalue = int.Parse(ddlRoleId.Items[i].Value);
                        system_role_user role = new system_role_user();
                        role.RoleID = itemvalue;
                        role.UserID =Convert.ToInt32( id);
                        role.Insert();
                    }  
                }
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加"+Name+":" + model.UserName); //记录日志
                return true;
            }
            return false;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            bool result = false;
            system_user model = db.GetModel<system_user>(_id);
            if (model == null)
            {
                return false;
            }
            model.UserType = int.Parse(ddlUserType.SelectedValue);
            if (cbIsLock.Checked == true)
            {
                model.Status = 1;
            }
            else
            {
                model.Status = 0;
            }

            model.UserName = txtUserName.Text.Trim();
            if (txtPassword.Text.Trim() != defaultpassword)
            {
                //获取用户已生成的salt作为密钥加密
                model.Password = DESEncrypt.Encrypt(txtPassword.Text.Trim(), model.Salt);
            }

            model.RealName = txtRealName.Text.Trim();
            model.Mobile = txtTelephone.Text.Trim();
            model.Email = txtEmail.Text.Trim();
           

            if (db.Update<system_user>(model)>0)
            {
                db.Delete<system_role_user>(" UserID='" + _id + "'");
                for (int i = 0; i < ddlRoleId.Items.Count; i++)
                {
                    if (ddlRoleId.Items[i].Selected)
                    {
                        int itemvalue = int.Parse(ddlRoleId.Items[i].Value);
                        system_role_user role = new system_role_user();
                        role.RoleID = itemvalue;
                        role.UserID = Convert.ToInt32(id);
                        role.Insert();
                    }
                }
                AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改" + Name + ":" + model.UserName); //记录日志
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
                JscriptMsg("修改管理员信息成功！",ListName, "Success");
            }
            else //添加
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.Add); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", "", "Error");
                    return;
                }
                JscriptMsg("添加管理员信息成功！",ListName, "Success");
            }
        }
    }
}