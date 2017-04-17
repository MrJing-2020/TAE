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
    public class RepositoryBase : RepositoryExtend,IRepositoryBase
    {
        public static DbContextExtend context = CacheHelper.GetItem<DbContextExtend>("DbContextBase", () => { return new DbContextBase(); });
        public RepositoryBase()
            : base(context)
        {
        }
        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
        }
    }
}
