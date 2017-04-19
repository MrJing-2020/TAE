using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TAE.Data.Model;
using TAE.WebServer.Common;
using TAE.WebServer.Common.Upload;

namespace TAE.WebServer.Controllers.Teacher
{
    public class CourseWareController : BaseApiController
    {
        /// <summary>
        /// 获取我的课件列表
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage AllCourseWares()
        {
            var list = ServiceBase.FindBy<CourseWare>(m => m.CreateUserId == LoginUser.UserInfo.Id);
            return Response(list);
        }

        /// <summary>
        /// 获取我的课件详情(包括附件信息)
        /// *****************查询次数太多，待优化*****************************
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage CourseWareDetail(string id)
        {
            var courseWare = ServiceBase.FindBy<CourseWare>(m => m.Id == id).FirstOrDefault();
            var handoutsList = ServiceBase.FindBy<Handouts>(m => m.CourseWareId == id).ToList();
            var testQuestionList = ServiceBase.FindBy<TestQuestion>(m => m.CourseWareId == id).ToList();
            var testPaperList = ServiceBase.FindBy<TestPaper>(m => m.CourseWareId == id).ToList();
            var videoList = ServiceBase.FindBy<Video>(m => m.CourseWareId == id).ToList();
            var powerPointList = ServiceBase.FindBy<PowerPoint>(m => m.CourseWareId == id).ToList();
            CourseWareViewModel data = new CourseWareViewModel
            {
                CourseWare = courseWare,
                Handouts = handoutsList,
                TestQuestion = testQuestionList,
                TestPaper = testPaperList,
                Video = videoList,
                PowerPoint = powerPointList
            };
            return Response();
        }

        #region 获取课件相关附件

        /// <summary>
        /// 根据课件id获取讲义(多个)
        /// </summary>
        /// <param name="id">课件(CourseWare)id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetHandouts(string id)
        {
            var handoutList = ServiceBase.FindBy<Handouts>(m => m.CourseWareId == id).ToList();
            return Response(handoutList);
        }

        [HttpGet]
        public HttpResponseMessage GetTestQuestion(string id)
        {
            var testQuestionList = ServiceBase.FindBy<TestQuestion>(m => m.CourseWareId == id).ToList();
            return Response();
        }

        [HttpGet]
        public HttpResponseMessage GetTestPaper(string id)
        {
            var testPaperList = ServiceBase.FindBy<TestPaper>(m => m.CourseWareId == id).ToList();
            return Response();
        }

        [HttpGet]
        public HttpResponseMessage GetVideo(string id)
        {
            var videoList = ServiceBase.FindBy<Video>(m => m.CourseWareId == id).ToList();
            return Response();
        }

        [HttpGet]
        public HttpResponseMessage GetPowerPoint(string id)
        {
            var powerPointList = ServiceBase.FindBy<PowerPoint>(m => m.CourseWareId == id).ToList();
            return Response();
        } 
        #endregion

        #region 课件编辑相关

        [HttpPost]
        public HttpResponseMessage SubCourseWarse(CourseWare model)
        {
            ServiceBase.SaveEntity<CourseWare>(model);
            return Response();
        }

        [HttpPost]
        public HttpResponseMessage SubHandouts(Handouts model)
        {
            ServiceBase.SaveEntity<Handouts>(model);
            return Response();
        }

        [HttpPost]
        public HttpResponseMessage SubTestQuestion(TestQuestion model)
        {
            ServiceBase.SaveEntity<TestQuestion>(model);
            return Response();
        }

        [HttpPost]
        public HttpResponseMessage SubTestPaper(TestPaper model)
        {
            ServiceBase.SaveEntity<TestPaper>(model);
            return Response();
        }

        /// <summary>
        /// 保存课件（视频）,包括文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> SubVideo()
        {
            // 是否请求包含multipart/form-data。
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var provider = new MultipartFormDataStreamProvider(UploadHelper.UploadPath);
            await Request.Content.ReadAsMultipartAsync(provider);
            Video video = new Video
            {
                Id = provider.FormData.GetValues("Id").FirstOrDefault(),
                Name = provider.FormData.GetValues("Name").FirstOrDefault(),
                Description = provider.FormData.GetValues("Description").FirstOrDefault(),
                CourseWareId = provider.FormData.GetValues("CourseWareId").FirstOrDefault(),
                //是否公开，0表示否
                IsPublic = provider.FormData.GetValues("IsPublic").FirstOrDefault() == "0" ? false : true,
                IsDel = false,
                CreateUserId = LoginUser.UserInfo.Id
            };
            video = ServiceBase.SaveEntity<Video>(video);

            //文件保存
            var fileList = UploadHelper.uploadFile(provider);
            foreach (var item in fileList)
            {
                item.FileType = Convert.ToInt32(FileTypeEnum.Vedio); ;
                //获取业务类型，此处的值为TypeId（Type表中，TypeGroup为TypeEnum.CourseAccessory，TypeName为"PPT"的列的Id）
                item.BusinessType = provider.FormData.GetValues("CourseAccessory").FirstOrDefault();
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
            // 是否请求包含multipart/form-data。
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var provider = new MultipartFormDataStreamProvider(UploadHelper.UploadPath);
            await Request.Content.ReadAsMultipartAsync(provider);
            PowerPoint ppt = new PowerPoint
            {
                Id = provider.FormData.GetValues("Id").FirstOrDefault(),
                Name = provider.FormData.GetValues("Name").FirstOrDefault(),
                CourseWareId = provider.FormData.GetValues("CourseWareId").FirstOrDefault(),
                //是否公开，0表示否
                IsPublic = provider.FormData.GetValues("IsPublic").FirstOrDefault() == "0" ? false : true,
                IsDel = false,
                CreateUserId = LoginUser.UserInfo.Id
            };
            ppt = ServiceBase.Insert<PowerPoint>(ppt);

            //文件保存
            var fileList = UploadHelper.uploadFile(provider);
            foreach (var item in fileList)
            {
                item.FileType = Convert.ToInt32(FileTypeEnum.PPT);
                //获取业务类型，此处的值为TypeId（Type表中，TypeGroup为TypeEnum.CourseAccessory，TypeName为"PPT"的列的Id）
                item.BusinessType = provider.FormData.GetValues("CourseAccessory").FirstOrDefault();
                item.LinkId = ppt.Id;
                item.UploadUserId = LoginUser.UserInfo.Id;
                item.CompanyId = LoginUser.UserInfo.CompanyId;
                item.DepartmentId = LoginUser.UserInfo.DepartmentId;
            }
            ServiceBase.Insert<FilesInfo>(fileList);
            return Response();
        } 

        #endregion
    }
}
