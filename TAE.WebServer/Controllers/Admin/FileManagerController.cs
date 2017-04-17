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

    /// <summary>
    /// 文件管理
    /// </summary>
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
                //
                item.FileType = 1;
                item.BusinessType = "";
                item.UploadUserId = LoginUser.UserInfo.Id;
                item.CompanyId = LoginUser.UserInfo.CompanyId;
                item.DepartmentId = LoginUser.UserInfo.DepartmentId;
            }
            ServiceBase.Insert<FilesInfo>(fileList);
            return Response();
        }

        [HttpPost]
        public HttpResponseMessage UploadFileByBase64(dynamic model)
        {
            string url = "";
            try
            {
                var strFile = model.file;
                var suffix = strFile.Substring(strFile.IndexOf("/") + 1, strFile.IndexOf(";") - strFile.IndexOf("/") - 1);
                string strBase64 = strFile.Substring(strFile.LastIndexOf(",") + 1);
                string fileName = new Guid() + suffix;
                //文件保存的路径
                url = HttpContext.Current.Server.MapPath(UploadHelper.UploadPath) + fileName;
                //byte[] fileBuffer = Convert.FromBase64String(strBase64);
                //using (FileStream fs = new FileStream(url, FileMode.CreateNew))
                //{
                //    fs.Write(fileBuffer, 0, fileBuffer.Length);
                //}
                MemoryStream ms = new MemoryStream(Convert.FromBase64String(strBase64));
                using (FileStream fs = new FileStream(url, FileMode.CreateNew))
                {
                    ms.WriteTo(fs);
                    ms.Close();
                }
            }
            catch (Exception)
            {
                return Response(HttpStatusCode.InternalServerError, new { msg = "服务器错误" });
            }
            return Response(url);
        }

        public HttpResponseMessage DelFile(string id)
        {
            ServiceBase.Remove<FilesInfo>(id);
            ServiceBase.DelFile(id);
            return Response();
        }

        [HttpGet]
        public HttpResponseMessage GetFileList()
        {
            var fileList = ServiceBase.FindBy<FilesInfo>().ToList();
            return ResponseList<FilesInfo>(fileList);
        }
    }
}
