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
        [HttpPost]
        public HttpResponseMessage AllUsers(dynamic param)
        {
            string sqlGetAll = "select Id, Email,PhoneNumber,UserName from AspNetUsers";
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
                return Response(HttpStatusCode.NotFound, new { error_description = "未找到任何信息" });
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> SubUserData(UserViewModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                var user = new AppUser { UserName = model.UserName, Email = model.Email };
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
                    user.PasswordHash = ServiceIdentity.GetHashPassword(model.Password);
                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    bool result = await ServiceIdentity.UpdateUser(user);
                    if (result == true)
                    {
                        return Response(user);
                    }
                    else
                    {
                        return Response(HttpStatusCode.InternalServerError, new { error_description = "服务器错误" });
                    }
                }
                else
                {
                    return Response(HttpStatusCode.InternalServerError, new { error_description = "服务器错误" });
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
    }
}
