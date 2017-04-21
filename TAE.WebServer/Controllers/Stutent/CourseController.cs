using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TAE.Data.Model;
using TAE.WebServer.Common;

namespace TAE.WebServer.Controllers
{
    public class CourseController : BaseApiController
    {
        /// <summary>
        /// 获取课程
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetCourses(dynamic param)
        {
            List<Course> list = new List<Course>();
            if (param.Category != null) {
                string categoryId=param.Category;
                list = ServiceBase.FindBy<Course>(m => m.CategoryId == categoryId).ToList();
            }
            else if (param.Name != null)
            {
                string courseName = param.Name;
                list = ServiceBase.FindBy<Course>(m => m.Name == courseName).ToList();
            }
            else
            {
                list = ServiceBase.FindBy<Course>(m => m.IsPublic == true).ToList();
            }
            return Response(list);
        }

        /// <summary>
        /// 获取课程详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetCourseDetail(string id)
        {
            var course = ServiceBase.FindBy<Course>(m => m.Id == id).FirstOrDefault();
            var sectionList = ServiceBase.FindBy<CourseSection>(m => m.CourseId == id).ToList();
            var teacher = ServiceBase.FindBy<TeacherInfo>(m => m.UserId == course.CreateUserId).FirstOrDefault();
            CourseViewModel courseData = new CourseViewModel
            {
                Course = course,
                Section = sectionList,
                Teacher = teacher
            };
            return Response(courseData);
        }

        /// <summary>
        /// 获取章节对应的讲义
        /// </summary>
        /// <param name="id">章节Id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetHandouts(string id)
        {
            var list = ServiceBase.FindBy<Handouts>(m => m.CourseSectionId == id).ToList();
            return Response();
        }

        /// <summary>
        /// 获取章节对应的PPT
        /// </summary>
        /// <param name="id">章节Id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetPPT(string id)
        {
            var list = ServiceBase.FindBy<PowerPoint>(m => m.CourseSectionId == id).ToList();
            return Response();
        }

        /// <summary>
        /// 获取章节对应的video
        /// </summary>
        /// <param name="id">章节Id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetVideo(string id)
        {
            var list = ServiceBase.FindBy<Video>(m => m.CourseSectionId == id).ToList();
            return Response();
        }
    }
}
