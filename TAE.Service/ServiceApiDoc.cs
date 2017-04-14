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
        public static IRepositoryApiDoc repositoryApiDoc = ServiceHelper.GetService<IRepositoryApiDoc>();
         public ServiceApiDoc()
            : base(repositoryApiDoc)
        {
        }
    }
}
