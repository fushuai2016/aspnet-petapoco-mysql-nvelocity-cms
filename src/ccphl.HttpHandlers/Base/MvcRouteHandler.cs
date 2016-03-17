using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace ccphl.HttpHandlers
{
    /// <summary>
    /// Mvc路由绑定MvcHandler处理接口
    /// </summary>
    public class MvcRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new MvcHandler(requestContext);
        }

        #region IRouteHandler Members

        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
        {
            return GetHttpHandler(requestContext);
        }
        #endregion
        
    }

}
