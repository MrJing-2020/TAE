using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TAE.Data.Model;

namespace TAE.WebServer.Common
{
    public class StudentApiController : BaseApiController
    {
        protected StudentInfo LoginStudent
        {
            get
            {
                return ServiceBase.FindBy<StudentInfo>(m => m.UserId == LoginUser.UserInfo.Id).FirstOrDefault();
            }
        }
    }
}
