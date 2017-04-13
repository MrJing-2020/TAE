using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    /// <summary>
    /// 用户-课程关联表
    /// </summary>
    public class UserRCourse : BaseModel
    {
        public string UserId { get;set; }
        public string CourseId { get; set; }
        public string CourseContentId { get; set; }
        public string CourseContentType { get; set; }
    }
}
