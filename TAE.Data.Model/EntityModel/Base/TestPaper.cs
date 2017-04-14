using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class TestPaper : CourseAccBaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        //试卷类型(例：自测试卷，考试试卷)
        public string TypeId { get; set; }

        //考试Id（Type对应考试试卷的时候才有意义）
        public string ExaminationId { get; set; }

    }
}
