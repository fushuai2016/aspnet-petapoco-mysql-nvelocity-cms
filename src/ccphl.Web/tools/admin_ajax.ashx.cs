using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.SessionState;
using ccphl.Web.UI;
using ccphl.Common;
using Model;
using ccphl.DBUtility;
using LitJson;
using ccphl.Entity;

namespace ccphl.Web.tools
{
    /// <summary>
    /// 管理后台AJAX处理页
    /// </summary>
    public class admin_ajax : IHttpHandler, IRequiresSessionState
    {
        private MySqlUtility db = new MySqlUtility();
        public void ProcessRequest(HttpContext context)
        {
            //取得处事类型
            string action = DTRequest.GetQueryString("action");

            switch (action)
            {
                case "template_dirs": //模板目录
                    template_dirs(context);
                    break;
                case "template_files": //目录下的文件
                    template_files(context);
                    break;
                //case "attribute_field_validate": //验证扩展字段是否重复
                //    attribute_field_validate(context);
                //    break;
                //case "channel_category_validate": //验证频道分类目录是否重复
                //    channel_category_validate(context);
                //    break;
                case "img_validate_code": //验证图片代码是否重复
                    img_validate_code(context);
                    break;
                case "channel_validate_code": //验证频道代码是否重复
                    channel_validate_code(context);
                    break;
                case "channel_validate_name": //验证频道名称是否重复
                    channel_validate_name(context);
                    break;
                //case "urlrewrite_name_validate": //验证URL调用名称是否重复
                //    urlrewrite_name_validate(context);
                //    break;
                case "navigation_validate": //验证导航菜单ID是否重复
                    navigation_validate(context);
                    break;
                case "site_validate_code": //验证站点代码是否重复
                    site_validate_code(context);
                    break;
                case "site_validate_name": //验证站点名称是否重复
                    site_validate_name(context);
                    break;
                case "site_validate_url": //验证站点域名是否重复
                    site_validate_url(context);
                    break;
                case "region_validate_code": //验证站点代码是否重复
                    region_validate_code(context);
                    break;
                case "region_validate_name": //验证站点名称是否重复
                    region_validate_name(context);
                    break;
                case "region_validate_organ": //验证组织机构是否重复
                    region_validate_organ(context);
                    break;
                case "column_validate_code": //验证栏目代码是否重复
                    column_validate_code(context);
                    break;
                case "column_validate_code2": //验证栏目代码是否重复
                    column_validate_code2(context);
                    break;
                case "adsposition_validate_code": //验证广告位置代码是否重复
                    adsposition_validate_code(context);
                    break;
                case "manager_validate": //验证管理员用户名是否重复
                    manager_validate(context);
                    break;
                case "get_remote_fileinfo": //获取远程文件的信息
                    get_remote_fileinfo(context);
                    break;
                case "get_navigation_list": //获取后台导航字符串
                    get_navigation_list(context);
                    break;
                //case "edit_order_status": //修改订单信息和状态
                //    edit_order_status(context);
                //    break;
                //case "validate_username": //验证会员用户名是否重复
                //    validate_username(context);
                //    break;
                //case "sms_message_post": //发送手机短信
                //    sms_message_post(context);
                //    break;
                //case "get_builder_urls": //获取要生成静态的地址
                //    get_builder_urls(context);
                //    break;
                //case "get_builder_html": //生成静态页面
                //    get_builder_html(context);
                //    break;
            }

        }
        #region 获取文件============================
        private void template_dirs(HttpContext context)
        {
            string dir = DTRequest.GetString("dir");//根目录
            if (string.IsNullOrEmpty(dir))
            {
                context.Response.Write("{ \"status\":\"0\",\"msg\":\"模板根目录不存在！\"}");
                return;
            }
            string[] dirList = Directory.GetDirectories(HttpContext.Current.Server.MapPath("~" + dir));
            Hashtable result = new Hashtable();
            result["status"] = 1;
            result["total"] = dirList.Length;
            List<Hashtable> dirFileList = new List<Hashtable>();
            for (int i = 0; i < dirList.Length; i++)
            {
                DirectoryInfo d = new DirectoryInfo(dirList[i]);
                if (d.Name.IndexOf("images") == -1 || d.Name.IndexOf("style") == -1 || d.Name.IndexOf("js") == -1)
                {
                    Hashtable hash = new Hashtable();
                    hash["value"] = dir + "/" + d.Name;
                    hash["filename"] = dir + "/" + d.Name;
                    dirFileList.Add(hash);
                }
            }
            result["filelist"] = dirFileList;
            context.Response.AddHeader("Content-Type", "application/json; charset=UTF-8");
            context.Response.Write(JsonMapper.ToJson(result));
            context.Response.End();
        }
        private void template_files(HttpContext context)
        {
            string dir = DTRequest.GetString("dir");//模板文件夹路径
            string type = DTRequest.GetString("type");//文件类型
            if (string.IsNullOrEmpty(dir))
            {
                context.Response.Write("{ \"status\":\"0\",\"msg\":\"模板根目录不存在！\"}");
                return;
            }
            //if (string.IsNullOrEmpty(type))
            //{
            //    context.Response.Write("{ \"status\":\"0\",\"msg\":\"请定义文件后缀字符串！\"}");
            //    return;
            //}
            DirectoryInfo fileDirectory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~" + dir));
            FileInfo[] fileinfo = fileDirectory.GetFiles();
            Hashtable result = new Hashtable();
            result["status"] = 1;
            result["total"] = fileinfo.Length;
            List<Hashtable> dirFileList = new List<Hashtable>();
            for (int i = fileinfo.Length - 1; i >= 0; i--)
            {
                if (fileinfo[i].Extension == ".html")
                {
                    if (fileinfo[i].Name.Replace(".html", "").EndsWith(type))
                    {
                    Hashtable hash = new Hashtable();
                    hash["value"] = fileinfo[i].Name;
                    hash["filename"] = fileinfo[i].Name;
                    dirFileList.Add(hash);
                    }
                }
            }
            result["filelist"] = dirFileList;
            context.Response.AddHeader("Content-Type", "application/json; charset=UTF-8");
            context.Response.Write(JsonMapper.ToJson(result));
            context.Response.End();
        }
        #endregion
        //#region 验证扩展字段是否重复============================
        //private void attribute_field_validate(HttpContext context)
        //{
        //    string column_name = DTRequest.GetString("param");
        //    if (string.IsNullOrEmpty(column_name))
        //    {
        //        context.Response.Write("{ \"info\":\"名称不可为空\", \"status\":\"n\" }");
        //        return;
        //    }
        //    BLL.article_attribute_field bll = new BLL.article_attribute_field();
        //    if (bll.Exists(column_name))
        //    {
        //        context.Response.Write("{ \"info\":\"该名称已被占用，请更换！\", \"status\":\"n\" }");
        //        return;
        //    }
        //    context.Response.Write("{ \"info\":\"该名称可使用\", \"status\":\"y\" }");
        //    return;
        //}
        //#endregion

        //#region 验证频道分类生成目录名是否可用==================
        //private void channel_category_validate(HttpContext context)
        //{
        //    string build_path = DTRequest.GetString("param");
        //    string old_build_path = DTRequest.GetString("old_build_path");
        //    if (string.IsNullOrEmpty(build_path))
        //    {
        //        context.Response.Write("{ \"info\":\"该目录名不可为空！\", \"status\":\"n\" }");
        //        return;
        //    }
        //    if (build_path.ToLower() == old_build_path.ToLower())
        //    {
        //        context.Response.Write("{ \"info\":\"该目录名可使用\", \"status\":\"y\" }");
        //        return;
        //    }
        //    BLL.channel_category bll = new BLL.channel_category();
        //    if (bll.Exists(build_path))
        //    {
        //        context.Response.Write("{ \"info\":\"该目录名已被占用，请更换！\", \"status\":\"n\" }");
        //        return;
        //    }
        //    context.Response.Write("{ \"info\":\"该目录名可使用\", \"status\":\"y\" }");
        //    return;
        //}
        //#endregion
        #region 验证图片类型代码是否可用========================
        private void img_validate_code(HttpContext context)
        {
            string img_code = DTRequest.GetString("param");
            string old_img_code = DTRequest.GetString("old_img_code");
            if (string.IsNullOrEmpty(img_code))
            {
                context.Response.Write("{ \"info\":\"图片代码不可为空！\", \"status\":\"n\" }");
                return;
            }
            if (img_code.ToLower() == old_img_code.ToLower())
            {
                context.Response.Write("{ \"info\":\"该名称可使用\", \"status\":\"y\" }");
                return;
            }
            XmlConfig<ImageConfig> xc = new XmlConfig<ImageConfig>();
            List<ImageConfig> list = xc.loadImgListConfig();
            if (list.Count(p => p.imgcode == img_code)>0)
            {
                context.Response.Write("{ \"info\":\"该名称已被占用，请更换！\", \"status\":\"n\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"该名称可使用\", \"status\":\"y\" }");
            return;
        }
       
        #endregion
        #region 验证频道名称是否是否可用========================
        private void channel_validate_code(HttpContext context)
        {
            int sid = DTRequest.GetQueryInt("siteid", 0);
            string channel_code = DTRequest.GetString("param");
            string old_channel_code = DTRequest.GetString("old_channel_code");
            if (string.IsNullOrEmpty(channel_code))
            {
                context.Response.Write("{ \"info\":\"频道代码不可为空！\", \"status\":\"n\" }");
                return;
            }
            if (channel_code.ToLower() == old_channel_code.ToLower())
            {
                context.Response.Write("{ \"info\":\"该名称可使用\", \"status\":\"y\" }");
                return;
            }
            if (db.IsExists<Model.channel>(" ChannelCode='" + channel_code + "' AND SiteID=" + sid))
            {
                context.Response.Write("{ \"info\":\"该名称已被占用，请更换！\", \"status\":\"n\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"该名称可使用\", \"status\":\"y\" }");
            return;
        }
        private void channel_validate_name(HttpContext context)
        {
            int sid = DTRequest.GetQueryInt("siteid",0);
            string channel_name = DTRequest.GetString("param");
            string old_channel_name = DTRequest.GetString("old_channel_name");
            if (string.IsNullOrEmpty(channel_name))
            {
                context.Response.Write("{ \"info\":\"频道名称不可为空！\", \"status\":\"n\" }");
                return;
            }
            if (channel_name.ToLower() == old_channel_name.ToLower())
            {
                context.Response.Write("{ \"info\":\"该名称可使用\", \"status\":\"y\" }");
                return;
            }
            if (db.IsExists<Model.channel>(" ChannelName='" + channel_name + "' AND SiteID="+sid))
            {
                context.Response.Write("{ \"info\":\"该名称已被占用，请更换！\", \"status\":\"n\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"该名称可使用\", \"status\":\"y\" }");
            return;
        }
        #endregion
        #region 验证站点是否可用========================
        private void site_validate_code(HttpContext context)
        {
            string site_code = DTRequest.GetString("param");
            string old_site_code = DTRequest.GetString("old_site_code");
            if (string.IsNullOrEmpty(site_code))
            {
                context.Response.Write("{ \"info\":\"站点代码不可为空！\", \"status\":\"n\" }");
                return;
            }
            if (site_code.ToLower() == old_site_code.ToLower())
            {
                context.Response.Write("{ \"info\":\"该代码可使用\", \"status\":\"y\" }");
                return;
            }
            if (db.IsExists<Model.system_site>(" SiteCode='" + site_code + "'"))
            {
                context.Response.Write("{ \"info\":\"该代码已被占用，请更换！\", \"status\":\"n\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"该名称可使用\", \"status\":\"y\" }");
            return;
        }
        private void site_validate_name(HttpContext context)
        {
            string site_name = DTRequest.GetString("param");
            string old_site_name = DTRequest.GetString("old_site_name");
            if (string.IsNullOrEmpty(site_name))
            {
                context.Response.Write("{ \"info\":\"站点名称不可为空！\", \"status\":\"n\" }");
                return;
            }
            if (site_name.ToLower() == old_site_name.ToLower())
            {
                context.Response.Write("{ \"info\":\"该名称可使用\", \"status\":\"y\" }");
                return;
            }
            if (db.IsExists<Model.system_site>(" SiteName='" + site_name + "'"))
            {
                context.Response.Write("{ \"info\":\"该名称已被占用，请更换！\", \"status\":\"n\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"该名称可使用\", \"status\":\"y\" }");
            return;
        }
        private void site_validate_url(HttpContext context)
        {
            string url_name = DTRequest.GetString("param");
            string old_site_url = DTRequest.GetString("old_site_url");
            if (string.IsNullOrEmpty(url_name))
            {
                context.Response.Write("{ \"info\":\"站点域名不可为空！\", \"status\":\"n\" }");
                return;
            }
            if (url_name.ToLower() == old_site_url.ToLower())
            {
                context.Response.Write("{ \"info\":\"该名称可使用\", \"status\":\"y\" }");
                return;
            }
            if (db.IsExists<Model.system_site>(" DomainName='" + url_name + "'"))
            {
                context.Response.Write("{ \"info\":\"该名称已被占用，请更换！\", \"status\":\"n\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"该名称可使用\", \"status\":\"y\" }");
            return;
        }
        #endregion
        #region 验证为民服务站点是否可用========================
        private void region_validate_code(HttpContext context)
        {
            string region_code = DTRequest.GetString("param");
            string old_region_code = DTRequest.GetString("old_region_code");
            if (string.IsNullOrEmpty(region_code))
            {
                context.Response.Write("{ \"info\":\"站点代码不可为空！\", \"status\":\"n\" }");
                return;
            }
            if (region_code.ToLower() == old_region_code.ToLower())
            {
                context.Response.Write("{ \"info\":\"该代码可使用\", \"status\":\"y\" }");
                return;
            }
            if (db.IsExists<Model.region>(" RegionCode='" + region_code + "'"))
            {
                context.Response.Write("{ \"info\":\"该代码已被占用，请更换！\", \"status\":\"n\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"该名称可使用\", \"status\":\"y\" }");
            return;
        }
        private void region_validate_name(HttpContext context)
        {
            string region_name = DTRequest.GetString("param");
            string old_region_name = DTRequest.GetString("old_region_name");
            if (string.IsNullOrEmpty(region_name))
            {
                context.Response.Write("{ \"info\":\"站点名称不可为空！\", \"status\":\"n\" }");
                return;
            }
            if (region_name.ToLower() == old_region_name.ToLower())
            {
                context.Response.Write("{ \"info\":\"该名称可使用\", \"status\":\"y\" }");
                return;
            }
            if (db.IsExists<Model.region>(" RegionName='" + region_name + "'"))
            {
                context.Response.Write("{ \"info\":\"该名称已被占用，请更换！\", \"status\":\"n\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"该名称可使用\", \"status\":\"y\" }");
            return;
        }
        private void region_validate_organ(HttpContext context)
        {
            string organ_code = DTRequest.GetString("param");
            string old_organ_code = DTRequest.GetString("old_organ_code");
            if (string.IsNullOrEmpty(organ_code))
            {
                context.Response.Write("{ \"info\":\"站点组织代码不可为空！\", \"status\":\"n\" }");
                return;
            }
            if (organ_code.ToLower() == old_organ_code.ToLower())
            {
                context.Response.Write("{ \"info\":\"该名称可使用\", \"status\":\"y\" }");
                return;
            }
            if (db.IsExists<Model.region>(" OrganCode='" + organ_code + "'"))
            {
                context.Response.Write("{ \"info\":\"该代码已被占用，请更换！\", \"status\":\"n\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"该名称可使用\", \"status\":\"y\" }");
            return;
        }
        #endregion
        #region 验证栏目代码是否重复========================
        private void column_validate_code(HttpContext context)
        {
            string column_code = DTRequest.GetString("param");
            string old_column_code = DTRequest.GetString("old_column_code");
            if (string.IsNullOrEmpty(column_code))
            {
                context.Response.Write("{ \"info\":\"栏目代码不可为空！\", \"status\":\"n\" }");
                return;
            }
            if (column_code.ToLower() == old_column_code.ToLower())
            {
                context.Response.Write("{ \"info\":\"该代码可使用\", \"status\":\"y\" }");
                return;
            }
            if (db.IsExists<Model.column>(" ColumnCode='" + column_code + "'"))
            {
                context.Response.Write("{ \"info\":\"该代码已被占用，请更换！\", \"status\":\"n\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"该代码可使用\", \"status\":\"y\" }");
            return;
        }
        private void column_validate_code2(HttpContext context)
        {
            string column_code = DTRequest.GetString("param");
            string old_column_code = DTRequest.GetString("old_column_code");
            int siteid = DTRequest.GetQueryInt("siteid");
            int channelid = DTRequest.GetQueryInt("channelid");
            if (siteid == 0 || channelid == 0)
            {
                context.Response.Write("{ \"info\":\"栏目站点或频道信息为空！\", \"status\":\"n\" }");
                return;
            }
            if (string.IsNullOrEmpty(column_code))
            {
                context.Response.Write("{ \"info\":\"栏目代码不可为空！\", \"status\":\"n\" }");
                return;
            }
            if (column_code.ToLower() == old_column_code.ToLower())
            {
                context.Response.Write("{ \"info\":\"该代码可使用\", \"status\":\"y\" }");
                return;
            }
            if (db.IsExists<Model.column>(" ColumnCode='" + column_code + "' AND SiteID="+siteid+" AND ChannelID="+channelid))
            {
                context.Response.Write("{ \"info\":\"该代码已被占用，请更换！\", \"status\":\"n\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"该代码可使用\", \"status\":\"y\" }");
            return;
        }
        #endregion

        #region 验证广告位置代码是否重复========================
        private void adsposition_validate_code(HttpContext context)
        {
            string position_code = DTRequest.GetString("param");
            string old_position_code = DTRequest.GetString("old_position_code");
            int siteid = DTRequest.GetQueryInt("siteid");
            if (siteid == 0)
            {
                context.Response.Write("{ \"info\":\"站点不存在无法验证！\", \"status\":\"n\" }");
                return;
            }
            if (string.IsNullOrEmpty(position_code))
            {
                context.Response.Write("{ \"info\":\"代码不可为空！\", \"status\":\"n\" }");
                return;
            }
            if (position_code.ToLower() == old_position_code.ToLower())
            {
                context.Response.Write("{ \"info\":\"该代码可使用\", \"status\":\"y\" }");
                return;
            }
            if (db.IsExists<Model.adsposition>(" PositionCode='" + position_code + "' AND SiteID="+siteid))
            {
                context.Response.Write("{ \"info\":\"该代码已被占用，请更换！\", \"status\":\"n\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"该代码可使用\", \"status\":\"y\" }");
            return;
        }
       
        #endregion
        //#region 验证URL调用名称是否重复=========================
        //private void urlrewrite_name_validate(HttpContext context)
        //{
        //    string new_name = DTRequest.GetString("param");
        //    string old_name = DTRequest.GetString("old_name");
        //    if (string.IsNullOrEmpty(new_name))
        //    {
        //        context.Response.Write("{ \"info\":\"名称不可为空！\", \"status\":\"n\" }");
        //        return;
        //    }
        //    if (new_name.ToLower() == old_name.ToLower())
        //    {
        //        context.Response.Write("{ \"info\":\"该名称可使用\", \"status\":\"y\" }");
        //        return;
        //    }
        //    BLL.url_rewrite bll = new BLL.url_rewrite();
        //    if (bll.Exists(new_name))
        //    {
        //        context.Response.Write("{ \"info\":\"该名称已被使用，请更换！\", \"status\":\"n\" }");
        //        return;
        //    }
        //    context.Response.Write("{ \"info\":\"该名称可使用\", \"status\":\"y\" }");
        //    return;
        //}
        //#endregion

        #region 验证导航菜单ID是否重复==========================
        private void navigation_validate(HttpContext context)
        {
            string navname = DTRequest.GetString("param");
            string old_name = DTRequest.GetString("old_name");
            if (string.IsNullOrEmpty(navname))
            {
                context.Response.Write("{ \"info\":\"该导航菜单ID不可为空！\", \"status\":\"n\" }");
                return;
            }
            if (navname.ToLower() == old_name.ToLower())
            {
                context.Response.Write("{ \"info\":\"该导航菜单ID可使用\", \"status\":\"y\" }");
                return;
            }
            //检查保留的名称开头
            if (navname.ToLower().StartsWith("column_"))
            {
                context.Response.Write("{ \"info\":\"该导航菜单ID系统保留，请更换！\", \"status\":\"n\" }");
                return;
            }
            if (db.IsExists<system_module>(" ModuleCode='" + navname + "'"))
            {
                context.Response.Write("{ \"info\":\"该导航菜单ID已被占用，请更换！\", \"status\":\"n\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"该导航菜单ID可使用\", \"status\":\"y\" }");
            return;
        }
        #endregion

        #region 验证管理员用户名是否重复========================
        private void manager_validate(HttpContext context)
        {
            string user_name = DTRequest.GetString("param");
            if (string.IsNullOrEmpty(user_name))
            {
                context.Response.Write("{ \"info\":\"请输入用户名\", \"status\":\"n\" }");
                return;
            }
            if (db.IsExists<system_user>(" UserName='"+user_name+"'"))
            {
                context.Response.Write("{ \"info\":\"用户名已被占用，请更换！\", \"status\":\"n\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"用户名可使用\", \"status\":\"y\" }");
            return;
        }
        #endregion

        #region 获取远程文件的信息==============================
        private void get_remote_fileinfo(HttpContext context)
        {
            string filePath = DTRequest.GetFormString("remotepath");
            if (string.IsNullOrEmpty(filePath))
            {
                context.Response.Write("{\"status\": 0, \"msg\": \"没有找到远程附件地址！\"}");
                return;
            }
            if (!filePath.ToLower().StartsWith("http://"))
            {
                context.Response.Write("{\"status\": 0, \"msg\": \"不是远程附件地址！\"}");
                return;
            }
            try
            {
                HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(filePath);
                HttpWebResponse _response = (HttpWebResponse)_request.GetResponse();
                int fileSize = (int)_response.ContentLength;
                string fileName = filePath.Substring(filePath.LastIndexOf("/") + 1);
                string fileExt = filePath.Substring(filePath.LastIndexOf(".") + 1).ToUpper();
                context.Response.Write("{\"status\": 1, \"msg\": \"获取远程文件成功！\", \"name\": \"" + fileName + "\", \"path\": \"" + filePath + "\", \"size\": " + fileSize + ", \"ext\": \"" + fileExt + "\"}");
            }
            catch
            {
                context.Response.Write("{\"status\": 0, \"msg\": \"远程文件不存在！\"}");
                return;
            }
        }
        #endregion

        #region 获取后台导航字符串==============================
        private void get_navigation_list(HttpContext context)
        {
            system_user adminModel = new ManagePage().GetAdminInfo(); //获得当前登录管理员信息
            if (adminModel == null)
            {
                return;
            }
            if (adminModel.Status == 0)
            {
                return;
            }
            List<system_module> modulelist = new List<system_module>();
            if (adminModel.UserType == 3)
            {
                modulelist = db.GetModelList<system_module>(" Status=1 ");
            }
            else
            {
                modulelist = db.GetModelList<system_module>(" ModuleID IN(SELECT ModuleID FROM ccphl_system_role_module_button WHERE RoleID IN(SELECT RoleID FROM ccphl_system_role_user WHERE UserID=" + adminModel.UserID + ") AND ButtonAuthority>0) AND Status=1");
            }
            if (modulelist.Count == 0)
            {
                return;
            }
            this.get_navigation_childs(context, modulelist, adminModel, 0);

        }
        //系统菜单
        private void get_navigation_childs(HttpContext context, List<system_module> list,system_user user, int parentid,string parentname="")
        {
            int siteid = DTRequest.GetQueryInt("siteid", 0);
            var mlist = list.Where(m => m.ParentID == parentid && m.Status == 1).OrderBy(b => b.OrderBy);
            int count = mlist.Count();
            int i = 0;
            foreach(var m in mlist)
            {

                //输出开始标记
                if (i == 0&&parentid > 0&& parentname != "sys_contents")
                {
                    context.Response.Write("<ul>\n");
                }
                i++;
                //以下是输出选项内容=======================
                if (m.ModuleLayer == 1)
                {
                    context.Response.Write("<div class=\"list-group\" icon=\""+m.Icon+"\" name=\"" +m.ModuleName + "\">\n");
                    if (m.ModuleCode != "sys_contents")
                    {
                        context.Response.Write("<h2>" + m.ModuleName + "<i></i></h2>\n");
                    }
                    else
                    {
                        if (siteid > 0)
                        {
                            var weblist = GetWebTreeList(user, siteid);
                            this.get_column_childs(context, weblist, m.ModuleID, siteid);
                        }
                    }
                    //调用自身迭代
                    this.get_navigation_childs(context, list,user, m.ModuleID);
                    context.Response.Write("</div>\n");
                }
                else
                {
                    context.Response.Write("<li>\n");
                    context.Response.Write("<a navid=\"" + m.ModuleCode + "\"");
                    if (!string.IsNullOrEmpty(m.ModuleUrl.Trim()))
                    {
                        if (m.ModuleUrl.IndexOf("?") > -1)
                        {
                            context.Response.Write(" href=\"" + m.ModuleUrl.Trim() + "&mid=" + m.ModuleID + "&siteid=" + siteid + "\" target=\"mainframe\"");
                        }
                        else
                        {
                            context.Response.Write(" href=\"" + m.ModuleUrl.Trim() + "?mid=" + m.ModuleID + "&siteid=" + siteid + "\" target=\"mainframe\"");
                        }
                    }
                    context.Response.Write(" class=\"item\">\n");
                    context.Response.Write("<span>" + m.ModuleName + "</span>\n");
                    context.Response.Write("</a>\n");
                    //调用自身迭代
                    this.get_navigation_childs(context, list,user, m.ModuleID, m.ModuleCode);
                    context.Response.Write("</li>\n");
                }
                //以上是输出选项内容=======================
                //输出结束标记
                if (i == count && parentid > 0 && parentname != "sys_contents")
                {
                    context.Response.Write("</ul>\n");
                }
            }
        }
        //网站菜单
        private void get_column_childs(HttpContext context, List<ChannelColumn> list,int mid,int sid)
        {
            var mlist = list.Where(p=>p.PId==0).ToList();
            int count = mlist.Count();
            int i = 0;
            system_site site = db.GetModel<system_site>(sid);
            context.Response.Write("<h2>" + site.SiteName + "<i></i></h2>\n");
            foreach (var m in mlist)
            {

                //输出开始标记
                if (i == 0)
                {
                    context.Response.Write("<ul>\n");
                }
                i++;
                context.Response.Write("<li>\n");
                context.Response.Write("<a navid=\"" + m.Id + "_" + m.ColumnId + "\"");
                context.Response.Write(" class=\"item\">\n");
                context.Response.Write("<span>" + m.Name + "</span>\n");
                context.Response.Write("</a>\n");
                
                var mlist2=list.Where(p=>p.PId==m.Id).ToList();
                int j = 0;
                int count2 = mlist2.Count();
                foreach (var m2 in mlist2)
                {
                    if (j == 0)
                    {
                        context.Response.Write("<ul>\n");
                    }
                    j++;
                    context.Response.Write("<li>\n");
                    context.Response.Write("<a navid=\"" + m2.Id + "_" + m2.ColumnId + "\"");
                    if (!string.IsNullOrEmpty(m2.Url.Trim()))
                    {
                        if (m2.Url.IndexOf("?") > -1)
                        {
                            context.Response.Write(" href=\"" + m2.Url.Trim() + "&mid=" + mid + "&siteid=" + sid + "&channelid=" + m2.Id + "&columntype=" + m2.ColumnId + "\" target=\"mainframe\"");
                        }
                        else
                        {
                            context.Response.Write(" href=\"" + m2.Url.Trim() + "?mid=" + mid + "&siteid=" + sid + "&channelid=" + m2.Id + "&columntype=" + m2.ColumnId + "\" target=\"mainframe\"");
                        }
                    }
                    context.Response.Write(" class=\"item\">\n");
                    context.Response.Write("<span>" + m2.Name + "</span>\n");
                    context.Response.Write("</a>\n");
                    context.Response.Write("</li>\n");
                    if (j == count2)
                    {
                        context.Response.Write("</ul>\n");
                    }
                }
                context.Response.Write("</li>\n");
                //以上是输出选项内容=======================
                //输出结束标记
                if (i == count)
                {
                    context.Response.Write("</ul>\n");
                }
            }
        }
        /// <summary>
        /// 递归树表，按层级关系，输出网站频道列表
        /// </summary>
        /// <param name="siteid"></param>
        /// <returns></returns>
        private List<ChannelColumn> GetWebTreeList(system_user user, int siteid)
        {
            List<ChannelColumn> list = new List<ChannelColumn>();
            if (user.UserType == 3)
            {
                var chlist = db.GetModelList<Model.channel>(" SiteID=" + siteid + " AND Status=1 ORDER BY OrderBy ASC");
                var ctlist = Utils.ColumnTypeList();
                foreach (var ch in chlist)
                {
                    list.Add(new ChannelColumn { Id = ch.ChannelID,PId=0, ColumnId = 0, Name = ch.ChannelName, Layer = 1,Url=""});
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
                            list.Add(new ChannelColumn { Id = ch.ChannelID, PId = ch.ChannelID, ColumnId = ct.Id, Name = ct.Name, Layer = 2, Url = ct.Url });
                        }
                    }
                }
            }
            else
            {
                var chlist = db.GetModelList<Model.channel>(" SiteID=" + siteid + " AND Status=1  AND ChannelID IN(SELECT ChannelID FROM ccphl_system_web_column WHERE RoleID IN(SELECT RoleID FROM ccphl_system_role_user WHERE UserID=" + user.UserID + ") AND ButtonAuthority>0) ORDER BY OrderBy ASC ");
                var ctlist0 = Utils.ColumnTypeList();
                foreach (var ch in chlist)
                {
                    list.Add(new ChannelColumn { Id = ch.ChannelID, PId = 0, ColumnId = 0, Name = ch.ChannelName, Layer = 1, Url = "" });
                    var mlist = db.GetModelList<Model.system_web_column>(" SiteID=" + siteid + " AND ChannelID=" + ch.ChannelID + " AND ButtonAuthority>0");
                    List<ccphl.Common.Utils.ColumnType> ctlist = new List<ccphl.Common.Utils.ColumnType>();
                    foreach (var m in mlist)
                    {
                        if (ctlist0.Count(p => p.Id == m.ColumnType)==1)
                        {
                            ctlist.Add(ctlist0.Where(k=>k.Id==m.ColumnType).First());
                        }
                    }
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
                            list.Add(new ChannelColumn { Id = ch.ChannelID, PId = ch.ChannelID, ColumnId = ct.Id, Name = ct.Name, Layer = 2, Url = ct.Url });
                        }
                    }
                }
            }
            return list;

        }
        #endregion

        //#region 修改订单信息和状态==============================
        //private void edit_order_status(HttpContext context)
        //{
        //    //取得管理员登录信息
        //    Model.manager adminInfo = new Web.UI.ManagePage().GetAdminInfo();
        //    if (adminInfo == null)
        //    {
        //        context.Response.Write("{\"status\": 0, \"msg\": \"未登录或已超时，请重新登录！\"}");
        //        return;
        //    }
        //    //取得站点配置信息
        //    Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig();
        //    //取得订单配置信息
        //    Model.orderconfig orderConfig = new BLL.orderconfig().loadConfig();

        //    string order_no = DTRequest.GetString("order_no");
        //    string edit_type = DTRequest.GetString("edit_type");
        //    if (order_no == "")
        //    {
        //        context.Response.Write("{\"status\": 0, \"msg\": \"传输参数有误，无法获取订单号！\"}");
        //        return;
        //    }
        //    if (edit_type == "")
        //    {
        //        context.Response.Write("{\"status\": 0, \"msg\": \"无法获取修改订单类型！\"}");
        //        return;
        //    }

        //    BLL.orders bll = new BLL.orders();
        //    Model.orders model = bll.GetModel(order_no);
        //    if (model == null)
        //    {
        //        context.Response.Write("{\"status\": 0, \"msg\": \"订单号不存在或已被删除！\"}");
        //        return;
        //    }
        //    switch (edit_type.ToLower())
        //    {
        //        case "order_confirm": //确认订单
        //            //检查权限
        //            if (!new BLL.manager_role().Exists(adminInfo.role_id, "order_list", DTEnums.ActionEnum.Confirm.ToString()))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"您没有确认订单的权限！\"}");
        //                return;
        //            }
        //            if (model.status > 1)
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"订单已经确认，不能重复处理！\"}");
        //                return;
        //            }
        //            model.status = 2;
        //            model.confirm_time = DateTime.Now;
        //            if (!bll.Update(model))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"订单确认失败！\"}");
        //                return;
        //            }
        //            new BLL.manager_log().Add(adminInfo.id, adminInfo.user_name, DTEnums.ActionEnum.Confirm.ToString(), "确认订单号:" + model.order_no); //记录日志
        //            #region 发送短信或邮件
        //            if (orderConfig.confirmmsg > 0)
        //            {
        //                switch (orderConfig.confirmmsg)
        //                {
        //                    case 1: //短信通知
        //                        if (string.IsNullOrEmpty(model.mobile))
        //                        {
        //                            context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功，但无法发送短信<br/ >对方未填写手机号码！\"}");
        //                            return;
        //                        }
        //                        Model.sms_template smsModel = new BLL.sms_template().GetModel(orderConfig.confirmcallindex); //取得短信内容
        //                        if (smsModel == null)
        //                        {
        //                            context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功，但无法发送短信<br/ >短信通知模板不存在！\"}");
        //                            return;
        //                        }
        //                        //替换标签
        //                        string msgContent = smsModel.content;
        //                        msgContent = msgContent.Replace("{webname}", siteConfig.webname);
        //                        msgContent = msgContent.Replace("{username}", model.accept_name);
        //                        msgContent = msgContent.Replace("{orderno}", model.order_no);
        //                        msgContent = msgContent.Replace("{amount}", model.order_amount.ToString());
        //                        //发送短信
        //                        string tipMsg = string.Empty;
        //                        bool sendStatus = new BLL.sms_message().Send(model.mobile, msgContent, 2, out tipMsg);
        //                        if (!sendStatus)
        //                        {
        //                            context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功，但无法发送短信<br/ >" + tipMsg + "\"}");
        //                            return;
        //                        }
        //                        break;
        //                    case 2: //邮件通知
        //                        //取得用户的邮箱地址
        //                        if (model.user_id > 0)
        //                        {
        //                            Model.users userModel = new BLL.users().GetModel(model.user_id);
        //                            if (userModel == null || string.IsNullOrEmpty(userModel.email))
        //                            {
        //                                context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功，但无法发送邮件<br/ >该用户不存在或没有填写邮箱地址。\"}");
        //                                return;
        //                            }
        //                            //取得邮件模板内容
        //                            Model.mail_template mailModel = new BLL.mail_template().GetModel(orderConfig.confirmcallindex);
        //                            if (mailModel == null)
        //                            {
        //                                context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功，但无法发送邮件<br/ >邮件通知模板不存在。\"}");
        //                                return;
        //                            }
        //                            //替换标签
        //                            string mailTitle = mailModel.maill_title;
        //                            mailTitle = mailTitle.Replace("{username}", model.user_name);
        //                            string mailContent = mailModel.content;
        //                            mailContent = mailContent.Replace("{webname}", siteConfig.webname);
        //                            mailContent = mailContent.Replace("{weburl}", siteConfig.weburl);
        //                            mailContent = mailContent.Replace("{webtel}", siteConfig.webtel);
        //                            mailContent = mailContent.Replace("{username}", model.user_name);
        //                            mailContent = mailContent.Replace("{orderno}", model.order_no);
        //                            mailContent = mailContent.Replace("{amount}", model.order_amount.ToString());
        //                            //发送邮件
        //                            DTMail.sendMail(siteConfig.emailsmtp, siteConfig.emailusername, siteConfig.emailpassword, siteConfig.emailnickname,
        //                                siteConfig.emailfrom, userModel.email, mailTitle, mailContent);
        //                        }
        //                        break;
        //                }
        //            }
        //            #endregion
        //            context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功！\"}");
        //            break;
        //        case "order_payment": //确认付款
        //            //检查权限
        //            if (!new BLL.manager_role().Exists(adminInfo.role_id, "order_list", DTEnums.ActionEnum.Confirm.ToString()))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"您没有确认付款的权限！\"}");
        //                return;
        //            }
        //            if (model.status > 1 || model.payment_status == 2)
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"订单已确认，不能重复处理！\"}");
        //                return;
        //            }
        //            model.payment_status = 2;
        //            model.payment_time = DateTime.Now;
        //            model.status = 2;
        //            model.confirm_time = DateTime.Now;
        //            if (!bll.Update(model))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"订单确认付款失败！\"}");
        //                return;
        //            }
        //            new BLL.manager_log().Add(adminInfo.id, adminInfo.user_name, DTEnums.ActionEnum.Confirm.ToString(), "确认付款订单号:" + model.order_no); //记录日志
        //            #region 发送短信或邮件
        //            if (orderConfig.confirmmsg > 0)
        //            {
        //                switch (orderConfig.confirmmsg)
        //                {
        //                    case 1: //短信通知
        //                        if (string.IsNullOrEmpty(model.mobile))
        //                        {
        //                            context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功，但无法发送短信<br/ >对方未填写手机号码！\"}");
        //                            return;
        //                        }
        //                        Model.sms_template smsModel = new BLL.sms_template().GetModel(orderConfig.confirmcallindex); //取得短信内容
        //                        if (smsModel == null)
        //                        {
        //                            context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功，但无法发送短信<br/ >短信通知模板不存在！\"}");
        //                            return;
        //                        }
        //                        //替换标签
        //                        string msgContent = smsModel.content;
        //                        msgContent = msgContent.Replace("{webname}", siteConfig.webname);
        //                        msgContent = msgContent.Replace("{username}", model.user_name);
        //                        msgContent = msgContent.Replace("{orderno}", model.order_no);
        //                        msgContent = msgContent.Replace("{amount}", model.order_amount.ToString());
        //                        //发送短信
        //                        string tipMsg = string.Empty;
        //                        bool sendStatus = new BLL.sms_message().Send(model.mobile, msgContent, 2, out tipMsg);
        //                        if (!sendStatus)
        //                        {
        //                            context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功，但无法发送短信<br/ >" + tipMsg + "\"}");
        //                            return;
        //                        }
        //                        break;
        //                    case 2: //邮件通知
        //                        //取得用户的邮箱地址
        //                        if (model.user_id > 0)
        //                        {
        //                            Model.users userModel = new BLL.users().GetModel(model.user_id);
        //                            if (userModel == null || string.IsNullOrEmpty(userModel.email))
        //                            {
        //                                context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功，但无法发送邮件<br/ >该用户不存在或没有填写邮箱地址。\"}");
        //                                return;
        //                            }
        //                            //取得邮件模板内容
        //                            Model.mail_template mailModel = new BLL.mail_template().GetModel(orderConfig.confirmcallindex);
        //                            if (mailModel == null)
        //                            {
        //                                context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功，但无法发送邮件<br/ >邮件通知模板不存在。\"}");
        //                                return;
        //                            }
        //                            //替换标签
        //                            string mailTitle = mailModel.maill_title;
        //                            mailTitle = mailTitle.Replace("{username}", model.user_name);
        //                            string mailContent = mailModel.content;
        //                            mailContent = mailContent.Replace("{webname}", siteConfig.webname);
        //                            mailContent = mailContent.Replace("{weburl}", siteConfig.weburl);
        //                            mailContent = mailContent.Replace("{webtel}", siteConfig.webtel);
        //                            mailContent = mailContent.Replace("{username}", model.user_name);
        //                            mailContent = mailContent.Replace("{orderno}", model.order_no);
        //                            mailContent = mailContent.Replace("{amount}", model.order_amount.ToString());
        //                            //发送邮件
        //                            DTMail.sendMail(siteConfig.emailsmtp, siteConfig.emailusername, siteConfig.emailpassword, siteConfig.emailnickname,
        //                                siteConfig.emailfrom, userModel.email, mailTitle, mailContent);
        //                        }
        //                        break;
        //                }
        //            }
        //            #endregion
        //            context.Response.Write("{\"status\": 1, \"msg\": \"订单确认付款成功！\"}");
        //            break;
        //        case "order_express": //确认发货
        //            //检查权限
        //            if (!new BLL.manager_role().Exists(adminInfo.role_id, "order_list", DTEnums.ActionEnum.Confirm.ToString()))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"您没有确认发货的权限！\"}");
        //                return;
        //            }
        //            if (model.status > 2 || model.express_status == 2)
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"订单已完成或已发货，不能重复处理！\"}");
        //                return;
        //            }
        //            int express_id = DTRequest.GetFormInt("express_id");
        //            string express_no = DTRequest.GetFormString("express_no");
        //            if (express_id == 0)
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"请选择配送方式！\"}");
        //                return;
        //            }
        //            model.express_id = express_id;
        //            model.express_no = express_no;
        //            model.express_status = 2;
        //            model.express_time = DateTime.Now;
        //            if (!bll.Update(model))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"订单发货失败！\"}");
        //                return;
        //            }
        //            new BLL.manager_log().Add(adminInfo.id, adminInfo.user_name, DTEnums.ActionEnum.Confirm.ToString(), "确认发货订单号:" + model.order_no); //记录日志
        //            #region 发送短信或邮件
        //            if (orderConfig.expressmsg > 0)
        //            {
        //                switch (orderConfig.expressmsg)
        //                {
        //                    case 1: //短信通知
        //                        if (string.IsNullOrEmpty(model.mobile))
        //                        {
        //                            context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功，但无法发送短信<br/ >对方未填写手机号码！\"}");
        //                            return;
        //                        }
        //                        Model.sms_template smsModel = new BLL.sms_template().GetModel(orderConfig.expresscallindex); //取得短信内容
        //                        if (smsModel == null)
        //                        {
        //                            context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功，但无法发送短信<br/ >短信通知模板不存在！\"}");
        //                            return;
        //                        }
        //                        //替换标签
        //                        string msgContent = smsModel.content;
        //                        msgContent = msgContent.Replace("{webname}", siteConfig.webname);
        //                        msgContent = msgContent.Replace("{username}", model.user_name);
        //                        msgContent = msgContent.Replace("{orderno}", model.order_no);
        //                        msgContent = msgContent.Replace("{amount}", model.order_amount.ToString());
        //                        //发送短信
        //                        string tipMsg = string.Empty;
        //                        bool sendStatus = new BLL.sms_message().Send(model.mobile, msgContent, 2, out tipMsg);
        //                        if (!sendStatus)
        //                        {
        //                            context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功，但无法发送短信<br/ >" + tipMsg + "\"}");
        //                            return;
        //                        }
        //                        break;
        //                    case 2: //邮件通知
        //                        //取得用户的邮箱地址
        //                        if (model.user_id > 0)
        //                        {
        //                            Model.users userModel = new BLL.users().GetModel(model.user_id);
        //                            if (userModel == null || string.IsNullOrEmpty(userModel.email))
        //                            {
        //                                context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功，但无法发送邮件<br/ >该用户不存在或没有填写邮箱地址。\"}");
        //                                return;
        //                            }
        //                            //取得邮件模板内容
        //                            Model.mail_template mailModel = new BLL.mail_template().GetModel(orderConfig.expresscallindex);
        //                            if (mailModel == null)
        //                            {
        //                                context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功，但无法发送邮件<br/ >邮件通知模板不存在。\"}");
        //                                return;
        //                            }
        //                            //替换标签
        //                            string mailTitle = mailModel.maill_title;
        //                            mailTitle = mailTitle.Replace("{username}", model.user_name);
        //                            string mailContent = mailModel.content;
        //                            mailContent = mailContent.Replace("{webname}", siteConfig.webname);
        //                            mailContent = mailContent.Replace("{weburl}", siteConfig.weburl);
        //                            mailContent = mailContent.Replace("{webtel}", siteConfig.webtel);
        //                            mailContent = mailContent.Replace("{username}", model.user_name);
        //                            mailContent = mailContent.Replace("{orderno}", model.order_no);
        //                            mailContent = mailContent.Replace("{amount}", model.order_amount.ToString());
        //                            //发送邮件
        //                            DTMail.sendMail(siteConfig.emailsmtp, siteConfig.emailusername, siteConfig.emailpassword, siteConfig.emailnickname,
        //                                siteConfig.emailfrom, userModel.email, mailTitle, mailContent);
        //                        }
        //                        break;
        //                }
        //            }
        //            #endregion
        //            context.Response.Write("{\"status\": 1, \"msg\": \"订单发货成功！\"}");
        //            break;
        //        case "order_complete": //完成订单=========================================
        //            //检查权限
        //            if (!new BLL.manager_role().Exists(adminInfo.role_id, "order_list", DTEnums.ActionEnum.Confirm.ToString()))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"您没有确认完成订单的权限！\"}");
        //                return;
        //            }
        //            if (model.status > 2)
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"订单已经完成，不能重复处理！\"}");
        //                return;
        //            }
        //            model.status = 3;
        //            model.complete_time = DateTime.Now;
        //            if (!bll.Update(model))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"确认订单完成失败！\"}");
        //                return;
        //            }
        //            //给会员增加积分检查升级
        //            if (model.user_id > 0 && model.point > 0)
        //            {
        //                new BLL.user_point_log().Add(model.user_id, model.user_name, model.point, "购物获得积分，订单号：" + model.order_no, true);
        //            }
        //            new BLL.manager_log().Add(adminInfo.id, adminInfo.user_name, DTEnums.ActionEnum.Confirm.ToString(), "确认交易完成订单号:" + model.order_no); //记录日志
        //            #region 发送短信或邮件
        //            if (orderConfig.completemsg > 0)
        //            {
        //                switch (orderConfig.completemsg)
        //                {
        //                    case 1: //短信通知
        //                        if (string.IsNullOrEmpty(model.mobile))
        //                        {
        //                            context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功，但无法发送短信<br/ >对方未填写手机号码！\"}");
        //                            return;
        //                        }
        //                        Model.sms_template smsModel = new BLL.sms_template().GetModel(orderConfig.completecallindex); //取得短信内容
        //                        if (smsModel == null)
        //                        {
        //                            context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功，但无法发送短信<br/ >短信通知模板不存在！\"}");
        //                            return;
        //                        }
        //                        //替换标签
        //                        string msgContent = smsModel.content;
        //                        msgContent = msgContent.Replace("{webname}", siteConfig.webname);
        //                        msgContent = msgContent.Replace("{username}", model.user_name);
        //                        msgContent = msgContent.Replace("{orderno}", model.order_no);
        //                        msgContent = msgContent.Replace("{amount}", model.order_amount.ToString());
        //                        //发送短信
        //                        string tipMsg = string.Empty;
        //                        bool sendStatus = new BLL.sms_message().Send(model.mobile, msgContent, 2, out tipMsg);
        //                        if (!sendStatus)
        //                        {
        //                            context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功，但无法发送短信<br/ >" + tipMsg + "\"}");
        //                            return;
        //                        }
        //                        break;
        //                    case 2: //邮件通知
        //                        //取得用户的邮箱地址
        //                        if (model.user_id > 0)
        //                        {
        //                            Model.users userModel = new BLL.users().GetModel(model.user_id);
        //                            if (userModel == null || string.IsNullOrEmpty(userModel.email))
        //                            {
        //                                context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功，但无法发送邮件<br/ >该用户不存在或没有填写邮箱地址。\"}");
        //                                return;
        //                            }
        //                            //取得邮件模板内容
        //                            Model.mail_template mailModel = new BLL.mail_template().GetModel(orderConfig.completecallindex);
        //                            if (mailModel == null)
        //                            {
        //                                context.Response.Write("{\"status\": 1, \"msg\": \"订单确认成功，但无法发送邮件<br/ >邮件通知模板不存在。\"}");
        //                                return;
        //                            }
        //                            //替换标签
        //                            string mailTitle = mailModel.maill_title;
        //                            mailTitle = mailTitle.Replace("{username}", model.user_name);
        //                            string mailContent = mailModel.content;
        //                            mailContent = mailContent.Replace("{webname}", siteConfig.webname);
        //                            mailContent = mailContent.Replace("{weburl}", siteConfig.weburl);
        //                            mailContent = mailContent.Replace("{webtel}", siteConfig.webtel);
        //                            mailContent = mailContent.Replace("{username}", model.user_name);
        //                            mailContent = mailContent.Replace("{orderno}", model.order_no);
        //                            mailContent = mailContent.Replace("{amount}", model.order_amount.ToString());
        //                            //发送邮件
        //                            DTMail.sendMail(siteConfig.emailsmtp, siteConfig.emailusername, siteConfig.emailpassword, siteConfig.emailnickname,
        //                                siteConfig.emailfrom, userModel.email, mailTitle, mailContent);
        //                        }
        //                        break;
        //                }
        //            }
        //            #endregion
        //            context.Response.Write("{\"status\": 1, \"msg\": \"确认订单完成成功！\"}");
        //            break;
        //        case "order_cancel": //取消订单==========================================
        //            //检查权限
        //            if (!new BLL.manager_role().Exists(adminInfo.role_id, "order_list", DTEnums.ActionEnum.Cancel.ToString()))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"您没有取消订单的权限！\"}");
        //                return;
        //            }
        //            if (model.status > 2)
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"订单已经完成，不能取消订单！\"}");
        //                return;
        //            }
        //            model.status = 4;
        //            if (!bll.Update(model))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"取消订单失败！\"}");
        //                return;
        //            }
        //            int check_revert1 = DTRequest.GetFormInt("check_revert");
        //            if (check_revert1 == 1)
        //            {
        //                //如果存在积分换购则返还会员积分
        //                if (model.user_id > 0 && model.point < 0)
        //                {
        //                    new BLL.user_point_log().Add(model.user_id, model.user_name, (model.point * -1), "取消订单返还积分，订单号：" + model.order_no, false);
        //                }
        //                //如果已支付则退还金额到会员账户
        //                if (model.user_id > 0 && model.payment_status == 2 && model.order_amount > 0)
        //                {
        //                    new BLL.user_amount_log().Add(model.user_id, model.user_name, DTEnums.AmountTypeEnum.BuyGoods.ToString(), model.order_amount, "取消订单退还金额，订单号：" + model.order_no);
        //                }
        //            }
        //            new BLL.manager_log().Add(adminInfo.id, adminInfo.user_name, DTEnums.ActionEnum.Cancel.ToString(), "取消订单号:" + model.order_no); //记录日志
        //            context.Response.Write("{\"status\": 1, \"msg\": \"取消订单成功！\"}");
        //            break;
        //        case "order_invalid": //作废订单==========================================
        //            //检查权限
        //            if (!new BLL.manager_role().Exists(adminInfo.role_id, "order_list", DTEnums.ActionEnum.Invalid.ToString()))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"您没有作废订单的权限！\"}");
        //                return;
        //            }
        //            if (model.status != 3)
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"订单尚未完成，不能作废订单！\"}");
        //                return;
        //            }
        //            model.status = 5;
        //            if (!bll.Update(model))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"作废订单失败！\"}");
        //                return;
        //            }
        //            int check_revert2 = DTRequest.GetFormInt("check_revert");
        //            if (check_revert2 == 1)
        //            {
        //                //扣除购物赠送的积分
        //                if (model.user_id > 0 && model.point > 0)
        //                {
        //                    new BLL.user_point_log().Add(model.user_id, model.user_name, (model.point * -1), "作废订单扣除积分，订单号：" + model.order_no, false);
        //                }
        //                //退还金额到会员账户
        //                if (model.user_id > 0 && model.order_amount > 0)
        //                {
        //                    new BLL.user_amount_log().Add(model.user_id, model.user_name, DTEnums.AmountTypeEnum.BuyGoods.ToString(), model.order_amount, "取消订单退还金额，订单号：" + model.order_no);
        //                }
        //            }
        //            new BLL.manager_log().Add(adminInfo.id, adminInfo.user_name, DTEnums.ActionEnum.Invalid.ToString(), "作废订单号:" + model.order_no); //记录日志
        //            context.Response.Write("{\"status\": 1, \"msg\": \"作废订单成功！\"}");
        //            break;
        //        case "edit_accept_info": //修改收货信息====================================
        //            //检查权限
        //            if (!new BLL.manager_role().Exists(adminInfo.role_id, "order_list", DTEnums.ActionEnum.Edit.ToString()))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"您没有修改收货信息的权限！\"}");
        //                return;
        //            }
        //            if (model.express_status == 2)
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"订单已经发货，不能修改收货信息！\"}");
        //                return;
        //            }
        //            string accept_name = DTRequest.GetFormString("accept_name");
        //            string province = DTRequest.GetFormString("province");
        //            string city = DTRequest.GetFormString("city");
        //            string area = DTRequest.GetFormString("area");
        //            string address = DTRequest.GetFormString("address");
        //            string post_code = DTRequest.GetFormString("post_code");
        //            string mobile = DTRequest.GetFormString("mobile");
        //            string telphone = DTRequest.GetFormString("telphone");

        //            if (accept_name == "")
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"请填写收货人姓名！\"}");
        //                return;
        //            }
        //            if (area == "")
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"请选择所在地区！\"}");
        //                return;
        //            }
        //            if (address == "")
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"请填写详细的送货地址！\"}");
        //                return;
        //            }
        //            if (mobile == "" && telphone == "")
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"联系手机或电话至少填写一项！\"}");
        //                return;
        //            }

        //            model.accept_name = accept_name;
        //            model.area = province + "," + city + "," + area;
        //            model.address = address;
        //            model.post_code = post_code;
        //            model.mobile = mobile;
        //            model.telphone = telphone;
        //            if (!bll.Update(model))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"修改收货人信息失败！\"}");
        //                return;
        //            }
        //            new BLL.manager_log().Add(adminInfo.id, adminInfo.user_name, DTEnums.ActionEnum.Edit.ToString(), "修改收货信息，订单号:" + model.order_no); //记录日志
        //            context.Response.Write("{\"status\": 1, \"msg\": \"修改收货人信息成功！\"}");
        //            break;
        //        case "edit_order_remark": //修改订单备注=================================
        //            string remark = DTRequest.GetFormString("remark");
        //            if (remark == "")
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"请填写订单备注内容！\"}");
        //                return;
        //            }
        //            model.remark = remark;
        //            if (!bll.Update(model))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"修改订单备注失败！\"}");
        //                return;
        //            }
        //            new BLL.manager_log().Add(adminInfo.id, adminInfo.user_name, DTEnums.ActionEnum.Edit.ToString(), "修改订单备注，订单号:" + model.order_no); //记录日志
        //            context.Response.Write("{\"status\": 1, \"msg\": \"修改订单备注成功！\"}");
        //            break;
        //        case "edit_real_amount": //修改商品总金额================================
        //            //检查权限
        //            if (!new BLL.manager_role().Exists(adminInfo.role_id, "order_list", DTEnums.ActionEnum.Edit.ToString()))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"您没有修改商品金额的权限！\"}");
        //                return;
        //            }
        //            if (model.status > 1)
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"订单已经确认，不能修改金额！\"}");
        //                return;
        //            }
        //            decimal real_amount = DTRequest.GetFormDecimal("real_amount", 0);
        //            model.real_amount = real_amount;
        //            if (!bll.Update(model))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"修改商品总金额失败！\"}");
        //                return;
        //            }
        //            new BLL.manager_log().Add(adminInfo.id, adminInfo.user_name, DTEnums.ActionEnum.Edit.ToString(), "修改商品金额，订单号:" + model.order_no); //记录日志
        //            context.Response.Write("{\"status\": 1, \"msg\": \"修改商品总金额成功！\"}");
        //            break;
        //        case "edit_express_fee": //修改配送费用==================================
        //            //检查权限
        //            if (!new BLL.manager_role().Exists(adminInfo.role_id, "order_list", DTEnums.ActionEnum.Edit.ToString()))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"您没有配送费用的权限！\"}");
        //                return;
        //            }
        //            if (model.status > 1)
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"订单已经确认，不能修改金额！\"}");
        //                return;
        //            }
        //            decimal express_fee = DTRequest.GetFormDecimal("express_fee", 0);
        //            model.express_fee = express_fee;
        //            if (!bll.Update(model))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"修改配送费用失败！\"}");
        //                return;
        //            }
        //            new BLL.manager_log().Add(adminInfo.id, adminInfo.user_name, DTEnums.ActionEnum.Edit.ToString(), "修改配送费用，订单号:" + model.order_no); //记录日志
        //            context.Response.Write("{\"status\": 1, \"msg\": \"修改配送费用成功！\"}");
        //            break;
        //        case "edit_payment_fee": //修改支付手续费=================================
        //            //检查权限
        //            if (!new BLL.manager_role().Exists(adminInfo.role_id, "order_list", DTEnums.ActionEnum.Edit.ToString()))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"您没有修改支付手续费的权限！\"}");
        //                return;
        //            }
        //            if (model.status > 1)
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"订单已经确认，不能修改金额！\"}");
        //                return;
        //            }
        //            decimal payment_fee = DTRequest.GetFormDecimal("payment_fee", 0);
        //            model.payment_fee = payment_fee;
        //            if (!bll.Update(model))
        //            {
        //                context.Response.Write("{\"status\": 0, \"msg\": \"修改支付手续费失败！\"}");
        //                return;
        //            }
        //            new BLL.manager_log().Add(adminInfo.id, adminInfo.user_name, DTEnums.ActionEnum.Edit.ToString(), "修改支付手续费，订单号:" + model.order_no); //记录日志
        //            context.Response.Write("{\"status\": 1, \"msg\": \"修改支付手续费成功！\"}");
        //            break;
        //    }

        //}
        //#endregion

        //#region 验证用户名是否可用==============================
        //private void validate_username(HttpContext context)
        //{
        //    string user_name = DTRequest.GetString("param");
        //    //如果为Null，退出
        //    if (string.IsNullOrEmpty(user_name))
        //    {
        //        context.Response.Write("{ \"info\":\"请输入用户名\", \"status\":\"n\" }");
        //        return;
        //    }
        //    Model.userconfig userConfig = new BLL.userconfig().loadConfig();
        //    //过滤注册用户名字符
        //    string[] strArray = userConfig.regkeywords.Split(',');
        //    foreach (string s in strArray)
        //    {
        //        if (s.ToLower() == user_name.ToLower())
        //        {
        //            context.Response.Write("{ \"info\":\"用户名不可用\", \"status\":\"n\" }");
        //            return;
        //        }
        //    }
        //    BLL.users bll = new BLL.users();
        //    //查询数据库
        //    if (bll.Exists(user_name.Trim()))
        //    {
        //        context.Response.Write("{ \"info\":\"用户名已重复\", \"status\":\"n\" }");
        //        return;
        //    }
        //    context.Response.Write("{ \"info\":\"用户名可用\", \"status\":\"y\" }");
        //    return;
        //}
        //#endregion

        //#region 发送手机短信====================================
        //private void sms_message_post(HttpContext context)
        //{
        //    //检查管理员是否登录
        //    if (!new ManagePage().IsAdminLogin())
        //    {
        //        context.Response.Write("{\"status\": 0, \"msg\": \"尚未登录或已超时，请登录后操作！\"}");
        //        return;
        //    }
        //    string mobiles = DTRequest.GetFormString("mobiles");
        //    string content = DTRequest.GetFormString("content");
        //    if (mobiles == "")
        //    {
        //        context.Response.Write("{\"status\": 0, \"msg\": \"手机号码不能为空！\"}");
        //        return;
        //    }
        //    if (content == "")
        //    {
        //        context.Response.Write("{\"status\": 0, \"msg\": \"短信内容不能为空！\"}");
        //        return;
        //    }
        //    //开始发送
        //    string msg = string.Empty;
        //    bool result = new BLL.sms_message().Send(mobiles, content, 2, out msg);
        //    if (result)
        //    {
        //        context.Response.Write("{\"status\": 1, \"msg\": \"" + msg + "\"}");
        //        return;
        //    }
        //    context.Response.Write("{\"status\": 0, \"msg\": \"" + msg + "\"}");
        //    return;
        //}
        //#endregion

        //#region 获取要生成静态的地址============================
        //private void get_builder_urls(HttpContext context)
        //{
        //    int state = GetIsLoginAndIsStaticstatus();
        //    if (state == 1)
        //        new HtmlBuilder().getpublishsite(context);
        //    else
        //        context.Response.Write(state);
        //}
        //#endregion

        //#region 生成静态页面====================================
        //private void get_builder_html(HttpContext context)
        //{
        //    int state = GetIsLoginAndIsStaticstatus();
        //    if (state == 1)
        //        new HtmlBuilder().handleHtml(context);
        //    else
        //        context.Response.Write(state);


        //}
        //#endregion

        //#region 判断是否登陆以及是否开启静态====================
        //private int GetIsLoginAndIsStaticstatus()
        //{
        //    Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig();
        //    //取得管理员登录信息
        //    Model.manager adminInfo = new Web.UI.ManagePage().GetAdminInfo();
        //    if (adminInfo == null)
        //        return  -1;
        //    else if (!new BLL.manager_role().Exists(adminInfo.role_id, "app_builder_html", DTEnums.ActionEnum.Build.ToString()))
        //        return -2;
        //    else if (siteConfig.staticstatus != 2)
        //        return -3;
        //    else
        //        return 1;
        //}
        //#endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}