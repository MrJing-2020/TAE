using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TAE.Data.Model;
using TAE.WebServer.Common;

namespace TAE.WebServer.Controllers.Teacher
{
    /// <summary>
    /// 课程
    /// </summary>
    public class CourseController : BaseApiController
    {
        /// <summary>
        /// 获取所有课程
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage AllCourses(bool onlyOurCompany = false)
        {
            List<Course> list = new List<Course>();
            if (onlyOurCompany == false)
            {
                list = ServiceBase.FindBy<Course>().ToList();
            }
            else
            {
                list = ServiceBase.FindBy<Course>(m => m.CompanyId == LoginUser.UserInfo.CompanyId).ToList();
            }
            return Response(list);
        }

        [HttpPost]
        public HttpResponseMessage SubCourseData(Course model)
        {
            ServiceBase.SaveEntity<Course>(model);
            return Response();
        }

        /// <summary>
        /// 将课程直接与用户关联
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CourseLinkToUser(BindOptionModel model)
        {
            var courseId = model.Id;
            List<UserRCourse> list = new List<UserRCourse>();
            foreach (var item in model.BindIds)
            {
                var userRCourser = new UserRCourse
                {
                    UserId = item,
                    CourseId = courseId
                };
                list.Add(userRCourser);
            }
            ServiceBase.Insert<UserRCourse>(list);
            return Response();
        }
    }
}
