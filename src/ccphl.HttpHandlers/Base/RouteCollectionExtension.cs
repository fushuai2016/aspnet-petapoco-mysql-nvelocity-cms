using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Resources;


namespace ccphl.HttpHandlers
{
    /// <summary>
    /// 路由注册
    /// </summary>
    public static class RouteCollectionExtension
    {
        public static void IgnoreRoute(this RouteCollection routes, string url)
        {
            IgnoreRoute(routes, url, null /* constraints */);
        }
        public static void IgnoreRoute(this RouteCollection routes, string url, object constraints)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            IgnoreRouteInternal route = new IgnoreRouteInternal(url)
            {
                Constraints = new RouteValueDictionary(constraints)
            };

            routes.Add(route);
        }
        public static Route MapMyRoute(this RouteCollection routes, string name, string url, object defaults)
        {
            return MapMyRoute(routes, name, url, defaults, null, null);
        }

        public static Route MapMyRoute(this RouteCollection routes,  string name, string url, object defaults, object constraints, string[] namespaces)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            // 在这里注册 Route 与 自定义RouteHandler的映射关系
            Route route = new Route(url, new MvcRouteHandler())
            {
                Defaults = CreateRouteValueDictionary(defaults),
                Constraints = CreateRouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary()
            };

            if ((namespaces != null) && (namespaces.Length > 0))
            {
                route.DataTokens["Namespaces"] = namespaces;
            }

            routes.Add(name, route);

            return route;
        }
        private static RouteValueDictionary CreateRouteValueDictionary(object values)
        {
            var dictionary = values as IDictionary<string, object>;
            if (dictionary != null)
            {
                return new RouteValueDictionary(dictionary);
            }

            return new RouteValueDictionary(values);
        }
        private sealed class IgnoreRouteInternal : Route
        {
            public IgnoreRouteInternal(string url)
                : base(url, new StopRoutingHandler())
            {
            }

            public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary routeValues)
            {
                // Never match during route generation. This avoids the scenario where an IgnoreRoute with
                // fairly relaxed constraints ends up eagerly matching all generated URLs.
                return null;
            }
        }
    }
}
