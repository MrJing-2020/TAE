using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TAE.WebServer.Controllers.Admin
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using TAE.Data.Model;
    using TAE.WebServer.Common;
    using TAE.WebServer.Common.Upload;
    public class FileManagerController : BaseApiController
    {
        [HttpPost]
        public async Task<HttpResponseMessage> UploadFile()
        {
            StringBuilder sb = new StringBuilder();
            // 是否请求包含multipart/form-data。
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var fileList = await UploadHelper.uploadFile(Request.Content);
            foreach (var item in fileList)
            {
                item.LinkId = LoginUser.UserInfo.Id;
                item.FileType = 1;
                item.BusinessType = "";
                item.UploadUserId = LoginUser.UserInfo.Id;
                item.CompanyId = LoginUser.UserInfo.CompanyId;
                item.DepartmentId = LoginUser.UserInfo.DepartmentId;
            }
            ServiceBase.Insert<FilesInfo>(fileList);
            return Response();
        }
    }
}
