using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TAE.WebServer.Controllers.Admin
{
    using Newtonsoft.Json.Linq;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using TAE.Data.Model;
    using TAE.Utility.Common;
    using TAE.WebServer.Common;

    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserManagerController : BaseApiController
    {
        private static string sqlGetUser = @"select a.*,b.CompanyName,c.DepartName,d.PositionName from AspNetUsers a 
                                            left join Company b on a.CompanyId=b.Id
                                            left join Department c on a.DepartmentId=c.Id
                                            left join Position d on a.PositionId=d.Id";

        [HttpPost]
        public HttpResponseMessage AllUsers(dynamic param)
        {
            string sqlGetAll = sqlGetUser;
            return GetDataList<UserViewModel>(param, sqlGetAll);
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetUserDetail(string id)
        {
            var appUser = await ServiceIdentity.FindUserById(id);
            //var user = Map<AppUser, UserViewModel>(AppUser);
            if (appUser != null)
            {
                return Response(appUser);
            }
            else
            {
                return Response(HttpStatusCode.NoContent, new { msg = "未找到任何信息" });
            }
        }

        /// <summary>
        /// 根据用户名判断用户是否已存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> ExistingUser(string name)
        {
            var appUser = await ServiceIdentity.FindUserByName(name);
            if (appUser == null)
            {
                return Response(new { Existing = false });
            }
            else
            {
                return Response(new { Existing = true });
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> SubUserData(UserViewModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                //var user = new AppUser { UserName = model.UserName, Email = model.Email };
                var user = new AppUser();
                model.Id = user.Id;
                user = Map<UserViewModel, AppUser>(model, user);
                //传入Password并转换成PasswordHash
                bool result = await ServiceIdentity.CreateUser(user, model.Password);
                if (result == true)
                {
                    return Response(user);
                }
                else
                {
                    return Response(HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                AppUser user = await ServiceIdentity.FindUserById(model.Id);
                if (user != null)
                {
                    user = Map<UserViewModel, AppUser>(model, user);
                    user.PasswordHash = ServiceIdentity.GetHashPassword(model.Password);
                    bool result = await ServiceIdentity.UpdateUser(user);
                    if (result == true)
                    {
                        return Response(user);
                    }
                    else
                    {
                        return Response(HttpStatusCode.InternalServerError);
                    }
                }
                else
                {
                    return Response(HttpStatusCode.InternalServerError);
                }
            }
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetRoleByUser(string id)
        {
            AppUser user = await ServiceIdentity.FindUserById(id);
            string[] roleIds = user.Roles.Select(x => x.RoleId).ToArray();
            List<AppRole> members = ServiceIdentity.FindRole().Where(m=>roleIds.Any(n=>n==m.Id)).ToList();
            List<AppRole> nonMembers = ServiceIdentity.FindRole().ToList().Except(members).ToList();
            return Response(new { UserIn = members, UserNotIn = nonMembers });
        }
        /// <summary>
        /// 角色分配
        /// </summary>
        /// <param name="model">Id:用户Id,BindIds:分配的角色Id(数组，可为多个)</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> RoleAllocationSub(BindOptionModel model)
        {
            var userId = model.Id;
            var appUser = await ServiceIdentity.FindUserById(userId);
            string[] oldRoleIds=appUser.Roles.Select(m=>m.RoleId).ToArray();
            //删除旧数据
            await ServiceIdentity.RemoveFromRoleById(userId, oldRoleIds);
            await ServiceIdentity.AddToRoleById(userId, model.BindIds);
            return Response();
        }

        /// <summary>
        /// 部门下拉框
        /// </summary>
        /// <param name="id">公司id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage DepSelectList(string id)
        {
            string sql = "select Id as 'Key',DepartName as 'Value' from Department where CompanyId = @Id";
            SqlParameter param = new SqlParameter("@Id", id);
            List<KeyValueModel> list = ServiceBase.FindBy<KeyValueModel>(sql,param).ToList();
            return Response(list);
        }
        /// <summary>
        /// 职位下拉框
        /// </summary>
        /// <param name="id">公司id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage PosSelectList(string id)
        {
            string sql = "select Id as 'Key',PositionName as 'Value' from Position where CompanyId = @Id";
            SqlParameter param = new SqlParameter("@Id", id);
            List<KeyValueModel> list = ServiceBase.FindBy<KeyValueModel>(sql, param).ToList();
            return Response(list);
        }
    }
}
