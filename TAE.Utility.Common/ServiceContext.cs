using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Utility.Common
{
    using TAE.Core.Cache;
    using TAE.IService;
    using TAE.Core.ServiceProvider;
    public class ServiceContext
    {
        public static ServiceContext Current
        {
            get
            {
                return CacheHelper.Get<ServiceContext>("ServiceContext", () => new ServiceContext());
            }
        }

        public IServiceBase ServiceBase
        {
            get
            {
                return ServiceHelper.GetService<IServiceBase>();
            }
        }
        public IServiceIdentity ServiceIdentity
        {
            get
            {
                return ServiceHelper.GetService<IServiceIdentity>();
            }
        }
    }
}
