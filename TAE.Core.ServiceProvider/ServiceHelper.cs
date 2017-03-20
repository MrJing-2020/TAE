using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TAE.Core.ServiceProvider
{
    using TAE.Core.Cache;
    public static class ServiceHelper
    {
        public static T GetService<T>()
        {
            T service = Instance<T>.Create;
            return service;
        }
    }
}
