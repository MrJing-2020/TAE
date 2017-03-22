using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TAE.WebServer
{
    using TAE.WebServer.Attribute;
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //删除xml解析器
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "AreaApi",
                routeTemplate: "api/{area}/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //自定义异常权限认证，异常处理
            config.Filters.Add(new ApiCustomAuthorizeAttribute());
            config.Filters.Add(new ApiExceptionFilterAttribute());
        }
    }
}
