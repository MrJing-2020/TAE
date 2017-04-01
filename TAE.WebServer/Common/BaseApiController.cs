using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace TAE.WebServer.Common
{
    using AutoMapper;
    using Newtonsoft.Json.Linq;
    using System.Data.SqlClient;
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
        protected LoginUser LoginUser
        {
            get
            {
                //string userName = User.Identity.Name;
                //AppUser user = null;
                ////开启新线程执行async方法，防止线程锁死(使用async await可不必如此，这里想让它以同步方式执行)
                //Task.Run<AppUser>(() => ServiceIdentity.FindLoginUserByName(User.Identity.Name))
                //.ContinueWith(m => { m.Wait(); user = m.Result; })
                //.Wait();
                //return user;
                LoginUser loginUser = new LoginUser();
                AppUser appUser = ServiceIdentity.FindUser(m => m.UserName == User.Identity.Name).FirstOrDefault();
                string[] roleIds = appUser.Roles.Select(m => m.RoleId).ToArray();
                List<DataRole> dataPower = ServiceBase.FindBy<DataRole>(m => roleIds.Any(n => n == m.RoleId)).ToList();
                loginUser.UserInfo = appUser;
                loginUser.DataPower = dataPower;
                return loginUser;
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

        /// <summary>
        /// 获取列表数据通用方法
        /// </summary>
        protected HttpResponseMessage GetDataList<T>(dynamic param,string sqlGetAll) where T : class
        {
            RequestArg arg = new RequestArg()
            {
                PageNumber = Convert.ToInt32(param.pageNumber),
                PageSize = Convert.ToInt32(param.pageSize),
            };
            PageList<T> list = new PageList<T>();
            try
            {
                //若为{}无法判断，所以这里放都try中
                if (param.search != null)
                {
                    //多字段模糊查询
                    string sqlSearchPart =sqlGetAll.Contains("where")? " where ":" and ";
                    JObject searchField = JObject.Parse(param.search.ToString());
                    foreach (var item in searchField)
                    {
                        sqlSearchPart += item.Key + " like " + "'%" + item.Value + "%' and ";
                    }
                    sqlSearchPart = sqlSearchPart.Substring(0, sqlSearchPart.LastIndexOf("and"));
                    sqlGetAll += sqlSearchPart;
                }
            }
            catch (Exception)
            {
                //单字段排序
                if (!string.IsNullOrEmpty(param.orderName.ToString()))
                {
                    sqlGetAll += " order by " + param.orderName.ToString() + ' ' + param.orderType.ToString();
                }
                list = ServiceBase.FindAllByPage<T>(sqlGetAll, arg);
                return Request.CreateResponse(list);
            }
            //单字段排序
            if (!string.IsNullOrEmpty(param.orderName.ToString()))
            {
                sqlGetAll += " order by " + param.orderName.ToString() + ' ' + param.orderType.ToString();
            }
            list = ServiceBase.FindAllByPage<T>(sqlGetAll, arg);
            return Request.CreateResponse(list);
        }

        /// <summary>
        /// 获取列表数据(通过数据权限过滤)
        /// </summary>
        protected HttpResponseMessage GetDataListFilt<T>(dynamic param, string sqlGetAll) where T : class
        {
            //数据权限过滤
            string sqlDataPowerPart = sqlGetAll.Contains("where") ? " where " : " and ";
            foreach (var item in LoginUser.DataPower)
            {
                sqlDataPowerPart += "(CompanyId = " + item.CompanyId + " and DepartmentId = " + item.DepartmentId + ") or";
            }
            sqlDataPowerPart = sqlDataPowerPart.Substring(0, sqlDataPowerPart.LastIndexOf("or"));
            sqlGetAll += sqlDataPowerPart;
            RequestArg arg = new RequestArg()
            {
                PageNumber = Convert.ToInt32(param.pageNumber),
                PageSize = Convert.ToInt32(param.pageSize),
            };
            PageList<T> list = new PageList<T>();
            try
            {
                //若为{} 无法判断，所以这里放都try中
                if (param.search != null)
                {
                    //多字段模糊查询
                    string sqlSearchPart = sqlGetAll.Contains("where") ? " where " : " and ";
                    JObject searchField = JObject.Parse(param.search.ToString());
                    foreach (var item in searchField)
                    {
                        sqlSearchPart += item.Key + " like " + "'%" + item.Value + "%' and ";
                    }
                    sqlSearchPart = sqlSearchPart.Substring(0, sqlSearchPart.LastIndexOf("and"));
                    sqlGetAll += sqlSearchPart;
                }
            }
            catch (Exception)
            {
                //单字段排序
                if (!string.IsNullOrEmpty(param.orderName.ToString()))
                {
                    sqlGetAll += " order by " + param.orderName.ToString() + ' ' + param.orderType.ToString();
                }
                list = ServiceBase.FindAllByPage<T>(sqlGetAll, arg);
                return Request.CreateResponse(list);
            }
            //单字段排序
            if (!string.IsNullOrEmpty(param.orderName.ToString()))
            {
                sqlGetAll += " order by " + param.orderName.ToString() + ' ' + param.orderType.ToString();
            }
            list = ServiceBase.FindAllByPage<T>(sqlGetAll, arg);
            return Request.CreateResponse(list);
        }
    }
}