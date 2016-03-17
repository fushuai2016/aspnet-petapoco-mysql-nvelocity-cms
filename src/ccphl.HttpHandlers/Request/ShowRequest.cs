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
    /// 详细显示处理类
    /// </summary>
    public class ShowRequest:IRequest
    {
        public override void ProcessRequest(System.Web.HttpContext context, RequestContext RequestContext)
        {
            //站点级别
            int level = Utils.ObjToInt(RequestContext.RouteData.Values["level"], 1);
            switch (level)
            {
                
                default:
                    ResponseError("未知站点级别！");
                    break;
            } 
        }

     
    }
}
