using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace ccphl.HttpHandlers
{
    /// <summary>
    ///MvcHandler处理接口
    /// </summary>
    public class MvcHandler : System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public RequestContext RequestContext { get; private set; }
        public MvcHandler(RequestContext requestContext)
        {
            this.RequestContext = requestContext;
        }

        public  void ProcessRequest(HttpContext context)
        {
            //执行统一的请求处理过程
            Request req = new Request();
            req.ProcessRequest(context, this.RequestContext);
        }
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}
