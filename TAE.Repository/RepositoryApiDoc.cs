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
        private DbContextApiDoc ContextApiDoc
        {
            get
            {
                return CacheHelper.GetItem<DbContextApiDoc>("DbContextApiDoc", () => { return new DbContextApiDoc(); });
            }
        }
        public RepositoryApiDoc()
            : base(CacheHelper.GetItem<DbContextApiDoc>("DbContextApiDoc", () => { return new DbContextApiDoc(); }))
        {
        }
        public new void Dispose()
        {
            base.Dispose();
            if (ContextApiDoc != null)
            {
                ContextApiDoc.Dispose();
            }
        }
    }
}
