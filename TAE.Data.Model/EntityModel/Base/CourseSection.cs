using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    /// <summary>
    /// 课程章节
    /// </summary>
    public class CourseSection : BaseModel
    {
        public string Name { get; set; }
        public string CourseId { get; set; }
        public string MainContent { get; set; }

        //可能存在章节下的小章节
        public string PreId { get; set; }
    }
}
