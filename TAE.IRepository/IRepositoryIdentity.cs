using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TAE.IRepository
{
    using TAE.Data.Model;
    public interface IRepositoryIdentity : IDisposable
    {
        #region AppUser增删查改

        #region AppUser查询
        IQueryable<AppUser> FindUser();
        IQueryable<AppUser> FindUser(Expression<Func<AppUser, bool>> conditions = null);
        Task<AppUser> FindByNameAndPas(string userName, string password);
        Task<AppUser> FindUserById(string userId);
        Task<AppUser> FindUserByEmail(string email);
        Task<AppUser> FindUserByName(string userName);
        Task<AppUser> FindLoginUserByName(string userName);
        #endregion

        Task<bool> CreateUser(AppUser user, string password);
        Task<bool> CreateUser(AppUser user);

        Task<bool> UpdateUser(AppUser user);

        Task<bool> DeleteUser(AppUser user);
        string GetHashPassword(string password);
        #endregion

        #region AppRole增删查改

        #region AppRole查询
        IQueryable<AppRole> FindRole();
        IQueryable<AppRole> FindRole(Expression<Func<AppRole, bool>> conditions = null);
        Task<AppRole> FindRoleById(string roleId);
        Task<AppRole> FindRoleByName(string roleName);
        Task<bool> RoleExists(string roleName);
        IQueryable<AppRole> FindRoleGeneral(Expression<Func<AppRole, bool>> conditions = null);
        #endregion

        Task<bool> CreateRole(AppRole role);

        Task<bool> UpdateRole(AppRole role);

        Task<bool> DeleteRole(AppRole role);
        #endregion

        Task<IQueryable<AppUser>> FindUserInToRole(string roleId);

        Task<IQueryable<AppUser>> FindUserNotInToRole(string roleId);

        Task<bool> AddToRoleById(string userId, string roleId);
        Task<bool> AddToRoleById(string userId, string[] roleIds);
        Task<bool> AddToRoleByName(string userId, string roleName);
        Task<bool> AddToRoleByName(string userId, string[] roleNames);

        Task<bool> RemoveFromRoleById(string userId, string roleId);
        Task<bool> RemoveFromRoleById(string userId, string[] roleIds);
        Task<bool> RemoveFromRoleByName(string userId, string roleName);
        Task<bool> RemoveFromRoleByName(string userId, string[] roleNames);

        Task<bool> AddRefreshToken(RefreshToken token);

        Task<bool> RemoveRefreshToken(string refreshTokenId);

        Task<bool> RemoveRefreshToken(RefreshToken refreshToken);

        Task<RefreshToken> FindRefreshToken(string refreshTokenId);
    }
}
