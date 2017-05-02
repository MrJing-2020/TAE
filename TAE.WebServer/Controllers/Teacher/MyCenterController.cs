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
    /// 教师个人中心
    /// </summary>
    public class MyCenterController : TeacherApiController
    {
        /// <summary>
        /// 获取我的个人信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetMyInfo()
        {
            return Response(LoginTeacher);
        }

        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SubMyInfo(TeacherInfo model)
        {
            ServiceBase.Update<TeacherInfo>(model);
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
            string typeId = ServiceBase.FindBy<TAE.Data.Model.Type>(m => m.TypeGroup == (int)TypeEnum.UserPhoto && m.TypeName == "Theacher").Select(m => m.Id).FirstOrDefault();
            var fileInfo = ServiceBase.FindBy<FilesInfo>(m => m.BusinessType == typeId && m.LinkId == LoginTeacher.Id).FirstOrDefault();
            ServiceBase.DelFile(fileInfo);
            // 是否请求包含multipart/form-data。
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var fileList = await UploadHelper.uploadFile(Request.Content);
            foreach (var item in fileList)
            {
                item.LinkId = LoginTeacher.Id;
                item.FileType = Convert.ToInt32(FileTypeEnum.Img); ;
                item.BusinessType = typeId;
                item.UploadUserId = LoginUser.UserInfo.Id;
                item.CompanyId = LoginUser.UserInfo.CompanyId;
                item.DepartmentId = LoginUser.UserInfo.DepartmentId;
            }
            ServiceBase.Insert<FilesInfo>(fileList);
            var myInfo = ServiceBase.FindBy<TeacherInfo>(m => m.UserId == LoginUser.UserInfo.Id).FirstOrDefault();
            myInfo.PhotoUrl = fileList.FirstOrDefault().RelativePath;
            ServiceBase.Update<TeacherInfo>(myInfo);
            return Response(new { url = myInfo.PhotoUrl });
        }
    }
}
