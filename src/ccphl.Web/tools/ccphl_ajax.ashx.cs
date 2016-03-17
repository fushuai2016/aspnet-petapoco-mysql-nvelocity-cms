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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace ccphl.Web.tools
{
    /// <summary>
    /// 为民服务站AJAX处理页
    /// </summary>
    public class ccphl_ajax : IHttpHandler, IRequiresSessionState
    {
        private MySqlUtility db = new MySqlUtility();
        private MySqlHelper db2 = MySqlHelper.GetInstance();
        static private int state = 1;
        public void ProcessRequest(HttpContext context)
        {
            //取得处事类型
            string action = DTRequest.GetQueryString("action");

            switch (action)
            {
                //case "region_post": //为民服务站点 数据更新
                //    region_post(context);
                //    break;
                case "region_pic_get": //为民服务站点 登陆数据获取
                    region_pic_get(context);
                    break;
                case "region_get": //为民服务站点 登陆数据获取
                    region_get(context);
                    break;
                case "region": //为民服务站点 登陆数据获取
                    region(context);
                    break;
                case "browserinfo": //为民服务站点 
                    browserinfo(context);
                    break;
                case "browserinfo_all": //为民服务站点 
                    browserinfo_all(context);
                    break;
                case "get_browserinfo": //为民服务站点 
                    get_browserinfo(context);
                    break;
                case "get_browserinfo_all": //为民服务站点 
                    get_browserinfo_all(context);
                    break;
              
            }

        }
        private void region(HttpContext context)
        {
            if (state != 1)
            {
                return;
            }
            state = 0;
            try
            {
                var list = db.GetModelList<regioninfo>("");
                foreach (var m in list)
                {
                    var reg = db.GetModel<region>(" RegionCode='" + m.RegionCode + "'");
                    if (reg.ID > 0)
                    {
                        reg.OrganCode = m.ORGCode;
                        reg.DepartmentCode = m.DepartmentID;
                        if (!string.IsNullOrWhiteSpace(m.DotNetApi))
                            reg.RegionUrl = m.DotNetApi.TrimEnd('/');
                        reg.Update();
                    }
                }
                state = 1;
            }
            catch { state = 1; }
            
        }
        #region 为民服务站点 数据更新============================
        private void region_post(HttpContext context)
        {
            if (state != 1)
            {
                return;
            }
            state = 0;
            string callback = context.Request["callback"];
            string response = "success";
            string call = callback + "(" + response + ")";
            try
            {
                string regiondata = DTRequest.GetQueryString("regiondata");
                string cip = DTRequest.GetQueryString("cip");
                List<ccphl_region> list = JsonConvert.DeserializeObject<List<ccphl_region>>(regiondata);
                list = list.OrderBy(o => o.id).ToList();
                foreach (var r in list)
                {
                    if (db.IsExists<region>(" RegionCode='" + r.id + "'"))
                    {
                        var reg = db.GetModel<region>(" RegionCode='" + r.id + "'");
                        if (!string.IsNullOrWhiteSpace(r.name) && reg.RegionName != r.name)
                        {
                            reg.RegionName = r.name;
                            db.UpdateTree<region>(reg, "Node", "Layer", "ParentID");
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(r.name) && !string.IsNullOrWhiteSpace(r.id) && !string.IsNullOrWhiteSpace(r.pId))
                        {
                            var preg = db.GetModel<region>(" RegionCode='" + r.pId + "'");
                            if (preg.ID != 0)
                            {
                                region reg = new region();
                                reg.GUID = Guid.NewGuid().ToString();
                                reg.AddTime = DateTime.Now;
                                reg.RegionCode = r.id;
                                reg.RegionName = r.name;
                                reg.ParentID = preg.ID;
                                reg.SortNo = 99;
                                reg.Status = 1;
                                reg.DepartmentCode = r.id;
                                reg.ClientIP = cip;
                                db.InsertTree<region>(reg, "Node", "Layer", "ParentID");
                            }
                        }
                    }
                }
                state = 1;
            }
            catch
            {
                state = 1;
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(call);
        }
        #endregion
        #region 为民服务站点 登陆数据获取============================
        private void region_pic_get(HttpContext context)
        {
            string callback = context.Request["callback"];
            string response = "";
            string url ="http://"+context.Request.Url.Authority;
            string regionid = DTRequest.GetQueryString("regionid");
            string suffix = DTRequest.GetQueryString("suffix");//后缀
            if (string.IsNullOrWhiteSpace(regionid))
            {
                response = "{\"status\": 0, \"msg\": \"行政区划代码为空！\"}";
            }
            else
            {
                var reg = db.GetModel<region>(" RegionCode='" + regionid + "'");
                if (reg.ID > 0)
                {
                    if (string.IsNullOrWhiteSpace(reg.ImageUrl))
                    {
                        string pic = ParentImage(reg.ParentID??0);
                        if (!string.IsNullOrWhiteSpace(pic))
                        {
                            if (!string.IsNullOrWhiteSpace(suffix))
                            {
                                pic = GetPicBySuffix(pic, suffix.Trim());
                            }
                            response = "{\"status\": 1, \"msg\": \"获取成功！\",\"img\": \"" + url + pic + "\"}";
                        }
                        else
                        {
                            response = "{\"status\": 0, \"msg\": \"行政区划封面图片不存在！\"}";
                        }
                    }
                    else
                    {
                        string pic = reg.ImageUrl;
                        if (!string.IsNullOrWhiteSpace(suffix))
                        {
                            pic = GetPicBySuffix(reg.ImageUrl, suffix.Trim());
                        }
                        response = "{\"status\": 1, \"msg\": \"获取成功！\",\"img\": \"" + url + pic + "\"}";
                    }
                }
                else
                {
                    response = "{\"status\": 0, \"msg\": \"行政区划不存在，请更新数据再试！\"}";
                }
            }
            string call = callback + "(" + response + ")";
            context.Response.ContentType = "text/plain";
            context.Response.Write(call);
        }
        #endregion
        #region 为民服务站点 登陆数据获取============================
        private void region_get(HttpContext context)
        {
            string callback = context.Request["callback"];
            string response = "";
            string url = "http://" + context.Request.Url.Authority;
            string regionid = DTRequest.GetQueryString("regionid");
            if (string.IsNullOrWhiteSpace(regionid))
            {
                response = "{\"status\": 0, \"msg\": \"行政区划代码为空！\"}";
            }
            else
            {
                var reg = db.GetModel<region>(" RegionCode='" + regionid + "'");
                if (reg.ID > 0)
                {
                    var rows = JsonConvert.SerializeObject(new { status = 1, msg="获取成功",reg});
                    response = rows; 
                }
                else
                {
                    response = "{\"status\": 0, \"msg\": \"行政区划不存在，请更新数据再试！\"}";
                }
            }
            string call = callback + "(" + response + ")";
            context.Response.ContentType = "text/plain";
            context.Response.Write(call);
        }
        #endregion
        #region 为民服务用户浏览器信息搜集============================
        private void browserinfo(HttpContext context)
        {
            string regionid = DTRequest.GetQueryString("regionid");
            string uid = DTRequest.GetQueryString("uid");
            string ucode = DTRequest.GetQueryString("ucode");
            string uname = DTRequest.GetQueryString("uname");
            string mobi = DTRequest.GetQueryString("mobi");
            string pwd = DTRequest.GetQueryString("pwd");

            try
            {
                //保存用户信息
                var u = db.GetModel<t_user>(" UserID='" + uid + "'");
                u.UserID = uid;
                u.UserCode = ucode;
                u.UserName = uname;
                u.RegionCode = regionid;
                u.Mobile = mobi;
                u.Password = pwd;
                if (u.ID > 0)
                {
                    u.UpdateTime = DateTime.Now;
                    u.Update();
                }
                else
                {
                    u.GUID = Guid.NewGuid().ToString();
                    u.Status = 1;
                    u.AddTime = DateTime.Now;
                    u.Insert();
                }

                //保存浏览器信息
                string userAgent = context.Request.UserAgent;
                if (!string.IsNullOrWhiteSpace(userAgent))
                {
                    string BrowserName = "";
                    string Ver = "";
                    string ng = "";
                    string platform = "";
                    string fullBrowserName = GetBrowserName(userAgent, out BrowserName, out Ver, out ng, out platform);
                    if (!string.IsNullOrWhiteSpace(BrowserName))
                    {
                        t_browser b = new t_browser();
                        b.UserID = uid;
                        b.UserCode = ucode;
                        b.RegionCode = regionid;
                        b.Browsers = BrowserName;
                        b.Engines = ng;
                        b.Ver = Ver;
                        b.PlatForms = platform;
                        b.AddTime = DateTime.Now;
                        b.Insert();
                    }
                }
                context.Response.Write("y");
            }
            catch { context.Response.Write("n"); }
        }
        #endregion
        #region 为民服务用户浏览器信息搜集============================
        private void browserinfo_all(HttpContext context)
        {
            string regionid = DTRequest.GetQueryString("regionid");

            try
            {
                //保存浏览器信息
                string userAgent = context.Request.UserAgent;
                if (!string.IsNullOrWhiteSpace(userAgent))
                {
                    string BrowserName = "";
                    string Ver = "";
                    string ng = "";
                    string platform = "";
                    string fullBrowserName = GetBrowserName(userAgent, out BrowserName, out Ver, out ng, out platform);
                    if (!string.IsNullOrWhiteSpace(BrowserName))
                    {
                        t_browsers_all b = new t_browsers_all();
                        b.RegionCode = regionid;
                        b.Browsers = BrowserName;
                        b.Engines = ng;
                        b.Ver = Ver;
                        b.PlatForms = platform;
                        b.AddTime = DateTime.Now;
                        b.Insert();
                    }
                }
                context.Response.Write("y");
            }
            catch { context.Response.Write("n"); }
        }
        #endregion
        #region 为民服务站 用户浏览器信息读取============================
        private void get_browserinfo(HttpContext context)
        {
            string type = DTRequest.GetQueryString("type");
            switch (type)
            {
                case "all_browserinfo":
                    all_browserinfo(context);
                    break;
                case "all_platforms":
                    all_platforms(context);
                    break;
                case "ie_ver":
                    ie_ver(context);
                    break;
                case "platforms_browser":
                    platforms_browser(context);
                    break;
                default:
                    context.Response.Write("{\"status\": 0, \"msg\": \"数据类型不存在！\"}");
                    break;
            }
        }
        private void all_browserinfo(HttpContext context)
        {
            long all = db.GetCount<t_browser>("");
            if (all > 0)
            {
                long ie = db.GetCount<t_browser>(" Browsers='ie'");
                long chrome = db.GetCount<t_browser>(" Browsers='chrome'");
                long firefox = db.GetCount<t_browser>(" Browsers='firefox'");
                long safari = db.GetCount<t_browser>(" Browsers='safari'");
                long other = db.GetCount<t_browser>(" (Browsers<>'safari' AND Browsers<>'ie' AND Browsers<>'chrome' AND Browsers<>'firefox')");
                float ie0 = ((float)ie/all)*100;
                float chrome0 = ((float)chrome / all) * 100;
                float firefox0 = ((float)firefox / all) * 100;
                float safari0 = ((float)safari / all) * 100;
                float other0 = ((float)other / all) * 100;
                List<browser> browse = new List<browser>();
                browse.Add(new browser("IE", ie0));
                browse.Add(new browser("Chrome", chrome0));
                browse.Add(new browser("FireFox", firefox0));
                browse.Add(new browser("Safari", safari0));
                browse.Add(new browser("Other", other0));
                string json = LitJson.JsonMapper.ToJson(new { status = 1, msg = "查询成功！", list = browse });
                context.Response.Write(json);
            }
            else
            {
                context.Response.Write("{\"status\": 0, \"msg\": \"数据不存在！\"}");
            }
        }
        private void all_platforms(HttpContext context)
        {
            long all = db.GetCount<t_browser>("");
            if (all > 0)
            {
                long xp = db.GetCount<t_browser>(" PlatForms='windows xp'");
                long win7 = db.GetCount<t_browser>(" PlatForms='windows 7'");
                long win8 = db.GetCount<t_browser>(" PlatForms='windows 8'");
                long win81 = db.GetCount<t_browser>(" PlatForms='windows 8.1'");
                long other = db.GetCount<t_browser>(" (PlatForms<>'windows xp' AND PlatForms<>'windows 7' AND PlatForms<>'windows 8' AND PlatForms<>'windows 8.1')");
                double xp0 = ((float)xp / all) * 100;
                double win70 = ((float)win7 / all) * 100;
                double win80 = ((float)win8 / all) * 100;
                double win810 = ((float)win81 / all) * 100;
                double other0 = ((float)other / all) * 100;
                List<browser> browse = new List<browser>();
                browse.Add(new browser("windows xp", xp0));
                browse.Add(new browser("windows 7", win70));
                browse.Add(new browser("windows 8", win80));
                browse.Add(new browser("windows 8.1", other0));
                browse.Add(new browser("其他", other0));
                string json = LitJson.JsonMapper.ToJson(new { status = 1, msg = "查询成功！", list = browse });
                context.Response.Write(json);
            }
            else
            {
                context.Response.Write("{\"status\": 0, \"msg\": \"数据不存在！\"}");
            }
        }
        private void ie_ver(HttpContext context)
        {
            long all = db.GetCount<t_browser>(" Browsers='ie'");
            if (all > 0)
            {
                long ie6 = db.GetCount<t_browser>(" Browsers='ie' AND Ver='6.0'");
                long ie7 = db.GetCount<t_browser>(" Browsers='ie' AND Ver='7.0'");
                long ie8 = db.GetCount<t_browser>(" Browsers='ie' AND Ver='8.0'");
                long ie9 = db.GetCount<t_browser>(" Browsers='ie' AND Ver='9.0'");
                long ie10 = db.GetCount<t_browser>(" Browsers='ie' AND Ver='10.0'");
                long ie11 = db.GetCount<t_browser>(" Browsers='ie' AND Ver='11.0'");
                double ie60 = ((float)ie6 / all) * 100;
                double ie70 = ((float)ie7 / all) * 100;
                double ie80 = ((float)ie8 / all) * 100;
                double ie90 = ((float)ie9 / all) * 100;
                double ie100 = ((float)ie10 / all) * 100;
                double ie110 = ((float)ie11 / all) * 100;
                List<browser> browse = new List<browser>();
                browse.Add(new browser("IE6.0", ie60));
                browse.Add(new browser("IE7.0", ie70));
                browse.Add(new browser("IE8.0", ie80));
                browse.Add(new browser("IE9.0", ie90));
                browse.Add(new browser("IE10.0", ie100));
                browse.Add(new browser("IE11.0", ie110));
                string json = LitJson.JsonMapper.ToJson(new { status = 1, msg = "查询成功！", list = browse });
                context.Response.Write(json);
            }
            else
            {
                context.Response.Write("{\"status\": 0, \"msg\": \"数据不存在！\"}");
            }
        }
        private void platforms_browser(HttpContext context)
        {
            string[] platforms = { "windows xp", "windows 7", "windows 8","other"};
            string[] browsers = { "ie", "chrome", "firefox", "safari","other" };
            string[] iever = { "6.0", "7.0", "8.0", "9.0", "10.0", "11.0" };
            string name = "";
            string data = "";
            string stack = "";
            var all = db.GetCount<t_browser>(" ");
            if (all > 0)
            {
                List<platform> plat = new List<platform>();
                for (int i = 0; i < browsers.Length; i++)
                {
                    stack = browsers[i];
                    if (browsers[i] == "ie")
                    {
                        for (int j = 1; j < iever.Length; j++)
                        {
                            Dictionary<string, long> d = new Dictionary<string, long>();
                            name = stack + iever[j];
                            for (int k = 0; k < platforms.Length; k++)
                            {
                                if (platforms[k] == "other")
                                {
                                    d.Add("num"+i+j+k,0);
                                }
                                else
                                {
                                    d.Add("num" + i + j + k, db.GetCount<t_browser>(" PlatForms='" + platforms[k] + "' AND Browsers='" + browsers[i] + "' AND Ver='" + iever[j] + "'"));
                                }
                            }
                            //data = LitJson.JsonMapper.ToJson(d);
                            plat.Add(new platform(name, d, stack));
                        }
                    }
                    else if (browsers[i] == "other")
                    {
                        name = stack;
                        Dictionary<string, long> d = new Dictionary<string, long>();
                        for (int k = 0; k < platforms.Length; k++)
                        {
                            if (platforms[k] == "other")
                            {
                                d.Add("num"+i+k, db.GetCount<t_browser>(" (PlatForms<>'windows xp' AND PlatForms<>'windows 7' AND PlatForms<>'windows 8') AND (Browsers<>'safari' AND Browsers<>'ie' AND Browsers<>'chrome' AND Browsers<>'firefox')"));
                                //data += db.GetCount<t_browser>(" (PlatForms<>'windows xp' AND PlatForms<>'windows 7' AND PlatForms<>'windows 8') AND (Browsers<>'safari' AND Browsers<>'ie' AND Browsers<>'chrome' AND Browsers<>'firefox')") + ",";
                            }
                            else
                            {
                                d.Add("num"+i+k, db.GetCount<t_browser>(" PlatForms='" + platforms[k] + "' AND (Browsers<>'safari' AND Browsers<>'ie' AND Browsers<>'chrome' AND Browsers<>'firefox')"));
                                //data += db.GetCount<t_browser>(" PlatForms='" + platforms[k] + "' AND (Browsers<>'safari' AND Browsers<>'ie' AND Browsers<>'chrome' AND Browsers<>'firefox')") + ",";
                            }
                        }
                        //data = LitJson.JsonMapper.ToJson(d);
                        plat.Add(new platform(name, d, stack));
                    }
                    else
                    {
                        name = stack;
                        Dictionary<string, long> d = new Dictionary<string, long>();
                        for (int k = 0; k < platforms.Length; k++)
                        {
                            if (platforms[k] == "other")
                            {
                                d.Add("num" + i + k, db.GetCount<t_browser>(" (PlatForms<>'windows xp' AND PlatForms<>'windows 7' AND PlatForms<>'windows 8') AND Browsers='" + browsers[i] + "'"));
                                //data += db.GetCount<t_browser>(" (PlatForms<>'windows xp' AND PlatForms<>'windows 7' AND PlatForms<>'windows 8') AND Browsers='" + browsers[i] + "'") + ",";
                            }
                            else
                            {
                                d.Add("num" + i + k, db.GetCount<t_browser>(" PlatForms='" + platforms[k] + "' AND Browsers='" + browsers[i] + "'"));
                                //data += db.GetCount<t_browser>(" PlatForms='" + platforms[k] + "' AND Browsers='" + browsers[i] + "'") + ",";
                            }
                        }
                        //data = LitJson.JsonMapper.ToJson(d);
                        plat.Add(new platform(name, d, stack));
                    }
                }
                string json = LitJson.JsonMapper.ToJson(new { status = 1, msg = "查询成功！", list = plat });
                context.Response.Write(json);
            }
            else
            {
                context.Response.Write("{\"status\": 0, \"msg\": \"数据不存在！\"}");
            }
        }
        #endregion

        #region 为民服务浏览器信息搜集============================
        private void get_browserinfo_all(HttpContext context)
        {
            string type = DTRequest.GetQueryString("type");
            switch (type)
            {
                case "all_browserinfo":
                    all_browserinfo2(context);
                    break;
               
                default:
                    context.Response.Write("{\"status\": 0, \"msg\": \"数据类型不存在！\"}");
                    break;
            }
        }
        private void all_browserinfo2(HttpContext context)
        {
            string callback = context.Request["callback"];
            long all = db.GetCount<t_browsers_all>("");
            if (all > 0)
            {
                string name = "";
                string[] browsers = { "ie", "chrome", "firefox", "safari", "other" };
                string[] iever = { "6.0", "7.0", "8.0", "9.0", "10.0", "11.0" };

                //区划分组
                List<string> xlist0 = db2.Fetch<string>("SELECT RegionCode FROM ccphl_t_browsers_all WHERE LENGTH(RegionCode)=6 GROUP BY RegionCode ORDER BY RegionCode ASC");
                List<regioninfo2> xlist1 = new List<regioninfo2>();
                List<string> xlist = new List<string>();
                foreach (var x in xlist0)
                {
                    var region = db.GetModel<region>(" RegionCode='"+x+"'");
                    var regcount = db.GetCount<t_browsers_all>(" RegionCode LIKE '" + x + "%'");
                    if (region.ID > 0)
                    {
                        xlist1.Add(new regioninfo2(region.RegionName, x, regcount));
                    }
                }
                xlist1 = xlist1.OrderByDescending(p=>p.count).ToList();
                foreach (var x in xlist1)
                {
                    xlist.Add(x.name);
                }
                List<allplatform> plat = new List<allplatform>();
              
                for (int i = 0; i < browsers.Length; i++)
                {
                    name = browsers[i];
                    if (browsers[i] == "ie")
                    {
                        for (int j = 1; j < iever.Length; j++)
                        {
                            Dictionary<string, long> d = new Dictionary<string, long>();
                            name = browsers[i] + iever[j];
                            for (int k = 0; k < xlist1.Count; k++)
                            {
                                d.Add("num" + i + j + k, db.GetCount<t_browsers_all>(" RegionCode LIKE '" + xlist1[k].code + "%' AND Browsers='" + browsers[i] + "' AND Ver='" + iever[j] + "'"));
                            }
                            plat.Add(new allplatform(name, d));
                        }
                    }
                    else if (browsers[i] == "other")
                    {
                        Dictionary<string, long> d = new Dictionary<string, long>();
                        for (int k = 0; k < xlist1.Count; k++)
                        {
                            d.Add("num" + i + k, db.GetCount<t_browsers_all>(" RegionCode LIKE '" + xlist1[k].code + "%' AND (Browsers<>'safari' AND Browsers<>'ie' AND Browsers<>'chrome' AND Browsers<>'firefox')"));
                        }
                        plat.Add(new allplatform(name, d));
                    }
                    else
                    {
                        Dictionary<string, long> d = new Dictionary<string, long>();
                        for (int k = 0; k < xlist1.Count; k++)
                        {
                            d.Add("num" + i + k, db.GetCount<t_browsers_all>(" RegionCode LIKE '" + xlist1[k].code + "%' AND Browsers='" + browsers[i] + "'"));
                        }
                        plat.Add(new allplatform(name, d));
                    }
                }

                string json = LitJson.JsonMapper.ToJson(new { status = 1, msg = "查询成功！", list = plat, xlist = xlist });
                if (!string.IsNullOrWhiteSpace(callback))
                {
                    context.Response.Write(callback + "(" + json + ")");
                }
                else
                {
                    context.Response.Write(json);
                }
            }
            else
            {
                context.Response.Write("{\"status\": 0, \"msg\": \"数据不存在！\"}");
            }
        }
        private class regioninfo2
        {
            public regioninfo2(string name, string code, long count)
            {
                this.name = name;
                this.code = code;
                this.count = count;
            }
            public string name { get; set; }
            public string code { get; set; }
            public long count { get; set; }
        }
        /// <summary>
        /// 回传数据
        /// </summary>
        private class allplatform
        {
            public allplatform(string name, Dictionary<string, long> data)
            {
                this.name = name;
                this.data = data;
            }
            public string name { get; set; }
            public Dictionary<string, long> data { get; set; }
        }
        #endregion

        private class ccphl_region
        {
            public string id{get;set;}
            public string pId{get;set;}
            public string name { get; set; }
        }
        private class browser
        {
            public browser(string name, double share)
            {
                this.name = name;
                this.share = share;
            }
            public string name { get; set; }
            public double share { get; set; }
        }
        private class platform
        {
            public platform(string name, Dictionary<string, long> data, string stack)
            {
                this.name = name;
                this.data = data;
                this.stack = stack;
            }
            public string name { get; set; }
            public Dictionary<string, long> data { get; set; }
            public string stack { get; set; }
        }
        private string GetPicBySuffix(string img,string suffix)
        {
            int index=img.LastIndexOf("/");
            string ext = img.Substring(index, img.Length - index).Split('.')[1];
            string filepath = img.Substring(0, img.LastIndexOf("."));
            string result = filepath + "_" + suffix +"."+ext;
            return result;
        }
        private string ParentImage(int parentid)
        {
            if (parentid > 0)
            {
                var reg = db.GetModel<region>(parentid);
                if (reg != null)
                {
                    if (!string.IsNullOrWhiteSpace(reg.ImageUrl))
                    {
                        return reg.ImageUrl;

                    }
                    else
                    {
                        return ParentImage(reg.ParentID??0);
                    }
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 获取浏览器名称
        /// </summary>
        /// <param name="userAgent"></param>
        /// <param name="browserName"></param>
        /// <param name="ver"></param>
        /// <returns></returns>
        private string GetBrowserName(string userAgent, out string browserName, out string ver,out string engines,out string platform)
        {
            string fullBrowserName = string.Empty;
            browserName = string.Empty;
            ver = string.Empty;
            engines = string.Empty;
            platform = string.Empty;

            //windows 操作系统
            string regexStr = @"windows nt (?<ver>[\d.]+)";
            Regex r = new Regex(regexStr, RegexOptions.IgnoreCase);
            Match m = r.Match(userAgent);
            if (m.Success)
            {
                ver = m.Groups["ver"].Value;
                if (ver == "5.0")
                {
                    platform = "windows 2000";
                }
                else if (ver == "5.1")
                {
                    platform = "windows xp";
                }
                else if (ver == "5.2")
                {
                    platform = "windows xp";
                }
                else if (ver == "6.0")
                {
                    platform = "windows vista";
                }
                else if (ver == "6.1")
                {
                    platform = "windows 7";
                }
                else if (ver == "6.2")
                {
                    platform = "windows 8";
                }
                else if (ver == "6.3")
                {
                    platform = "windows 8.1";
                }
                else if (ver == "6.4")
                {
                    platform = "windows 10";
                }
            }
            else
            {
                regexStr = @"cpu os ";
                r = new Regex(regexStr, RegexOptions.IgnoreCase);
                m = r.Match(userAgent);
                if (m.Success)
                {
                    platform = "ipad";
                }
                else
                {
                    regexStr = @"iphone os ";
                    r = new Regex(regexStr, RegexOptions.IgnoreCase);
                    m = r.Match(userAgent);
                    if (m.Success)
                    {
                        platform = "iphone";
                    }
                    else
                    {
                        regexStr = @"os x ";
                        r = new Regex(regexStr, RegexOptions.IgnoreCase);
                        m = r.Match(userAgent);
                        if (m.Success)
                        {
                            platform = "mac";
                        }
                        else
                        {
                            regexStr = @"android ";
                            r = new Regex(regexStr, RegexOptions.IgnoreCase);
                            m = r.Match(userAgent);
                            if (m.Success)
                            {
                                platform = "android";
                            }
                            else
                            {
                                regexStr = @"linux ";
                                r = new Regex(regexStr, RegexOptions.IgnoreCase);
                                m = r.Match(userAgent);
                                if (m.Success)
                                {
                                    platform = "linux";
                                }
                            }
                        }
                    }
                }
            }
            // IE 6-10
            regexStr = @"msie (?<ver>[\d.]+)";
            r = new Regex(regexStr, RegexOptions.IgnoreCase);
            m = r.Match(userAgent);
            if (m.Success)
            {
                browserName = "ie";
                ver = m.Groups["ver"].Value;
                fullBrowserName = string.Format("{0} {1}", browserName, ver);
                engines = "trident";
                return fullBrowserName;
            }
            // IE11
            regexStr = @"trident\/(?<ver>[\d.]+)";
            r = new Regex(regexStr, RegexOptions.IgnoreCase);
            m = r.Match(userAgent);
            if (m.Success)
            {
                browserName = "ie";
                ver ="11.0";
                fullBrowserName = string.Format("{0} {1}", browserName, ver);
                engines = "gecko";
                return fullBrowserName;
            }
            // Chrome
            regexStr = @"chrome\/(?<ver>[\d.]+)";
            r = new Regex(regexStr, RegexOptions.IgnoreCase);
            m = r.Match(userAgent);
            if (m.Success)
            {
                browserName = "chrome";
                ver = m.Groups["ver"].Value;
                fullBrowserName = string.Format("{0} {1}", browserName, ver);
                engines = "webkit";
                return fullBrowserName;
            }
            // Firefox
            regexStr = @"firefox\/(?<ver>[\d.]+)";
            r = new Regex(regexStr, RegexOptions.IgnoreCase);
            m = r.Match(userAgent);
            if (m.Success)
            {
                browserName = "firefox";
                ver = m.Groups["ver"].Value;
                engines = "gecko";
                fullBrowserName = string.Format("{0} {1}", browserName, ver);
                return fullBrowserName;
            }
            // Opera
            regexStr = @"opera\/(?<ver>[\d.]+)";
            r = new Regex(regexStr, RegexOptions.IgnoreCase);
            m = r.Match(userAgent);
            if (m.Success)
            {
                browserName = "opera";
                ver = m.Groups["ver"].Value;
                engines = "presto";
                fullBrowserName = string.Format("{0} {1}", browserName, ver);
                return fullBrowserName;
            }
            // Safari
            regexStr = @"version\/(?<ver>[\d.]+).*safari";
            r = new Regex(regexStr, RegexOptions.IgnoreCase);
            m = r.Match(userAgent);
            if (m.Success)
            {
                browserName = "safari";
                ver = m.Groups["ver"].Value;
                engines = "webkit";
                fullBrowserName = string.Format("{0} {1}", browserName, ver);
                return fullBrowserName;
            }
            // iemobile
            regexStr = @"iemobile\/(?<ver>[\d.]+)";
            r = new Regex(regexStr, RegexOptions.IgnoreCase);
            m = r.Match(userAgent);
            if (m.Success)
            {
                browserName = "iemobile";
                ver = m.Groups["ver"].Value;
                engines = "trident";
                fullBrowserName = string.Format("{0} {1}", browserName, ver);
                return fullBrowserName;
            }
            return fullBrowserName;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}