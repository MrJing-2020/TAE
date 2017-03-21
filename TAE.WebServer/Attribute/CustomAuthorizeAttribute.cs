using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TAE.WebServer.Attribute
{
    using TAE.Data.Model;
    using TAE.IService;
    using TAE.Utility.Common;
    /// <summary>
    /// 此处的AuthorizeAttribute在System.Web.Mvc命名空间下，只能用于mvc的请求验证，对webapi无效
    /// </summary>
    public class CustomAuthorizeAttribute : AuthorizeAttribute
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
        /// 认证逻辑：未查到对应的权限菜单，认为没有权限限制
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("HttpContext");
            }
            if (!httpContext.User.Identity.IsAuthenticated)
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
            if (RoleNames.Any(httpContext.User.IsInRole))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 根据区域，控制器和方法名获取对应的权限角色
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            string areaName = filterContext.RequestContext.RouteData.DataTokens["area"].ToString();
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            //根据控制器名和方法名获取角色名
            var menu = ServiceBase.FindBy<Menu>(m => m.Area == areaName && m.Controller == controllerName && m.Action == actionName).FirstOrDefault();
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