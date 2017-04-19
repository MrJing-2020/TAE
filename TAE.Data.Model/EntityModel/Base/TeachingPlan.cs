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

        //创建教学大纲时选择是否自己创建教学计划（若为false则直接发放到指定对象，由该对象创建教学计划）
        public bool CreateCourseBySelf { get; set; }
    }
}
