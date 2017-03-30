using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TAE.WebServer.Controllers.Admin
{
    using System.Data.SqlClient;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using TAE.Data.Model;
    using TAE.WebServer.Common;

    /// <summary>
    /// 菜单管理
    /// </summary>
    public class MenuManagerController : BaseApiController
    {
        [HttpGet]
        public HttpResponseMessage GetAllMenus(int pageNumber = 1, int pageSize = RequestArg.defualtPageSize, string orderName = "")
        {
            RequestArg arg = new RequestArg()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            string sqlGetAll = "select * from Menu";
            if (!string.IsNullOrEmpty(orderName))
            {
                SqlParameter para = new SqlParameter("@orderName", orderName);
                sqlGetAll = "select * from Menu order by @orderName";
            }
            var list = ServiceBase.FindAllByPage<UserViewModel>(sqlGetAll, arg);
            if (list != null)
            {
                return Response(list);
            }
            else
            {
                return Response(HttpStatusCode.NotFound, "未找到任何信息");
            }
        }
        [HttpGet]
        public HttpResponseMessage GetMenuDetail(int id)
        {
            var menu = ServiceBase.FindBy<Menu>(m => m.Id == id).FirstOrDefault();
            if (menu != null)
            {
                return Response(menu);
            }
            else
            {
                return Response(HttpStatusCode.NotFound, "未找到任何信息");
            }
        }
        [HttpPost]
        public HttpResponseMessage SubUserData(Menu menu)
        {
            var menuEntity = ServiceBase.SaveEntity<Menu>(menu);
            return Response(menuEntity);
        }


        #region 私有方法

        //[NonAction]
        //private HttpResponseMessage GetList<T>(Expression<Func<T, bool>> where) where T:class
        //{
        //    var list = ServiceBase.FindBy<T>(where);
        //    return Response(list);
        //} 

        #endregion
    }
}
