using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TAE.Data.Model;
using TAE.WebServer.Common;

namespace TAE.WebServer.Controllers.Stutent
{
    public class MainController : BaseApiController
    {
        /// <summary>
        /// 获取课程列表(未登录也可见)
        /// </summary>
        /// <param name="param">categoryId:课程分类Id，name:课程名（模糊查询）</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetCourses(string categoryId = "", string name = "")
        {
            List<Course> list = new List<Course>();
            if (categoryId != "")
            {
                list = ServiceBase.FindBy<Course>(m => m.CategoryId == categoryId && m.IsPublic == true).ToList();
            }
            else if (name != "")
            {
                list = ServiceBase.FindBy<Course>(m => m.Name == name && m.IsPublic == true).ToList();
            }
            else
            {
                list = ServiceBase.FindBy<Course>(m => m.IsPublic == true).ToList();
            }
            return Response(list);
        }

        /// <summary>
        /// 获取推荐教师
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage RecommendTeachers()
        {
            var list = ServiceBase.FindBy<TeacherInfo>(m => m.IsRecommend == true).ToList();
            return Response(list);
        }

        /// <summary>
        /// 获取教师课程
        /// </summary>
        /// <param name="id">userId</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetCoursesByTeacher(string id)
        {
            var teacher = ServiceBase.FindBy<TeacherInfo>(m => m.UserId == id).FirstOrDefault();
            var list = ServiceBase.FindBy<Course>(m => m.CreateUserId == id);
            return Response(new { teacher = teacher, coursers = list });
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
            return Response(list);
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
            return Response(list);
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
            return Response(list);
        }
    }
}
