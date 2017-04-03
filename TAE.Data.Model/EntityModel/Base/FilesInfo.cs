using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class FilesInfo : ComBaseModel
    {
        public string OldFileName { get; set; }
        /// <summary>
        /// 扩展名
        /// </summary>
        public string ExtName { get; set; }
        /// <summary>
        /// 文件类型(文档，视频...)
        /// </summary>
        public int FileType { get; set; }
        /// <summary>
        /// 文件对应的业务类型
        /// </summary>
        public string BusinessType { get; set; }
        /// <summary>
        /// 文件对应的业务Id(如用户id)
        /// </summary>
        public string LinkId { get; set; }
        public string NewFileName { get; set; }
        public string AbsolutePath { get; set; }
        public string RelativePath { get; set; }
        public string UploadUserId { get; set; }
    }
}
