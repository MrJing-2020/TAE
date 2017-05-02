using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TAE.Data.Model;
using TAE.WebServer.Common;
using TAE.WebServer.Common.Upload;

namespace TAE.WebServer.Controllers.Stutent
{
    /// <summary>
    /// 个人中心(学员),需要先登录
    /// </summary>
    public class MyCenterController : StudentApiController
    {
        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetMyInfo()
        {
            return Response(LoginStudent);
        }

        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SubMyInfo(StudentInfo model)
        {
            ServiceBase.Update<StudentInfo>(model);
            return Response();
        }

        /// <summary>
        /// 头像上传
        /// </summary>
        /// <returns>头像地址</returns>
        [HttpPost]
        public async Task<HttpResponseMessage> SubMyPhoto()
        {
            //获取教师头像对应的TypeId
            string typeId = ServiceBase.FindBy<TAE.Data.Model.Type>(m => m.TypeGroup == (int)TypeEnum.UserPhoto && m.TypeName == "Student").Select(m => m.Id).FirstOrDefault();
            var fileInfo = ServiceBase.FindBy<FilesInfo>(m => m.BusinessType == typeId && m.LinkId == LoginStudent.Id).FirstOrDefault();
            ServiceBase.DelFile(fileInfo);
            // 是否请求包含multipart/form-data。
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var fileList = await UploadHelper.uploadFile(Request.Content);
            foreach (var item in fileList)
            {
                item.LinkId = LoginStudent.Id;
                item.FileType = Convert.ToInt32(FileTypeEnum.Img); ;
                item.BusinessType = typeId;
                item.UploadUserId = LoginUser.UserInfo.Id;
                item.CompanyId = LoginUser.UserInfo.CompanyId;
                item.DepartmentId = LoginUser.UserInfo.DepartmentId;
            }
            ServiceBase.Insert<FilesInfo>(fileList);
            var myInfo = ServiceBase.FindBy<StudentInfo>(m => m.UserId == LoginUser.UserInfo.Id).FirstOrDefault();
            myInfo.PhotoUrl = fileList.FirstOrDefault().RelativePath;
            ServiceBase.Update<StudentInfo>(myInfo);
            return Response(new { url = myInfo.PhotoUrl });
        }

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
