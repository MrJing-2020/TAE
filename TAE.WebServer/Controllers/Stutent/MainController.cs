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
    /// <summary>
    /// 允许匿名访问
    /// </summary>
    [AllowAnonymous]
    public class MainController : BaseApiController
    {
        /// <summary>
        /// 获取课程列表(未登录也可见)
        /// </summary>
        /// <param name="param">categoryId:课程分类Id，name:课程名（模糊查询）typeId:课程类型（公共，选修，必修）</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetCourses(dynamic param)
        {
            PageList<Course> pageList = new PageList<Course>();
            RequestArg arg = new RequestArg()
            {
                PageNumber = Convert.ToInt32(param.pageNumber),
                PageSize = Convert.ToInt32(param.pageSize),
            };
            if (!string.IsNullOrEmpty(param.categoryId.ToString()))
            {
                string categoryId = param.categoryId.ToString();
                pageList = ServiceBase.FindAllByPage<Course>(m => m.CategoryId == categoryId && m.IsPublic == true,arg);
            }
            else if (!string.IsNullOrEmpty(param.name.ToString()))
            {
                string name = param.name.ToString();
                pageList = ServiceBase.FindAllByPage<Course>("select * from Course where Name like '%" + name + "%'", arg);
            }
            else if (!string.IsNullOrEmpty(param.typeId.ToString()))
            {
                string typeId = param.typeId.ToString();
                pageList = ServiceBase.FindAllByPage<Course>(m => m.TypeId == typeId && m.IsPublic == true,arg);
            }
            else
            {
                pageList = ServiceBase.FindAllByPage<Course>(m=>m.IsPublic==true, arg);
            };
            if (pageList.Total == 0)
            {
                return Response(HttpStatusCode.NoContent, new { msg = "没有任何信息" });
            }
            else
            {
                return Response(pageList);
            }
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
        public HttpResponseMessage GetHandouts(string id, bool isWholeCourse= false)
        {
            List<Handouts> list = new List<Handouts>();
            if (isWholeCourse == false)
            {
                list = ServiceBase.FindBy<Handouts>(m => m.CourseSectionId == id).ToList();
            }
            else
            {
                list = ServiceBase.FindBy<Handouts>(m => m.CourseId == id).ToList();
            }
            return Response(list);
        }

        /// <summary>
        /// 获取讲义列表
        /// </summary>
        /// <param name="param">categoryId:课程分类id，name：讲义名</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetHandoutsList(dynamic param)
        {
            PageList<Handouts> pageList = new PageList<Handouts>();
            RequestArg arg = new RequestArg()
            {
                PageNumber = Convert.ToInt32(param.pageNumber),
                PageSize = Convert.ToInt32(param.pageSize),
            };
            if (!string.IsNullOrEmpty(param.categoryId.ToString()))
            {
                string categoryId = param.categoryId.ToString();
                var coursesId = ServiceBase.FindBy<Course>(m => m.CategoryId == categoryId).Select(m => m.Id).ToArray();
                pageList = ServiceBase.FindAllByPage<Handouts>(m => coursesId.Any(n => n == m.CourseId), arg);
            }
            else if (!string.IsNullOrEmpty(param.name.ToString()))
            {
                string name = param.name.ToString();
                pageList = ServiceBase.FindAllByPage<Handouts>("select * from Handouts where Name like '%" + name + "%'", arg);
            }
            else
            {
                pageList = ServiceBase.FindAllByPage<Handouts>(m => m.IsPublic == true, arg);
            };
            if (pageList.Total == 0)
            {
                return Response(HttpStatusCode.NoContent, new { msg = "没有任何信息" });
            }
            else
            {
                return Response(pageList);
            }
        }

        /// <summary>
        /// 获取章节对应的PPT
        /// </summary>
        /// <param name="id">章节Id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetPPT(string id, bool isWholeCourse = false)
        {
            List<PowerPoint> list = new List<PowerPoint>();
            if (isWholeCourse == false)
            {
                list = ServiceBase.FindBy<PowerPoint>(m => m.CourseSectionId == id).ToList();
            }
            else
            {
                list = ServiceBase.FindBy<PowerPoint>(m => m.CourseId == id).ToList();
            }
            return Response(list);
        }

        /// <summary>
        /// 获取PPT列表
        /// </summary>
        /// <param name="param">categoryId:课程分类id，name：PPT名</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetPPTList(dynamic param)
        {
            PageList<PowerPoint> pageList = new PageList<PowerPoint>();
            RequestArg arg = new RequestArg()
            {
                PageNumber = Convert.ToInt32(param.pageNumber),
                PageSize = Convert.ToInt32(param.pageSize),
            };
            if (!string.IsNullOrEmpty(param.categoryId.ToString()))
            {
                string categoryId = param.categoryId.ToString();
                var coursesId = ServiceBase.FindBy<Course>(m => m.CategoryId == categoryId).Select(m => m.Id).ToArray();
                pageList = ServiceBase.FindAllByPage<PowerPoint>(m => coursesId.Any(n => n == m.CourseId), arg);
            }
            else if (!string.IsNullOrEmpty(param.name.ToString()))
            {
                string name = param.name.ToString();
                pageList = ServiceBase.FindAllByPage<PowerPoint>("select * from PowerPoint where Name like '%" + name + "%'", arg);
            }
            else
            {
                pageList = ServiceBase.FindAllByPage<PowerPoint>(m => m.IsPublic == true, arg);
            };
            if (pageList.Total == 0)
            {
                return Response(HttpStatusCode.NoContent, new { msg = "没有任何信息" });
            }
            else
            {
                return Response(pageList);
            }
        }

        /// <summary>
        /// 获取章节对应的video
        /// </summary>
        /// <param name="id">章节Id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetVideo(string id, bool isWholeCourse = false)
        {
            List<Video> list = new List<Video>();
            if (isWholeCourse == false)
            {
                list = ServiceBase.FindBy<Video>(m => m.CourseSectionId == id).ToList();
            }
            else
            {
                list = ServiceBase.FindBy<Video>(m => m.CourseId == id).ToList();
            }
            return Response(list);
        }

        /// <summary>
        /// 获取视频列表
        /// </summary>
        /// <param name="param">categoryId:课程分类id，name：视频名</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetVideoList(dynamic param)
        {
            PageList<Video> pageList = new PageList<Video>();
            RequestArg arg = new RequestArg()
            {
                PageNumber = Convert.ToInt32(param.pageNumber),
                PageSize = Convert.ToInt32(param.pageSize),
            };
            if (!string.IsNullOrEmpty(param.categoryId.ToString()))
            {
                string categoryId = param.categoryId.ToString();
                var coursesId = ServiceBase.FindBy<Course>(m => m.CategoryId == categoryId).Select(m => m.Id).ToArray();
                pageList = ServiceBase.FindAllByPage<Video>(m => coursesId.Any(n => n == m.CourseId), arg);
            }
            else if (!string.IsNullOrEmpty(param.name.ToString()))
            {
                string name = param.name.ToString();
                pageList = ServiceBase.FindAllByPage<Video>("select * from Video where Name like '%" + name + "%'", arg);
            }
            else
            {
                pageList = ServiceBase.FindAllByPage<Video>(m => m.IsPublic == true, arg);
            };
            if (pageList.Total == 0)
            {
                return Response(HttpStatusCode.NoContent, new { msg = "没有任何信息" });
            }
            else
            {
                return Response(pageList);
            }
        }

        /// <summary>
        /// 通过id获取文件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetFileInfo(string id)
        {
            var list = ServiceBase.FindBy<FilesInfo>(m => m.LinkId == id);
            return Response(list);
        }

        /// <summary>
        /// 获取课程分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage CourseCategory()
        {
            var list = ServiceBase.FindBy<CourseCategory>("select * from CourseCategory order by LastModifiedTime").ToList();
            
            return Response(list);
        }

        /// <summary>
        /// 获取课程的类型（公共，选修，必修）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage AllCourseType()
        {
            int courseType = Convert.ToInt32(TypeEnum.Course);
            string sqlGetAllCourseType = @"select * from Type where TypeGroup = " + courseType;
            var list = ServiceBase.FindBy<Data.Model.Type>(sqlGetAllCourseType).ToList();
            return Response(list);
        }
    }
}
