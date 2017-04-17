using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Entity
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using TAE.Core.Config;
    using TAE.Data.Model;
    public class DbContextApiDoc : DbContextExtend
    {
        private static string connString = CachedConfigContext.Current.DaoConfig.TAE_ApiDoc;
        public DbContextApiDoc()
            : base(connString)
        {
            this.Database.Connection.ConnectionString = connString;
        }

        #region 映射的数据库表
        public DbSet<DocMain> DocMain { get; set; }
        public DbSet<OpenApi> OpenApi { get; set; }
        #endregion
    }
}
