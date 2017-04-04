using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TAE.WebServer.Controllers.Admin
{
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using TAE.Data.Model;
    using TAE.WebServer.Common;

    /// <summary>
    /// 授权管理
    /// </summary>
    public class AuthorityController : BaseApiController
    {
        /// <summary>
        /// 获取用户权限信息
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetUserAuthority(string id)
        {
            List<Menu> menuList;
            //获取用户角色(或许有多个)
            var roleIds = ServiceIdentity.FindUser(m => m.Id == id).FirstOrDefault().Roles.Select(m => m.RoleId).ToArray();
            var roleIdsStr = string.Join("','", roleIds);
            string sqlGetAuthority = "select * from Menu where Id in (select MenuId from MenuRole where RoleId in ( '" + roleIdsStr + "' )) ";
            //根据角色id获取菜单列表
            menuList = ServiceBase.FindBy<Menu>(sqlGetAuthority).ToList();
            return Response(menuList);
        }

        /// <summary>
        /// 获取角色权限信息
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetRoleAuthority(string id)
        {
            List<Menu> menuListIn = new List<Menu>();
            List<Menu> menuList = new List<Menu>();
            SqlParameter parameter = new SqlParameter("@roleId", id);
            string sqlGetAuthority = "select * from Menu where Id in (select MenuId from MenuRole where RoleId = @roleId)";
            menuListIn = ServiceBase.FindBy<Menu>(sqlGetAuthority,parameter).ToList();
            menuList = ServiceBase.FindBy<Menu>().Except<Menu>(menuListIn).ToList();
            return Response(new { menuIn = menuListIn, menuAll = menuList });
        }

        /// <summary>
        /// 更新角色权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpdateAuthority(BindOptionModel model)
        {
            var roleId = model.Id;
            string[] menuIds = model.BindIds;
            List<MenuRole> menuRoleList = new List<MenuRole>();
            foreach (var item in menuIds)
	        {
                var menurole = new MenuRole()
                {
                    MenuId = item,
                    RoleId = roleId
                };
                menuRoleList.Add(menurole);
	        }
            ServiceBase.Remove<MenuRole>(m => m.RoleId == roleId);
            ServiceBase.Insert<MenuRole>(menuRoleList);
            return Response();
        }
    }
}
