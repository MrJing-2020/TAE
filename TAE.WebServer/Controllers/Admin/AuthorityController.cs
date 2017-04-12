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
            //获取用户角色(或许有多个)
            var roleIds = ServiceIdentity.FindUser(m => m.Id == id).FirstOrDefault().Roles.Select(m => m.RoleId).ToArray();
            var roleIdsStr = string.Join("','", roleIds);
            string sqlGetAuthority = "select * from Menu where Id in (select MenuId from MenuRole where RoleId in ( '" + roleIdsStr + "' )) ";
            string sqlGetDataAuthority = "select * from Department where Id in (select DepartmentId from DataRole where RoleId in ( '" + roleIdsStr + "' )) ";
            //根据角色id获取菜单列表
            List<Menu> menuList = ServiceBase.FindBy<Menu>(sqlGetAuthority).ToList();
            List<Department> depList = ServiceBase.FindBy<Department>(sqlGetDataAuthority).ToList();
            List<Company> comList = ServiceBase.FindBy<Company>().ToList(); 
            List<JsTreeModel> optionList = new List<JsTreeModel>();
            List<JsTreeModel> dataList = new List<JsTreeModel>();
            foreach (var item in menuList)
            {
                var treeItem = new JsTreeModel
                {
                    id = item.Id,
                    text = item.MenuName,
                    parent = item.MenuPareId,
                    icon = "fa fa-folder"
                };
                optionList.Add(treeItem);
            }
            foreach (var item in depList)
            {
                var treeItem = new JsTreeModel
                {
                    id = item.Id,
                    text = item.DepartName,
                    parent = item.CompanyId,
                    icon = "fa fa-database"
                };
                dataList.Add(treeItem);
                GetPreCompany(dataList, comList, item.CompanyId);
            }
            return Response(new { optionList = optionList, dataList = dataList });
        }

        /// <summary>
        /// 获取角色权限信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetRoleAuthority(string id)
        {
            List<Menu> menuList = new List<Menu>();
            SqlParameter parameter = new SqlParameter("@roleId", id);
            string sqlGetAuthority = "select Id from Menu where Id in (select MenuId from MenuRole where RoleId = @roleId)";
            string[] menuIdsIn = ServiceBase.FindBy<string>(sqlGetAuthority, parameter).ToArray();
            menuList = ServiceBase.FindBy<Menu>("select * from Menu").ToList();
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
                if (menuIdsIn.Contains(item.Id) && item.MenuLever == 3)
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
            var menuList = ServiceBase.FindBy<Menu>("select * from Menu").ToList();
            var roleId = model.Id;
            string[] menuIds = model.BindIds;
            List<MenuRole> menuRoleList = new List<MenuRole>();
            foreach (var item in menuIds)
	        {
                //判断父菜单是否被选中，若没有，则添加
                var parentId = menuList.Where(m => m.Id == item).Select(m => m.MenuPareId).FirstOrDefault();
                var parentMenu = menuList.Where(m => m.Id == parentId).FirstOrDefault();
                if (!menuIds.Contains(parentId))
                {
                    var menuPraRole = new MenuRole()
                    {
                        MenuId = parentId,
                        RoleId = roleId
                    };
                    menuRoleList.Add(menuPraRole);
                    if (parentMenu != null && !menuIds.Contains(parentMenu.MenuPareId) && parentMenu.MenuPareId != "#")
                    {
                        var menuGraPraRole = new MenuRole()
                        {
                            MenuId = parentMenu.MenuPareId,
                            RoleId = roleId
                        };
                        menuRoleList.Add(menuGraPraRole);
                    }
                }
                var menuRole = new MenuRole()
                {
                    MenuId = item,
                    RoleId = roleId
                };
                menuRoleList.Add(menuRole);
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
            IEnumerable<Department> depList;
            SqlParameter parameter = new SqlParameter("@roleId", id);
            string sqlGetDataAuthority = "select Id from Department where Id in (select DepartmentId from DataRole where RoleId = @roleId)";
            string[] depIdsIn = ServiceBase.FindBy<string>(sqlGetDataAuthority, parameter).ToArray();
            depList = ServiceBase.FindBy<Department>("select * from Department");
            List<JsTreeModel> treeList = new List<JsTreeModel>();
            foreach (var item in depList)
            {
                var treeItem = new JsTreeModel
                {
                    id = item.CompanyId + "$" + item.Id,
                    text = item.DepartName,
                    parent = item.CompanyId,
                    icon = "fa fa-database"
                };
                if (depIdsIn.Contains(item.Id))
                {
                    treeItem.state = new TreeStateModel { selected = true };
                }
                treeList.Add(treeItem);
            }
            IEnumerable<Company> comList = ServiceBase.FindBy<Company>("select * from Company");
            foreach (var item in comList)
            {
                var treeItem = new JsTreeModel
                {
                    id = item.Id,
                    text = item.CompanyName,
                    parent = item.PreCompanyId,
                    icon = "fa fa-database"
                };
                treeList.Add(treeItem);
            }
            return Response(treeList);
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
                if (item.Contains('$'))
                {
                    var arrTemp = item.Split(new Char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);
                    var datarole = new DataRole()
                    {
                        CompanyId = arrTemp[0],
                        DepartmentId = arrTemp[1],
                        RoleId = roleId
                    };
                    dataRoleList.Add(datarole);
                }
            }
            ServiceBase.Remove<DataRole>(m => m.RoleId == roleId);
            ServiceBase.Insert<DataRole>(dataRoleList);
            return Response();
        }

        #region 私有方法

        /// <summary>
        /// 递归获取母公司
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="comList"></param>
        /// <param name="PreCompanyId"></param>
        private void GetPreCompany(List<JsTreeModel> dataList, List<Company> comList, string PreCompanyId)
        {
            if (PreCompanyId == "#")
            {
                return;
            }
            if (dataList.Where(m => m.id == PreCompanyId).Count() <= 0)
            {
                var company = comList.Where(m => m.Id == PreCompanyId).FirstOrDefault();
                var treeItemCom = new JsTreeModel
                {
                    id = company.Id,
                    text = company.CompanyName,
                    parent = company.PreCompanyId,
                    icon = "fa fa-database"
                };
                dataList.Add(treeItemCom);
                GetPreCompany(dataList, comList, company.PreCompanyId);
            }
        } 
        #endregion
    }
}
