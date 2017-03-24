using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace TAE.WebServer.Attribute
{
    using TAE.Data.Model;
    using TAE.IService;
    using TAE.Utility.Common;
    public class ApiCustomAuthorizeAttribute : AuthorizeAttribute
    {
        private string[] RoleNames { get; set; }
        private IServiceBase ServiceBase
        {
            get
            {
                return ServiceContext.Current.ServiceBase;
            }
        }
        private IServiceIdentity ServiceIdentity
        {
            get
            {
                return ServiceContext.Current.ServiceIdentity;
            }
        }
        /// <summary>
        /// 重写验证不通过时的处理，返回403，json格式数据，防止跳转到登录页
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            var response = filterContext.Response = filterContext.Response ?? new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Forbidden;
            response.Content = new StringContent(Json.Encode(new
            {
                success = false,
                errs = "访问被拒绝:您没有权限!"
            }), Encoding.UTF8, "application/json");
        }

        protected override bool IsAuthorized(HttpActionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("HttpActionContext");
            }
            if (!filterContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                return false;
            }
            if (RoleNames == null)
            {
                return true;
            }
            if (RoleNames.Length == 0)
            {
                return true;
            }
            if (RoleNames.Any(filterContext.RequestContext.Principal.IsInRole))
            {
                return true;
            }
            return false;
        }

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            //string areaName = filterContext.RequestContext.RouteData.Route.DataTokens["area"].ToString();
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            //根据控制器名和方法名获取角色名
            var menu = ServiceBase.FindBy<Menu>(m => m.Controller == controllerName && m.Action == actionName).FirstOrDefault();
            int menuId = 0;
            if (menu != null)
            {
                menuId = menu.Id;
            }
            string[] roleIds = ServiceBase.FindBy<MenuRole>(m => m.MenuId == menuId).Select(m => m.RoleId).ToArray();
            //开启新线程执行async方法，防止线程锁死
            Task.Run<string[]>(() => ServiceIdentity.FindRoleGeneral(m => roleIds.Any(y => y == m.Id)).Select(m => m.Name).ToArray())
            .ContinueWith(m => { m.Wait(); RoleNames = m.Result; })
            .Wait();
            base.OnAuthorization(filterContext);
        }
    }
}