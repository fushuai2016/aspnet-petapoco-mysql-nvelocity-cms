using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace ccphl.HttpHandlers
{
    /// <summary>
    /// http路由拦截
    /// </summary>
    public class MyRoutingModule : IHttpModule
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.PostResolveRequestCache += new EventHandler(context_PostResolveRequestCache);
        }

        void context_PostResolveRequestCache(object sender, EventArgs e)
        {
            HttpContextBase context = new HttpContextWrapper(((HttpApplication)sender).Context);
            this.PostResolveRequestCache(context);

        }

        private void PostResolveRequestCache(HttpContextBase context)
        {
            RouteData routeData = RouteTable.Routes.GetRouteData(context);

            if (routeData != null)
            {
                IRouteHandler routeHandler = routeData.RouteHandler;
                if (routeHandler != null)
                {
                    RequestContext requestContext = new RequestContext(context, routeData);
                    try
                    {
                        IHttpHandler httpHandler = routeHandler.GetHttpHandler(requestContext);
                        if (httpHandler != null)
                        {
                            //throw new InvalidOperationException("无法创建对应的HttpHandler对象");
                            context.RemapHandler(httpHandler);
                        }
                    }
                    catch
                    {
                        //var originalPath = context.Request.Path;
                        //HttpContext.Current.RewritePath(context.Request.ApplicationPath, false);
                        //HttpContext.Current.RewritePath(originalPath, false);
                    }
                }
            }
           
        }
    }
}
