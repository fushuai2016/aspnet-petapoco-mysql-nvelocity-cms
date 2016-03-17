using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using ccphl.DBUtility;
using Model;
using System.Web.Routing;
using ccphl.Common;
using Newtonsoft.Json;

namespace ccphl.HttpHandlers
{
    /// <summary>
    /// 请求处理接口
    /// </summary>
    public abstract class IRequest
    {
        /// <summary>
        /// 站点域名
        /// </summary>
        public string UrlAuthority;
        /// <summary>
        /// 主站点域名
        /// </summary>
        public static string HomeDomain = "ylxf.yn.gov.cn";
        /// <summary>
        /// 集群站点域名
        /// </summary>
        public static string TwoDomain = "zswldj.yn.gov.cn";

        public static string LocaHostDomain = "localhost:9000";
        /// <summary>
        /// 域名字典集
        /// </summary>
        public static Dictionary<string, int> Domains = new Dictionary<string, int>();
        /// <summary>
        /// 默认列表页面新闻显示数量
        /// </summary>
        public int DefaultPageSize = 20;
        /// <summary>
        /// 是否跳转 全局设定
        /// </summary>
        public bool IsRedirect = true;
        /// <summary>
        /// 分页符
        /// </summary>
        public const string PageBreak = "$[page]$";//分页符
        /// <summary>
        /// 分页显示条目数
        /// </summary>
        public const int centSize = 8;
        /// <summary>
        /// 封装过的数据接口 通用 不防注入 
        /// </summary>
        protected internal MySqlUtility db;
        /// <summary>
        /// 原生接口 
        /// </summary>
        protected internal MySqlHelper db2;
        public IRequest()
        {
            if(db==null)
                db = new MySqlUtility();
            if (db2 == null)
                db2 = MySqlHelper.GetInstance();
            if (Domains.Count == 0)
            {
                if (!Domains.Keys.Contains(HomeDomain))
                {
                    Domains.Add(HomeDomain, 1);
                }
                if (!Domains.Keys.Contains(TwoDomain))
                {
                    Domains.Add(TwoDomain, 1);
                }
                if (!Domains.Keys.Contains(LocaHostDomain))
                {
                    Domains.Add(LocaHostDomain, 1);
                }
            }
            if (string.IsNullOrWhiteSpace(UrlAuthority))
            {
                UrlAuthority = HttpContext.Current.Request.Url.Authority.ToLower();
            }
        }
        /// <summary>
        /// 入口方法
        /// </summary>
        /// <param name="context"></param>
        public abstract void ProcessRequest(HttpContext context, RequestContext RequestContext);
        /// <summary>
        /// 利用反射机制创建IRequest接口
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IRequest CreateIRequest(string action)
        {
            return (IRequest)Assembly.Load("ccphl.HttpHandlers").CreateInstance("ccphl.HttpHandlers." + action + "Request");
        }
        /// <summary>
        /// 站点模板
        /// </summary>
        public nTemplate template = new nTemplate();
        
        /// <summary>
        /// 模板公共类
        /// </summary>
        public class nTemplate
        {
            public string TemplatePath { get; set; }
            public string IndexTemplate { get; set; }
            public string ListTemplate { get; set; }
            public string ShowTemplate { get; set; }
            public string PicTemplate { get; set; }
            public string SearchTemplate { get; set; }
        }
        /// <summary>
        /// SEO引擎优化
        /// </summary>
        public class SEO
        {
            public string Title { get; set; }
            public string Keywords { get; set; }
            public string Description { get; set; }
        }
        /// <summary>
        /// seo
        /// </summary>
        public SEO seo = new SEO();
       
        /// <summary>
        /// 错误返回
        /// </summary>
        public void ResponseError(string msg)
        {
            //HttpContext.Current.Response.StatusCode = 404;
            HttpContext.Current.Response.Write(msg);
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// 链接跳转
        /// </summary>
        /// <param name="type">跳转方式</param>
        /// <param name="url">链接地址</param>
        public void ResponseRedirect(string url, string type = "_blank")
        {
            HttpContext.Current.Response.Redirect(url);
            //break;
            //switch (type)
            //{
            //    case "_blank":
            //        HttpContext.Current.Response.Write("<script>windows.location.href='" + url + "';</script>");
            //        break;
            //    case "_self":
            //        HttpContext.Current.Response.Redirect(url);
            //        break;
            //    default:
            //        HttpContext.Current.Response.Write("<script>window.open('" + url + "','_blank')</script>");
            //        break;

            //}
            HttpContext.Current.Response.End();
        }
        
        #region 依据标示符分页
        /// <summary>
        /// 输出分页内容，及分页导航条
        /// </summary>
        /// <param name="p_strContent"></param>
        /// <param name="pageIndex"></param>
        /// <param name="Url"></param>
        /// <param name="strContentLink"></param>
        /// <returns></returns>
        public string OutputByChar(string p_strContent, int pageIndex, string Url, out string strContentLink)
        {
            strContentLink = "<div class=\"contentpages\"></div>";
            string m_strRet = "";
            //设置显示页数
            int totalCount = getPage(p_strContent, PageBreak);

            string[] strContent = StringSplit(p_strContent, PageBreak);

            m_strRet = strContent[pageIndex - 1].ToString();

            if (totalCount<=1)
            {
                return p_strContent;
            }
            else
            {
                if (pageIndex > totalCount)
                {
                    pageIndex = totalCount;
                }
            }
            
            //计算页数
            long pageCount = totalCount;

            StringBuilder pageStr = new StringBuilder();
            string firstBtn = "<a href=\"" + Url + "_" + (pageIndex - 1) + ".htm\">«上一页</a>";
            string lastBtn = "<a href=\"" + Url + "_" + (pageIndex + 1) + ".htm\">下一页»</a>";
            string firstStr = "<a href=\"" + Url + ".htm\">1</a>";
            string lastStr = "<a href=\"" + Url + "_" + pageCount + ".htm\">" + pageCount + "</a>";

            if (pageIndex <= 1)
            {
                firstBtn = "<span>«上一页</span>";
            }
            if (pageIndex >= pageCount)
            {
                lastBtn = "<span>下一页»</span>";
            }
            if (pageIndex == 1)
            {
                firstStr = "<a class=\"cur\">1</a>";
            }
            if (pageIndex == pageCount)
            {
                lastStr = "<a class=\"cur\">" + pageCount.ToString() + "</a>";
            }
            int firstNum = pageIndex - (centSize / 2); //中间开始的页码
            if (pageIndex < centSize)
                firstNum = 2;
            long lastNum = pageIndex + centSize - ((centSize / 2) + 1); //中间结束的页码
            if (lastNum >= pageCount)
                lastNum = pageCount - 1;
            pageStr.Append(firstBtn + firstStr);
            if (pageIndex >= centSize)
            {
                pageStr.Append("<span>...</span>\n");
            }
            for (int i = firstNum; i <= lastNum; i++)
            {
                if (i == pageIndex)
                {
                    pageStr.Append("<a class=\"cur\">" + i + "</a>");
                }
                else
                {
                    pageStr.Append("<a href=\"" + Url + "_" + i + ".htm\">" + i + "</a>");
                }
            }
            if (pageCount - pageIndex > centSize - ((centSize / 2)))
            {
                pageStr.Append("<span>...</span>");
            }
            pageStr.Append(lastStr + lastBtn);
            if (totalCount > 0)
                strContentLink = " <div class=\"contentpages\">" + pageStr.ToString() +"</div>";
            return m_strRet;
        }
        /// <summary>
        /// 获取分页数量
        /// </summary>
        /// <param name="strContent"></param>
        /// <param name="PageBreak"></param>
        /// <returns></returns>
        public int getPage(string strContent, string PageBreak)
        {
            if (PageBreak.Trim() == "") return 0;
            string[] str = StringSplit(strContent, PageBreak);
            int iPage = str.Length;

            return iPage;
        }

        /// <summary>
        /// 将字符串分割成数组
        /// </summary>
        /// <param name="strSource"></param>
        /// <param name="strSplit"></param>
        /// <returns></returns>
        public string[] StringSplit(string strSource, string PageBreak)
        {
            string[] strtmp = new string[1];
            if (PageBreak.Trim() == "")
            {
                strtmp[0] = strSource;
                return strtmp;
            }
            int index = strSource.IndexOf(PageBreak, 0);
            if (index < 0)
            {
                strtmp[0] = strSource;

                return strtmp;
            }
            else
            {
                strtmp[0] = strSource.Substring(0, index);
                return StringSplit(strSource.Substring(index + PageBreak.Length), strtmp, PageBreak);
            }
        }
        /// <summary>
        /// 采用递归将字符串分割成数组
        /// </summary>
        /// <param name="strSource"></param>
        /// <param name="strSplit"></param>
        /// <param name="attachArray"></param>
        /// <returns></returns>
        public string[] StringSplit(string strSource, string[] attachArray, string PageBreak)
        {
            string[] strtmp = new string[attachArray.Length + 1];
            attachArray.CopyTo(strtmp, 0);
            int index = strSource.IndexOf(PageBreak, 0);
            if (index < 0)
            {
                strtmp[attachArray.Length] = strSource;

                return strtmp;
            }
            else
            {
                strtmp[attachArray.Length] = strSource.Substring(0, index);
                return StringSplit(strSource.Substring(index + PageBreak.Length), strtmp, PageBreak);
            }
        }
        #endregion 
        /// <summary>
        /// 统一请求返回实体
        /// </summary>
        public class JsonResult
        {
            private int _status = 0;
            private string _msg = "";
            private object _data = null;
            private int _total = 0;
            private int _page = 0;
            private int _size = 0;
            public int status
            {
                set { this._status = value; }
                get { return this._status; }
            }
            public string msg
            {
                set { this._msg = value; }
                get { return this._msg; }
            }
            public object data
            {
                set { this._data = value; }
                get { return this._data; }
            }
            public int total
            {
                set { this._total = value; }
                get { return this._total; }
            }
            public int page
            {
                set { this._page = value; }
                get { return this._page; }
            }
            public int size
            {
                set { this._size = value; }
                get { return this._size; }
            }
        }
        /// <summary>
        /// json返回
        /// </summary>
        /// <param name="js"></param>
        public void Json(JsonResult js)
        {
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "text/plain";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(js));
        }

        /// <summary>
        /// 文本返回
        /// </summary>
        /// <param name="js"></param>
        public void Json(string result)
        {
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "text/plain";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Write(result);
        }
        /// <summary>
        /// 跨域返回
        /// </summary>
        /// <param name="js"></param>
        public void Jsonp(JsonResult js)
        {
            string callback = HttpContext.Current.Request.QueryString["callback"];
            string result = JsonConvert.SerializeObject(js);
            string r = "";
            if (!string.IsNullOrWhiteSpace(callback))
            {
                r = String.Format("{0}({1})", callback, result);
            }
            else
            {
                r = result;
            }
            HttpContext.Current.Response.ContentType = "text/plain";
            HttpContext.Current.Response.Write(r);
        }
    }
}
