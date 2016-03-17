using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;

namespace ccphl.Web.admin.manager
{
    public partial class role_edit : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "管理角色";
        //列表页面名称
        public const string ListName = "role_list.aspx";
        //编辑页面名称
        public const string EditName = "role_edit.aspx";

        public system_site site;

        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");
            this.id = DTRequest.GetQueryInt("id");
            site = siteConfig;
            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", "", "Error");
                    return;
                }
                if (!db.IsExists<system_role>(this.id))
                {
                    JscriptMsg("角色不存在或已被删除！", "", "Error");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.View); //检查权限
               
                NavBind(); //绑定导航
                WebBind();
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
            }
        }

        #region 导航菜单=================================
        private void NavBind()
        {
            this.rptList.DataSource=GetModelTreeList(0);
            this.rptList.DataBind();
        }
        #endregion
        #region 网站内容=================================
        private void WebBind()
        {
            this.rptList2.DataSource = GetWebTreeList(siteConfig.SiteID);
            this.rptList2.DataBind();
        }
        #endregion

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            system_role model = db.GetModel<system_role>(_id);
            txtRoleName.Text = model.RoleName;
            if (model.Status == 1)
            {
                cbIsLock.Checked = true;
            }
            else
            {
                cbIsLock.Checked = false;
            }
            if (model.RoleID>0)
            {
                //系统权限
                for (int i = 0; i < rptList.Items.Count; i++)
                {
                    string navId = ((HiddenField)rptList.Items[i].FindControl("hidId")).Value;
                    long actionType = Convert.ToInt64(((HiddenField)rptList.Items[i].FindControl("hidActionType")).Value);
                    var rolemb = db.GetModel<system_role_module_button>(" ModuleID=" + navId+" AND RoleID="+_id);
                    if (rolemb.ButtonAuthority > 0)
                    {
                        CheckBoxList cblActionType = (CheckBoxList)rptList.Items[i].FindControl("cblActionType");
                        for (int n = 0; n < cblActionType.Items.Count; n++)
                        {
                            long cblvalue = Convert.ToInt64(cblActionType.Items[n].Value);
                            if ((cblvalue & rolemb.ButtonAuthority) == cblvalue)
                            {
                                cblActionType.Items[n].Selected = true;
                            }
                        }
                    }
                }
                //网站权限
                for (int i = 0; i < rptList2.Items.Count; i++)
                {
                    string chid = ((HiddenField)rptList2.Items[i].FindControl("hidId")).Value;
                    string cid = ((HiddenField)rptList2.Items[i].FindControl("hidColumnId")).Value;
                    long actionType = Convert.ToInt64(((HiddenField)rptList2.Items[i].FindControl("hidActionType")).Value);
                    var rolemb = db.GetModel<system_web_column>(" SiteID=" + site.SiteID + " AND ChannelID=" + chid + " AND ColumnType=" + cid + " AND RoleID=" + _id);
                    if (rolemb.ButtonAuthority > 0)
                    {
                        CheckBoxList cblActionType = (CheckBoxList)rptList2.Items[i].FindControl("cblActionType");
                        for (int n = 0; n < cblActionType.Items.Count; n++)
                        {
                            long cblvalue = Convert.ToInt64(cblActionType.Items[n].Value);
                            if ((cblvalue & rolemb.ButtonAuthority) == cblvalue)
                            {
                                cblActionType.Items[n].Selected = true;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            bool result = false;
            system_role model = new system_role();
            model.GUID = Guid.NewGuid().ToString();
            model.SiteID = siteConfig.SiteID;
            model.SiteNode = siteConfig.SiteNode;
            model.AddTime = DateTime.Now;
            model.RoleName = txtRoleName.Text;
            if (cbIsLock.Checked)
            {
                model.Status= 1;
            }
            else
            {
                model.Status = 0;
            }
            long id = db.Insert<system_role>(model);
            if (id > 0)
            {
                //系统权限
                for (int i = 0; i < rptList.Items.Count; i++)
                {
                    string navId = ((HiddenField)rptList.Items[i].FindControl("hidId")).Value;
                    CheckBoxList cblActionType = (CheckBoxList)rptList.Items[i].FindControl("cblActionType");
                    long buttonvalue = 0;
                    for (int n = 0; n < cblActionType.Items.Count; n++)
                    {
                        if (cblActionType.Items[n].Selected == true)
                        {
                            buttonvalue += Convert.ToInt64(cblActionType.Items[n].Value);
                        }
                    }
                    system_role_module_button srmb = new system_role_module_button();
                    srmb.RoleID = Convert.ToInt32(id);
                    srmb.ModuleID = Convert.ToInt32(navId);
                    srmb.ButtonAuthority = buttonvalue;
                    srmb.Insert();
                }
                //网站权限
                for (int i = 0; i < rptList2.Items.Count; i++)
                {
                    string chid = ((HiddenField)rptList2.Items[i].FindControl("hidId")).Value;
                    string cid = ((HiddenField)rptList2.Items[i].FindControl("hidColumnId")).Value;
                    CheckBoxList cblActionType = (CheckBoxList)rptList2.Items[i].FindControl("cblActionType");
                    long buttonvalue = 0;
                    for (int n = 0; n < cblActionType.Items.Count; n++)
                    {
                        if (cblActionType.Items[n].Selected == true)
                        {
                            buttonvalue += Convert.ToInt64(cblActionType.Items[n].Value);
                        }
                    }
                    system_web_column srmb = new system_web_column();
                    srmb.SiteID = site.SiteID;
                    srmb.RoleID = Convert.ToInt32(id);
                    srmb.ChannelID = Convert.ToInt32(chid);
                    srmb.ColumnType = Convert.ToInt32(cid);
                    srmb.ButtonAuthority = buttonvalue;
                    srmb.Insert();
                }
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加"+Name+":" + model.RoleName); //记录日志
                result = true;
            }
            return result;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            bool result = false;
            system_role model = db.GetModel<system_role>(_id);
            model.RoleName = txtRoleName.Text;
            if (cbIsLock.Checked)
            {
                model.Status = 1;
            }
            else
            {
                model.Status = 0;
            }
            if (db.Update<system_role>(model)>0)
            {
                //系统权限
                db.Delete<system_role_module_button>(" RoleID="+_id);
                for (int i = 0; i < rptList.Items.Count; i++)
                {
                    string navId = ((HiddenField)rptList.Items[i].FindControl("hidId")).Value;
                    CheckBoxList cblActionType = (CheckBoxList)rptList.Items[i].FindControl("cblActionType");
                    long buttonvalue = 0;
                    for (int n = 0; n < cblActionType.Items.Count; n++)
                    {
                        if (cblActionType.Items[n].Selected == true)
                        {
                            buttonvalue += Convert.ToInt64(cblActionType.Items[n].Value);
                        }
                    }
                    system_role_module_button srmb = new system_role_module_button();
                    srmb.RoleID = Convert.ToInt32(id);
                    srmb.ModuleID = Convert.ToInt32(navId);
                    srmb.ButtonAuthority = buttonvalue;
                    srmb.Insert();
                }
                //网站权限
                db.Delete<system_web_column>(" RoleID=" + _id);
                for (int i = 0; i < rptList2.Items.Count; i++)
                {
                    string chid = ((HiddenField)rptList2.Items[i].FindControl("hidId")).Value;
                    string cid = ((HiddenField)rptList2.Items[i].FindControl("hidColumnId")).Value;
                    CheckBoxList cblActionType = (CheckBoxList)rptList2.Items[i].FindControl("cblActionType");
                    long buttonvalue = 0;
                    for (int n = 0; n < cblActionType.Items.Count; n++)
                    {
                        if (cblActionType.Items[n].Selected == true)
                        {
                            buttonvalue += Convert.ToInt64(cblActionType.Items[n].Value);
                        }
                    }
                    system_web_column srmb = new system_web_column();
                    srmb.SiteID = site.SiteID;
                    srmb.RoleID = Convert.ToInt32(id);
                    srmb.ChannelID = Convert.ToInt32(chid);
                    srmb.ColumnType = Convert.ToInt32(cid);
                    srmb.ButtonAuthority = buttonvalue;
                    srmb.Insert();
                }
                AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改"+Name+":" + model.RoleName); //记录日志
                result = true;
            }
            return result;
        }
        #endregion

        //美化列表
        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                //美化导航树结构
                Literal LitFirst = (Literal)e.Item.FindControl("LitFirst");
                HiddenField hidLayer = (HiddenField)e.Item.FindControl("hidLayer");
                string LitStyle = "<span style=\"display:inline-block;width:{0}px;\"></span>{1}{2}";
                string LitImg1 = "<span class=\"folder-open\"></span>";
                string LitImg2 = "<span class=\"folder-line\"></span>";

                int classLayer = Convert.ToInt32(hidLayer.Value);
                if (classLayer == 1)
                {
                    LitFirst.Text = LitImg1;
                }
                else
                {
                    LitFirst.Text = string.Format(LitStyle, (classLayer - 2) * 24, LitImg2, LitImg1);
                }
                //绑定导航权限资源
                long actionType =Convert.ToInt64( ((HiddenField)e.Item.FindControl("hidActionType")).Value);
                CheckBoxList cblActionType = (CheckBoxList)e.Item.FindControl("cblActionType");
                cblActionType.Items.Clear();
                foreach(KeyValuePair<string, int> kvp in Utils.ActionType())
                {
                    if ((kvp.Value & actionType) == kvp.Value)
                    {
                        cblActionType.Items.Add(new ListItem(kvp.Key, kvp.Value.ToString()));
                    }
                }
            }
        }
        //美化列表2
        protected void rptList2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                //美化导航树结构
                Literal LitFirst = (Literal)e.Item.FindControl("LitFirst");
                HiddenField hidLayer = (HiddenField)e.Item.FindControl("hidLayer");
                string LitStyle = "<span style=\"display:inline-block;width:{0}px;\"></span>{1}{2}";
                string LitImg1 = "<span class=\"folder-open\"></span>";
                string LitImg2 = "<span class=\"folder-line\"></span>";

                int classLayer = Convert.ToInt32(hidLayer.Value);
                if (classLayer == 1)
                {
                    LitFirst.Text = LitImg1;
                }
                else
                {
                    LitFirst.Text = string.Format(LitStyle, (classLayer - 2) * 24, LitImg2, LitImg1);
                }
                //绑定导航权限资源
                long actionType = Convert.ToInt64(((HiddenField)e.Item.FindControl("hidActionType")).Value);
                CheckBoxList cblActionType = (CheckBoxList)e.Item.FindControl("cblActionType");
                cblActionType.Items.Clear();
                foreach (KeyValuePair<string, int> kvp in Utils.ActionType())
                {
                    if ((kvp.Value & actionType) == kvp.Value)
                    {
                        cblActionType.Items.Add(new ListItem(kvp.Key, kvp.Value.ToString()));
                    }
                }
            }
        }
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
                JscriptMsg("修改管理角色成功！", ListName, "Success");
            }
            else //添加
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.Add); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", "", "Error");
                    return;
                }
                JscriptMsg("添加管理角色成功！", ListName, "Success");
            }
        }
    }
}