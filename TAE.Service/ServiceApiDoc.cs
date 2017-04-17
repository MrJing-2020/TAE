using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAE.Core.ServiceProvider;
using TAE.IRepository;
using TAE.IService;

namespace TAE.Service
{
    public class ServiceApiDoc : ServiceExtend, IServiceApiDoc
    {
        public IRepositoryApiDoc RepositoryApiDoc
        {
            get
            {
                return ServiceHelper.GetService<IRepositoryApiDoc>();
            }
        }
        public ServiceApiDoc()
            : base(ServiceHelper.GetService<IRepositoryApiDoc>())
        {
        }

        public new void Dispose()
        {
            base.Dispose();
            if (RepositoryApiDoc != null)
            {
                RepositoryApiDoc.Dispose();
            }
        }
    }
}
