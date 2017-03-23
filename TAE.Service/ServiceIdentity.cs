using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Service
{
    using TAE.Core.ServiceProvider;
    using TAE.Data.Model;
    using TAE.IRepository;
    using TAE.IService;
    public class ServiceIdentity : IServiceIdentity
    {
        private IRepositoryIdentity repositoryIdentity 
        {
            get 
            {
                return ServiceHelper.GetService<IRepositoryIdentity>();
            }
        }

        #region AppUser增删查改

        #region AppUser查询
        public IQueryable<AppUser> FindUser()
        {
            return repositoryIdentity.FindUser();
        }
        public IQueryable<AppUser> FindUser(Expression<Func<AppUser, bool>> conditions = null)
        {
            return repositoryIdentity.FindUser(conditions);
        }
        public PageList<AppUser> FindUserByPage(Expression<Func<AppUser, bool>> conditions, Expression<Func<AppUser, object>> orderBy, RequestArg arg)
        {
            PageList<AppUser> pageList = new PageList<AppUser>() 
            {
                DataList = repositoryIdentity.FindUser(conditions),
                PageNumber = arg.PageNumber,
                PageSize = arg.PageSize,
            };
            pageList.Total = pageList.DataList.Count();
            pageList.DataList = (!arg.IsAsc ? pageList.DataList.OrderByDescending<AppUser, object>(orderBy) : pageList.DataList.OrderBy<AppUser, object>(orderBy));
            pageList.DataList = pageList.DataList.Skip((arg.PageNumber - 1) * arg.PageSize).Take(arg.PageSize);
            return pageList;
        }
        public PageList<AppUser> FindUserByPage(Expression<Func<AppUser, bool>> conditions)
        {
            return FindUserByPage(m => true, n => n.Id, new RequestArg());
        }
        public async Task<AppUser> FindByNameAndPas(string userName, string password)
        {
            return await repositoryIdentity.FindByNameAndPas(userName, userName);
        }
        public async Task<AppUser> FindUserById(string userId)
        {
            return await repositoryIdentity.FindUserById(userId);
        }
        public async Task<AppUser> FindUserByEmail(string email)
        {
            return await repositoryIdentity.FindUserByEmail(email);
        }
        public async Task<AppUser> FindUserByName(string userName)
        {
            return await repositoryIdentity.FindUserByName(userName);
        }
        public async Task<AppUser> FindLoginUserByName(string userName)
        {
            return await repositoryIdentity.FindLoginUserByName(userName);
        }
        #endregion

        public async Task<bool> CreateUser(AppUser user, string password)
        {
            return await repositoryIdentity.CreateUser(user,password);
        }
        public async Task<bool> CreateUser(AppUser user)
        {
            return await repositoryIdentity.CreateUser(user);
        }

        public async Task<bool> UpdateUser(AppUser user)
        {
            return await repositoryIdentity.UpdateUser(user);
        }

        public async Task<bool> DeleteUser(AppUser user)
        {
            return await repositoryIdentity.DeleteUser(user);
        }
        public string GetHashPassword(string password)
        {
            return repositoryIdentity.GetHashPassword(password);
        }
        #endregion

        #region AppRole增删查改

        #region AppRole查询
        public IQueryable<AppRole> FindRole()
        {
            return repositoryIdentity.FindRole();
        }
        public IQueryable<AppRole> FindRole(Expression<Func<AppRole, bool>> conditions = null)
        {
            return repositoryIdentity.FindRole(conditions);
        }
        public PageList<AppRole> FindRoleByPage(Expression<Func<AppRole, bool>> conditions, Expression<Func<AppRole, object>> orderBy, RequestArg arg)
        {
            PageList<AppRole> pageList = new PageList<AppRole>()
            {
                DataList = repositoryIdentity.FindRole(conditions),
                PageNumber = arg.PageNumber,
                PageSize = arg.PageSize,
            };
            pageList.Total = pageList.DataList.Count();
            pageList.DataList = (!arg.IsAsc ? pageList.DataList.OrderByDescending<AppRole, object>(orderBy) : pageList.DataList.OrderBy<AppRole, object>(orderBy));
            pageList.DataList = pageList.DataList.Skip((arg.PageNumber - 1) * arg.PageSize).Take(arg.PageSize);
            return pageList;
        }
        public PageList<AppRole> FindRoleByPage(Expression<Func<AppRole, bool>> conditions)
        {
            return FindRoleByPage(m => true, n => n.Id, new RequestArg());
        }
        public async Task<AppRole> FindRoleById(string roleId)
        {
            return await repositoryIdentity.FindRoleById(roleId);
        }
        public async Task<AppRole> FindRoleByName(string roleName)
        {
            return await repositoryIdentity.FindRoleByName(roleName);
        }
        public async Task<bool> RoleExists(string roleName)
        {
            return await repositoryIdentity.RoleExists(roleName);
        }
        public IQueryable<AppRole> FindRoleGeneral(Expression<Func<AppRole, bool>> conditions = null)
        {
            return repositoryIdentity.FindRoleGeneral(conditions);
        }
        #endregion

        public async Task<bool> CreateRole(AppRole role)
        {
            return await repositoryIdentity.CreateRole(role);
        }

        public async Task<bool> UpdateRole(AppRole role)
        {
            return await repositoryIdentity.UpdateRole(role);
        }

        public async Task<bool> DeleteRole(AppRole role)
        {
            return await repositoryIdentity.DeleteRole(role);
        }
        #endregion

        public async Task<IQueryable<AppUser>> FindUserInToRole(string roleId)
        {
            return await repositoryIdentity.FindUserInToRole(roleId);
        }

        public async Task<IQueryable<AppUser>> FindUserNotInToRole(string roleId)
        {
            return await repositoryIdentity.FindUserNotInToRole(roleId);
        }

        public async Task<bool> AddToRole(string userId, string roleName)
        {
            return await repositoryIdentity.AddToRole(userId,roleName);
        }

        public async Task<bool> RemoveFromRole(string userId, string roleName)
        {
            return await repositoryIdentity.RemoveFromRole(userId, roleName);
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            return await repositoryIdentity.AddRefreshToken(token);
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            return await repositoryIdentity.RemoveRefreshToken(refreshTokenId);
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            return await repositoryIdentity.RemoveRefreshToken(refreshToken);
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            return await repositoryIdentity.FindRefreshToken(refreshTokenId);
        }

        public void Dispose()
        {
            if (repositoryIdentity != null)
            {
                repositoryIdentity.Dispose();
            }
        }
    }
}
