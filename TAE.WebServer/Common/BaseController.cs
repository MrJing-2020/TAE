using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAE.WebServer.Common
{
    using AutoMapper;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using TAE.Data.Model;
    using TAE.IService;
    using TAE.Utility.Common;

    public class BaseController : Controller
    {
        protected IServiceBase ServiceBase
        {
            get
            {
                return ServiceContext.Current.ServiceBase;
            }
        }
        protected IServiceIdentity ServiceIdentity
        {
            get
            {
                return ServiceContext.Current.ServiceIdentity;
            }
        }
        protected AppUser LoginUser
        {
            get
            {
                string userName = User.Identity.Name;
                AppUser user = null;
                //开启新线程执行async方法，防止线程锁死(使用async await可不必如此，这里想让它以同步方式执行)
                Task.Run<AppUser>(() => ServiceIdentity.FindLoginUserByName(User.Identity.Name))
                .ContinueWith(m => { m.Wait(); user = m.Result; })
                .Wait();
                return user;
            }
        }

        /// <summary>
        /// 将T1映射为T2
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        protected T2 Map<T1, T2>(T1 model)
        {
            return Mapper.Map<T2>(model);
        }
    }
}