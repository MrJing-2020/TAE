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
                var studentinfo = ServiceBase.FindBy<StudentInfo>(m => m.UserId == LoginUser.UserInfo.Id).FirstOrDefault();
                if (studentinfo == null)
                {
                    AppUser user = LoginUser.UserInfo;
                    StudentInfo student = new StudentInfo
                    {
                        UserId = user.Id,
                        NickName = "默认昵称",
                        PhotoUrl = "",
                        CompanyId = user.CompanyId,
                        DepartmentId = user.DepartmentId
                    };
                    ServiceBase.Insert<StudentInfo>(student);
                    return student;
                }
                else
                {
                    return studentinfo;
                }
            }
        }
    }
}
