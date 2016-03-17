using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Model;
using System.IO;
using ccphl.Common;
using System.Text.RegularExpressions;

namespace ccphl.HttpHandlers
{
    /// <summary>
    /// 列表显示处理类
    /// </summary>
    public class ListRequest:IRequest
    {
        public override void ProcessRequest(System.Web.HttpContext context, RequestContext RequestContext)
        {
            //站点级别
            int level = Utils.ObjToInt(RequestContext.RouteData.Values["level"], 1);
            switch (level)
            {
                case 1:
                    MainSite(context, RequestContext);
                    break;
                case 2:
                    AreaSite(context, RequestContext);
                    break;
                case 3:
                    CountySite(context, RequestContext);
                    break;
                case 4:
                    CountrySite(context, RequestContext);
                    break;
                case 5:
                    VillageSite(context, RequestContext);
                    break;
                default:
                    ResponseError("未知站点级别！");
                    break;
            } 
        }

        #region 主站点
        /// <summary>
        /// 主站点
        /// </summary>
        /// <param name="context"></param>
        /// <param name="RequestContext"></param>
        private void MainSite(System.Web.HttpContext context, RequestContext RequestContext)
        {
            
           
        }
        #endregion

        #region 区市级站点
        /// <summary>
        /// 区市级站点
        /// </summary>
        /// <param name="context"></param>
        /// <param name="RequestContext"></param>
        private void AreaSite(System.Web.HttpContext context, RequestContext RequestContext)
        {
            
        }
        #endregion

        #region 县级站点
        /// <summary>
        /// 县级站点
        /// </summary>
        /// <param name="context"></param>
        /// <param name="RequestContext"></param>
        private void CountySite(System.Web.HttpContext context, RequestContext RequestContext)
        {
            
        }
        #endregion

        #region 乡级站点
        /// <summary>
        /// 乡级站点
        /// </summary>
        /// <param name="context"></param>
        /// <param name="RequestContext"></param>
        private void CountrySite(System.Web.HttpContext context, RequestContext RequestContext)
        {
            
        }
        #endregion

        #region 村级站点
        /// <summary>
        /// 村级站点
        /// </summary>
        /// <param name="context"></param>
        /// <param name="RequestContext"></param>
        private void VillageSite(System.Web.HttpContext context, RequestContext RequestContext)
        {
            
        }
        #endregion

        #region 独立域名处理
        /// <summary>
        /// 站点独立域名处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="RequestContext"></param>
        private void SingleDomian(System.Web.HttpContext context, RequestContext RequestContext)
        {
            string area = Utils.ObjectToStr(RequestContext.RouteData.Values["area"]);
            string county = Utils.ObjectToStr(RequestContext.RouteData.Values["county"]);
            string country = Utils.ObjectToStr(RequestContext.RouteData.Values["country"]);
            string village = Utils.ObjectToStr(RequestContext.RouteData.Values["village"]);
            
        }
        #endregion
    }
}
