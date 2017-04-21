using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    /// <summary>
    /// 与Type表中的TypeName，TypeGroup对应
    /// </summary>
    public enum TypeEnum
    {
        //1表示类型是工作流
        WorkFlow = 1,
        //2表示课程类型(必修，选修)
        Course=2,
        //3表示课程附件(ppt,vedio等)
        CourseAccessory = 3,
        //教师类型
        Teacher = 4
    }
}
