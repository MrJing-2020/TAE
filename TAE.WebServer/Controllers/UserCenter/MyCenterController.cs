using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TAE.Data.Model;
using TAE.WebServer.Common;

namespace TAE.WebServer.Controllers.UserCenter
{
    public class MyCenterController : BaseApiController
    {
        [HttpGet]
        public HttpResponseMessage GetMyInfo()
        {
            return Response(LoginUser);
        }

        [HttpGet]
        public HttpResponseMessage GetMyInfoAndAuthority()
        {
            List<Menu> menuList = this.MenuAuthority;
            List<OperationViewModel> operationList = new List<OperationViewModel>();
            foreach (var menu in menuList)
            {
                if (menu.MenuLever == 1 || menu.MenuLever == 2)
                {
                    OperationViewModel operation = new OperationViewModel
                    {
                        Menu = menu,
                        Actions = new List<Menu>()
                    };
                    foreach (var action in menuList.Where(m=>m.MenuLever==3))
                    {
                        if (action.MenuPareId == menu.Id)
                        {
                            operation.Actions.Add(action);
                        }
                    }
                    operationList.Add(operation);
                }
            }
            return Response(new { userInfo = LoginUser.UserInfo, operation = operationList });
        }
    }
}
