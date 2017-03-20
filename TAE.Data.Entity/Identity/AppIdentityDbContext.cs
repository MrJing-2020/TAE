using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Entity
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using TAE.Core.Config;
    using TAE.Data.Model;
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        private static string connString = CachedConfigContext.Current.DaoConfig.TAE_Base;
        public AppIdentityDbContext()
            : base(connString)
        {
        }
        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            //防止生产复数表名
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public static AppIdentityDbContext Create()
        {
            return new AppIdentityDbContext();
        }

        #region 映射的数据库表
        public DbSet<RefreshToken> RefreshToken { get; set; }
        #endregion
    }
}
