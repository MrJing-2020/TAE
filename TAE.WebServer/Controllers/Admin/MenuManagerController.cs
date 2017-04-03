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
        public HttpResponseMessage GetMenuDetail(string id)
        {
            var menu = ServiceBase.FindBy<Menu>(m => m.Id == id).FirstOrDefault();
            if (menu != null)
            {
                return Response(menu);
            }
            else
            {
                return Response(HttpStatusCode.NoContent,new { msg="没有任何信息"});
            }
        }
        [HttpGet]
        public HttpResponseMessage GetActions(string id)
        {
            var actions = ServiceBase.FindBy<Menu>(m => m.MenuPareId == id && m.MenuLever == 3).ToList();
            if (actions.Count()>0)
            {
                return Response(actions);
            }
            else
            {
                return Response(HttpStatusCode.NoContent, new { msg = "没有任何信息" });
            }
        }
        /// <summary>
        /// model包含（Id:自身Id，Action:方法名，MenuName：菜单名，MenuPareId：父菜单Id）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SubAction(dynamic model)
        {
            if (string.IsNullOrEmpty(model.Id.ToString()))
            {
                string menuPareId = model.MenuPareId;
                var menu = ServiceBase.FindBy<Menu>("select * from Menu where Id = @Id",new SqlParameter("@Id",menuPareId)).FirstOrDefault();
                menu.Id = "";
                menu.MenuPareId = menuPareId;
                menu.MenuLever = 3;
                menu.Action = model.Action;
                menu.MenuName = model.MenuName;
                ServiceBase.Insert<Menu>(menu);
            }
            else
            {
                string id = model.Id;
                var menu = ServiceBase.FindBy<Menu>(m => m.Id == id).FirstOrDefault();
                menu.Action = model.Action;
                menu.MenuName = model.MenuName;
                ServiceBase.Update<Menu>(menu);
            }
            return Response(new { msg = "提交成功！" });
        }

        [HttpPost]
        public HttpResponseMessage SubMenuData(Menu model)
        {
            ServiceBase.SaveEntity<Menu>(model);
            return Response(new { msg = "提交成功" });
        }
    }
}
