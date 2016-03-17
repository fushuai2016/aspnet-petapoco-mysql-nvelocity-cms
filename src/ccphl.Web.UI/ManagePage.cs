using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;
using ccphl.DBUtility;
using ccphl.Entity;

namespace ccphl.Web.UI
{
    public class ManagePage : System.Web.UI.Page
    {
        static private SystemConfig sysConfig;
        XmlConfig<SystemConfig> xc = new XmlConfig<SystemConfig>();
        /// <summary>
        /// 站点信息
        /// </summary>
        protected internal system_site siteConfig;
        /// <summary>
        /// 数据库操作句柄
        /// </summary>
        protected internal MySqlUtility db=null;
        /// <summary>
        /// 菜单ID
        /// </summary>
        public int ModuleID = 0;
        public ManagePage()
        {
            if(db==null)
              db = new MySqlUtility();
            this.Load += new EventHandler(ManagePage_Load);
            siteConfig = db.GetModel<system_site>("", "order by SiteID asc");
            sysConfig = xc.loadSystemConfig();
            if(ModuleID==0)
                ModuleID = DTRequest.GetQueryInt("mid", 0);
        }
        private void ManagePage_Load(object sender, EventArgs e)
        {
            //判断管理员是否登录
            if (!IsAdminLogin())
            {
                Response.Write("<script>parent.location.href='" + sysConfig.webpath+ sysConfig.webmanagepath + "/login.aspx'</script>");
                Response.End();
            }
        }

        #region 管理员============================================
        /// <summary>
        /// 判断管理员是否已经登录(解决Session超时问题)
        /// </summary>
        public bool IsAdminLogin()
        {
            //如果Session为Null
            if (Session[DTKeys.SESSION_ADMIN_INFO] != null)
            {
                return true;
            }
            else
            {
                //检查Cookies
                string adminname = Utils.GetCookie("AdminName", "ccphlcms");
                string adminpwd = Utils.GetCookie("AdminPwd", "ccphlcms");
                if (adminname != "" && adminpwd != "")
                {
                    system_user model = db.GetModel<system_user>(" UserName='" + adminname + "'");
                    if (model != null)
                    {
                        if (adminpwd == model.Password)
                        {
                            Session[DTKeys.SESSION_ADMIN_INFO] = model;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        
        /// <summary>
        /// 取得管理员信息
        /// </summary>
        public system_user GetAdminInfo()
        {
            if (IsAdminLogin())
            {
                system_user model = Session[DTKeys.SESSION_ADMIN_INFO] as system_user;
                if (model != null)
                {
                    return model;
                }
            }
            return null;
        }

        /// <summary>
        /// 检查管理员权限
        /// </summary>
        /// <param name="Authority">操作类型的权值</param>
        public void ChkAdminLevel(long Authority)
        {
            bool result=false;
            system_user model = GetAdminInfo();
            //超级管理员
            if (model.UserType == 3)
            {
                result = true;
            }
            else
            {
                //计算权值
                long qvalue = 0;
                var plist = db.GetModelList<system_role_module_button>(" RoleID IN(SELECT RoleID FROM ccphl_system_role_user WHERE UserID=" + model.UserID + ") AND ModuleID=" + ModuleID);
                foreach (var p in plist)
                {
                    qvalue = (qvalue | (p.ButtonAuthority ?? 0));
                }
                if ((qvalue & Authority) == Authority)
                    result = true;
            }
            if (!result)
            {
                string tipname="";
                var AuthorityList = Utils.ActionType();
                foreach (var a in AuthorityList)
                {
                    if (Authority == (long)a.Value)
                    {
                        tipname = a.Key;
                        break;
                    }
                }
                string msgbox = "parent.jsdialog(\"错误提示\", \"您没有该页面的" + tipname + "权限！\", \"\", \"Error\")";
                Response.Write("<script type=\"text/javascript\">" + msgbox + "</script>");
                Response.End();
            }
        }
        /// <summary>
        /// 检查管理员网页管理权限
        /// </summary>
        /// <param name="Authority">操作类型的权值</param>
        public void ChkAdminWebLevel(int siteid,int channelid,int columntype,long Authority)
        {
            bool result = false;
            system_user model = GetAdminInfo();
            //超级管理员
            if (model.UserType == 3)
            {
                result = true;
            }
            else
            {
                if (siteid == 0 || channelid == 0 || columntype==0)
                {
                    result = false;
                }
                else
                {
                    //计算权值
                    long qvalue = 0;
                    var plist = db.GetModelList<system_web_column>(" RoleID IN(SELECT RoleID FROM ccphl_system_role_user WHERE UserID=" + model.UserID + ") AND SiteID=" + siteid + " AND ChannelID=" + channelid + " AND ColumnType=" + columntype);
                    foreach (var p in plist)
                    {
                        qvalue = (qvalue | (p.ButtonAuthority ?? 0));
                    }
                    if ((qvalue & Authority) == Authority)
                        result = true;
                }
            }
            if (!result)
            {
                string tipname = "";
                var AuthorityList = Utils.ActionType();
                foreach (var a in AuthorityList)
                {
                    if (Authority == (long)a.Value)
                    {
                        tipname = a.Key;
                        break;
                    }
                }
                string msgbox = "parent.jsdialog(\"错误提示\", \"您没有该页面的" + tipname + "权限！\", \"\", \"Error\")";
                Response.Write("<script type=\"text/javascript\">" + msgbox + "</script>");
                Response.End();
            }
        }

        /// <summary>
        /// 写入管理日志
        /// </summary>
        /// <param name="action_type"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool AddAdminLog(string action_type, string remark)
        {
            system_user model = GetAdminInfo();
            system_log log = new system_log();
            log.ClientHost = DTRequest.GetIP();
            log.GUID = Guid.NewGuid().ToString();
            log.SiteID = model.SiteID;
            log.SiteNode = model.SiteNode;
            log.UserID = model.UserID;
            log.UserName = model.UserName;
            log.OperatedTime = DateTime.Now;
            log.OperateType = action_type;
            log.LogCotent = remark;
            if (db.Insert<system_log>(log) > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 递归树表，按层级关系，输出系统菜单列表
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public List<system_module> GetModelTreeList(int parentid = 0)
        {
            List<system_module> list = new List<system_module>();
            string sql = "";
            if (parentid == 0)
            {
                sql = "  ParentID=" + parentid + "   ORDER BY OrderBy ASC ";
            }
            else
            {
                sql = "  ParentID=" + parentid + "  ORDER BY OrderBy ASC ";
            }
            var mlist = db.GetModelList<system_module>(sql);
            foreach (var m in mlist)
            {
                if (db.GetCount<system_module>(" ParentID=" + m.ModuleID + " AND Status=1 ") > 0)
                {
                    list.Add(m);
                    var childlist = GetModelTreeList(m.ModuleID);
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
        /// <summary>
        /// 递归树表，按层级关系，输出网站频道列表
        /// </summary>
        /// <param name="siteid"></param>
        /// <returns></returns>
        public List<ChannelColumn> GetWebTreeList(int siteid)
        {
            List<ChannelColumn> list = new List<ChannelColumn>();
            var chlist = db.GetModelList<Model.channel>(" SiteID=" + siteid + " AND Status=1  ORDER BY OrderBy ASC");
            var ctlist = Utils.ColumnTypeList();
            foreach (var ch in chlist)
            {
                list.Add(new ChannelColumn { Id = ch.ChannelID,ColumnId=0, Name = ch.ChannelName, Layer = 1, Authority = 1 });
                foreach (var ct in ctlist)
                {
                    if (ch.IsEdit == 0)
                    {
                        if (ct.Id == 2)
                        {
                            list.Add(new ChannelColumn { Id = ch.ChannelID, ColumnId = ct.Id, Name = ct.Name, Layer = 2, Authority = ct.Authority });
                        }
                    }
                    else
                    {
                        list.Add(new ChannelColumn { Id = ch.ChannelID, ColumnId = ct.Id, Name = ct.Name, Layer = 2, Authority = ct.Authority });
                    }
                }
            }
            return list;

        }
        /// <summary>
        /// 递归树表，按层级关系，输出站点列表
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public List<system_site> GetSiteTreeList(int parentid = 0)
        {
            List<system_site> list = new List<system_site>();
            string sql = "";

            sql = "  ParentID=" + parentid + " AND Status=1 ORDER BY OrderBy ASC ";

            var mlist = db.GetModelList<system_site>(sql);
            foreach (var m in mlist)
            {
                if (db.GetCount<system_site>(" ParentID=" + m.SiteID + " AND Status=1 ") > 0)
                {
                    list.Add(m);
                    var childlist = GetSiteTreeList(m.SiteID);
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
        /// <summary>
        /// 递归树表，按层级关系，输出站点列表
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public List<region> GetRegionTreeList(int parentid = 0)
        {
            List<region> list = new List<region>();
            string sql = "";

            sql = "  ParentID=" + parentid + " ORDER BY SortNo ASC ";

            var mlist = db.GetModelList<region>(sql);
            foreach (var m in mlist)
            {
                if (db.GetCount<region>(" ParentID=" + m.ID + "") > 0)
                {
                    list.Add(m);
                    var childlist = GetRegionTreeList(m.ID);
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
        /// <summary>
        /// 递归树表，按层级关系，输出栏目列表
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public List<column> GetColumnTreeList(int siteid,int channelid,int parentid = 0)
        {
            List<column> list = new List<column>();
            string sql = "";
            if (siteid != 0)
            {
                sql += " SiteID=" + siteid + " AND ";
            }
            if (channelid != 0)
            {
                sql += " ChannelID=" + channelid + " AND ";
            }
            sql += "  ParentID=" + parentid + " ORDER BY OrderBy ASC ";
            var mlist = db.GetModelList<column>(sql);
            foreach (var m in mlist)
            {
                string sql2 = "";
                if (siteid != 0)
                {
                    sql2 += " SiteID=" + siteid + " AND ";
                }
                if (channelid != 0)
                {
                    sql2 += " ChannelID=" + channelid + " AND ";
                }
                sql2 += "  ParentID=" + m.ColumnID + " ";
                if (db.GetCount<column>(sql2) > 0)
                {
                    list.Add(m);
                    var childlist = GetColumnTreeList(siteid,channelid, m.ColumnID);
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
        /// <summary>
        /// 递归树表，按层级关系，输出栏目列表
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public List<column> GetColumnTreeList2(int siteid, int channelid, int parentid = 0)
        {
            List<column> list = new List<column>();
            if (channelid == 0)
                return list;
            string sql = "";
            if (siteid != 0)
            {
                sql += " SiteID=" + siteid + " AND ";
            }
            if (channelid != 0)
            {
                sql += " ChannelID=" + channelid + " AND ";
            }
            sql += "  ParentID=" + parentid + " AND Status=1 ORDER BY OrderBy ASC ";
            var mlist = db.GetModelList<column>(sql);
            foreach (var m in mlist)
            {
                string sql2 = "";
                if (siteid != 0)
                {
                    sql2 += " SiteID=" + siteid + " AND ";
                }
                if (channelid != 0)
                {
                    sql2 += " ChannelID=" + channelid + " AND ";
                }
                sql2 += "  ParentID=" + m.ColumnID + " AND Status=1 ";
                if (db.GetCount<column>(sql2) > 0)
                {
                    list.Add(m);
                    var childlist = GetColumnTreeList2(siteid, channelid, m.ColumnID);
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
        /// <summary>
        /// 递归树表，按层级关系，输出广告位置列表
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public List<adsposition> GetAdsPosTreeList(int siteid,int parentid = 0)
        {
            List<adsposition> list = new List<adsposition>();
            string sql = "";

            sql = " SiteID=" + siteid + " AND ParentID=" + parentid + " AND Status=1 ORDER BY OrderBy ASC ";

            var mlist = db.GetModelList<adsposition>(sql);
            foreach (var m in mlist)
            {
                if (db.GetCount<adsposition>(" SiteID=" + siteid + " AND ParentID=" + m.AdsPositionID + " AND Status=1 ") > 0)
                {
                    list.Add(m);
                    var childlist = GetAdsPosTreeList(siteid,m.AdsPositionID);
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
        #endregion

        #region JS提示============================================
        /// <summary>
        /// 添加编辑删除提示
        /// </summary>
        /// <param name="msgtitle">提示文字</param>
        /// <param name="url">返回地址</param>
        /// <param name="msgcss">CSS样式</param>
        protected void JscriptMsg(string msgtitle, string url, string msgcss)
        {
            if (!string.IsNullOrWhiteSpace(url) && url!="back")
            {
                if (url.IndexOf("?") > -1)
                {
                    if (url.IndexOf("mid") == -1)
                    {
                        url += "&mid=" + ModuleID;
                    }
                    if (url.IndexOf("siteid") == -1)
                    {
                        url += "&siteid=" + siteConfig.SiteID;
                    }
                }
                else
                {
                    if (url.IndexOf("mid") == -1)
                    {
                        url += "?mid=" + ModuleID;
                    }
                    if (url.IndexOf("siteid") == -1)
                    {
                        url += "&siteid=" + siteConfig.SiteID;
                    }
                }
            }
            string msbox = "parent.jsprint(\"" + msgtitle + "\", \"" + url + "\", \"" + msgcss + "\")";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "JsPrint", msbox, true);
        }
        /// <summary>
        /// 带回传函数的添加编辑删除提示
        /// </summary>
        /// <param name="msgtitle">提示文字</param>
        /// <param name="url">返回地址</param>
        /// <param name="msgcss">CSS样式</param>
        /// <param name="callback">JS回调函数</param>
        protected void JscriptMsg(string msgtitle, string url, string msgcss, string callback)
        {
            if (!string.IsNullOrWhiteSpace(url) && url != "back")
            {
                if (url.IndexOf("?") > -1)
                {
                    if (url.IndexOf("mid") == -1)
                    {
                        url += "&mid=" + ModuleID;
                    }
                    if (url.IndexOf("siteid") == -1)
                    {
                        url += "&siteid=" + siteConfig.SiteID;
                    }
                }
                else
                {
                    if (url.IndexOf("mid") == -1)
                    {
                        url += "?mid=" + ModuleID;
                    }
                    if (url.IndexOf("siteid") == -1)
                    {
                        url += "&siteid=" + siteConfig.SiteID;
                    }
                }
            }
            string msbox = "parent.jsprint(\"" + msgtitle + "\", \"" + url + "\", \"" + msgcss + "\", " + callback + ")";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "JsPrint", msbox, true);
        }
        #endregion

    }
}
