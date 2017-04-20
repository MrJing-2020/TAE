using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    /// <summary>
    /// 课程
    /// </summary>
    public class Course : ComBaseModel
    {
        public string Name { get; set; }
        public string ClassId { get; set; }
    }
}
