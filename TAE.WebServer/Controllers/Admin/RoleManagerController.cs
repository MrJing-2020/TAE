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
    public class RoleManagerController : BaseApiController
    {
        [HttpPost]
        public HttpResponseMessage AllRoles(dynamic param)
        {
            string sqlGetAll = "select a.*,b.CompanyName from AspNetRoles a left join Company b on a.CompanyId=b.Id";
            return GetDataList<RoleViewModel>(param, sqlGetAll);
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetRoleDetail(string id)
        {
            var appRole = await ServiceIdentity.FindRoleById(id);
            return Response(appRole);

        }

        [HttpPost]
        public async Task<HttpResponseMessage> SubRoleData(AppRole model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                bool result = await ServiceIdentity.CreateRole(model);
                if (result == true)
                {
                    return Response(model);
                }
                else
                {
                    return ResponseException();
                }
            }
            else
            {
                AppRole role = await ServiceIdentity.FindRoleById(model.Id);
                if (role != null)
                {
                    role.Name = model.Name;
                    role.CompanyId = model.CompanyId;
                    bool result = await ServiceIdentity.UpdateRole(role);
                    if (result == true)
                    {
                        return Response(role);
                    }
                    else
                    {
                        return ResponseException();
                    }
                }
                else
                {
                    return ResponseException();
                }
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetUserByRole(string id)
        {
            AppRole role = await ServiceIdentity.FindRoleById(id);
            string[] memberIds = role.Users.Select(x => x.UserId).ToArray();
            IEnumerable<AppUser> members = await ServiceIdentity.FindUserInRole(id);
            IEnumerable<AppUser> nonMembers = await ServiceIdentity.FindUserNotInRole(id); 
            return Response(new { UserIn = members.ToList(), UserNotIn = nonMembers.ToList() });
        }

        [HttpPost]
        public async Task<HttpResponseMessage> RoleAllocationSub(BindOptionModel model)
        {
            var roleId = model.Id;
            AppRole appRole = await ServiceIdentity.FindRoleById(roleId);
            string[] oldUserIds = appRole.Users.Select(m => m.UserId).ToArray();
            //删除旧数据
            await ServiceIdentity.RemoveFromRoleById(oldUserIds, roleId);
            await ServiceIdentity.AddToRoleById(model.BindIds, roleId);
            return Response();
        }
    }
}
