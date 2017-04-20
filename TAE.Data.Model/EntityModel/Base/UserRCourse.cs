using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    /// <summary>
    /// 用户-课程关联表(适用对象)
    /// </summary>
    public class UserRCourse : BaseModel
    {
        public string UserId { get;set; }
        public string CompanyId { get; set; }
        public string DepartmentId { get; set; }
        public string PositionId { get; set; }

        //课程Id
        public string CourseId { get; set; }

        //课件Id
        public string CourseWareId { get; set; }

        //课件附件Id(例：ppt，video)
        public string CourseAccId { get; set; }
    }
}
