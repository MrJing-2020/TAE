using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
    }
}
