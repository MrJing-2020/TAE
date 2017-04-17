using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAE.Core.Cache;
using TAE.Data.Entity;
using TAE.IRepository;

namespace TAE.Repository
{
    public class RepositoryApiDoc : RepositoryExtend, IRepositoryApiDoc
    {
        private static DbContextApiDoc context = CacheHelper.GetItem<DbContextApiDoc>("DbContextApiDoc", () => { return new DbContextApiDoc(); });
        public RepositoryApiDoc()
            : base(context)
        {
        }
        //public void Dispose()
        //{
        //    if (Context != null)
        //    {
        //        Context.Dispose();
        //    }
        //}
    }
}
