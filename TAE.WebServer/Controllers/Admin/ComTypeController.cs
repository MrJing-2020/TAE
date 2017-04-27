using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TAE.WebServer.Common;
using TAE.Data.Model;

namespace TAE.WebServer.Controllers.Admin
{
    public class ComTypeController : BaseApiController
    {
        [HttpPost]
        public HttpResponseMessage AllTypes(dynamic param)
        {
            string sqlGetAll = @"select a.* from Type as a";
            return GetDataList<TAE.Data.Model.Type>(param, sqlGetAll);
        }

        [HttpPost]
        public HttpResponseMessage SubTypeData(Data.Model.Type model)
        {
            ServiceBase.SaveEntity<Data.Model.Type>(model);
            return Response();
        }
    }
}
