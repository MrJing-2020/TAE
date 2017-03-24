using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;

namespace TAE.Utility.Common
{
    public class UploadAvatar
    {
        #region 属性
        /// <summary>
        ///     图片高
        /// </summary>
        int imgmaxheight = 800;
        /// <summary>
        /// 图片宽
        /// </summary>
        int imgmaxwidth = 800;
        /// <summary>
        ///  生成缩略图高
        /// </summary>
        int thumbnailwidth = 100;
        /// <summary>
        ///生成缩略图的高
        /// </summary>
        int thumbnailheight = 100;
        /// <summary>
        /// 默认水印花类型
        /// </summary>
        int watermarktype = 1;
        /// <summary>
        /// 路径
        /// </summary>
        string path = "";
        #endregion

        //上传头像路径
        //private static readonly string UploadFolder = HttpContext.Current.Server.MapPath("/upload/avatars/");//Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"upload\avatars\");
        public Result UploadImage(HttpPostedFileBase _file)
        {
            bool isThumbnail = true;
            bool isWater = false;
            try
            {
                string fileExt = GetFileExt(_file.ContentType); //文件扩展名，不含“.”

                path = HttpContext.Current.Server.MapPath("/upload/avatars/");//上传头像路径
                int fileSize = _file.ContentLength; //获得文件大小，以字节为单位
                //string fileName = _file.FileName.Substring(_file.FileName.LastIndexOf(@"\") + 1) + "." + fileExt; //取得原文件名
                string newFileName = GetRamCode() + "." + fileExt; //随机生成新的文件名
                string newThumbnailFileName = "thumb_" + newFileName; //随机生成缩略图文件名
                string upLoadPath = "/upload/avatars/yt/" + newFileName;//上传原图目录相对路径
                string upLoadPathS = "/upload/avatars/slt/" + newFileName;//上传缩略图目录相对路径

                //是否存在存放缩略图和原图的文件夹 没有则创建
                string pathS = HttpContext.Current.Server.MapPath("/upload/avatars/slt/");
                string pathY = HttpContext.Current.Server.MapPath("/upload/avatars/yt/");
                if (!Directory.Exists(pathS))
                {
                    Directory.CreateDirectory(pathS);
                }
                if (!Directory.Exists(pathY))
                {
                    Directory.CreateDirectory(pathY);
                }

                string newFilePath = path + "yt\\" + newFileName; //上传后原图的路径
                string newThumbnailPath = path + "slt\\" + newFileName; //上传后的缩略图路径

                #region 检查文件扩展名是否合法
                if (CheckFileExt(fileExt))
                {
                    return new Result() { success = false, msg = "不允许上传" + fileExt + "类型的文件！" };
                }
                #endregion

                #region 检查文件大小是否合法
                if (!CheckFileSize(fileExt, fileSize))
                {
                    return new Result() { success = false, msg = "文件超过限制的大小！" };
                }
                #endregion

                #region 检查上传的物理路径是否存在，不存在则创建
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                #endregion

                #region 保存文件
                _file.SaveAs(newFilePath);
                #endregion

                #region 图片剪裁
                //如果是图片，检查图片是否超出最大尺寸，是则裁剪
                //if (IsImage(fileExt) && (imgmaxheight > 0 || imgmaxwidth > 0))
                //{
                //    /*
                //    生成缩略图
                //    源图路径（绝对路径）
                //    缩略图路径（绝对路径）
                //    缩略图宽度
                //    缩略图高度
                //    生成缩略图的方式*/
                //    Thumbnail.MakeThumbnailImage(newFilePath, newFilePath, imgmaxwidth, imgmaxheight);
                //}
                //如果是图片，检查是否需要生成缩略图，是则生成
                if (IsImage(fileExt) && isThumbnail && thumbnailwidth > 0 && thumbnailheight > 0)
                {
                    Thumbnail.MakeThumbnailImage(newFilePath, newThumbnailPath, thumbnailwidth, thumbnailheight, "Cut");
                }
                else
                {
                    newThumbnailPath = newFilePath; //不生成缩略图则返回原图
                }
                //如果是图片，检查是否需要打水印
                if (IsWaterMark(fileExt) && isWater)
                {
                    switch (watermarktype)
                    {
                        case 1:
                            WaterMark.AddImageSignText(newFilePath, newFilePath, "水印", 1, 50, "宋体", 14);
                            break;
                        case 2:
                            WaterMark.AddImageSignPic(newFilePath, newFilePath, "", 1, 33, 3);
                            break;
                    }
                }
                #endregion

                //处理完毕，返回JOSN格式的文件信息
                return new Result() { success = true, msg = "操作成功！", sourceUrl = upLoadPathS };
            }
            catch
            {
                return new Result() { success = false, msg = "上传过程中发生意外错误！" };

            }
        }

        public Result MobileUploadImage(Image _file)
        {
            bool isThumbnail = true;
            try
            {
                string fileExt = "jpg"; //文件扩展名，不含“.”

                path = HttpContext.Current.Server.MapPath("/upload/avatars/");//上传头像路径
                string newFileName = GetRamCode() + "." + fileExt; //随机生成新的文件名
                string newThumbnailFileName = "thumb_" + newFileName; //随机生成缩略图文件名
                string upLoadPath = "/upload/avatars/yt/" + newFileName;//上传原图目录相对路径
                string upLoadPathS = "/upload/avatars/slt/" + newFileName;//上传缩略图目录相对路径

                //是否存在存放缩略图和原图的文件夹 没有则创建
                string pathS = HttpContext.Current.Server.MapPath("/upload/avatars/slt/");
                string pathY = HttpContext.Current.Server.MapPath("/upload/avatars/yt/");
                if (!Directory.Exists(pathS))
                {
                    Directory.CreateDirectory(pathS);
                }
                if (!Directory.Exists(pathY))
                {
                    Directory.CreateDirectory(pathY);
                }

                string newFilePath = path + "yt\\" + newFileName; //上传后原图的路径
                string newThumbnailPath = path + "slt\\" + newFileName; //上传后的缩略图路径

                #region 检查上传的物理路径是否存在，不存在则创建
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                #endregion

                #region 保存文件
                _file.Save(newFilePath);
                #endregion

                #region 图片剪裁
                //如果是图片，检查是否需要生成缩略图，是则生成
                if (IsImage(fileExt) && isThumbnail && thumbnailwidth > 0 && thumbnailheight > 0)
                {
                    Thumbnail.MakeThumbnailImage(newFilePath, newThumbnailPath, thumbnailwidth, thumbnailheight, "Cut");
                }
                else
                {
                    newThumbnailPath = newFilePath; //不生成缩略图则返回原图
                }
                #endregion

                //处理完毕，返回JOSN格式的文件信息
                return new Result() { success = true, msg = "操作成功！", sourceUrl = upLoadPathS };
            }
            catch
            {
                return new Result() { success = false, msg = "上传过程中发生意外错误！" };
            }
        }

        /// <summary>
        /// 生成随机长度的随机码
        /// </summary>
        /// <param name="length">随机码长度</param>
        /// <returns></returns>
        private static string CreateRandomCode(int length)
        {
            string[] codes = new string[36] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            StringBuilder randomCode = new StringBuilder();
            Random rand = new Random();
            for (int i = 0; i < length; i++)
            {
                randomCode.Append(codes[rand.Next(codes.Length)]);
            }
            return randomCode.ToString();
        }

        #region 辅助方法
        /// <summary>
        /// 返回文件扩展名，不含“.”
        /// </summary>
        /// <param name="_filepath">文件全名称</param>
        /// <returns>string</returns>
        public static string GetFileExt(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return "jpg";
            }
            if (_filepath.LastIndexOf(".") > 0)
            {
                return _filepath.Substring(_filepath.LastIndexOf(".") + 1); //文件扩展名，不含“.”
            }
            switch (_filepath.ToLower())
            {
                case "image/jpeg":
                    return "jpg";
                case "image/pjpeg":
                    return "jpg";
                case "image/gif":
                    return "gif";
                case "image/bmp":
                    return "bmp";
                case "image/x-png":
                    return "png";
                default:
                    return "jpg";
            }
        }

        #region 生成日期随机码
        /// <summary>
        /// 生成日期随机码
        /// </summary>
        /// <returns></returns>
        public static string GetRamCode()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssffff");
        }
        #endregion
        #region 获得当前绝对路径
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (strPath.ToLower().StartsWith("http://"))
            {
                return strPath;
            }
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        #endregion
        #endregion

        #region 私有方法
        /// <summary>
        /// 返回上传目录相对路径
        /// </summary>
        /// <param name="fileName">上传文件名</param>
        private string GetUpLoadPath()
        {
            string path = "/upload/";
            path += DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("dd");
            return path + "/";
        }

        /// <summary>
        /// 是否需要打水印
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        private bool IsWaterMark(string _fileExt)
        {
            //判断是否开启水印
            //if (this.siteConfig.watermarktype > 0)
            //{
            //判断是否可以打水印的图片类型
            ArrayList al = new ArrayList();
            al.Add("bmp");
            al.Add("jpeg");
            al.Add("jpg");
            al.Add("png");
            if (al.Contains(_fileExt.ToLower()))
            {
                return true;
            }
            //}
            return false;
        }

        /// <summary>
        /// 是否为图片文件
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        private bool IsImage(string _fileExt)
        {
            ArrayList al = new ArrayList();
            al.Add("bmp");
            al.Add("jpeg");
            al.Add("jpg");
            al.Add("gif");
            al.Add("png");
            if (al.Contains(_fileExt.ToLower()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查是否为合法的上传文件
        /// </summary>
        private bool CheckFileExt(string _fileExt)
        {
            //检查危险文件
            string[] excExt = { "asp", "aspx", "ashx", "asa", "asmx", "asax", "php", "jsp", "htm", "html" };
            for (int i = 0; i < excExt.Length; i++)
            {
                if (excExt[i].ToLower() == _fileExt.ToLower())
                {
                    return false;
                }
            }
            //检查合法文件
            //string[] allowExt = (this.siteConfig.fileextension + "," + this.siteConfig.videoextension).Split(',');
            //for (int i = 0; i < allowExt.Length; i++)
            //{
            //    if (allowExt[i].ToLower() == _fileExt.ToLower())
            //    {
            //        return true;
            //    }
            //}
            return false;
        }

        /// <summary>
        /// 检查文件大小是否合法
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        /// <param name="_fileSize">文件大小(B)</param>
        private bool CheckFileSize(string _fileExt, int _fileSize)
        {
            //将视频扩展名转换成ArrayList
            //ArrayList lsVideoExt = new ArrayList(this.siteConfig.videoextension.ToLower().Split(','));
            ////判断是否为图片文件
            //if (IsImage(_fileExt))
            //{
            //    if (this.siteConfig.imgsize > 0 && _fileSize > this.siteConfig.imgsize * 1024)
            //    {
            //        return false;
            //    }
            //}
            //else if (lsVideoExt.Contains(_fileExt.ToLower()))
            //{
            //    if (this.siteConfig.videosize > 0 && _fileSize > this.siteConfig.videosize * 1024)
            //    {
            //        return false;
            //    }
            //}
            //else
            //{
            //    if (this.siteConfig.attachsize > 0 && _fileSize > this.siteConfig.attachsize * 1024)
            //    {
            //        return false;
            //    }
            //}
            return true;
        }
        #endregion

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="_file"></param>
        /// <returns></returns>
        public FileResult UploadFiles(HttpPostedFileBase postedFile)
        {
            try
            {
                //文件扩展名,不含“.”
                string fileExt = GetFileExt(postedFile.FileName);
                //获取文件大小，以字节为单位
                int fileSize = postedFile.ContentLength;
                //取得原文件名
                string fileName = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(@"\") + 1);
                //随机生成新的文件名
                string newFileName = GetRamCode() + "." + fileExt;
                //上传目录相对路径
                string upLoadPath = GetUpLoadPath();
                //上传目录的物理路径
                string fullUploadPath = GetMapPath(upLoadPath);
                //上传后的路径
                string newFilePath = upLoadPath + newFileName;
                if (!Directory.Exists(fullUploadPath))
                {
                    Directory.CreateDirectory(fullUploadPath);
                }
                //保存文件
                postedFile.SaveAs(fullUploadPath + newFileName);
                //处理完毕，返回JOSN格式的文件信息
                return new FileResult { status = "1", msg = "上传文件成功！", name = fileName, path = newFilePath, thumb = newFilePath, size = fileSize.ToString(), ext = fileExt };
            }
            catch
            {
                return new FileResult { status = "0", msg = "上传过程中发生意外错误！" };
            }
        }


        /// <summary>
        /// 上传图片 指定生成缩略图的尺寸
        /// </summary>
        /// <param name="_file"></param>
        /// <param name="isThumbnail"></param>
        /// <param name="isWater"></param>
        /// <param name="thumbnailwidth"></param>
        /// <param name="thumbnailheight"></param>
        /// <returns></returns>
        public Result UploadImage(HttpPostedFileBase _file, bool isThumbnail, bool isWater, int thumbnailwidth, int thumbnailheight, string thumbModel = "Cut")
        {
            try
            {
                string fileExt = GetFileExt(_file.ContentType); //文件扩展名，不含“.”
                //上传目录相对路径
                string upLoadPath = GetUpLoadPath();
                //上传目录的物理路径
                string fullUploadPath = GetMapPath(upLoadPath);
                path = fullUploadPath;
                int fileSize = _file.ContentLength; //获得文件大小，以字节为单位
                string newFileName = GetRamCode() + "." + fileExt; //随机生成新的文件名
                string newThumbnailFileName = "thumb_" + newFileName; //随机生成缩略图文件名
                string newFilePath = fullUploadPath + newFileName; //上传后原图的路径
                string newThumbnailPath = fullUploadPath + "slt\\" + newFileName; //上传后的缩略图路径

                #region 检查文件扩展名是否合法
                if (CheckFileExt(fileExt))
                {
                    return new Result() { success = false, msg = "不允许上传" + fileExt + "类型的文件！" };
                }
                #endregion

                #region 检查文件大小是否合法
                if (!CheckFileSize(fileExt, fileSize))
                {
                    return new Result() { success = false, msg = "文件超过限制的大小！" };
                }
                #endregion

                #region 检查上传的物理路径是否存在，不存在则创建
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (!Directory.Exists(path + "slt\\"))
                {
                    Directory.CreateDirectory(path + "slt\\");
                }

                #endregion

                #region 保存文件
                _file.SaveAs(newFilePath);
                #endregion

                #region 图片剪裁
                //如果是图片，检查图片是否超出最大尺寸，是则裁剪
                //if (IsImage(fileExt) && (imgmaxheight > 0 || imgmaxwidth > 0))
                //{
                //    /*
                //    生成缩略图
                //    源图路径（绝对路径）
                //    缩略图路径（绝对路径）
                //    缩略图宽度
                //    缩略图高度
                //    生成缩略图的方式*/
                //    Thumbnail.MakeThumbnailImage(newFilePath, newFilePath, imgmaxwidth, imgmaxheight);
                //}
                //如果是图片，检查是否需要生成缩略图，是则生成
                if (IsImage(fileExt) && isThumbnail && thumbnailwidth > 0 && thumbnailheight > 0)
                {
                    Thumbnail.MakeThumbnailImage(newFilePath, newThumbnailPath, thumbnailwidth, thumbnailheight, thumbModel);
                }
                else
                {
                    newThumbnailPath = newFilePath; //不生成缩略图则返回原图
                }
                //如果是图片，检查是否需要打水印
                if (IsWaterMark(fileExt) && isWater)
                {
                    switch (watermarktype)
                    {
                        case 1:
                            WaterMark.AddImageSignText(newFilePath, newFilePath, "水印", 1, 50, "宋体", 14);
                            break;
                        case 2:
                            WaterMark.AddImageSignPic(newFilePath, newFilePath, "", 1, 33, 3);
                            break;
                    }
                }
                #endregion

                //处理完毕，返回JOSN格式的文件信息
                return new Result() { success = true, msg = "操作成功！", fileName = _file.FileName, sourceUrl = upLoadPath + newFileName, avatarUrls = new ArrayList() { upLoadPath + "slt/" + newFileName } };
            }
            catch
            {
                return new Result() { success = false, msg = "上传过程中发生意外错误！" };

            }
        }

        private static Dictionary<String, ImageFormat> GetImageFormats()
        {
            var dic = new Dictionary<String, ImageFormat>();
            var properties = typeof(ImageFormat).GetProperties(BindingFlags.Static | BindingFlags.Public);
            foreach (var property in properties)
            {
                var format = property.GetValue(null, null) as ImageFormat;
                if (format == null) continue;
                dic.Add((property.Name).ToLower(), format);
            }
            return dic;
        }

        private static Dictionary<String, ImageFormat> _imageFormats;

        /// <summary>
        /// 获取 所有支持的图片格式字典
        /// </summary>
        public static Dictionary<String, ImageFormat> ImageFormats
        {
            get
            {
                return _imageFormats ?? (_imageFormats = GetImageFormats());
            }
        }

        /// 根据图像获取图像的扩展名
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static String GetExtension(Image image)
        {
            foreach (var pair in ImageFormats)
            {

                if (pair.Value.Guid == image.RawFormat.Guid)
                {
                    return pair.Key;
                }
            }
            throw new BadImageFormatException();
        }

        public Result UploadImage(Image _file, bool isThumbnail = false)
        {
            try
            {
                string fileExt = GetExtension(_file); //获取文件扩展名，不含“.”
                path = GetUpLoadPath();//上传目录相对路径
                var pathThumb = path + "slt/";//如果生成缩略图的话，缩略图的路径
                string newFileName = GetRamCode() + "." + fileExt; //随机生成新的文件名
                string newThumbnailFileName = "thumb_" + newFileName; //随机生成缩略图文件名
                string upLoadPath = path + newFileName;//上传原图目录相对路径
                string upLoadPathS = pathThumb + newThumbnailFileName;//上传缩略图目录相对路径

                //是否存在存放缩略图和原图的文件夹 没有则创建
                string pathS = GetMapPath(pathThumb);
                string pathY = GetMapPath(path);

                if (!Directory.Exists(pathY))
                {
                    Directory.CreateDirectory(pathY);
                }
                if (isThumbnail)
                {
                    if (!Directory.Exists(pathS))
                    {
                        Directory.CreateDirectory(pathS);
                    }
                }

                string newFilePath = pathY + newFileName; //上传后原图的路径
                string newThumbnailPath = pathS + newFileName; //上传后的缩略图路径

                #region 保存文件
                _file.Save(newFilePath);
                #endregion

                #region 图片剪裁
                //如果是图片，检查是否需要生成缩略图，是则生成
                if (IsImage(fileExt) && isThumbnail && thumbnailwidth > 0 && thumbnailheight > 0)
                {
                    Thumbnail.MakeThumbnailImage(newFilePath, newThumbnailPath, thumbnailwidth, thumbnailheight, "Cut");
                }
                else
                {
                    newThumbnailPath = newFilePath; //不生成缩略图则返回原图
                }
                #endregion

                //处理完毕，返回JOSN格式的文件信息
                return new Result() { success = true, msg = "操作成功！", sourceUrl = upLoadPath, fileName = newFileName };
            }
            catch
            {
                return new Result() { success = false, msg = "上传过程中发生意外错误！" };
            }
        }
    }

    /// <summary>
    /// 表示图片的上传结果
    /// </summary>
    public struct Result
    {
        /// <summary>
        /// 表示图片是否已上传成功
        /// </summary>
        public bool success;
        /// <summary>
        /// 自定义的附加消息
        /// </summary>
        public string msg;
        /// <summary>
        /// 原始文件名称
        /// </summary>
        public string fileName;
        /// <summary>
        /// 表示原始图片的保存地址
        /// </summary>
        public string sourceUrl;
        /// <summary>
        /// 表示所有头像图片的保存地址,该变量为一个数组
        /// </summary>
        public ArrayList avatarUrls;
    }

    public struct FileResult
    {
        public string status;

        public string msg;

        public string name;

        public string path;
        public string thumb { get; set; }
        public string size { get; set; }
        public string ext { get; set; }
    }
}
