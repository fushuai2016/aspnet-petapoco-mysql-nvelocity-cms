
namespace ccphl.HttpHandlers
{
    /// <summary>
    /// 路由设置
    /// </summary>
    /// <remarks>
    /// CLR版本： 4.0.30319.18444<br/>
    /// 组织名(版权)：Copyright (c) 2015,云南红岭云科技有限公司 - www.ccphl.com All rights reserved.<br/>
    /// 文件名：  RegisterRoutes.cs<br/>
    /// 命名空间：ccphl.CMS.HttpHandlers.Base<br/>
    /// 作者: yhly-fu<br/>
    /// 计算机名称：YHLY-FU-PC<br/>
    /// 创建时间: 2015/5/29 9:41:21<br/>
    /// </remarks>
    public class RegisterRoutes
    {
        /// <summary>
        /// 路由集合
        /// </summary>
        /// <param name="routes"></param>
        public static void myRoutes(System.Web.Routing.RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /********ajax请求************/
            routes.MapMyRoute("ajax_0",
                   "ajax/{type}",
                   new { action = "Ajax" }, new { type = "^[A-Za-z]+$" }, null);
            /********ajax请求************/

            /********站内搜索************/
            routes.MapMyRoute("search_0",
                   "search/{type}",
                   new { level = 1, action = "Search" }, new { type = "^(title|keywords|source)$" }, null);
            routes.MapMyRoute("search_2",
                   "{area}/search/{type}",
                   new { level = 2, action = "Search" }, new { area = "^[A-Za-z]+$", type = "^(title|keywords|source)$" }, null);
            routes.MapMyRoute("search_3",
                   "{area}/{county}/search/{type}",
                   new { level = 3, action = "Search" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", type = "^(title|keywords|source)$" }, null);
            routes.MapMyRoute("search_4",
                   "{area}/{county}/{country}/search/{type}",
                   new { level = 4, action = "Search" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", type = "^(title|keywords|source)$" }, null);
            routes.MapMyRoute("search_5",
                   "{area}/{county}/{country}/{village}/search/{type}",
                   new { level = 5, action = "Search" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", village = "^[A-Za-z]+$", type = "^(title|keywords|source)$" }, null);
            routes.MapMyRoute("search_00",
                   "search/{type}/{page}",
                   new { level = 1, action = "Search" }, new { type = "^(title|keywords|source)$", page = "^[0-9]+$" }, null);
            routes.MapMyRoute("search_20",
                   "{area}/search/{type}/{page}",
                   new { level = 2, action = "Search" }, new { area = "^[A-Za-z]+$", type = "^(title|keywords|source)$", page = "^[0-9]+$" }, null);
            routes.MapMyRoute("search_30",
                   "{area}/{county}/search/{type}/{page}",
                   new { level = 3, action = "Search" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", type = "^(title|keywords|source)$", page = "^[0-9]+$" }, null);
            routes.MapMyRoute("search_40",
                   "{area}/{county}/{country}/search/{type}/{page}",
                   new { level = 4, action = "Search" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", type = "^(title|keywords|source)$", page = "^[0-9]+$" }, null);
            routes.MapMyRoute("search_50",
                   "{area}/{county}/{country}/{village}/search/{type}/{page}",
                   new { level = 5, action = "Search" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", village = "^[A-Za-z]+$", type = "^(title|keywords|source)$", page = "^[0-9]+$" }, null);
            routes.MapMyRoute("search_000",
                    "search/{type}/{page}/{size}",
                    new { level = 1, action = "Search" }, new { type = "^(title|keywords|source)$", page = "^[0-9]+$", size = "^[0-9]+$" }, null);
            routes.MapMyRoute("search_200",
                   "{area}/search/{type}/{page}/{size}",
                   new { level = 2, action = "Search" }, new { area = "^[A-Za-z]+$", type = "^(title|keywords|source)$", page = "^[0-9]+$", size = "^[0-9]+$" }, null);
            routes.MapMyRoute("search_300",
                   "{area}/{county}/search/{type}/{page}/{size}",
                   new { level = 3, action = "Search" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", type = "^(title|keywords|source)$", page = "^[0-9]+$", size = "^[0-9]+$" }, null);
            routes.MapMyRoute("search_400",
                   "{area}/{county}/{country}/search/{type}/{page}/{size}",
                   new { level = 4, action = "Search" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", type = "^(title|keywords|source)$", page = "^[0-9]+$", size = "^[0-9]+$" }, null);
            routes.MapMyRoute("search_500",
                   "{area}/{county}/{country}/{village}/search/{type}/{page}/{size}",
                   new { level = 5, action = "Search" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", village = "^[A-Za-z]+$", type = "^(title|keywords|source)$", page = "^[0-9]+$", size = "^[0-9]+$" }, null);
            /********站内搜索************/
            //主页
            routes.MapMyRoute("index_0",
                   "index.htm",
                   new { level = 1 });
            routes.MapMyRoute("index_1",
                   "{area}/index.htm",
                   new { level = 2 }, new { area = "^[A-Za-z]+$" }, null);
            routes.MapMyRoute("index_2",
                   "{area}/{county}/index.htm",
                   new { level = 3 }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$" }, null);
            routes.MapMyRoute("index_3",
                   "{area}/{county}/{country}/index.htm",
                   new { level = 4 }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$" }, null);
            routes.MapMyRoute("index_4",
                  "{area}/{county}/{country}/{village}/index.htm",
                  new { level = 5 }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", village = "^[A-Za-z]+$" }, null);
            routes.MapMyRoute("index_6",
                   "{area}",
                   new { level = 2 }, new { area = "^[A-Za-z]+$" }, null);
            routes.MapMyRoute("index_7",
                   "{area}/{county}",
                   new { level = 3 }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$" }, null);
            routes.MapMyRoute("index_8",
                   "{area}/{county}/{country}",
                   new { level = 4 }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$" }, null);
            routes.MapMyRoute("index_9",
                  "{area}/{county}/{country}/{village}",
                  new { level = 5 }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", village = "^[A-Za-z]+$" }, null);

            #region 列表页面路由配置
            /************列表页面*****************/

            /********列表页面 1级站点(目前1一级站点一个)************/
            routes.MapMyRoute("list_0_0",
                   "newslist/{cid}",
                   new { level = 1, action = "List" }, new { cid = "^[A-Za-z-0-9]+$" }, null);
            routes.MapMyRoute("list_0_1",
                   "newslist/{cid}/{page}",
                   new { level = 1, action = "List" }, new { cid = "^[A-Za-z-0-9]+$", page = "^[0-9]+$" }, null);
            routes.MapMyRoute("list_0_2",
                   "newslist/{cid}/{page}/{size}",
                   new { level = 1, action = "List" }, new { cid = "^[A-Za-z-0-9]+$", page = "^[0-9]+$", size = "^[0-9]+$" }, null);
            routes.MapMyRoute("list_0_3",
                   "newslist/{cid}.htm",
                   new { level = 1, action = "List" }, new { cid = "^[A-Za-z-0-9]+$" }, null);
            routes.MapMyRoute("list_0_4",
                   "newslist/{cid}/{page}.htm",
                   new { level = 1, action = "List" }, new { cid = "^[A-Za-z-0-9]+$", page = "^[0-9]+$" }, null);
            routes.MapMyRoute("list_0_5",
                   "newslist/{cid}/{page}/{size}.htm",
                   new { level = 1, action = "List" }, new { cid = "^[A-Za-z-0-9]+$", page = "^[0-9]+$", size = "^[0-9]+$" }, null);
            /********列表页面 1级站点(目前1一级站点一个)************/

            /********列表页面 2级站点************/
            routes.MapMyRoute("list_1_0",
                   "{area}/newslist/{cid}",
                   new { level = 2, action = "List" }, new { area = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$" }, null);
            routes.MapMyRoute("list_1_1",
                   "{area}/newslist/{cid}/{page}",
                   new { level = 2, action = "List" }, new { area = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", page = "^[0-9]+$" }, null);
            routes.MapMyRoute("list_1_2",
                   "{area}/newslist/{cid}/{page}/{size}",
                   new { level = 2, action = "List" }, new { area = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", page = "^[0-9]+$", size = "^[0-9]+$" }, null);
            routes.MapMyRoute("list_1_3",
                   "{area}/newslist/{cid}.htm",
                   new { level = 2, action = "List" }, new { area = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$" }, null);
            routes.MapMyRoute("list_1_4",
                   "{area}/newslist/{cid}/{page}.htm",
                   new { level = 2, action = "List" }, new { area = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", page = "^[0-9]+$" }, null);
            routes.MapMyRoute("list_1_5",
                   "{area}/newslist/{cid}/{page}/{size}.htm",
                   new { level = 2, action = "List" }, new { area = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", page = "^[0-9]+$", size = "^[0-9]+$" }, null);
            /********列表页面 2级站点************/

            /********列表页面 3级站点************/
            routes.MapMyRoute("list_2_0",
                   "{area}/{county}/newslist/{cid}",
                   new { level = 3, action = "List" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$" }, null);
            routes.MapMyRoute("list_2_1",
                   "{area}/{county}/newslist/{cid}/{page}",
                   new { level = 3, action = "List" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", page = "^[0-9]+$" }, null);
            routes.MapMyRoute("list_2_2",
                   "{area}/{county}/newslist/{cid}/{page}/{size}",
                   new { level = 3, action = "List" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", page = "^[0-9]+$", size = "^[0-9]+$" }, null);
            routes.MapMyRoute("list_2_3",
                   "{area}/{county}/newslist/{cid}.htm",
                   new { level = 3, action = "List" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$" }, null);
            routes.MapMyRoute("list_2_4",
                   "{area}/{county}/newslist/{cid}/{page}.htm",
                   new { level = 3, action = "List" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", page = "^[0-9]+$" }, null);
            routes.MapMyRoute("list_2_5",
                   "{area}/{county}/newslist/{cid}/{page}/{size}.htm",
                   new { level = 3, action = "List" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", page = "^[0-9]+$", size = "^[0-9]+$" }, null);
            /********列表页面 3级站点************/

            /********列表页面 4级站点************/
            routes.MapMyRoute("list_3_0",
                   "{area}/{county}/{country}/newslist/{cid}",
                   new { level = 4, action = "List" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$" }, null);
            routes.MapMyRoute("list_3_1",
                   "{area}/{county}/{country}/newslist/{cid}/{page}",
                   new { level = 4, action = "List" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", page = "^[0-9]+$" }, null);
            routes.MapMyRoute("list_3_2",
                   "{area}/{county}/{country}/newslist/{cid}/{page}/{size}",
                   new { level = 4, action = "List" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", page = "^[0-9]+$", size = "^[0-9]+$" }, null);
            routes.MapMyRoute("list_3_3",
                   "{area}/{county}/{country}/newslist/{cid}.htm",
                   new { level = 4, action = "List" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$" }, null);
            routes.MapMyRoute("list_3_4",
                   "{area}/{county}/{country}/newslist/{cid}/{page}.htm",
                   new { level = 4, action = "List" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", page = "^[0-9]+$" }, null);
            routes.MapMyRoute("list_3_5",
                   "{area}/{county}/{country}/newslist/{cid}/{page}/{size}.htm",
                   new { level = 4, action = "List" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", page = "^[0-9]+$", size = "^[0-9]+$" }, null);
            /********列表页面 4级站点************/

            /********列表页面 5级站点************/
            routes.MapMyRoute("list_4_0",
                   "{area}/{county}/{country}/{village}/newslist/{cid}",
                   new { level = 5, action = "List" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", village = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$" }, null);
            routes.MapMyRoute("list_4_1",
                   "{area}/{county}/{country}/{village}/newslist/{cid}/{page}",
                   new { level = 5, action = "List" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", village = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", page = "^[0-9]+$" }, null);
            routes.MapMyRoute("list_4_2",
                   "{area}/{county}/{country}/{village}/newslist/{cid}/{page}/{size}",
                   new { level = 5, action = "List" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", village = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", page = "^[0-9]+$", size = "^[0-9]+$" }, null);
            routes.MapMyRoute("list_4_3",
                   "{area}/{county}/{country}/{village}/newslist/{cid}.htm",
                   new { level = 5, action = "List" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", village = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$" }, null);
            routes.MapMyRoute("list_4_4",
                   "{area}/{county}/{country}/{village}/newslist/{cid}/{page}.htm",
                   new { level = 5, action = "List" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", village = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", page = "^[0-9]+$" }, null);
            routes.MapMyRoute("list_4_5",
                   "{area}/{county}/{country}/{village}/newslist/{cid}/{page}/{size}.htm",
                   new { level = 5, action = "List" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", village = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", page = "^[0-9]+$", size = "^[0-9]+$" }, null);
            /********列表页面 5级站点************/

            /************列表页面*****************/
            #endregion

            #region 详细页面路由配置
            /************详细页面 不带长文章分页*****************/

            routes.MapMyRoute("show_0",
                   "news/{cid}/{id}",
                   new { level = 1, action = "Show" }, new { cid = "^[A-Za-z-0-9]+$", id = "^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$" }, null);
            routes.MapMyRoute("show_1",
                  "news/{cid}/{id}.htm",
                  new { level = 1, action = "Show" }, new { cid = "^[A-Za-z-0-9]+$", id = "^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$" }, null);

            routes.MapMyRoute("show_2",
                   "{area}/news/{cid}/{id}",
                   new { level = 2, action = "Show" }, new { area = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", id = "^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$" }, null);
            routes.MapMyRoute("show_3",
                  "{area}/news/{cid}/{id}.htm",
                  new { level = 2, action = "Show" }, new { area = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", id = "^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$" }, null);

            routes.MapMyRoute("show_4",
                   "{area}/{county}/news/{cid}/{id}",
                   new { level = 3, action = "Show" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", id = "^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$" }, null);
            routes.MapMyRoute("show_5",
                  "{area}/{county}/news/{cid}/{id}.htm",
                  new { level = 3, action = "Show" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", id = "^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$" }, null);

            routes.MapMyRoute("show_6",
                   "{area}/{county}/{country}/news/{cid}/{id}",
                   new { level = 4, action = "Show" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", id = "^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$" }, null);
            routes.MapMyRoute("show_7",
                  "{area}/{county}/{country}/news/{cid}/{id}.htm",
                  new { level = 4, action = "Show" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", id = "^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$" }, null);

            routes.MapMyRoute("show_8",
                  "{area}/{county}/{country}/{village}/news/{cid}/{id}",
                  new { level = 5, action = "Show" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", village = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", id = "^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$" }, null);
            routes.MapMyRoute("show_9",
                  "{area}/{county}/{country}/{village}/news/{cid}/{id}.htm",
                  new { level = 5, action = "Show" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", village = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", id = "^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$" }, null);

            /************详细页面*****************/
            /************详细页面 带长文章分页*****************/

            routes.MapMyRoute("show_0_0",
                   "news/{cid}/{id}_{page}",
                   new { level = 1, action = "Show" }, new { cid = "^[A-Za-z-0-9]+$", id = "^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$", page = "^[0-9]+$" }, null);
            routes.MapMyRoute("show_1_0",
                  "news/{cid}/{id}_{page}.htm",
                  new { level = 1, action = "Show" }, new { cid = "^[A-Za-z-0-9]+$", id = "^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$", page = "^[0-9]+$" }, null);

            routes.MapMyRoute("show_2_0",
                   "{area}/news/{cid}/{id}_{page}",
                   new { level = 2, action = "Show" }, new { area = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", id = "^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$", page = "^[0-9]+$" }, null);
            routes.MapMyRoute("show_3_0",
                  "{area}/news/{cid}/{id}_{page}.htm",
                  new { level = 2, action = "Show" }, new { area = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", id = "^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$", page = "^[0-9]+$" }, null);

            routes.MapMyRoute("show_4_0",
                   "{area}/{county}/news/{cid}/{id}_{page}",
                   new { level = 3, action = "Show" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", id = "^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$", page = "^[0-9]+$" }, null);
            routes.MapMyRoute("show_5_0",
                  "{area}/{county}/news/{cid}/{id}_{page}.htm",
                  new { level = 3, action = "Show" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", id = "^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$", page = "^[0-9]+$" }, null);

            routes.MapMyRoute("show_6_0",
                   "{area}/{county}/{country}/news/{cid}/{id}_{page}",
                   new { level = 4, action = "Show" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", id = "^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$", page = "^[0-9]+$" }, null);
            routes.MapMyRoute("show_7_0",
                  "{area}/{county}/{country}/news/{cid}/{id}_{page}.htm",
                  new { level = 4, action = "Show" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", id = "^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$", page = "^[0-9]+$" }, null);

            routes.MapMyRoute("show_8_0",
                  "{area}/{county}/{country}/{village}/news/{cid}/{id}_{page}",
                  new { level = 5, action = "Show" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", village = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", id = "^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$", page = "^[0-9]+$" }, null);
            routes.MapMyRoute("show_9_0",
                  "{area}/{county}/{country}/{village}/news/{cid}/{id}_{page}.htm",
                  new { level = 5, action = "Show" }, new { area = "^[A-Za-z]+$", county = "^[A-Za-z]+$", country = "^[A-Za-z]+$", village = "^[A-Za-z]+$", cid = "^[A-Za-z-0-9]+$", id = "^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$", page = "^[0-9]+$" }, null);

            /************详细页面*****************/

            #endregion

        }
    }
}
