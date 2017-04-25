using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TAE.Data.Model;
using TAE.WebServer.Common;

namespace TAE.WebServer.Controllers.Stutent
{
    public class MyCenterController : BaseApiController
    {
        /// <summary>
        /// 获取我的所有课程
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetMyCourses()
        {
            string sqlGetMyCourses = @"select * from Course where Id in(select CourseId from UserRCourse where UserId = @UserId)";
            SqlParameter param = new SqlParameter("@UserId", LoginUser.UserInfo.Id);
            List<Course> courseList = ServiceBase.FindBy<Course>(sqlGetMyCourses, param).ToList();
            return Response(courseList);
        }

        /// <summary>
        /// 将课程加入我的学习计划(订阅)
        /// </summary>
        /// <param name="model">Id：课程Id</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddToMyPlan(OneParam model)
        {
            UserRCourse userRCourse = new UserRCourse
            {
                UserId = LoginUser.UserInfo.Id,
                CourseId = model.Id
            };
            ServiceBase.Insert<UserRCourse>(userRCourse);
            return Response();
        }
    }
}
