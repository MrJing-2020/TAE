using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace TAE.WebServer.Common
{
    using AutoMapper;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TAE.Data.Model;
    using TAE.IService;
    using TAE.Utility.Common;

    public class BaseApiController : ApiController
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

        protected HttpResponseMessage Response()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        protected HttpResponseMessage Response(object obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK,obj);
        }
        protected HttpResponseMessage Response<T>(T data)
        {
            return Request.CreateResponse<T>(HttpStatusCode.OK, data);
        }
        protected HttpResponseMessage Response<T>(HttpStatusCode statusCode, T data)
        {
            return Request.CreateResponse<T>(statusCode, data);
        }
        protected HttpResponseMessage Response<T>(T data, Uri url)
        {
            var response = Request.CreateResponse<T>(HttpStatusCode.OK, data);
            response.Headers.Location = url;
            return response;
        }

        /// <summary>
        /// 将T1映射为T2
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2">返回类型</typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        protected T2 Map<T1, T2>(T1 model)
        {
            return Mapper.Map<T2>(model);
        }
    }
}