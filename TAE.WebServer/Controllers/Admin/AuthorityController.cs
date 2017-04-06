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
        /// 获取角色操作权限信息
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <returns></returns>
        //[HttpGet]
        //public HttpResponseMessage GetRoleAuthority(string id)
        //{
        //    List<MenuViewModel> menuList = new List<MenuViewModel>();
        //    SqlParameter parameter = new SqlParameter("@roleId", id);
        //    string sqlGetAuthority = "select Id from Menu where Id in (select MenuId from MenuRole where RoleId = @roleId)";
        //    string[] menuIdsIn = ServiceBase.FindBy<string>(sqlGetAuthority,parameter).ToArray();
        //    menuList = ServiceBase.FindBy<MenuViewModel>("select * from Menu").ToList();
        //    foreach (var item in menuList)
        //    {
        //        if (menuIdsIn.Contains(item.Id))
        //        {
        //            item.IsInAuthority = true;
        //        }
        //        else
        //        {
        //            item.IsInAuthority = false;
        //        }
        //    }
        //    return Response(menuList);
        //}

        [HttpGet]
        public HttpResponseMessage GetRoleAuthority(string id)
        {
            List<MenuViewModel> menuList = new List<MenuViewModel>();
            SqlParameter parameter = new SqlParameter("@roleId", id);
            string sqlGetAuthority = "select Id from Menu where Id in (select MenuId from MenuRole where RoleId = @roleId)";
            string[] menuIdsIn = ServiceBase.FindBy<string>(sqlGetAuthority, parameter).ToArray();
            menuList = ServiceBase.FindBy<MenuViewModel>("select * from Menu").ToList();
            List<JsTreeModel> treeList = new List<JsTreeModel>();
            foreach (var item in menuList)
            {
                var treeItem = new JsTreeModel
                {
                    id = item.Id,
                    text = item.MenuName,
                    parent = item.MenuPareId,
                    icon = "fa fa-folder"
                };
                if (menuIdsIn.Contains(item.Id))
                {
                    treeItem.state = new TreeStateModel { selected = true };
                }
                treeList.Add(treeItem);
            }
            return Response(treeList);
        }

        /// <summary>
        /// 更新角色操作权限
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

        /// <summary>
        /// 获取角色数据权限信息
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetRoleDataAuthority(string id)
        {
            IEnumerable<DepViewModel> depList;
            SqlParameter parameter = new SqlParameter("@roleId", id);
            string sqlGetDataAuthority = "select Id from Department where Id in (select MenuId from RoleData where RoleId = @roleId)";
            string[] depIdsIn = ServiceBase.FindBy<string>(sqlGetDataAuthority, parameter).ToArray();
            depList = ServiceBase.FindBy<DepViewModel>("select * from Department");
            foreach (var item in depList)
            {
                if (depIdsIn.Contains(item.Id))
                {
                    item.IsInAuthority = true;
                }
                else
                {
                    item.IsInAuthority = false;
                }
            }
            IEnumerable<OrgnViewModel> orgList= ServiceBase.FindBy<OrgnViewModel>("select * from Company");
            foreach (var item in orgList)
            {
                item.Departments = depList.Where(m => m.CompanyId == item.Id).ToList();
            }
            return Response(orgList);
        }

        /// <summary>
        /// 更新角色数据权限
        /// </summary>
        /// <param name="model">model.BindIds传公司和部门Id，用$分割开</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpdateDataAuthority(BindOptionModel model)
        {
            var roleId = model.Id;
            string[] depIds = model.BindIds;
            List<DataRole> dataRoleList = new List<DataRole>();
            foreach (var item in depIds)
            {
                var arrTemp= item.Split(new Char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);
                var datarole = new DataRole()
                {
                    CompanyId=arrTemp[0],
                    DepartmentId = arrTemp[1],
                    RoleId = roleId
                };
                dataRoleList.Add(datarole);
            }
            ServiceBase.Remove<DataRole>(m => m.RoleId == roleId);
            ServiceBase.Insert<DataRole>(dataRoleList);
            return Response();
        }
    }
}
