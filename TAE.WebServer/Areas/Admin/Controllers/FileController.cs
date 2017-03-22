using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TAE.WebServer.Areas.Admin.Controllers
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using TAE.WebServer.Common;
    public class FileController : BaseApiController
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
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);
            // 阅读表格数据并返回一个异步任务.
            await Request.Content.ReadAsMultipartAsync(provider);

            // 如何上传文件到文件名.
            foreach (var file in provider.FileData)
            {
                string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                FileInfo fileinfo = new FileInfo(file.LocalFileName);
                //sb.Append(string.Format("Uploaded file: {0} ({1} bytes)\n", fileInfo.Name, fileInfo.Length));
                //最大文件大小
                //int maxSize = Convert.ToInt32(SettingConfig.MaxSize);
                string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                //定义允许上传的文件扩展名
                //String fileTypes = SettingConfig.FileTypes;
                //if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
                //{
                //    json.Msg = "图片类型不正确";
                //    json.Code = 303;
                //}
                //else
                //{
                //String ymd = DateTime.Now.ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                //String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", System.Globalization.DateTimeFormatInfo.InvariantInfo);

                fileinfo.CopyTo(Path.Combine(root, fileinfo.Name + fileExt), true);
                sb.Append("/UploadFiles/" + fileinfo.Name + fileExt);
                //}
                fileinfo.Delete();//删除原文件
            }
            return Response(new { path = sb.ToString() });
        }
    }
}
