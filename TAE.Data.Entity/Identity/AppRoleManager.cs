using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Entity
{
    using TAE.Data.Model;
    public class AppRoleManager : RoleManager<AppRole>
    {
        public AppRoleManager(RoleStore<AppRole> store)
            : base(store)
        {
        }

        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options, IOwinContext context)
        {
            return new AppRoleManager(new RoleStore<AppRole>(context.Get<AppIdentityDbContext>()));
        }

        /// <summary>
        /// 解决context无法跨线程的问题(供查询角色用)
        /// </summary>
        /// <returns></returns>
        public static AppRoleManager GetRoleManager()
        {
            return new AppRoleManager(new RoleStore<AppRole>(new AppIdentityDbContext()));
        }
    }
}
