using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAE.WebServer.Common.Upload
{
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    public class UploadHelper
    {
        private static string UploadPath
        {   get 
            {
                return HttpContext.Current.Server.MapPath("~/App_Data"); 
            } 
        }
        public static async Task<string> uploadFile(HttpContent content)
        {
            StringBuilder sb = new StringBuilder();
            var provider = new MultipartFormDataStreamProvider(UploadPath);
            await content.ReadAsMultipartAsync(provider);
            foreach (var file in provider.FileData)
            {
                string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                //获取文件绝对地址
                FileInfo fileinfo = new FileInfo(file.LocalFileName);
                //文件扩展名
                string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                fileinfo.CopyTo(Path.Combine(UploadPath, fileinfo.Name + fileExt), true);
                sb.Append("~/App_Data/" + fileinfo.Name + fileExt);
                fileinfo.Delete();//删除原文件

                //sb.Append(string.Format("Uploaded file: {0} ({1} bytes)\n", fileInfo.Name, fileInfo.Length));
                //最大文件大小
                //int maxSize = Convert.ToInt32(SettingConfig.MaxSize);
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
                //}
            }
            return sb.ToString() ;
        }
    }
}