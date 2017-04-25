using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Repository
{
    using TAE.Data.Entity;
    using TAE.IRepository;
    using TAE.Data.Model;
    using Core.Cache;

    /// <summary>
    /// EF增删查改封装类（仓储层）
    /// </summary>
    public class RepositoryBase : RepositoryExtend, IRepositoryBase
    {
        public DbContextBase ContextBase
        {
            get
            {
                return CacheHelper.GetItem<DbContextBase>("DbContextBase", () => { return new DbContextBase(); });
            }
        }
        public RepositoryBase()
            : base(CacheHelper.GetItem<DbContextBase>("DbContextBase", () => { return new DbContextBase(); }))
        {
        }
        public new void Dispose()
        {
            base.Dispose();
            if (ContextBase != null)
            {
                ContextBase.Dispose();
            }
        }
    }
}
