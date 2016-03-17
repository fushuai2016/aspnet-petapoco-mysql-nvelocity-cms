using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;
using System.IO;

namespace ccphl.Web.admin.ccphl
{
    public partial class region_edit : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "为民服务站点";
        //列表页面名称
        public const string ListName = "region_list.aspx";
        //编辑页面名称
        public const string EditName = "region_edit.aspx";

        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;
        private string regioncode2 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");
            this.id = DTRequest.GetQueryInt("id");
            regioncode2 = DTRequest.GetQueryString("regioncode");
            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", "", "Error");
                    return;
                }
                if (!db.IsExists<region>(this.id))
                {
                    JscriptMsg("站点不存在或已被删除！", "", "Error");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.View); //检查权限
                TreeBind(); //绑定站点
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
                else
                {
                    if (this.id > 0)
                    {
                        //this.ddlParentId.SelectedValue = this.id.ToString();
                    }
                    regionname.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=region_validate_name");
                    regionname.Focus(); //设置焦点，防止JS无法提交
                    regioncode.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=region_validate_code");
                    organcode.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=region_validate_organ");
                }
            }
        }

        #region 绑定=============================
        private void TreeBind()
        {

            //List<region> list = GetRegionTreeListByLayer();

            //this.ddlParentId.Items.Clear();
            //this.ddlParentId.Items.Add(new ListItem("无父级站点", "0"));
            //foreach(var m in list)
            //{
            //    string Id = m.ID.ToString();
            //    int ClassLayer = m.Layer??1;
            //    string Title = m.RegionName;

            //    if (ClassLayer == 1)
            //    {
            //        this.ddlParentId.Items.Add(new ListItem(Title, Id));
            //    }
            //    else
            //    {
            //        Title = "├ " + Title;
            //        Title = Utils.StringOfChar(ClassLayer - 1, "　") + Title;
            //        this.ddlParentId.Items.Add(new ListItem(Title, Id));
            //    }
            //}
        }
        #endregion

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            region model = db.GetModel<region>(_id);
            if (model.ParentID != 0)
            {
                var parent = db.GetModel<region>(model.ParentID);
                parentid.Text = parent.RegionName;
                hidparentid.Value = parent.ID.ToString();
            }
            else
            {
                parentid.Text ="无父级站点";
                hidparentid.Value = "0";
            }
            parentid.ReadOnly = true;
            //ddlParentId.SelectedValue = model.ParentID.ToString();
           
            txtSortId.Text = model.SortNo.ToString();
            if (model.Status == 0)
            {
                cbIsLock.Checked = true;
            }
            if (model.IsOpen == 1)
            {
                cbIsOpen.Checked = true;
            }
            regioncode.Text = model.RegionCode;
            regionname.Text = model.RegionName;
            organcode.Text = model.OrganCode;
            regionurl.Text = model.RegionUrl;
            regionname.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=region_validate_name&old_region_name="+Utils.UrlEncode(model.RegionName));
            regionname.Focus(); //设置焦点，防止JS无法提交
            regioncode.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=region_validate_code&old_region_code="+Utils.UrlEncode(model.RegionCode));
            organcode.Attributes.Add("ajaxurl", "../../tools/admin_ajax.ashx?action=region_validate_organ&old_organ_code="+Utils.UrlEncode(model.OrganCode));
            
            icon.Text = model.Icon;
            imgurl.Text = model.ImageUrl;
            clientip.Text = model.ClientIP;
            latitude.Text = model.Latitude;
            longitude.Text = model.Longitude;
            departmentid.Text = model.DepartmentCode;
            weathercode.Text = model.WeatherCode;
            remark.Text = model.Remark;
            DistanceEducationUrl.Text = model.DistanceEducationUrl;
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            try
            {
                region model = new region();

                //model.ParentID = int.Parse(ddlParentId.SelectedValue);
                model.ParentID = 0;
                model.SortNo = int.Parse(txtSortId.Text.Trim());
                model.Status = 1;
                if (cbIsLock.Checked == true)
                {
                    model.Status = 0;
                }
                if (cbIsOpen.Checked == true)
                {
                    model.IsOpen = 1;
                }
                model.AddTime = DateTime.Now;
                model.GUID = Guid.NewGuid().ToString();

                model.RegionCode = regioncode.Text;
                model.RegionName=regionname.Text;
                model.OrganCode=organcode.Text;
                model.RegionUrl = regionurl.Text;

                model.Icon=icon.Text;
                model.ImageUrl=imgurl.Text;
                model.ClientIP = clientip.Text;
                model.Latitude= latitude.Text;
                model.Longitude=longitude.Text;
                model.DepartmentCode =departmentid.Text;
                model.WeatherCode = weathercode.Text;
                model.Remark = remark.Text;
                model.DistanceEducationUrl = DistanceEducationUrl.Text;
                long _id = db.InsertTree<region>(model, "Node", "Layer", "ParentID");
                if (_id > 0)
                {
                    AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加"+Name+"信息:" + model.RegionName); //记录日志
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
                region model = db.GetModel<region>(_id);

                //model.ParentID = int.Parse(ddlParentId.SelectedValue);
                model.SortNo = int.Parse(txtSortId.Text.Trim());
                model.Status = 1;
                if (cbIsLock.Checked == true)
                {
                    model.Status = 0;
                }
                if (cbIsOpen.Checked == true)
                {
                    model.IsOpen = 1;
                }
                if (string.IsNullOrEmpty(model.GUID))
                {
                    model.GUID = Guid.NewGuid().ToString();
                }
                model.RegionCode = regioncode.Text;
                model.RegionName = regionname.Text;
                model.OrganCode = organcode.Text;
                model.RegionUrl = regionurl.Text;

                model.Icon = icon.Text;
                model.ImageUrl = imgurl.Text;
                model.ClientIP = clientip.Text;
                model.Latitude = latitude.Text;
                model.Longitude = longitude.Text;
                model.DepartmentCode = departmentid.Text;
                model.WeatherCode = weathercode.Text;
                model.Remark = remark.Text;
                model.DistanceEducationUrl = DistanceEducationUrl.Text;
                //if (db.UpdateTree<region>(model, "Node", "Layer", "ParentID") > 0)
                //{
                //    AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改"+Name+"信息:" + model.RegionName); //记录日志
                //    return true;
                //}
                if (db.Update<region>(model) > 0)
                {
                    AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改" + Name + "信息:" + model.RegionName); //记录日志
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
                JscriptMsg("修改站点成功！", ListName + "?regioncode=" + regioncode2, "Success", "parent.loadMenuTree");
            }
            else //添加
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.Add); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", "", "Error");
                    return;
                }
                JscriptMsg("添加站点成功！", ListName + "?regioncode=" + regioncode2, "Success", "parent.loadMenuTree");
            }
        }
        /// <summary>
        /// 递归树表，按层级关系，输出站点列表
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public List<region> GetRegionTreeListByLayer()
        {
            string sql = "";

            sql = " Layer<=4 ORDER BY RegionCode ASC ";

            var mlist = db.GetModelList<region>(sql);
          
            return mlist;

        }

    }
}