using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TAE.Data.Model;
using TAE.WebServer.Common;
using TAE.WebServer.Common.Upload;

namespace TAE.WebServer.Controllers.Teacher
{
    /// <summary>
    /// 教师端（课程管理，课程编辑...）
    /// </summary>
    public class CourseController : TeacherApiController
    {
        /// <summary>
        /// 获取我创建的所有课程
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage AllCourses()
        {
            List<Course> list = ServiceBase.FindBy<Course>(m => m.CreateUserId == LoginUser.UserInfo.Id).ToList();
            return Response(list);
        }

        /// <summary>
        /// 编辑课程
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SubCourseData(Course model)
        {
            model.CompanyId = LoginUser.UserInfo.CompanyId;
            model.DepartmentId = LoginUser.UserInfo.DepartmentId;
            model.CreateUserId = LoginUser.UserInfo.Id;
            ServiceBase.SaveEntity<Course>(model);
            return Response();
        }

        /// <summary>
        /// 课程章节
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SubCourseSectionData(CourseSection model)
        {
            ServiceBase.SaveEntity<CourseSection>(model);
            return Response();
        }

        /// <summary>
        /// 保存课件（视频）,包括文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> SubVideo()
        {
            //获取ppt对应的TypeId
            string typeId = ServiceBase.FindBy<TAE.Data.Model.Type>(m => m.TypeGroup == (int)TypeEnum.CourseAccessory && m.TypeName == "video").Select(m => m.Id).FirstOrDefault();
            // 是否请求包含multipart/form-data。
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var provider = new MultipartFormDataStreamProvider(UploadHelper.UploadPath);
            await Request.Content.ReadAsMultipartAsync(provider);
            Video video = new Video
            {
                Name = provider.FormData.GetValues("Name").FirstOrDefault(),
                Description = provider.FormData.GetValues("Description").FirstOrDefault(),
                //是否公开，0表示否
                IsPublic = provider.FormData.GetValues("IsPublic").FirstOrDefault() == "0" ? false : true,
                IsDel = false,
                CourseId= provider.FormData.GetValues("CourseId").FirstOrDefault(),
                CourseSectionId = provider.FormData.GetValues("CourseSectionId").FirstOrDefault()
            };
            if (string.IsNullOrEmpty(provider.FormData.GetValues("Id").FirstOrDefault()))
            {
                video = ServiceBase.Insert<Video>(video);
            }
            else
            {
                video.Id = provider.FormData.GetValues("Id").FirstOrDefault(); 
                video = ServiceBase.Update<Video>(video);
            }
            //文件保存
            var fileList = UploadHelper.uploadFile(provider);
            foreach (var item in fileList)
            {
                item.FileType = Convert.ToInt32(FileTypeEnum.Vedio); ;
                //获取业务类型，此处的值为TypeId（Type表中，TypeGroup为TypeEnum.CourseAccessory，TypeName为"PPT"的列的Id）
                //item.BusinessType = provider.FormData.GetValues("CourseAccessory").FirstOrDefault();
                item.BusinessType = typeId;
                item.LinkId = video.Id;
                item.UploadUserId = LoginUser.UserInfo.Id;
                item.CompanyId = LoginUser.UserInfo.CompanyId;
                item.DepartmentId = LoginUser.UserInfo.DepartmentId;
            }
            ServiceBase.Insert<FilesInfo>(fileList);
            return Response();
        }

        /// <summary>
        /// 保存课件（ppt）,包括文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> SubPowerPoint()
        {
            //获取ppt对应的TypeId
            string typeId = ServiceBase.FindBy<TAE.Data.Model.Type>(m => m.TypeGroup == (int)TypeEnum.CourseAccessory && m.TypeName == "PPT").Select(m => m.Id).FirstOrDefault();
            // 是否请求包含multipart/form-data。
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var provider = new MultipartFormDataStreamProvider(UploadHelper.UploadPath);
            await Request.Content.ReadAsMultipartAsync(provider);
            PowerPoint ppt = new PowerPoint
            {
                Name = provider.FormData.GetValues("Name").FirstOrDefault(),
                //是否公开，0表示否
                IsPublic = provider.FormData.GetValues("IsPublic").FirstOrDefault() == "0" ? false : true,
                IsDel = false,
                CourseId = provider.FormData.GetValues("CourseId").FirstOrDefault(),
                CourseSectionId = provider.FormData.GetValues("CourseSectionId").FirstOrDefault()
            };
            if (string.IsNullOrEmpty(provider.FormData.GetValues("Id").FirstOrDefault()))
            {
                ppt = ServiceBase.Insert<PowerPoint>(ppt);
            }
            else
            {
                ppt.Id = provider.FormData.GetValues("Id").FirstOrDefault();
                ppt = ServiceBase.Update<PowerPoint>(ppt);
            }
            //文件保存
            var fileList = UploadHelper.uploadFile(provider);
            foreach (var item in fileList)
            {
                item.FileType = Convert.ToInt32(FileTypeEnum.PPT);
                //获取业务类型，此处的值为TypeId（Type表中，TypeGroup为TypeEnum.CourseAccessory，TypeName为"PPT"的列的Id）
                //item.BusinessType = provider.FormData.GetValues("TypeId").FirstOrDefault();
                item.BusinessType = typeId;
                item.LinkId = ppt.Id;
                item.UploadUserId = LoginUser.UserInfo.Id;
                item.CompanyId = LoginUser.UserInfo.CompanyId;
                item.DepartmentId = LoginUser.UserInfo.DepartmentId;
            }
            ServiceBase.Insert<FilesInfo>(fileList);
            return Response();
        }

        /// <summary>
        /// 保存课件（讲义）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SubHandouts(Handouts model)
        {
            ServiceBase.SaveEntity<Handouts>(model);
            return Response();
        }
    }
}
