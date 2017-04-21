using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    /// <summary>
    /// 课程附件基类
    /// </summary>
    public class CourseAccBaseModel : BaseModel
    {
        public string CourseId { get; set; }
        public string CourseSectionId { get; set; }
        public bool IsPublic { get; set; }
    }
}
