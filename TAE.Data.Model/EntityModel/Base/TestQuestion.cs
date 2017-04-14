using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    /// <summary>
    /// 试题表
    /// </summary>
    public class TestQuestion : CourseAccBaseModel
    {
        public string MainContent { get; set; }

        /// <summary>
        /// 题目类型(主观，客观题)
        /// </summary>
        public string TypeId { get; set; }
        public string Answer { get; set; }

        /// <summary>
        /// 题目分值
        /// </summary>
        public int Score { get; set; }
    }
}
