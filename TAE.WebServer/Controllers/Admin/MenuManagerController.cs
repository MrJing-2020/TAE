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
        [HttpPost]
        public HttpResponseMessage AllMenus(dynamic param)
        {
            string sqlGetAll = @"select a.*,b.MenuName as PareMenuName from Menu as a left join Menu as b
                                on a.MenuPareId=b.Id
                                where (a.MenuLever=1 or a.MenuLever=2)";
            return GetDataList<MenuViewModel>(param, sqlGetAll);
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
    }
}
