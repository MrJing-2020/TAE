using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    /// <summary>
    /// 教学大纲（办班计划）
    /// </summary>
    public class TeachingPlan : ComBaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Goal { get; set; }
    }
}
