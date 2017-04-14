using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    /// <summary>
    /// 讲义
    /// </summary>
    public class Handouts : CourseAccBaseModel
    {
        public string Name { get; set; }
        public string MainContent { get; set; }
    }
}
