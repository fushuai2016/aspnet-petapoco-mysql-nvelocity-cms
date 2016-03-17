using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace ccphl.HttpHandlers
{
     /// <summary>
    /// 请求统一入口
    /// </summary>
    public class Request:IRequest
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="context"></param>
        public override void ProcessRequest(System.Web.HttpContext context, RequestContext RequestContext)
        {
            string action = RequestContext.RouteData.Values["action"]==null? "Index" : RequestContext.RouteData.Values["action"].ToString();
            IRequest request = (IRequest)this.CreateIRequest(action);
            if (request != null)
            {
                request.ProcessRequest(context, RequestContext);
            }
        }
    }
}
