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
    using TAE.Data.Model;
    using TAE.Utility.Common;
    public class UploadHelper
    {
        private ServiceContext serviceContext;
        public UploadHelper()
        {
            serviceContext = ServiceContext.Current;
        }
        private static string UploadPath
        {   get 
            {
                return HttpContext.Current.Server.MapPath("~/App_Data"); 
            } 
        }
        /// <summary>
        /// 文件上传，返回FilesInfo列表，需要补充文件类型，业务类型，关联id后插入数据库
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task<List<FilesInfo>> uploadFile(HttpContent content)
        {
            StringBuilder sb = new StringBuilder();
            var provider = new MultipartFormDataStreamProvider(UploadPath);
            await content.ReadAsMultipartAsync(provider);
            List<FilesInfo> list = new List<FilesInfo>();
            foreach (var file in provider.FileData)
            {
                
                string oldFilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                //获取文件绝对地址
                FileInfo fileinfo = new FileInfo(file.LocalFileName);
                //文件扩展名
                string fileExt = oldFilename.Substring(oldFilename.LastIndexOf('.'));
                string newFileName = fileinfo.Name.Substring(fileinfo.Name.IndexOf('_') + 1) + fileExt;
                fileinfo.CopyTo(Path.Combine(UploadPath, newFileName), true);
                sb.Append("~/App_Data/" + newFileName);
                FilesInfo filesInfo = new FilesInfo()
                {
                    OldFileName = oldFilename,
                    ExtName = fileExt,
                    NewFileName = newFileName,
                    RelativePath = sb.ToString(),
                    AbsolutePath = HttpContext.Current.Server.MapPath(sb.ToString())
                };
                list.Add(filesInfo);
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
            return list ;
        }
    }
}