using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;
using System.Linq;

namespace ccphl.Web.admin.ccphl
{
    public partial class region_list : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "为民服务站点";
        //列表页面名称
        public const string ListName = "region_list.aspx";
        //编辑页面名称
        public const string EditName = "region_edit.aspx";
        public string regioncode = "";
        public int rid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            regioncode = DTRequest.GetQueryString("regioncode");
            if (!Page.IsPostBack)
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.View); //检查权限
                this.regionid.Items.Clear();
                List<region> rows = GetRegionTreeListByLayer();
                foreach (var dr in rows)
                {
                    string Id = dr.RegionCode.ToString();
                    int ClassLayer = dr.Layer ?? 0;
                    string Title = dr.RegionName;

                    if (ClassLayer == 1)
                    {
                        this.regionid.Items.Add(new ListItem(Title, Id));
                    }
                    else
                    {
                        Title = "├ " + Title;
                        Title = Utils.StringOfChar(ClassLayer - 1, "　") + Title;
                        this.regionid.Items.Add(new ListItem(Title, Id));
                    }
                }
                
                if (regioncode == "")
                {
                    regioncode = "53";
                    var reg = rows.SingleOrDefault(p => p.RegionCode == regioncode);
                    this.regionid.SelectedValue = regioncode;
                    rid = reg.ID;
                }
                else
                {
                    var reg = rows.SingleOrDefault(p => p.RegionCode == regioncode);
                    this.regionid.SelectedValue = regioncode;
                    rid = reg.ID;
                }
                RptBind(rid);
            }
        }

        //数据绑定
        private void RptBind(int parentid)
        {
            List<region> list = new List<region>();
            if (parentid == 0 || parentid == 1)
            {
                list = db.GetModelList<region>(" Layer<=2 ORDER BY RegionCode ASC");
            }
            else
            {
                list = GetRegionTreeList(parentid);
            }
            this.rptList.DataSource = list;
            this.rptList.DataBind();
        }

        //美化列表
        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
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
            }
        }

        //保存排序
        protected void btnSave_Click(object sender, EventArgs e)
        {
            ChkAdminLevel((long)DTEnums.ActionEnum.Edit); //检查权限
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                int sortId;
                if (!int.TryParse(((TextBox)rptList.Items[i].FindControl("txtSortId")).Text.Trim(), out sortId))
                {
                    sortId = 99;
                }
                db.UpdateField<region>(id, "SortNo", sortId);
            }
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "保存" + Name + "排序"); //记录日志
            JscriptMsg("保存排序成功！", ListName, "Success");
        }

        //删除导航
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChkAdminLevel((long)DTEnums.ActionEnum.Delete); //检查权限
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    db.Delete<region>(id);
                }
            }
            AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除" + Name + "信息"); //记录日志
            JscriptMsg("删除数据成功！", ListName, "Success", "parent.loadMenuTree");
        }

        //筛选类别
        protected void regionid_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt(ListName, "regioncode={0}",
                this.regionid.SelectedValue.ToString()));
        }
        /// <summary>
        /// 递归树表，按层级关系，输出站点列表
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public List<region> GetRegionTreeListByLayer()
        {
            //List<region> list = new List<region>();
            string sql = "";

            sql = " Layer<=3 ORDER BY RegionCode ASC ";

            var mlist = db.GetModelList<region>(sql);
            //foreach (var m in mlist)
            //{
            //    if (m.Layer <= 3)
            //    {
            //        if (db.GetCount<region>(" ParentID=" + m.ID + " ") > 0)
            //        {
            //            list.Add(m);
            //            var childlist = GetRegionTreeList(m.ID);
            //            foreach (var c in childlist)
            //            {
            //                list.Add(c);
            //            }
            //        }
            //        else
            //        {
            //            list.Add(m);
            //        }
            //    }
            //}
            return mlist;

        }
    }
}